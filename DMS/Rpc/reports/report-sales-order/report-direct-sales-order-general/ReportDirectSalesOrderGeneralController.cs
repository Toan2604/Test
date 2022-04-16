using DMS.Common;
using DMS.DWModels;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MDistrict;
using DMS.Services.MOrganization;
using DMS.Services.MProvince;
using DMS.Services.MStore;
using DMS.Services.MStoreStatus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_direct_sales_order_general
{
    public class ReportDirectSalesOrderGeneralController : RpcController
    {
        private DataContext DataContext;
        private DWContext DWContext;
        private IAppUserService AppUserService;
        private IOrganizationService OrganizationService;
        private IStoreService StoreService;
        private IStoreStatusService StoreStatusService;
        private ICurrentContext CurrentContext;
        private IProvinceService ProvinceService;
        private IDistrictService DistrictService;
        public ReportDirectSalesOrderGeneralController(
            DataContext DataContext,
            DWContext DWContext,
            IAppUserService AppUserService,
            IOrganizationService OrganizationService,
            IStoreService StoreService,
            IStoreStatusService StoreStatusService,
            IProvinceService ProvinceService,
            IDistrictService DistrictService,
            ICurrentContext CurrentContext)
        {
            this.DataContext = DataContext;
            this.DWContext = DWContext;
            this.AppUserService = AppUserService;
            this.OrganizationService = OrganizationService;
            this.StoreService = StoreService;
            this.StoreStatusService = StoreStatusService;
            this.CurrentContext = CurrentContext;
            this.ProvinceService = ProvinceService;
            this.DistrictService = DistrictService;
        }

        [Route(ReportDirectSalesOrderGeneralRoute.FilterListAppUser), HttpPost]
        public async Task<List<ReportDirectSalesOrderGeneral_AppUserDTO>> FilterListAppUser([FromBody] ReportDirectSalesOrderGeneral_AppUserFilterDTO ReportDirectSalesOrderGeneral_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.Id = ReportDirectSalesOrderGeneral_AppUserFilterDTO.Id;
            AppUserFilter.Username = ReportDirectSalesOrderGeneral_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = ReportDirectSalesOrderGeneral_AppUserFilterDTO.DisplayName;
            AppUserFilter.OrganizationId = ReportDirectSalesOrderGeneral_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<ReportDirectSalesOrderGeneral_AppUserDTO> StoreCheckerReport_AppUserDTOs = AppUsers
                .Select(x => new ReportDirectSalesOrderGeneral_AppUserDTO(x)).ToList();
            return StoreCheckerReport_AppUserDTOs;
        }

        [Route(ReportDirectSalesOrderGeneralRoute.FilterListOrganization), HttpPost]
        public async Task<List<ReportDirectSalesOrderGeneral_OrganizationDTO>> FilterListOrganization()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

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
            List<ReportDirectSalesOrderGeneral_OrganizationDTO> StoreCheckerReport_OrganizationDTOs = Organizations
                .Select(x => new ReportDirectSalesOrderGeneral_OrganizationDTO(x)).ToList();
            return StoreCheckerReport_OrganizationDTOs;
        }

        [Route(ReportDirectSalesOrderGeneralRoute.FilterListStore), HttpPost]
        public async Task<List<ReportDirectSalesOrderGeneral_StoreDTO>> FilterListStore([FromBody] ReportDirectSalesOrderGeneral_StoreFilterDTO ReportDirectSalesOrderGeneral_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = ReportDirectSalesOrderGeneral_StoreFilterDTO.Id;
            StoreFilter.Code = ReportDirectSalesOrderGeneral_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ReportDirectSalesOrderGeneral_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ReportDirectSalesOrderGeneral_StoreFilterDTO.Name;
            StoreFilter.OrganizationId = ReportDirectSalesOrderGeneral_StoreFilterDTO.OrganizationId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<ReportDirectSalesOrderGeneral_StoreDTO> ReportDirectSalesOrderGeneral_StoreDTOs = Stores
                .Select(x => new ReportDirectSalesOrderGeneral_StoreDTO(x)).ToList();
            return ReportDirectSalesOrderGeneral_StoreDTOs;
        }

        [Route(ReportDirectSalesOrderGeneralRoute.FilterListStoreStatus), HttpPost]
        public async Task<List<ReportDirectSalesOrderGeneral_StoreStatusDTO>> FilterListStoreStatus([FromBody] ReportDirectSalesOrderGeneral_StoreStatusFilterDTO ReportDirectSalesOrderGeneral_StoreStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreStatusFilter StoreStatusFilter = new StoreStatusFilter();
            StoreStatusFilter.Skip = 0;
            StoreStatusFilter.Take = 20;
            StoreStatusFilter.OrderBy = StoreStatusOrder.Id;
            StoreStatusFilter.OrderType = OrderType.ASC;
            StoreStatusFilter.Selects = StoreStatusSelect.ALL;
            StoreStatusFilter.Id = ReportDirectSalesOrderGeneral_StoreStatusFilterDTO.Id;
            StoreStatusFilter.Code = ReportDirectSalesOrderGeneral_StoreStatusFilterDTO.Code;
            StoreStatusFilter.Name = ReportDirectSalesOrderGeneral_StoreStatusFilterDTO.Name;

            List<StoreStatus> StoreStatuses = await StoreStatusService.List(StoreStatusFilter);
            List<ReportDirectSalesOrderGeneral_StoreStatusDTO> ReportDirectSalesOrderGeneral_StoreStatusDTOs = StoreStatuses
                .Select(x => new ReportDirectSalesOrderGeneral_StoreStatusDTO(x)).ToList();
            return ReportDirectSalesOrderGeneral_StoreStatusDTOs;
        }

        [Route(ReportDirectSalesOrderGeneralRoute.FilterListProvince), HttpPost]
        public async Task<List<ReportDirectSalesOrderGeneral_ProvinceDTO>> FilterListProvince([FromBody] ReportDirectSalesOrderGeneral_ProvinceFilterDTO ReportDirectSalesOrderGeneral_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = ReportDirectSalesOrderGeneral_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = ReportDirectSalesOrderGeneral_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = ReportDirectSalesOrderGeneral_ProvinceFilterDTO.StatusId;

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<ReportDirectSalesOrderGeneral_ProvinceDTO> ReportDirectSalesOrderGeneral_ProvinceDTOs = Provinces
                .Select(x => new ReportDirectSalesOrderGeneral_ProvinceDTO(x)).ToList();
            return ReportDirectSalesOrderGeneral_ProvinceDTOs;
        }

        [Route(ReportDirectSalesOrderGeneralRoute.FilterListDistrict), HttpPost]
        public async Task<List<ReportDirectSalesOrderGeneral_DistrictDTO>> FilterListDistrict([FromBody] ReportDirectSalesOrderGeneral_DistrictFilterDTO ReportDirectSalesOrderGeneral_DistrictFilterDTO)
        {
            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = ReportDirectSalesOrderGeneral_DistrictFilterDTO.Id;
            DistrictFilter.Name = ReportDirectSalesOrderGeneral_DistrictFilterDTO.Name;
            DistrictFilter.Priority = ReportDirectSalesOrderGeneral_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = ReportDirectSalesOrderGeneral_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = ReportDirectSalesOrderGeneral_DistrictFilterDTO.StatusId;

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<ReportDirectSalesOrderGeneral_DistrictDTO> ReportDirectSalesOrderGeneral_DistrictDTOs = Districts
                .Select(x => new ReportDirectSalesOrderGeneral_DistrictDTO(x)).ToList();
            return ReportDirectSalesOrderGeneral_DistrictDTOs;
        }


        [Route(ReportDirectSalesOrderGeneralRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.HasValue == false)
                return 0;

            DateTime Start = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrderDate.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return 0;

            long? SaleEmployeeId = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.AppUserId?.Equal;
            long? BuyerStoreId = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.BuyerStoreId?.Equal;
            long? StoreStatusId = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.StoreStatusId?.Equal;
            bool? CreatedInCheckin = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.CreatedInCheckin;
            long? ProvinceId = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.DistrictId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();

            AppUserFilter AppUserFilter = new AppUserFilter
            {
                OrganizationId = new IdFilter { In = OrganizationIds },
                Id = new IdFilter { },
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.DisplayName | AppUserSelect.Organization | AppUserSelect.Username
            };
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            var AppUsers = await AppUserService.List(AppUserFilter);
            var AppUserIds = AppUsers.Select(x => x.Id).ToList();
            if (SaleEmployeeId != null)
                AppUserIds = AppUserIds.Intersect(new List<long> { SaleEmployeeId.Value }).ToList();

            var store_query = DWContext.Dim_Store.AsNoTracking();
            store_query = store_query.Where(x => x.DeletedAt == null);
            store_query = store_query.Where(x => x.StoreId, new IdFilter { Equal = BuyerStoreId });
            store_query = store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            store_query = store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            if (StoreStatusId != null && StoreStatusId != StoreStatusEnum.ALL.Id)
                store_query = store_query.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusId });
            List<long> BuyerStoreIds = await store_query.Select(x => x.StoreId).ToListWithNoLockAsync();

            var directsalesorder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            directsalesorder_query = directsalesorder_query.Where(x => x.BuyerStoreId, new IdFilter { In = BuyerStoreIds });
            directsalesorder_query = directsalesorder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            directsalesorder_query = directsalesorder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            directsalesorder_query = directsalesorder_query.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            if (CreatedInCheckin.HasValue)
                directsalesorder_query = directsalesorder_query.Where(x => x.StoreCheckingId.HasValue == CreatedInCheckin);
            directsalesorder_query = directsalesorder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long>
            {
                GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id
            }
            });

            int count = await directsalesorder_query.Distinct().CountWithNoLockAsync();
            return count;
        }

        [Route(ReportDirectSalesOrderGeneralRoute.List), HttpPost]
        public async Task<ActionResult<List<ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO>>> List([FromBody] ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.HasValue == false)
                return new List<ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO>();

            DateTime Start = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrderDate.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return BadRequest(new { message = "Chỉ được phép xem tối đa trong vòng 31 ngày" });

            List<ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO> ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTOs = await ListDataDW(ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO, Start, End);
            return ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTOs.Where(x => x.SalesOrders.Any()).ToList();
        }

        [Route(ReportDirectSalesOrderGeneralRoute.Total), HttpPost]
        public async Task<ReportDirectSalesOrderGeneral_TotalDTO> Total([FromBody] ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.HasValue == false)
                return new ReportDirectSalesOrderGeneral_TotalDTO();

            DateTime Start = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrderDate.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return new ReportDirectSalesOrderGeneral_TotalDTO();

            ReportDirectSalesOrderGeneral_TotalDTO ReportDirectSalesOrderGeneral_TotalDTO = await TotalDataDW(ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO, Start, End);
            return ReportDirectSalesOrderGeneral_TotalDTO;
        }

        [Route(ReportDirectSalesOrderGeneralRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrderDate.LessEqual.Value;

            ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.Skip = 0;
            ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.Take = int.MaxValue;
            List<ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO> ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTOs = await ListDataDW(ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO, Start, End);

            ReportDirectSalesOrderGeneral_TotalDTO ReportDirectSalesOrderGeneral_TotalDTO = await TotalDataDW(ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO, Start, End);
            long stt = 1;
            foreach (ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO in ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTOs)
            {
                foreach (var DirectSalesOrder in ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO.SalesOrders)
                {
                    DirectSalesOrder.STT = stt;
                    stt++;
                    DirectSalesOrder.eOrderDate = DirectSalesOrder.OrderDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
                    DirectSalesOrder.eCreatedAt = DirectSalesOrder.CreatedAt.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
                }
            }
            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            string path = "Templates/Report_Direct_Sales_Order_General.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.ReportSalesOrderGenerals = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTOs;
            Data.Total = ReportDirectSalesOrderGeneral_TotalDTO;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "ReportDirectSalesOrderGeneral.xlsx");
        }


        private async Task<List<ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO>> ListDataDW(
            ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO,
            DateTime Start, DateTime End)
        {
            long? SaleEmployeeId = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.AppUserId?.Equal;
            long? BuyerStoreId = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.BuyerStoreId?.Equal;
            long? StoreStatusId = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.StoreStatusId?.Equal;
            bool? CreatedInCheckin = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.CreatedInCheckin;
            long? ProvinceId = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.DistrictId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();

            AppUserFilter AppUserFilter = new AppUserFilter
            {
                OrganizationId = new IdFilter { In = OrganizationIds },
                Id = new IdFilter { },
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.DisplayName | AppUserSelect.Organization | AppUserSelect.Username
            };
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            var AppUsers = await AppUserService.List(AppUserFilter);
            var AppUserIds = AppUsers.Select(x => x.Id).ToList();
            if (SaleEmployeeId != null)
                AppUserIds = AppUserIds.Intersect(new List<long> { SaleEmployeeId.Value }).ToList();

            var store_query = DWContext.Dim_Store.AsNoTracking();
            store_query = store_query.Where(x => x.DeletedAt == null);
            store_query = store_query.Where(x => x.StoreId, new IdFilter { Equal = BuyerStoreId });
            store_query = store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            store_query = store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            if (StoreStatusId != null && StoreStatusId != StoreStatusEnum.ALL.Id)
                store_query = store_query.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusId });
            List<long> BuyerStoreIds = await store_query.Select(x => x.StoreId).ToListWithNoLockAsync();

            var directsalesorder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            directsalesorder_query = directsalesorder_query.Where(x => x.BuyerStoreId, new IdFilter { In = BuyerStoreIds });
            directsalesorder_query = directsalesorder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            directsalesorder_query = directsalesorder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            directsalesorder_query = directsalesorder_query.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            if (CreatedInCheckin.HasValue)
                directsalesorder_query = directsalesorder_query.Where(x => x.StoreCheckingId.HasValue == CreatedInCheckin);
            directsalesorder_query = directsalesorder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long>
            {
                GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id
            }
            });

            var DirectSalesOrderDAOs = await directsalesorder_query.Select(x => new DirectSalesOrderDAO
            {
                Id = x.DirectSalesOrderId,
                OrganizationId = x.OrganizationId,
                Code = x.Code,
                BuyerStoreId = x.BuyerStoreId,
                SaleEmployeeId = x.SaleEmployeeId,
                OrderDate = x.OrderDate,
                GeneralDiscountAmount = x.GeneralDiscountAmount,
                TotalTaxAmount = x.TotalTaxAmount,
                SubTotal = x.SubTotal,
                TotalAfterTax = x.TotalAfterTax,
                PromotionValue = x.PromotionValue,
                Total = x.Total,
                CreatedAt = x.CreatedAt,
                StoreCheckingId = x.StoreCheckingId,
            }).OrderBy(x => x.OrganizationId).ThenBy(x => x.OrderDate)
                .Skip(ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.Skip)
                .Take(ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.Take)
                .ToListWithNoLockAsync();
            var StoreIds = DirectSalesOrderDAOs.Select(x => x.BuyerStoreId).Distinct().ToList();
            var DbStoreQuery = DataContext.Store.AsNoTracking();
            List<Store> Stores = await DbStoreQuery
                .Where(x => x.Id, new IdFilter { In = StoreIds })
                .Select(x => new Store
                {
                    Id = x.Id,
                    Name = x.Name,
                    StoreStatusId = x.StoreStatusId,
                    Code = x.Code,
                    CodeDraft = x.CodeDraft,
                    StoreStatus = x.StoreStatus == null ? null : new StoreStatus
                    {
                        Id = x.StoreStatus.Id,
                        Code = x.StoreStatus.Code,
                        Name = x.StoreStatus.Name
                    },
                    Province = x.Province == null ? null : new Province
                    {
                        Id = x.Province.Id,
                        Code = x.Province.Code,
                        Name = x.Province.Name,
                    },
                    District = x.District == null ? null : new District
                    {
                        Id = x.District.Id,
                        Code = x.District.Code,
                        Name = x.District.Name,
                    }
                }).ToListWithNoLockAsync();
            var OrgIds = DirectSalesOrderDAOs.Select(x => x.OrganizationId).Distinct().ToList();
            var Orgs = OrganizationDAOs.Where(x => OrgIds.Contains(x.Id)).ToList();
            List<string> OrganizationNames = Orgs.Select(o => o.Name).Distinct().ToList();
            List<ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO> ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTOs = OrganizationNames.Select(on => new ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO
            {
                OrganizationName = on,
            }).ToList();
            foreach (var DirectSalesOrderDAO in DirectSalesOrderDAOs)
            {
                DirectSalesOrderDAO.SaleEmployee = AppUsers.Where(x => x.Id == DirectSalesOrderDAO.SaleEmployeeId)
                    .Select(x => new AppUserDAO
                    {
                        Id = x.Id,
                        DisplayName = x.DisplayName,
                        Username = x.Username,

                    }).FirstOrDefault();
                DirectSalesOrderDAO.BuyerStore = Stores.Where(x => x.Id == DirectSalesOrderDAO.BuyerStoreId)
                   .Select(x => new StoreDAO
                   {
                       Id = x.Id,
                       Code = x.Code,
                       CodeDraft = x.CodeDraft,
                       Name = x.Name,
                       StoreStatus = x.StoreStatus == null ? null : new StoreStatusDAO
                       {
                           Name = x.StoreStatus.Name
                       },
                       Province = x.Province == null ? null : new ProvinceDAO { Name = x.Province.Name },
                       District = x.District == null ? null : new DistrictDAO { Name = x.District.Name }
                       
                   }).FirstOrDefault();
            }
            foreach (ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO in ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTOs)
            {
                var Org = OrganizationDAOs.Where(x => x.Name == ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO.OrganizationName).FirstOrDefault();
                ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO.SalesOrders = DirectSalesOrderDAOs
                    .Where(x => x.OrganizationId == Org.Id)
                    .Select(x => new ReportDirectSalesOrderGeneral_DirectSalesOrderDTO
                    {
                        Id = x.Id,
                        Code = x.Code,
                        BuyerStoreCode = x.BuyerStore.Code,
                        BuyerStoreCodeDraft = x.BuyerStore.CodeDraft,
                        BuyerStoreName = x.BuyerStore.Name,
                        BuyerStoreStatusName = x.BuyerStore.StoreStatus.Name,
                        BuyerStoreProvinceName = x.BuyerStore.Province?.Name,
                        BuyerStoreDistrictName = x.BuyerStore.District?.Name,
                        SaleEmployeeName = x.SaleEmployee.DisplayName,
                        SaleEmployeeUsername = x.SaleEmployee.Username,
                        OrderDate = x.OrderDate,
                        CreatedAt = x.CreatedAt,
                        Discount = x.GeneralDiscountAmount ?? 0,
                        TaxValue = x.TotalTaxAmount,
                        SubTotal = x.SubTotal,
                        Total = x.TotalAfterTax,
                        PromotionValue = x.PromotionValue.GetValueOrDefault(0),
                        TotalAfterPromotion = x.Total,
                        StoreCheckingId = x.StoreCheckingId,
                        CreatedInCheckin = x.StoreCheckingId != null,
                        CreatedInCheckinString = x.StoreCheckingId != null ? "x" : "",
                    })
                    .ToList();
            }

            return ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTOs.Where(x => x.SalesOrders.Any()).ToList();
        }
        private async Task<ReportDirectSalesOrderGeneral_TotalDTO> TotalDataDW(
            ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO,
            DateTime Start, DateTime End)
        {
            long? SaleEmployeeId = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.AppUserId?.Equal;
            long? BuyerStoreId = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.BuyerStoreId?.Equal;
            long? StoreStatusId = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.StoreStatusId?.Equal;
            bool? CreatedInCheckin = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.CreatedInCheckin;
            long? ProvinceId = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.DistrictId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();

            AppUserFilter AppUserFilter = new AppUserFilter
            {
                OrganizationId = new IdFilter { In = OrganizationIds },
                Id = new IdFilter { },
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.DisplayName | AppUserSelect.Organization | AppUserSelect.Username
            };
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            var AppUsers = await AppUserService.List(AppUserFilter);
            var AppUserIds = AppUsers.Select(x => x.Id).ToList();
            if (SaleEmployeeId != null)
                AppUserIds = AppUserIds.Intersect(new List<long> { SaleEmployeeId.Value }).ToList();

            var store_query = DWContext.Dim_Store.AsNoTracking();
            store_query = store_query.Where(x => x.DeletedAt == null);
            store_query = store_query.Where(x => x.StoreId, new IdFilter { Equal = BuyerStoreId });
            store_query = store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            store_query = store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            if (StoreStatusId != null && StoreStatusId != StoreStatusEnum.ALL.Id)
                store_query = store_query.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusId });
            List<long> BuyerStoreIds = await store_query.Select(x => x.StoreId).ToListWithNoLockAsync();

            var directsalesorder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            directsalesorder_query = directsalesorder_query.Where(x => x.BuyerStoreId, new IdFilter { In = BuyerStoreIds });
            directsalesorder_query = directsalesorder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            directsalesorder_query = directsalesorder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            directsalesorder_query = directsalesorder_query.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            if (CreatedInCheckin.HasValue)
                directsalesorder_query = directsalesorder_query.Where(x => x.StoreCheckingId.HasValue == CreatedInCheckin);
            directsalesorder_query = directsalesorder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long>
            {
                GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id
            }
            });

            var DirectSalesOrderDAOs = await directsalesorder_query.Select(x => new DirectSalesOrderDAO
            {
                Id = x.DirectSalesOrderId,
                GeneralDiscountAmount = x.GeneralDiscountAmount,
                TotalTaxAmount = x.TotalTaxAmount,
                SubTotal = x.SubTotal,
                TotalAfterTax = x.TotalAfterTax,
                PromotionValue = x.PromotionValue,
                Total = x.Total,
            }).ToListWithNoLockAsync();

            ReportDirectSalesOrderGeneral_TotalDTO ReportDirectSalesOrderGeneral_TotalDTO = new ReportDirectSalesOrderGeneral_TotalDTO
            {
                TotalDiscount = DirectSalesOrderDAOs.Where(x => x.GeneralDiscountAmount.HasValue)
                .Select(x => x.GeneralDiscountAmount.Value)
                .DefaultIfEmpty(0).Sum(),
                TotalTax = DirectSalesOrderDAOs.Select(x => x.TotalTaxAmount).DefaultIfEmpty(0).Sum(),
                SubTotal = DirectSalesOrderDAOs.Select(x => x.SubTotal).DefaultIfEmpty(0).Sum(),
                Total = DirectSalesOrderDAOs.Select(x => x.TotalAfterTax).DefaultIfEmpty(0).Sum(),
                PromotionValue = DirectSalesOrderDAOs.Select(x => x.PromotionValue.GetValueOrDefault(0)).DefaultIfEmpty(0).Sum(),
                TotalAfterPromotion = DirectSalesOrderDAOs.Select(x => x.Total).DefaultIfEmpty(0).Sum(),
            };

            return ReportDirectSalesOrderGeneral_TotalDTO;
        }

    }
}
