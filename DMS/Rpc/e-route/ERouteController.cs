using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Services.MAppUser;
using DMS.Services.MERoute;
using DMS.Services.MERouteType;
using DMS.Services.MOrganization;
using DMS.Services.MStatus;
using DMS.Services.MStore;
using DMS.Services.MStoreChecking;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreStatus;
using DMS.Services.MStoreType;
using DMS.Services.MWorkflow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.e_route
{
    public partial class ERouteController : RpcController
    {
        private IAppUserService AppUserService;
        private IERouteTypeService ERouteTypeService;
        private IOrganizationService OrganizationService;
        private IRequestStateService RequestStateService;
        private IStatusService StatusService;
        private IStoreStatusService StoreStatusService;
        private IERouteService ERouteService;
        private IStoreService StoreService;
        private IStoreCheckingService StoreCheckingService;
        private IStoreGroupingService StoreGroupingService;
        private IStoreTypeService StoreTypeService;
        private ICurrentContext CurrentContext;
        public ERouteController(
            IAppUserService AppUserService,
            IERouteTypeService ERouteTypeService,
            IOrganizationService OrganizationService,
            IRequestStateService RequestStateService,
            IStatusService StatusService,
            IStoreStatusService StoreStatusService,
            IERouteService ERouteService,
            IStoreService StoreService,
            IStoreCheckingService StoreCheckingService,
            IStoreGroupingService StoreGroupingService,
            IStoreTypeService StoreTypeService,
            ICurrentContext CurrentContext
        )
        {
            this.AppUserService = AppUserService;
            this.ERouteTypeService = ERouteTypeService;
            this.OrganizationService = OrganizationService;
            this.RequestStateService = RequestStateService;
            this.StatusService = StatusService;
            this.StoreStatusService = StoreStatusService;
            this.ERouteService = ERouteService;
            this.StoreService = StoreService;
            this.StoreCheckingService = StoreCheckingService;
            this.StoreGroupingService = StoreGroupingService;
            this.StoreTypeService = StoreTypeService;
            this.CurrentContext = CurrentContext;
        }

        [Route(ERouteRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] ERoute_ERouteFilterDTO ERoute_ERouteFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ERouteFilter ERouteFilter = ConvertFilterDTOToFilterEntity(ERoute_ERouteFilterDTO);
            ERouteFilter = await ERouteService.ToFilter(ERouteFilter);
            int count = await ERouteService.Count(ERouteFilter);
            return count;
        }

        [Route(ERouteRoute.List), HttpPost]
        public async Task<ActionResult<List<ERoute_ERouteDTO>>> List([FromBody] ERoute_ERouteFilterDTO ERoute_ERouteFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ERouteFilter ERouteFilter = ConvertFilterDTOToFilterEntity(ERoute_ERouteFilterDTO);
            ERouteFilter = await ERouteService.ToFilter(ERouteFilter);
            List<ERoute> ERoutes = await ERouteService.List(ERouteFilter);
            List<ERoute_ERouteDTO> ERoute_ERouteDTOs = ERoutes
                .Select(c => new ERoute_ERouteDTO(c)).ToList();
            return ERoute_ERouteDTOs;
        }

        [Route(ERouteRoute.CountNew), HttpPost]
        public async Task<ActionResult<int>> CountNew([FromBody] ERoute_ERouteFilterDTO ERoute_ERouteFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ERouteFilter ERouteFilter = ConvertFilterDTOToFilterEntity(ERoute_ERouteFilterDTO);
            ERouteFilter = await ERouteService.ToFilter(ERouteFilter);
            int count = await ERouteService.CountNew(ERouteFilter);
            return count;
        }

        [Route(ERouteRoute.ListNew), HttpPost]
        public async Task<ActionResult<List<ERoute_ERouteDTO>>> ListNew([FromBody] ERoute_ERouteFilterDTO ERoute_ERouteFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ERouteFilter ERouteFilter = ConvertFilterDTOToFilterEntity(ERoute_ERouteFilterDTO);
            ERouteFilter = await ERouteService.ToFilter(ERouteFilter);
            List<ERoute> ERoutes = await ERouteService.ListNew(ERouteFilter);
            List<ERoute_ERouteDTO> ERoute_ERouteDTOs = ERoutes
                .Select(c => new ERoute_ERouteDTO(c)).ToList();
            return ERoute_ERouteDTOs;
        }


        [Route(ERouteRoute.CountPending), HttpPost]
        public async Task<ActionResult<int>> CountPending([FromBody] ERoute_ERouteFilterDTO ERoute_ERouteFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ERouteFilter ERouteFilter = ConvertFilterDTOToFilterEntity(ERoute_ERouteFilterDTO);
            ERouteFilter = await ERouteService.ToFilter(ERouteFilter);
            int count = await ERouteService.CountPending(ERouteFilter);
            return count;
        }

        [Route(ERouteRoute.ListPending), HttpPost]
        public async Task<ActionResult<List<ERoute_ERouteDTO>>> ListPending([FromBody] ERoute_ERouteFilterDTO ERoute_ERouteFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ERouteFilter ERouteFilter = ConvertFilterDTOToFilterEntity(ERoute_ERouteFilterDTO);
            ERouteFilter = await ERouteService.ToFilter(ERouteFilter);
            List<ERoute> ERoutes = await ERouteService.ListPending(ERouteFilter);
            List<ERoute_ERouteDTO> ERoute_ERouteDTOs = ERoutes
                .Select(c => new ERoute_ERouteDTO(c)).ToList();
            return ERoute_ERouteDTOs;
        }


        [Route(ERouteRoute.CountCompleted), HttpPost]
        public async Task<ActionResult<int>> CountCompleted([FromBody] ERoute_ERouteFilterDTO ERoute_ERouteFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ERouteFilter ERouteFilter = ConvertFilterDTOToFilterEntity(ERoute_ERouteFilterDTO);
            ERouteFilter = await ERouteService.ToFilter(ERouteFilter);
            int count = await ERouteService.CountCompleted(ERouteFilter);
            return count;
        }

        [Route(ERouteRoute.ListCompleted), HttpPost]
        public async Task<ActionResult<List<ERoute_ERouteDTO>>> ListCompleted([FromBody] ERoute_ERouteFilterDTO ERoute_ERouteFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ERouteFilter ERouteFilter = ConvertFilterDTOToFilterEntity(ERoute_ERouteFilterDTO);
            ERouteFilter = await ERouteService.ToFilter(ERouteFilter);
            List<ERoute> ERoutes = await ERouteService.ListCompleted(ERouteFilter);
            List<ERoute_ERouteDTO> ERoute_ERouteDTOs = ERoutes
                .Select(c => new ERoute_ERouteDTO(c)).ToList();
            return ERoute_ERouteDTOs;
        }

        [Route(ERouteRoute.Get), HttpPost]
        public async Task<ActionResult<ERoute_ERouteDTO>> Get([FromBody] ERoute_ERouteDTO ERoute_ERouteDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ERoute_ERouteDTO.Id))
                return Forbid();

            ERoute ERoute = await ERouteService.Get(ERoute_ERouteDTO.Id);
            return new ERoute_ERouteDTO(ERoute);
        }

        [Route(ERouteRoute.GetDetail), HttpPost]
        public async Task<ActionResult<ERoute_ERouteDTO>> GetDetail([FromBody] ERoute_ERouteDTO ERoute_ERouteDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ERoute ERoute = await ERouteService.GetDetail(ERoute_ERouteDTO.Id);
            ERoute_ERouteDTO = new ERoute_ERouteDTO(ERoute);
            return ERoute_ERouteDTO;
        }

        [Route(ERouteRoute.Create), HttpPost]
        public async Task<ActionResult<ERoute_ERouteDTO>> Create([FromBody] ERoute_ERouteDTO ERoute_ERouteDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ERoute_ERouteDTO.Id))
                return Forbid();

            ERoute ERoute = ConvertDTOToEntity(ERoute_ERouteDTO);
            ERoute = await ERouteService.Create(ERoute);
            ERoute_ERouteDTO = new ERoute_ERouteDTO(ERoute);
            if (ERoute.IsValidated)
                return ERoute_ERouteDTO;
            else
                return BadRequest(ERoute_ERouteDTO);
        }

        [Route(ERouteRoute.Update), HttpPost]
        public async Task<ActionResult<ERoute_ERouteDTO>> Update([FromBody] ERoute_ERouteDTO ERoute_ERouteDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ERoute_ERouteDTO.Id))
                return Forbid();

            ERoute ERoute = ConvertDTOToEntity(ERoute_ERouteDTO);
            ERoute = await ERouteService.Update(ERoute);
            ERoute_ERouteDTO = new ERoute_ERouteDTO(ERoute);
            if (ERoute.IsValidated)
                return ERoute_ERouteDTO;
            else
                return BadRequest(ERoute_ERouteDTO);
        }

        [Route(ERouteRoute.Send), HttpPost]
        public async Task<ActionResult<ERoute_ERouteDTO>> Send([FromBody] ERoute_ERouteDTO ERoute_ERouteDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ERoute_ERouteDTO.Id))
                return Forbid();

            ERoute ERoute = ConvertDTOToEntity(ERoute_ERouteDTO);
            ERoute = await ERouteService.Send(ERoute);
            ERoute_ERouteDTO = new ERoute_ERouteDTO(ERoute);
            if (ERoute.IsValidated)
                return ERoute_ERouteDTO;
            else
                return BadRequest(ERoute_ERouteDTO);
        }

        [Route(ERouteRoute.Approve), HttpPost]
        public async Task<ActionResult<ERoute_ERouteDTO>> Approve([FromBody] ERoute_ERouteDTO ERoute_ERouteDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ERoute_ERouteDTO.Id))
                return Forbid();

            ERoute ERoute = ConvertDTOToEntity(ERoute_ERouteDTO);
            ERoute = await ERouteService.Approve(ERoute);
            ERoute_ERouteDTO = new ERoute_ERouteDTO(ERoute);
            if (ERoute.IsValidated)
                return ERoute_ERouteDTO;
            else
                return BadRequest(ERoute_ERouteDTO);
        }

        [Route(ERouteRoute.Reject), HttpPost]
        public async Task<ActionResult<ERoute_ERouteDTO>> Reject([FromBody] ERoute_ERouteDTO ERoute_ERouteDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ERoute_ERouteDTO.Id))
                return Forbid();

            ERoute ERoute = ConvertDTOToEntity(ERoute_ERouteDTO);
            ERoute = await ERouteService.Reject(ERoute);
            ERoute_ERouteDTO = new ERoute_ERouteDTO(ERoute);
            if (ERoute.IsValidated)
                return ERoute_ERouteDTO;
            else
                return BadRequest(ERoute_ERouteDTO);
        }

        [Route(ERouteRoute.Delete), HttpPost]
        public async Task<ActionResult<ERoute_ERouteDTO>> Delete([FromBody] ERoute_ERouteDTO ERoute_ERouteDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ERoute_ERouteDTO.Id))
                return Forbid();

            ERoute ERoute = ConvertDTOToEntity(ERoute_ERouteDTO);
            ERoute = await ERouteService.Delete(ERoute);
            ERoute_ERouteDTO = new ERoute_ERouteDTO(ERoute);
            if (ERoute.IsValidated)
                return ERoute_ERouteDTO;
            else
                return BadRequest(ERoute_ERouteDTO);
        }

        [Route(ERouteRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ERouteFilter ERouteFilter = new ERouteFilter();
            ERouteFilter = await ERouteService.ToFilter(ERouteFilter);
            ERouteFilter.Id = new IdFilter { In = Ids };
            ERouteFilter.Selects = ERouteSelect.Id | ERouteSelect.RequestState;
            ERouteFilter.Skip = 0;
            ERouteFilter.Take = int.MaxValue;

            List<ERoute> ERoutes = await ERouteService.List(ERouteFilter);
            ERoutes = await ERouteService.BulkDelete(ERoutes);
            if (ERoutes.Any(x => !x.IsValidated))
                return BadRequest(ERoutes.Where(x => !x.IsValidated));
            return true;
        }

        [Route(ERouteRoute.Import), HttpPost]
        public async Task<ActionResult<List<ERoute_ERouteContentDTO>>> Import([FromForm] long ERouteId, [FromForm] long AppUserId, [FromForm] IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ERouteId))
                return Forbid();

            FileInfo FileInfo = new FileInfo(file.FileName);
            if (!FileInfo.Extension.Equals(".xlsx"))
                return BadRequest("Định dạng file không hợp lệ");

            var StoreFilter = new StoreFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.CodeDraft | StoreSelect.Name | StoreSelect.Address,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };
            var Stores = await StoreService.List(StoreFilter);

            var StoreAppUserFilter = new StoreFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.CodeDraft | StoreSelect.Name | StoreSelect.Address,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                AppUserId = new IdFilter { Equal = AppUserId }
            };
            var AppUserStores = await StoreService.List(StoreAppUserFilter);

            ERoute ERoute;
            List<ERoute_ERouteContentDTO> ERoute_ERouteContentDTOs = new List<ERoute_ERouteContentDTO>();
            if (ERouteId != 0)
            {
                ERoute = await ERouteService.Get(ERouteId);
                ERoute_ERouteContentDTOs = ERoute.ERouteContents?.Select(x => new ERoute_ERouteContentDTO(x)).ToList();
            }

            StringBuilder errorContent = new StringBuilder();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Tuyến"];
                if (worksheet == null)
                    return BadRequest("File không đúng biểu mẫu import");
                int StartColumn = 1;
                int StartRow = 3;
                int SttColumnn = 0 + StartColumn;
                int StoreCodeColumn = 1 + StartColumn;
                int MondayColumn = 9 + StartColumn;
                int TuesdayColumn = 10 + StartColumn;
                int WednesdayColumn = 11 + StartColumn;
                int ThursdayColumn = 12 + StartColumn;
                int FridayColumn = 13 + StartColumn;
                int SaturdayColumn = 14 + StartColumn;
                int SundayColumn = 15 + StartColumn;
                int Week1Column = 16 + StartColumn;
                int Week2Column = 17 + StartColumn;
                int Week3Column = 18 + StartColumn;
                int Week4Column = 19 + StartColumn;

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    string stt = worksheet.Cells[i, SttColumnn].Value?.ToString();
                    if (stt != null && stt.ToLower() == "END".ToLower())
                        break;

                    string StoreCodeValue = worksheet.Cells[i, StoreCodeColumn].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(StoreCodeValue) && i != worksheet.Dimension.End.Row)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i}: Chưa nhập mã đại lý");
                        continue;
                    }
                    else if (string.IsNullOrWhiteSpace(StoreCodeValue) && i == worksheet.Dimension.End.Row)
                        break;

                    var Store = Stores.Where(x => x.Code == StoreCodeValue).FirstOrDefault();
                    if (Store == null || Store.DeletedAt.HasValue)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i}: Mã đại lý không tồn tại hoặc đã dừng hoạt động");
                        continue;
                    }

                    var AppUserStore = AppUserStores.Where(x => x.Code == StoreCodeValue).FirstOrDefault();
                    if (AppUserStore == null)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i}: Mã đại lý không thuộc phạm vi đi tuyến của nhân viên");
                        continue;
                    }

                    string MondayValue = worksheet.Cells[i, MondayColumn].Value?.ToString();
                    string TuesdayValue = worksheet.Cells[i, TuesdayColumn].Value?.ToString();
                    string WednesdayValue = worksheet.Cells[i, WednesdayColumn].Value?.ToString();
                    string ThursdayValue = worksheet.Cells[i, ThursdayColumn].Value?.ToString();
                    string FridayValue = worksheet.Cells[i, FridayColumn].Value?.ToString();
                    string SaturdayValue = worksheet.Cells[i, SaturdayColumn].Value?.ToString();
                    string SundayValue = worksheet.Cells[i, SundayColumn].Value?.ToString();
                    string Week1Value = worksheet.Cells[i, Week1Column].Value?.ToString();
                    string Week2Value = worksheet.Cells[i, Week2Column].Value?.ToString();
                    string Week3Value = worksheet.Cells[i, Week3Column].Value?.ToString();
                    string Week4Value = worksheet.Cells[i, Week4Column].Value?.ToString();

                    var content = ERoute_ERouteContentDTOs.Where(x => x.StoreId == Store.Id).FirstOrDefault();
                    if (content == null)
                    {
                        content = new ERoute_ERouteContentDTO()
                        {
                            StoreId = Store.Id,
                            Store = new ERoute_StoreDTO(Store),
                            ERouteId = ERouteId
                        };
                        ERoute_ERouteContentDTOs.Add(content);
                    }
                    if (!string.IsNullOrWhiteSpace(MondayValue) && (MondayValue.ToLower() == "x"))
                    {
                        content.Monday = true;
                    }
                    else
                    {
                        content.Monday = false;
                    }

                    if (!string.IsNullOrWhiteSpace(TuesdayValue) && (TuesdayValue.ToLower() == "x"))
                    {
                        content.Tuesday = true;
                    }
                    else
                    {
                        content.Tuesday = false;
                    }

                    if (!string.IsNullOrWhiteSpace(WednesdayValue) && (WednesdayValue.ToLower() == "x"))
                    {
                        content.Wednesday = true;
                    }
                    else
                    {
                        content.Wednesday = false;
                    }

                    if (!string.IsNullOrWhiteSpace(ThursdayValue) && (ThursdayValue.ToLower() == "x"))
                    {
                        content.Thursday = true;
                    }
                    else
                    {
                        content.Thursday = false;
                    }

                    if (!string.IsNullOrWhiteSpace(FridayValue) && (FridayValue.ToLower() == "x"))
                    {
                        content.Friday = true;
                    }
                    else
                    {
                        content.Friday = false;
                    }

                    if (!string.IsNullOrWhiteSpace(SaturdayValue) && (SaturdayValue.ToLower() == "x"))
                    {
                        content.Saturday = true;
                    }
                    else
                    {
                        content.Saturday = false;
                    }

                    if (!string.IsNullOrWhiteSpace(SundayValue) && (SundayValue.ToLower() == "x"))
                    {
                        content.Sunday = true;
                    }
                    else
                    {
                        content.Sunday = false;
                    }

                    if (!string.IsNullOrWhiteSpace(Week1Value) && (Week1Value.ToLower() == "x"))
                    {
                        content.Week1 = true;
                    }
                    else
                    {
                        content.Week1 = false;
                    }

                    if (!string.IsNullOrWhiteSpace(Week2Value) && (Week2Value.ToLower() == "x"))
                    {
                        content.Week2 = true;
                    }
                    else
                    {
                        content.Week2 = false;
                    }

                    if (!string.IsNullOrWhiteSpace(Week3Value) && (Week3Value.ToLower() == "x"))
                    {
                        content.Week3 = true;
                    }
                    else
                    {
                        content.Week3 = false;
                    }

                    if (!string.IsNullOrWhiteSpace(Week4Value) && (Week4Value.ToLower() == "x"))
                    {
                        content.Week4 = true;
                    }
                    else
                    {
                        content.Week4 = false;
                    }
                }
                if (errorContent.Length > 0)
                    return BadRequest(errorContent.ToString());
            }

            return ERoute_ERouteContentDTOs;
        }

        [Route(ERouteRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] ERoute_ERouteDTO ERoute_ERouteDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long ERouteId = ERoute_ERouteDTO?.Id ?? 0;
            ERoute ERoute = await ERouteService.Get(ERouteId);
            if (ERoute == null)
                return BadRequest("Tuyến không tồn tại");

            List<ERoute_ExportDTO> ERoute_ExportDTOs = ERoute.ERouteContents?.Select(x => new ERoute_ExportDTO(x)).ToList();
            var stt = 1;
            foreach (var ERoute_ExportDTO in ERoute_ExportDTOs)
            {
                ERoute_ExportDTO.STT = stt++;
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string tempPath = "Templates/ERoute_Export.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(tempPath);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Contents = ERoute_ExportDTOs;
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };
            var fileName = $"ERoute_{ERoute.Code.Replace(".","-").Replace("/","-")}_{ERoute.SaleEmployee?.Username}.xlsx";
            return File(output.ToArray(), "application/octet-stream", fileName);
        }

        [Route(ERouteRoute.ExportTemplate), HttpPost]
        public async Task<ActionResult> ExportTemplate([FromBody] ERoute_ERouteDTO ERoute_ERouteDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var appUser = await AppUserService.Get(ERoute_ERouteDTO.SaleEmployeeId);
            if (appUser == null)
            {
                return BadRequest("Chưa chọn nhân viên hoặc nhân viên không tồn tại!");
            }
            var StoreIds = appUser.AppUserStoreMappings.Select(x => x.StoreId).ToList();
            var StoreFilter = new StoreFilter
            {
                Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.CodeDraft | StoreSelect.Name | StoreSelect.Address
                        | StoreSelect.Province | StoreSelect.District | StoreSelect.Ward | StoreSelect.StoreStatus | StoreSelect.StoreStatusId,
                Skip = 0,
                Take = int.MaxValue,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                // OrganizationId = new IdFilter { Equal = appUser.OrganizationId }
                // In case Store in Route scoped but out of SaleEmployee's organization.
            };
            if (StoreIds.Any())
            {
                StoreFilter.Id = new IdFilter { In = StoreIds };
            }
            var Stores = await StoreService.List(StoreFilter);
            var DraftStoreInScoped = Stores.Where(x => x.StoreStatusId == StoreStatusEnum.DRAFT.Id).ToList();
            var StoreInScoped = Stores.Where(x => x.StoreStatusId == StoreStatusEnum.OFFICIAL.Id).ToList();
            var DraftStoreFilter = new StoreFilter
            {
                Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.CodeDraft | StoreSelect.Name | StoreSelect.Address
                        | StoreSelect.Province | StoreSelect.District | StoreSelect.Ward | StoreSelect.StoreStatus | StoreSelect.StoreStatusId,
                Skip = 0,
                Take = int.MaxValue,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                StoreStatusId = new IdFilter { Equal = StoreStatusEnum.DRAFT.Id },
                OrganizationId = new IdFilter { Equal = appUser.OrganizationId },
                Id = new IdFilter { NotIn = StoreIds }
            };
            var DraftStores = await StoreService.List(DraftStoreFilter);
            DraftStores.AddRange(DraftStoreInScoped);
            List<ERoute_StoreExportDTO> ERoute_StoreExportDTOs = StoreInScoped.Select(x => new ERoute_StoreExportDTO(x)).ToList();
            List<ERoute_StoreExportDTO> ERoute_DraftStoreExportDTOs = DraftStores.Select(x => new ERoute_StoreExportDTO(x)).ToList();
            var stt = 1;
            var stt1 = 1;
            foreach (var ERoute_StoreExportDTO in ERoute_StoreExportDTOs)
            {
                ERoute_StoreExportDTO.STT = stt++;
            }
            foreach (var ERoute_DraftStoreExportDTO in ERoute_DraftStoreExportDTOs)
            {
                ERoute_DraftStoreExportDTO.STT = stt1++;
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string tempPath = "Templates/ERoute_Template.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(tempPath);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Stores = ERoute_StoreExportDTOs;
            Data.DraftStores = ERoute_DraftStoreExportDTOs;
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };
            return File(output.ToArray(), "application/octet-stream", "ERoute_Template.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            ERouteFilter ERouteFilter = new ERouteFilter();
            ERouteFilter = await ERouteService.ToFilter(ERouteFilter);
            if (Id == 0)
            {

            }
            else
            {
                ERouteFilter.Id = new IdFilter { Equal = Id };
                int count = await ERouteService.Count(ERouteFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private ERoute ConvertDTOToEntity(ERoute_ERouteDTO ERoute_ERouteDTO)
        {
            ERoute_ERouteDTO.TrimString();
            ERoute ERoute = new ERoute();
            ERoute.Id = ERoute_ERouteDTO.Id;
            ERoute.Code = ERoute_ERouteDTO.Code;
            ERoute.Name = ERoute_ERouteDTO.Name;
            ERoute.SaleEmployeeId = ERoute_ERouteDTO.SaleEmployeeId;
            ERoute.OrganizationId = ERoute_ERouteDTO.OrganizationId;
            ERoute.StartDate = ERoute_ERouteDTO.StartDate;
            ERoute.EndDate = ERoute_ERouteDTO.EndDate;
            ERoute.ERouteTypeId = ERoute_ERouteDTO.ERouteTypeId;
            ERoute.RequestStateId = ERoute_ERouteDTO.RequestStateId;
            ERoute.StatusId = ERoute_ERouteDTO.StatusId;
            ERoute.CreatorId = ERoute_ERouteDTO.CreatorId;
            ERoute.Creator = ERoute_ERouteDTO.Creator == null ? null : new AppUser
            {
                Id = ERoute_ERouteDTO.Creator.Id,
                Username = ERoute_ERouteDTO.Creator.Username,
                DisplayName = ERoute_ERouteDTO.Creator.DisplayName,
                Address = ERoute_ERouteDTO.Creator.Address,
                Email = ERoute_ERouteDTO.Creator.Email,
                Phone = ERoute_ERouteDTO.Creator.Phone,
                PositionId = ERoute_ERouteDTO.Creator.PositionId,
                Department = ERoute_ERouteDTO.Creator.Department,
                OrganizationId = ERoute_ERouteDTO.Creator.OrganizationId,
                SexId = ERoute_ERouteDTO.Creator.SexId,
                StatusId = ERoute_ERouteDTO.Creator.StatusId,
                Avatar = ERoute_ERouteDTO.Creator.Avatar,
                Birthday = ERoute_ERouteDTO.Creator.Birthday,
                ProvinceId = ERoute_ERouteDTO.Creator.ProvinceId,
            };
            ERoute.Organization = ERoute_ERouteDTO.Organization == null ? null : new Organization
            {
                Id = ERoute_ERouteDTO.Organization.Id,
                Code = ERoute_ERouteDTO.Organization.Code,
                Name = ERoute_ERouteDTO.Organization.Name,
                ParentId = ERoute_ERouteDTO.Organization.ParentId,
                Path = ERoute_ERouteDTO.Organization.Path,
                Level = ERoute_ERouteDTO.Organization.Level,
                StatusId = ERoute_ERouteDTO.Organization.StatusId,
                Phone = ERoute_ERouteDTO.Organization.Phone,
                Address = ERoute_ERouteDTO.Organization.Address,
                Email = ERoute_ERouteDTO.Organization.Email,
            };
            ERoute.ERouteType = ERoute_ERouteDTO.ERouteType == null ? null : new ERouteType
            {
                Id = ERoute_ERouteDTO.ERouteType.Id,
                Code = ERoute_ERouteDTO.ERouteType.Code,
                Name = ERoute_ERouteDTO.ERouteType.Name,
            };
            ERoute.RequestState = ERoute_ERouteDTO.RequestState == null ? null : new RequestState
            {
                Id = ERoute_ERouteDTO.RequestState.Id,
                Code = ERoute_ERouteDTO.RequestState.Code,
                Name = ERoute_ERouteDTO.RequestState.Name,
            };
            ERoute.SaleEmployee = ERoute_ERouteDTO.SaleEmployee == null ? null : new AppUser
            {
                Id = ERoute_ERouteDTO.SaleEmployee.Id,
                Username = ERoute_ERouteDTO.SaleEmployee.Username,
                DisplayName = ERoute_ERouteDTO.SaleEmployee.DisplayName,
                Address = ERoute_ERouteDTO.SaleEmployee.Address,
                Email = ERoute_ERouteDTO.SaleEmployee.Email,
                Phone = ERoute_ERouteDTO.SaleEmployee.Phone,
                PositionId = ERoute_ERouteDTO.SaleEmployee.PositionId,
                Department = ERoute_ERouteDTO.SaleEmployee.Department,
                OrganizationId = ERoute_ERouteDTO.SaleEmployee.OrganizationId,
                SexId = ERoute_ERouteDTO.SaleEmployee.SexId,
                StatusId = ERoute_ERouteDTO.SaleEmployee.StatusId,
                Avatar = ERoute_ERouteDTO.SaleEmployee.Avatar,
                Birthday = ERoute_ERouteDTO.SaleEmployee.Birthday,
                ProvinceId = ERoute_ERouteDTO.SaleEmployee.ProvinceId,
            };
            ERoute.Status = ERoute_ERouteDTO.Status == null ? null : new Status
            {
                Id = ERoute_ERouteDTO.Status.Id,
                Code = ERoute_ERouteDTO.Status.Code,
                Name = ERoute_ERouteDTO.Status.Name,
            };
            ERoute.ERouteContents = ERoute_ERouteDTO.ERouteContents?.Select(x => new ERouteContent
            {
                Id = x.Id,
                ERouteId = x.ERouteId,
                StoreId = x.StoreId,
                OrderNumber = x.OrderNumber,
                Monday = x.Monday,
                Tuesday = x.Tuesday,
                Wednesday = x.Wednesday,
                Thursday = x.Thursday,
                Friday = x.Friday,
                Saturday = x.Saturday,
                Sunday = x.Sunday,
                Week1 = x.Week1,
                Week2 = x.Week2,
                Week3 = x.Week3,
                Week4 = x.Week4,
                Store = new Store
                {
                    Id = x.Store.Id,
                    Code = x.Store.Code,
                    CodeDraft = x.Store.CodeDraft,
                    Name = x.Store.Name,
                    ParentStoreId = x.Store.ParentStoreId,
                    OrganizationId = x.Store.OrganizationId,
                    StoreTypeId = x.Store.StoreTypeId,
                    Telephone = x.Store.Telephone,
                    ProvinceId = x.Store.ProvinceId,
                    DistrictId = x.Store.DistrictId,
                    WardId = x.Store.WardId,
                    Address = x.Store.Address,
                    DeliveryAddress = x.Store.DeliveryAddress,
                    Latitude = x.Store.Latitude,
                    Longitude = x.Store.Longitude,
                    DeliveryLatitude = x.Store.DeliveryLatitude,
                    DeliveryLongitude = x.Store.DeliveryLongitude,
                    OwnerName = x.Store.OwnerName,
                    OwnerPhone = x.Store.OwnerPhone,
                    OwnerEmail = x.Store.OwnerEmail,
                    TaxCode = x.Store.TaxCode,
                    LegalEntity = x.Store.LegalEntity,
                    StatusId = x.Store.StatusId,
                },
            }).ToList();
            ERoute.BaseLanguage = CurrentContext.Language;
            return ERoute;
        }

        private ERouteFilter ConvertFilterDTOToFilterEntity(ERoute_ERouteFilterDTO ERoute_ERouteFilterDTO)
        {
            ERouteFilter ERouteFilter = new ERouteFilter();
            ERouteFilter.Selects = ERouteSelect.ALL;
            ERouteFilter.Skip = ERoute_ERouteFilterDTO.Skip;
            ERouteFilter.Take = ERoute_ERouteFilterDTO.Take;
            ERouteFilter.OrderBy = ERoute_ERouteFilterDTO.OrderBy;
            ERouteFilter.OrderType = ERoute_ERouteFilterDTO.OrderType;

            ERouteFilter.Id = ERoute_ERouteFilterDTO.Id;
            ERouteFilter.Code = ERoute_ERouteFilterDTO.Code;
            ERouteFilter.Name = ERoute_ERouteFilterDTO.Name;
            ERouteFilter.OrganizationId = ERoute_ERouteFilterDTO.OrganizationId;
            ERouteFilter.AppUserId = ERoute_ERouteFilterDTO.AppUserId;
            ERouteFilter.StartDate = ERoute_ERouteFilterDTO.StartDate;
            ERouteFilter.EndDate = ERoute_ERouteFilterDTO.EndDate;
            ERouteFilter.RequestStateId = ERoute_ERouteFilterDTO.RequestStateId;
            ERouteFilter.ERouteTypeId = ERoute_ERouteFilterDTO.ERouteTypeId;
            ERouteFilter.StatusId = ERoute_ERouteFilterDTO.StatusId;
            ERouteFilter.CreatorId = ERoute_ERouteFilterDTO.CreatorId;
            ERouteFilter.StoreId = ERoute_ERouteFilterDTO.StoreId;
            ERouteFilter.CreatedAt = ERoute_ERouteFilterDTO.CreatedAt;
            ERouteFilter.UpdatedAt = ERoute_ERouteFilterDTO.UpdatedAt;
            return ERouteFilter;
        }
    }
}

