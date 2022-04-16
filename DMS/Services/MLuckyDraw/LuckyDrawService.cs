using TrueSight.Common;

using DMS.Common;
using DMS.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using DMS.Repositories;
using DMS.Entities;
using DMS.Enums;
using DMS.Services.MImage;
using DMS.Handlers;
using DMS.Rpc.lucky_draw;
using DMS.Handlers.Configuration;
using Hangfire;

namespace DMS.Services.MLuckyDraw
{
    public interface ILuckyDrawService :  IServiceScoped
    {
        Task<int> Count(LuckyDrawFilter LuckyDrawFilter);
        Task<List<LuckyDraw>> List(LuckyDrawFilter LuckyDrawFilter);
        Task<List<Store>> ListStore(StoreFilter StoreFilter);
        Task<int> CountStore(StoreFilter StoreFilter);
        Task<LuckyDraw> Get(long Id);
        Task<LuckyDraw> Create(LuckyDraw LuckyDraw);
        Task<LuckyDraw> Update(LuckyDraw LuckyDraw);
        Task<LuckyDraw> Delete(LuckyDraw LuckyDraw);
        Task<LuckyDraw> DrawByEmployee(LuckyDraw LuckyDraw);
        Task<List<LuckyDraw>> BulkDelete(List<LuckyDraw> LuckyDraws);
        Task<List<LuckyDraw>> Import(List<LuckyDraw> LuckyDraws);
        Task<LuckyDrawFilter> ToFilter(LuckyDrawFilter LuckyDrawFilter);
        Task<List<LuckyDraw>> Export(LuckyDrawFilter LuckyDrawFilter);
        Task<Image> SaveImage(Image Image);
    }

    public class LuckyDrawService : BaseService, ILuckyDrawService
    {
        private IUOW UOW;
        private IImageService ImageService;
        private ILuckyDrawTemplate LuckyDrawTemplate;
        private IRabbitManager RabbitManager;
        private ILogging Logging;
        private ICurrentContext CurrentContext;        
        private ILuckyDrawValidator LuckyDrawValidator;

        public LuckyDrawService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            ILuckyDrawTemplate LuckyDrawTemplate,
            IImageService ImageService,
            IRabbitManager RabbitManager,
            ILuckyDrawValidator LuckyDrawValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.ImageService = ImageService;
            this.LuckyDrawTemplate = LuckyDrawTemplate;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.Logging = Logging;
           
