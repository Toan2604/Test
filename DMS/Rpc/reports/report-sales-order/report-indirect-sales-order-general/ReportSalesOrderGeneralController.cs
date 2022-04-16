using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Services.MOrganization;
using Microsoft.AspNetCore.Mvc;
using DMS.Services.MStore;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreType;
using System;
using DMS.Helpers;
using Microsoft.EntityFrameworkCore;
using DMS.Services.MProduct;
using DMS.Services.MAppUser;
using System.IO;
using System.Dynamic;
using NGS.Templater;
using DMS.Services.MStoreStatus;
using Thinktecture.EntityFrameworkCore.TempTables;
using Thinktecture;
using TrueSight.Common;
using DMS.Services.MProvince;
using DMS.Services.MDistrict;
using DMS.DWModels;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_general
{
    public class ReportSalesOrderGeneralController : RpcController
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
        private IStoreTypeService StoreTypeService;
        public ReportSalesOrderGeneralController(
            DataContext DataContext,
            DWContext DWContext,
            IAppUserService AppUserService,
            IOrganizationService OrganizationService,
            IStoreService StoreService,
            IStoreStatusService StoreStatusService,
            IProvinceService ProvinceService,
            IDistrictService DistrictService,
            IStoreTypeService StoreTypeService,
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
            this.StoreTypeService = StoreTypeService;
        }

        [Route(ReportSalesOrderGeneralRoute.FilterListAppUser), HttpPost]
        public async Task<List<ReportSalesOrderGeneral_AppUserDTO>> FilterListAppUser([FromBody] ReportSalesOrderGeneral_AppUserFilterDTO ReportSalesOrderGeneral_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.Id = ReportSalesOrderGeneral_AppUserFilterDTO.Id;
            AppUserFilter.Username = ReportSalesOrderGeneral_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = ReportSalesOrderGeneral_AppUserFilterDTO.DisplayName;
            AppUserFilter.OrganizationId = ReportSalesOrderGeneral_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<ReportSalesOrderGeneral_AppUserDTO> StoreCheckerReport_AppUserDTOs = AppUsers
                .Select(x => new ReportSalesOrderGeneral_AppUserDTO(x)).ToList();
            return StoreCheckerReport_AppUserDTOs;
        }

        [Route(ReportSalesOrderGeneralRoute.FilterListOrganization), HttpPost]
        public async Task<List<ReportSalesOrderGeneral_OrganizationDTO>> FilterListOrganization()
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
            List<ReportSalesOrderGeneral_OrganizationDTO> StoreCheckerReport_OrganizationDTOs = Organizations
                .Select(x => new ReportSalesOrderGeneral_OrganizationDTO(x)).ToList();
            return StoreCheckerReport_OrganizationDTOs;
        }

        [Route(ReportSalesOrderGeneralRoute.FilterListStore), HttpPost]
        public async Task<List<ReportSalesOrderGeneral_StoreDTO>> FilterListStore([FromBody] ReportSalesOrderGeneral_StoreFilterDTO ReportSalesOrderGeneral_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = ReportSalesOrderGeneral_StoreFilterDTO.Id;
            StoreFilter.Code = ReportSalesOrderGeneral_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ReportSalesOrderGeneral_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ReportSalesOrderGeneral_StoreFilterDTO.Name;
            StoreFilter.OrganizationId = ReportSalesOrderGeneral_StoreFilterDTO.OrganizationId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<ReportSalesOrderGeneral_StoreDTO> ReportSalesOrderGeneral_StoreDTOs = Stores
                .Select(x => new ReportSalesOrderGeneral_StoreDTO(x)).ToList();
            return ReportSalesOrderGeneral_StoreDTOs;
        }

        [Route(ReportSalesOrderGeneralRoute.FilterListStoreStatus), HttpPost]
        public async Task<List<ReportSalesOrderGeneral_StoreStatusDTO>> FilterListStoreStatus([FromBody] ReportSalesOrderGeneral_StoreStatusFilterDTO ReportSalesOrderGeneral_StoreStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreStatusFilter StoreStatusFilter = new StoreStatusFilter();
            StoreStatusFilter.Skip = 0;
            StoreStatusFilter.Take = 20;
            StoreStatusFilter.OrderBy = StoreStatusOrder.Id;
            StoreStatusFilter.OrderType = OrderType.ASC;
            StoreStatusFilter.Selects = StoreStatusSelect.ALL;
            StoreStatusFilter.Id = ReportSalesOrderGeneral_StoreStatusFilterDTO.Id;
            StoreStatusFilter.Code = ReportSalesOrderGeneral_StoreStatusFilterDTO.Code;
            StoreStatusFilter.Name = ReportSalesOrderGeneral_StoreStatusFilterDTO.Name;

            List<StoreStatus> StoreStatuses = await StoreStatusService.List(StoreStatusFilter);
            List<ReportSalesOrderGeneral_StoreStatusDTO> ReportSalesOrderGeneral_StoreStatusDTOs = StoreStatuses
                .Select(x => new ReportSalesOrderGeneral_StoreStatusDTO(x)).ToList();
            return ReportSalesOrderGeneral_StoreStatusDTOs;
        }

        [Route(ReportSalesOrderGeneralRoute.FilterListStoreType), HttpPost]
        public async Task<List<ReportSalesOrderGeneral_StoreTypeDTO>> FilterListStoreType([FromBody] ReportSalesOrderGeneral_StoreTypeFilterDTO ReportSalesOrderGeneral_StoreTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = ReportSalesOrderGeneral_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = ReportSalesOrderGeneral_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = ReportSalesOrderGeneral_StoreTypeFilterDTO.Name;

            List<StoreType> StoreTypees = await StoreTypeService.List(StoreTypeFilter);
            List<ReportSalesOrderGeneral_StoreTypeDTO> ReportSalesOrderGeneral_StoreTypeDTOs = StoreTypees
                .Select(x => new ReportSalesOrderGeneral_StoreTypeDTO(x)).ToList();
            return ReportSalesOrderGeneral_StoreTypeDTOs;
        }

        [Route(ReportSalesOrderGeneralRoute.FilterListProvince), HttpPost]
        public async Task<List<ReportSalesOrderGeneral_ProvinceDTO>> FilterListProvince([FromBody] ReportSalesOrderGeneral_ProvinceFilterDTO ReportSalesOrderGeneral_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = ReportSalesOrderGeneral_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = ReportSalesOrderGeneral_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = ReportSalesOrderGeneral_ProvinceFilterDTO.StatusId;

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<ReportSalesOrderGeneral_ProvinceDTO> ReportSalesOrderGeneral_ProvinceDTOs = Provinces
                .Select(x => new ReportSalesOrderGeneral_ProvinceDTO(x)).ToList();
            return ReportSalesOrderGeneral_ProvinceDTOs;
        }

        [Route(ReportSalesOrderGeneralRoute.FilterListDistrict), HttpPost]
        public async Task<List<ReportSalesOrderGeneral_DistrictDTO>> FilterListDistrict([FromBody] ReportSalesOrderGeneral_DistrictFilterDTO ReportSalesOrderGeneral_DistrictFilterDTO)
        {
            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = ReportSalesOrderGeneral_DistrictFilterDTO.Id;
            DistrictFilter.Name = ReportSalesOrderGeneral_DistrictFilterDTO.Name;
            DistrictFilter.Priority = ReportSalesOrderGeneral_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = ReportSalesOrderGeneral_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = ReportSalesOrderGeneral_DistrictFilterDTO.StatusId;

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<ReportSalesOrderGeneral_DistrictDTO> ReportSalesOrderGeneral_DistrictDTOs = Districts
                .Select(x => new ReportSalesOrderGeneral_DistrictDTO(x)).ToList();
            return ReportSalesOrderGeneral_DistrictDTOs;
        }


        [Route(ReportSalesOrderGeneralRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.HasValue == false)
                return 0;

            DateTime Start = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrderDate.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return 0;

            long? SaleEmployeeId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.AppUserId?.Equal;
            long? BuyerStoreId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.BuyerStoreId?.Equal;
            long? SellerStoreId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.SellerStoreId?.Equal;
            long? StoreStatusId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.StoreStatusId?.Equal;
            long? StoreTypeId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.StoreTypeId?.Equal;
            bool? CreatedInCheckin = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.CreatedInCheckin;
            long? ProvinceId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.DistrictId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization
                .AsNoTracking()
                .Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id)))
                .ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
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

            List<long> StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            var StoreQuery = DataContext.Store.AsNoTracking();
            var store_query = StoreQuery.Where(x => x.Id, new IdFilter { In = StoreIds });
            store_query = store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            store_query = store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            //store_query = store_query.Where(x => x.StoreTypeId, new IdFilter { Equal = StoreTypeId });
            store_query = store_query.Where(x => x.DeletedAt == null);
            if (StoreStatusId.HasValue && StoreStatusId != StoreStatusEnum.ALL.Id) store_query = store_query.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusId.Value });
            if (SellerStoreId.HasValue)
                store_query = store_query.Where(x => x.Id, new IdFilter { Equal = SellerStoreId });
            var BuyerStoreIds = await store_query.Select(x => x.Id).ToListWithNoLockAsync();

            var DW_IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, new IdFilter { In = BuyerStoreIds });
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.BuyerStoreTypeId, new IdFilter { Equal = StoreTypeId });
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            if (SaleEmployeeId.HasValue)
                DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, new IdFilter { Equal = SaleEmployeeId });
            if (BuyerStoreId.HasValue)
                DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, new IdFilter { Equal = BuyerStoreId });
            if (SellerStoreId.HasValue)
                DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.SellerStoreId, new IdFilter { Equal = SellerStoreId });
            if (CreatedInCheckin.HasValue)
                DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.StoreCheckingId.HasValue == CreatedInCheckin);

            int count = await DW_IndirectSalesOrderQuery.Distinct().CountWithNoLockAsync();
            return count;
        }

        [Route(ReportSalesOrderGeneralRoute.List), HttpPost]
        public async Task<ActionResult<List<ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO>>> List([FromBody] ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.HasValue == false)
                return new List<ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO>();

            DateTime Start = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrderDate.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return BadRequest(new { message = "Chỉ được phép xem tối đa trong vòng 31 ngày" });

            List<ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO> ReportSalesOrderGeneral_ReportSalesOrderGeneralDTOs = await ListDataDW(ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO, Start, End);
            return ReportSalesOrderGeneral_ReportSalesOrderGeneralDTOs.Where(x => x.SalesOrders.Any()).ToList();
        }

        [Route(ReportSalesOrderGeneralRoute.Total), HttpPost]
        public async Task<ReportSalesOrderGeneral_TotalDTO> Total([FromBody] ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.HasValue == false)
                return new ReportSalesOrderGeneral_TotalDTO();

            DateTime Start = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrderDate.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return new ReportSalesOrderGeneral_TotalDTO();

            ReportSalesOrderGeneral_TotalDTO ReportSalesOrderGeneral_TotalDTO = await TotalDataDW(ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO, Start, End);
            return ReportSalesOrderGeneral_TotalDTO;
        }

        [Route(ReportSalesOrderGeneralRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrderDate.LessEqual.Value;

            ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.Skip = 0;
            ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.Take = int.MaxValue;
            List<ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO> ReportSalesOrderGeneral_ReportSalesOrderGeneralDTOs = await ListDataDW(ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO, Start, End);

            ReportSalesOrderGeneral_TotalDTO ReportSalesOrderGeneral_TotalDTO = await TotalDataDW(ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO, Start, End);
            long stt = 1;
            foreach (ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO in ReportSalesOrderGeneral_ReportSalesOrderGeneralDTOs)
            {
                foreach (var IndirectSalesOrder in ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO.SalesOrders)
                {
                    IndirectSalesOrder.STT = stt;
                    stt++;
                    IndirectSalesOrder.eOrderDate = IndirectSalesOrder.OrderDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
                    IndirectSalesOrder.eCreatedAt = IndirectSalesOrder.CreatedAt.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
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

            string path = "Templates/Report_Sales_Order_General.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.ReportSalesOrderGenerals = ReportSalesOrderGeneral_ReportSalesOrderGeneralDTOs;
            Data.Total = ReportSalesOrderGeneral_TotalDTO;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "ReportSalesOrderGeneral.xlsx");
        }

        private async Task<List<ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO>> ListDataDW(
            ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO,
            DateTime Start, DateTime End)
        {
            long? SaleEmployeeId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.AppUserId?.Equal;
            long? BuyerStoreId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.BuyerStoreId?.Equal;
            long? SellerStoreId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.SellerStoreId?.Equal;
            long? StoreStatusId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.StoreStatusId?.Equal;
            long? StoreTypeId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.StoreTypeId?.Equal;
            bool? CreatedInCheckin = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.CreatedInCheckin;
            long? ProvinceId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.DistrictId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization
                .AsNoTracking()
                .Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id)))
                .ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
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

            List<long> StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            var StoreQuery = DataContext.Store.AsNoTracking();
            var store_query = StoreQuery.Where(x => x.Id, new IdFilter { In = StoreIds });
            store_query = store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            store_query = store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            //store_query = store_query.Where(x => x.StoreTypeId, new IdFilter { Equal = StoreTypeId });
            store_query = store_query.Where(x => x.DeletedAt == null);
            if (StoreStatusId.HasValue && StoreStatusId != StoreStatusEnum.ALL.Id) store_query = store_query.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusId.Value });
            if (SellerStoreId.HasValue)
                store_query = store_query.Where(x => x.Id, new IdFilter { Equal = SellerStoreId });
            var BuyerStoreIds = await store_query.Select(x => x.Id).ToListWithNoLockAsync();


            var DW_IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, new IdFilter { In = BuyerStoreIds });
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.BuyerStoreTypeId, new IdFilter { Equal = StoreTypeId });
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            if (SaleEmployeeId.HasValue)
                DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, new IdFilter { Equal = SaleEmployeeId });
            if (BuyerStoreId.HasValue)
                DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, new IdFilter { Equal = BuyerStoreId });
            if (SellerStoreId.HasValue)
                DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.SellerStoreId, new IdFilter { Equal = SellerStoreId });
            if (CreatedInCheckin.HasValue)
                DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.StoreCheckingId.HasValue == CreatedInCheckin);

            var IndirectSalesOrderIds = await DW_IndirectSalesOrderQuery.Select(x => x.IndirectSalesOrderId).ToListWithNoLockAsync();
            var IndirectSalesOrderQuery = DataContext.IndirectSalesOrder.AsNoTracking().Where(x => x.Id, new IdFilter { In = IndirectSalesOrderIds });

            List<IndirectSalesOrderDAO> IndirectSalesOrderDAOs = await IndirectSalesOrderQuery
                .OrderBy(x => x.OrganizationId).ThenBy(x => x.OrderDate)
                .Skip(ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.Skip)
                .Take(ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.Take)                
                .ToListWithNoLockAsync();
            StoreIds = new List<long>();
            StoreIds.AddRange(IndirectSalesOrderDAOs.Select(x => x.SellerStoreId));
            StoreIds.AddRange(IndirectSalesOrderDAOs.Select(x => x.BuyerStoreId));
            StoreIds = StoreIds.Distinct().ToList();

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
                    StoreType = x.StoreType == null ? null : new StoreType
                    {
                        Id = x.StoreType.Id,    
                        Code = x.StoreType.Code,
                        Name = x.StoreType.Name,
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
            var OrgIds = IndirectSalesOrderDAOs.Select(x => x.OrganizationId).Distinct().ToList();
            var Orgs = OrganizationDAOs.Where(x => OrgIds.Contains(x.Id)).ToList();
            List<string> OrganizationNames = Orgs.Select(o => o.Name).Distinct().ToList();
            var StoreTypes = await StoreTypeService.List(new StoreTypeFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreTypeSelect.ALL
            });
            List<ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO> ReportSalesOrderGeneral_ReportSalesOrderGeneralDTOs = OrganizationNames.Select(on => new ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO
            {
                OrganizationName = on,
            }).ToList();
            foreach (var IndirectSalesOrderDAO in IndirectSalesOrderDAOs)
            {
                IndirectSalesOrderDAO.BuyerStoreType = StoreTypes.Where(x => x.Id == IndirectSalesOrderDAO.BuyerStoreTypeId)
                    .Select(x => new StoreTypeDAO
                    {
                        Name = x.Name,
                    }).FirstOrDefault();
                IndirectSalesOrderDAO.SaleEmployee = AppUsers.Where(x => x.Id == IndirectSalesOrderDAO.SaleEmployeeId)
                    .Select(x => new AppUserDAO
                    {
                        Id = x.Id,
                        DisplayName = x.DisplayName,
                        Username = x.Username,
                    }).FirstOrDefault();
                IndirectSalesOrderDAO.SellerStore = Stores.Where(x => x.Id == IndirectSalesOrderDAO.SellerStoreId)
                    .Select(x => new StoreDAO
                    {
                        Id = x.Id,
                        Code = x.Code,
                        CodeDraft = x.CodeDraft,
                        Name = x.Name,
                    }).FirstOrDefault();
                IndirectSalesOrderDAO.BuyerStore = Stores.Where(x => x.Id == IndirectSalesOrderDAO.BuyerStoreId)
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
                       StoreType = x.StoreType == null ? null : new StoreTypeDAO
                       {
                           Name = x.StoreType.Name
                       },
                       Province = x.Province == null ? null : new ProvinceDAO { Name = x.Province.Name },
                       District = x.District == null ? null : new DistrictDAO { Name = x.District.Name },
                   }).FirstOrDefault();
            }
            foreach (ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO in ReportSalesOrderGeneral_ReportSalesOrderGeneralDTOs)
            {
                var Org = OrganizationDAOs.Where(x => x.Name == ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO.OrganizationName).FirstOrDefault();
                ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO.SalesOrders = IndirectSalesOrderDAOs
                    .Where(x => x.OrganizationId == Org.Id)
                    .Select(x => new ReportSalesOrderGeneral_IndirectSalesOrderDTO
                    {
                        Id = x.Id,
                        Code = x.Code,
                        BuyerStoreCode = x.BuyerStore.Code,
                        BuyerStoreCodeDraft = x.BuyerStore.CodeDraft,
                        BuyerStoreName = x.BuyerStore.Name,
                        BuyerStoreStatusName = x.BuyerStore.StoreStatus.Name,
                        BuyerStoreProvinceName = x.BuyerStore.Province?.Name,
                        BuyerStoreDistrictName = x.BuyerStore.District?.Name,
                        BuyerStoreTypeName = x.BuyerStoreType.Name,
                        SellerStoreName = x.SellerStore.Name,
                        SaleEmployeeName = x.SaleEmployee.DisplayName,
                        SaleEmployeeUsername = x.SaleEmployee.Username,
                        OrderDate = x.OrderDate,
                        CreatedAt = x.CreatedAt,
                        Discount = x.GeneralDiscountAmount ?? 0,
                        SubTotal = x.SubTotal,
                        Total = x.Total,
                        StoreCheckingId = x.StoreCheckingId,
                        CreatedInCheckin = x.StoreCheckingId != null,
                        CreatedInCheckinString = x.StoreCheckingId != null ? "x" : "",
                    })
                    .ToList();
            }

            return ReportSalesOrderGeneral_ReportSalesOrderGeneralDTOs.Where(x => x.SalesOrders.Any()).ToList();
        }

        private async Task<ReportSalesOrderGeneral_TotalDTO> TotalDataDW(
            ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO,
            DateTime Start, DateTime End)
        {
            long? SaleEmployeeId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.AppUserId?.Equal;
            long? BuyerStoreId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.BuyerStoreId?.Equal;
            long? SellerStoreId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.SellerStoreId?.Equal;
            long? StoreStatusId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.StoreStatusId?.Equal;
            long? ProvinceId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.DistrictId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
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
            var StoreQuery = DataContext.Store.AsNoTracking();
            var store_query = StoreQuery.Where(x => x.Id, new IdFilter { In = StoreIds });
            store_query = store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            store_query = store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            store_query = store_query.Where(x => x.DeletedAt == null);
            if (StoreStatusId.HasValue && StoreStatusId != StoreStatusEnum.ALL.Id) store_query = store_query.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusId.Value });
            if (SellerStoreId.HasValue)
                store_query = store_query.Where(x => x.Id, new IdFilter { Equal = SellerStoreId });
            var BuyerStoreIds = await store_query.Select(x => x.Id).ToListWithNoLockAsync();

            var DW_IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, new IdFilter { In = BuyerStoreIds });
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            if (SaleEmployeeId.HasValue)
                DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, new IdFilter { Equal = SaleEmployeeId });
            if (BuyerStoreId.HasValue)
                DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, new IdFilter { Equal = BuyerStoreId });
            if (SellerStoreId.HasValue)
                DW_IndirectSalesOrderQuery = DW_IndirectSalesOrderQuery.Where(x => x.SellerStoreId, new IdFilter { Equal = SellerStoreId });

            var IndirectSalesOrderIds = await DW_IndirectSalesOrderQuery.Select(x => x.IndirectSalesOrderId).ToListWithNoLockAsync();
            var IndirectSalesOrderQuery = DataContext.IndirectSalesOrder.AsNoTracking().Where(x => x.Id, new IdFilter { In = IndirectSalesOrderIds });


            List<IndirectSalesOrderDAO> IndirectSalesOrderDAOs = await IndirectSalesOrderQuery.ToListWithNoLockAsync();
            ReportSalesOrderGeneral_TotalDTO ReportSalesOrderGeneral_TotalDTO = new ReportSalesOrderGeneral_TotalDTO
            {
                TotalDiscount = IndirectSalesOrderDAOs.Where(x => x.GeneralDiscountAmount.HasValue)
                .Select(x => x.GeneralDiscountAmount.Value)
                .DefaultIfEmpty(0).Sum(),
                SubTotal = IndirectSalesOrderDAOs.Select(x => x.SubTotal).DefaultIfEmpty(0).Sum(),
                TotalRevenue = IndirectSalesOrderDAOs.Select(x => x.Total).DefaultIfEmpty(0).Sum(),
            };

            return ReportSalesOrderGeneral_TotalDTO;
        }
    }
}
