using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Services.MAppUser;
using DMS.Services.MKpiCriteriaGeneral;
using DMS.Services.MKpiGeneral;
using DMS.Services.MKpiGeneralContent;
using DMS.Services.MKpiPeriod;
using DMS.Services.MKpiYear;
using DMS.Services.MOrganization;
using DMS.Services.MStatus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.kpi_general
{
    public class KpiGeneralController : RpcController
    {
        private IAppUserService AppUserService;
        private IKpiYearService KpiYearService;
        private IOrganizationService OrganizationService;
        private IStatusService StatusService;
        private IKpiCriteriaGeneralService KpiCriteriaGeneralService;
        private IKpiGeneralService KpiGeneralService;
        private IKpiGeneralContentService KpiGeneralContentService;
        private IKpiPeriodService KpiPeriodService;
        private ICurrentContext CurrentContext;
        public KpiGeneralController(
            IAppUserService AppUserService,
            IKpiYearService KpiYearService,
            IOrganizationService OrganizationService,
            IStatusService StatusService,
            IKpiCriteriaGeneralService KpiCriteriaGeneralService,
            IKpiGeneralService KpiGeneralService,
            IKpiGeneralContentService KpiGeneralContentService,
            IKpiPeriodService KpiPeriodService,
            ICurrentContext CurrentContext
        )
        {
            this.AppUserService = AppUserService;
            this.KpiYearService = KpiYearService;
            this.OrganizationService = OrganizationService;
            this.StatusService = StatusService;
            this.KpiCriteriaGeneralService = KpiCriteriaGeneralService;
            this.KpiGeneralService = KpiGeneralService;
            this.KpiGeneralContentService = KpiGeneralContentService;
            this.KpiPeriodService = KpiPeriodService;
            this.CurrentContext = CurrentContext;
        }

        [Route(KpiGeneralRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] KpiGeneral_KpiGeneralFilterDTO KpiGeneral_KpiGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiGeneralFilter KpiGeneralFilter = ConvertFilterDTOToFilterEntity(KpiGeneral_KpiGeneralFilterDTO);
            KpiGeneralFilter = await KpiGeneralService.ToFilter(KpiGeneralFilter);
            int count = await KpiGeneralService.Count(KpiGeneralFilter);
            return count;
        }

        [Route(KpiGeneralRoute.List), HttpPost]
        public async Task<List<KpiGeneral_KpiGeneralDTO>> List([FromBody] KpiGeneral_KpiGeneralFilterDTO KpiGeneral_KpiGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiGeneralFilter KpiGeneralFilter = ConvertFilterDTOToFilterEntity(KpiGeneral_KpiGeneralFilterDTO);
            KpiGeneralFilter = await KpiGeneralService.ToFilter(KpiGeneralFilter);
            List<KpiGeneral> KpiGenerals = await KpiGeneralService.List(KpiGeneralFilter);
            List<KpiGeneral_KpiGeneralDTO> KpiGeneral_KpiGeneralDTOs = KpiGenerals
                .Select(c => new KpiGeneral_KpiGeneralDTO(c)).ToList();
            return KpiGeneral_KpiGeneralDTOs;
        }

        [Route(KpiGeneralRoute.Get), HttpPost]
        public async Task<ActionResult<KpiGeneral_KpiGeneralDTO>> Get([FromBody] KpiGeneral_KpiGeneralDTO KpiGeneral_KpiGeneralDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(KpiGeneral_KpiGeneralDTO.Id))
                return Forbid();
            List<KpiPeriod> KpiPeriods = await KpiPeriodService.List(new KpiPeriodFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiPeriodSelect.ALL,
            });
            KpiGeneral KpiGeneral = await KpiGeneralService.Get(KpiGeneral_KpiGeneralDTO.Id);
            List<KpiCriteriaGeneral> KpiCriteriaGenerals = await KpiCriteriaGeneralService.List(new KpiCriteriaGeneralFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiCriteriaGeneralSelect.ALL,
                OrderBy = KpiCriteriaGeneralOrder.Id,
            });

            KpiGeneral_KpiGeneralDTO = new KpiGeneral_KpiGeneralDTO(KpiGeneral);
            (KpiGeneral_KpiGeneralDTO.CurrentMonth, KpiGeneral_KpiGeneralDTO.CurrentQuarter, KpiGeneral_KpiGeneralDTO.CurrentYear) = ConvertDateTime(StaticParams.DateTimeNow);


            if (KpiCriteriaGenerals.Count > KpiGeneral_KpiGeneralDTO.KpiGeneralContents.Count)
            {
                Dictionary<long, decimal?> KpiGeneralContentKpiPeriodMappings = new Dictionary<long, decimal?>();
                foreach (var KpiPeriod in KpiPeriods)
                {
                    KpiGeneralContentKpiPeriodMappings.Add(KpiPeriod.Id, null);
                }

                for (int i = 0; i < KpiCriteriaGenerals.Count; i++)
                {
                    KpiGeneral_KpiGeneralContentDTO KpiGeneral_KpiGeneralContentDTO = KpiGeneral_KpiGeneralDTO.KpiGeneralContents
                        .Where(x => x.KpiCriteriaGeneralId == KpiCriteriaGenerals[i].Id)
                        .FirstOrDefault();
                    if (KpiGeneral_KpiGeneralContentDTO == null)
                    {
                        KpiGeneral_KpiGeneralDTO.KpiGeneralContents.Add(new KpiGeneral_KpiGeneralContentDTO
                        {
                            KpiCriteriaGeneralId = KpiCriteriaGenerals[i].Id,
                            KpiCriteriaGeneral = new KpiGeneral_KpiCriteriaGeneralDTO
                            {
                                Id = KpiCriteriaGenerals[i].Id,
                                Code = KpiCriteriaGenerals[i].Code,
                                Name = KpiCriteriaGenerals[i].Name,
                            },
                            KpiGeneralContentKpiPeriodMappings = KpiGeneralContentKpiPeriodMappings,
                            KpiGeneralContentKpiPeriodMappingEnables = new Dictionary<long, bool>()
                        });

                    }
                }
            }

            KpiGeneral_KpiGeneralDTO.KpiGeneralContents = KpiGeneral_KpiGeneralDTO.KpiGeneralContents.OrderBy(x => x.KpiCriteriaGeneralId).ToList();

            foreach (var KpiGeneralContent in KpiGeneral_KpiGeneralDTO.KpiGeneralContents)
            {
                KpiGeneralContent.KpiGeneralContentKpiPeriodMappingEnables = GetPeriodEnables(KpiGeneral_KpiGeneralDTO, KpiPeriods);
            }

            return KpiGeneral_KpiGeneralDTO;
        }

        [Route(KpiGeneralRoute.GetDraft), HttpPost]
        public async Task<ActionResult<KpiGeneral_KpiGeneralDTO>> GetDraft([FromBody] KpiGeneral_KpiGeneralDTO KpiGeneral_KpiGeneralDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long KpiYearId = KpiGeneral_KpiGeneralDTO.KpiYearId == 0 ? StaticParams.DateTimeNow.Year : KpiGeneral_KpiGeneralDTO.KpiYearId;

            List<KpiCriteriaGeneral> KpiCriteriaGenerals = await KpiCriteriaGeneralService.List(new KpiCriteriaGeneralFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiCriteriaGeneralSelect.ALL,
            });
            List<KpiPeriod> KpiPeriods = await KpiPeriodService.List(new KpiPeriodFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiPeriodSelect.ALL,
            });
            if (KpiGeneral_KpiGeneralDTO == null) KpiGeneral_KpiGeneralDTO = new KpiGeneral_KpiGeneralDTO();
            (KpiGeneral_KpiGeneralDTO.CurrentMonth, KpiGeneral_KpiGeneralDTO.CurrentQuarter, KpiGeneral_KpiGeneralDTO.CurrentYear) = ConvertDateTime(StaticParams.DateTimeNow);
            KpiGeneral_KpiGeneralDTO.KpiPeriods = KpiPeriods.Select(x => new KpiGeneral_KpiPeriodDTO(x)).ToList();
            KpiGeneral_KpiGeneralDTO.KpiYearId = KpiYearId;
            KpiGeneral_KpiGeneralDTO.KpiYear = Enums.KpiYearEnum.KpiYearEnumList.Where(x => x.Id == KpiYearId)
                .Select(x => new KpiGeneral_KpiYearDTO
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                })
                .FirstOrDefault();
            KpiGeneral_KpiGeneralDTO.KpiGeneralContents = KpiCriteriaGenerals.Select(x => new KpiGeneral_KpiGeneralContentDTO
            {
                KpiCriteriaGeneralId = x.Id,
                KpiCriteriaGeneral = new KpiGeneral_KpiCriteriaGeneralDTO(x),
                KpiGeneralContentKpiPeriodMappings = KpiPeriods.ToDictionary(x => x.Id, y => (decimal?)null),
                KpiGeneralContentKpiPeriodMappingEnables = GetPeriodEnables(KpiGeneral_KpiGeneralDTO, KpiPeriods),
                Status = new KpiGeneral_StatusDTO
                {
                    Id = Enums.StatusEnum.ACTIVE.Id,
                    Code = Enums.StatusEnum.ACTIVE.Code,
                    Name = Enums.StatusEnum.ACTIVE.Name
                },
                StatusId = Enums.StatusEnum.ACTIVE.Id,
            }).OrderBy(x => x.KpiCriteriaGeneralId).ToList();
            KpiGeneral_KpiGeneralDTO.Status = new KpiGeneral_StatusDTO
            {
                Code = Enums.StatusEnum.ACTIVE.Code,
                Id = Enums.StatusEnum.ACTIVE.Id,
                Name = Enums.StatusEnum.ACTIVE.Name
            };
            KpiGeneral_KpiGeneralDTO.StatusId = Enums.StatusEnum.ACTIVE.Id;

            KpiGeneral_KpiGeneralDTO.KpiGeneralContents = KpiGeneral_KpiGeneralDTO.KpiGeneralContents.OrderBy(x => x.KpiCriteriaGeneralId).ToList();

            return KpiGeneral_KpiGeneralDTO;
        }
        [Route(KpiGeneralRoute.Create), HttpPost]
        public async Task<ActionResult<KpiGeneral_KpiGeneralDTO>> Create([FromBody] KpiGeneral_KpiGeneralDTO KpiGeneral_KpiGeneralDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(KpiGeneral_KpiGeneralDTO.Id))
                return Forbid();

            List<KpiPeriod> KpiPeriods = await KpiPeriodService.List(new KpiPeriodFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiPeriodSelect.ALL,
            });
            KpiGeneral KpiGeneral = ConvertDTOToEntity(KpiGeneral_KpiGeneralDTO);
            KpiGeneral = await KpiGeneralService.Create(KpiGeneral);
            KpiGeneral_KpiGeneralDTO = new KpiGeneral_KpiGeneralDTO(KpiGeneral);
            (KpiGeneral_KpiGeneralDTO.CurrentMonth, KpiGeneral_KpiGeneralDTO.CurrentQuarter, KpiGeneral_KpiGeneralDTO.CurrentYear) = ConvertDateTime(StaticParams.DateTimeNow);
            foreach (var KpiGeneralContent in KpiGeneral_KpiGeneralDTO.KpiGeneralContents)
            {
                KpiGeneralContent.KpiGeneralContentKpiPeriodMappingEnables = GetPeriodEnables(KpiGeneral_KpiGeneralDTO, KpiPeriods);
            }
            if (KpiGeneral.IsValidated)
                return KpiGeneral_KpiGeneralDTO;
            else
                return BadRequest(KpiGeneral_KpiGeneralDTO);
        }

        [Route(KpiGeneralRoute.Update), HttpPost]
        public async Task<ActionResult<KpiGeneral_KpiGeneralDTO>> Update([FromBody] KpiGeneral_KpiGeneralDTO KpiGeneral_KpiGeneralDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(KpiGeneral_KpiGeneralDTO.Id))
                return Forbid();

            List<KpiPeriod> KpiPeriods = await KpiPeriodService.List(new KpiPeriodFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiPeriodSelect.ALL,
            });

            KpiGeneral KpiGeneral = ConvertDTOToEntity(KpiGeneral_KpiGeneralDTO);
            KpiGeneral = await KpiGeneralService.Update(KpiGeneral);
            KpiGeneral_KpiGeneralDTO = new KpiGeneral_KpiGeneralDTO(KpiGeneral);
            (KpiGeneral_KpiGeneralDTO.CurrentMonth, KpiGeneral_KpiGeneralDTO.CurrentQuarter, KpiGeneral_KpiGeneralDTO.CurrentYear) = ConvertDateTime(StaticParams.DateTimeNow);
            foreach (var KpiGeneralContent in KpiGeneral_KpiGeneralDTO.KpiGeneralContents)
            {
                KpiGeneralContent.KpiGeneralContentKpiPeriodMappingEnables = GetPeriodEnables(KpiGeneral_KpiGeneralDTO, KpiPeriods);
            }
            if (KpiGeneral.IsValidated)
                return KpiGeneral_KpiGeneralDTO;
            else
                return BadRequest(KpiGeneral_KpiGeneralDTO);
        }

        [Route(KpiGeneralRoute.Delete), HttpPost]
        public async Task<ActionResult<KpiGeneral_KpiGeneralDTO>> Delete([FromBody] KpiGeneral_KpiGeneralDTO KpiGeneral_KpiGeneralDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(KpiGeneral_KpiGeneralDTO.Id))
                return Forbid();

            KpiGeneral KpiGeneral = ConvertDTOToEntity(KpiGeneral_KpiGeneralDTO);
            KpiGeneral = await KpiGeneralService.Delete(KpiGeneral);
            KpiGeneral_KpiGeneralDTO = new KpiGeneral_KpiGeneralDTO(KpiGeneral);
            if (KpiGeneral.IsValidated)
                return KpiGeneral_KpiGeneralDTO;
            else
                return BadRequest(KpiGeneral_KpiGeneralDTO);
        }

        [Route(KpiGeneralRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiGeneralFilter KpiGeneralFilter = new KpiGeneralFilter();
            KpiGeneralFilter = await KpiGeneralService.ToFilter(KpiGeneralFilter);
            KpiGeneralFilter.Id = new IdFilter { In = Ids };
            KpiGeneralFilter.Selects = KpiGeneralSelect.Id;
            KpiGeneralFilter.Skip = 0;
            KpiGeneralFilter.Take = int.MaxValue;

            List<KpiGeneral> KpiGenerals = await KpiGeneralService.List(KpiGeneralFilter);
            KpiGenerals = await KpiGeneralService.BulkDelete(KpiGenerals);
            return true;
        }

        [Route(KpiGeneralRoute.Import), HttpPost]
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

            var AppUser = await AppUserService.Get(CurrentContext.UserId);

            AppUserFilter EmployeeFilter = new AppUserFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName | AppUserSelect.Organization,
                OrganizationId = new IdFilter { Equal = AppUser.OrganizationId }
            };
            List<AppUser> Employees = await AppUserService.List(EmployeeFilter);

            GenericEnum KpiYear;
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["KPI nhân viên"];

                if (worksheet == null)
                {
                    errorContent.AppendLine("File không đúng biểu mẫu import");
                    return BadRequest(errorContent.ToString());
                }

                string KpiYearValue = worksheet.Cells[2, 7].Value?.ToString();
                if (long.TryParse(KpiYearValue, out long KpiYearId))
                {
                    KpiYear = KpiYearEnum.KpiYearEnumList.Where(x => x.Id == KpiYearId).FirstOrDefault();
                    if (KpiYear != null && KpiYear.Id < StaticParams.DateTimeNow.Year)
                    {
                        errorContent.AppendLine("Không thể nhập Kpi trong quá khứ");
                        return BadRequest(errorContent.ToString());
                    }
                }
                else
                {
                    errorContent.AppendLine("Chưa chọn năm Kpi hoặc năm không hợp lệ");
                    return BadRequest(errorContent.ToString());
                }
            }

            var AppUserIds = Employees.Select(x => x.Id).ToList();
            List<KpiGeneral> KpiGenerals = await KpiGeneralService.List(new KpiGeneralFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                AppUserId = new IdFilter { In = AppUserIds },
                KpiYearId = new IdFilter { Equal = KpiYear.Id },
                Selects = KpiGeneralSelect.ALL
            });
            var KpiGeneralIds = KpiGenerals.Select(x => x.Id).ToList();
            List<KpiGeneralContent> KpiGeneralContents = await KpiGeneralContentService.List(new KpiGeneralContentFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiGeneralContentSelect.ALL,
                KpiGeneralId = new IdFilter { In = KpiGeneralIds },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            });
            foreach (KpiGeneral KpiGeneral in KpiGenerals)
            {
                KpiGeneral.KpiGeneralContents = KpiGeneralContents.Where(x => x.KpiGeneralId == KpiGeneral.Id).ToList();
            }

            List<KpiGeneral_ImportDTO> KpiGeneral_ImportDTOs = new List<KpiGeneral_ImportDTO>();

            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["KPI nhân viên"];

                if (worksheet == null)
                {
                    errorContent.AppendLine("File không đúng biểu mẫu import");
                    return BadRequest(errorContent.ToString());
                }
                int StartColumn = 1;
                int StartRow = 6;
                int UsernameColumn = 0 + StartColumn;
                int DisplayNameColumn = 1 + StartColumn;
                int CriterialColumn = 2 + StartColumn;
                int M1Column = 3 + StartColumn;
                int M2Column = 4 + StartColumn;
                int M3Column = 5 + StartColumn;
                int M4Column = 6 + StartColumn;
                int M5Column = 7 + StartColumn;
                int M6Column = 8 + StartColumn;
                int M7Column = 9 + StartColumn;
                int M8Column = 10 + StartColumn;
                int M9Column = 11 + StartColumn;
                int M10Column = 12 + StartColumn;
                int M11Column = 13 + StartColumn;
                int M12Column = 14 + StartColumn;
                int Q1Column = 15 + StartColumn;
                int Q2Column = 16 + StartColumn;
                int Q3Column = 17 + StartColumn;
                int Q4Column = 18 + StartColumn;
                int YColumn = 19 + StartColumn;

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    string UsernameValue = worksheet.Cells[i, UsernameColumn].Value?.ToString();
                    string CriterialValue = worksheet.Cells[i, CriterialColumn].Value?.ToString();
                    if (UsernameValue != null && UsernameValue.ToLower() == "END".ToLower())
                        break;
                    else if (!string.IsNullOrWhiteSpace(UsernameValue) && string.IsNullOrWhiteSpace(CriterialValue))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(UsernameValue) && i != worksheet.Dimension.End.Row)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i}: Chưa nhập mã nhân viên");
                        continue;
                    }
                    else if (string.IsNullOrWhiteSpace(UsernameValue) && i == worksheet.Dimension.End.Row)
                        break;

                    KpiGeneral_ImportDTO KpiGeneral_ImportDTO = new KpiGeneral_ImportDTO();
                    AppUser Employee = Employees.Where(x => x.Username.ToLower() == UsernameValue.ToLower()).FirstOrDefault();
                    if (Employee == null)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i}: Nhân viên không tồn tại");
                        continue;
                    }
                    else
                    {
                        KpiGeneral_ImportDTO.EmployeeId = Employee.Id;
                    }

                    if (!string.IsNullOrWhiteSpace(CriterialValue))
                    {
                        GenericEnum KpiCriteriaGeneral = KpiCriteriaGeneralEnum.KpiCriteriaGeneralEnumList.Where(x => x.Name == CriterialValue).FirstOrDefault();
                        if (KpiCriteriaGeneral == null)
                        {
                            errorContent.AppendLine($"Lỗi dòng thứ {i}: Tên chỉ tiêu không hợp lệ");
                            continue;
                        }
                        else
                        {
                            KpiGeneral_ImportDTO.KpiCriteriaGeneralId = KpiCriteriaGeneral.Id;
                        }
                    }

                    KpiGeneral_ImportDTO.Stt = i;
                    KpiGeneral_ImportDTO.UsernameValue = UsernameValue;
                    KpiGeneral_ImportDTO.CriterialValue = CriterialValue;
                    KpiGeneral_ImportDTO.M1Value = worksheet.Cells[i, M1Column].Value?.ToString();
                    KpiGeneral_ImportDTO.M2Value = worksheet.Cells[i, M2Column].Value?.ToString();
                    KpiGeneral_ImportDTO.M3Value = worksheet.Cells[i, M3Column].Value?.ToString();
                    KpiGeneral_ImportDTO.M4Value = worksheet.Cells[i, M4Column].Value?.ToString();
                    KpiGeneral_ImportDTO.M5Value = worksheet.Cells[i, M5Column].Value?.ToString();
                    KpiGeneral_ImportDTO.M6Value = worksheet.Cells[i, M6Column].Value?.ToString();
                    KpiGeneral_ImportDTO.M7Value = worksheet.Cells[i, M7Column].Value?.ToString();
                    KpiGeneral_ImportDTO.M8Value = worksheet.Cells[i, M8Column].Value?.ToString();
                    KpiGeneral_ImportDTO.M9Value = worksheet.Cells[i, M9Column].Value?.ToString();
                    KpiGeneral_ImportDTO.M10Value = worksheet.Cells[i, M10Column].Value?.ToString();
                    KpiGeneral_ImportDTO.M11Value = worksheet.Cells[i, M11Column].Value?.ToString();
                    KpiGeneral_ImportDTO.M12Value = worksheet.Cells[i, M12Column].Value?.ToString();
                    KpiGeneral_ImportDTO.Q1Value = worksheet.Cells[i, Q1Column].Value?.ToString();
                    KpiGeneral_ImportDTO.Q2Value = worksheet.Cells[i, Q2Column].Value?.ToString();
                    KpiGeneral_ImportDTO.Q3Value = worksheet.Cells[i, Q3Column].Value?.ToString();
                    KpiGeneral_ImportDTO.Q4Value = worksheet.Cells[i, Q4Column].Value?.ToString();
                    KpiGeneral_ImportDTO.YValue = worksheet.Cells[i, YColumn].Value?.ToString();
                    KpiGeneral_ImportDTO.KpiYearId = KpiYear.Id;
                    KpiGeneral_ImportDTOs.Add(KpiGeneral_ImportDTO);
                }
            }

            Dictionary<long, StringBuilder> Errors = new Dictionary<long, StringBuilder>();
            HashSet<KpiGeneral_RowDTO> KpiGeneral_RowDTOs = new HashSet<KpiGeneral_RowDTO>(KpiGenerals.Select(x => new KpiGeneral_RowDTO { AppUserId = x.EmployeeId, KpiYearId = x.KpiYearId }).ToList());
            foreach (KpiGeneral_ImportDTO KpiGeneral_ImportDTO in KpiGeneral_ImportDTOs)
            {
                Errors.Add(KpiGeneral_ImportDTO.Stt, new StringBuilder(""));
                KpiGeneral_ImportDTO.IsNew = false;
                if (!KpiGeneral_RowDTOs.Contains(new KpiGeneral_RowDTO { AppUserId = KpiGeneral_ImportDTO.EmployeeId, KpiYearId = KpiGeneral_ImportDTO.KpiYearId }))
                {
                    KpiGeneral_RowDTOs.Add(new KpiGeneral_RowDTO { AppUserId = KpiGeneral_ImportDTO.EmployeeId, KpiYearId = KpiGeneral_ImportDTO.KpiYearId });
                    KpiGeneral_ImportDTO.IsNew = true;

                    var Employee = Employees.Where(x => x.Username.ToLower() == KpiGeneral_ImportDTO.UsernameValue.ToLower()).FirstOrDefault();
                    KpiGeneral_ImportDTO.OrganizationId = Employee.OrganizationId;
                    KpiGeneral_ImportDTO.EmployeeId = Employee.Id;
                }
            }

            HashSet<long> KpiPeriodIds = new HashSet<long>();
            long CurrentMonth = 100 + StaticParams.DateTimeNow.Month;
            long CurrentQuater = 0;
            if (Enums.KpiPeriodEnum.PERIOD_MONTH01.Id <= CurrentMonth && CurrentMonth <= Enums.KpiPeriodEnum.PERIOD_MONTH03.Id)
                CurrentQuater = Enums.KpiPeriodEnum.PERIOD_QUATER01.Id;
            if (Enums.KpiPeriodEnum.PERIOD_MONTH04.Id <= CurrentMonth && CurrentMonth <= Enums.KpiPeriodEnum.PERIOD_MONTH06.Id)
                CurrentQuater = Enums.KpiPeriodEnum.PERIOD_QUATER02.Id;
            if (Enums.KpiPeriodEnum.PERIOD_MONTH07.Id <= CurrentMonth && CurrentMonth <= Enums.KpiPeriodEnum.PERIOD_MONTH09.Id)
                CurrentQuater = Enums.KpiPeriodEnum.PERIOD_QUATER03.Id;
            if (Enums.KpiPeriodEnum.PERIOD_MONTH10.Id <= CurrentMonth && CurrentMonth <= Enums.KpiPeriodEnum.PERIOD_MONTH12.Id)
                CurrentQuater = Enums.KpiPeriodEnum.PERIOD_QUATER04.Id;
            if (KpiYear.Id >= StaticParams.DateTimeNow.Year)
            {
                KpiPeriodIds.Add(KpiYear.Id);
            }
            foreach (var KpiPeriod in KpiPeriodEnum.KpiPeriodEnumList)
            {
                if (CurrentMonth <= KpiPeriod.Id && KpiPeriod.Id <= Enums.KpiPeriodEnum.PERIOD_MONTH12.Id)
                    KpiPeriodIds.Add(KpiPeriod.Id);
                if (CurrentQuater <= KpiPeriod.Id && KpiPeriod.Id <= Enums.KpiPeriodEnum.PERIOD_QUATER04.Id)
                    KpiPeriodIds.Add(KpiPeriod.Id);
            }

            foreach (var KpiGeneral_ImportDTO in KpiGeneral_ImportDTOs)
            {
                if (KpiGeneral_ImportDTO.HasValue == false)
                {
                    Errors[KpiGeneral_ImportDTO.Stt].Append($"Lỗi dòng thứ {KpiGeneral_ImportDTO.Stt}: Chưa nhập chỉ tiêu");
                    continue;
                }
                KpiGeneral_ImportDTO.KpiCriteriaGeneralId = KpiCriteriaGeneralEnum.KpiCriteriaGeneralEnumList.Where(x => x.Name.ToLower() == KpiGeneral_ImportDTO.CriterialValue.ToLower()).Select(x => x.Id).FirstOrDefault();
                KpiGeneral KpiGeneral;
                if (KpiGeneral_ImportDTO.IsNew)
                {
                    KpiGeneral = new KpiGeneral();
                    KpiGenerals.Add(KpiGeneral);
                    KpiGeneral.EmployeeId = KpiGeneral_ImportDTO.EmployeeId;
                    KpiGeneral.OrganizationId = KpiGeneral_ImportDTO.OrganizationId;
                    KpiGeneral.KpiYearId = KpiGeneral_ImportDTO.KpiYearId;
                    KpiGeneral.RowId = Guid.NewGuid();
                    KpiGeneral.KpiGeneralContents = KpiCriteriaGeneralEnum.KpiCriteriaGeneralEnumList.Select(x => new KpiGeneralContent
                    {
                        KpiCriteriaGeneralId = x.Id,
                        RowId = Guid.NewGuid(),
                        StatusId = StatusEnum.ACTIVE.Id,
                        KpiGeneralContentKpiPeriodMappings = KpiPeriodEnum.KpiPeriodEnumList.Select(x => new KpiGeneralContentKpiPeriodMapping
                        {
                            KpiPeriodId = x.Id
                        }).ToList()
                    }).ToList();
                }
                else
                {
                    KpiGeneral = KpiGenerals.Where(x => x.EmployeeId == KpiGeneral_ImportDTO.EmployeeId && x.KpiYearId == KpiGeneral_ImportDTO.KpiYearId).FirstOrDefault();
                }

                KpiGeneralContent KpiGeneralContent = KpiGeneral.KpiGeneralContents.Where(x => x.KpiCriteriaGeneralId == KpiGeneral_ImportDTO.KpiCriteriaGeneralId).FirstOrDefault();
                if (KpiGeneralContent != null)
                {
                    foreach (var KpiGeneralContentKpiPeriodMapping in KpiGeneralContent.KpiGeneralContentKpiPeriodMappings)
                    {
                        if (decimal.TryParse(KpiGeneral_ImportDTO.M1Value, out decimal M1) &&
                        KpiGeneralContentKpiPeriodMapping.KpiPeriodId == KpiPeriodEnum.PERIOD_MONTH01.Id &&
                        KpiPeriodIds.Contains(KpiPeriodEnum.PERIOD_MONTH01.Id))
                        {
                            KpiGeneralContentKpiPeriodMapping.Value = M1;
                        }
                        else if (decimal.TryParse(KpiGeneral_ImportDTO.M2Value, out decimal M2) &&
                        KpiGeneralContentKpiPeriodMapping.KpiPeriodId == KpiPeriodEnum.PERIOD_MONTH02.Id &&
                        KpiPeriodIds.Contains(KpiPeriodEnum.PERIOD_MONTH02.Id))
                        {
                            KpiGeneralContentKpiPeriodMapping.Value = M2;
                        }
                        else if (decimal.TryParse(KpiGeneral_ImportDTO.M3Value, out decimal M3) &&
                        KpiGeneralContentKpiPeriodMapping.KpiPeriodId == KpiPeriodEnum.PERIOD_MONTH03.Id &&
                        KpiPeriodIds.Contains(KpiPeriodEnum.PERIOD_MONTH03.Id))
                        {
                            KpiGeneralContentKpiPeriodMapping.Value = M3;
                        }
                        else if (decimal.TryParse(KpiGeneral_ImportDTO.M4Value, out decimal M4) &&
                        KpiGeneralContentKpiPeriodMapping.KpiPeriodId == KpiPeriodEnum.PERIOD_MONTH04.Id &&
                        KpiPeriodIds.Contains(KpiPeriodEnum.PERIOD_MONTH04.Id))
                        {
                            KpiGeneralContentKpiPeriodMapping.Value = M4;
                        }
                        else if (decimal.TryParse(KpiGeneral_ImportDTO.M5Value, out decimal M5) &&
                        KpiGeneralContentKpiPeriodMapping.KpiPeriodId == KpiPeriodEnum.PERIOD_MONTH05.Id &&
                        KpiPeriodIds.Contains(KpiPeriodEnum.PERIOD_MONTH05.Id))
                        {
                            KpiGeneralContentKpiPeriodMapping.Value = M5;
                        }
                        else if (decimal.TryParse(KpiGeneral_ImportDTO.M6Value, out decimal M6) &&
                        KpiGeneralContentKpiPeriodMapping.KpiPeriodId == KpiPeriodEnum.PERIOD_MONTH06.Id &&
                        KpiPeriodIds.Contains(KpiPeriodEnum.PERIOD_MONTH06.Id))
                        {
                            KpiGeneralContentKpiPeriodMapping.Value = M6;
                        }
                        else if (decimal.TryParse(KpiGeneral_ImportDTO.M7Value, out decimal M7) &&
                        KpiGeneralContentKpiPeriodMapping.KpiPeriodId == KpiPeriodEnum.PERIOD_MONTH07.Id &&
                        KpiPeriodIds.Contains(KpiPeriodEnum.PERIOD_MONTH07.Id))
                        {
                            KpiGeneralContentKpiPeriodMapping.Value = M7;
                        }
                        else if (decimal.TryParse(KpiGeneral_ImportDTO.M8Value, out decimal M8) &&
                        KpiGeneralContentKpiPeriodMapping.KpiPeriodId == KpiPeriodEnum.PERIOD_MONTH08.Id &&
                        KpiPeriodIds.Contains(KpiPeriodEnum.PERIOD_MONTH08.Id))
                        {
                            KpiGeneralContentKpiPeriodMapping.Value = M8;
                        }
                        else if (decimal.TryParse(KpiGeneral_ImportDTO.M9Value, out decimal M9) &&
                        KpiGeneralContentKpiPeriodMapping.KpiPeriodId == KpiPeriodEnum.PERIOD_MONTH09.Id &&
                        KpiPeriodIds.Contains(KpiPeriodEnum.PERIOD_MONTH09.Id))
                        {
                            KpiGeneralContentKpiPeriodMapping.Value = M9;
                        }
                        else if (decimal.TryParse(KpiGeneral_ImportDTO.M10Value, out decimal M10) &&
                        KpiGeneralContentKpiPeriodMapping.KpiPeriodId == KpiPeriodEnum.PERIOD_MONTH10.Id &&
                        KpiPeriodIds.Contains(KpiPeriodEnum.PERIOD_MONTH10.Id))
                        {
                            KpiGeneralContentKpiPeriodMapping.Value = M10;
                        }
                        else if (decimal.TryParse(KpiGeneral_ImportDTO.M11Value, out decimal M11) &&
                        KpiGeneralContentKpiPeriodMapping.KpiPeriodId == KpiPeriodEnum.PERIOD_MONTH11.Id &&
                        KpiPeriodIds.Contains(KpiPeriodEnum.PERIOD_MONTH11.Id))
                        {
                            KpiGeneralContentKpiPeriodMapping.Value = M11;
                        }
                        else if (decimal.TryParse(KpiGeneral_ImportDTO.M12Value, out decimal M12) &&
                        KpiGeneralContentKpiPeriodMapping.KpiPeriodId == KpiPeriodEnum.PERIOD_MONTH12.Id &&
                        KpiPeriodIds.Contains(KpiPeriodEnum.PERIOD_MONTH12.Id))
                        {
                            KpiGeneralContentKpiPeriodMapping.Value = M12;
                        }
                        else if (decimal.TryParse(KpiGeneral_ImportDTO.Q1Value, out decimal Q1) &&
                        KpiGeneralContentKpiPeriodMapping.KpiPeriodId == KpiPeriodEnum.PERIOD_QUATER01.Id &&
                        KpiPeriodIds.Contains(KpiPeriodEnum.PERIOD_QUATER01.Id))
                        {
                            KpiGeneralContentKpiPeriodMapping.Value = Q1;
                        }
                        else if (decimal.TryParse(KpiGeneral_ImportDTO.Q2Value, out decimal Q2) &&
                        KpiGeneralContentKpiPeriodMapping.KpiPeriodId == KpiPeriodEnum.PERIOD_QUATER02.Id &&
                        KpiPeriodIds.Contains(KpiPeriodEnum.PERIOD_QUATER02.Id))
                        {
                            KpiGeneralContentKpiPeriodMapping.Value = Q2;
                        }
                        else if (decimal.TryParse(KpiGeneral_ImportDTO.Q4Value, out decimal Q4) &&
                        KpiGeneralContentKpiPeriodMapping.KpiPeriodId == KpiPeriodEnum.PERIOD_QUATER04.Id &&
                        KpiPeriodIds.Contains(KpiPeriodEnum.PERIOD_QUATER04.Id))
                        {
                            KpiGeneralContentKpiPeriodMapping.Value = Q4;
                        }
                        else if (decimal.TryParse(KpiGeneral_ImportDTO.YValue, out decimal Y) &&
                        KpiGeneralContentKpiPeriodMapping.KpiPeriodId == KpiPeriodEnum.PERIOD_YEAR01.Id)
                        {
                            KpiGeneralContentKpiPeriodMapping.Value = Y;
                        }
                    }
                    bool flag = false;
                    foreach (var item in KpiGeneralContent.KpiGeneralContentKpiPeriodMappings)
                    {
                        if (item.Value != null) flag = true;
                    }
                    if (flag == false)
                    {
                        Errors[KpiGeneral_ImportDTO.Stt].AppendLine($"Lỗi dòng thứ {KpiGeneral_ImportDTO.Stt}: Không thể nhập dữ liệu KPI cho các kỳ trong quá khứ");
                        continue;
                    }
                }
                KpiGeneral.CreatorId = AppUser.Id;
                KpiGeneral.StatusId = StatusEnum.ACTIVE.Id;
            }

            if (errorContent.Length > 0)
                return BadRequest(errorContent.ToString());
            string error = string.Join("\n", Errors.Where(x => !string.IsNullOrWhiteSpace(x.Value.ToString())).Select(x => x.Value.ToString()).ToList());
            if (!string.IsNullOrWhiteSpace(error))
                return BadRequest(error);

            KpiGenerals = await KpiGeneralService.Import(KpiGenerals);
            List<KpiGeneral_KpiGeneralDTO> KpiGeneral_KpiGeneralDTOs = KpiGenerals
                .Select(c => new KpiGeneral_KpiGeneralDTO(c)).ToList();
            return Ok(KpiGeneral_KpiGeneralDTOs);
        }

        [Route(KpiGeneralRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] KpiGeneral_KpiGeneralFilterDTO KpiGeneral_KpiGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (KpiGeneral_KpiGeneralFilterDTO.KpiYearId.Equal.HasValue == false)
                return BadRequest(new { message = "Chưa chọn năm Kpi" });

            long KpiYearId = KpiGeneral_KpiGeneralFilterDTO.KpiYearId.Equal.Value;
            var KpiYear = KpiYearEnum.KpiYearEnumList.Where(x => x.Id == KpiYearId).FirstOrDefault();

            KpiGeneral_KpiGeneralFilterDTO.Skip = 0;
            KpiGeneral_KpiGeneralFilterDTO.Take = int.MaxValue;
            List<KpiGeneral_KpiGeneralDTO> KpiGeneral_KpiGeneralDTOs = await List(KpiGeneral_KpiGeneralFilterDTO);
            var KpiGeneralIds = KpiGeneral_KpiGeneralDTOs.Select(x => x.Id).ToList();
            List<KpiGeneralContent> KpiGeneralContents = await KpiGeneralContentService.List(new KpiGeneralContentFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiGeneralContentSelect.ALL,
                KpiGeneralId = new IdFilter { In = KpiGeneralIds },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            });
            List<KpiGeneral_KpiGeneralContentDTO> KpiGeneral_KpiGeneralContentDTOs = KpiGeneralContents
                .Select(x => new KpiGeneral_KpiGeneralContentDTO(x)).ToList();
            List<KpiCriteriaGeneral> KpiCriteriaGenerals = await KpiCriteriaGeneralService.List(new KpiCriteriaGeneralFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiCriteriaGeneralSelect.ALL
            });
            List<KpiGeneral_ExportDTO> KpiGeneral_ExportDTOs = new List<KpiGeneral_ExportDTO>();
            foreach (var KpiGeneral in KpiGeneral_KpiGeneralDTOs)
            {
                KpiGeneral_ExportDTO KpiGeneral_ExportDTO = new KpiGeneral_ExportDTO();
                KpiGeneral_ExportDTO.Username = KpiGeneral.Employee.Username;
                KpiGeneral_ExportDTO.DisplayName = KpiGeneral.Employee.DisplayName;
                KpiGeneral_ExportDTO.CriteriaContents = new List<KpiGeneral_ExportCriterialDTO>();
                foreach (var KpiCriteriaGeneral in KpiCriteriaGenerals)
                {
                    var content = KpiGeneral_KpiGeneralContentDTOs
                    .Where(x => x.KpiCriteriaGeneralId == KpiCriteriaGeneral.Id)
                    .Where(x => x.KpiGeneralId == KpiGeneral.Id)
                    .Select(x => x.KpiGeneralContentKpiPeriodMappings)
                    .FirstOrDefault();
                    if (content == null || content.Count < 17) continue;
                    KpiGeneral_ExportCriterialDTO KpiGeneral_ExportCriterialDTO = new KpiGeneral_ExportCriterialDTO();

                    KpiGeneral_ExportCriterialDTO.UserId = KpiGeneral.Employee.Username;
                    KpiGeneral_ExportCriterialDTO.UserName = KpiGeneral.Employee.DisplayName;
                    KpiGeneral_ExportCriterialDTO.CriteriaName = KpiCriteriaGeneral.Name;
                    KpiGeneral_ExportCriterialDTO.M1Value = content[KpiPeriodEnum.PERIOD_MONTH01.Id];
                    KpiGeneral_ExportCriterialDTO.M2Value = content[KpiPeriodEnum.PERIOD_MONTH02.Id];
                    KpiGeneral_ExportCriterialDTO.M3Value = content[KpiPeriodEnum.PERIOD_MONTH03.Id];
                    KpiGeneral_ExportCriterialDTO.M4Value = content[KpiPeriodEnum.PERIOD_MONTH04.Id];
                    KpiGeneral_ExportCriterialDTO.M5Value = content[KpiPeriodEnum.PERIOD_MONTH05.Id];
                    KpiGeneral_ExportCriterialDTO.M6Value = content[KpiPeriodEnum.PERIOD_MONTH06.Id];
                    KpiGeneral_ExportCriterialDTO.M7Value = content[KpiPeriodEnum.PERIOD_MONTH07.Id];
                    KpiGeneral_ExportCriterialDTO.M8Value = content[KpiPeriodEnum.PERIOD_MONTH08.Id];
                    KpiGeneral_ExportCriterialDTO.M9Value = content[KpiPeriodEnum.PERIOD_MONTH09.Id];
                    KpiGeneral_ExportCriterialDTO.M10Value = content[KpiPeriodEnum.PERIOD_MONTH10.Id];
                    KpiGeneral_ExportCriterialDTO.M11Value = content[KpiPeriodEnum.PERIOD_MONTH11.Id];
                    KpiGeneral_ExportCriterialDTO.M12Value = content[KpiPeriodEnum.PERIOD_MONTH12.Id];
                    KpiGeneral_ExportCriterialDTO.Q1Value = content[KpiPeriodEnum.PERIOD_QUATER01.Id];
                    KpiGeneral_ExportCriterialDTO.Q2Value = content[KpiPeriodEnum.PERIOD_QUATER02.Id];
                    KpiGeneral_ExportCriterialDTO.Q3Value = content[KpiPeriodEnum.PERIOD_QUATER03.Id];
                    KpiGeneral_ExportCriterialDTO.Q4Value = content[KpiPeriodEnum.PERIOD_QUATER04.Id];
                    KpiGeneral_ExportCriterialDTO.YValue = content[KpiPeriodEnum.PERIOD_YEAR01.Id];

                    KpiGeneral_ExportDTO.CriteriaContents.Add(KpiGeneral_ExportCriterialDTO);
                }

                KpiGeneral_ExportDTOs.Add(KpiGeneral_ExportDTO);
            }

            MemoryStream output = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(output))
            {
                #region Tieu de bao cao
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add("KPI nhân viên");
                ws.Cells.Style.Font.Name = "Times New Roman";
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                ws.Cells["D1"].Value = "THIẾT LẬP KPI CHO NHÂN VIÊN";
                ws.Cells["D1:K1"].Merge = true;
                ws.Cells["D1:K1"].Style.Font.Size = 14;
                ws.Cells["D1:K1"].Style.Font.Bold = true;
                ws.Cells["D1:K1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                ws.Cells["G2"].Value = "Năm";
                ws.Cells["G2"].Style.Font.Bold = true;
                ws.Cells["H2"].Value = KpiYear.Name;
                #endregion

                #region Header bang ket qua
                List<string> headers = new List<string>
                {
                    "Mã nhân viên",
                    "Tên nhân viên",
                    "Chỉ tiêu"
                };
                List<string> headerLine2 = new List<string>
                {
                    "", "", "",
                    "Tháng 1",
                    "Tháng 2",
                    "Tháng 3",
                    "Tháng 4",
                    "Tháng 5",
                    "Tháng 6",
                    "Tháng 7",
                    "Tháng 8",
                    "Tháng 9",
                    "Tháng 10",
                    "Tháng 11",
                    "Tháng 12",
                    "Quý 1",
                    "Quý 2",
                    "Quý 3",
                    "Quý 4",
                };

                int endColumnNumber = 20;
                string endColumnString = Char.ConvertFromUtf32(endColumnNumber + 64);
                if (endColumnNumber > 26) endColumnString = Char.ConvertFromUtf32(endColumnNumber / 26 + 64) + Char.ConvertFromUtf32(endColumnNumber % 26 + 64); ;

                string headerRange = $"A4:" + $"{endColumnString}5";
                List<string[]> Header = new List<string[]> { headers.ToArray(), headerLine2.ToArray() };
                ws.Cells[headerRange].LoadFromArrays(Header);
                ws.Cells[headerRange].Style.Font.Bold = true;
                ws.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#9BC2E6"));
                ws.Cells["A4:A5"].Merge = true;
                ws.Cells["B4:B5"].Merge = true;
                ws.Cells["C4:C5"].Merge = true;
                ws.Cells["D4"].Value = "Tháng";
                ws.Cells["D4:O4"].Merge = true;
                ws.Cells["P4"].Value = "Quý";
                ws.Cells["P4:S4"].Merge = true;
                ws.Cells["T4"].Value = "Năm";
                ws.Cells["T4:T5"].Merge = true;

                ws.Cells[headerRange].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[headerRange].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Cells["D5:S5"].AutoFitColumns();
                ws.Column(1).Width = 15;
                ws.Column(2).Width = 15;
                ws.Column(3).Width = 35;
                #endregion

                #region Du lieu bao cao
                int startRow = 6;

                if (KpiGeneral_ExportDTOs != null && KpiGeneral_ExportDTOs.Count > 0)
                {
                    foreach (var KpiGeneral_ExportDTO in KpiGeneral_ExportDTOs)
                    {
                        int currentRow = startRow;
                        ws.Cells[$"A{currentRow}"].Value = $"{KpiGeneral_ExportDTO.Username} - {KpiGeneral_ExportDTO.DisplayName}";
                        ws.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                        ws.Cells[$"A{currentRow}:{endColumnString}{currentRow}"].Merge = true;
                        ws.Cells[$"A{currentRow}:{endColumnString}{currentRow}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells[$"A{currentRow}:{endColumnString}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[$"A{currentRow}:{endColumnString}{currentRow}"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

                        currentRow += KpiGeneral_ExportDTO.CriteriaContents.Count;

                        ws.Cells[$"A{startRow + 1}:{endColumnString}{currentRow}"].LoadFromCollection(KpiGeneral_ExportDTO.CriteriaContents);
                        //ws.Cells[$"A{startRow + 1}:A{currentRow}"].Merge = true;
                        //ws.Cells[$"B{startRow + 1}:B{currentRow}"].Merge = true;
                        startRow = currentRow + 1; // Gán lại start row cho employee tiếp theo
                    }

                    ws.Cells[$"D7:{endColumnString}{startRow}"].Style.Numberformat.Format = "#,##0"; // format number column value
                }
                // All borders
                ws.Cells[$"A4:{endColumnString}{startRow}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{startRow}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{startRow}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{startRow}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                #endregion

                excel.Save();
            }

            return File(output.ToArray(), "application/octet-stream", "KpiGenerals.xlsx");
        }

        [Route(KpiGeneralRoute.ExportTemplate), HttpPost]
        public async Task<FileResult> ExportTemplate()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var appUser = await AppUserService.Get(CurrentContext.UserId);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = int.MaxValue;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.OrganizationId = new IdFilter { Equal = appUser.OrganizationId };
            AppUserFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            {
                if (AppUserFilter.Id.In == null) AppUserFilter.Id.In = new List<long>();
                AppUserFilter.Id.In.AddRange(await FilterAppUser(AppUserService, OrganizationService, CurrentContext));
            }
            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);

            List<KpiCriteriaGeneral> KpiCriteriaGenerals = await KpiCriteriaGeneralService.List(new KpiCriteriaGeneralFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiCriteriaGeneralSelect.Code | KpiCriteriaGeneralSelect.Name,
                OrderBy = KpiCriteriaGeneralOrder.Id,
                OrderType = OrderType.ASC
            });

            List<KpiGeneral_ExportDTO> KpiGeneral_ExportDTOs = new List<KpiGeneral_ExportDTO>();
            foreach (var AppUser in AppUsers)
            {
                KpiGeneral_ExportDTO KpiGeneral_ExportDTO = new KpiGeneral_ExportDTO();
                KpiGeneral_ExportDTO.Username = AppUser.Username;
                KpiGeneral_ExportDTO.DisplayName = AppUser.DisplayName;
                KpiGeneral_ExportDTO.CriteriaContents = new List<KpiGeneral_ExportCriterialDTO>();

                foreach (var KpiCriteriaGeneral in KpiCriteriaGenerals)
                {
                    KpiGeneral_ExportCriterialDTO KpiGeneral_ExportCriterialDTO = new KpiGeneral_ExportCriterialDTO();
                    KpiGeneral_ExportCriterialDTO.CriteriaName = KpiCriteriaGeneral.Name;
                    KpiGeneral_ExportDTO.CriteriaContents.Add(KpiGeneral_ExportCriterialDTO);
                }

                KpiGeneral_ExportDTOs.Add(KpiGeneral_ExportDTO);
            }

            MemoryStream output = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(output))
            {
                #region Tieu de bao cao
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add("KPI nhân viên");
                ws.Cells.Style.Font.Name = "Times New Roman";
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                ws.Cells["D1"].Value = "THIẾT LẬP KPI CHO NHÂN VIÊN";
                ws.Cells["D1:K1"].Merge = true;
                ws.Cells["D1:K1"].Style.Font.Size = 14;
                ws.Cells["D1:K1"].Style.Font.Bold = true;
                ws.Cells["D1:K1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                ws.Cells["F2"].Value = "Năm";
                ws.Cells["F2"].Style.Font.Bold = true;
                ws.Cells["G2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["G2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#E7E6E6"));
                var YearDropdown = ws.DataValidations.AddListValidation("G2");
                for (int i = 0; i < KpiYearEnum.KpiYearEnumList.Count; i++)
                {
                    YearDropdown.Formula.Values.Add(KpiYearEnum.KpiYearEnumList[i].Name);
                }
                #endregion

                #region Header bang ket qua
                List<string> headers = new List<string>
                {
                    "Mã nhân viên",
                    "Tên nhân viên",
                    "Chỉ tiêu"
                };
                List<string> headerLine2 = new List<string>
                {
                    "", "", "",
                    "Tháng 1",
                    "Tháng 2",
                    "Tháng 3",
                    "Tháng 4",
                    "Tháng 5",
                    "Tháng 6",
                    "Tháng 7",
                    "Tháng 8",
                    "Tháng 9",
                    "Tháng 10",
                    "Tháng 11",
                    "Tháng 12",
                    "Quý 1",
                    "Quý 2",
                    "Quý 3",
                    "Quý 4",
                };

                int endColumnNumber = 20;
                string endColumnString = Char.ConvertFromUtf32(endColumnNumber + 64);
                if (endColumnNumber > 26) endColumnString = Char.ConvertFromUtf32(endColumnNumber / 26 + 64) + Char.ConvertFromUtf32(endColumnNumber % 26 + 64); ;

                string headerRange = $"A4:" + $"{endColumnString}5";
                List<string[]> Header = new List<string[]> { headers.ToArray(), headerLine2.ToArray() };
                ws.Cells[headerRange].LoadFromArrays(Header);
                ws.Cells[headerRange].Style.Font.Bold = true;
                ws.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#9BC2E6"));
                ws.Cells["A4:A5"].Merge = true;
                ws.Cells["B4:B5"].Merge = true;
                ws.Cells["C4:C5"].Merge = true;
                ws.Cells["D4"].Value = "Tháng";
                ws.Cells["D4:O4"].Merge = true;
                ws.Cells["P4"].Value = "Quý";
                ws.Cells["P4:S4"].Merge = true;
                ws.Cells["T4"].Value = "Năm";
                ws.Cells["T4:T5"].Merge = true;

                ws.Cells[headerRange].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[headerRange].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Cells["D5:S5"].AutoFitColumns();
                ws.Column(1).Width = 15;
                ws.Column(2).Width = 15;
                ws.Column(3).Width = 35;
                #endregion

                #region Du lieu bao cao
                int startRow = 6;
                if (KpiGeneral_ExportDTOs != null && KpiGeneral_ExportDTOs.Count > 0)
                {
                    foreach (var KpiGeneral_ExportDTO in KpiGeneral_ExportDTOs)
                    {
                        int currentRow = startRow;
                        ws.Cells[$"A{currentRow}"].Value = $"{KpiGeneral_ExportDTO.Username} - {KpiGeneral_ExportDTO.DisplayName}";
                        ws.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                        ws.Cells[$"A{currentRow}:{endColumnString}{currentRow}"].Merge = true;
                        ws.Cells[$"A{currentRow}:{endColumnString}{currentRow}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells[$"A{currentRow}:{endColumnString}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[$"A{currentRow}:{endColumnString}{currentRow}"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

                        List<string[]> Contents = new List<string[]>();
                        foreach (var content in KpiGeneral_ExportDTO.CriteriaContents)
                        {
                            currentRow++;
                            List<string> lineData = new List<string>
                        {
                            KpiGeneral_ExportDTO.Username,
                            KpiGeneral_ExportDTO.DisplayName,
                            content.CriteriaName,
                        };

                            Contents.Add(lineData.ToArray());
                        }
                        ws.Cells[$"A{startRow + 1}:{endColumnString}{currentRow}"].LoadFromArrays(Contents);
                        startRow = currentRow + 1; // Gán lại start row cho employee tiếp theo
                    }
                }

                ws.Cells[$"A{startRow}"].Value = "END";
                ws.Cells[$"B{startRow}"].Value = "Vui lòng insert các chỉ tiêu vào theo từng nhân viên";
                ws.Cells[$"A{startRow}:B{startRow}"].Style.Font.Bold = true;
                ws.Cells[$"A4:{endColumnString}{startRow}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{startRow}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{startRow}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{startRow}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                ws.Cells[$"D7:{endColumnString}{startRow}"].Style.Numberformat.Format = "#,##0"; // format number column value
                #endregion

                #region Sheet chi tieu
                ExcelWorksheet ws2 = excel.Workbook.Worksheets.Add("Chỉ tiêu");
                ws2.Cells["A1"].Value = "Tên chỉ tiêu";
                ws2.Cells["A1"].Style.Font.Bold = true;
                ws2.Cells["A1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws2.Cells["A1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFE699"));
                List<string[]> criterias = new List<string[]>();
                foreach (var KpiCriteriaGeneral in KpiCriteriaGenerals)
                {
                    criterias.Add(new string[] { KpiCriteriaGeneral.Name });
                }
                ws2.Cells[$"A2:A{KpiCriteriaGenerals.Count + 1}"].LoadFromArrays(criterias);
                ws2.Column(1).Width = 30;
                #endregion

                #region Sheet Quy tắc import
                ExcelWorksheet ws3 = excel.Workbook.Worksheets.Add("Quy tắc import");
                ws3.Cells["A1"].Style.Font.Bold = true;
                List<string[]> ws3Text = new List<string[]>
                {
                    new string[] {"Quy tắc Import:" },
                    new string[] {"1. Năm KPI được nhập dựa theo giá trị nhập ở ô G2, dữ liệu được lựa chọn từ combo" },
                    new string[] {"2. Khi tải biểu mẫu thì hệ thống sẽ xuất ra file excel có đầy đủ thông tin như sau:" },
                    new string[] {" - Nhân viên: hiển thị toàn bộ nhân viên theo phân quyền của người dùng (orgunit, appuser, userID)" },
                    new string[] {" - Danh sách chỉ tiêu: xuất toàn bộ chỉ tiêu như đã có của hệ thống" },
                    new string[] {" - Giá trị: xuất giá trị đã có của nhân viên đó" },
                    new string[] {"3. Khi import thì check:" },
                    new string[] {" - Nếu đã tồn tại nhân viên + năm: update vào bản ghi đã tồn tại" },
                    new string[] {" - Nếu chưa có nhân viên + năm: insert KPI cho nhân viên đó" },
                    new string[] {" - Khi import thì check:" },
                    new string[] {"  + Mã nhân viên: check có tồn tại không, nếu không thì hiển thị thông báo 'Mã nhân viên không tồn tại'" },
                    new string[] {"  + Mã nhân viên - Tên nhân viên:  mã và tên phải trùng nhau, nếu không trùng thì thông báo 'Mã và tên nhân viên không khớp'" },
                    new string[] {"  + Giá trị các chỉ tiêu: nếu nhập khác số thì hiển thị thông báo 'Giá trị chỉ tiêu{Mã chỉ tiêu} ở dòng {dòng} sai định dạng'" },
                    new string[] {" - Kết thúc ở dòng END" }
                };
                ws3.Cells["A1:A14"].LoadFromArrays(ws3Text);
                ws3.Cells["A1:A14"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws3.Cells["A1:A14"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFF2CC"));
                ws3.Column(1).Width = 105;

                #endregion

                excel.Save();
            }
            return File(output.ToArray(), "application/octet-stream", "Template_Kpi_General.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            KpiGeneralFilter KpiGeneralFilter = new KpiGeneralFilter();
            KpiGeneralFilter = await KpiGeneralService.ToFilter(KpiGeneralFilter);
            if (Id == 0)
            {

            }
            else
            {
                KpiGeneralFilter.Id = new IdFilter { Equal = Id };
                int count = await KpiGeneralService.Count(KpiGeneralFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private KpiGeneral ConvertDTOToEntity(KpiGeneral_KpiGeneralDTO KpiGeneral_KpiGeneralDTO)
        {
            KpiGeneral_KpiGeneralDTO.TrimString();
            KpiGeneral KpiGeneral = new KpiGeneral();
            KpiGeneral.Id = KpiGeneral_KpiGeneralDTO.Id;
            KpiGeneral.OrganizationId = KpiGeneral_KpiGeneralDTO.OrganizationId;
            KpiGeneral.EmployeeId = KpiGeneral_KpiGeneralDTO.EmployeeId;
            KpiGeneral.EmployeeIds = KpiGeneral_KpiGeneralDTO.EmployeeIds;
            KpiGeneral.KpiYearId = KpiGeneral_KpiGeneralDTO.KpiYearId;
            KpiGeneral.StatusId = KpiGeneral_KpiGeneralDTO.StatusId;
            KpiGeneral.CreatorId = KpiGeneral_KpiGeneralDTO.CreatorId;
            KpiGeneral.Creator = KpiGeneral_KpiGeneralDTO.Creator == null ? null : new AppUser
            {
                Id = KpiGeneral_KpiGeneralDTO.Creator.Id,
                Username = KpiGeneral_KpiGeneralDTO.Creator.Username,
                DisplayName = KpiGeneral_KpiGeneralDTO.Creator.DisplayName,
                Address = KpiGeneral_KpiGeneralDTO.Creator.Address,
                Email = KpiGeneral_KpiGeneralDTO.Creator.Email,
                Phone = KpiGeneral_KpiGeneralDTO.Creator.Phone,
                PositionId = KpiGeneral_KpiGeneralDTO.Creator.PositionId,
                Department = KpiGeneral_KpiGeneralDTO.Creator.Department,
                OrganizationId = KpiGeneral_KpiGeneralDTO.Creator.OrganizationId,
                StatusId = KpiGeneral_KpiGeneralDTO.Creator.StatusId,
                Avatar = KpiGeneral_KpiGeneralDTO.Creator.Avatar,
                ProvinceId = KpiGeneral_KpiGeneralDTO.Creator.ProvinceId,
                SexId = KpiGeneral_KpiGeneralDTO.Creator.SexId,
                Birthday = KpiGeneral_KpiGeneralDTO.Creator.Birthday,
            };
            KpiGeneral.Employee = KpiGeneral_KpiGeneralDTO.Employee == null ? null : new AppUser
            {
                Id = KpiGeneral_KpiGeneralDTO.Employee.Id,
                Username = KpiGeneral_KpiGeneralDTO.Employee.Username,
                DisplayName = KpiGeneral_KpiGeneralDTO.Employee.DisplayName,
                Address = KpiGeneral_KpiGeneralDTO.Employee.Address,
                Email = KpiGeneral_KpiGeneralDTO.Employee.Email,
                Phone = KpiGeneral_KpiGeneralDTO.Employee.Phone,
                PositionId = KpiGeneral_KpiGeneralDTO.Employee.PositionId,
                Department = KpiGeneral_KpiGeneralDTO.Employee.Department,
                OrganizationId = KpiGeneral_KpiGeneralDTO.Employee.OrganizationId,
                StatusId = KpiGeneral_KpiGeneralDTO.Employee.StatusId,
                Avatar = KpiGeneral_KpiGeneralDTO.Employee.Avatar,
                ProvinceId = KpiGeneral_KpiGeneralDTO.Employee.ProvinceId,
                SexId = KpiGeneral_KpiGeneralDTO.Employee.SexId,
                Birthday = KpiGeneral_KpiGeneralDTO.Employee.Birthday,
            };
            KpiGeneral.KpiYear = KpiGeneral_KpiGeneralDTO.KpiYear == null ? null : new KpiYear
            {
                Id = KpiGeneral_KpiGeneralDTO.KpiYear.Id,
                Code = KpiGeneral_KpiGeneralDTO.KpiYear.Code,
                Name = KpiGeneral_KpiGeneralDTO.KpiYear.Name,
            };
            KpiGeneral.Organization = KpiGeneral_KpiGeneralDTO.Organization == null ? null : new Organization
            {
                Id = KpiGeneral_KpiGeneralDTO.Organization.Id,
                Code = KpiGeneral_KpiGeneralDTO.Organization.Code,
                Name = KpiGeneral_KpiGeneralDTO.Organization.Name,
                ParentId = KpiGeneral_KpiGeneralDTO.Organization.ParentId,
                Path = KpiGeneral_KpiGeneralDTO.Organization.Path,
                Level = KpiGeneral_KpiGeneralDTO.Organization.Level,
                StatusId = KpiGeneral_KpiGeneralDTO.Organization.StatusId,
                Phone = KpiGeneral_KpiGeneralDTO.Organization.Phone,
                Email = KpiGeneral_KpiGeneralDTO.Organization.Email,
                Address = KpiGeneral_KpiGeneralDTO.Organization.Address,
            };
            KpiGeneral.Status = KpiGeneral_KpiGeneralDTO.Status == null ? null : new Status
            {
                Id = KpiGeneral_KpiGeneralDTO.Status.Id,
                Code = KpiGeneral_KpiGeneralDTO.Status.Code,
                Name = KpiGeneral_KpiGeneralDTO.Status.Name,
            };
            KpiGeneral.KpiGeneralContents = KpiGeneral_KpiGeneralDTO.KpiGeneralContents?
                .Select(x => new KpiGeneralContent
                {
                    Id = x.Id,
                    KpiCriteriaGeneralId = x.KpiCriteriaGeneralId,
                    StatusId = x.StatusId,
                    KpiCriteriaGeneral = x.KpiCriteriaGeneral == null ? null : new KpiCriteriaGeneral
                    {
                        Id = x.KpiCriteriaGeneral.Id,
                        Code = x.KpiCriteriaGeneral.Code,
                        Name = x.KpiCriteriaGeneral.Name,
                    },
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                    KpiGeneralContentKpiPeriodMappings = x.KpiGeneralContentKpiPeriodMappings.Select(p => new KpiGeneralContentKpiPeriodMapping
                    {
                        Value = p.Value,
                        KpiPeriodId = p.Key
                    }).ToList()
                }).ToList();
            KpiGeneral.BaseLanguage = CurrentContext.Language;
            return KpiGeneral;
        }

        private KpiGeneralFilter ConvertFilterDTOToFilterEntity(KpiGeneral_KpiGeneralFilterDTO KpiGeneral_KpiGeneralFilterDTO)
        {
            KpiGeneralFilter KpiGeneralFilter = new KpiGeneralFilter();
            KpiGeneralFilter.Selects = KpiGeneralSelect.ALL;
            KpiGeneralFilter.Skip = KpiGeneral_KpiGeneralFilterDTO.Skip;
            KpiGeneralFilter.Take = KpiGeneral_KpiGeneralFilterDTO.Take;
            KpiGeneralFilter.OrderBy = KpiGeneral_KpiGeneralFilterDTO.OrderBy;
            KpiGeneralFilter.OrderType = KpiGeneral_KpiGeneralFilterDTO.OrderType;

            KpiGeneralFilter.Id = KpiGeneral_KpiGeneralFilterDTO.Id;
            KpiGeneralFilter.OrganizationId = KpiGeneral_KpiGeneralFilterDTO.OrganizationId;
            KpiGeneralFilter.AppUserId = KpiGeneral_KpiGeneralFilterDTO.AppUserId;
            KpiGeneralFilter.KpiYearId = KpiGeneral_KpiGeneralFilterDTO.KpiYearId;
            KpiGeneralFilter.StatusId = KpiGeneral_KpiGeneralFilterDTO.StatusId;
            KpiGeneralFilter.CreatorId = KpiGeneral_KpiGeneralFilterDTO.CreatorId;
            KpiGeneralFilter.CreatedAt = KpiGeneral_KpiGeneralFilterDTO.CreatedAt;
            KpiGeneralFilter.UpdatedAt = KpiGeneral_KpiGeneralFilterDTO.UpdatedAt;
            return KpiGeneralFilter;
        }

        private Tuple<GenericEnum, GenericEnum, GenericEnum> ConvertDateTime(DateTime date)
        {
            GenericEnum monthName = Enums.KpiPeriodEnum.PERIOD_MONTH01;
            GenericEnum quarterName = Enums.KpiPeriodEnum.PERIOD_MONTH01;
            GenericEnum yearName = Enums.KpiYearEnum.KpiYearEnumList.Where(x => x.Id == StaticParams.DateTimeNow.Year).FirstOrDefault();
            switch (date.Month)
            {
                case 1:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH01;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER01;
                    break;
                case 2:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH02;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER01;
                    break;
                case 3:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH03;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER01;
                    break;
                case 4:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH04;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER02;
                    break;
                case 5:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH05;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER02;
                    break;
                case 6:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH06;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER02;
                    break;
                case 7:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH07;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER03;
                    break;
                case 8:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH08;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER03;
                    break;
                case 9:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH09;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER03;
                    break;
                case 10:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH10;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER04;
                    break;
                case 11:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH11;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER04;
                    break;
                case 12:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH12;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER04;
                    break;
            }
            return Tuple.Create(monthName, quarterName, yearName);
        }

        [Route(KpiGeneralRoute.FilterListAppUser), HttpPost]
        public async Task<List<KpiGeneral_AppUserDTO>> FilterListAppUser([FromBody] KpiGeneral_AppUserFilterDTO KpiGeneral_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = KpiGeneral_AppUserFilterDTO.Id;
            AppUserFilter.Username = KpiGeneral_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = KpiGeneral_AppUserFilterDTO.DisplayName;
            AppUserFilter.OrganizationId = KpiGeneral_AppUserFilterDTO.OrganizationId;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<KpiGeneral_AppUserDTO> KpiGeneral_AppUserDTOs = AppUsers
                .Select(x => new KpiGeneral_AppUserDTO(x)).ToList();
            return KpiGeneral_AppUserDTOs;
        }

        [Route(KpiGeneralRoute.FilterListCreator), HttpPost]
        public async Task<List<KpiGeneral_AppUserDTO>> FilterListCreator([FromBody] KpiGeneral_AppUserFilterDTO KpiGeneral_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var appUser = await AppUserService.Get(CurrentContext.UserId);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = KpiGeneral_AppUserFilterDTO.Id;
            AppUserFilter.Username = KpiGeneral_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = KpiGeneral_AppUserFilterDTO.DisplayName;
            AppUserFilter.OrganizationId = new IdFilter { Equal = appUser.OrganizationId };

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<KpiGeneral_AppUserDTO> KpiGeneral_AppUserDTOs = AppUsers
                .Select(x => new KpiGeneral_AppUserDTO(x)).ToList();
            return KpiGeneral_AppUserDTOs;
        }
        [Route(KpiGeneralRoute.FilterListKpiYear), HttpPost]
        public async Task<List<KpiGeneral_KpiYearDTO>> FilterListKpiYear([FromBody] KpiGeneral_KpiYearFilterDTO KpiGeneral_KpiYearFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiYearFilter KpiYearFilter = new KpiYearFilter();
            KpiYearFilter.Skip = 0;
            KpiYearFilter.Take = int.MaxValue;
            KpiYearFilter.Take = 20;
            KpiYearFilter.OrderBy = KpiYearOrder.Id;
            KpiYearFilter.OrderType = OrderType.ASC;
            KpiYearFilter.Selects = KpiYearSelect.ALL;

            List<KpiYear> KpiYears = await KpiYearService.List(KpiYearFilter);
            List<KpiGeneral_KpiYearDTO> KpiGeneral_KpiYearDTOs = KpiYears
                .Select(x => new KpiGeneral_KpiYearDTO(x)).ToList();
            return KpiGeneral_KpiYearDTOs;
        }
        [Route(KpiGeneralRoute.FilterListOrganization), HttpPost]
        public async Task<List<KpiGeneral_OrganizationDTO>> FilterListOrganization([FromBody] KpiGeneral_OrganizationFilterDTO KpiGeneral_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = KpiGeneral_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = KpiGeneral_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = KpiGeneral_OrganizationFilterDTO.Name;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<KpiGeneral_OrganizationDTO> KpiGeneral_OrganizationDTOs = Organizations
                .Select(x => new KpiGeneral_OrganizationDTO(x)).ToList();
            return KpiGeneral_OrganizationDTOs;
        }
        [Route(KpiGeneralRoute.FilterListStatus), HttpPost]
        public async Task<List<KpiGeneral_StatusDTO>> FilterListStatus([FromBody] KpiGeneral_StatusFilterDTO KpiGeneral_StatusFilterDTO)
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
            List<KpiGeneral_StatusDTO> KpiGeneral_StatusDTOs = Statuses
                .Select(x => new KpiGeneral_StatusDTO(x)).ToList();
            return KpiGeneral_StatusDTOs;
        }

        [Route(KpiGeneralRoute.FilterListKpiCriteriaGeneral), HttpPost]
        public async Task<List<KpiGeneral_KpiCriteriaGeneralDTO>> FilterListKpiCriteriaGeneral([FromBody] KpiGeneral_KpiCriteriaGeneralFilterDTO KpiGeneral_KpiCriteriaGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiCriteriaGeneralFilter KpiCriteriaGeneralFilter = new KpiCriteriaGeneralFilter();
            KpiCriteriaGeneralFilter.Skip = 0;
            KpiCriteriaGeneralFilter.Take = int.MaxValue;
            KpiCriteriaGeneralFilter.Take = 20;
            KpiCriteriaGeneralFilter.OrderBy = KpiCriteriaGeneralOrder.Id;
            KpiCriteriaGeneralFilter.OrderType = OrderType.ASC;
            KpiCriteriaGeneralFilter.Selects = KpiCriteriaGeneralSelect.ALL;

            List<KpiCriteriaGeneral> KpiCriteriaGenerals = await KpiCriteriaGeneralService.List(KpiCriteriaGeneralFilter);
            List<KpiGeneral_KpiCriteriaGeneralDTO> KpiGeneral_KpiCriteriaGeneralDTOs = KpiCriteriaGenerals
                .Select(x => new KpiGeneral_KpiCriteriaGeneralDTO(x)).ToList();
            return KpiGeneral_KpiCriteriaGeneralDTOs;
        }

        [Route(KpiGeneralRoute.SingleListAppUser), HttpPost]
        public async Task<List<KpiGeneral_AppUserDTO>> SingleListAppUser([FromBody] KpiGeneral_AppUserFilterDTO KpiGeneral_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = KpiGeneral_AppUserFilterDTO.Id;
            AppUserFilter.Username = KpiGeneral_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = KpiGeneral_AppUserFilterDTO.DisplayName;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<KpiGeneral_AppUserDTO> KpiGeneral_AppUserDTOs = AppUsers
                .Select(x => new KpiGeneral_AppUserDTO(x)).ToList();
            return KpiGeneral_AppUserDTOs;
        }
        [Route(KpiGeneralRoute.SingleListKpiYear), HttpPost]
        public async Task<List<KpiGeneral_KpiYearDTO>> SingleListKpiYear([FromBody] KpiGeneral_KpiYearFilterDTO KpiGeneral_KpiYearFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiYearFilter KpiYearFilter = new KpiYearFilter();
            KpiYearFilter.Skip = 0;
            KpiYearFilter.Take = int.MaxValue;
            KpiYearFilter.Take = 20;
            KpiYearFilter.OrderBy = KpiYearOrder.Id;
            KpiYearFilter.OrderType = OrderType.ASC;
            KpiYearFilter.Selects = KpiYearSelect.ALL;

            List<KpiYear> KpiYears = await KpiYearService.List(KpiYearFilter);
            List<KpiGeneral_KpiYearDTO> KpiGeneral_KpiYearDTOs = KpiYears
                .Select(x => new KpiGeneral_KpiYearDTO(x)).ToList();
            return KpiGeneral_KpiYearDTOs;
        }
        [Route(KpiGeneralRoute.SingleListOrganization), HttpPost]
        public async Task<List<KpiGeneral_OrganizationDTO>> SingleListOrganization([FromBody] KpiGeneral_OrganizationFilterDTO KpiGeneral_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = KpiGeneral_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = KpiGeneral_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = KpiGeneral_OrganizationFilterDTO.Name;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<KpiGeneral_OrganizationDTO> KpiGeneral_OrganizationDTOs = Organizations
                .Select(x => new KpiGeneral_OrganizationDTO(x)).ToList();
            return KpiGeneral_OrganizationDTOs;
        }
        [Route(KpiGeneralRoute.SingleListStatus), HttpPost]
        public async Task<List<KpiGeneral_StatusDTO>> SingleListStatus([FromBody] KpiGeneral_StatusFilterDTO KpiGeneral_StatusFilterDTO)
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
            List<KpiGeneral_StatusDTO> KpiGeneral_StatusDTOs = Statuses
                .Select(x => new KpiGeneral_StatusDTO(x)).ToList();
            return KpiGeneral_StatusDTOs;
        }

        [Route(KpiGeneralRoute.SingleListKpiCriteriaGeneral), HttpPost]
        public async Task<List<KpiGeneral_KpiCriteriaGeneralDTO>> SingleListKpiCriteriaGeneral([FromBody] KpiGeneral_KpiCriteriaGeneralFilterDTO KpiGeneral_KpiCriteriaGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiCriteriaGeneralFilter KpiCriteriaGeneralFilter = new KpiCriteriaGeneralFilter();
            KpiCriteriaGeneralFilter.Skip = 0;
            KpiCriteriaGeneralFilter.Take = int.MaxValue;
            KpiCriteriaGeneralFilter.Take = 20;
            KpiCriteriaGeneralFilter.OrderBy = KpiCriteriaGeneralOrder.Id;
            KpiCriteriaGeneralFilter.OrderType = OrderType.ASC;
            KpiCriteriaGeneralFilter.Selects = KpiCriteriaGeneralSelect.ALL;

            List<KpiCriteriaGeneral> KpiCriteriaGenerals = await KpiCriteriaGeneralService.List(KpiCriteriaGeneralFilter);
            List<KpiGeneral_KpiCriteriaGeneralDTO> KpiGeneral_KpiCriteriaGeneralDTOs = KpiCriteriaGenerals
                .Select(x => new KpiGeneral_KpiCriteriaGeneralDTO(x)).ToList();
            return KpiGeneral_KpiCriteriaGeneralDTOs;
        }

        [Route(KpiGeneralRoute.CountAppUser), HttpPost]
        public async Task<long> CountAppUser([FromBody] KpiGeneral_AppUserFilterDTO KpiGeneral_AppUserFilterDTO)
        {
            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Id = KpiGeneral_AppUserFilterDTO.Id;
            AppUserFilter.Username = KpiGeneral_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = KpiGeneral_AppUserFilterDTO.DisplayName;
            AppUserFilter.Email = KpiGeneral_AppUserFilterDTO.Email;
            AppUserFilter.Phone = KpiGeneral_AppUserFilterDTO.Phone;
            AppUserFilter.OrganizationId = KpiGeneral_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            {
                if (AppUserFilter.Id.In == null) AppUserFilter.Id.In = new List<long>();
                AppUserFilter.Id.In.AddRange(await FilterAppUser(AppUserService, OrganizationService, CurrentContext));
            }

            return await KpiGeneralService.CountAppUser(AppUserFilter, KpiGeneral_AppUserFilterDTO.KpiYearId);
        }

        [Route(KpiGeneralRoute.ListAppUser), HttpPost]
        public async Task<List<KpiGeneral_AppUserDTO>> ListAppUser([FromBody] KpiGeneral_AppUserFilterDTO KpiGeneral_AppUserFilterDTO)
        {
            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = KpiGeneral_AppUserFilterDTO.Skip;
            AppUserFilter.Take = KpiGeneral_AppUserFilterDTO.Take;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = KpiGeneral_AppUserFilterDTO.Id;
            AppUserFilter.Username = KpiGeneral_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = KpiGeneral_AppUserFilterDTO.DisplayName;
            AppUserFilter.Email = KpiGeneral_AppUserFilterDTO.Email;
            AppUserFilter.OrganizationId = KpiGeneral_AppUserFilterDTO.OrganizationId;
            AppUserFilter.Phone = KpiGeneral_AppUserFilterDTO.Phone;
            AppUserFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            {
                if (AppUserFilter.Id.In == null) AppUserFilter.Id.In = new List<long>();
                AppUserFilter.Id.In.AddRange(await FilterAppUser(AppUserService, OrganizationService, CurrentContext));
            }

            List<AppUser> AppUsers = await KpiGeneralService.ListAppUser(AppUserFilter, KpiGeneral_AppUserFilterDTO.KpiYearId);
            List<KpiGeneral_AppUserDTO> KpiGeneral_AppUserDTOs = AppUsers
                .Select(x => new KpiGeneral_AppUserDTO(x)).ToList();
            return KpiGeneral_AppUserDTOs;
        }

        private Dictionary<long, bool> GetPeriodEnables(KpiGeneral_KpiGeneralDTO KpiGeneral_KpiGeneralDTO, List<KpiPeriod> KpiPeriods)
        {
            long CurrentMonth = 100 + StaticParams.DateTimeNow.Month;
            long CurrentQuater = 0;
            if (Enums.KpiPeriodEnum.PERIOD_MONTH01.Id <= CurrentMonth && CurrentMonth <= Enums.KpiPeriodEnum.PERIOD_MONTH03.Id)
                CurrentQuater = Enums.KpiPeriodEnum.PERIOD_QUATER01.Id;
            if (Enums.KpiPeriodEnum.PERIOD_MONTH04.Id <= CurrentMonth && CurrentMonth <= Enums.KpiPeriodEnum.PERIOD_MONTH06.Id)
                CurrentQuater = Enums.KpiPeriodEnum.PERIOD_QUATER02.Id;
            if (Enums.KpiPeriodEnum.PERIOD_MONTH07.Id <= CurrentMonth && CurrentMonth <= Enums.KpiPeriodEnum.PERIOD_MONTH09.Id)
                CurrentQuater = Enums.KpiPeriodEnum.PERIOD_QUATER03.Id;
            if (Enums.KpiPeriodEnum.PERIOD_MONTH10.Id <= CurrentMonth && CurrentMonth <= Enums.KpiPeriodEnum.PERIOD_MONTH12.Id)
                CurrentQuater = Enums.KpiPeriodEnum.PERIOD_QUATER04.Id;

            Dictionary<long, bool> KpiGeneralContentKpiPeriodMappingEnables = KpiPeriods.ToDictionary(x => x.Id, y => false);
            foreach (KpiPeriod KpiPeriod in KpiPeriods)
            {
                if (KpiGeneral_KpiGeneralDTO.KpiYearId > StaticParams.DateTimeNow.Year)
                {
                    KpiGeneralContentKpiPeriodMappingEnables[KpiPeriod.Id] = true;
                }
                else if (KpiGeneral_KpiGeneralDTO.KpiYearId == StaticParams.DateTimeNow.Year)
                {
                    KpiGeneralContentKpiPeriodMappingEnables[Enums.KpiPeriodEnum.PERIOD_YEAR01.Id] = true;
                    if (CurrentMonth <= KpiPeriod.Id && KpiPeriod.Id <= Enums.KpiPeriodEnum.PERIOD_MONTH12.Id)
                        KpiGeneralContentKpiPeriodMappingEnables[KpiPeriod.Id] = true;
                    if (CurrentQuater <= KpiPeriod.Id && KpiPeriod.Id <= Enums.KpiPeriodEnum.PERIOD_QUATER04.Id)
                        KpiGeneralContentKpiPeriodMappingEnables[KpiPeriod.Id] = true;
                }
            }


            return KpiGeneralContentKpiPeriodMappingEnables;
        }
    }
}