            this.LuckyDrawValidator = LuckyDrawValidator;
        }
        public async Task<int> Count(LuckyDrawFilter LuckyDrawFilter)
        {
            try
            {
                int result = await UOW.LuckyDrawRepository.Count(LuckyDrawFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return 0;
        }
        public async Task<List<LuckyDraw>> List(LuckyDrawFilter LuckyDrawFilter)
        {
            try
            {
                List<LuckyDraw> LuckyDraws = await UOW.LuckyDrawRepository.List(LuckyDrawFilter);
                return LuckyDraws;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return null;
        }
        public async Task<LuckyDraw> Get(long Id)
        {
            try
            {
                LuckyDraw LuckyDraw = await UOW.LuckyDrawRepository.Get(Id);

                foreach (var LuckyDrawStructure in LuckyDraw.LuckyDrawStructures)
                {
                    LuckyDrawStructure.UsedQuantity = LuckyDraw.LuckyDrawWinners.Count(x => x.LuckyDrawStructureId == LuckyDrawStructure.Id);
                }
                if (LuckyDraw == null)
                    return null;
                return LuckyDraw;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return null;

        }        
        public async Task<LuckyDraw> Create(LuckyDraw LuckyDraw)
        {
            if (!await LuckyDrawValidator.Create(LuckyDraw))
                return LuckyDraw;

            try
            {
                await UOW.LuckyDrawRepository.Create(LuckyDraw);
                LuckyDraw = await UOW.LuckyDrawRepository.Get(LuckyDraw.Id);
                await CreateLuckyDrawNumber(LuckyDraw);
                #region Bắn thông báo
                //Bắn thông báo cho người tạo ngay khi tạo thành công
                var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                GlobalUserNotification UserNotification = LuckyDrawTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, LuckyDraw, CurrentUser, NotificationType.TOCREATOR);
                RabbitManager.PublishSingle(UserNotification, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                if (LuckyDraw.StatusId == StatusEnum.ACTIVE.Id)
                {
                    List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                    //bắn thông báo cho các app user liên quan khi chương trình có hiệu lực
                    var AppUsers = await ListAppUserRecipient(LuckyDraw);
                    foreach (var Recipient in AppUsers)
                    {
                        GlobalUserNotification GlobalUserNotification = LuckyDrawTemplate.CreateAppUserNotification(CurrentUser.RowId, Recipient.RowId, LuckyDraw, CurrentUser, NotificationType.CREATE);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                    //bắn thông báo cho các store user liên quan khi chương trình có hiệu lực
                    var StoreUsers = await ListStoreUserRecipient(LuckyDraw);
                    foreach (var Recipient in StoreUsers)
                    {
                        GlobalUserNotification GlobalUserNotification = LuckyDrawTemplate.CreateStoreUserNotification(CurrentUser.RowId, Recipient.RowId, LuckyDraw, CurrentUser, NotificationType.CREATE);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                    if (LuckyDraw.StartAt.Date == StaticParams.DateTimeNow.Date)
                    {
                        RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                    }
                    else
                    {
                        var Start = LuckyDraw.StartAt.AddHours(7);
                        BackgroundJob.Schedule(() => RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code), new DateTime(Start.Year, Start.Month, Start.Day, 0, 0, 0));
                    }
                }

                #endregion
                Logging.CreateAuditLog(LuckyDraw, new { }, nameof(LuckyDrawService));
                return LuckyDraw;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return null;
        }
        public async Task<LuckyDraw> Update(LuckyDraw LuckyDraw)
        {
            if (!await LuckyDrawValidator.Update(LuckyDraw))
                return LuckyDraw;
            try
            {
                var oldData = await UOW.LuckyDrawRepository.Get(LuckyDraw.Id);
                if (oldData.Used == true)
                {
                    oldData.StatusId = LuckyDraw.StatusId;
                    oldData.EndAt = LuckyDraw.EndAt;
                    oldData.Name = LuckyDraw.Name;
                    oldData.ImageId = LuckyDraw.ImageId;
                    oldData.AvatarImageId = LuckyDraw.AvatarImageId;
                    oldData.Description = LuckyDraw.Description;
                    await UOW.LuckyDrawRepository.Update(oldData);
                    foreach (var Structure in oldData.LuckyDrawStructures)
                    {
                        Structure.Quantity = LuckyDraw.LuckyDrawStructures.Where(x => x.Id == Structure.Id).FirstOrDefault().Quantity;
                    }
                    await UOW.LuckyDrawStructureRepository.BulkMerge(oldData.LuckyDrawStructures);
                }
                else
                {
                    await UOW.LuckyDrawRepository.Update(LuckyDraw);
                }

                LuckyDraw = await UOW.LuckyDrawRepository.Get(LuckyDraw.Id);
                await CreateLuckyDrawNumber(LuckyDraw);
                #region Thông báo
                var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                GlobalUserNotification UserNotification = LuckyDrawTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, LuckyDraw, CurrentUser, NotificationType.TOUPDATER);
                if (oldData.StatusId != LuckyDraw.StatusId && LuckyDraw.StatusId == StatusEnum.ACTIVE.Id)
                {
                    List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                    //bắn thông báo cho các app user liên quan khi chương trình có hiệu lực
                    var AppUsers = await ListAppUserRecipient(LuckyDraw);
                    foreach (var Recipient in AppUsers)
                    {
                        GlobalUserNotification GlobalUserNotification = LuckyDrawTemplate.CreateAppUserNotification(CurrentUser.RowId, Recipient.RowId, LuckyDraw, CurrentUser, NotificationType.CREATE);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                    //bắn thông báo cho các store user liên quan khi chương trình có hiệu lực
                    var StoreUsers = await ListStoreUserRecipient(LuckyDraw);
                    foreach (var Recipient in StoreUsers)
                    {
                        GlobalUserNotification GlobalUserNotification = LuckyDrawTemplate.CreateStoreUserNotification(CurrentUser.RowId, Recipient.RowId, LuckyDraw, CurrentUser, NotificationType.CREATE);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                    var Start = LuckyDraw.StartAt.AddHours(7);
                    BackgroundJob.Schedule(() => RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code), new DateTime(Start.Year, Start.Month, Start.Day, 0, 0, 0));
                }
                #endregion
                Logging.CreateAuditLog(LuckyDraw, oldData, nameof(LuckyDrawService));
                return LuckyDraw;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return null;
        }
        public async Task<LuckyDraw> Delete(LuckyDraw LuckyDraw)
        {
            if (!await LuckyDrawValidator.Delete(LuckyDraw))
                return LuckyDraw;

            try
            {
                await UOW.LuckyDrawRepository.Delete(LuckyDraw);
                Logging.CreateAuditLog(new { }, LuckyDraw, nameof(LuckyDrawService));
                return LuckyDraw;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return null;
        }
        public async Task<List<LuckyDraw>> BulkDelete(List<LuckyDraw> LuckyDraws)
        {
            if (!await LuckyDrawValidator.BulkDelete(LuckyDraws))
                return LuckyDraws;

            try
            {
                await UOW.LuckyDrawRepository.BulkDelete(LuckyDraws);
                Logging.CreateAuditLog(new { }, LuckyDraws, nameof(LuckyDrawService));
                return LuckyDraws;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return null;

        }        
        public async Task<List<LuckyDraw>> Import(List<LuckyDraw> LuckyDraws)
        {
            if (!await LuckyDrawValidator.Import(LuckyDraws))
                return LuckyDraws;
            try
            {
                await UOW.LuckyDrawRepository.BulkMerge(LuckyDraws);

                Logging.CreateAuditLog(LuckyDraws, new { }, nameof(LuckyDrawService));
                return LuckyDraws;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return null;
        }
        public async Task<List<LuckyDraw>> Export(LuckyDrawFilter LuckyDrawFilter)
        {
            try
            {
                LuckyDrawFilter.Selects = LuckyDrawSelect.Id;
                List<LuckyDraw> LuckyDraws = await UOW.LuckyDrawRepository.List(LuckyDrawFilter);
                List<long> Ids = LuckyDraws.Select(x => x.Id).ToList();
                LuckyDraws = new List<LuckyDraw>();
                for (int i = 0; i < Ids.Count; i += 1000)
                {
                    List<long> SubIds = Ids.Skip(i).Take(1000).ToList();
                    List<LuckyDraw> SubLuckyDraws = await UOW.LuckyDrawRepository.List(SubIds);
                    LuckyDraws.AddRange(SubLuckyDraws);
                }
                return LuckyDraws;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return null;
        }
        public async Task<LuckyDrawFilter> ToFilter(LuckyDrawFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<LuckyDrawFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                LuckyDrawFilter subFilter = new LuckyDrawFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Code))
                        subFilter.Code = FilterBuilder.Merge(subFilter.Code, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Name))
                        subFilter.Name = FilterBuilder.Merge(subFilter.Name, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.LuckyDrawTypeId))
                        subFilter.LuckyDrawTypeId = FilterBuilder.Merge(subFilter.LuckyDrawTypeId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.OrganizationId))
                        subFilter.OrganizationId = FilterBuilder.Merge(subFilter.OrganizationId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.RevenuePerTurn))
                        subFilter.RevenuePerTurn = FilterBuilder.Merge(subFilter.RevenuePerTurn, FilterPermissionDefinition.DecimalFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StartAt))
                        subFilter.StartAt = FilterBuilder.Merge(subFilter.StartAt, FilterPermissionDefinition.DateFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.EndAt))
                        subFilter.EndAt = FilterBuilder.Merge(subFilter.EndAt, FilterPermissionDefinition.DateFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.ImageId))
                        subFilter.ImageId = FilterBuilder.Merge(subFilter.ImageId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StatusId))
                        subFilter.StatusId = FilterBuilder.Merge(subFilter.StatusId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(CurrentContext.UserId) && FilterPermissionDefinition.IdFilter != null)
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.IS.Id)
                        {
                        }
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.ISNT.Id)
                        {
                        }
                    }
                }
            }
            return filter;
        }
        public async Task<Image> SaveImage(Image Image)
        {
            FileInfo fileInfo = new FileInfo(Image.Name);
            string path = $"/lucky-draw/images/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}{fileInfo.Extension}";
            Image = await ImageService.Create(Image, path);
            return Image;
        }
        public async Task<LuckyDraw> DrawByEmployee(LuckyDraw LuckyDraw)
        {
            if (!await LuckyDrawValidator.DrawByEmployee(LuckyDraw))
                return LuckyDraw;
            try
            {
                var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                var StoreId = LuckyDraw.StoreId;
                LuckyDraw = await UOW.LuckyDrawRepository.Get(LuckyDraw.Id);

                var LuckyDrawNumberId = await GetRandomNumber(LuckyDraw.LuckyDrawStructures);
                LuckyDrawNumber LuckyDrawNumber = await UOW.LuckyDrawNumberRepository.Get(LuckyDrawNumberId);
                var LuckyDrawRegistration = (await UOW.LuckyDrawRegistrationRepository.List(new LuckyDrawRegistrationFilter
                {
                    Take = 1,
                    Skip = 0,
                    OrderBy = LuckyDrawRegistrationOrder.Id,
                    OrderType = OrderType.ASC,
                    Selects = LuckyDrawRegistrationSelect.ALL,
                    LuckyDrawId = new IdFilter { Equal = LuckyDraw.Id },
                    AppUserId = new IdFilter { Equal = CurrentUser.Id },
                    StoreId = new IdFilter { Equal = StoreId },
                    IsDrawnByStore = false,
                    RemainingTurnCounter = new LongFilter { Greater = 0 }
                })).FirstOrDefault();
                LuckyDrawWinner LuckyDrawWinner = new LuckyDrawWinner
                {
                    LuckyDrawId = LuckyDraw.Id,
                    LuckyDrawStructureId = LuckyDrawNumber.LuckyDrawStructureId,
                    LuckyDrawNumberId = LuckyDrawNumberId,
                    RowId = Guid.NewGuid(),
                    LuckyDrawRegistrationId = LuckyDrawRegistration.Id,
                    Time = StaticParams.DateTimeNow
                };
                await UOW.LuckyDrawWinnerRepository.Create(LuckyDrawWinner);
                LuckyDrawRegistration.RemainingTurnCounter -= 1;
                await UOW.LuckyDrawRegistrationRepository.Update(LuckyDrawRegistration);
                await UOW.LuckyDrawNumberRepository.Used(new List<long> { LuckyDrawNumberId });
                LuckyDraw = await UOW.LuckyDrawRepository.Get(LuckyDraw.Id);
                LuckyDraw.LuckyDrawNumberId = LuckyDrawNumberId;
                LuckyDraw.LuckyDrawNumber = new LuckyDrawNumber
                {
                    LuckyDrawStructure = new LuckyDrawStructure
                    {
                        LuckyDrawId = LuckyDraw.Id,
                        Name = LuckyDrawNumber.LuckyDrawStructure.Name,
                        Value = LuckyDrawNumber.LuckyDrawStructure.Value,
                        Quantity = 1
                    },
                    LuckyDrawStructureId = LuckyDrawNumber.LuckyDrawStructureId,
                    Id = LuckyDrawNumberId,
                };
                Logging.CreateAuditLog(LuckyDraw, new { }, nameof(LuckyDrawService));
                return LuckyDraw;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return null;
        }
        public async Task<List<Store>> ListStore(StoreFilter StoreFilter)
        {
            try
            {
                var CurrentUserId = CurrentContext.UserId;
                StoreFilter.AppUserId = new IdFilter { Equal = CurrentUserId };
                if (StoreFilter.LuckyDrawId != null && StoreFilter.LuckyDrawId.Equal.HasValue)
                {
                    var LuckyDrawId = StoreFilter.LuckyDrawId.Equal.Value;
                    LuckyDraw LuckyDraw = await UOW.LuckyDrawRepository.Get(LuckyDrawId);
                    if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.STOREGROUPING.Id)
                    {
                        List<long> StoreGroupingIds = LuckyDraw.LuckyDrawStoreGroupingMappings.Select(x => x.StoreGroupingId).ToList();
                        StoreFilter.StoreGroupingId = new IdFilter { In = StoreGroupingIds };
                    }
                    else if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.STORETYPE.Id)
                    {
                        List<long> StoreTypeIds = LuckyDraw.LuckyDrawStoreTypeMappings.Select(x => x.StoreTypeId).ToList();
                        StoreFilter.StoreTypeId = new IdFilter { In = StoreTypeIds };
                    }
                    else if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.STORE.Id)
                    {
                        List<long> StoreIds = LuckyDraw.LuckyDrawStoreMappings.Select(x => x.StoreId).ToList();
                        StoreFilter.Id = new IdFilter { In = StoreIds };
                    }
                }
                List<Store> Stores = await UOW.StoreRepository.List(StoreFilter);
                return Stores;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return null;
        }
        public async Task<int> CountStore(StoreFilter StoreFilter)
        {
            try
            {
                var CurrentUserId = CurrentContext.UserId;
                StoreFilter.AppUserId = new IdFilter { Equal = CurrentUserId };
                if (StoreFilter.LuckyDrawId != null && StoreFilter.LuckyDrawId.Equal.HasValue)
                {
                    var LuckyDrawId = StoreFilter.LuckyDrawId.Equal.Value;
                    LuckyDraw LuckyDraw = await UOW.LuckyDrawRepository.Get(LuckyDrawId);
                    if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.STOREGROUPING.Id)
                    {
                        List<long> StoreGroupingIds = LuckyDraw.LuckyDrawStoreGroupingMappings.Select(x => x.StoreGroupingId).ToList();
                        StoreFilter.StoreGroupingId = new IdFilter { In = StoreGroupingIds };
                    }
                    else if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.STORETYPE.Id)
                    {
                        List<long> StoreTypeIds = LuckyDraw.LuckyDrawStoreTypeMappings.Select(x => x.StoreTypeId).ToList();
                        StoreFilter.StoreTypeId = new IdFilter { In = StoreTypeIds };
                    }
                    else if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.STORE.Id)
                    {
                        List<long> StoreIds = LuckyDraw.LuckyDrawStoreMappings.Select(x => x.StoreId).ToList();
                        StoreFilter.Id = new IdFilter { In = StoreIds };
                    }
                }
                var count = await UOW.StoreRepository.Count(StoreFilter);
                return count;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return 0;
        }
        private async Task CreateLuckyDrawNumber(LuckyDraw LuckyDraw)
        {
            var LuckyDrawStructureIds = LuckyDraw.LuckyDrawStructures.Select(x => x.Id).ToList();
            List<LuckyDrawNumber> LuckyDrawNumbers = await UOW.LuckyDrawNumberRepository.List(new LuckyDrawNumberFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = LuckyDrawNumberSelect.ALL,
                OrderBy = LuckyDrawNumberOrder.Id,
                OrderType = OrderType.ASC,
                LuckyDrawStructureId = new IdFilter { In = LuckyDrawStructureIds }
            });
            foreach(LuckyDrawStructure LuckyDrawStructure in LuckyDraw.LuckyDrawStructures)
            {
                var LuckyDrawNumberCheckList = new List<LuckyDrawNumber>();
                var LuckyDrawNumbersInStructure = LuckyDrawNumbers.Where(x => x.LuckyDrawStructureId == LuckyDrawStructure.Id).ToList();
                if (LuckyDrawNumbersInStructure.Count > LuckyDrawStructure.Quantity) //nếu số lượng giải thưởng giảm
                {
                    var NotUsedLuckyDrawNumbers = LuckyDrawNumbersInStructure.Where(x => x.Used == false).ToList();
                    int count = LuckyDrawNumbersInStructure.Count - (int)LuckyDrawStructure.Quantity;
                    int start = NotUsedLuckyDrawNumbers.Count - count;
                    LuckyDrawNumberCheckList = NotUsedLuckyDrawNumbers.GetRange(start, count);
                    await UOW.LuckyDrawNumberRepository.BulkDelete(LuckyDrawNumberCheckList);
                }
                else if (LuckyDrawNumbersInStructure.Count < LuckyDrawStructure.Quantity) //nếu số lượng giải thưởng tăng (gồm cả tạo mới)
                {
                    List<long> LuckyDrawNumberIds = new List<long>();
                    LuckyDrawNumberIds.AddRange(GenerateLuckyDrawNumberId(1, LuckyDrawStructure.Quantity, LuckyDrawStructure.Id));
                    if (LuckyDrawNumbersInStructure.Count > 0) //nếu update số lượng giải
                    {
                        var LuckyDrawNumberIdsInStructure = LuckyDrawNumbersInStructure.Select(x => x.Id).ToList();
                        int count = (int)LuckyDrawStructure.Quantity - LuckyDrawNumbersInStructure.Count;
                        LuckyDrawNumberIds = LuckyDrawNumberIds.Except(LuckyDrawNumberIdsInStructure).ToList().GetRange(0, count);
                    }
                    LuckyDrawNumberCheckList = LuckyDrawNumberIds.Select(x => new LuckyDrawNumber
                    {
                        Id = x,
                        LuckyDrawStructureId = LuckyDrawStructure.Id,
                        Used = false
                    }).ToList();
                    await UOW.LuckyDrawNumberRepository.BulkMerge(LuckyDrawNumberCheckList);
                }
            }
        }
        private List<long> GenerateLuckyDrawNumberId(long start, long end, long prefix)
        {
            List<long> list = new List<long>();
            var limit = prefix * 1_000_000;
            for (long i = start; i <= end; i++)
            {
                list.Add(limit + i);
            }
            return list;
        }
        private async Task<long> GetRandomNumber(List<LuckyDrawStructure> LuckyDrawStructures)
        {
            List<long> LuckyDrawStructureIds = LuckyDrawStructures.Select(x => x.Id).ToList();
            List<LuckyDrawNumber> LuckyDrawNumbers = await UOW.LuckyDrawNumberRepository.List(new LuckyDrawNumberFilter
            {
                Take = int.MaxValue,
                Selects = LuckyDrawNumberSelect.Id,
                Skip = 0,
                LuckyDrawStructureId = new IdFilter { In = LuckyDrawStructureIds },
                Used = false,
            });
            List<long> LuckyDrawNumberIds = LuckyDrawNumbers.Select(x => x.Id).ToList();
            var random = new Random();
            var result = random.Next(LuckyDrawNumberIds.Count);
            return LuckyDrawNumberIds.ElementAt(result);
        }
        private async Task<List<StoreUser>> ListStoreUserRecipient(LuckyDraw LuckyDraw)
        {
            var StoreUsers = await UOW.StoreUserRepository.List(new StoreUserFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = StoreUserSelect.Store | StoreUserSelect.RowId | StoreUserSelect.Id
            });
            if (LuckyDraw.LuckyDrawStoreGroupingMappings != null && LuckyDraw.LuckyDrawStoreGroupingMappings.Any())
            {
                var StoreGroupingIds = LuckyDraw.LuckyDrawStoreGroupingMappings.Select(x => x.StoreGroupingId).ToList();
                var Stores = await UOW.StoreRepository.List(new StoreFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = StoreSelect.Id,
                    StoreGroupingId = new IdFilter { In = StoreGroupingIds }
                });
                var StoreIds = Stores.Select(x => x.Id).ToList();
                StoreUsers = StoreUsers.Where(x => StoreIds.Contains(x.StoreId)).ToList();
            }
            else if (LuckyDraw.LuckyDrawStoreTypeMappings != null && LuckyDraw.LuckyDrawStoreTypeMappings.Any())
            {
                var StoreTypeIds = LuckyDraw.LuckyDrawStoreTypeMappings.Select(x => x.StoreTypeId).ToList();
                StoreUsers = StoreUsers.Where(x => StoreTypeIds.Contains(x.Store.StoreTypeId)).ToList();
            }
            else if (LuckyDraw.LuckyDrawStoreMappings != null && LuckyDraw.LuckyDrawStoreMappings.Any())
            {
                var StoreIds = LuckyDraw.LuckyDrawStoreMappings.Select(x => x.StoreId).ToList();
                StoreUsers = StoreUsers.Where(x => StoreIds.Contains(x.StoreId)).ToList();
            }
            else if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.ALLSTORE.Id)
            {
                var Stores = await UOW.StoreRepository.List(new StoreFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = StoreSelect.Id,
                    OrganizationId = new IdFilter { Equal = LuckyDraw.OrganizationId },
                });
                var StoreIds = Stores.Select(x => x.Id).ToList();
                StoreUsers = StoreUsers.Where(x => StoreIds.Contains(x.StoreId)).ToList();
            }
            return StoreUsers;
        }
        private async Task<List<AppUser>> ListAppUserRecipient(LuckyDraw LuckyDraw)
        {
            return await UOW.AppUserRepository.List(new AppUserFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = AppUserSelect.RowId,
                OrganizationId = new IdFilter { Equal = LuckyDraw.OrganizationId }
            });
        }
    }
}
