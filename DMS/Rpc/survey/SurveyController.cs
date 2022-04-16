using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Services.MAppUser;
using DMS.Services.MOrganization;
using DMS.Services.MStatus;
using DMS.Services.MSurvey;
using DMS.Services.MSurveyResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
using File = DMS.Entities.File;

namespace DMS.Rpc.survey
{
    public class SurveyController : RpcController
    {
        private ISurveyQuestionTypeService SurveyQuestionTypeService;
        private ISurveyOptionTypeService SurveyOptionTypeService;
        private ISurveyService SurveyService;
        private ISurveyResultService SurveyResultService;
        private IAppUserService AppUserService;
        private IStatusService StatusService;
        private IOrganizationService OrganizationService;
        private ICurrentContext CurrentContext;
        public SurveyController(
            ISurveyQuestionTypeService SurveyQuestionTypeService,
            ISurveyOptionTypeService SurveyOptionTypeService,
            ISurveyService SurveyService,
            ISurveyResultService SurveyResultService,
            IAppUserService AppUserService,
            IStatusService StatusService,
            IOrganizationService OrganizationService,
            ICurrentContext CurrentContext
        )
        {
            this.SurveyOptionTypeService = SurveyOptionTypeService;
            this.SurveyQuestionTypeService = SurveyQuestionTypeService;
            this.SurveyService = SurveyService;
            this.SurveyResultService = SurveyResultService;
            this.AppUserService = AppUserService;
            this.StatusService = StatusService;
            this.OrganizationService = OrganizationService;
            this.CurrentContext = CurrentContext;
        }

        [Route(SurveyRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] Survey_SurveyFilterDTO Survey_SurveyFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            SurveyFilter SurveyFilter = ConvertFilterDTOToFilterEntity(Survey_SurveyFilterDTO);
            SurveyFilter = SurveyService.ToFilter(SurveyFilter);
            int count = await SurveyService.Count(SurveyFilter);
            return count;
        }

        [Route(SurveyRoute.List), HttpPost]
        public async Task<ActionResult<List<Survey_SurveyDTO>>> List([FromBody] Survey_SurveyFilterDTO Survey_SurveyFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            SurveyFilter SurveyFilter = ConvertFilterDTOToFilterEntity(Survey_SurveyFilterDTO);
            SurveyFilter = SurveyService.ToFilter(SurveyFilter);
            List<Survey> Surveys = await SurveyService.List(SurveyFilter);
            List<Survey_SurveyDTO> Survey_SurveyDTOs = Surveys
                .Select(c => new Survey_SurveyDTO(c)).ToList();
            return Survey_SurveyDTOs;
        }

        [Route(SurveyRoute.Get), HttpPost]
        public async Task<ActionResult<Survey_SurveyDTO>> Get([FromBody] Survey_SurveyDTO Survey_SurveyDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Survey_SurveyDTO.Id))
                return Forbid();

