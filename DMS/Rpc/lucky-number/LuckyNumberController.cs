using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MLuckyNumber;
using DMS.Services.MLuckyNumberGrouping;
using DMS.Services.MOrganization;
using DMS.Services.MRewardHistory;
using DMS.Services.MRewardHistoryContent;
using DMS.Services.MRewardStatus;
using DMS.Services.MStore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using TrueSight.Common;

namespace DMS.Rpc.lucky_number
{
    public partial class LuckyNumberController : RpcController
    {
        private IAppUserService AppUserService;
        private IOrganizationService OrganizationService;
        private IRewardStatusService RewardStatusService;
        private ILuckyNumberService LuckyNumberService;
        private ILuckyNumberGroupingService LuckyNumberGroupingService;
        private IRewardHistoryService RewardHistoryService;
        private IRewardHistoryContentService RewardHistoryContentService;
        private IStoreService StoreService;
        private DataContext DataContext;
        private ICurrentContext CurrentContext;
        public LuckyNumberController(
            IAppUserService AppUserService,
            IOrganizationService OrganizationService,
            IRewardStatusService RewardStatusService,
            ILuckyNumberService LuckyNumberService,
            ILuckyNumberGroupingService LuckyNumberGroupingService,
            IRewardHistoryService RewardHistoryService,
            IRewardHistoryContentService RewardHistoryContentService,
            IStoreService StoreService,
            DataContext DataContext,
            ICurrentContext CurrentContext
        )
        {
            this.AppUserService = AppUserService;
            this.OrganizationService = OrganizationService;
            this.RewardStatusService = RewardStatusService;
            this.LuckyNumberService = LuckyNumberService;
            this.LuckyNumberGroupingService = LuckyNumberGroupingService;
            this.RewardHistoryService = RewardHistoryService;
            this.RewardHistoryContentService = RewardHistoryContentService;
            this.StoreService = StoreService;
            this.DataContext = DataContext;
            this.CurrentContext = CurrentContext;
        }

        [Route(LuckyNumberRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] LuckyNumber_LuckyNumberFilterDTO LuckyNumber_LuckyNumberFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyNumberFilter LuckyNumberFilter = ConvertFilterDTOToFilterEntity(LuckyNumber_LuckyNumberFilterDTO);
            LuckyNumberFilter = await LuckyNumberService.ToFilter(LuckyNumberFilter);
            int count = await LuckyNumberService.Count(LuckyNumberFilter);
            return count;
        }

        [Route(LuckyNumberRoute.List), HttpPost]
        public async Task<ActionResult<List<LuckyNumber_LuckyNumberDTO>>> List([FromBody] LuckyNumber_LuckyNumberFilterDTO LuckyNumber_LuckyNumberFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyNumberFilter LuckyNumberFilter = ConvertFilterDTOToFilterEntity(LuckyNumber_LuckyNumberFilterDTO);
            LuckyNumberFilter = await LuckyNumberService.ToFilter(LuckyNumberFilter);
            List<LuckyNumber> LuckyNumbers = await LuckyNumberService.List(LuckyNumberFilter);
            List<LuckyNumber_LuckyNumberDTO> LuckyNumber_LuckyNumberDTOs = LuckyNumbers
                .Select(c => new LuckyNumber_LuckyNumberDTO(c)).ToList();
            return LuckyNumber_LuckyNumberDTOs;
        }

        [Route(LuckyNumberRoute.Get), HttpPost]
        public async Task<ActionResult<LuckyNumber_LuckyNumberDTO>> Get([FromBody] LuckyNumber_LuckyNumberDTO LuckyNumber_LuckyNumberDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(LuckyNumber_LuckyNumberDTO.Id))
                return Forbid();

            LuckyNumber LuckyNumber = await LuckyNumberService.Get(LuckyNumber_LuckyNumberDTO.Id);
            return new LuckyNumber_LuckyNumberDTO(LuckyNumber);
        }

        [Route(LuckyNumberRoute.Create), HttpPost]
        public async Task<ActionResult<LuckyNumber_LuckyNumberDTO>> Create([FromBody] LuckyNumber_LuckyNumberDTO LuckyNumber_LuckyNumberDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(LuckyNumber_LuckyNumberDTO.Id))
                return Forbid();

