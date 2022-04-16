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
using System.IO;
using System.Dynamic;
using NGS.Templater;
using OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers;
using DMS.Services.MStoreStatus;
using Thinktecture.EntityFrameworkCore.TempTables;
using Thinktecture;
using DMS.Services.MAppUser;
using DMS.Services.MProductGrouping;
using TrueSight.Common;
using DMS.Services.MProvince;
using DMS.Services.MDistrict;
using DMS.DWModels;

namespace DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_store_and_item
{
    public class ReportDirectSalesOrderByStoreAndItemController : RpcController
    {
        private DataContext DataContext;
        private DWContext DWContext;
        private IAppUserService AppUserService;
        private IItemService ItemService;
        private IOrganizationService OrganizationService;
        private IStoreService StoreService;
        private IStoreGroupingService StoreGroupingService;
        private IStoreTypeService StoreTypeService;
        private IStoreStatusService StoreStatusService;
        private IProductGroupingService ProductGroupingService;
        private ICurrentContext CurrentContext;
        private IProvinceService ProvinceService;
        private IDistrictService DistrictService;
        public ReportDirectSalesOrderByStoreAndItemController(
            DataContext DataContext,
            DWContext DWContext,
            IAppUserService AppUserService,
            IItemService ItemService,
            IOrganizationService OrganizationService,
            IStoreService StoreService,
            IStoreGroupingService StoreGroupingService,
            IStoreTypeService StoreTypeService,
            IStoreStatusService StoreStatusService,
            IProductGroupingService ProductGroupingService,
            IProvinceService ProvinceService,
            IDistrictService DistrictService,
            ICurrentContext CurrentContext)
        {
            this.DataContext = DataContext;
            this.DWContext = DWContext;
            this.AppUserService = AppUserService;
            this.ItemService = ItemService;
            this.OrganizationService = OrganizationService;
            this.StoreService = StoreService;
            this.StoreGroupingService = StoreGroupingService;
            this.StoreTypeService = StoreTypeService;
            this.StoreStatusService = StoreStatusService;
            this.ProductGroupingService = ProductGroupingService;
            this.CurrentContext = CurrentContext;
            this.ProvinceService = ProvinceService;
            this.DistrictService = DistrictService;
        }

        [Route(ReportDirectSalesOrderByStoreAndItemRoute.FilterListOrganization), HttpPost]
        public async Task<List<ReportDirectSalesOrderByStoreAndItem_OrganizationDTO>> FilterListOrganization()
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
            List<ReportDirectSalesOrderByStoreAndItem_OrganizationDTO> StoreCheckerReport_OrganizationDTOs = Organizations
                .Select(x => new ReportDirectSalesOrderByStoreAndItem_OrganizationDTO(x)).ToList();
            return StoreCheckerReport_OrganizationDTOs;
        }

        [Route(ReportDirectSalesOrderByStoreAndItemRoute.FilterListItem), HttpPost]
        public async Task<List<ReportDirectSalesOrderByStoreAndItem_ItemDTO>> FilterListItem([FromBody] ReportDirectSalesOrderByStoreAndItem_ItemFilterDTO ReportDirectSalesOrderByStoreAndItem_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Skip = 0;
            ItemFilter.Take = 20;
            ItemFilter.OrderBy = ItemOrder.Id;
            ItemFilter.OrderType = OrderType.ASC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.Id = ReportDirectSalesOrderByStoreAndItem_ItemFilterDTO.Id;
            ItemFilter.Code = ReportDirectSalesOrderByStoreAndItem_ItemFilterDTO.Code;
            ItemFilter.Name = ReportDirectSalesOrderByStoreAndItem_ItemFilterDTO.Name;
            ItemFilter.StatusId = ReportDirectSalesOrderByStoreAndItem_ItemFilterDTO.StatusId;
            ItemFilter.Search = ReportDirectSalesOrderByStoreAndItem_ItemFilterDTO.Search;

            List<Item> Items = await ItemService.List(ItemFilter);
            List<ReportDirectSalesOrderByStoreAndItem_ItemDTO> ReportDirectSalesOrderByStoreAndItem_ItemDTOs = Items
                .Select(x => new ReportDirectSalesOrderByStoreAndItem_ItemDTO(x)).ToList();
            return ReportDirectSalesOrderByStoreAndItem_ItemDTOs;
        }

        [Route(ReportDirectSalesOrderByStoreAndItemRoute.FilterListStore), HttpPost]
        public async Task<List<ReportDirectSalesOrderByStoreAndItem_StoreDTO>> FilterListStore([FromBody] ReportDirectSalesOrderByStoreAndItem_StoreFilterDTO ReportDirectSalesOrderByStoreAndItem_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = ReportDirectSalesOrderByStoreAndItem_StoreFilterDTO.Id;
            StoreFilter.Code = ReportDirectSalesOrderByStoreAndItem_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ReportDirectSalesOrderByStoreAndItem_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ReportDirectSalesOrderByStoreAndItem_StoreFilterDTO.Name;
            StoreFilter.OrganizationId = ReportDirectSalesOrderByStoreAndItem_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = ReportDirectSalesOrderByStoreAndItem_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = ReportDirectSalesOrderByStoreAndItem_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<ReportDirectSalesOrderByStoreAndItem_StoreDTO> ReportDirectSalesOrderByStoreAndItem_StoreDTOs = Stores
                .Select(x => new ReportDirectSalesOrderByStoreAndItem_StoreDTO(x)).ToList();
            return ReportDirectSalesOrderByStoreAndItem_StoreDTOs;
        }

