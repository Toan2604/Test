using DMS.Common;
using DMS.DWModels;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MKpiCriteriaItem;
using DMS.Services.MKpiItemType;
using DMS.Services.MKpiPeriod;
using DMS.Services.MKpiYear;
using DMS.Services.MOrganization;
using DMS.Services.MProduct;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_item_report
{
    public class KpiItemReportController : RpcController
    {
        private DataContext DataContext;
        private DWContext DWContext;
        private IOrganizationService OrganizationService;
        private IAppUserService AppUserService;
        private IKpiYearService KpiYearService;
        private IKpiPeriodService KpiPeriodService;
        private IItemService ItemService;
        private IKpiItemTypeService KpiItemTypeService;
        private IKpiCriteriaItemService KpiCriteriaItemService;
        private ICurrentContext CurrentContext;
        public KpiItemReportController(DataContext DataContext,
            DWContext DWContext,
            IOrganizationService OrganizationService,
            IAppUserService AppUserService,
            IKpiYearService KpiYearService,
            IKpiPeriodService KpiPeriodService,
            IItemService ItemService,
            IKpiItemTypeService KpiItemTypeService,
            IKpiCriteriaItemService KpiCriteriaItemService,
            ICurrentContext CurrentContext)
        {
            this.DataContext = DataContext;
            this.DWContext = DWContext;
            this.OrganizationService = OrganizationService;
            this.AppUserService = AppUserService;
            this.KpiPeriodService = KpiPeriodService;
            this.KpiYearService = KpiYearService;
            this.ItemService = ItemService;
            this.KpiItemTypeService = KpiItemTypeService;
            this.KpiCriteriaItemService = KpiCriteriaItemService;
            this.CurrentContext = CurrentContext;
        }

        #region Filter List

        [Route(KpiItemReportRoute.ListKpiCriteriaItem), HttpPost]
        public async Task<List<KpiItemReport_KpiCriteriaItemDTO>> ListKpiCriteriaItem([FromBody] KpiItemReport_KpiCriteriaItemFilterDTO KpiItemReport_KpiCriteriaItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiCriteriaItemFilter KpiCriteriaItemFilter = new KpiCriteriaItemFilter();
            KpiCriteriaItemFilter.Skip = 0;
            KpiCriteriaItemFilter.Take = int.MaxValue;
            KpiCriteriaItemFilter.Selects = KpiCriteriaItemSelect.ALL;

            List<KpiCriteriaItem> KpiCriteriaItems = await KpiCriteriaItemService.List(KpiCriteriaItemFilter);
            List<KpiItemReport_KpiCriteriaItemDTO> KpiItemReport_KpiCriteriaItemDTOs = KpiCriteriaItems
                .Select(x => new KpiItemReport_KpiCriteriaItemDTO(x)).ToList();
            return KpiItemReport_KpiCriteriaItemDTOs;
        }

        [Route(KpiItemReportRoute.FilterListAppUser), HttpPost]
        public async Task<List<KpiItemReport_AppUserDTO>> FilterListAppUser([FromBody] KpiItemReport_AppUserFilterDTO KpiItemReport_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.Id = KpiItemReport_AppUserFilterDTO.Id;
            AppUserFilter.OrganizationId = KpiItemReport_AppUserFilterDTO.OrganizationId;
            AppUserFilter.Username = KpiItemReport_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = KpiItemReport_AppUserFilterDTO.DisplayName;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<KpiItemReport_AppUserDTO> KpiItemReport_AppUserDTOs = AppUsers
                .Select(x => new KpiItemReport_AppUserDTO(x)).ToList();
            return KpiItemReport_AppUserDTOs;
        }

        [Route(KpiItemReportRoute.FilterListOrganization), HttpPost]
        public async Task<List<KpiItemReport_OrganizationDTO>> FilterListOrganization([FromBody] KpiItemReport_OrganizationFilterDTO KpiItemReport_OrganizationFilterDTO)
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
            List<KpiItemReport_OrganizationDTO> KpiItemReport_OrganizationDTOs = Organizations
                .Select(x => new KpiItemReport_OrganizationDTO(x)).ToList();
            return KpiItemReport_OrganizationDTOs;
        }

        [Route(KpiItemReportRoute.FilterListItem), HttpPost]
        public async Task<List<KpiItemReport_ItemDTO>> FilterListItem([FromBody] KpiItemReport_ItemFilterDTO KpiItemReport_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Skip = KpiItemReport_ItemFilterDTO.Skip;
            ItemFilter.Take = KpiItemReport_ItemFilterDTO.Take;
            ItemFilter.OrderBy = ItemOrder.Id;
            ItemFilter.OrderType = OrderType.ASC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            ItemFilter.Id = KpiItemReport_ItemFilterDTO.Id;
            ItemFilter.Code = KpiItemReport_ItemFilterDTO.Code;
            ItemFilter.Name = KpiItemReport_ItemFilterDTO.Name;
            ItemFilter.Search = KpiItemReport_ItemFilterDTO.Search;

            List<Item> Items = await ItemService.List(ItemFilter);
            List<KpiItemReport_ItemDTO> KpiItemReport_ItemDTOs = Items
                .Select(x => new KpiItemReport_ItemDTO(x)).ToList();
            return KpiItemReport_ItemDTOs;
        }

        [Route(KpiItemReportRoute.FilterListKpiPeriod), HttpPost]
        public async Task<List<KpiItemReport_KpiPeriodDTO>> FilterListKpiPeriod([FromBody] KpiItemReport_KpiPeriodFilterDTO KpiItemReport_KpiPeriodFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiPeriodFilter KpiPeriodFilter = new KpiPeriodFilter();
            KpiPeriodFilter.Skip = 0;
            KpiPeriodFilter.Take = 20;
            KpiPeriodFilter.OrderBy = KpiPeriodOrder.Id;
            KpiPeriodFilter.OrderType = OrderType.ASC;
            KpiPeriodFilter.Selects = KpiPeriodSelect.ALL;
            KpiPeriodFilter.Id = KpiItemReport_KpiPeriodFilterDTO.Id;
            KpiPeriodFilter.Code = KpiItemReport_KpiPeriodFilterDTO.Code;
            KpiPeriodFilter.Name = KpiItemReport_KpiPeriodFilterDTO.Name;

            List<KpiPeriod> KpiPeriods = await KpiPeriodService.List(KpiPeriodFilter);
            List<KpiItemReport_KpiPeriodDTO> KpiItemReport_KpiPeriodDTOs = KpiPeriods
                .Select(x => new KpiItemReport_KpiPeriodDTO(x)).ToList();
            return KpiItemReport_KpiPeriodDTOs;
        }

        [Route(KpiItemReportRoute.FilterListKpiYear), HttpPost]
        public async Task<List<KpiItemReport_KpiYearDTO>> FilterListKpiYear([FromBody] KpiItemReport_KpiYearFilterDTO KpiItemReport_KpiYearFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiYearFilter KpiYearFilter = new KpiYearFilter();
            KpiYearFilter.Skip = 0;
            KpiYearFilter.Take = 20;
            KpiYearFilter.OrderBy = KpiYearOrder.Id;
            KpiYearFilter.OrderType = OrderType.ASC;
            KpiYearFilter.Selects = KpiYearSelect.ALL;
            KpiYearFilter.Id = KpiItemReport_KpiYearFilterDTO.Id;
            KpiYearFilter.Code = KpiItemReport_KpiYearFilterDTO.Code;
            KpiYearFilter.Name = KpiItemReport_KpiYearFilterDTO.Name;

            List<KpiYear> KpiYears = await KpiYearService.List(KpiYearFilter);
            List<KpiItemReport_KpiYearDTO> KpiItemReport_KpiYearDTOs = KpiYears
                .Select(x => new KpiItemReport_KpiYearDTO(x)).ToList();
            return KpiItemReport_KpiYearDTOs;
        }

        [Route(KpiItemReportRoute.FilterListKpiItemType), HttpPost]
        public async Task<List<KpiItemReport_KpiItemTypeDTO>> FilterListKpiItemType([FromBody] KpiItemReport_KpiItemTypeFilterDTO KpiItemReport_KpiItemTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiItemTypeFilter KpiItemTypeFilter = new KpiItemTypeFilter();
            KpiItemTypeFilter.Skip = 0;
            KpiItemTypeFilter.Take = 20;
            KpiItemTypeFilter.OrderBy = KpiItemTypeOrder.Id;
            KpiItemTypeFilter.OrderType = OrderType.ASC;
            KpiItemTypeFilter.Selects = KpiItemTypeSelect.ALL;
            KpiItemTypeFilter.Id = KpiItemReport_KpiItemTypeFilterDTO.Id;
            KpiItemTypeFilter.Code = KpiItemReport_KpiItemTypeFilterDTO.Code;
            KpiItemTypeFilter.Name = KpiItemReport_KpiItemTypeFilterDTO.Name;

            List<KpiItemType> KpiItemTypes = await KpiItemTypeService.List(KpiItemTypeFilter);
            List<KpiItemReport_KpiItemTypeDTO> KpiItemReport_KpiItemTypeDTOs = KpiItemTypes
                .Select(x => new KpiItemReport_KpiItemTypeDTO(x)).ToList();
            return KpiItemReport_KpiItemTypeDTOs;
        }
        #endregion

        [Route(KpiItemReportRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] KpiItemReport_KpiItemReportFilterDTO KpiItemReport_KpiItemReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState); // to do kpi year and period
            
            long? SaleEmployeeId = KpiItemReport_KpiItemReportFilterDTO.AppUserId?.Equal;
            long? ItemId = KpiItemReport_KpiItemReportFilterDTO.ItemId?.Equal;
            if (KpiItemReport_KpiItemReportFilterDTO.KpiPeriodId?.Equal.HasValue == false ||
                KpiItemReport_KpiItemReportFilterDTO.KpiYearId?.Equal.HasValue == false)
                return 0;
            long? KpiPeriodId = KpiItemReport_KpiItemReportFilterDTO.KpiPeriodId?.Equal.Value;
            long? KpiYearId = KpiItemReport_KpiItemReportFilterDTO.KpiYearId?.Equal.Value;
            long? KpiItemTypeId = KpiItemReport_KpiItemReportFilterDTO.KpiItemTypeId?.Equal.Value;
 
            List<long> AppUserIds, OrganizationIds;
            (AppUserIds, OrganizationIds) = await FilterOrganizationAndUser(KpiItemReport_KpiItemReportFilterDTO.OrganizationId,
                AppUserService, OrganizationService, CurrentContext, DataContext);

            var query = from ki in DataContext.KpiItem
                        join kic in DataContext.KpiItemContent on ki.Id equals kic.KpiItemId
                        join i in DataContext.Item on kic.ItemId equals i.Id
                        where OrganizationIds.Contains(ki.OrganizationId) &&
                        AppUserIds.Contains(ki.EmployeeId) &&
                        (SaleEmployeeId == null || ki.Id == SaleEmployeeId.Value) &&
                        (ItemId == null || i.Id == ItemId.Value) &&
                        (ki.KpiPeriodId == KpiPeriodId.Value) &&
                        (ki.KpiYearId == KpiYearId) &&
                        (ki.KpiItemTypeId == KpiItemTypeId.Value) &&
                        ki.DeletedAt == null &&
                        ki.StatusId == StatusEnum.ACTIVE.Id
                        select new
                        {
                            SaleEmployeeId = ki.EmployeeId,
                            ItemId = i.Id,
                        };
            return await query.Distinct().CountWithNoLockAsync();
        }

        [Route(KpiItemReportRoute.List), HttpPost]
        public async Task<ActionResult<List<KpiItemReport_KpiItemReportDTO>>> List([FromBody] KpiItemReport_KpiItemReportFilterDTO KpiItemReport_KpiItemReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (KpiItemReport_KpiItemReportFilterDTO.KpiPeriodId?.Equal.HasValue == false)
                return BadRequest(new { message = "Chưa chọn kì KPI" });
            if (KpiItemReport_KpiItemReportFilterDTO.KpiYearId?.Equal.HasValue == false)
                return BadRequest(new { message = "Chưa chọn năm KPI" });
            if (KpiItemReport_KpiItemReportFilterDTO.KpiItemTypeId?.Equal.HasValue == false)
                return BadRequest(new { message = "Chưa chọn loại KPI" });

            DateTime StartDate, EndDate;
            long? SaleEmployeeId = KpiItemReport_KpiItemReportFilterDTO.AppUserId?.Equal;
            long? ItemId = KpiItemReport_KpiItemReportFilterDTO.ItemId?.Equal;
            long? KpiPeriodId = KpiItemReport_KpiItemReportFilterDTO.KpiPeriodId?.Equal.Value;
            long? KpiYearId = KpiItemReport_KpiItemReportFilterDTO.KpiYearId?.Equal.Value;
            long? KpiItemTypeId = KpiItemReport_KpiItemReportFilterDTO.KpiItemTypeId?.Equal.Value;
            (StartDate, EndDate) = DateTimeConvert(KpiPeriodId.Value, KpiYearId.Value);
            if (KpiItemReport_KpiItemReportFilterDTO.OrderDate != null)
            {
                if (KpiItemReport_KpiItemReportFilterDTO.OrderDate.GreaterEqual != null)
                {
                    var StartDateFilter = KpiItemReport_KpiItemReportFilterDTO.OrderDate.GreaterEqual.Value;
                    StartDate = StartDateFilter >= StartDate ? StartDateFilter : StartDate; // Don't allow get date out of Kpi period range
                }
                if (KpiItemReport_KpiItemReportFilterDTO.OrderDate.LessEqual != null)
                {
                    var EndDateFilter = KpiItemReport_KpiItemReportFilterDTO.OrderDate.LessEqual.Value;
                    EndDate = EndDateFilter <= EndDate ? EndDateFilter : EndDate; // Don't allow get date out of Kpi period range
                }
            }

            IdFilter SaleEmployeeIdFilter = new IdFilter() { Equal = SaleEmployeeId };
            DateFilter OrderDateFilter = new DateFilter() { GreaterEqual = StartDate, LessEqual = EndDate };

            List<long> AppUserIds, OrganizationIds;
            (AppUserIds, OrganizationIds) = await FilterOrganizationAndUser(KpiItemReport_KpiItemReportFilterDTO.OrganizationId,
                AppUserService, OrganizationService, CurrentContext, DataContext);

            var query = from ki in DataContext.KpiItem
                        join au in DataContext.AppUser on ki.EmployeeId equals au.Id
                        join kic in DataContext.KpiItemContent on ki.Id equals kic.KpiItemId
                        join i in DataContext.Item on kic.ItemId equals i.Id
                        where OrganizationIds.Contains(ki.OrganizationId) &&
                        AppUserIds.Contains(au.Id) &&
                        (SaleEmployeeId.HasValue == false || ki.EmployeeId == SaleEmployeeId.Value) &&
                        (ItemId.HasValue == false || i.Id == ItemId.Value) &&
                        (ki.KpiPeriodId == KpiPeriodId.Value) &&
                        (ki.KpiYearId == KpiYearId.Value) &&
                        (ki.KpiItemTypeId == KpiItemTypeId.Value) &&
                        ki.DeletedAt == null &&
                        ki.StatusId == StatusEnum.ACTIVE.Id
                        select new
                        {
                            SaleEmployeeId = au.Id,
                            Username = au.Username,
                            DisplayName = au.DisplayName,
                            OrganizationId = au.OrganizationId,
                            ItemId = i.Id,
                            ItemCode = i.Code,
                            ItemName = i.Name,
                        };

            var ItemContents = await query.Distinct()
                .OrderBy(q => q.OrganizationId).ThenBy(x => x.DisplayName)
                .Skip(KpiItemReport_KpiItemReportFilterDTO.Skip)
                .Take(KpiItemReport_KpiItemReportFilterDTO.Take)
                .ToListWithNoLockAsync();

            List<long> SaleEmployeeIds = ItemContents.Select(x => x.SaleEmployeeId).Distinct().ToList();


            List<KpiItemReport_KpiItemReportDTO> KpiItemReport_KpiItemReportDTOs = new List<KpiItemReport_KpiItemReportDTO>();
            foreach (var EmployeeId in SaleEmployeeIds)
            {
                KpiItemReport_KpiItemReportDTO KpiItemReport_KpiItemReportDTO = new KpiItemReport_KpiItemReportDTO()
                {
                    SaleEmployeeId = EmployeeId,
                    DisplayName = ItemContents.Where(x => x.SaleEmployeeId == EmployeeId).Select(x => x.DisplayName).FirstOrDefault(),
                    Username = ItemContents.Where(x => x.SaleEmployeeId == EmployeeId).Select(x => x.Username).FirstOrDefault(),
                    ItemContents = ItemContents.Where(x => x.SaleEmployeeId == EmployeeId).Select(x => new KpiItemReport_KpiItemContentDTO
                    {
                        ItemId = x.ItemId,
                        SaleEmployeeId = EmployeeId,
                        ItemName = x.ItemName,
                        ItemCode = x.ItemCode,
                    })
                    .Where(x => x.SaleEmployeeId == EmployeeId)
                    .ToList()
                };
                KpiItemReport_KpiItemReportDTOs.Add(KpiItemReport_KpiItemReportDTO);
            }

            // lay du lieu bang mapping
            var query_detail = from km in DataContext.KpiItemContentKpiCriteriaItemMapping
                               join kc in DataContext.KpiItemContent on km.KpiItemContentId equals kc.Id
                               join k in DataContext.KpiItem on kc.KpiItemId equals k.Id
                               join i in DataContext.Item on kc.ItemId equals i.Id
                               where (SaleEmployeeIds.Contains(k.EmployeeId) &&
                                      k.KpiYearId == KpiYearId &&
                                      k.KpiPeriodId == KpiPeriodId &&
                                      k.KpiItemTypeId == KpiItemTypeId &&
                                      (ItemId == null || i.Id == ItemId)) &&
                                      km.Value.HasValue &&
                                      k.DeletedAt == null &&
                                      k.StatusId == StatusEnum.ACTIVE.Id
                               select new
                               {
                                   SaleEmployeeId = k.EmployeeId,
                                   KpiCriteriaItemId = km.KpiCriteriaItemId,
                                   Value = km.Value.Value,
                                   ItemId = i.Id,
                               };

            List<KpiItemReport_KpiItemContentKpiCriteriaItemMappingDTO>
                KpiItemReport_KpiItemContentKpiCriteriaItemMappingDTOs = (await query_detail.Distinct()
                .ToListWithNoLockAsync())
                .Select(x => new KpiItemReport_KpiItemContentKpiCriteriaItemMappingDTO
                {
                    SaleEmployeeId = x.SaleEmployeeId,
                    KpiCriteriaItemId = x.KpiCriteriaItemId,
                    Value = x.Value,
                    ItemId = x.ItemId,
                })
                .ToList();

            // DirectSalesOrderTransaction
            var direct_sales_order_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            direct_sales_order_query = direct_sales_order_query.Where(q => q.DeletedAt == null);
            direct_sales_order_query = direct_sales_order_query.Where(q => q.OrderDate, OrderDateFilter);
            direct_sales_order_query = direct_sales_order_query.Where(q => q.SaleEmployeeId, SaleEmployeeIdFilter);
            direct_sales_order_query = direct_sales_order_query.Where(q => q.GeneralApprovalStateId, new IdFilter()
            {
                In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });
            var direct_sales_order_transaction_query = from tr in DWContext.Fact_DirectSalesOrderTransaction
                                                       join dr in direct_sales_order_query on tr.DirectSalesOrderId equals dr.DirectSalesOrderId
                                                       select tr;

            direct_sales_order_transaction_query = direct_sales_order_transaction_query.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id });
            var DirectSalesOrderTransactionDAOs = await direct_sales_order_transaction_query.ToListWithNoLockAsync();

            // IndirectSalesOrderTransaction
            var indirect_sales_order_transaction_query = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            indirect_sales_order_transaction_query = indirect_sales_order_transaction_query.Where(q => q.OrderDate, OrderDateFilter);
            indirect_sales_order_transaction_query = indirect_sales_order_transaction_query.Where(q => q.SalesEmployeeId, SaleEmployeeIdFilter);
            indirect_sales_order_transaction_query = indirect_sales_order_transaction_query.Where(q => q.RequestStateId, new IdFilter() { Equal = RequestStateEnum.APPROVED.Id });
            indirect_sales_order_transaction_query = indirect_sales_order_transaction_query.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id });
            indirect_sales_order_transaction_query = indirect_sales_order_transaction_query.Where(x => !x.DeletedAt.HasValue);

            var InDirectSalesOrderTransactionDAOs = await indirect_sales_order_transaction_query.ToListWithNoLockAsync();

            foreach (var Employee in KpiItemReport_KpiItemReportDTOs)
            {
                foreach (var ItemContent in Employee.ItemContents)
                {
                    //lấy tất cả đơn hàng được thực hiện bởi nhân viên đang xét
                    var IndirectSalesOrderTransactions = InDirectSalesOrderTransactionDAOs
                            .Where(x => x.SalesEmployeeId == Employee.SaleEmployeeId)
                            .ToList();
                    var DirectSalesOrderTransactions = DirectSalesOrderTransactionDAOs
                        .Where(x => x.SalesEmployeeId == Employee.SaleEmployeeId)
                        .ToList();

                    #region Doanh thu theo đơn hàng trực tiếp
                    //kế hoạch
                    ItemContent.DirectRevenuePlanned = KpiItemReport_KpiItemContentKpiCriteriaItemMappingDTOs
                            .Where(x => x.SaleEmployeeId == ItemContent.SaleEmployeeId &&
                            x.ItemId == ItemContent.ItemId &&
                            x.KpiCriteriaItemId == KpiCriteriaItemEnum.DIRECT_REVENUE.Id)
                            .Select(x => x.Value).FirstOrDefault();

                    //thực hiện
                    if (ItemContent.DirectRevenuePlanned.HasValue)
                    {
                        ItemContent.DirectRevenue = ItemContent.DirectRevenuePlanned == null ? null :
                            DirectSalesOrderTransactions == null ? 0 : 
                            (decimal?)DirectSalesOrderTransactions.Where(t => t.ItemId == ItemContent.ItemId)
                            .Sum(t => (t.Amount - t.GeneralDiscountAmount ?? 0));
                        ItemContent.DirectRevenue = Math.Round(ItemContent.DirectRevenue.Value, 0);
                        //tỉ lệ
                        ItemContent.DirectRevenueRatio = ItemContent.DirectRevenuePlanned == 0 || ItemContent.DirectRevenuePlanned == null ?
                            null : (decimal?)
                            Math.Round((ItemContent.DirectRevenue.Value / ItemContent.DirectRevenuePlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Đại lý theo đơn hàng trực tiếp
                    //kế hoạch
                    ItemContent.DirectStorePlanned = KpiItemReport_KpiItemContentKpiCriteriaItemMappingDTOs
                            .Where(x => x.SaleEmployeeId == ItemContent.SaleEmployeeId &&
                            x.ItemId == ItemContent.ItemId &&
                            x.KpiCriteriaItemId == KpiCriteriaItemEnum.DIRECT_STORE.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    //thực hiện
                    if (ItemContent.DirectStorePlanned.HasValue)
                    {
                        var BuyerStoreIds = ItemContent.DirectStorePlanned == null ? null : 
                            DirectSalesOrderTransactions.Where(t => t.ItemId == ItemContent.ItemId).Select(t => t.BuyerStoreId)
                            .Distinct().ToList();
                        ItemContent.StoreDirectIds = new HashSet<long>(BuyerStoreIds);
                        if (BuyerStoreIds == null) ItemContent.StoreDirectIds = new HashSet<long>();
                        //tỉ lệ
                        ItemContent.DirectStoreRatio = ItemContent.DirectStorePlanned == null || ItemContent.DirectStorePlanned == 0 ?
                            null : (decimal?)
                            Math.Round((ItemContent.DirectStore.Value / ItemContent.DirectStorePlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Doanh thu theo đơn hàng gián tiếp
                    //kế hoạch
                    ItemContent.IndirectRevenuePlanned = KpiItemReport_KpiItemContentKpiCriteriaItemMappingDTOs
                            .Where(x => x.SaleEmployeeId == ItemContent.SaleEmployeeId &&
                            x.ItemId == ItemContent.ItemId &&
                            x.KpiCriteriaItemId == KpiCriteriaItemEnum.INDIRECT_REVENUE.Id)
                            .Select(x => x.Value).FirstOrDefault();

                    //thực hiện
                    if (ItemContent.IndirectRevenuePlanned.HasValue)
                    {
                        ItemContent.IndirectRevenue = IndirectSalesOrderTransactions == null ? 0 : IndirectSalesOrderTransactions.Where(t => t.ItemId == ItemContent.ItemId)
                            .Sum(t =>(t.Amount - t.GeneralDiscountAmount ?? 0));
                        ItemContent.IndirectRevenue = Math.Round(ItemContent.IndirectRevenue.Value, 0);
                        //tỉ lệ
                        ItemContent.IndirectRevenueRatio = ItemContent.IndirectRevenuePlanned == 0 || ItemContent.IndirectRevenuePlanned == null ?
                            null : (decimal?)
                            Math.Round((ItemContent.IndirectRevenue.Value / ItemContent.IndirectRevenuePlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Đại lý theo đơn hàng gián tiếp
                    //kế hoạch
                    ItemContent.IndirectStorePlanned = KpiItemReport_KpiItemContentKpiCriteriaItemMappingDTOs
                            .Where(x => x.SaleEmployeeId == ItemContent.SaleEmployeeId &&
                            x.ItemId == ItemContent.ItemId &&
                            x.KpiCriteriaItemId == KpiCriteriaItemEnum.INDIRECT_STORE.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    //thực hiện
                    if (ItemContent.IndirectStorePlanned.HasValue)
                    {
                        var BuyerStoreIds = IndirectSalesOrderTransactions.Where(t => t.ItemId == ItemContent.ItemId).Select(t => t.BuyerStoreId)
                            .Distinct().ToList();
                        ItemContent.StoreIndirectIds = new HashSet<long>(BuyerStoreIds);
                        if (BuyerStoreIds == null) ItemContent.StoreIndirectIds = new HashSet<long>();
                        //tỉ lệ
                        ItemContent.IndirectStoreRatio = ItemContent.IndirectStorePlanned == null || ItemContent.IndirectStorePlanned == 0 ?
                            null : (decimal?)
                            Math.Round((ItemContent.IndirectStore.Value / ItemContent.IndirectStorePlanned.Value) * 100, 2);
                    }
                    #endregion
                }
            };
            KpiItemReport_KpiItemReportDTOs = KpiItemReport_KpiItemReportDTOs.Where(x => x.ItemContents.Any()).ToList();
            return KpiItemReport_KpiItemReportDTOs;
        }

        [Route(KpiItemReportRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] KpiItemReport_KpiItemReportFilterDTO KpiItemReport_KpiItemReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (KpiItemReport_KpiItemReportFilterDTO.KpiPeriodId?.Equal.HasValue == false)
                return BadRequest(new { message = "Chưa chọn kì KPI" });
            if (KpiItemReport_KpiItemReportFilterDTO.KpiYearId?.Equal.HasValue == false)
                return BadRequest(new { message = "Chưa chọn năm KPI" });
            if (KpiItemReport_KpiItemReportFilterDTO.KpiItemTypeId?.Equal.HasValue == false)
                return BadRequest(new { message = "Chưa chọn loại KPI" });

            var KpiPeriod = KpiPeriodEnum.KpiPeriodEnumList.Where(x => x.Id == KpiItemReport_KpiItemReportFilterDTO.KpiPeriodId.Equal.Value).FirstOrDefault();
            var KpiYear = KpiYearEnum.KpiYearEnumList.Where(x => x.Id == KpiItemReport_KpiItemReportFilterDTO.KpiYearId.Equal.Value).FirstOrDefault();
            var KpiItemType = KpiItemTypeEnum.KpiItemTypeEnumList.Where(x => x.Id == KpiItemReport_KpiItemReportFilterDTO.KpiItemTypeId.Equal.Value).FirstOrDefault();

            DateTime StartDate, EndDate;
            long KpiPeriodId = KpiItemReport_KpiItemReportFilterDTO.KpiPeriodId?.Equal ?? 100 + StaticParams.DateTimeNow.AddHours(0 - CurrentContext.TimeZone).Month;
            long KpiYearId = KpiItemReport_KpiItemReportFilterDTO.KpiYearId?.Equal ?? StaticParams.DateTimeNow.AddHours(0 - CurrentContext.TimeZone).Year;
            (StartDate, EndDate) = DateTimeConvert(KpiPeriodId, KpiYearId);
            if (KpiItemReport_KpiItemReportFilterDTO.OrderDate.HasValue)
            {
                StartDate = KpiItemReport_KpiItemReportFilterDTO.OrderDate.GreaterEqual == null ? StartDate : KpiItemReport_KpiItemReportFilterDTO.OrderDate.GreaterEqual.Value;
                EndDate = KpiItemReport_KpiItemReportFilterDTO.OrderDate.GreaterEqual == null ? EndDate : KpiItemReport_KpiItemReportFilterDTO.OrderDate.LessEqual.Value;
            }

            KpiItemReport_KpiItemReportFilterDTO.Skip = 0;
            KpiItemReport_KpiItemReportFilterDTO.Take = int.MaxValue;
            List<KpiItemReport_KpiItemReportDTO> KpiItemReport_KpiItemReportDTOs = (await List(KpiItemReport_KpiItemReportFilterDTO)).Value;

            long stt = 1;
            foreach (KpiItemReport_KpiItemReportDTO KpiItemReport_KpiItemReportDTO in KpiItemReport_KpiItemReportDTOs)
            {
                foreach (var ItemContent in KpiItemReport_KpiItemReportDTO.ItemContents)
                {
                    ItemContent.STT = stt;
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

            List <KpiCriteriaItem> KpiCriteriaItems = await KpiCriteriaItemService.List(new KpiCriteriaItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiCriteriaItemSelect.ALL,
            });

            List<KpiItemReport_ExportDTO> KpiItemReport_ExportDTOs = await ConvertReportToExportDTO(KpiItemReport_KpiItemReportDTOs);

            MemoryStream output = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(output))
            {
                #region Tieu de bao cao
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add("Báo cáo KPI sản phẩm");
                ws.Cells.Style.Font.Name = "Times New Roman";
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                ws.Cells["A1"].Value = OrgRoot.Name.ToUpper();
                ws.Cells["A1"].Style.Font.Size = 11;
                ws.Cells["A1"].Style.Font.Bold = true;

                ws.Cells["A3"].Value = "BÁO CÁO KPI SẢN PHẨM";

                ws.Cells["B4"].Value = KpiItemType.Name;
                ws.Cells["B4"].Style.Font.Italic = true;
                ws.Cells["B4"].Style.Font.Bold = true;
                ws.Cells["B4:C4"].Merge = true;

                ws.Cells["D4"].Value = $"Kỳ KPI: {KpiPeriod.Name} - Năm: {KpiYear.Name}";
                ws.Cells["D4"].Style.Font.Italic = true;
                ws.Cells["D4"].Style.Font.Bold = true;
                ws.Cells["D4:G4"].Merge = true;

                ws.Cells["B5"].Value = "Từ ngày";
                ws.Cells["C5"].Value = StartDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");

                ws.Cells["D5"].Value = "Đến ngày";
                ws.Cells["E5"].Value = EndDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");

                #endregion

                #region Header bang ket qua
                List<string> headers = new List<string>
                {
                    "STT",
                    "Mã sản phẩm",
                    "Tên sản phẩm"
                };
                List<string> headerLine2 = new List<string>
                {
                    "", "", ""
                };

                KpiCriteriaItems = KpiCriteriaItems.OrderBy(x => x.Id).ToList();
                for (int i = 0; i < KpiCriteriaItems.Count; i++)
                {
                    headers.Add(KpiCriteriaItems[i].Name);
                    headers.Add("");
                    headers.Add(""); // thêm ô trống vào để merge 3 ô dòng trên lại
                    headerLine2.Add("Kế hoạch");
                    headerLine2.Add("Thực hiện");
                    headerLine2.Add("Tỷ lệ(%)");
                }
                int endColumnNumber = headers.Count;
                string endColumnString = Char.ConvertFromUtf32(endColumnNumber + 64);
                if (endColumnNumber > 26) endColumnString = Char.ConvertFromUtf32(endColumnNumber / 26 + 64) + Char.ConvertFromUtf32(endColumnNumber % 26 + 64); ;

                // format lại tiêu đề theo số cột của dữ liệu
                ws.Cells[$"A3:{endColumnString}3"].Merge = true;
                ws.Cells[$"A3:{endColumnString}3"].Style.Font.Size = 20;
                ws.Cells[$"A3:{endColumnString}3"].Style.Font.Bold = true;
                ws.Cells[$"A3:{endColumnString}3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                string headerRange = $"A7:" + $"{endColumnString}8";
                List<string[]> Header = new List<string[]> { headers.ToArray(), headerLine2.ToArray() };
                ws.Cells[headerRange].LoadFromArrays(Header);
                ws.Cells[headerRange].Style.Font.Size = 11;
                ws.Cells[headerRange].Style.Font.Bold = true;
                ws.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                ws.Cells["A7:A8"].Merge = true;
                ws.Cells["B7:B8"].Merge = true;
                ws.Cells["C7:C8"].Merge = true;

                for (int i = 1; i <= endColumnNumber; i += 3)
                {
                    if (i == 1) continue;

                    string startMerge = Char.ConvertFromUtf32(i + 64);
                    if (i > 26) startMerge = Char.ConvertFromUtf32(i / 26 + 64) + Char.ConvertFromUtf32(i % 26 + 64);
                    string endMerge = Char.ConvertFromUtf32(i + 2 + 64);
                    if (i + 2 > 26) endMerge = Char.ConvertFromUtf32((i + 2) / 26 + 64) + Char.ConvertFromUtf32((i + 2) % 26 + 64);
                    ws.Cells[$"{startMerge}7:{endMerge}7"].Merge = true; // merge cai dong tren cua header
                }
                string headerRange2 = $"A8:" + $"{endColumnString}8";

                ws.Cells[headerRange].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[headerRange].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Cells[headerRange2].AutoFitColumns();
                ws.Column(2).Width = 15;
                ws.Column(3).Width = 20;
                #endregion

                #region Du lieu bao cao
                int startRow = 9;
                int startColumn = 1;
                int endColumn = startColumn;
                int endRow = startRow;
                if (KpiItemReport_ExportDTOs != null && KpiItemReport_ExportDTOs.Count > 0)
                {
                    foreach (var KpiItemReport_ExportDTO in KpiItemReport_ExportDTOs)
                    {
                        ws.Cells[$"A{startRow}"].Value = $"{KpiItemReport_ExportDTO.Username} - {KpiItemReport_ExportDTO.DisplayName}";
                        ws.Cells[$"A{startRow}"].Style.Font.Bold = true;
                        ws.Cells[$"A{startRow}"].Style.Font.Italic = true;
                        ws.Cells[$"A{startRow}:{endColumnString}{startRow}"].Merge = true;
                        ws.Cells[$"A{startRow}:{endColumnString}{startRow}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        startRow++;

                        #region Cột STT
                        List<Object[]> SttData = new List<object[]>();
                        endRow = startRow;
                        foreach (var item in KpiItemReport_ExportDTO.Items)
                        {
                            SttData.Add(new object[]
                            {
                                item.STT,
                            });
                            endRow++;
                        }
                        string startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                        if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                        string currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                        if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);

                        ws.Cells[$"{currentColumnString}{startRow}:{startColumnString}{endRow - 1}"].LoadFromArrays(SttData);
                        endColumn += 1;
                        startColumn = endColumn;
                        #endregion

                        #region Cột mã nhân viên, tên nhân viên
                        List<Object[]> KpiData = new List<object[]>();
                        endRow = startRow;
                        foreach (var item in KpiItemReport_ExportDTO.Items)
                        {
                            KpiData.Add(new object[]
                            {
                                item.ItemCode,
                                item.ItemName,
                            });
                            endRow++;
                        }
                        startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                        if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                        currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                        if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);

                        ws.Cells[$"{currentColumnString}{startRow}:{startColumnString}{endRow - 1}"].LoadFromArrays(KpiData);
                        endColumn += 2;
                        startColumn = endColumn;
                        #endregion

                        #region Các cột giá trị
                        for (int i = 0; i < KpiCriteriaItems.Count; i++)
                        {
                            List<Object[]> ValueData = new List<object[]>();
                            endRow = startRow; // gán lại dòng bắt đầu
                            foreach (var item in KpiItemReport_ExportDTO.Items)
                            {
                                KpiItemReport_CriteriaContentDTO criteriaContent = item
                                    .CriteriaContents.Where(x => x.CriteriaId == KpiCriteriaItems[i].Id).FirstOrDefault(); // Lấy ra giá trị của criteria tương ứng
                                ValueData.Add(new object[]
                                {
                                criteriaContent.Planned,
                                criteriaContent.Result,
                                criteriaContent.Ratio,
                                });
                                endRow++;
                            }
                            startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                            if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                            currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                            if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);


                            ws.Cells[$"{currentColumnString}{startRow}:{startColumnString}{endRow - 1}"].LoadFromArrays(ValueData); // fill dữ liệu

                            endColumn += 3; // Chiếm 3 cột cho mỗi criteria
                            startColumn = endColumn;
                        }
                        startColumn = 1;
                        endColumn = startColumn; // gán lại cột bắt đầu
                        startRow = endRow; // gán dòng bắt đầu cho org tiếp theo
                        #endregion
                    }
                }

                ws.Column(3).Width = 45; // column ten san pham
                ws.Cells[$"D9:{endColumnString}{endRow}"].Style.Numberformat.Format = "#,##0.00"; // format number column value

                // All borders
                ws.Cells[$"A7:{endColumnString}{endRow}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A7:{endColumnString}{endRow}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A7:{endColumnString}{endRow}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A7:{endColumnString}{endRow}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                #endregion

                excel.Save();
            }

            return File(output.ToArray(), "application/octet-stream", "KpiItemReport.xlsx");
        }

        private async Task<List<KpiItemReport_ExportDTO>> ConvertReportToExportDTO(List<KpiItemReport_KpiItemReportDTO> KpiItemReport_KpiItemReportDTOs)
        {
            List<KpiItemReport_ExportDTO> KpiItemReport_ExportDTOs = new List<KpiItemReport_ExportDTO>();
            List<KpiCriteriaItem> KpiCriteriaItems = await KpiCriteriaItemService.List(new KpiCriteriaItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiCriteriaItemSelect.Id,
            });
            List<long> KpiCriteriaItemIds = KpiCriteriaItems.Select(x => x.Id).ToList();

            foreach (var KpiItemReport_KpiItemReportDTO in KpiItemReport_KpiItemReportDTOs)
            {
                KpiItemReport_ExportDTO KpiItemReport_ExportDTO = new KpiItemReport_ExportDTO();
                KpiItemReport_ExportDTO.Username = KpiItemReport_KpiItemReportDTO.Username;
                KpiItemReport_ExportDTO.DisplayName = KpiItemReport_KpiItemReportDTO.DisplayName;
                KpiItemReport_ExportDTO.Items = new List<KpiItemReport_ItemExportDTO>();

                foreach (var KpiItemReport_KpiItemContentDTO in KpiItemReport_KpiItemReportDTO.ItemContents)
                {
                    KpiItemReport_ItemExportDTO KpiItemReport_LineDTO = new KpiItemReport_ItemExportDTO();
                    KpiItemReport_LineDTO.STT = KpiItemReport_KpiItemContentDTO.STT;
                    KpiItemReport_LineDTO.ItemCode = KpiItemReport_KpiItemContentDTO.ItemCode;
                    KpiItemReport_LineDTO.ItemName = KpiItemReport_KpiItemContentDTO.ItemName;
                    KpiItemReport_LineDTO.CriteriaContents = new List<KpiItemReport_CriteriaContentDTO>();

                    if (KpiCriteriaItemIds.Contains(KpiCriteriaItemEnum.INDIRECT_REVENUE.Id))
                    {
                        KpiItemReport_LineDTO.CriteriaContents.Add(new KpiItemReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaItemEnum.INDIRECT_REVENUE.Id,
                            CriteriaName = KpiCriteriaItemEnum.INDIRECT_REVENUE.Name,
                            Planned = KpiItemReport_KpiItemContentDTO.IndirectRevenuePlanned,
                            Result = KpiItemReport_KpiItemContentDTO.IndirectRevenue,
                            Ratio = KpiItemReport_KpiItemContentDTO.IndirectRevenueRatio
                        });
                    }
                    if (KpiCriteriaItemIds.Contains(KpiCriteriaItemEnum.INDIRECT_STORE.Id))
                    {
                        KpiItemReport_LineDTO.CriteriaContents.Add(new KpiItemReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaItemEnum.INDIRECT_STORE.Id,
                            CriteriaName = KpiCriteriaItemEnum.INDIRECT_STORE.Name,
                            Planned = KpiItemReport_KpiItemContentDTO.IndirectStorePlanned,
                            Result = KpiItemReport_KpiItemContentDTO.IndirectStore,
                            Ratio = KpiItemReport_KpiItemContentDTO.IndirectStoreRatio
                        });
                    }
                    if (KpiCriteriaItemIds.Contains(KpiCriteriaItemEnum.DIRECT_REVENUE.Id))
                    {
                        KpiItemReport_LineDTO.CriteriaContents.Add(new KpiItemReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaItemEnum.DIRECT_REVENUE.Id,
                            CriteriaName = KpiCriteriaItemEnum.DIRECT_REVENUE.Name,
                            Planned = KpiItemReport_KpiItemContentDTO.DirectRevenuePlanned,
                            Result = KpiItemReport_KpiItemContentDTO.DirectRevenue,
                            Ratio = KpiItemReport_KpiItemContentDTO.DirectRevenueRatio
                        });
                    }
                    if (KpiCriteriaItemIds.Contains(KpiCriteriaItemEnum.DIRECT_STORE.Id))
                    {
                        KpiItemReport_LineDTO.CriteriaContents.Add(new KpiItemReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaItemEnum.DIRECT_STORE.Id,
                            CriteriaName = KpiCriteriaItemEnum.DIRECT_STORE.Name,
                            Planned = KpiItemReport_KpiItemContentDTO.DirectStorePlanned,
                            Result = KpiItemReport_KpiItemContentDTO.DirectStore,
                            Ratio = KpiItemReport_KpiItemContentDTO.DirectStoreRatio
                        });
                    }

                    KpiItemReport_ExportDTO.Items.Add(KpiItemReport_LineDTO);
                }
                KpiItemReport_ExportDTOs.Add(KpiItemReport_ExportDTO);
            }

            return KpiItemReport_ExportDTOs;
        }

        private Tuple<DateTime, DateTime> DateTimeConvert(long KpiPeriodId, long KpiYearId)
        {
            DateTime startDate = StaticParams.DateTimeNow;
            DateTime endDate = StaticParams.DateTimeNow;
            if (KpiPeriodId <= Enums.KpiPeriodEnum.PERIOD_MONTH12.Id)
            {
                startDate = new DateTime((int)KpiYearId, (int)(KpiPeriodId % 100), 1);
                endDate = startDate.AddMonths(1).AddSeconds(-1);
            }
            else
            {
                if (KpiPeriodId == Enums.KpiPeriodEnum.PERIOD_QUATER01.Id)
                {
                    startDate = new DateTime((int)KpiYearId, 1, 1);
                    endDate = startDate.AddMonths(3).AddSeconds(-1);
                }
                if (KpiPeriodId == Enums.KpiPeriodEnum.PERIOD_QUATER02.Id)
                {
                    startDate = new DateTime((int)KpiYearId, 4, 1);
                    endDate = startDate.AddMonths(3).AddSeconds(-1);
                }
                if (KpiPeriodId == Enums.KpiPeriodEnum.PERIOD_QUATER03.Id)
                {
                    startDate = new DateTime((int)KpiYearId, 7, 1);
                    endDate = startDate.AddMonths(3).AddSeconds(-1);
                }
                if (KpiPeriodId == Enums.KpiPeriodEnum.PERIOD_QUATER04.Id)
                {
                    startDate = new DateTime((int)KpiYearId, 10, 1);
                    endDate = startDate.AddMonths(3).AddSeconds(-1);
                }
                if (KpiPeriodId == Enums.KpiPeriodEnum.PERIOD_YEAR01.Id)
                {
                    startDate = new DateTime((int)KpiYearId, 1, 1);
                    endDate = startDate.AddYears(1).AddSeconds(-1);
                }
            }

            return Tuple.Create(startDate.AddHours(0 - CurrentContext.TimeZone), endDate.AddHours(0 - CurrentContext.TimeZone));
        }

        public async Task<ActionResult<List<KpiItemReport_KpiItemReportDTO>>> List_Old([FromBody] KpiItemReport_KpiItemReportFilterDTO KpiItemReport_KpiItemReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (KpiItemReport_KpiItemReportFilterDTO.KpiPeriodId?.Equal.HasValue == false)
                return BadRequest(new { message = "Chưa chọn kì KPI" });
            if (KpiItemReport_KpiItemReportFilterDTO.KpiYearId?.Equal.HasValue == false)
                return BadRequest(new { message = "Chưa chọn năm KPI" });
            if (KpiItemReport_KpiItemReportFilterDTO.KpiItemTypeId?.Equal.HasValue == false)
                return BadRequest(new { message = "Chưa chọn loại KPI" });

            DateTime StartDate, EndDate;
            long? SaleEmployeeId = KpiItemReport_KpiItemReportFilterDTO.AppUserId?.Equal;
            long? ItemId = KpiItemReport_KpiItemReportFilterDTO.ItemId?.Equal;
            long? KpiPeriodId = KpiItemReport_KpiItemReportFilterDTO.KpiPeriodId?.Equal.Value;
            long? KpiYearId = KpiItemReport_KpiItemReportFilterDTO.KpiYearId?.Equal.Value;
            long? KpiItemTypeId = KpiItemReport_KpiItemReportFilterDTO.KpiItemTypeId?.Equal.Value;
            (StartDate, EndDate) = DateTimeConvert(KpiPeriodId.Value, KpiYearId.Value);
            if (KpiItemReport_KpiItemReportFilterDTO.OrderDate != null)
            {
                if (KpiItemReport_KpiItemReportFilterDTO.OrderDate.GreaterEqual != null)
                {
                    var StartDateFilter = KpiItemReport_KpiItemReportFilterDTO.OrderDate.GreaterEqual.Value.AddHours(0 - CurrentContext.TimeZone);
                    StartDate = StartDateFilter >= StartDate ? StartDateFilter : StartDate; // Don't allow get date out of Kpi period range
                }
                if (KpiItemReport_KpiItemReportFilterDTO.OrderDate.LessEqual != null)
                {
                    var EndDateFilter = KpiItemReport_KpiItemReportFilterDTO.OrderDate.LessEqual.Value.AddDays(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    EndDate = EndDateFilter <= EndDate ? EndDateFilter : EndDate; // Don't allow get date out of Kpi period range
                }
            }

            List<long> AppUserIds, OrganizationIds;
            (AppUserIds, OrganizationIds) = await FilterOrganizationAndUser(KpiItemReport_KpiItemReportFilterDTO.OrganizationId,
                AppUserService, OrganizationService, CurrentContext, DataContext);

            var query = from ki in DataContext.KpiItem
                        join au in DataContext.AppUser on ki.EmployeeId equals au.Id
                        join kic in DataContext.KpiItemContent on ki.Id equals kic.KpiItemId
                        join i in DataContext.Item on kic.ItemId equals i.Id
                        where OrganizationIds.Contains(ki.OrganizationId) &&
                        AppUserIds.Contains(au.Id) &&
                        (SaleEmployeeId.HasValue == false || ki.EmployeeId == SaleEmployeeId.Value) &&
                        (ItemId.HasValue == false || i.Id == ItemId.Value) &&
                        (ki.KpiPeriodId == KpiPeriodId.Value) &&
                        (ki.KpiYearId == KpiYearId.Value) &&
                        (ki.KpiItemTypeId == KpiItemTypeId.Value) &&
                        ki.DeletedAt == null &&
                        ki.StatusId == StatusEnum.ACTIVE.Id
                        select new
                        {
                            SaleEmployeeId = au.Id,
                            Username = au.Username,
                            DisplayName = au.DisplayName,
                            OrganizationId = au.OrganizationId,
                            ItemId = i.Id,
                            ItemCode = i.Code,
                            ItemName = i.Name,
                        };

            var ItemContents = await query.Distinct()
                .OrderBy(q => q.OrganizationId).ThenBy(x => x.DisplayName)
                .Skip(KpiItemReport_KpiItemReportFilterDTO.Skip)
                .Take(KpiItemReport_KpiItemReportFilterDTO.Take)
                .ToListWithNoLockAsync();

            List<long> SaleEmployeeIds = ItemContents.Select(x => x.SaleEmployeeId).Distinct().ToList();


            List<KpiItemReport_KpiItemReportDTO> KpiItemReport_KpiItemReportDTOs = new List<KpiItemReport_KpiItemReportDTO>();
            foreach (var EmployeeId in SaleEmployeeIds)
            {
                KpiItemReport_KpiItemReportDTO KpiItemReport_KpiItemReportDTO = new KpiItemReport_KpiItemReportDTO()
                {
                    SaleEmployeeId = EmployeeId,
                    DisplayName = ItemContents.Where(x => x.SaleEmployeeId == EmployeeId).Select(x => x.DisplayName).FirstOrDefault(),
                    Username = ItemContents.Where(x => x.SaleEmployeeId == EmployeeId).Select(x => x.Username).FirstOrDefault(),
                    ItemContents = ItemContents.Where(x => x.SaleEmployeeId == EmployeeId).Select(x => new KpiItemReport_KpiItemContentDTO
                    {
                        ItemId = x.ItemId,
                        SaleEmployeeId = EmployeeId,
                        ItemName = x.ItemName,
                        ItemCode = x.ItemCode,
                    })
                    .Where(x => x.SaleEmployeeId == EmployeeId)
                    .ToList()
                };
                KpiItemReport_KpiItemReportDTOs.Add(KpiItemReport_KpiItemReportDTO);
            }

            // lay du lieu bang mapping
            var query_detail = from km in DataContext.KpiItemContentKpiCriteriaItemMapping
                               join kc in DataContext.KpiItemContent on km.KpiItemContentId equals kc.Id
                               join k in DataContext.KpiItem on kc.KpiItemId equals k.Id
                               join i in DataContext.Item on kc.ItemId equals i.Id
                               where (SaleEmployeeIds.Contains(k.EmployeeId) &&
                                      k.KpiYearId == KpiYearId &&
                                      k.KpiPeriodId == KpiPeriodId &&
                                      k.KpiItemTypeId == KpiItemTypeId &&
                                      (ItemId == null || i.Id == ItemId)) &&
                                      km.Value.HasValue &&
                                      k.DeletedAt == null &&
                                      k.StatusId == StatusEnum.ACTIVE.Id
                               select new
                               {
                                   SaleEmployeeId = k.EmployeeId,
                                   KpiCriteriaItemId = km.KpiCriteriaItemId,
                                   Value = km.Value.Value,
                                   ItemId = i.Id,
                               };

            List<KpiItemReport_KpiItemContentKpiCriteriaItemMappingDTO>
                KpiItemReport_KpiItemContentKpiCriteriaItemMappingDTOs = (await query_detail.Distinct()
                .ToListWithNoLockAsync())
                .Select(x => new KpiItemReport_KpiItemContentKpiCriteriaItemMappingDTO
                {
                    SaleEmployeeId = x.SaleEmployeeId,
                    KpiCriteriaItemId = x.KpiCriteriaItemId,
                    Value = x.Value,
                    ItemId = x.ItemId,
                })
                .ToList();




            var IndirectSalesOrderDAOs = await DataContext.IndirectSalesOrder
                .Where(x => SaleEmployeeIds.Contains(x.SaleEmployeeId) &&
                x.OrderDate >= StartDate && x.OrderDate <= EndDate &&
                x.RequestStateId == RequestStateEnum.APPROVED.Id)
                .Select(x => new IndirectSalesOrderDAO
                {
                    Id = x.Id,
                    Total = x.Total,
                    SaleEmployeeId = x.SaleEmployeeId,
                    OrderDate = x.OrderDate,
                    BuyerStoreId = x.BuyerStoreId,
                    IndirectSalesOrderContents = x.IndirectSalesOrderContents.Select(c => new IndirectSalesOrderContentDAO
                    {
                        IndirectSalesOrderId = x.Id,
                        RequestedQuantity = c.RequestedQuantity,
                        ItemId = c.ItemId,
                        Amount = c.Amount,
                        GeneralDiscountAmount = c.GeneralDiscountAmount,
                        TaxAmount = c.TaxAmount
                    }).ToList(),
                })
                .ToListWithNoLockAsync();

            var DirectSalesOrderDAOs = await DataContext.DirectSalesOrder
                .Where(x => SaleEmployeeIds.Contains(x.SaleEmployeeId) &&
                x.OrderDate >= StartDate && x.OrderDate <= EndDate &&
                x.RequestStateId == RequestStateEnum.APPROVED.Id)
                .Select(x => new DirectSalesOrderDAO
                {
                    Id = x.Id,
                    Total = x.Total,
                    SaleEmployeeId = x.SaleEmployeeId,
                    OrderDate = x.OrderDate,
                    BuyerStoreId = x.BuyerStoreId,
                    DirectSalesOrderContents = x.DirectSalesOrderContents.Select(c => new DirectSalesOrderContentDAO
                    {
                        DirectSalesOrderId = x.Id,
                        RequestedQuantity = c.RequestedQuantity,
                        ItemId = c.ItemId,
                        Amount = c.Amount,
                        GeneralDiscountAmount = c.GeneralDiscountAmount,
                        TaxAmount = c.TaxAmount
                    }).ToList(),
                })
                .ToListWithNoLockAsync();

            foreach (var Employee in KpiItemReport_KpiItemReportDTOs)
            {
                foreach (var ItemContent in Employee.ItemContents)
                {
                    //lấy tất cả đơn hàng được thực hiện bởi nhân viên đang xét
                    var IndirectSalesOrders = IndirectSalesOrderDAOs
                            .Where(x => x.SaleEmployeeId == Employee.SaleEmployeeId)
                            .ToList();

                    var DirectSalesOrders = DirectSalesOrderDAOs
                           .Where(x => x.SaleEmployeeId == Employee.SaleEmployeeId)
                           .ToList();

                    #region Doanh thu theo đơn hàng gián tiếp
                    //kế hoạch
                    ItemContent.IndirectRevenuePlanned = KpiItemReport_KpiItemContentKpiCriteriaItemMappingDTOs
                            .Where(x => x.SaleEmployeeId == ItemContent.SaleEmployeeId &&
                            x.ItemId == ItemContent.ItemId &&
                            x.KpiCriteriaItemId == KpiCriteriaItemEnum.INDIRECT_REVENUE.Id)
                            .Select(x => x.Value).FirstOrDefault();

                    //thực hiện
                    if (ItemContent.IndirectRevenuePlanned.HasValue)
                    {
                        ItemContent.IndirectRevenue = 0;
                        foreach (var IndirectSalesOrder in IndirectSalesOrders)
                        {
                            foreach (var content in IndirectSalesOrder.IndirectSalesOrderContents)
                            {
                                if (content.ItemId == ItemContent.ItemId)
                                {
                                    ItemContent.IndirectRevenue += (content.Amount - content.GeneralDiscountAmount ?? 0);
                                }
                            }
                        }
                        ItemContent.IndirectRevenue = Math.Round(ItemContent.IndirectRevenue.Value, 0);
                        //tỉ lệ
                        ItemContent.IndirectRevenueRatio = ItemContent.IndirectRevenuePlanned == 0 || ItemContent.IndirectRevenuePlanned == null ?
                            null : (decimal?)
                            Math.Round((ItemContent.IndirectRevenue.Value / ItemContent.IndirectRevenuePlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Đại lý theo đơn hàng gián tiếp
                    //kế hoạch
                    ItemContent.IndirectStorePlanned = KpiItemReport_KpiItemContentKpiCriteriaItemMappingDTOs
                            .Where(x => x.SaleEmployeeId == ItemContent.SaleEmployeeId &&
                            x.ItemId == ItemContent.ItemId &&
                            x.KpiCriteriaItemId == KpiCriteriaItemEnum.INDIRECT_STORE.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    //thực hiện
                    if (ItemContent.IndirectStorePlanned.HasValue)
                    {
                        ItemContent.StoreIndirectIds = new HashSet<long>();
                        foreach (var IndirectSalesOrder in IndirectSalesOrders)
                        {
                            foreach (var content in IndirectSalesOrder.IndirectSalesOrderContents)
                            {
                                if (content.ItemId == ItemContent.ItemId)
                                    ItemContent.StoreIndirectIds.Add(IndirectSalesOrder.BuyerStoreId);
                            }
                        }
                        //tỉ lệ
                        ItemContent.IndirectStoreRatio = ItemContent.IndirectStorePlanned == null || ItemContent.IndirectStorePlanned == 0 ?
                            null : (decimal?)
                            Math.Round((ItemContent.IndirectStore.Value / ItemContent.IndirectStorePlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Sản lượng theo đơn hàng trực tiếp
                    //kế hoạch
                    //ItemContent.DirectQuantityPlanned = KpiItemReport_KpiItemContentKpiCriteriaItemMappingDTOs
                    //        .Where(x => x.SaleEmployeeId == ItemContent.SaleEmployeeId &&
                    //        x.ItemId == ItemContent.ItemId &&
                    //        x.KpiCriteriaItemId == KpiCriteriaItemEnum.DIRECT_QUANTITY.Id)
                    //        .Select(x => x.Value).FirstOrDefault();
                    ////thực hiện
                    //if (ItemContent.DirectQuantityPlanned.HasValue)
                    //{
                    //    ItemContent.DirectQuantity = 0;
                    //    foreach (var DirectSalesOrder in DirectSalesOrders)
                    //    {
                    //        foreach (var content in DirectSalesOrder.DirectSalesOrderContents)
                    //        {
                    //            if (content.ItemId == ItemContent.ItemId)
                    //            {
                    //                ItemContent.DirectQuantity += content.RequestedQuantity;
                    //            }
                    //        }
                    //    }
                    //    //tỉ lệ
                    //    ItemContent.DirectQuantityRatio = ItemContent.DirectQuantityPlanned == 0 || ItemContent.DirectQuantityPlanned == null ?
                    //        null : (decimal?)
                    //        Math.Round((ItemContent.DirectQuantity.Value / ItemContent.DirectQuantityPlanned.Value) * 100, 2);
                    //}
                    #endregion

                    #region Doanh thu theo đơn hàng trực tiếp
                    //kế hoạch
                    //ItemContent.DirectRevenuePlanned = KpiItemReport_KpiItemContentKpiCriteriaItemMappingDTOs
                    //        .Where(x => x.SaleEmployeeId == ItemContent.SaleEmployeeId &&
                    //        x.ItemId == ItemContent.ItemId &&
                    //        x.KpiCriteriaItemId == KpiCriteriaItemEnum.DIRECT_REVENUE.Id)
                    //        .Select(x => x.Value).FirstOrDefault();

                    ////thực hiện
                    //if (ItemContent.DirectRevenuePlanned.HasValue)
                    //{
                    //    ItemContent.DirectRevenue = 0;
                    //    foreach (var DirectSalesOrder in DirectSalesOrders)
                    //    {
                    //        foreach (var content in DirectSalesOrder.DirectSalesOrderContents)
                    //        {
                    //            if (content.ItemId == ItemContent.ItemId)
                    //            {
                    //                ItemContent.DirectRevenue += content.Amount;
                    //                ItemContent.DirectRevenue += content.TaxAmount ?? 0;
                    //                ItemContent.DirectRevenue -= content.GeneralDiscountAmount ?? 0;
                    //            }
                    //        }
                    //    }
                    //    ItemContent.DirectRevenue = Math.Round(ItemContent.DirectRevenue.Value, 0);

                    //    //tỉ lệ
                    //    ItemContent.DirectRevenueRatio = ItemContent.DirectRevenuePlanned == 0 || ItemContent.DirectRevenuePlanned == null ?
                    //        null : (decimal?)
                    //        Math.Round((ItemContent.DirectRevenue.Value / ItemContent.DirectRevenuePlanned.Value) * 100, 2);
                    //}
                    #endregion

                    #region Số đơn hàng trực tiếp
                    //kế hoạch
                    //ItemContent.DirectAmountPlanned = KpiItemReport_KpiItemContentKpiCriteriaItemMappingDTOs
                    //        .Where(x => x.SaleEmployeeId == ItemContent.SaleEmployeeId &&
                    //        x.ItemId == ItemContent.ItemId &&
                    //        x.KpiCriteriaItemId == KpiCriteriaItemEnum.DIRECT_AMOUNT.Id)
                    //        .Select(x => x.Value).FirstOrDefault();
                    ////thực hiện
                    //if (ItemContent.DirectAmountPlanned.HasValue)
                    //{
                    //    ItemContent.DirectSalesOrderIds = new HashSet<long>();
                    //    foreach (var DirectSalesOrder in DirectSalesOrders)
                    //    {
                    //        foreach (var content in DirectSalesOrder.DirectSalesOrderContents)
                    //        {
                    //            if (content.ItemId == ItemContent.ItemId)
                    //                ItemContent.DirectSalesOrderIds.Add(content.DirectSalesOrderId);
                    //        }
                    //    }
                    //    //tỉ lệ
                    //    ItemContent.DirectAmountRatio = ItemContent.DirectAmountPlanned == null || ItemContent.DirectAmountPlanned == 0 ?
                    //        null : (decimal?)
                    //        Math.Round((ItemContent.DirectAmount.Value / ItemContent.DirectAmountPlanned.Value) * 100, 2);
                    //}
                    #endregion

                    #region Đại lý theo đơn hàng trực tiếp
                    //kế hoạch
                    //ItemContent.DirectStorePlanned = KpiItemReport_KpiItemContentKpiCriteriaItemMappingDTOs
                    //        .Where(x => x.SaleEmployeeId == ItemContent.SaleEmployeeId &&
                    //        x.ItemId == ItemContent.ItemId &&
                    //        x.KpiCriteriaItemId == KpiCriteriaItemEnum.DIRECT_STORE.Id)
                    //        .Select(x => x.Value).FirstOrDefault();
                    ////thực hiện
                    //if (ItemContent.DirectStorePlanned.HasValue)
                    //{
                    //    ItemContent.StoreDirectIds = new HashSet<long>();
                    //    foreach (var DirectSalesOrder in DirectSalesOrders)
                    //    {
                    //        foreach (var content in DirectSalesOrder.DirectSalesOrderContents)
                    //        {
                    //            if (content.ItemId == ItemContent.ItemId)
                    //                ItemContent.StoreDirectIds.Add(DirectSalesOrder.BuyerStoreId);
                    //        }
                    //    }
                    //    //tỉ lệ
                    //    ItemContent.DirectStoreRatio = ItemContent.DirectStorePlanned == null || ItemContent.DirectStorePlanned == 0 ?
                    //        null : (decimal?)
                    //        Math.Round((ItemContent.DirectStore.Value / ItemContent.DirectStorePlanned.Value) * 100, 2);
                    //}
                    #endregion
                }
            };
            KpiItemReport_KpiItemReportDTOs = KpiItemReport_KpiItemReportDTOs.Where(x => x.ItemContents.Any()).ToList();
            return KpiItemReport_KpiItemReportDTOs;
        }
    }
}