            Survey Survey = await SurveyService.Get(Survey_SurveyDTO.Id);
            return new Survey_SurveyDTO(Survey);
        }

        [Route(SurveyRoute.Create), HttpPost]
        public async Task<ActionResult<Survey_SurveyDTO>> Create([FromBody] Survey_SurveyDTO Survey_SurveyDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Survey_SurveyDTO.Id))
                return Forbid();

            Survey Survey = ConvertDTOToEntity(Survey_SurveyDTO);

            if (Survey.StartAt == default(DateTime))
                Survey.StartAt = StaticParams.DateTimeNow;
            else
                Survey.StartAt = StartDay(Survey.StartAt);
            if (Survey.EndAt != null || Survey.EndAt.HasValue)
                Survey.EndAt = EndDay(Survey.EndAt.Value);


            Survey = await SurveyService.Create(Survey);
            Survey_SurveyDTO = new Survey_SurveyDTO(Survey);
            if (Survey.IsValidated)
                return Survey_SurveyDTO;
            else
                return BadRequest(Survey_SurveyDTO);
        }

        [Route(SurveyRoute.Update), HttpPost]
        public async Task<ActionResult<Survey_SurveyDTO>> Update([FromBody] Survey_SurveyDTO Survey_SurveyDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Survey_SurveyDTO.Id))
                return Forbid();

            Survey Survey = ConvertDTOToEntity(Survey_SurveyDTO);

            if (Survey.StartAt == default(DateTime))
                Survey.StartAt = StaticParams.DateTimeNow;
            else
                Survey.StartAt = StartDay(Survey.StartAt);
            if (Survey.EndAt != null || Survey.EndAt.HasValue)
                Survey.EndAt = EndDay(Survey.EndAt.Value);

            Survey = await SurveyService.Update(Survey);
            Survey_SurveyDTO = new Survey_SurveyDTO(Survey);
            if (Survey.IsValidated)
                return Survey_SurveyDTO;
            else
                return BadRequest(Survey_SurveyDTO);
        }

        [Route(SurveyRoute.GetSurveyForm), HttpPost]
        public async Task<ActionResult<Survey_SurveyDTO>> GetSurveyForm([FromBody] Survey_SurveyDTO Survey_SurveyDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            //if (!await HasPermission(Survey_SurveyDTO.Id))
            //    return Forbid();

            Survey Survey = await SurveyService.GetForm(Survey_SurveyDTO.Id);
            Survey_SurveyDTO = new Survey_SurveyDTO(Survey);
            if (Survey.IsValidated)
                return Survey_SurveyDTO;
            else
                return BadRequest(Survey_SurveyDTO);
        }

        [Route(SurveyRoute.SaveSurveyForm), HttpPost]
        public async Task<ActionResult<Survey_SurveyDTO>> SaveSurveyForm([FromBody] Survey_SurveyDTO Survey_SurveyDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Survey_SurveyDTO.Id))
                return Forbid();

            Survey Survey = ConvertDTOToEntity(Survey_SurveyDTO);
            Survey = await SurveyService.SaveForm(Survey);
            Survey_SurveyDTO = new Survey_SurveyDTO(Survey);
            if (Survey.IsValidated)
                return Survey_SurveyDTO;
            else
                return BadRequest(Survey_SurveyDTO);
        }

        [Route(SurveyRoute.Delete), HttpPost]
        public async Task<ActionResult<Survey_SurveyDTO>> Delete([FromBody] Survey_SurveyDTO Survey_SurveyDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Survey_SurveyDTO.Id))
                return Forbid();

            Survey Survey = ConvertDTOToEntity(Survey_SurveyDTO);
            Survey = await SurveyService.Delete(Survey);
            Survey_SurveyDTO = new Survey_SurveyDTO(Survey);
            if (Survey.IsValidated)
                return Survey_SurveyDTO;
            else
                return BadRequest(Survey_SurveyDTO);
        }


        [Route(SurveyRoute.AnswerStatistics), HttpPost]
        public async Task<ActionResult<Survey_AnswerStatisticsDTO>> AnswerStatistics([FromBody] Survey_SurveyDTO Survey_SurveyDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Survey_SurveyDTO.Id))
                return Forbid();

            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);

            List<SurveyResult> SurveyResults = await SurveyResultService.List(new SurveyResultFilter
            {
                Selects = SurveyResultSelect.Id | SurveyResultSelect.Store | SurveyResultSelect.AppUser | SurveyResultSelect.StoreScouting
                | SurveyResultSelect.RespondentAddress | SurveyResultSelect.RespondentEmail | SurveyResultSelect.RespondentName | SurveyResultSelect.RespondentPhone
                | SurveyResultSelect.SurveyRespondentType,
                Skip = 0,
                Take = int.MaxValue,
                OrderBy = SurveyResultOrder.Time,
                OrderType = OrderType.ASC,
                SurveyId = new IdFilter { Equal = Survey_SurveyDTO.Id },
                OrganizationId = new IdFilter { Equal = CurrentUser.OrganizationId }
            });

            Survey_AnswerStatisticsDTO Survey_AnswerStatisticsDTO = new Survey_AnswerStatisticsDTO();
            Survey_AnswerStatisticsDTO.StoreCounter = SurveyResults.Where(s => s.StoreId.HasValue).Count();
            Survey_AnswerStatisticsDTO.StoreScoutingCounter = SurveyResults.Where(s => s.StoreScoutingId.HasValue).Count();
            Survey_AnswerStatisticsDTO.OtherCounter = SurveyResults.Where(s => !s.StoreId.HasValue && !s.StoreScoutingId.HasValue).Count();
            Survey_AnswerStatisticsDTO.TotalCounter = SurveyResults.Count();
            Survey_AnswerStatisticsDTO.StoreResults = SurveyResults.Where(s => s.StoreId.HasValue).Select(x => new Survey_StoreResultStatisticsDTO
            {
                OrganizationId = x.AppUser.Organization.Id,
                OrganizationName = x.AppUser.Organization.Name,
                StoreId = x.StoreId.Value,
                StoreCode = x.Store.Code,
                StoreName = x.Store.Name,
            }).ToList();

            Survey_AnswerStatisticsDTO.StoreScoutingResults = SurveyResults.Where(s => s.StoreScoutingId.HasValue).Select(x => new Survey_StoreScoutingResultStatisticsDTO
            {
                OrganizationId = x.AppUser.Organization.Id,
                OrganizationName = x.AppUser.Organization.Name,
                StoreScoutingId = x.StoreScoutingId.Value,
                StoreScoutingCode = x.StoreScouting.Code,
                StoreScoutingName = x.StoreScouting.Name,
            }).ToList();

            Survey_AnswerStatisticsDTO.OtherResults = SurveyResults.Where(s => !s.StoreId.HasValue && !s.StoreScoutingId.HasValue).Select(x => new Survey_OtherStatisticsDTO
            {
                Id = x.RowId,
                DisplayName = x.RespondentName,
                Address = x.RespondentAddress,
                Phone = x.RespondentPhone,
                Email = x.RespondentEmail,
            }).ToList();

            return Survey_AnswerStatisticsDTO;
        }

        [Route(SurveyRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] Survey_SurveyFilterDTO Survey_SurveyFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            long SurveyId = Survey_SurveyFilterDTO.Id?.Equal ?? 0;
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                #region Survey
                Survey Survey = await SurveyService.Get(SurveyId);
                if (Survey == null)
                    return null;
                List<SurveyResult> SurveyResults = await SurveyResultService.List(new SurveyResultFilter
                {
                    SurveyId = new IdFilter { Equal = SurveyId },
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = SurveyResultSelect.ALL,
                    OrderBy = SurveyResultOrder.Time,
                    OrderType = OrderType.DESC
                });

                List<string> header = new List<string>
                {
                    "Thời gian",
                    "Đối tượng khảo sát",
                    "Mã đại lý/Email",
                    "Tên đại lý/Họ và tên",
                    "Điện thoại",
                    "Địa chỉ",
                    "Đơn vị quản lý",
                    "Nhân viên khảo sát",
                };

                if (Survey.SurveyQuestions != null)
                    foreach (SurveyQuestion surveyQuestion in Survey.SurveyQuestions)
                    {
                        if (surveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.QUESTION_MULTIPLE_CHOICE.Id ||
                            surveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.QUESTION_SINGLE_CHOICE.Id)
                        {
                            header.Add($"{surveyQuestion.Content}");
                        }
                        if (surveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.TABLE_MULTIPLE_CHOICE.Id ||
                            surveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.TABLE_SINGLE_CHOICE.Id)
                        {
                            List<SurveyOption> SurveyOptions = surveyQuestion.SurveyOptions.Where(so => so.SurveyOptionTypeId == SurveyOptionTypeEnum.ROW.Id).ToList();
                            foreach (SurveyOption SurveyOption in SurveyOptions)
                            {
                                header.Add($"{surveyQuestion.Content} [{SurveyOption.Content}]");
                            }
                        }
                        if (surveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.QUESTION_TEXT.Id)
                        {
                            header.Add($"{surveyQuestion.Content}");
                        }
                    }

                List<long> AppUserIds = SurveyResults.Select(sr => sr.AppUserId).Distinct().ToList();
                List<AppUser> appUsers = await AppUserService.List(new AppUserFilter
                {
                    Id = new IdFilter { In = AppUserIds },
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = AppUserSelect.Id | AppUserSelect.DisplayName,
                });
                List<List<string>> result = new List<List<string>>();

                foreach (var SurveyResult in SurveyResults)
                {
                    List<string> optionResults = new List<string>();
                    result.Add(optionResults);

                    //thời gian
                    string Time = SurveyResult.Time.AddHours(CurrentContext.TimeZone).ToString("dd/MM/yyyy");
                    //đối tượng khảo sát
                    string SurveyRespondentType = SurveyResult.SurveyRespondentType?.Name;

                    string StoreCodeOrEmail = "";
                    string StoreNameOrDisplayName = "";
                    string Phone = "";
                    string Address = "";
                    string OrganizationName = "";
                    if (SurveyResult.SurveyRespondentTypeId == SurveyRespondentTypeEnum.STORE.Id)
                    {
                        StoreCodeOrEmail = SurveyResult.Store?.Code;
                        StoreNameOrDisplayName = SurveyResult.Store?.Name;
                        Phone = SurveyResult.Store?.Telephone;
                        Address = SurveyResult.Store?.Address;
                        OrganizationName = SurveyResult.Store?.Organization?.Name;
                    }
                    else if (SurveyResult.SurveyRespondentTypeId == SurveyRespondentTypeEnum.STORE_SCOUTING.Id)
                    {
                        StoreCodeOrEmail = SurveyResult.StoreScouting?.Code;
                        StoreNameOrDisplayName = SurveyResult.StoreScouting?.Name;
                        Phone = SurveyResult.StoreScouting?.OwnerPhone;
                        Address = SurveyResult.StoreScouting?.Address;
                        OrganizationName = SurveyResult.StoreScouting?.Organization?.Name;
                    }
                    else
                    {
                        StoreCodeOrEmail = SurveyResult.RespondentEmail;
                        StoreNameOrDisplayName = SurveyResult.RespondentName;
                        Phone = SurveyResult.RespondentPhone;
                        Address = SurveyResult.RespondentAddress;
                        OrganizationName = SurveyResult.AppUser?.Organization?.Name;
                    }
                    //nhân viên
                    string AppUserName = appUsers.Where(a => a.Id == SurveyResult.AppUserId).Select(a => a.DisplayName).FirstOrDefault();

                    optionResults.Add(Time);
                    optionResults.Add(SurveyRespondentType);
                    optionResults.Add(StoreCodeOrEmail);
                    optionResults.Add(StoreNameOrDisplayName);
                    optionResults.Add(Phone);
                    optionResults.Add(Address);
                    optionResults.Add(OrganizationName);
                    optionResults.Add(AppUserName);
                    if (Survey.SurveyQuestions != null)
                        foreach (SurveyQuestion surveyQuestion in Survey.SurveyQuestions)
                        {
                            if (surveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.QUESTION_MULTIPLE_CHOICE.Id ||
                            surveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.QUESTION_SINGLE_CHOICE.Id)
                            {
                                List<SurveyResultSingle> SurveyResultSingles = SurveyResult.SurveyResultSingles.Where(sr => sr.SurveyQuestionId == surveyQuestion.Id).ToList();
                                List<string> results = new List<string>();
                                foreach (SurveyResultSingle SurveyResultSingle in SurveyResultSingles)
                                {
                                    SurveyOption SurveyOption = surveyQuestion.SurveyOptions.Where(so => so.Id == SurveyResultSingle.SurveyOptionId).FirstOrDefault();
                                    if (SurveyOption != null)
                                        results.Add(SurveyOption.Content);
                                }
                                optionResults.Add(string.Join(';', results));
                            }
                            if (surveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.TABLE_MULTIPLE_CHOICE.Id ||
                                surveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.TABLE_SINGLE_CHOICE.Id)
                            {
                                List<SurveyOption> RowOptions = surveyQuestion.SurveyOptions.Where(so => so.SurveyOptionTypeId == SurveyOptionTypeEnum.ROW.Id).ToList();
                                foreach (SurveyOption RowOption in RowOptions)
                                {
                                    List<string> results = new List<string>();
                                    List<SurveyResultCell> SurveyResultCells = SurveyResult.SurveyResultCells.Where(sr => sr.SurveyQuestionId == surveyQuestion.Id && sr.RowOptionId == RowOption.Id).ToList();
                                    foreach (SurveyResultCell SurveyResultCell in SurveyResultCells)
                                    {
                                        SurveyOption ColumnOption = surveyQuestion.SurveyOptions.Where(so => so.Id == SurveyResultCell.ColumnOptionId).FirstOrDefault();
                                        if (ColumnOption != null)
                                            results.Add(ColumnOption.Content);
                                    }
                                    optionResults.Add(string.Join(';', results));
                                }
                            }
                            if (surveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.QUESTION_TEXT.Id)
                            {
                                List<SurveyResultText> SurveyResultTexts = SurveyResult.SurveyResultTexts.Where(sr => sr.SurveyQuestionId == surveyQuestion.Id).ToList();
                                foreach (SurveyResultText SurveyResultText in SurveyResultTexts)
                                {
                                    optionResults.Add(SurveyResultText.Content);
                                }
                            }
                        }
                }

                List<Object[]> SurveyData = new List<object[]>();
                foreach (var list in result)
                {
                    Object[] obj = list.ToArray();
                    SurveyData.Add(obj);
                }
                excel.GenerateWorksheet("Survey", header, SurveyData);
                #endregion
                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", "Survey.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            SurveyFilter SurveyFilter = new SurveyFilter();
            SurveyFilter = SurveyService.ToFilter(SurveyFilter);
            if (Id == 0)
            {

            }
            else
            {
                SurveyFilter.Id = new IdFilter { Equal = Id };
                int count = await SurveyService.Count(SurveyFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private Survey ConvertDTOToEntity(Survey_SurveyDTO Survey_SurveyDTO)
        {
            Survey_SurveyDTO.TrimString();
            Survey Survey = new Survey();
            Survey.Id = Survey_SurveyDTO.Id;
            Survey.Title = Survey_SurveyDTO.Title;
            Survey.Description = Survey_SurveyDTO.Description;
            Survey.StartAt = Survey_SurveyDTO.StartAt;
            Survey.EndAt = Survey_SurveyDTO.EndAt;
            Survey.StatusId = Survey_SurveyDTO.StatusId;
            Survey.OrganizationId = Survey_SurveyDTO.OrganizationId;
            Survey.StoreId = Survey_SurveyDTO.StoreId;
            Survey.SurveyQuestions = Survey_SurveyDTO.SurveyQuestions?
                .Select(x => new SurveyQuestion
                {
                    Id = x.Id,
                    Content = x.Content,
                    SurveyQuestionTypeId = x.SurveyQuestionTypeId,
                    IsMandatory = x.IsMandatory,
                    SurveyQuestionType = x.SurveyQuestionType == null ? null : new SurveyQuestionType
                    {
                        Id = x.SurveyQuestionType.Id,
                        Code = x.SurveyQuestionType.Code,
                        Name = x.SurveyQuestionType.Name,
                    },
                    SurveyOptions = x.SurveyOptions?.Select(x => new SurveyOption
                    {
                        Id = x.Id,
                        Content = x.Content,
                        SurveyOptionTypeId = x.SurveyOptionTypeId,
                        SurveyQuestionId = x.SurveyOptionTypeId
                    }).ToList(),
                    SurveyQuestionFileMappings = x.SurveyQuestionFileMappings?.Select(x => new SurveyQuestionFileMapping
                    {
                        FileId = x.FileId,
                        SurveyQuestionId = x.SurveyQuestionId
                    }).ToList(),
                    SurveyQuestionImageMappings = x.SurveyQuestionImageMappings?.Select(x => new SurveyQuestionImageMapping
                    {
                        ImageId = x.ImageId,
                        SurveyQuestionId = x.SurveyQuestionId
                    }).ToList(),
                    TableResult = x.TableResult,
                    ListResult = x.ListResult,
                }).ToList();
            Survey.BaseLanguage = CurrentContext.Language;
            return Survey;
        }

        private SurveyFilter ConvertFilterDTOToFilterEntity(Survey_SurveyFilterDTO Survey_SurveyFilterDTO)
        {
            SurveyFilter SurveyFilter = new SurveyFilter();
            SurveyFilter.Selects = SurveySelect.ALL;
            SurveyFilter.Skip = Survey_SurveyFilterDTO.Skip;
            SurveyFilter.Take = Survey_SurveyFilterDTO.Take;
            SurveyFilter.OrderBy = Survey_SurveyFilterDTO.OrderBy;
            SurveyFilter.OrderType = Survey_SurveyFilterDTO.OrderType;

            SurveyFilter.Id = Survey_SurveyFilterDTO.Id;
            SurveyFilter.Title = Survey_SurveyFilterDTO.Title;
            SurveyFilter.CreatorId = Survey_SurveyFilterDTO.CreatorId;
            SurveyFilter.OrganizationId = Survey_SurveyFilterDTO.OrganizationId;
            SurveyFilter.Description = Survey_SurveyFilterDTO.Description;
            SurveyFilter.StartAt = Survey_SurveyFilterDTO.StartAt;
            SurveyFilter.EndAt = Survey_SurveyFilterDTO.EndAt;
            SurveyFilter.StatusId = Survey_SurveyFilterDTO.StatusId;
            SurveyFilter.CreatedAt = Survey_SurveyFilterDTO.CreatedAt;
            SurveyFilter.UpdatedAt = Survey_SurveyFilterDTO.UpdatedAt;
            return SurveyFilter;
        }

        [Route(SurveyRoute.FilterListAppUser), HttpPost]
        public async Task<List<Survey_AppUserDTO>> FilterListAppUser([FromBody] Survey_AppUserFilterDTO Survey_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.Id = Survey_AppUserFilterDTO.Id;
            AppUserFilter.Username = Survey_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = Survey_AppUserFilterDTO.DisplayName;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<Survey_AppUserDTO> Survey_AppUserDTOs = AppUsers
                .Select(x => new Survey_AppUserDTO(x)).ToList();
            return Survey_AppUserDTOs;
        }
        [Route(SurveyRoute.FilterListStatus), HttpPost]
        public async Task<List<Survey_StatusDTO>> FilterListStatus([FromBody] Survey_StatusFilterDTO Survey_StatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = int.MaxValue;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<Survey_StatusDTO> Survey_StatusDTOs = Statuses
                .Select(x => new Survey_StatusDTO(x)).ToList();
            return Survey_StatusDTOs;
        }
        [Route(SurveyRoute.FilterListSurveyQuestionType), HttpPost]
        public async Task<List<Survey_SurveyQuestionTypeDTO>> FilterListSurveyQuestionType([FromBody] Survey_SurveyQuestionTypeFilterDTO Survey_SurveyQuestionTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            SurveyQuestionTypeFilter SurveyQuestionTypeFilter = new SurveyQuestionTypeFilter();
            SurveyQuestionTypeFilter.Skip = 0;
            SurveyQuestionTypeFilter.Take = int.MaxValue;
            SurveyQuestionTypeFilter.Take = 20;
            SurveyQuestionTypeFilter.OrderBy = SurveyQuestionTypeOrder.Id;
            SurveyQuestionTypeFilter.OrderType = OrderType.ASC;
            SurveyQuestionTypeFilter.Selects = SurveyQuestionTypeSelect.ALL;

            List<SurveyQuestionType> SurveyQuestionTypes = await SurveyQuestionTypeService.List(SurveyQuestionTypeFilter);
            List<Survey_SurveyQuestionTypeDTO> Survey_SurveyQuestionTypeDTOs = SurveyQuestionTypes
                .Select(x => new Survey_SurveyQuestionTypeDTO(x)).ToList();
            return Survey_SurveyQuestionTypeDTOs;
        }
        [Route(SurveyRoute.FilterListSurveyOptionType), HttpPost]
        public async Task<List<Survey_SurveyOptionTypeDTO>> FilterListSurveyOptionType([FromBody] Survey_SurveyOptionTypeFilterDTO Survey_SurveyOptionTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            SurveyOptionTypeFilter SurveyOptionTypeFilter = new SurveyOptionTypeFilter();
            SurveyOptionTypeFilter.Skip = 0;
            SurveyOptionTypeFilter.Take = int.MaxValue;
            SurveyOptionTypeFilter.Take = 20;
            SurveyOptionTypeFilter.OrderBy = SurveyOptionTypeOrder.Id;
            SurveyOptionTypeFilter.OrderType = OrderType.ASC;
            SurveyOptionTypeFilter.Selects = SurveyOptionTypeSelect.ALL;

            List<SurveyOptionType> SurveyOptionTypes = await SurveyOptionTypeService.List(SurveyOptionTypeFilter);
            List<Survey_SurveyOptionTypeDTO> Survey_SurveyOptionTypeDTOs = SurveyOptionTypes
                .Select(x => new Survey_SurveyOptionTypeDTO(x)).ToList();
            return Survey_SurveyOptionTypeDTOs;
        }

        [Route(SurveyRoute.FilterListOrganization), HttpPost]
        public async Task<List<Survey_OrganizationDTO>> FilterListOrganization()
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            OrganizationFilter.IsDisplay = true;
            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<Survey_OrganizationDTO> Survey_OrganizationDTOs = Organizations
                .Select(x => new Survey_OrganizationDTO(x)).ToList();
            return Survey_OrganizationDTOs;
        }
        [Route(SurveyRoute.SingleListSurveyQuestionType), HttpPost]
        public async Task<List<Survey_SurveyQuestionTypeDTO>> SingleListSurveyQuestionType([FromBody] Survey_SurveyQuestionTypeFilterDTO Survey_SurveyQuestionTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            SurveyQuestionTypeFilter SurveyQuestionTypeFilter = new SurveyQuestionTypeFilter();
            SurveyQuestionTypeFilter.Skip = 0;
            SurveyQuestionTypeFilter.Take = int.MaxValue;
            SurveyQuestionTypeFilter.Take = 20;
            SurveyQuestionTypeFilter.OrderBy = SurveyQuestionTypeOrder.Id;
            SurveyQuestionTypeFilter.OrderType = OrderType.ASC;
            SurveyQuestionTypeFilter.Selects = SurveyQuestionTypeSelect.ALL;

            List<SurveyQuestionType> SurveyQuestionTypes = await SurveyQuestionTypeService.List(SurveyQuestionTypeFilter);
            List<Survey_SurveyQuestionTypeDTO> Survey_SurveyQuestionTypeDTOs = SurveyQuestionTypes
                .Select(x => new Survey_SurveyQuestionTypeDTO(x)).ToList();
            return Survey_SurveyQuestionTypeDTOs;
        }
        [Route(SurveyRoute.SingleListAppUser), HttpPost]
        public async Task<List<Survey_AppUserDTO>> SingleListAppUser([FromBody] Survey_AppUserFilterDTO Survey_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.Id = Survey_AppUserFilterDTO.Id;
            AppUserFilter.Username = Survey_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = Survey_AppUserFilterDTO.DisplayName;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<Survey_AppUserDTO> Survey_AppUserDTOs = AppUsers
                .Select(x => new Survey_AppUserDTO(x)).ToList();
            return Survey_AppUserDTOs;
        }
        [Route(SurveyRoute.SingleListStatus), HttpPost]
        public async Task<List<Survey_StatusDTO>> SingleListStatus([FromBody] Survey_StatusFilterDTO Survey_StatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = int.MaxValue;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<Survey_StatusDTO> Survey_StatusDTOs = Statuses
                .Select(x => new Survey_StatusDTO(x)).ToList();
            return Survey_StatusDTOs;
        }
        [Route(SurveyRoute.SingleListSurveyOptionType), HttpPost]
        public async Task<List<Survey_SurveyOptionTypeDTO>> SingleListSurveyOptionType([FromBody] Survey_SurveyOptionTypeFilterDTO Survey_SurveyOptionTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            SurveyOptionTypeFilter SurveyOptionTypeFilter = new SurveyOptionTypeFilter();
            SurveyOptionTypeFilter.Skip = 0;
            SurveyOptionTypeFilter.Take = int.MaxValue;
            SurveyOptionTypeFilter.Take = 20;
            SurveyOptionTypeFilter.OrderBy = SurveyOptionTypeOrder.Id;
            SurveyOptionTypeFilter.OrderType = OrderType.ASC;
            SurveyOptionTypeFilter.Selects = SurveyOptionTypeSelect.ALL;

            List<SurveyOptionType> SurveyOptionTypes = await SurveyOptionTypeService.List(SurveyOptionTypeFilter);
            List<Survey_SurveyOptionTypeDTO> Survey_SurveyOptionTypeDTOs = SurveyOptionTypes
                .Select(x => new Survey_SurveyOptionTypeDTO(x)).ToList();
            return Survey_SurveyOptionTypeDTOs;
        }
        [Route(SurveyRoute.SingleListOrganization), HttpPost]
        public async Task<List<Survey_OrganizationDTO>> SingleListOrganization()
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            OrganizationFilter.IsDisplay = true;
            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<Survey_OrganizationDTO> Survey_OrganizationDTOs = Organizations
                .Select(x => new Survey_OrganizationDTO(x)).ToList();
            return Survey_OrganizationDTOs;
        }
        [Route(SurveyRoute.SaveQuestionImage), HttpPost]
        public async Task<ActionResult<Survey_ImageDTO>> SaveImage(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            MemoryStream memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            Image Image = new Image
            {
                Name = file.FileName,
                Content = memoryStream.ToArray(),
            };
            Image = await SurveyService.SaveImage(Image);
            if (Image == null)
                return BadRequest();
            Survey_ImageDTO product_ImageDTO = new Survey_ImageDTO
            {
                Id = Image.Id,
                Name = Image.Name,
                Url = Image.Url,
                ThumbnailUrl = Image.ThumbnailUrl,
            };
            return Ok(product_ImageDTO);
        }

        [HttpPost]
        [Route(SurveyRoute.SaveQuestionMultiFile)]
        public async Task<ActionResult<List<Survey_FileDTO>>> MultiUpload([FromForm] List<IFormFile> files)
        {
            List<Survey_FileDTO> Survey_FileDTOs = new List<Survey_FileDTO>();
            foreach (IFormFile file in files)
            {
                MemoryStream memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                File File = new File
                {
                    Name = file.FileName,
                    Content = memoryStream.ToArray(),
                };
                File = await SurveyService.SaveFile(File);
                if (File == null)
                    return BadRequest();
                Survey_FileDTO Company_FileDTO = new Survey_FileDTO(File);
                Survey_FileDTOs.Add(Company_FileDTO);
            }
            return Survey_FileDTOs;
        }

        private DateTime StartDay(DateTime date)
        {
            return date
                .AddHours(CurrentContext.TimeZone)
                .Date
                .AddHours(0 - CurrentContext.TimeZone);
        }

        private DateTime EndDay(DateTime date)
        {
            return date
                .AddHours(CurrentContext.TimeZone)
                .Date
                .AddHours(0 - CurrentContext.TimeZone)
                .AddDays(1)
                .AddSeconds(-1);
        }
    }
}