            LuckyNumber LuckyNumber = ConvertDTOToEntity(LuckyNumber_LuckyNumberDTO);
            LuckyNumber = await LuckyNumberService.Create(LuckyNumber);
            LuckyNumber_LuckyNumberDTO = new LuckyNumber_LuckyNumberDTO(LuckyNumber);
            if (LuckyNumber.IsValidated)
                return LuckyNumber_LuckyNumberDTO;
            else
                return BadRequest(LuckyNumber_LuckyNumberDTO);
        }

        [Route(LuckyNumberRoute.Update), HttpPost]
        public async Task<ActionResult<LuckyNumber_LuckyNumberDTO>> Update([FromBody] LuckyNumber_LuckyNumberDTO LuckyNumber_LuckyNumberDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(LuckyNumber_LuckyNumberDTO.Id))
                return Forbid();

            LuckyNumber LuckyNumber = ConvertDTOToEntity(LuckyNumber_LuckyNumberDTO);
            LuckyNumber = await LuckyNumberService.Update(LuckyNumber);
            LuckyNumber_LuckyNumberDTO = new LuckyNumber_LuckyNumberDTO(LuckyNumber);
            if (LuckyNumber.IsValidated)
                return LuckyNumber_LuckyNumberDTO;
            else
                return BadRequest(LuckyNumber_LuckyNumberDTO);
        }

        [Route(LuckyNumberRoute.Delete), HttpPost]
        public async Task<ActionResult<LuckyNumber_LuckyNumberDTO>> Delete([FromBody] LuckyNumber_LuckyNumberDTO LuckyNumber_LuckyNumberDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(LuckyNumber_LuckyNumberDTO.Id))
                return Forbid();

            LuckyNumber LuckyNumber = ConvertDTOToEntity(LuckyNumber_LuckyNumberDTO);
            LuckyNumber = await LuckyNumberService.Delete(LuckyNumber);
            LuckyNumber_LuckyNumberDTO = new LuckyNumber_LuckyNumberDTO(LuckyNumber);
            if (LuckyNumber.IsValidated)
                return LuckyNumber_LuckyNumberDTO;
            else
                return BadRequest(LuckyNumber_LuckyNumberDTO);
        }

        [Route(LuckyNumberRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyNumberFilter LuckyNumberFilter = new LuckyNumberFilter();
            LuckyNumberFilter = await LuckyNumberService.ToFilter(LuckyNumberFilter);
            LuckyNumberFilter.Id = new IdFilter { In = Ids };
            LuckyNumberFilter.Selects = LuckyNumberSelect.Id;
            LuckyNumberFilter.Skip = 0;
            LuckyNumberFilter.Take = int.MaxValue;

            List<LuckyNumber> LuckyNumbers = await LuckyNumberService.List(LuckyNumberFilter);
            LuckyNumbers = await LuckyNumberService.BulkDelete(LuckyNumbers);
            if (LuckyNumbers.Any(x => !x.IsValidated))
                return BadRequest(LuckyNumbers.Where(x => !x.IsValidated));
            return true;
        }

        [Route(LuckyNumberRoute.Import), HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            FileInfo FileInfo = new FileInfo(file.FileName);
            StringBuilder errorContent = new StringBuilder();
            if (!FileInfo.Extension.Equals(".xlsx"))
            {
                errorContent.AppendLine("Định dạng file không hợp lệ");
                return BadRequest(errorContent.ToString());
            }

            var OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            OrganizationFilter OrganizationFilter = new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.Id | OrganizationSelect.Code | OrganizationSelect.Name,
                Id = new IdFilter { In = OrganizationIds }
            };
            var Organizations = await OrganizationService.List(OrganizationFilter);

            List<LuckyNumberGrouping> LuckyNumberGroupings = await LuckyNumberGroupingService.List(new LuckyNumberGroupingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = LuckyNumberGroupingSelect.ALL,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            });

            List<LuckyNumber> LuckyNumbers = await LuckyNumberService.List(new LuckyNumberFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = LuckyNumberSelect.ALL,
            });

            HashSet<string> Codes = LuckyNumbers.Select(x => x.Code).ToHashSet();
            HashSet<string> OrganizationCodes = Organizations.Select(x => x.Code).ToHashSet();

            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Giải thưởng"];

                if (worksheet == null)
                {
                    errorContent.AppendLine("File không đúng biểu mẫu import");
                    return BadRequest(errorContent.ToString());
                }

                int StartColumn = 1;
                int StartRow = 2;
                int SttColumnn = 0 + StartColumn;
                int CodeColumn = 1 + StartColumn;
                int NameColumn = 2 + StartColumn;
                int ValueColumn = 3 + StartColumn;
                int OrganizationColumn = 4 + StartColumn;

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    string stt = worksheet.Cells[i, SttColumnn].Value?.ToString();
                    if (stt != null && stt.ToLower() == "END".ToLower())
                        break;
                    string CodeValue = worksheet.Cells[i, CodeColumn].Value?.ToString().Trim();
                    if (string.IsNullOrWhiteSpace(CodeValue) && i != worksheet.Dimension.End.Row)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i}: Chưa nhập mã quay thưởng");
                        continue;
                    }
                    else if (string.IsNullOrWhiteSpace(CodeValue) && i == worksheet.Dimension.End.Row)
                        break;
                    else if (Codes.Contains(CodeValue))
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i}: Mã quay thưởng đã tồn tại");
                        continue;
                    }

                    string OrganizationCodeValue = worksheet.Cells[i, OrganizationColumn].Value?.ToString().Trim();
                    if (string.IsNullOrWhiteSpace(OrganizationCodeValue) && i != worksheet.Dimension.End.Row)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i}: Chưa nhập mã đơn vị");
                        continue;
                    }
                    else if (!OrganizationCodes.Contains(OrganizationCodeValue))
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i}: Mã đơn vị không tồn tại");
                        continue;
                    }

                    Organization Organization = Organizations.Where(x => x.Code == OrganizationCodeValue).FirstOrDefault();
                    LuckyNumberGrouping LuckyNumberGrouping = LuckyNumberGroupings.Where(x => x.OrganizationId == Organization.Id).FirstOrDefault();
                    if (LuckyNumberGrouping == null)
                    {
                        LuckyNumberGrouping = new LuckyNumberGrouping()
                        {
                            OrganizationId = Organization.Id,
                            CreatedAt = StaticParams.DateTimeNow,
                            StatusId = StatusEnum.ACTIVE.Id,
                            StartDate = StaticParams.DateTimeNow,
                            Code = Guid.NewGuid().ToString(),
                            Name = "LuckyNumbers",

                        };
                        LuckyNumberGroupings.Add(LuckyNumberGrouping);
                    }
                    if (LuckyNumberGrouping.LuckyNumbers == null)
                        LuckyNumberGrouping.LuckyNumbers = new List<LuckyNumber>();
                    Codes.Add(CodeValue);
                    string NameValue = worksheet.Cells[i, NameColumn].Value?.ToString();
                    string ValueValue = worksheet.Cells[i, ValueColumn].Value?.ToString();

                    LuckyNumber LuckyNumber = new LuckyNumber();
                    LuckyNumber.Code = CodeValue;
                    LuckyNumber.Name = NameValue;
                    LuckyNumber.RewardStatusId = RewardStatusEnum.ACTIVE.Id;
                    LuckyNumber.RowId = Guid.NewGuid();
                    if (!string.IsNullOrWhiteSpace(ValueValue))
                    {
                        LuckyNumber.Value = ValueValue;
                    }
                    else
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i}: Chưa nhập giá trị giải thưởng");
                        continue;
                    }

                    LuckyNumberGrouping.LuckyNumbers.Add(LuckyNumber);
                }
            }

            if (errorContent.Length > 0)
                return BadRequest(errorContent.ToString());

            LuckyNumberGroupings = await LuckyNumberGroupingService.Import(LuckyNumberGroupings);
            List<LuckyNumber_LuckyNumberDTO> LuckyNumber_LuckyNumberDTOs = LuckyNumberGroupings.Where(x => x.LuckyNumbers != null).SelectMany(x => x.LuckyNumbers
                .Select(c => new LuckyNumber_LuckyNumberDTO(c))).ToList();
            return Ok(LuckyNumber_LuckyNumberDTOs);
        }

        [Route(LuckyNumberRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] LuckyNumber_LuckyNumberFilterDTO LuckyNumber_LuckyNumberFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyNumber_LuckyNumberFilterDTO.Skip = 0;
            LuckyNumber_LuckyNumberFilterDTO.Take = int.MaxValue;
            List<LuckyNumber_LuckyNumberDTO> LuckyNumber_LuckyNumberDTOs = (await List(LuckyNumber_LuckyNumberFilterDTO)).Value;
            long STT = 1;
            List<LuckyNumber_LuckyNumberExportDTO> LuckyNumber_LuckyNumberExportDTOs = LuckyNumber_LuckyNumberDTOs.Select(x => new LuckyNumber_LuckyNumberExportDTO
            {
                STT = STT++,
                Code = x.Code,
                Name = x.Name,
                Value = x.Value,
                RewardStatus = x.RewardStatus?.Name,
                Date = x.UsedAt?.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy")
            }).ToList();
            string path = "Templates/Lucky_Number_Report.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Exports = LuckyNumber_LuckyNumberExportDTOs;
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "LuckyNumberReport.xlsx");
        }

        [Route(LuckyNumberRoute.ExportStore), HttpPost]
        public async Task<ActionResult> ExportStore([FromBody] LuckyNumber_LuckyNumberFilterDTO LuckyNumber_LuckyNumberFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();

            AppUserFilter AppUserFilter = new AppUserFilter
            {
                OrganizationId = new IdFilter { In = OrganizationIds },
                Id = new IdFilter { },
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.DisplayName | AppUserSelect.Organization
            };
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            var AppUsers = await AppUserService.List(AppUserFilter);
            var AppUserIds = AppUsers.Select(x => x.Id).ToList();

            List<long> StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
                    .BulkInsertValuesIntoTempTableAsync<long>(StoreIds);

            var query = from rh in DataContext.RewardHistory
                        join tt in tempTableQuery.Query on rh.StoreId equals tt.Column1
                        where AppUserIds.Contains(rh.AppUserId)
                        select rh.Id;

            var RewardHistoryIds = await query.ToListWithNoLockAsync();
            RewardHistoryFilter RewardHistoryFilter = new RewardHistoryFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = RewardHistorySelect.ALL,
                Id = new IdFilter { In = RewardHistoryIds },
                CreatedAt = LuckyNumber_LuckyNumberFilterDTO.UsedAt
            };
            List<RewardHistory> RewardHistories = await RewardHistoryService.List(RewardHistoryFilter);
            RewardHistoryContentFilter RewardHistoryContentFilter = new RewardHistoryContentFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = RewardHistoryContentSelect.ALL,
                RewardHistoryId = new IdFilter { In = RewardHistoryIds }
            };
            List<RewardHistoryContent> RewardHistoryContents = await RewardHistoryContentService.List(RewardHistoryContentFilter);

            foreach (var RewardHistory in RewardHistories)
            {
                RewardHistory.RewardHistoryContents = RewardHistoryContents.Where(x => x.RewardHistoryId == RewardHistory.Id).ToList();
            }

            //var LuckyNumber_RewardHistoryDTOs = RewardHistories.Select(x => new LuckyNumber_RewardHistoryDTO(x)).ToList();

            var LuckyNumber_StoreExportDTOs = new List<LuckyNumber_LuckyNumberStoreExportDTO>();
            long stt = 1;
            foreach (var RewardHistory in RewardHistories)
            {
                foreach (var Content in RewardHistory.RewardHistoryContents)
                {
                    LuckyNumber_LuckyNumberStoreExportDTO LuckyNumber_LuckyNumberStoreExportDTO = new LuckyNumber_LuckyNumberStoreExportDTO()
                    {
                        STT = stt++,
                        StoreCode = RewardHistory.Store.Code,
                        StoreName = RewardHistory.Store.Name,
                        Revenue = RewardHistory.Revenue,
                        AppUserName = RewardHistory.AppUser.DisplayName,
                        LuckyNumberCode = Content.LuckyNumber.Code,
                        LuckyNumberName = Content.LuckyNumber.Name,
                        LuckyNumberValue = Content.LuckyNumber.Value,
                        LuckyNumberUsedAt = Content.LuckyNumber.UsedAt?.ToString("dd-MM-yyyy"),
                    };
                    LuckyNumber_StoreExportDTOs.Add(LuckyNumber_LuckyNumberStoreExportDTO);
                }
            }

            //var STT = 1;
            //foreach (var LuckyNumber_RewardHistoryDTO in LuckyNumber_RewardHistoryDTOs)
            //{
            //    LuckyNumber_RewardHistoryDTO.STT = STT++;
            //    LuckyNumber_RewardHistoryDTO.Date = LuckyNumber_RewardHistoryDTO.CreatedAt.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            //}
            string path = "Templates/Lucky_Number_Store_Report.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.LuckyNumber_StoreExportDTOs = LuckyNumber_StoreExportDTOs;
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "LuckyNumberStoreReport.xlsx");
        }

        [Route(LuckyNumberRoute.ExportTemplate), HttpPost]
        public async Task<FileResult> ExportTemplate([FromBody] LuckyNumber_LuckyNumberFilterDTO LuckyNumber_LuckyNumberFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            OrganizationFilter OrganizationFilter = new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.Id | OrganizationSelect.Code | OrganizationSelect.Name,
                Id = new IdFilter { In = OrganizationIds }
            };
            var Organizations = await OrganizationService.List(OrganizationFilter);

            string path = "Templates/Lucky_Number.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Organizations = Organizations.Select(x => new
            {
                Code = Utils.ReplaceHexadecimalSymbols(x.Code),
                Name = Utils.ReplaceHexadecimalSymbols(x.Name),
            });
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };
            return File(output.ToArray(), "application/octet-stream", "LuckyNumberTemplate.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            LuckyNumberFilter LuckyNumberFilter = new LuckyNumberFilter();
            LuckyNumberFilter = await LuckyNumberService.ToFilter(LuckyNumberFilter);
            if (Id == 0)
            {

            }
            else
            {
                LuckyNumberFilter.Id = new IdFilter { Equal = Id };
                int count = await LuckyNumberService.Count(LuckyNumberFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private LuckyNumber ConvertDTOToEntity(LuckyNumber_LuckyNumberDTO LuckyNumber_LuckyNumberDTO)
        {
            LuckyNumber_LuckyNumberDTO.TrimString();
            LuckyNumber LuckyNumber = new LuckyNumber();
            LuckyNumber.Id = LuckyNumber_LuckyNumberDTO.Id;
            LuckyNumber.Code = LuckyNumber_LuckyNumberDTO.Code;
            LuckyNumber.Name = LuckyNumber_LuckyNumberDTO.Name;
            LuckyNumber.RewardStatusId = LuckyNumber_LuckyNumberDTO.RewardStatusId;
            LuckyNumber.RowId = LuckyNumber_LuckyNumberDTO.RowId;
            LuckyNumber.RewardStatus = LuckyNumber_LuckyNumberDTO.RewardStatus == null ? null : new RewardStatus
            {
                Id = LuckyNumber_LuckyNumberDTO.RewardStatus.Id,
                Code = LuckyNumber_LuckyNumberDTO.RewardStatus.Code,
                Name = LuckyNumber_LuckyNumberDTO.RewardStatus.Name,
            };
            LuckyNumber.BaseLanguage = CurrentContext.Language;
            return LuckyNumber;
        }

        private LuckyNumberFilter ConvertFilterDTOToFilterEntity(LuckyNumber_LuckyNumberFilterDTO LuckyNumber_LuckyNumberFilterDTO)
        {
            LuckyNumberFilter LuckyNumberFilter = new LuckyNumberFilter();
            LuckyNumberFilter.Selects = LuckyNumberSelect.ALL;
            LuckyNumberFilter.Skip = LuckyNumber_LuckyNumberFilterDTO.Skip;
            LuckyNumberFilter.Take = LuckyNumber_LuckyNumberFilterDTO.Take;
            LuckyNumberFilter.OrderBy = LuckyNumber_LuckyNumberFilterDTO.OrderBy;
            LuckyNumberFilter.OrderType = LuckyNumber_LuckyNumberFilterDTO.OrderType;

            LuckyNumberFilter.Id = LuckyNumber_LuckyNumberFilterDTO.Id;
            LuckyNumberFilter.Code = LuckyNumber_LuckyNumberFilterDTO.Code;
            LuckyNumberFilter.Name = LuckyNumber_LuckyNumberFilterDTO.Name;
            LuckyNumberFilter.OrganizationId = LuckyNumber_LuckyNumberFilterDTO.OrganizationId;
            LuckyNumberFilter.RewardStatusId = LuckyNumber_LuckyNumberFilterDTO.RewardStatusId;
            LuckyNumberFilter.RowId = LuckyNumber_LuckyNumberFilterDTO.RowId;
            LuckyNumberFilter.CreatedAt = LuckyNumber_LuckyNumberFilterDTO.CreatedAt;
            LuckyNumberFilter.UpdatedAt = LuckyNumber_LuckyNumberFilterDTO.UpdatedAt;
            LuckyNumberFilter.UsedAt = LuckyNumber_LuckyNumberFilterDTO.UsedAt;
            return LuckyNumberFilter;
        }
    }
}