        [Route(ReportDirectSalesOrderByStoreAndItemRoute.FilterListStoreGrouping), HttpPost]
        public async Task<List<ReportDirectSalesOrderByStoreAndItem_StoreGroupingDTO>> FilterListStoreGrouping([FromBody] ReportDirectSalesOrderByStoreAndItem_StoreGroupingFilterDTO ReportDirectSalesOrderByStoreAndItem_StoreGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = 99999;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Code = ReportDirectSalesOrderByStoreAndItem_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = ReportDirectSalesOrderByStoreAndItem_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.Level = ReportDirectSalesOrderByStoreAndItem_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.Path = ReportDirectSalesOrderByStoreAndItem_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreGroupingFilter.Id == null) StoreGroupingFilter.Id = new IdFilter();
            StoreGroupingFilter.Id.In = await FilterStoreGrouping(StoreGroupingService, CurrentContext);
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<ReportDirectSalesOrderByStoreAndItem_StoreGroupingDTO> ReportDirectSalesOrderByStoreAndItem_StoreGroupingDTOs = StoreGroupings
                .Select(x => new ReportDirectSalesOrderByStoreAndItem_StoreGroupingDTO(x)).ToList();
            return ReportDirectSalesOrderByStoreAndItem_StoreGroupingDTOs;
        }
        [Route(ReportDirectSalesOrderByStoreAndItemRoute.FilterListStoreType), HttpPost]
        public async Task<List<ReportDirectSalesOrderByStoreAndItem_StoreTypeDTO>> FilterListStoreType([FromBody] ReportDirectSalesOrderByStoreAndItem_StoreTypeFilterDTO ReportDirectSalesOrderByStoreAndItem_StoreTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = ReportDirectSalesOrderByStoreAndItem_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = ReportDirectSalesOrderByStoreAndItem_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = ReportDirectSalesOrderByStoreAndItem_StoreTypeFilterDTO.Name;
            StoreTypeFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreTypeFilter.Id == null) StoreTypeFilter.Id = new IdFilter();
            StoreTypeFilter.Id.In = await FilterStoreType(StoreTypeService, CurrentContext);
            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<ReportDirectSalesOrderByStoreAndItem_StoreTypeDTO> ReportDirectSalesOrderByStoreAndItem_StoreTypeDTOs = StoreTypes
                .Select(x => new ReportDirectSalesOrderByStoreAndItem_StoreTypeDTO(x)).ToList();
            return ReportDirectSalesOrderByStoreAndItem_StoreTypeDTOs;
        }

        [Route(ReportDirectSalesOrderByStoreAndItemRoute.FilterListStoreStatus), HttpPost]
        public async Task<List<ReportDirectSalesOrderByStoreAndItem_StoreStatusDTO>> FilterListStoreStatus([FromBody] ReportDirectSalesOrderByStoreAndItem_StoreStatusFilterDTO ReportDirectSalesOrderByStoreAndItem_StoreStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreStatusFilter StoreStatusFilter = new StoreStatusFilter();
            StoreStatusFilter.Skip = 0;
            StoreStatusFilter.Take = 20;
            StoreStatusFilter.OrderBy = StoreStatusOrder.Id;
            StoreStatusFilter.OrderType = OrderType.ASC;
            StoreStatusFilter.Selects = StoreStatusSelect.ALL;
            StoreStatusFilter.Id = ReportDirectSalesOrderByStoreAndItem_StoreStatusFilterDTO.Id;
            StoreStatusFilter.Code = ReportDirectSalesOrderByStoreAndItem_StoreStatusFilterDTO.Code;
            StoreStatusFilter.Name = ReportDirectSalesOrderByStoreAndItem_StoreStatusFilterDTO.Name;

            List<StoreStatus> StoreStatuses = await StoreStatusService.List(StoreStatusFilter);
            List<ReportDirectSalesOrderByStoreAndItem_StoreStatusDTO> ReportDirectSalesOrderByStoreAndItem_StoreStatusDTOs = StoreStatuses
                .Select(x => new ReportDirectSalesOrderByStoreAndItem_StoreStatusDTO(x)).ToList();
            return ReportDirectSalesOrderByStoreAndItem_StoreStatusDTOs;
        }

        [Route(ReportDirectSalesOrderByStoreAndItemRoute.FilterListProductGrouping), HttpPost]
        public async Task<List<ReportDirectSalesOrderByStoreAndItem_ProductGroupingDTO>> FilterListProductGrouping([FromBody] ReportDirectSalesOrderByStoreAndItem_ProductGroupingFilterDTO ReportDirectSalesOrderByStoreAndItem_ProductGroupingFilterDTO)
        {
            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter();
            ProductGroupingFilter.Skip = 0;
            ProductGroupingFilter.Take = int.MaxValue;
            ProductGroupingFilter.OrderBy = ProductGroupingOrder.Id;
            ProductGroupingFilter.OrderType = OrderType.ASC;
            ProductGroupingFilter.Selects = ProductGroupingSelect.Id | ProductGroupingSelect.Code
                | ProductGroupingSelect.Name | ProductGroupingSelect.Parent;

            List<ProductGrouping> ProductGroupings = await ProductGroupingService.List(ProductGroupingFilter);
            List<ReportDirectSalesOrderByStoreAndItem_ProductGroupingDTO> ReportDirectSalesOrderByStoreAndItem_ProductGroupingDTOs = ProductGroupings
                .Select(x => new ReportDirectSalesOrderByStoreAndItem_ProductGroupingDTO(x)).ToList();
            return ReportDirectSalesOrderByStoreAndItem_ProductGroupingDTOs;
        }

        [Route(ReportDirectSalesOrderByStoreAndItemRoute.FilterListProvince), HttpPost]
        public async Task<List<ReportDirectSalesOrderByStoreAndItem_ProvinceDTO>> FilterListProvince([FromBody] ReportDirectSalesOrderByStoreAndItem_ProvinceFilterDTO ReportDirectSalesOrderByStoreAndItem_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = ReportDirectSalesOrderByStoreAndItem_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = ReportDirectSalesOrderByStoreAndItem_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = ReportDirectSalesOrderByStoreAndItem_ProvinceFilterDTO.StatusId;

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<ReportDirectSalesOrderByStoreAndItem_ProvinceDTO> ReportDirectSalesOrderByStoreAndItem_ProvinceDTOs = Provinces
                .Select(x => new ReportDirectSalesOrderByStoreAndItem_ProvinceDTO(x)).ToList();
            return ReportDirectSalesOrderByStoreAndItem_ProvinceDTOs;
        }

        [Route(ReportDirectSalesOrderByStoreAndItemRoute.FilterListDistrict), HttpPost]
        public async Task<List<ReportDirectSalesOrderByStoreAndItem_DistrictDTO>> FilterListDistrict([FromBody] ReportDirectSalesOrderByStoreAndItem_DistrictFilterDTO ReportDirectSalesOrderByStoreAndItem_DistrictFilterDTO)
        {
            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = ReportDirectSalesOrderByStoreAndItem_DistrictFilterDTO.Id;
            DistrictFilter.Name = ReportDirectSalesOrderByStoreAndItem_DistrictFilterDTO.Name;
            DistrictFilter.Priority = ReportDirectSalesOrderByStoreAndItem_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = ReportDirectSalesOrderByStoreAndItem_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = ReportDirectSalesOrderByStoreAndItem_DistrictFilterDTO.StatusId;

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<ReportDirectSalesOrderByStoreAndItem_DistrictDTO> ReportDirectSalesOrderByStoreAndItem_DistrictDTOs = Districts
                .Select(x => new ReportDirectSalesOrderByStoreAndItem_DistrictDTO(x)).ToList();
            return ReportDirectSalesOrderByStoreAndItem_DistrictDTOs;
        }


        [Route(ReportDirectSalesOrderByStoreAndItemRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.HasValue == false)
                return 0;

            DateTime Start = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrderDate.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return 0;

            long? StoreId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreId?.Equal;
            long? StoreTypeId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreTypeId?.Equal;
            long? StoreGroupingId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreGroupingId?.Equal;
            long? StoreStatusId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreStatusId?.Equal;
            List<long> ItemIds = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.ItemId?.In;
            long? ProductGroupingId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.ProductGroupingId?.Equal;
            long? ProvinceId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.DistrictId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
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
            if (StoreId.HasValue)
            {
                var listId = new List<long> { StoreId.Value };
                StoreIds = StoreIds.Intersect(listId).ToList();
            }
            List<long> StoreTypeIds = await FilterStoreType(StoreTypeService, CurrentContext);
            if (StoreTypeId.HasValue)
            {
                var listId = new List<long> { StoreTypeId.Value };
                StoreTypeIds = StoreTypeIds.Intersect(listId).ToList();
            }
            List<long> StoreGroupingIds = await FilterStoreGrouping(StoreGroupingService, CurrentContext);
            if (StoreGroupingId.HasValue)
            {
                var listId = new List<long> { StoreGroupingId.Value };
                StoreGroupingIds = StoreGroupingIds.Intersect(listId).ToList();
            }

            if (ProductGroupingId.HasValue)
            {
                var ItemDAOs = await ItemService.List(new ItemFilter
                {
                    ProductGroupingId = new IdFilter { Equal = ProductGroupingId.Value },
                    Selects = ItemSelect.Id
                });
                if (ItemIds != null)
                {
                    ItemIds = ItemIds.Union(ItemDAOs.Select(x => x.Id).ToList())
                            .Distinct()
                            .ToList();
                }
                else
                {
                    ItemIds = ItemDAOs.Select(x => x.Id).ToList();
                }
            }

            var StoreGrouping_query = DataContext.StoreStoreGroupingMapping.AsNoTracking();
            List<long> StoreHasGroupingIds = await StoreGrouping_query.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();
            StoreGrouping_query = StoreGrouping_query.Where(x => x.StoreGroupingId, new IdFilter { In = StoreGroupingIds });
            List<long> StoreInGroupingFilterIds = await StoreGrouping_query.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();

            var StoreNoGrouping_query = DWContext.Dim_Store.AsNoTracking();
            StoreNoGrouping_query = StoreNoGrouping_query.Where(x => x.StoreId, new IdFilter { NotIn = StoreHasGroupingIds });
            var StoreNoGroupingIds = await StoreNoGrouping_query.Select(x => x.StoreId).ToListWithNoLockAsync();
            StoreInGroupingFilterIds.AddRange(StoreNoGroupingIds);
            StoreIds = StoreIds.Intersect(StoreInGroupingFilterIds).ToList();

            var Store_query = DWContext.Dim_Store.AsNoTracking();
            Store_query = Store_query.Where(x => x.DeletedAt == null);
            Store_query = Store_query.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            Store_query = Store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            Store_query = Store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            Store_query = Store_query.Where(x => x.StoreTypeId, new IdFilter { In = StoreTypeIds });
            if (StoreStatusId != null && StoreStatusId != StoreStatusEnum.ALL.Id)
                Store_query = Store_query.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusId });
            Store_query = Store_query.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            StoreIds = await Store_query.Select(x => x.StoreId).ToListWithNoLockAsync();

            var DirectSalesOrder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long>{
                    GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });

            var DirectSalesOrderIds = await DirectSalesOrder_query.Select(x => x.DirectSalesOrderId).Distinct().ToListWithNoLockAsync();

            var Transaction_query = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
            Transaction_query = Transaction_query.Where(x => x.DeletedAt == null);
            Transaction_query = Transaction_query.Where(x => x.ItemId, new IdFilter { In = ItemIds });
            Transaction_query = Transaction_query.Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds });
        
            int count = await Transaction_query.Distinct().CountWithNoLockAsync();
            return count;
        }

        [Route(ReportDirectSalesOrderByStoreAndItemRoute.List), HttpPost]
        public async Task<ActionResult<List<ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO>>> List([FromBody] ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.HasValue == false)
                return new List<ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO>();

            DateTime Start = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrderDate.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return BadRequest(new { message = "Chỉ được phép xem tối đa trong vòng 31 ngày" });

            List<ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO> ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs = await ListDataDW(ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO, Start, End);
            return ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs;
        }

        [Route(ReportDirectSalesOrderByStoreAndItemRoute.Total), HttpPost]
        public async Task<ReportDirectSalesOrderByStoreAndItem_TotalDTO> Total([FromBody] ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.HasValue == false)
                return new ReportDirectSalesOrderByStoreAndItem_TotalDTO();

            DateTime Start = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrderDate.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return new ReportDirectSalesOrderByStoreAndItem_TotalDTO();

            ReportDirectSalesOrderByStoreAndItem_TotalDTO ReportDirectSalesOrderByStoreAndItem_TotalDTO = await TotalDataDW(ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO, Start, End);
            return ReportDirectSalesOrderByStoreAndItem_TotalDTO;
        }

        [Route(ReportDirectSalesOrderByStoreAndItemRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrderDate.LessEqual.Value;

            ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.Skip = 0;
            ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.Take = int.MaxValue;
            List<ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO> ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs = await ListDataDW(ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO, Start, End);

            ReportDirectSalesOrderByStoreAndItem_TotalDTO ReportDirectSalesOrderByStoreAndItem_TotalDTO = await TotalDataDW(ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO, Start, End);
            long stt = 1;
            foreach (ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO in ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs)
            {
                foreach (var Store in ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO.Stores)
                {
                    Store.STT = stt;
                    stt++;
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

            string path = "Templates/Report_Direct_Sales_Order_By_Store_And_Item.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.ReportSalesOrderByStoreAndItems = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs;
            Data.Total = ReportDirectSalesOrderByStoreAndItem_TotalDTO;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "ReportDirectSalesOrderByStoreAndItem.xlsx");
        }
        private async Task<List<ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO>> ListDataDW(
            ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO,
            DateTime Start, DateTime End)
        {
            long? StoreId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreId?.Equal;
            long? StoreTypeId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreTypeId?.Equal;
            long? StoreGroupingId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreGroupingId?.Equal;
            long? StoreStatusId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreStatusId?.Equal;
            List<long> ItemIds = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.ItemId?.In;
            long? ProductGroupingId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.ProductGroupingId?.Equal;
            long? ProvinceId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.DistrictId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
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
            if (StoreId.HasValue)
            {
                var listId = new List<long> { StoreId.Value };
                StoreIds = StoreIds.Intersect(listId).ToList();
            }
            List<long> StoreTypeIds = await FilterStoreType(StoreTypeService, CurrentContext);
            if (StoreTypeId.HasValue)
            {
                var listId = new List<long> { StoreTypeId.Value };
                StoreTypeIds = StoreTypeIds.Intersect(listId).ToList();
            }
            List<long> StoreGroupingIds = await FilterStoreGrouping(StoreGroupingService, CurrentContext);
            if (StoreGroupingId.HasValue)
            {
                var listId = new List<long> { StoreGroupingId.Value };
                StoreGroupingIds = StoreGroupingIds.Intersect(listId).ToList();
            }

            if (ProductGroupingId.HasValue)
            {
                var ItemDAOs = await ItemService.List(new ItemFilter
                {
                    ProductGroupingId = new IdFilter { Equal = ProductGroupingId.Value },
                    Selects = ItemSelect.Id
                });
                if (ItemIds != null)
                {
                    ItemIds = ItemIds.Union(ItemDAOs.Select(x => x.Id).ToList())
                            .Distinct()
                            .ToList();
                }
                else
                {
                    ItemIds = ItemDAOs.Select(x => x.Id).ToList();
                }
            }

            var StoreGrouping_query = DataContext.StoreStoreGroupingMapping.AsNoTracking();
            List<long> StoreHasGroupingIds = await StoreGrouping_query.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();
            StoreGrouping_query = StoreGrouping_query.Where(x => x.StoreGroupingId, new IdFilter { In = StoreGroupingIds });
            List<long> StoreInGroupingFilterIds = await StoreGrouping_query.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();

            var StoreNoGrouping_query = DWContext.Dim_Store.AsNoTracking();
            StoreNoGrouping_query = StoreNoGrouping_query.Where(x => x.StoreId, new IdFilter { NotIn = StoreHasGroupingIds });
            var StoreNoGroupingIds = await StoreNoGrouping_query.Select(x => x.StoreId).ToListWithNoLockAsync();
            StoreInGroupingFilterIds.AddRange(StoreNoGroupingIds);
            StoreIds = StoreIds.Intersect(StoreInGroupingFilterIds).ToList();

            var Store_query = DWContext.Dim_Store.AsNoTracking();
            Store_query = Store_query.Where(x => x.DeletedAt == null);
            Store_query = Store_query.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            Store_query = Store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            Store_query = Store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            Store_query = Store_query.Where(x => x.StoreTypeId, new IdFilter { In = StoreTypeIds });
            if (StoreStatusId != null && StoreStatusId != StoreStatusEnum.ALL.Id)
                Store_query = Store_query.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusId });
            Store_query = Store_query.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            StoreIds = await Store_query.Select(x => x.StoreId).ToListWithNoLockAsync();

            var DirectSalesOrder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long>{
                    GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });

            var DirectSalesOrderIds = await DirectSalesOrder_query.Select(x => x.DirectSalesOrderId).Distinct().ToListWithNoLockAsync();

            var Transaction_query = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
            Transaction_query = Transaction_query.Where(x => x.DeletedAt == null);
            Transaction_query = Transaction_query.Where(x => x.ItemId, new IdFilter { In = ItemIds });
            Transaction_query = Transaction_query.Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds });
            List<long> BuyerStoreIds = await Transaction_query.Select(x => x.BuyerStoreId).Distinct().ToListWithNoLockAsync();

            var Buyerstore_query = DataContext.Store.AsNoTracking();
            Buyerstore_query = Buyerstore_query.Where(x => x.Id, new IdFilter { In = BuyerStoreIds });

            List<Store> BuyerStores = await Buyerstore_query.Select(s => new Store
            {
                Id = s.Id,
                Code = s.Code,
                CodeDraft = s.CodeDraft,
                Name = s.Name,
                Address = s.Address,
                OrganizationId = s.OrganizationId,
                StoreStatusId = s.StoreStatusId,
                ProvinceId = s.ProvinceId,
                DistrictId = s.DistrictId,
                Province = s.Province == null ? null : new Province
                {
                    Id = s.Province.Id,
                    Code = s.Province.Code,
                    Name = s.Province.Name,
                },
                District = s.District == null ? null : new District
                {
                    Id = s.District.Id,
                    Code = s.District.Code,
                    Name = s.District.Name,
                }
            }).OrderBy(x => x.OrganizationId).ThenBy(x => x.Name)
                .Skip(ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.Skip)
                .Take(ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.Take)
                .ToListWithNoLockAsync();

            var StoreOrganizationIds = BuyerStores.Select(x => x.OrganizationId).Distinct().ToList();
            var StoreOrganizationDAOs = await DataContext.Organization.Where(x => StoreOrganizationIds.Contains(x.Id)).ToListWithNoLockAsync();

            List<ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO> ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs = StoreOrganizationDAOs.Select(on => new ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO
            {
                OrganizationId = on.Id,
                OrganizationName = on.Name,
            }).ToList();
            foreach (ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO in ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs)
            {
                ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO.Stores = BuyerStores
                    .Where(x => x.OrganizationId == ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO.OrganizationId)
                    .Select(x => new ReportDirectSalesOrderByStoreAndItem_StoreDTO(x))
                    .ToList();
            }

            StoreIds = BuyerStores.Select(s => s.Id).ToList();

            Transaction_query = Transaction_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });

            DirectSalesOrderIds = await Transaction_query.Select(x => x.DirectSalesOrderId).ToListWithNoLockAsync();
            List<DirectSalesOrderDAO> DirectSalesOrderDAOs = await DWContext.Fact_DirectSalesOrder.AsNoTracking()
                .Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds })
                .Select(x => new DirectSalesOrderDAO
                {
                    Id = x.DirectSalesOrderId,
                    BuyerStoreId = x.BuyerStoreId
                }).ToListWithNoLockAsync();
            List<DirectSalesOrderContentDAO> DirectSalesOrderContentDAOs = await DataContext.DirectSalesOrderContent
                 .Where(x => DirectSalesOrderIds.Contains(x.DirectSalesOrderId) &&
                (ItemIds == null || ItemIds.Count == 0 || ItemIds.Contains(x.ItemId)))
                .Select(x => new DirectSalesOrderContentDAO
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    DiscountAmount = x.DiscountAmount,
                    GeneralDiscountAmount = x.GeneralDiscountAmount,
                    PrimaryPrice = x.PrimaryPrice,
                    RequestedQuantity = x.RequestedQuantity,
                    SalePrice = x.SalePrice,
                    TaxAmount = x.TaxAmount,
                    DirectSalesOrderId = x.DirectSalesOrderId,
                    ItemId = x.ItemId,
                    PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    DirectSalesOrder = x.DirectSalesOrder == null ? null : new DirectSalesOrderDAO
                    {
                        BuyerStoreId = x.DirectSalesOrder.BuyerStoreId,
                    }
                })
                .ToListWithNoLockAsync();
            List<DirectSalesOrderPromotionDAO> DirectSalesOrderPromotionDAOs = await DataContext.DirectSalesOrderPromotion
                 .Where(x => DirectSalesOrderIds.Contains(x.DirectSalesOrderId) &&
                (ItemIds == null || ItemIds.Count == 0 || ItemIds.Contains(x.ItemId)))
                .Select(x => new DirectSalesOrderPromotionDAO
                {
                    Id = x.Id,
                    RequestedQuantity = x.RequestedQuantity,
                    DirectSalesOrderId = x.DirectSalesOrderId,
                    ItemId = x.ItemId,
                    PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    DirectSalesOrder = x.DirectSalesOrder == null ? null : new DirectSalesOrderDAO
                    {
                        BuyerStoreId = x.DirectSalesOrder.BuyerStoreId,
                    }
                })
                .ToListWithNoLockAsync();
            ItemIds = new List<long>();
            ItemIds.AddRange(DirectSalesOrderContentDAOs.Select(x => x.ItemId));
            ItemIds.AddRange(DirectSalesOrderPromotionDAOs.Select(x => x.ItemId));
            ItemIds = ItemIds.Distinct().ToList();

            var Items = await DataContext.Item.Where(x => ItemIds.Contains(x.Id))
                .Where(x => x.DeletedAt == null)
                .Select(x => new Item
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    SalePrice = x.SalePrice,
                }).ToListWithNoLockAsync();

            List<UnitOfMeasureDAO> UnitOfMeasureDAOs = await DataContext.UnitOfMeasure.ToListWithNoLockAsync();
            // khởi tạo khung dữ liệu
            foreach (ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO in ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs)
            {
                foreach (var Store in ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO.Stores)
                {
                    var SalesOrderIds = DirectSalesOrderDAOs.Where(x => x.BuyerStoreId == Store.Id).Select(x => x.Id).ToList();
                    Store.StoreStatus = StoreStatusEnum.StoreStatusEnumList.Where(x => x.Id == Store.StoreStatusId).Select(x => new ReportDirectSalesOrderByStoreAndItem_StoreStatusDTO
                    {
                        Name = x.Name
                    }).FirstOrDefault();
                    if (Store.Items == null)
                        Store.Items = new List<ReportDirectSalesOrderByStoreAndItem_ItemDTO>();
                    foreach (DirectSalesOrderContentDAO DirectSalesOrderContentDAO in DirectSalesOrderContentDAOs)
                    {
                        if (SalesOrderIds.Contains(DirectSalesOrderContentDAO.DirectSalesOrderId))
                        {
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO ReportDirectSalesOrderByStoreAndItem_ItemDTO = Store.Items.Where(i => i.Id == DirectSalesOrderContentDAO.ItemId).FirstOrDefault();
                            if (ReportDirectSalesOrderByStoreAndItem_ItemDTO == null)
                            {
                                var item = Items.Where(x => x.Id == DirectSalesOrderContentDAO.ItemId).FirstOrDefault();
                                if (item == null)
                                    continue;
                                var UOMName = UnitOfMeasureDAOs.Where(x => x.Id == DirectSalesOrderContentDAO.PrimaryUnitOfMeasureId).Select(x => x.Name).FirstOrDefault();
                                ReportDirectSalesOrderByStoreAndItem_ItemDTO = new ReportDirectSalesOrderByStoreAndItem_ItemDTO
                                {
                                    Id = item.Id,
                                    Code = item.Code,
                                    Name = item.Name,
                                    UnitOfMeasureName = UOMName,
                                    DirectSalesOrderIds = new HashSet<long>(),
                                };
                                Store.Items.Add(ReportDirectSalesOrderByStoreAndItem_ItemDTO);
                            }
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO.DirectSalesOrderIds.Add(DirectSalesOrderContentDAO.DirectSalesOrderId);
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO.SaleStock += DirectSalesOrderContentDAO.RequestedQuantity;
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO.SalePriceAverage += (DirectSalesOrderContentDAO.SalePrice * DirectSalesOrderContentDAO.RequestedQuantity);
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO.Revenue += (DirectSalesOrderContentDAO.Amount - (DirectSalesOrderContentDAO.GeneralDiscountAmount ?? 0) + (DirectSalesOrderContentDAO.TaxAmount ?? 0));
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO.Discount += ((DirectSalesOrderContentDAO.DiscountAmount ?? 0) + (DirectSalesOrderContentDAO.GeneralDiscountAmount ?? 0));
                        }
                    }

                    foreach (var item in Store.Items)
                    {
                        item.SalePriceAverage = item.SalePriceAverage / item.SaleStock;
                    }

                    foreach (DirectSalesOrderPromotionDAO DirectSalesOrderPromotionDAO in DirectSalesOrderPromotionDAOs)
                    {
                        if (SalesOrderIds.Contains(DirectSalesOrderPromotionDAO.DirectSalesOrderId))
                        {
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO ReportDirectSalesOrderByStoreAndItem_ItemDTO = Store.Items.Where(i => i.Id == DirectSalesOrderPromotionDAO.ItemId).FirstOrDefault();
                            if (ReportDirectSalesOrderByStoreAndItem_ItemDTO == null)
                            {
                                var item = Items.Where(x => x.Id == DirectSalesOrderPromotionDAO.ItemId).FirstOrDefault();
                                if (item == null)
                                    continue;
                                var UOMName = UnitOfMeasureDAOs.Where(x => x.Id == DirectSalesOrderPromotionDAO.PrimaryUnitOfMeasureId).Select(x => x.Name).FirstOrDefault();
                                ReportDirectSalesOrderByStoreAndItem_ItemDTO = new ReportDirectSalesOrderByStoreAndItem_ItemDTO
                                {
                                    Id = item.Id,
                                    Code = item.Code,
                                    Name = item.Name,
                                    UnitOfMeasureName = UOMName,
                                    DirectSalesOrderIds = new HashSet<long>(),
                                };
                                Store.Items.Add(ReportDirectSalesOrderByStoreAndItem_ItemDTO);
                            }
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO.DirectSalesOrderIds.Add(DirectSalesOrderPromotionDAO.DirectSalesOrderId);
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO.PromotionStock += DirectSalesOrderPromotionDAO.RequestedQuantity;
                        }
                    }
                }
            }

            //làm tròn số
            foreach (var ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO in ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs)
            {
                foreach (var Store in ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO.Stores)
                {
                    foreach (var item in Store.Items)
                    {
                        item.Revenue = Math.Round(item.Revenue, 0);
                        item.Discount = Math.Round(item.Discount, 0);
                        item.SalePriceAverage = Math.Round(item.SalePriceAverage, 0);
                    }
                }
            }

            return ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs;
        }
        private async Task<ReportDirectSalesOrderByStoreAndItem_TotalDTO> TotalDataDW(
            ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO,
            DateTime Start, DateTime End)
        {
            long? StoreId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreId?.Equal;
            long? StoreTypeId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreTypeId?.Equal;
            long? StoreGroupingId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreGroupingId?.Equal;
            long? StoreStatusId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreStatusId?.Equal;
            List<long> ItemIds = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.ItemId?.In;
            long? ProductGroupingId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.ProductGroupingId?.Equal;
            long? ProvinceId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.DistrictId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
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
            if (StoreId.HasValue)
            {
                var listId = new List<long> { StoreId.Value };
                StoreIds = StoreIds.Intersect(listId).ToList();
            }
            List<long> StoreTypeIds = await FilterStoreType(StoreTypeService, CurrentContext);
            if (StoreTypeId.HasValue)
            {
                var listId = new List<long> { StoreTypeId.Value };
                StoreTypeIds = StoreTypeIds.Intersect(listId).ToList();
            }
            List<long> StoreGroupingIds = await FilterStoreGrouping(StoreGroupingService, CurrentContext);
            if (StoreGroupingId.HasValue)
            {
                var listId = new List<long> { StoreGroupingId.Value };
                StoreGroupingIds = StoreGroupingIds.Intersect(listId).ToList();
            }

            if (ProductGroupingId.HasValue)
            {
                var ItemDAOs = await ItemService.List(new ItemFilter
                {
                    ProductGroupingId = new IdFilter { Equal = ProductGroupingId.Value },
                    Selects = ItemSelect.Id
                });
                if (ItemIds != null)
                {
                    ItemIds = ItemIds.Union(ItemDAOs.Select(x => x.Id).ToList())
                            .Distinct()
                            .ToList();
                }
                else
                {
                    ItemIds = ItemDAOs.Select(x => x.Id).ToList();
                }
            }

            var StoreGrouping_query = DataContext.StoreStoreGroupingMapping.AsNoTracking();
            List<long> StoreHasGroupingIds = await StoreGrouping_query.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();
            StoreGrouping_query = StoreGrouping_query.Where(x => x.StoreGroupingId, new IdFilter { In = StoreGroupingIds });
            List<long> StoreInGroupingFilterIds = await StoreGrouping_query.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();

            var StoreNoGrouping_query = DWContext.Dim_Store.AsNoTracking();
            StoreNoGrouping_query = StoreNoGrouping_query.Where(x => x.StoreId, new IdFilter { NotIn = StoreHasGroupingIds });
            var StoreNoGroupingIds = await StoreNoGrouping_query.Select(x => x.StoreId).ToListWithNoLockAsync();
            StoreInGroupingFilterIds.AddRange(StoreNoGroupingIds);
            StoreIds = StoreIds.Intersect(StoreInGroupingFilterIds).ToList();

            var Store_query = DWContext.Dim_Store.AsNoTracking();
            Store_query = Store_query.Where(x => x.DeletedAt == null);
            Store_query = Store_query.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            Store_query = Store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            Store_query = Store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            Store_query = Store_query.Where(x => x.StoreTypeId, new IdFilter { In = StoreTypeIds });
            if (StoreStatusId != null && StoreStatusId != StoreStatusEnum.ALL.Id)
                Store_query = Store_query.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusId });
            Store_query = Store_query.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            StoreIds = await Store_query.Select(x => x.StoreId).ToListWithNoLockAsync();

            var DirectSalesOrder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long>{
                    GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });

            var DirectSalesOrderIds = await DirectSalesOrder_query.Select(x => x.DirectSalesOrderId).Distinct().ToListWithNoLockAsync();

            var DirectSalesOrderTransactionQuery = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DeletedAt == null);
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.ItemId, new IdFilter { In = ItemIds });
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds });
            if (ItemIds != null && ItemIds.Count > 0)
            {
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.ItemId, new IdFilter { In = ItemIds });
            }

            var DirectSalesOrderContentQuery = DirectSalesOrderTransactionQuery.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id });

            var DirectSalesOrderPromotionQuery = DirectSalesOrderTransactionQuery.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.PROMOTION.Id });

            ReportDirectSalesOrderByStoreAndItem_TotalDTO ReportDirectSalesOrderByStoreAndItem_TotalDTO = new ReportDirectSalesOrderByStoreAndItem_TotalDTO();

            ReportDirectSalesOrderByStoreAndItem_TotalDTO.TotalSalesStock = DirectSalesOrderContentQuery.Select(x => x.RequestedQuantity ?? 0).Sum();

            ReportDirectSalesOrderByStoreAndItem_TotalDTO.TotalPromotionStock = DirectSalesOrderPromotionQuery.Select(x => x.RequestedQuantity ?? 0).Sum();

            ReportDirectSalesOrderByStoreAndItem_TotalDTO.TotalRevenue = Math.Round(DirectSalesOrderContentQuery.Select(x => x.Amount).Sum()
                - DirectSalesOrderContentQuery.Where(x => x.GeneralDiscountAmount.HasValue).Select(x => x.GeneralDiscountAmount.Value).Sum()
                + DirectSalesOrderContentQuery.Where(x => x.TaxAmount.HasValue).Select(x => x.TaxAmount.Value).Sum(), 0);

            ReportDirectSalesOrderByStoreAndItem_TotalDTO.TotalDiscount = Math.Round(DirectSalesOrderContentQuery.Where(x => x.GeneralDiscountAmount.HasValue).Select(x => x.GeneralDiscountAmount.Value).Sum()
                + DirectSalesOrderContentQuery.Where(x => x.DiscountAmount.HasValue).Select(x => x.DiscountAmount.Value).Sum(), 0);
            return ReportDirectSalesOrderByStoreAndItem_TotalDTO;
        }
        private async Task<List<ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO>> ListData(
            ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO,
            DateTime Start, DateTime End)
        {
            long? StoreId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreId?.Equal;
            long? StoreTypeId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreTypeId?.Equal;
            long? StoreGroupingId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreGroupingId?.Equal;
            long? StoreStatusId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreStatusId?.Equal;
            List<long> ItemIds = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.ItemId?.In;
            long? ProductGroupingId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.ProductGroupingId?.Equal;
            long? ProvinceId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.DistrictId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
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
            if (StoreId.HasValue)
            {
                var listId = new List<long> { StoreId.Value };
                StoreIds = StoreIds.Intersect(listId).ToList();
            }
            List<long> StoreTypeIds = await FilterStoreType(StoreTypeService, CurrentContext);
            if (StoreTypeId.HasValue)
            {
                var listId = new List<long> { StoreTypeId.Value };
                StoreTypeIds = StoreTypeIds.Intersect(listId).ToList();
            }
            List<long> StoreGroupingIds = await FilterStoreGrouping(StoreGroupingService, CurrentContext);
            if (StoreGroupingId.HasValue)
            {
                var listId = new List<long> { StoreGroupingId.Value };
                StoreGroupingIds = StoreGroupingIds.Intersect(listId).ToList();
            }

            if (ProductGroupingId.HasValue)
            {
                var ItemDAOs = await ItemService.List(new ItemFilter
                {
                    ProductGroupingId = new IdFilter { Equal = ProductGroupingId.Value },
                    Selects = ItemSelect.Id
                });
                if (ItemIds != null)
                {
                    ItemIds = ItemIds.Union(ItemDAOs.Select(x => x.Id).ToList())
                            .Distinct()
                            .ToList();
                }
                else
                {
                    ItemIds = ItemDAOs.Select(x => x.Id).ToList();
                }
            }

            var Store_query = DataContext.Store.AsNoTracking();
            Store_query = Store_query.Where(x => x.Id, new IdFilter { In = StoreIds });
            Store_query = Store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            Store_query = Store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            StoreIds = await Store_query.Select(x => x.Id).ToListWithNoLockAsync();

            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
                        .BulkInsertValuesIntoTempTableAsync<long>(StoreIds);

            var orderQuery = from i in DataContext.DirectSalesOrder
                             join s in DataContext.Store on i.BuyerStoreId equals s.Id
                             join tt in tempTableQuery.Query on i.BuyerStoreId equals tt.Column1
                             join sg in DataContext.StoreStoreGroupingMapping on s.Id equals sg.StoreId into subquery
                             from q in subquery.DefaultIfEmpty()
                             where i.OrderDate >= Start && i.OrderDate <= End &&
                             AppUserIds.Contains(i.SaleEmployeeId) &&
                             (StoreTypeIds.Contains(s.StoreTypeId)) &&
                             StoreTypeIds.Contains(s.StoreTypeId) &&
                             (
                                (
                                    StoreGroupingId.HasValue == false &&
                                    (q == null || StoreGroupingIds.Contains(q.StoreGroupingId))
                                ) ||
                                (
                                    StoreGroupingId.HasValue && StoreGroupingId.Value == q.StoreGroupingId
                                )
                             ) &&
                             (StoreStatusId.HasValue == false || StoreStatusId.Value == StoreStatusEnum.ALL.Id || s.StoreStatusId == StoreStatusId.Value) &&
                             (OrganizationIds.Contains(s.OrganizationId)) &&
                             (i.GeneralApprovalStateId == GeneralApprovalStateEnum.APPROVED.Id || i.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_APPROVED.Id) &&
                             s.DeletedAt == null
                             select i.Id;

            var Ids = await orderQuery.Distinct().ToListWithNoLockAsync();
            ITempTableQuery<TempTable<long>> tempTableQuery2 = await DataContext
                       .BulkInsertValuesIntoTempTableAsync<long>(Ids);

            var transaction_query = DataContext.DirectSalesOrderTransaction.AsNoTracking();
            transaction_query = transaction_query.Where(x => x.ItemId, new IdFilter { In = ItemIds });
            transaction_query = transaction_query.Where(x => x.DirectSalesOrderId, new IdFilter { In = Ids });
            List<long> BuyerStoreIds = await transaction_query.Select(x => x.BuyerStoreId).Distinct().ToListWithNoLockAsync();

            var buyerstore_query = DataContext.Store.AsNoTracking();
            buyerstore_query = buyerstore_query.Where(x => x.Id, new IdFilter { In = BuyerStoreIds });

            List<Store> BuyerStores = await buyerstore_query.Select(s => new Store
            {
                Id = s.Id,
                Code = s.Code,
                CodeDraft = s.CodeDraft,
                Name = s.Name,
                Address = s.Address,
                OrganizationId = s.OrganizationId,
                StoreStatusId = s.StoreStatusId,
                ProvinceId = s.ProvinceId,
                DistrictId = s.DistrictId,
                Province = s.Province == null ? null : new Province
                {
                    Id = s.Province.Id,
                    Code = s.Province.Code,
                    Name = s.Province.Name,
                },
                District = s.District == null ? null : new District
                {
                    Id = s.District.Id,
                    Code = s.District.Code,
                    Name = s.District.Name,
                }
            }).OrderBy(x => x.OrganizationId).ThenBy(x => x.Name)
                .Skip(ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.Skip)
                .Take(ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.Take)
                .ToListWithNoLockAsync();

            var StoreOrganizationIds = BuyerStores.Select(x => x.OrganizationId).Distinct().ToList();
            var StoreOrganizationDAOs = await DataContext.Organization.Where(x => StoreOrganizationIds.Contains(x.Id)).ToListWithNoLockAsync();

            List<ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO> ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs = StoreOrganizationDAOs.Select(on => new ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO
            {
                OrganizationId = on.Id,
                OrganizationName = on.Name,
            }).ToList();
            foreach (ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO in ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs)
            {
                ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO.Stores = BuyerStores
                    .Where(x => x.OrganizationId == ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO.OrganizationId)
                    .Select(x => new ReportDirectSalesOrderByStoreAndItem_StoreDTO(x))
                    .ToList();
            }

            StoreIds = BuyerStores.Select(s => s.Id).ToList();
            var transactionQuery2 = from t in DataContext.DirectSalesOrderTransaction
                                    join tt in tempTableQuery2.Query on t.DirectSalesOrderId equals tt.Column1
                                    where StoreIds.Contains(t.BuyerStoreId) &&
                                    (StoreId.HasValue == false || t.BuyerStoreId == StoreId.Value) &&
                                    OrganizationIds.Contains(t.OrganizationId) &&
                                    (ItemIds == null || ItemIds.Count == 0 || ItemIds.Contains(t.ItemId))
                                    select t.DirectSalesOrderId;

            var DirectSalesOrderIds = await transactionQuery2.ToListWithNoLockAsync();
            List<DirectSalesOrderDAO> DirectSalesOrderDAOs = await DataContext.DirectSalesOrder
                .Where(x => DirectSalesOrderIds.Contains(x.Id))
                .Select(x => new DirectSalesOrderDAO
                {
                    Id = x.Id,
                    BuyerStoreId = x.BuyerStoreId
                }).ToListWithNoLockAsync();
            List<DirectSalesOrderContentDAO> DirectSalesOrderContentDAOs = await DataContext.DirectSalesOrderContent
                 .Where(x => DirectSalesOrderIds.Contains(x.DirectSalesOrderId) &&
                (ItemIds == null || ItemIds.Count == 0 || ItemIds.Contains(x.ItemId)))
                .Select(x => new DirectSalesOrderContentDAO
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    DiscountAmount = x.DiscountAmount,
                    GeneralDiscountAmount = x.GeneralDiscountAmount,
                    PrimaryPrice = x.PrimaryPrice,
                    RequestedQuantity = x.RequestedQuantity,
                    SalePrice = x.SalePrice,
                    TaxAmount = x.TaxAmount,
                    DirectSalesOrderId = x.DirectSalesOrderId,
                    ItemId = x.ItemId,
                    PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    DirectSalesOrder = x.DirectSalesOrder == null ? null : new DirectSalesOrderDAO
                    {
                        BuyerStoreId = x.DirectSalesOrder.BuyerStoreId,
                    }
                })
                .ToListWithNoLockAsync();
            List<DirectSalesOrderPromotionDAO> DirectSalesOrderPromotionDAOs = await DataContext.DirectSalesOrderPromotion
                 .Where(x => DirectSalesOrderIds.Contains(x.DirectSalesOrderId) &&
                (ItemIds == null || ItemIds.Count == 0 || ItemIds.Contains(x.ItemId)))
                .Select(x => new DirectSalesOrderPromotionDAO
                {
                    Id = x.Id,
                    RequestedQuantity = x.RequestedQuantity,
                    DirectSalesOrderId = x.DirectSalesOrderId,
                    ItemId = x.ItemId,
                    PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    DirectSalesOrder = x.DirectSalesOrder == null ? null : new DirectSalesOrderDAO
                    {
                        BuyerStoreId = x.DirectSalesOrder.BuyerStoreId,
                    }
                })
                .ToListWithNoLockAsync();
            ItemIds = new List<long>();
            ItemIds.AddRange(DirectSalesOrderContentDAOs.Select(x => x.ItemId));
            ItemIds.AddRange(DirectSalesOrderPromotionDAOs.Select(x => x.ItemId));
            ItemIds = ItemIds.Distinct().ToList();

            var Items = await DataContext.Item.Where(x => ItemIds.Contains(x.Id))
                .Where(x => x.DeletedAt == null)
                .Select(x => new Item
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    SalePrice = x.SalePrice,
                }).ToListWithNoLockAsync();

            List<UnitOfMeasureDAO> UnitOfMeasureDAOs = await DataContext.UnitOfMeasure.ToListWithNoLockAsync();
            // khởi tạo khung dữ liệu
            foreach (ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO in ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs)
            {
                foreach (var Store in ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO.Stores)
                {
                    var SalesOrderIds = DirectSalesOrderDAOs.Where(x => x.BuyerStoreId == Store.Id).Select(x => x.Id).ToList();
                    Store.StoreStatus = StoreStatusEnum.StoreStatusEnumList.Where(x => x.Id == Store.StoreStatusId).Select(x => new ReportDirectSalesOrderByStoreAndItem_StoreStatusDTO
                    {
                        Name = x.Name
                    }).FirstOrDefault();
                    if (Store.Items == null)
                        Store.Items = new List<ReportDirectSalesOrderByStoreAndItem_ItemDTO>();
                    foreach (DirectSalesOrderContentDAO DirectSalesOrderContentDAO in DirectSalesOrderContentDAOs)
                    {
                        if (SalesOrderIds.Contains(DirectSalesOrderContentDAO.DirectSalesOrderId))
                        {
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO ReportDirectSalesOrderByStoreAndItem_ItemDTO = Store.Items.Where(i => i.Id == DirectSalesOrderContentDAO.ItemId).FirstOrDefault();
                            if (ReportDirectSalesOrderByStoreAndItem_ItemDTO == null)
                            {
                                var item = Items.Where(x => x.Id == DirectSalesOrderContentDAO.ItemId).FirstOrDefault();
                                if (item == null)
                                    continue;
                                var UOMName = UnitOfMeasureDAOs.Where(x => x.Id == DirectSalesOrderContentDAO.PrimaryUnitOfMeasureId).Select(x => x.Name).FirstOrDefault();
                                ReportDirectSalesOrderByStoreAndItem_ItemDTO = new ReportDirectSalesOrderByStoreAndItem_ItemDTO
                                {
                                    Id = item.Id,
                                    Code = item.Code,
                                    Name = item.Name,
                                    UnitOfMeasureName = UOMName,
                                    DirectSalesOrderIds = new HashSet<long>(),
                                };
                                Store.Items.Add(ReportDirectSalesOrderByStoreAndItem_ItemDTO);
                            }
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO.DirectSalesOrderIds.Add(DirectSalesOrderContentDAO.DirectSalesOrderId);
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO.SaleStock += DirectSalesOrderContentDAO.RequestedQuantity;
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO.SalePriceAverage += (DirectSalesOrderContentDAO.SalePrice * DirectSalesOrderContentDAO.RequestedQuantity);
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO.Revenue += (DirectSalesOrderContentDAO.Amount - (DirectSalesOrderContentDAO.GeneralDiscountAmount ?? 0) + (DirectSalesOrderContentDAO.TaxAmount ?? 0));
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO.Discount += ((DirectSalesOrderContentDAO.DiscountAmount ?? 0) + (DirectSalesOrderContentDAO.GeneralDiscountAmount ?? 0));
                        }
                    }

                    foreach (var item in Store.Items)
                    {
                        item.SalePriceAverage = item.SalePriceAverage / item.SaleStock;
                    }

                    foreach (DirectSalesOrderPromotionDAO DirectSalesOrderPromotionDAO in DirectSalesOrderPromotionDAOs)
                    {
                        if (SalesOrderIds.Contains(DirectSalesOrderPromotionDAO.DirectSalesOrderId))
                        {
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO ReportDirectSalesOrderByStoreAndItem_ItemDTO = Store.Items.Where(i => i.Id == DirectSalesOrderPromotionDAO.ItemId).FirstOrDefault();
                            if (ReportDirectSalesOrderByStoreAndItem_ItemDTO == null)
                            {
                                var item = Items.Where(x => x.Id == DirectSalesOrderPromotionDAO.ItemId).FirstOrDefault();
                                if (item == null)
                                    continue;
                                var UOMName = UnitOfMeasureDAOs.Where(x => x.Id == DirectSalesOrderPromotionDAO.PrimaryUnitOfMeasureId).Select(x => x.Name).FirstOrDefault();
                                ReportDirectSalesOrderByStoreAndItem_ItemDTO = new ReportDirectSalesOrderByStoreAndItem_ItemDTO
                                {
                                    Id = item.Id,
                                    Code = item.Code,
                                    Name = item.Name,
                                    UnitOfMeasureName = UOMName,
                                    DirectSalesOrderIds = new HashSet<long>(),
                                };
                                Store.Items.Add(ReportDirectSalesOrderByStoreAndItem_ItemDTO);
                            }
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO.DirectSalesOrderIds.Add(DirectSalesOrderPromotionDAO.DirectSalesOrderId);
                            ReportDirectSalesOrderByStoreAndItem_ItemDTO.PromotionStock += DirectSalesOrderPromotionDAO.RequestedQuantity;
                        }
                    }
                }
            }

            //làm tròn số
            foreach (var ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO in ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs)
            {
                foreach (var Store in ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO.Stores)
                {
                    foreach (var item in Store.Items)
                    {
                        item.Revenue = Math.Round(item.Revenue, 0);
                        item.Discount = Math.Round(item.Discount, 0);
                        item.SalePriceAverage = Math.Round(item.SalePriceAverage, 0);
                    }
                }
            }

            return ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs;
        }

        private async Task<ReportDirectSalesOrderByStoreAndItem_TotalDTO> TotalData(
            ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO,
            DateTime Start, DateTime End)
        {
            long? StoreId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreId?.Equal;
            long? StoreTypeId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreTypeId?.Equal;
            long? StoreGroupingId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreGroupingId?.Equal;
            long? StoreStatusId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreStatusId?.Equal;
            List<long> ItemIds = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.ItemId?.In;
            long? ProvinceId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.DistrictId?.Equal;

            ReportDirectSalesOrderByStoreAndItem_TotalDTO ReportDirectSalesOrderByStoreAndItem_TotalDTO = new ReportDirectSalesOrderByStoreAndItem_TotalDTO();
            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
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
            if (StoreId.HasValue)
            {
                var listId = new List<long> { StoreId.Value };
                StoreIds = StoreIds.Intersect(listId).ToList();
            }
            List<long> StoreTypeIds = await FilterStoreType(StoreTypeService, CurrentContext);
            if (StoreTypeId.HasValue)
            {
                var listId = new List<long> { StoreTypeId.Value };
                StoreTypeIds = StoreTypeIds.Intersect(listId).ToList();
            }
            List<long> StoreGroupingIds = await FilterStoreGrouping(StoreGroupingService, CurrentContext);
            if (StoreGroupingId.HasValue)
            {
                var listId = new List<long> { StoreGroupingId.Value };
                StoreGroupingIds = StoreGroupingIds.Intersect(listId).ToList();
            }

            var Store_query = DataContext.Store.AsNoTracking();
            Store_query = Store_query.Where(x => x.Id, new IdFilter { In = StoreIds });
            Store_query = Store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            Store_query = Store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            StoreIds = await Store_query.Select(x => x.Id).ToListWithNoLockAsync();
            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
                       .BulkInsertValuesIntoTempTableAsync<long>(StoreIds);

            var orderQuery = from i in DataContext.DirectSalesOrder
                             join s in DataContext.Store on i.BuyerStoreId equals s.Id
                             join tt in tempTableQuery.Query on i.BuyerStoreId equals tt.Column1
                             join sg in DataContext.StoreStoreGroupingMapping on s.Id equals sg.StoreId into subquery
                             from q in subquery.DefaultIfEmpty()
                             where i.OrderDate >= Start && i.OrderDate <= End &&
                             AppUserIds.Contains(i.SaleEmployeeId) &&
                             StoreTypeIds.Contains(s.StoreTypeId) &&
                             (
                                (
                                    StoreGroupingId.HasValue == false &&
                                    (q == null || StoreGroupingIds.Contains(q.StoreGroupingId))
                                ) ||
                                (
                                    StoreGroupingId.HasValue && StoreGroupingId.Value == q.StoreGroupingId
                                )
                             ) &&
                             (StoreStatusId.HasValue == false || StoreStatusId.Value == StoreStatusEnum.ALL.Id || s.StoreStatusId == StoreStatusId.Value) &&
                             (OrganizationIds.Contains(s.OrganizationId)) &&
                             (i.GeneralApprovalStateId == GeneralApprovalStateEnum.APPROVED.Id || i.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_APPROVED.Id) &&
                             s.DeletedAt == null
                             select i.Id;

            var Ids = await orderQuery.Distinct().ToListWithNoLockAsync();
            ITempTableQuery<TempTable<long>> tempTableQuery2 = await DataContext
                      .BulkInsertValuesIntoTempTableAsync<long>(Ids);

            var transactionQuery = from t in DataContext.DirectSalesOrderTransaction
                                   join tt in tempTableQuery2.Query on t.DirectSalesOrderId equals tt.Column1
                                   join s in DataContext.Store on t.BuyerStoreId equals s.Id
                                   where (ItemIds == null || ItemIds.Count == 0 || ItemIds.Contains(t.ItemId))
                                   select new Store
                                   {
                                       Id = s.Id,
                                       Code = s.Code,
                                       CodeDraft = s.CodeDraft,
                                       Name = s.Name,
                                       Address = s.Address,
                                       OrganizationId = s.OrganizationId,
                                       StoreStatusId = s.StoreStatusId
                                   };

            List<Store> Stores = await transactionQuery.Distinct().OrderBy(x => x.OrganizationId).ThenBy(x => x.Name)
                .ToListWithNoLockAsync();

            var StoreOrganizationIds = Stores.Select(x => x.OrganizationId).Distinct().ToList();
            var StoreOrganizationDAOs = await DataContext.Organization.Where(x => OrganizationIds.Contains(x.Id)).ToListWithNoLockAsync();

            List<ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO> ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs = StoreOrganizationDAOs.Select(on => new ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO
            {
                OrganizationId = on.Id,
                OrganizationName = on.Name,
            }).ToList();
            foreach (ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO in ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTOs)
            {
                ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO.Stores = Stores
                    .Where(x => x.OrganizationId == ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO.OrganizationId)
                    .Select(x => new ReportDirectSalesOrderByStoreAndItem_StoreDTO
                    {
                        Id = x.Id,
                        Code = x.Code,
                        CodeDraft = x.CodeDraft,
                        Name = x.Name,
                    })
                    .ToList();
            }

            StoreIds = Stores.Select(s => s.Id).ToList();
            var DirectSalesOrderContentQuery = DataContext.DirectSalesOrderContent
                .Where(x => StoreIds.Contains(x.DirectSalesOrder.BuyerStoreId) &&
                AppUserIds.Contains(x.DirectSalesOrder.SaleEmployeeId) &&
                OrganizationIds.Contains(x.DirectSalesOrder.OrganizationId) &&
                (ItemIds == null || ItemIds.Count == 0 || ItemIds.Contains(x.ItemId)) &&
                Start <= x.DirectSalesOrder.OrderDate && x.DirectSalesOrder.OrderDate <= End &&
                (x.DirectSalesOrder.GeneralApprovalStateId == GeneralApprovalStateEnum.APPROVED.Id || x.DirectSalesOrder.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_APPROVED.Id));
            var DirectSalesOrderPromotionQuery = DataContext.DirectSalesOrderPromotion
                .Where(x => StoreIds.Contains(x.DirectSalesOrder.BuyerStoreId) &&
                AppUserIds.Contains(x.DirectSalesOrder.SaleEmployeeId) &&
                OrganizationIds.Contains(x.DirectSalesOrder.OrganizationId) &&
                (ItemIds == null || ItemIds.Count == 0 || ItemIds.Contains(x.ItemId)) &&
                Start <= x.DirectSalesOrder.OrderDate && x.DirectSalesOrder.OrderDate <= End &&
                (x.DirectSalesOrder.GeneralApprovalStateId == GeneralApprovalStateEnum.APPROVED.Id || x.DirectSalesOrder.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_APPROVED.Id));

            ReportDirectSalesOrderByStoreAndItem_TotalDTO.TotalSalesStock = DirectSalesOrderContentQuery.Select(x => x.RequestedQuantity).Sum();

            ReportDirectSalesOrderByStoreAndItem_TotalDTO.TotalPromotionStock = DirectSalesOrderPromotionQuery.Select(x => x.RequestedQuantity).Sum();

            ReportDirectSalesOrderByStoreAndItem_TotalDTO.TotalRevenue = Math.Round(DirectSalesOrderContentQuery.Select(x => x.Amount).Sum()
                - DirectSalesOrderContentQuery.Where(x => x.GeneralDiscountAmount.HasValue).Select(x => x.GeneralDiscountAmount.Value).Sum()
                + DirectSalesOrderContentQuery.Where(x => x.TaxAmount.HasValue).Select(x => x.TaxAmount.Value).Sum(), 0);

            ReportDirectSalesOrderByStoreAndItem_TotalDTO.TotalDiscount = Math.Round(DirectSalesOrderContentQuery.Where(x => x.GeneralDiscountAmount.HasValue).Select(x => x.GeneralDiscountAmount.Value).Sum()
                + DirectSalesOrderContentQuery.Where(x => x.DiscountAmount.HasValue).Select(x => x.DiscountAmount.Value).Sum(), 0);
            return ReportDirectSalesOrderByStoreAndItem_TotalDTO;
        }

    }
}
