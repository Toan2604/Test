using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers.Configuration;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Services.MAppUser;
using DMS.Services.MFile;
using DMS.Services.MImage;
using DMS.Services.MNotification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MSurvey
{
    public interface ISurveyService : IServiceScoped
    {
        Task<int> Count(SurveyFilter SurveyFilter);
        Task<List<Survey>> List(SurveyFilter SurveyFilter);
        Task<Survey> Get(long Id);
        Task<Survey> Create(Survey Survey);
        Task<Survey> Update(Survey Survey);
        Task<Survey> Delete(Survey Survey);
        Task<Survey> GetForm(long Id);
        Task<Survey> SaveForm(Survey Survey);
        Task<DMS.Entities.File> SaveFile(DMS.Entities.File File);
        Task<DMS.Entities.Image> SaveImage(DMS.Entities.Image Image);
        SurveyFilter ToFilter(SurveyFilter SurveyFilter);
    }

    public class SurveyService : BaseService, ISurveyService
    {
        private ICurrentContext CurrentContext;
        private IFileService FileService;
        private ILogging Logging;
        private IImageService ImageService;
        private INotificationService NotificationService;
        private IRabbitManager RabbitManager;
        private ISurveyTemplate SurveyTemplate;
        private ISurveyValidator SurveyValidator;
        private IAppUserService AppUserService;
        private IUOW UOW;

        public SurveyService(
            IUOW UOW,
            ILogging Logging,
            INotificationService NotificationService,
            ISurveyTemplate SurveyTemplate,
            IImageService ImageService,
            IFileService FileService,
            ICurrentContext CurrentContext,
            IRabbitManager RabbitManager,
            IAppUserService AppUserService,
            ISurveyValidator SurveyValidator
        )
        {
            this.CurrentContext = CurrentContext;
            this.FileService = FileService;
            this.Logging = Logging;
            this.NotificationService = NotificationService;
            this.ImageService = ImageService;
            this.RabbitManager = RabbitManager;
            this.SurveyTemplate = SurveyTemplate;
            this.SurveyValidator = SurveyValidator;
            this.AppUserService = AppUserService;
            this.UOW = UOW;
        }
        public async Task<int> Count(SurveyFilter SurveyFilter)
        {
            try
            {
                int result = await UOW.SurveyRepository.Count(SurveyFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SurveyService));
            }
            return 0;
        }

        public async Task<List<Survey>> List(SurveyFilter SurveyFilter)
        {
            try
            {
                //AppUser AppUser = await AppUserService.Get(CurrentContext.UserId);
                //SurveyFilter.OrganizationId = new IdFilter { Equal = AppUser.OrganizationId };
                List<Survey> Surveys = await UOW.SurveyRepository.List(SurveyFilter);
                return Surveys;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SurveyService));
            }
            return null;
        }
        public async Task<Survey> Get(long Id)
        {
            Survey Survey = await UOW.SurveyRepository.Get(Id);
            if (Survey != null)
            {
                SurveyResultFilter SurveyResultFilter = new SurveyResultFilter
                {
                    SurveyId = new IdFilter { Equal = Survey.Id }
                };

                Survey.ResultCounter = await UOW.SurveyResultRepository.Count(SurveyResultFilter);
            }
            return Survey;
        }

        public async Task<Survey> Create(Survey Survey)
        {
            if (!await SurveyValidator.Create(Survey))
                return Survey;

            try
            {
                Survey.CreatorId = CurrentContext.UserId;

                await UOW.SurveyRepository.Create(Survey);


                var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                var RecipientRowIds = (await UOW.AppUserRepository.List(new AppUserFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = AppUserSelect.RowId,
                    OrganizationId = new IdFilter { }
                })).Select(x => x.RowId).ToList();
                foreach (var RowId in RecipientRowIds)
                {
                    GlobalUserNotification GlobalUserNotification = new GlobalUserNotification();
                    if (RowId == CurrentUser.RowId)
                        GlobalUserNotification = SurveyTemplate.CreateAppUserNotification(CurrentUser.RowId, RowId, Survey, CurrentUser, NotificationType.TOCREATOR);
                    else
                        GlobalUserNotification = SurveyTemplate.CreateAppUserNotification(CurrentUser.RowId, RowId, Survey, CurrentUser, NotificationType.CREATE);
                    GlobalUserNotifications.Add(GlobalUserNotification);
                }
                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                Logging.CreateAuditLog(Survey, new { }, nameof(SurveyService));
                return await UOW.SurveyRepository.Get(Survey.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SurveyService));
            }
            return null;
        }

        public async Task<Survey> Update(Survey Survey)
        {
            if (!await SurveyValidator.Update(Survey))
                return Survey;
            try
            {
                var oldData = await UOW.SurveyRepository.Get(Survey.Id);


                await UOW.SurveyRepository.Update(Survey);


                var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                var RecipientRowIds = (await UOW.AppUserRepository.List(new AppUserFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = AppUserSelect.RowId,
                    OrganizationId = new IdFilter { }
                })).Select(x => x.RowId).ToList();
                foreach (var RowId in RecipientRowIds)
                {
                    GlobalUserNotification GlobalUserNotification = new GlobalUserNotification();
                    if (RowId == CurrentUser.RowId)
                        GlobalUserNotification = SurveyTemplate.CreateAppUserNotification(CurrentUser.RowId, RowId, Survey, CurrentUser, NotificationType.TOUPDATER);
                    else
                        GlobalUserNotification = SurveyTemplate.CreateAppUserNotification(CurrentUser.RowId, RowId, Survey, CurrentUser, NotificationType.UPDATE);
                    GlobalUserNotifications.Add(GlobalUserNotification);
                }
                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);

                var newData = await UOW.SurveyRepository.Get(Survey.Id);
                Logging.CreateAuditLog(newData, oldData, nameof(SurveyService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SurveyService));
            }
            return null;
        }

        public async Task<Survey> Delete(Survey Survey)
        {
            if (!await SurveyValidator.Delete(Survey))
                return Survey;

            try
            {

                await UOW.SurveyRepository.Delete(Survey);


                var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                var RecipientRowIds = (await UOW.AppUserRepository.List(new AppUserFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = AppUserSelect.RowId,
                    OrganizationId = new IdFilter { }
                })).Select(x => x.RowId).ToList();
                foreach (var RowId in RecipientRowIds)
                {
                    GlobalUserNotification GlobalUserNotification = new GlobalUserNotification();
                    if (RowId == CurrentUser.RowId)
                        GlobalUserNotification = SurveyTemplate.CreateAppUserNotification(CurrentUser.RowId, RowId, Survey, CurrentUser, NotificationType.TODELETER);
                    else
                        GlobalUserNotification = SurveyTemplate.CreateAppUserNotification(CurrentUser.RowId, RowId, Survey, CurrentUser, NotificationType.DELETE);
                    GlobalUserNotifications.Add(GlobalUserNotification);
                }

                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);

                Logging.CreateAuditLog(new { }, Survey, nameof(SurveyService));
                return Survey;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SurveyService));
            }
            return null;
        }

        public SurveyFilter ToFilter(SurveyFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<SurveyFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                SurveyFilter subFilter = new SurveyFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterPermissionDefinition.IdFilter;
                    if (FilterPermissionDefinition.Name == nameof(subFilter.OrganizationId))
                        subFilter.OrganizationId = FilterPermissionDefinition.IdFilter;
                }
            }
            return filter;
        }

        public async Task<Survey> GetForm(long Id)
        {
            try
            {
                Survey Survey = await UOW.SurveyRepository.Get(Id);
                if (Survey.SurveyQuestions != null)
                {
                    foreach (SurveyQuestion SurveyQuestion in Survey.SurveyQuestions)
                    {
                        if (SurveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.QUESTION_MULTIPLE_CHOICE.Id ||
                            SurveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.QUESTION_SINGLE_CHOICE.Id)
                        {
                            SurveyQuestion.ListResult = new Dictionary<long, bool>();
                            if (SurveyQuestion.SurveyOptions != null)
                            {
                                foreach (SurveyOption SurveyOption in SurveyQuestion.SurveyOptions)
                                {
                                    SurveyQuestion.ListResult.Add(SurveyOption.Id, false);
                                }
                            }
                        }
                        if (SurveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.TABLE_MULTIPLE_CHOICE.Id ||
                            SurveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.TABLE_SINGLE_CHOICE.Id)
                        {
                            SurveyQuestion.TableResult = new Dictionary<long, Dictionary<long, bool>>();
                            if (SurveyQuestion.SurveyOptions != null)
                            {
                                List<SurveyOption> Columns = SurveyQuestion.SurveyOptions.Where(so => so.SurveyOptionTypeId == SurveyOptionTypeEnum.COLUMN.Id).ToList();
                                List<SurveyOption> Rows = SurveyQuestion.SurveyOptions.Where(so => so.SurveyOptionTypeId == SurveyOptionTypeEnum.ROW.Id).ToList();
                                foreach (SurveyOption Row in Rows)
                                {
                                    Dictionary<long, bool> RowResult = new Dictionary<long, bool>();
                                    SurveyQuestion.TableResult.Add(Row.Id, RowResult);
                                    foreach (SurveyOption Column in Columns)
                                    {
                                        RowResult.Add(Column.Id, false);
                                    }
                                }
                            }
                        }
                        if (SurveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.QUESTION_TEXT.Id)
                        {
                            SurveyQuestion.TextResult = "";
                        }
                    }
                }
                return Survey;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SurveyService));
            }
            return null;
        }

        public async Task<Survey> SaveForm(Survey Survey)
        {
            if (!await SurveyValidator.SaveForm(Survey))
                return Survey;

            try
            {
                AppUser AppUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                SurveyResult SurveyResult = new SurveyResult();
                SurveyResult.SurveyId = Survey.Id;
                SurveyResult.AppUserId = CurrentContext.UserId;
                SurveyResult.OrganizationId = AppUser.OrganizationId;
                SurveyResult.StoreId = Survey.StoreId;
                SurveyResult.StoreScoutingId = Survey.StoreScoutingId;
                SurveyResult.SurveyRespondentTypeId = Survey.SurveyRespondentTypeId;
                SurveyResult.Time = StaticParams.DateTimeNow;
                SurveyResult.RespondentAddress = Survey.RespondentAddress;
                SurveyResult.RespondentEmail = Survey.RespondentEmail;
                SurveyResult.RespondentName = Survey.RespondentName;
                SurveyResult.RespondentPhone = Survey.RespondentPhone;

                SurveyResult.SurveyResultSingles = new List<SurveyResultSingle>();
                SurveyResult.SurveyResultCells = new List<SurveyResultCell>();
                SurveyResult.SurveyResultTexts = new List<SurveyResultText>();
                if (Survey.SurveyQuestions != null)
                {
                    foreach (SurveyQuestion SurveyQuestion in Survey.SurveyQuestions)
                    {
                        if (SurveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.QUESTION_MULTIPLE_CHOICE.Id ||
                            SurveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.QUESTION_SINGLE_CHOICE.Id)
                        {
                            foreach (SurveyOption SurveyOption in SurveyQuestion.SurveyOptions)
                            {
                                if (SurveyQuestion.ListResult.ContainsKey(SurveyOption.Id) && SurveyQuestion.ListResult[SurveyOption.Id])
                                {
                                    SurveyResultSingle SurveyResultSingle = new SurveyResultSingle
                                    {
                                        SurveyOptionId = SurveyOption.Id,
                                        SurveyQuestionId = SurveyQuestion.Id,
                                    };
                                    SurveyResult.SurveyResultSingles.Add(SurveyResultSingle);
                                }
                            }
                        }
                        if (SurveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.TABLE_MULTIPLE_CHOICE.Id ||
                            SurveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.TABLE_SINGLE_CHOICE.Id)
                        {
                            if (SurveyQuestion.SurveyOptions != null)
                            {
                                List<SurveyOption> Columns = SurveyQuestion.SurveyOptions.Where(so => so.SurveyOptionTypeId == SurveyOptionTypeEnum.COLUMN.Id).ToList();
                                List<SurveyOption> Rows = SurveyQuestion.SurveyOptions.Where(so => so.SurveyOptionTypeId == SurveyOptionTypeEnum.ROW.Id).ToList();
                                foreach (SurveyOption Row in Rows)
                                {
                                    if (SurveyQuestion.TableResult.ContainsKey(Row.Id))
                                    {
                                        Dictionary<long, bool> ColumnResult = SurveyQuestion.TableResult[Row.Id];
                                        foreach (SurveyOption Column in Columns)
                                        {
                                            if (ColumnResult.ContainsKey(Column.Id) && ColumnResult[Column.Id])
                                            {
                                                SurveyResultCell SurveyResultCell = new SurveyResultCell
                                                {
                                                    SurveyQuestionId = SurveyQuestion.Id,
                                                    ColumnOptionId = Column.Id,
                                                    RowOptionId = Row.Id,
                                                };
                                                SurveyResult.SurveyResultCells.Add(SurveyResultCell);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (SurveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.QUESTION_TEXT.Id)
                        {
                            SurveyResultText SurveyResultText = new SurveyResultText
                            {
                                SurveyQuestionId = SurveyQuestion.Id,
                                Content = SurveyQuestion.TextResult,
                            };
                            SurveyResult.SurveyResultTexts.Add(SurveyResultText);
                        }
                    }
                }

                await UOW.SurveyResultRepository.Create(SurveyResult);
                Sync(new List<SurveyResult> { SurveyResult });

                return await GetForm(Survey.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SurveyService));
            }
            return null;
        }

        public async Task<DMS.Entities.File> SaveFile(DMS.Entities.File File)
        {
            FileInfo fileInfo = new FileInfo(File.Name);
            string path = $"/survey-question/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}{fileInfo.Extension}";

            File = await FileService.Create(File, path);
            return File;
        }

        public async Task<Image> SaveImage(Image Image)
        {
            FileInfo fileInfo = new FileInfo(Image.Name);
            string path = $"/survey-question/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}{fileInfo.Extension}";
            string thumbnailPath = $"/survey-question/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}{fileInfo.Extension}";
            Image = await ImageService.Create(Image, path, thumbnailPath, 128, 128);
            return Image;
        }

        private void Sync(List<SurveyResult> SurveyResults)
        {
            List<Store> Stores = SurveyResults.Where(x => x.StoreId != null).Select(x => new Store { Id = (long)x.StoreId }).Distinct().ToList();
            RabbitManager.PublishList(Stores, RoutingKeyEnum.StoreUsed.Code);
        }
    }
}
