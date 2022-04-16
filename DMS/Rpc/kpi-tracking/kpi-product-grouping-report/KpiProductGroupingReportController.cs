using DMS.Common;
using DMS.DWModels;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MKpiPeriod;
using DMS.Services.MKpiProductGroupingCriteria;
using DMS.Services.MKpiProductGroupingType;
using DMS.Services.MKpiYear;
using DMS.Services.MOrganization;
using DMS.Services.MProduct;
using DMS.Services.MProductGrouping;
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

namespace DMS.Rpc.kpi_tracking.kpi_product_grouping_report
{
    public class KpiProductGroupingReportController : RpcController
    {
        private DataContext DataContext;
        private DWContext DWContext;
        private IOrganizationService OrganizationService;
        private IAppUserService AppUserService;
        private IKpiYearService KpiYearService;
        private IKpiPeriodService KpiPeriodService;
        private IItemService ItemService;
        private IKpiProductGroupingTypeService KpiProductGroupingTypeService;
        private IKpiProductGroupingCriteriaService KpiProductGroupingCriteriaService;
        private IProductGroupingService ProductGroupingService;
        private ICurrentContext CurrentContext;
        public KpiProductGroupingReportController(DataContext DataContext,
            DWContext DWContext,
            IOrganizationService OrganizationService,
            IAppUserService AppUserService,
            IKpiYearService KpiYearService,
            IKpiPeriodService KpiPeriodService,
            IItemService ItemService,
            IKpiProductGroupingTypeService KpiProductGroupingTypeService,
            IKpiProductGroupingCriteriaService KpiProductGroupingCriteriaService,
            IProductGroupingService ProductGroupingService,
            ICurrentContext CurrentContext)
        {
            this.DataContext = DataContext;
            this.DWContext = DWContext;
            this.OrganizationService = OrganizationService;
            this.AppUserService = AppUserService;
            this.KpiPeriodService = KpiPeriodService;
            this.KpiYearService = KpiYearService;
            this.ItemService = ItemService;
            this.KpiProductGroupingTypeService = KpiProductGroupingTypeService;
            this.KpiProductGroupingCriteriaService = KpiProductGroupingCriteriaService;
            this.ProductGroupingService = ProductGroupingService;
            this.CurrentContext = CurrentContext;
        }

        #region Filter List

        [Route(KpiProductGroupingReportRoute.ListKpiProductGroupingCriteria), HttpPost]
        public async Task<List<KpiProductGroupingReport_KpiProductGroupingCriteriaDTO>> ListKpiProductGroupingCriteria([FromBody] KpiProductGroupingReport_KpiProductGroupingCriteriaFilterDTO KpiProductGroupingReport_KpiProductGroupingCriteriaFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiProductGroupingCriteriaFilter KpiProductGroupingCriteriaFilter = new KpiProductGroupingCriteriaFilter();
            KpiProductGroupingCriteriaFilter.Skip = 0;
            KpiProductGroupingCriteriaFilter.Take = int.MaxValue;
            KpiProductGroupingCriteriaFilter.Selects = KpiProductGroupingCriteriaSelect.ALL;

            List<KpiProductGroupingCriteria> KpiProductGroupingCriterias = await KpiProductGroupingCriteriaService.List(KpiProductGroupingCriteriaFilter);
            List<KpiProductGroupingReport_KpiProductGroupingCriteriaDTO> KpiProductGroupingReport_KpiProductGroupingCriteriaDTOs = KpiProductGroupingCriterias
                .Select(x => new KpiProductGroupingReport_KpiProductGroupingCriteriaDTO(x)).ToList();
            return KpiProductGroupingReport_KpiProductGroupingCriteriaDTOs;
        }

        [Route(KpiProductGroupingReportRoute.FilterListAppUser), HttpPost]
        public async Task<List<KpiProductGroupingReport_AppUserDTO>> FilterListAppUser([FromBody] KpiProductGroupingReport_AppUserFilterDTO KpiProductGroupingReport_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.Id = KpiProductGroupingReport_AppUserFilterDTO.Id;
            AppUserFilter.OrganizationId = KpiProductGroupingReport_AppUserFilterDTO.OrganizationId;
            AppUserFilter.Username = KpiProductGroupingReport_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = KpiProductGroupingReport_AppUserFilterDTO.DisplayName;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<KpiProductGroupingReport_AppUserDTO> KpiProductGroupingReport_AppUserDTOs = AppUsers
                .Select(x => new KpiProductGroupingReport_AppUserDTO(x)).ToList();
            return KpiProductGroupingReport_AppUserDTOs;
        }

        [Route(KpiProductGroupingReportRoute.FilterListOrganization), HttpPost]
        public async Task<List<KpiProductGroupingReport_OrganizationDTO>> FilterListOrganization([FromBody] KpiProductGroupingReport_OrganizationFilterDTO KpiProductGroupingReport_OrganizationFilterDTO)
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
            List<KpiProductGroupingReport_OrganizationDTO> KpiProductGroupingReport_OrganizationDTOs = Organizations
                .Select(x => new KpiProductGroupingReport_OrganizationDTO(x)).ToList();
            return KpiProductGroupingReport_OrganizationDTOs;
        }

        [Route(KpiProductGroupingReportRoute.FilterListKpiPeriod), HttpPost]
        public async Task<List<KpiProductGroupingReport_KpiPeriodDTO>> FilterListKpiPeriod([FromBody] KpiProductGroupingReport_KpiPeriodFilterDTO KpiProductGroupingReport_KpiPeriodFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiPeriodFilter KpiPeriodFilter = new KpiPeriodFilter();
            KpiPeriodFilter.Skip = 0;
            KpiPeriodFilter.Take = 20;
            KpiPeriodFilter.OrderBy = KpiPeriodOrder.Id;
            KpiPeriodFilter.OrderType = OrderType.ASC;
            KpiPeriodFilter.Selects = KpiPeriodSelect.ALL;
            KpiPeriodFilter.Id = KpiProductGroupingReport_KpiPeriodFilterDTO.Id;
            KpiPeriodFilter.Code = KpiProductGroupingReport_KpiPeriodFilterDTO.Code;
            KpiPeriodFilter.Name = KpiProductGroupingReport_KpiPeriodFilterDTO.Name;

            List<KpiPeriod> KpiPeriods = await KpiPeriodService.List(KpiPeriodFilter);
            List<KpiProductGroupingReport_KpiPeriodDTO> KpiProductGroupingReport_KpiPeriodDTOs = KpiPeriods
                .Select(x => new KpiProductGroupingReport_KpiPeriodDTO(x)).ToList();
            return KpiProductGroupingReport_KpiPeriodDTOs;
        }

        [Route(KpiProductGroupingReportRoute.FilterListKpiYear), HttpPost]
        public async Task<List<KpiProductGroupingReport_KpiYearDTO>> FilterListKpiYear([FromBody] KpiProductGroupingReport_KpiYearFilterDTO KpiProductGroupingReport_KpiYearFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiYearFilter KpiYearFilter = new KpiYearFilter();
            KpiYearFilter.Skip = 0;
            KpiYearFilter.Take = 20;
            KpiYearFilter.OrderBy = KpiYearOrder.Id;
            KpiYearFilter.OrderType = OrderType.ASC;
            KpiYearFilter.Selects = KpiYearSelect.ALL;
            KpiYearFilter.Id = KpiProductGroupingReport_KpiYearFilterDTO.Id;
            KpiYearFilter.Code = KpiProductGroupingReport_KpiYearFilterDTO.Code;
            KpiYearFilter.Name = KpiProductGroupingReport_KpiYearFilterDTO.Name;

            List<KpiYear> KpiYears = await KpiYearService.List(KpiYearFilter);
            List<KpiProductGroupingReport_KpiYearDTO> KpiProductGroupingReport_KpiYearDTOs = KpiYears
                .Select(x => new KpiProductGroupingReport_KpiYearDTO(x)).ToList();
            return KpiProductGroupingReport_KpiYearDTOs;
        }

        [Route(KpiProductGroupingReportRoute.FilterListKpiProductGroupingType), HttpPost]
        public async Task<List<KpiProductGroupingReport_KpiProductGroupingTypeDTO>> FilterListKpiProductGroupingType([FromBody] KpiProductGroupingReport_KpiProductGroupingTypeFilterDTO KpiProductGroupingReport_KpiProductGroupingTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiProductGroupingTypeFilter KpiProductGroupingTypeFilter = new KpiProductGroupingTypeFilter();
            KpiProductGroupingTypeFilter.Skip = 0;
            KpiProductGroupingTypeFilter.Take = 20;
            KpiProductGroupingTypeFilter.OrderBy = KpiProductGroupingTypeOrder.Id;
            KpiProductGroupingTypeFilter.OrderType = OrderType.ASC;
            KpiProductGroupingTypeFilter.Selects = KpiProductGroupingTypeSelect.ALL;
            KpiProductGroupingTypeFilter.Id = KpiProductGroupingReport_KpiProductGroupingTypeFilterDTO.Id;
            KpiProductGroupingTypeFilter.Code = KpiProductGroupingReport_KpiProductGroupingTypeFilterDTO.Code;
            KpiProductGroupingTypeFilter.Name = KpiProductGroupingReport_KpiProductGroupingTypeFilterDTO.Name;

            List<KpiProductGroupingType> KpiProductGroupingTypes = await KpiProductGroupingTypeService.List(KpiProductGroupingTypeFilter);
            List<KpiProductGroupingReport_KpiProductGroupingTypeDTO> KpiProductGroupingReport_KpiProductGroupingTypeDTOs = KpiProductGroupingTypes
                .Select(x => new KpiProductGroupingReport_KpiProductGroupingTypeDTO(x)).ToList();
            return KpiProductGroupingReport_KpiProductGroupingTypeDTOs;
        }


        [Route(KpiProductGroupingReportRoute.FilterListProductGrouping), HttpPost]
        public async Task<List<KpiProductGroupingReport_ProductGroupingDTO>> FilterListProductGrouping([FromBody] KpiProductGroupingReport_ProductGroupingFilterDTO KpiProductGroupingReport_ProductGroupingFilterDTO)
        {
            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter();
            ProductGroupingFilter.Skip = 0;
            ProductGroupingFilter.Take = int.MaxValue;
            ProductGroupingFilter.OrderBy = ProductGroupingOrder.Id;
            ProductGroupingFilter.OrderType = OrderType.ASC;
            ProductGroupingFilter.Selects = ProductGroupingSelect.Id | ProductGroupingSelect.Code
                | ProductGroupingSelect.Name | ProductGroupingSelect.Parent;

            ProductGroupingFilter.Id = KpiProductGroupingReport_ProductGroupingFilterDTO.Id;
            ProductGroupingFilter.Code = KpiProductGroupingReport_ProductGroupingFilterDTO.Code;
            ProductGroupingFilter.Name = KpiProductGroupingReport_ProductGroupingFilterDTO.Name;

            List<ProductGrouping> ProductGroupings = await ProductGroupingService.List(ProductGroupingFilter);
            List<KpiProductGroupingReport_ProductGroupingDTO> KpiProductGroupingReport_ProductGroupingDTOs = ProductGroupings
                .Select(x => new KpiProductGroupingReport_ProductGroupingDTO(x)).ToList();
            return KpiProductGroupingReport_ProductGroupingDTOs;
        }
        #endregion

        [Route(KpiProductGroupingReportRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] KpiProductGroupingReport_KpiProductGroupingReportFilterDTO KpiProductGroupingReport_KpiProductGroupingReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            #region tính thời gian bắt đầu, kết thúc, lấy ra Id nhân viên và orgUnit từ filter

            long? SaleEmployeeId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.AppUserId?.Equal;
            long? ProductGroupingId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.ProductGroupingId?.Equal;
            if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiPeriodId?.Equal.HasValue == false ||
                KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiYearId?.Equal.HasValue == false)
                return 0;
            long? KpiPeriodId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiPeriodId?.Equal.Value;
            long? KpiYearId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiYearId?.Equal.Value;
            long? KpiProductGroupingTypeId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiProductGroupingTypeId?.Equal.Value;

            List<long> AppUserIds, OrganizationIds;
            (AppUserIds, OrganizationIds) = await FilterOrganizationAndUser(KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrganizationId,
                AppUserService, OrganizationService, CurrentContext, DataContext);
            #endregion

            var query = from kpg in DataContext.KpiProductGrouping
                        join kpgc in DataContext.KpiProductGroupingContent on kpg.Id equals kpgc.KpiProductGroupingId
                        join pg in DataContext.ProductGrouping on kpgc.ProductGroupingId equals pg.Id
                        where OrganizationIds.Contains(kpg.OrganizationId) &&
                        AppUserIds.Contains(kpg.EmployeeId) &&
                        (SaleEmployeeId.HasValue == false || kpg.EmployeeId == SaleEmployeeId.Value) &&
                        (ProductGroupingId.HasValue == false || pg.Id == ProductGroupingId.Value) &&
                        (kpg.KpiPeriodId == KpiPeriodId.Value) &&
                        (kpg.KpiYearId == KpiYearId) &&
                        (KpiProductGroupingTypeId.HasValue == false || kpg.KpiProductGroupingTypeId == KpiProductGroupingTypeId.Value) &&
                        kpg.DeletedAt == null &&
                        kpg.StatusId == StatusEnum.ACTIVE.Id
                        select new
                        {
                            OrganizationId = kpg.OrganizationId,
                            SaleEmployeeId = kpg.EmployeeId,
                        }; // grouping kpi nhóm sản phẩm theo Organization và ProductGrouping
            return await query.Distinct().CountWithNoLockAsync();
        }

        [Route(KpiProductGroupingReportRoute.List), HttpPost]
        public async Task<ActionResult<List<KpiProductGroupingReport_KpiProductGroupingReportDTO>>> List([FromBody] KpiProductGroupingReport_KpiProductGroupingReportFilterDTO KpiProductGroupingReport_KpiProductGroupingReportFilterDTO)
        {
            try
            {
                #region validate dữ liệu filter bắt buộc có 
                if (!ModelState.IsValid)
                    throw new BindException(ModelState);
                if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiPeriodId?.Equal.HasValue == false)
                    return BadRequest(new { message = "Chưa chọn kì KPI" });
                if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiYearId?.Equal.HasValue == false)
                    return BadRequest(new { message = "Chưa chọn năm KPI" });
                if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiProductGroupingTypeId?.Equal.HasValue == false)
                    return BadRequest(new { message = "Chưa chọn loại KPI" });
                #endregion

                #region tính thời gian bắt đầu, kết thúc, lấy ra Id nhân viên và orgUnit từ filter
                DateTime StartDate, EndDate;
                long? SaleEmployeeId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.AppUserId?.Equal;
                long? ProductGroupingId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.ProductGroupingId?.Equal;
                long? KpiPeriodId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiPeriodId?.Equal.Value;
                long? KpiYearId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiYearId?.Equal.Value;
                long? KpiProductGroupingTypeId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiProductGroupingTypeId?.Equal.Value;
                (StartDate, EndDate) = DateTimeConvert(KpiPeriodId.Value, KpiYearId.Value);
                (StartDate, EndDate) = DateTimeConvert(KpiPeriodId.Value, KpiYearId.Value);
                if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrderDate != null)
                {
                    if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrderDate.GreaterEqual != null)
                    {
                        var StartDateFilter = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrderDate.GreaterEqual.Value;
                        StartDate = StartDateFilter >= StartDate ? StartDateFilter : StartDate; // Don't allow get date out of Kpi period range
                    }
                    if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrderDate.LessEqual != null)
                    {
                        var EndDateFilter = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrderDate.LessEqual.Value;
                        EndDate = EndDateFilter <= EndDate ? EndDateFilter : EndDate; // Don't allow get date out of Kpi period range
                    }
                }

                IdFilter SaleEmployeeIdFilter = new IdFilter() { Equal = SaleEmployeeId };
                DateFilter OrderDateFilter = new DateFilter() { GreaterEqual = StartDate, LessEqual = EndDate };

                List<long> AppUserIds, OrganizationIds;
                (AppUserIds, OrganizationIds) = await FilterOrganizationAndUser(KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrganizationId,
                    AppUserService, OrganizationService, CurrentContext, DataContext);
                #endregion

                #region lấy dữ liệu báo cáo
                var query = from kpg in DataContext.KpiProductGrouping
                            join au in DataContext.AppUser on kpg.EmployeeId equals au.Id
                            join kpgc in DataContext.KpiProductGroupingContent on kpg.Id equals kpgc.KpiProductGroupingId
                            join pg in DataContext.ProductGrouping on kpgc.ProductGroupingId equals pg.Id
                            where OrganizationIds.Contains(kpg.OrganizationId) &&
                            AppUserIds.Contains(kpg.EmployeeId) &&
                            (SaleEmployeeId.HasValue == false || kpg.EmployeeId == SaleEmployeeId.Value) &&
                            (ProductGroupingId.HasValue == false || pg.Id == ProductGroupingId.Value) &&
                            (kpg.KpiPeriodId == KpiPeriodId.Value) &&
                            (kpg.KpiYearId == KpiYearId) &&
                            (KpiProductGroupingTypeId.HasValue == false || kpg.KpiProductGroupingTypeId == KpiProductGroupingTypeId.Value) &&
                            kpg.DeletedAt == null &&
                            kpg.StatusId == StatusEnum.ACTIVE.Id
                            select new
                            {
                                KpiProductGroupingId = kpg.Id,
                                OrganizationId = kpg.OrganizationId,
                                SaleEmployeeId = kpg.EmployeeId,
                                Username = au.Username,
                                DisplayName = au.DisplayName,
                                ProductGroupingId = pg.Id,
                                ProductGroupingCode = pg.Code,
                                ProductGroupingName = pg.Name,
                            }; // grouping kpi nhóm sản phẩm theo Organization và ProductGrouping

                var datas = await query.Distinct()
                    .OrderBy(x => x.OrganizationId)
                    .ThenBy(x => x.DisplayName)
                    .Skip(KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.Skip)
                    .Take(KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.Take)
                    .ToListWithNoLockAsync(); // lấy ra toàn bộ dữ liệu theo filter
                var KpiProductGroupingIds = datas
                    .Select(x => x.KpiProductGroupingId)
                    .Distinct().ToList();
                OrganizationIds = datas
                    .Select(x => x.OrganizationId)
                    .Distinct().ToList();
                AppUserIds = datas
                    .Select(x => x.SaleEmployeeId)
                    .Distinct().ToList();
                var OrganizationDAOs = await DataContext.Organization.AsNoTracking()
                    .Where(x => OrganizationIds.Contains(x.Id))
                    .OrderBy(x => x.Id)
                    .Select(x => new OrganizationDAO
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToListWithNoLockAsync(); // lấy ra toàn bộ Org trong danh sách phân trang
                var AppUserDAOs = await DataContext.AppUser.AsNoTracking()
                    .Where(x => AppUserIds.Contains(x.Id))
                    .OrderBy(x => x.Id)
                    .Select(x => new AppUserDAO
                    {
                        Id = x.Id,
                        Username = x.Username,
                        DisplayName = x.DisplayName,
                        OrganizationId = x.OrganizationId
                    }).ToListWithNoLockAsync(); // lấy ra toàn bộ Nhân viên trong danh sách phân trang

                var query_content = from km in DataContext.KpiProductGroupingContentCriteriaMapping
                                    join kc in DataContext.KpiProductGroupingContent on km.KpiProductGroupingContentId equals kc.Id
                                    join k in DataContext.KpiProductGrouping on kc.KpiProductGroupingId equals k.Id
                                    join pg in DataContext.ProductGrouping on kc.ProductGroupingId equals pg.Id
                                    where KpiProductGroupingIds.Contains(k.Id) &&
                                    (ProductGroupingId.HasValue == false || pg.Id == ProductGroupingId.Value)
                                    select new
                                    {
                                        SaleEmployeeId = k.EmployeeId,
                                        ProductGroupingId = kc.ProductGroupingId,
                                        KpiProductGroupingContentId = kc.Id,
                                        KpiProductGroupingCriteriaId = km.KpiProductGroupingCriteriaId,
                                        Value = km.Value,
                                        SelectAllCurrentItem = kc.SelectAllCurrentItem,
                                    };

                List<KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTO> KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTOs =
                    await query_content.Distinct().Select(x => new KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTO
                    {
                        SaleEmployeeId = x.SaleEmployeeId,
                        ProductGroupingId = x.ProductGroupingId,
                        KpiProductGroupingContentId = x.KpiProductGroupingContentId,
                        KpiProductGroupingCriteriaId = x.KpiProductGroupingCriteriaId,
                        SelectAllCurrentItem = x.SelectAllCurrentItem,
                        Value = x.Value,
                    }).ToListWithNoLockAsync();

                List<long> KpiProductGroupingContentIds = KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTOs
                    .Select(x => x.KpiProductGroupingContentId)
                    .Distinct()
                    .ToList();
                List<long> ProductGroupingIds = KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTOs
                    .Select(x => x.ProductGroupingId)
                    .Distinct().ToList();
                List<long> ProductGroupingSelectAllItemIds = KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTOs
                    .Where(x => x.SelectAllCurrentItem).Select(x => x.ProductGroupingId)
                    .Distinct().ToList();
                List<ProductGroupingDAO> ProductGroupingDAOs = await DataContext.ProductGrouping.AsNoTracking()
                    .Where(x => ProductGroupingIds.Contains(x.Id))
                    .Select(x => new ProductGroupingDAO
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                    })
                    .ToListWithNoLockAsync();
                List<KpiProductGroupingContentItemMappingDAO> KpiProductGroupingContentItemMappingDAOs = await DataContext.KpiProductGroupingContentItemMapping.AsNoTracking()
                    .Where(x => KpiProductGroupingContentIds.Contains(x.KpiProductGroupingContentId))
                    .Select(x => new KpiProductGroupingContentItemMappingDAO
                    {
                        ItemId = x.ItemId,
                        KpiProductGroupingContentId = x.KpiProductGroupingContentId,
                    })
                    .ToListWithNoLockAsync(); // lay ra toan bo itemId duoc map voi contentIds
                List<long> ItemIds = KpiProductGroupingContentItemMappingDAOs
                    .Select(x => x.ItemId)
                    .Distinct()
                    .ToList();
                var ProductProductGroupingMapping_query = DataContext.ProductProductGroupingMapping.AsNoTracking();
                ProductProductGroupingMapping_query = ProductProductGroupingMapping_query.Where(x => x.ProductGroupingId, new IdFilter { In = ProductGroupingSelectAllItemIds });
                if(KpiProductGroupingTypeId == KpiProductGroupingTypeEnum.NEW_PRODUCT_GROUPING.Id )
                    ProductProductGroupingMapping_query = ProductProductGroupingMapping_query.Where(x => x.Product.IsNew);
                var ProductProductGroupingMapping = await ProductProductGroupingMapping_query
                    .Select(x => new {x.ProductGroupingId, x.ProductId}).ToListWithNoLockAsync();

                var ItemOfProductGroupingSelectAllItems = await DataContext.Item.AsNoTracking()
                    .Where(x => x.ProductId, new IdFilter { In = ProductProductGroupingMapping.Select(x => x.ProductId).Distinct().ToList() })
                    .Select(x => new
                    {
                        x.ProductId,
                        x.Id
                    }).Distinct().ToListWithNoLockAsync();
                ItemIds.AddRange(ItemOfProductGroupingSelectAllItems.Select(x => x.Id).Distinct().ToList());

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
                IdFilter ItemIdFilter = new IdFilter() { In = ItemIds };
                var indirect_sales_order_transaction = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
                indirect_sales_order_transaction = indirect_sales_order_transaction.Where(q => q.OrderDate, OrderDateFilter);
                indirect_sales_order_transaction = indirect_sales_order_transaction.Where(q => q.SalesEmployeeId, SaleEmployeeIdFilter);
                indirect_sales_order_transaction = indirect_sales_order_transaction.Where(q => q.RequestStateId, new IdFilter() { Equal = RequestStateEnum.APPROVED.Id });
                indirect_sales_order_transaction = indirect_sales_order_transaction.Where(q => q.ItemId, ItemIdFilter);
                indirect_sales_order_transaction = indirect_sales_order_transaction.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id });
                indirect_sales_order_transaction = indirect_sales_order_transaction.Where(x => !x.DeletedAt.HasValue);

                var InDirectSalesOrderTransactionDAOs = await indirect_sales_order_transaction.Distinct().ToListWithNoLockAsync();
                #endregion

                #region tổng hợp dữ liệu báo cáo
                List<KpiProductGroupingReport_KpiProductGroupingReportDTO> KpiProductGroupingReport_KpiProductGroupingReportDTOs = new List<KpiProductGroupingReport_KpiProductGroupingReportDTO>();
                foreach (var Organization in OrganizationDAOs)
                {
                    KpiProductGroupingReport_KpiProductGroupingReportDTO KpiProductGroupingReport_KpiProductGroupingReportDTO = new KpiProductGroupingReport_KpiProductGroupingReportDTO
                    {
                        OrganizationId = Organization.Id,
                        OrganizationName = Organization.Name,
                        SaleEmployees = new List<KpiProductGroupingReport_KpiSaleEmployeetDTO>(),
                    };
                    KpiProductGroupingReport_KpiProductGroupingReportDTO.SaleEmployees = datas.Where(x => x.OrganizationId == Organization.Id).Select(x => new KpiProductGroupingReport_KpiSaleEmployeetDTO
                    {
                        Id = x.SaleEmployeeId,
                        UserName = x.Username,
                        DisplayName = x.DisplayName,
                        OrganizationId = x.OrganizationId,
                    }).Distinct().ToList();
                    KpiProductGroupingReport_KpiProductGroupingReportDTOs.Add(KpiProductGroupingReport_KpiProductGroupingReportDTO);
                }

                foreach (var Organization in KpiProductGroupingReport_KpiProductGroupingReportDTOs)
                {
                    foreach (var Employee in Organization.SaleEmployees)
                    {

                        Employee.Contents = new List<KpiProductGroupingReport_KpiProductGroupingContentDTO>();
                        List<KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTO> ContentCriteriaMappings = KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTOs
                            .Where(x => x.SaleEmployeeId == Employee.Id)
                            .ToList();
                        List<long> SubProductGroupingIds = ContentCriteriaMappings.Select(x => x.ProductGroupingId)
                            .Distinct()
                            .ToList();
                        Dictionary<long, bool> DictionarySubProductGrouping = ContentCriteriaMappings
                            .Select(x => new {x.ProductGroupingId, x.SelectAllCurrentItem}).Distinct()
                            .ToDictionary(x => x.ProductGroupingId, y => y.SelectAllCurrentItem);
                        List<long> SubkpiProductGroupingContentIds = ContentCriteriaMappings.Select(x => x.KpiProductGroupingContentId)
                            .Distinct()
                            .ToList();
                        List<ProductGroupingDAO> SubProductGroupings = ProductGroupingDAOs
                            .Where(x => SubProductGroupingIds.Contains(x.Id))
                            .ToList();
                        foreach (var ProductGrouping in SubProductGroupings)
                        {
                            KpiProductGroupingReport_KpiProductGroupingContentDTO Content = new KpiProductGroupingReport_KpiProductGroupingContentDTO();
                            Content.ProductGroupingId = ProductGrouping.Id;
                            Content.ProductGroupingCode = ProductGrouping.Code;
                            Content.ProductGroupingName = ProductGrouping.Name;
                            Content.SaleEmployeeId = Employee.Id;

                            List<KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTO> SubContentCriteriaMappings = ContentCriteriaMappings
                                .Where(x => x.ProductGroupingId == ProductGrouping.Id)
                                .ToList();
                            List<long> SubContentIds = SubContentCriteriaMappings.Select(x => x.KpiProductGroupingContentId)
                                .Distinct()
                                .ToList();
                            List<long> SubProductIds = ProductProductGroupingMapping.Where(x => x.ProductGroupingId == ProductGrouping.Id)
                                .Select(x => x.ProductId).ToList();
                            List<long> SubItemIds = new List<long>();
                            if (DictionarySubProductGrouping[ProductGrouping.Id])
                            {
                                SubItemIds = ItemOfProductGroupingSelectAllItems.Where(x => SubProductIds.Contains(x.ProductId))
                                                                    .Select(x => x.Id)
                                                                    .Distinct()
                                                                    .ToList();
                            }
                            else
                            {
                                SubItemIds = KpiProductGroupingContentItemMappingDAOs.Where(x => SubContentIds.Contains(x.KpiProductGroupingContentId))
                                    .Select(x => x.ItemId)
                                    .Distinct()
                                    .ToList();
                            }
                            var SubIndirectTransactions = InDirectSalesOrderTransactionDAOs
                                .Where(x => x.SalesEmployeeId == Employee.Id)
                                .Where(x => SubItemIds.Contains(x.ItemId))
                                .ToList(); // lay transaction don gian tiep theo SalesEmployee va Item
                            var SubDirectTransactions = DirectSalesOrderTransactionDAOs
                                .Where(x => x.SalesEmployeeId == Employee.Id)
                                .Where(x => SubItemIds.Contains(x.ItemId))
                                .ToList(); // lay transaction don gian tiep theo SalesEmployee va Item

                            #region thống kê doanh thu đơn trực tiếp
                            Content.DirectRevenuePlanned = ContentCriteriaMappings
                                .Where(x => x.ProductGroupingId == ProductGrouping.Id)
                                .Where(x => x.KpiProductGroupingCriteriaId == KpiProductGroupingCriteriaEnum.DIRECT_REVENUE.Id)
                                .Where(x => x.SaleEmployeeId == Employee.Id)
                                .Select(x => x.Value)
                                .FirstOrDefault(); // lấy ra giá trị kế hoạch theo nhóm sản phẩm, user và loại kpi
                            // Thực hiện
                            if (Content.DirectRevenuePlanned.HasValue)
                            {
                                Content.DirectRevenue = SubDirectTransactions == null ? 0 : SubDirectTransactions
                                    .Sum(x => (x.Amount - x.GeneralDiscountAmount ?? 0));
                                Content.DirectRevenue = Math.Round(Content.DirectRevenue.Value, 0);
                                // Tỷ lệ
                                Content.DirectRevenueRatio = Content.DirectRevenuePlanned == 0 || Content.DirectRevenuePlanned == null || Content.DirectRevenue == null ?
                                    null : (decimal?)
                                    Math.Round((Content.DirectRevenue.Value / Content.DirectRevenuePlanned.Value) * 100, 2);
                            }
                            #endregion

                            #region thống kê số đại lý đơn trực tiếp
                            Content.DirectStorePlanned = ContentCriteriaMappings
                                .Where(x => x.ProductGroupingId == ProductGrouping.Id)
                                .Where(x => x.KpiProductGroupingCriteriaId == KpiProductGroupingCriteriaEnum.DIRECT_STORE.Id)
                                .Where(x => x.SaleEmployeeId == Employee.Id)
                                .Select(x => x.Value)
                                .FirstOrDefault(); // lấy ra giá trị kế hoạch theo nhóm sản phẩm, user và loại kpi

                            if (Content.DirectStorePlanned.HasValue)
                            {
                                var BuyStoreIds = SubDirectTransactions
                                    .Select(x => x.BuyerStoreId)
                                    .Distinct().ToList();
                                Content.StoreDirectIds = new HashSet<long>(BuyStoreIds);
                                if (BuyStoreIds == null) Content.StoreDirectIds = new HashSet<long>();
                                Content.DirectStoreRatio = Content.DirectStorePlanned == 0 || Content.DirectStorePlanned == null || Content.DirectStore == null ?
                                    null : (decimal?)
                                    Math.Round((Content.DirectStore.Value / Content.DirectStorePlanned.Value) * 100, 2);
                            }
                            #endregion

                            #region thống kê doanh thu đơn gián tiếp
                            Content.IndirectRevenuePlanned = ContentCriteriaMappings
                                .Where(x => x.ProductGroupingId == ProductGrouping.Id)
                                .Where(x => x.KpiProductGroupingCriteriaId == KpiProductGroupingCriteriaEnum.INDIRECT_REVENUE.Id)
                                .Where(x => x.SaleEmployeeId == Employee.Id)
                                .Select(x => x.Value)
                                .FirstOrDefault(); // lấy ra giá trị kế hoạch theo nhóm sản phẩm, user và loại kpi
                            // Thực hiện
                            if (Content.IndirectRevenuePlanned.HasValue)
                            {
                                Content.IndirectRevenue = SubIndirectTransactions == null ? 0 : SubIndirectTransactions
                                    .Sum(x => (x.Amount - x.GeneralDiscountAmount ?? 0));
                                Content.IndirectRevenue = Math.Round(Content.IndirectRevenue.Value, 0);
                                // Tỷ lệ
                                Content.IndirectRevenueRatio = Content.IndirectRevenuePlanned == 0 || Content.IndirectRevenuePlanned == null || Content.IndirectRevenue == null ?
                                    null : (decimal?)
                                    Math.Round((Content.IndirectRevenue.Value / Content.IndirectRevenuePlanned.Value) * 100, 2);
                            }
                            #endregion

                            #region thống kê số đại lý đơn gián tiếp
                            Content.IndirectStorePlanned = ContentCriteriaMappings
                                .Where(x => x.ProductGroupingId == ProductGrouping.Id)
                                .Where(x => x.KpiProductGroupingCriteriaId == KpiProductGroupingCriteriaEnum.INDIRECT_STORE.Id)
                                .Where(x => x.SaleEmployeeId == Employee.Id)
                                .Select(x => x.Value)
                                .FirstOrDefault(); // lấy ra giá trị kế hoạch theo nhóm sản phẩm, user và loại kpi

                            if (Content.IndirectStorePlanned.HasValue)
                            {
                                var BuyStoreIds = SubIndirectTransactions
                                    .Select(x => x.BuyerStoreId)
                                    .Distinct().ToList();
                                Content.StoreIndirectIds = new HashSet<long>(BuyStoreIds);
                                if (BuyStoreIds == null) Content.StoreIndirectIds = new HashSet<long>();
                                Content.IndirectStoreRatio = Content.IndirectStorePlanned == 0 || Content.IndirectStorePlanned == null || Content.IndirectStore == null ?
                                    null : (decimal?)
                                    Math.Round((Content.IndirectStore.Value / Content.IndirectStorePlanned.Value) * 100, 2);
                            }
                            #endregion

                            Employee.Contents.Add(Content); // thêm content
                        }
                    }
                }
                #endregion

                return KpiProductGroupingReport_KpiProductGroupingReportDTOs;
            }
            catch (Exception Exception)
            {

                throw Exception;
            }
        }

        [Route(KpiProductGroupingReportRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] KpiProductGroupingReport_KpiProductGroupingReportFilterDTO KpiProductGroupingReport_KpiProductGroupingReportFilterDTO)
        {
            #region validate dữ liệu filter bắt buộc có 
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiPeriodId?.Equal == null)
                return BadRequest(new { message = "Chưa chọn kì KPI" });
            if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiYearId?.Equal == null)
                return BadRequest(new { message = "Chưa chọn năm KPI" });
            if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiProductGroupingTypeId?.Equal == null)
                return BadRequest(new { message = "Chưa chọn loại KPI" });
            #endregion

            #region tính thời gian bắt đầu, kết thúc, lấy ra Id nhân viên và orgUnit từ filter
            var KpiPeriod = KpiPeriodEnum.KpiPeriodEnumList
                .Where(x => x.Id == KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiPeriodId.Equal.Value)
                .FirstOrDefault();
            var KpiYear = KpiYearEnum.KpiYearEnumList
                .Where(x => x.Id == KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiYearId.Equal.Value)
                .FirstOrDefault();
            var KpiProductGroupingType = KpiProductGroupingTypeEnum.KpiProductGroupingTypeEnumList
                .Where(x => x.Id == KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiProductGroupingTypeId.Equal.Value)
                .FirstOrDefault();

            DateTime StartDate, EndDate;
            long KpiPeriodId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiPeriodId?.Equal ?? 100 + StaticParams.DateTimeNow.AddHours(0 - CurrentContext.TimeZone).Month;
            long KpiYearId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiYearId?.Equal ?? StaticParams.DateTimeNow.AddHours(0 - CurrentContext.TimeZone).Year;
            (StartDate, EndDate) = DateTimeConvert(KpiPeriodId, KpiYearId);
            if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrderDate.HasValue)
            {
                StartDate = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrderDate.GreaterEqual == null ? StartDate : KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrderDate.GreaterEqual.Value;
                EndDate = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrderDate.GreaterEqual == null ? EndDate : KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrderDate.LessEqual.Value;
            }

            KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.Skip = 0;
            KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.Take = int.MaxValue;
            List<KpiProductGroupingReport_KpiProductGroupingReportDTO> KpiProductGroupingReport_KpiProductGroupingReportDTOs = (await List(KpiProductGroupingReport_KpiProductGroupingReportFilterDTO)).Value;
            long stt = 1;
            foreach (var KpiProductGroupingReport_KpiProductGroupingReportDTO in KpiProductGroupingReport_KpiProductGroupingReportDTOs)
            {
                foreach (var employee in KpiProductGroupingReport_KpiProductGroupingReportDTO.SaleEmployees)
                {
                    foreach (var content in employee.Contents)
                    {
                        content.STT = stt;
                        stt++;
                    }
                }
            }
            #endregion

            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            List<KpiProductGroupingReport_ExportDTO> KpiProductGroupingReport_ExportDTOs = await ConvertReportToExportDTO(KpiProductGroupingReport_KpiProductGroupingReportDTOs);

            List<KpiProductGroupingCriteria> KpiProductGroupingCriterias = await KpiProductGroupingCriteriaService.List(new KpiProductGroupingCriteriaFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiProductGroupingCriteriaSelect.ALL,
            });

            MemoryStream output = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(output))
            {
                #region Tieu de bao cao
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add("Báo cáo KPI nhóm sản phẩm");
                ws.Cells.Style.Font.Name = "Times New Roman";
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                ws.Cells["A1"].Value = OrgRoot.Name.ToUpper();
                ws.Cells["A1"].Style.Font.Size = 11;
                ws.Cells["A1"].Style.Font.Bold = true;

                ws.Cells["A3"].Value = "BÁO CÁO KPI NHÓM SẢN PHẨM";

                ws.Cells["E4"].Value = KpiProductGroupingType.Name;
                ws.Cells["E4"].Style.Font.Italic = true;
                ws.Cells["E4"].Style.Font.Bold = true;
                ws.Cells["E4:F4"].Merge = true;

                ws.Cells["G4"].Value = $"Kỳ KPI: {KpiPeriod.Name} - Năm: {KpiYear.Name}";
                ws.Cells["G4"].Style.Font.Italic = true;
                ws.Cells["G4"].Style.Font.Bold = true;
                ws.Cells["G4:J4"].Merge = true;

                ws.Cells["E5"].Value = "Từ ngày";
                ws.Cells["F5"].Value = StartDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");

                ws.Cells["G5"].Value = "Đến ngày";
                ws.Cells["H5"].Value = EndDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");

                #endregion

                #region Header bang ket qua
                List<string> headers = new List<string>
                {
                    "STT",
                    "Mã nhân viên",
                    "Tên nhân viên",
                    "Mã nhóm sản phẩm",
                    "Tên nhóm sản phẩm"
                };
                List<string> headerLine2 = new List<string>
                {
                    "", "", "", "", ""
                };

                KpiProductGroupingCriterias = KpiProductGroupingCriterias.OrderBy(x => x.Id).ToList();
                for (int i = 0; i < KpiProductGroupingCriterias.Count; i++)
                {
                    headers.Add(KpiProductGroupingCriterias[i].Name);
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
                ws.Cells["D7:D8"].Merge = true;
                ws.Cells["E7:E8"].Merge = true;

                for (int i = 6; i <= endColumnNumber; i += 3)
                {
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
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;
                #endregion

                #region Du lieu bao cao
                int startRow = 9;
                int endRow = startRow;
                if (KpiProductGroupingReport_ExportDTOs != null && KpiProductGroupingReport_ExportDTOs.Count > 0)
                {
                    foreach (var KpiProductGroupingReport_ExportDTO in KpiProductGroupingReport_ExportDTOs)
                    {
                        int startColumn = 1; // gán lại cột bắt đầu từ A
                        int endColumn = startColumn;

                        ws.Cells[$"A{startRow}"].Value = KpiProductGroupingReport_ExportDTO.OrganizationName;
                        ws.Cells[$"A{startRow}"].Style.Font.Bold = true;
                        ws.Cells[$"A{startRow}"].Style.Font.Italic = true;
                        ws.Cells[$"A{startRow}:{endColumnString}{startRow}"].Merge = true;
                        ws.Cells[$"A{startRow}:{endColumnString}{startRow}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                        endRow = startRow; // Gán lại endrow = start row dể bắt đầu vùng dữ liệu mới
                        #region Cột STT
                        List<Object[]> SttData = new List<object[]>();
                        endRow = startRow;
                        foreach (var employee in KpiProductGroupingReport_ExportDTO.Employees)
                        {
                            SttData.Add(new object[]
                            {
                                employee.STT,
                            });
                            endRow++;
                        }
                        string startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                        if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                        string currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                        if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);

                        ws.Cells[$"{currentColumnString}{startRow + 1}:{startColumnString}{endRow}"].LoadFromArrays(SttData);
                        // dòng đầu tiên là dòng org nên startRow + 1
                        endColumn += 1; // chiếm 1 cột
                        startColumn = endColumn; // gán lại để tiếp tục fill dữ liệu tiếp sau
                        #endregion

                        #region Cột mã nhân viên, tên nhân viên, mã nhóm sản phẩm, tên nhóm sản phẩm
                        List<Object[]> KpiData = new List<object[]>();
                        endRow = startRow; // gán lại dòng bắt đầu
                        foreach (var employee in KpiProductGroupingReport_ExportDTO.Employees)
                        {
                            KpiData.Add(new object[]
                            {
                                employee.UserName,
                                employee.DisplayName,
                                employee.ProductGroupingCode,
                                employee.ProductGroupingName,
                            });
                            endRow++;
                        }
                        startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                        if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                        currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                        if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);

                        ws.Cells[$"{currentColumnString}{startRow + 1}:{startColumnString}{endRow}"].LoadFromArrays(KpiData);
                        endColumn += 4; // chiếm 4 cột
                        startColumn = endColumn;
                        #endregion

                        #region Các cột giá trị
                        for (int i = 0; i < KpiProductGroupingCriterias.Count; i++)
                        {
                            List<Object[]> ValueData = new List<object[]>();
                            endRow = startRow; // gán lại dòng bắt đầu
                            foreach (var employee in KpiProductGroupingReport_ExportDTO.Employees)
                            {
                                endColumn = startColumn; // gán lại vị trí cột bắt đầu
                                KpiProductGroupingReport_KpiProductGroupingContentExportDTO criteriaContent = employee
                                    .CriteriaContents.Where(x => x.CriteriaId == KpiProductGroupingCriterias[i].Id).FirstOrDefault(); // Lấy ra giá trị của criteria tương ứng
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

                            endColumn += 2; // Chiếm 3 cột cho mỗi criteria
                            ws.Cells[$"{currentColumnString}{startRow + 1}:{startColumnString}{endRow}"].LoadFromArrays(ValueData); // fill dữ liệu

                            startColumn = endColumn + 1; // gán lại cột bắt đầu cho dữ liệu tiếp sau
                        }
                        #endregion
                        startRow = endRow + 1; // gán dòng bắt đầu cho org tiếp theo

                    }

                }
                ws.Cells[$"F9:{endColumnString}{endRow}"].Style.Numberformat.Format = "#,##0.00"; // format number column value
                                                                                                  // All borders
                ws.Cells[$"A7:{endColumnString}{endRow}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A7:{endColumnString}{endRow}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A7:{endColumnString}{endRow}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A7:{endColumnString}{endRow}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                #endregion

                excel.Save();
            }

            return File(output.ToArray(), "application/octet-stream", "KpiProductGroupingReport.xlsx");
        }

        private async Task<List<KpiProductGroupingReport_ExportDTO>> ConvertReportToExportDTO(List<KpiProductGroupingReport_KpiProductGroupingReportDTO> KpiProductGroupingReport_KpiProductGroupingReportDTOs)
        {
            List<KpiProductGroupingReport_ExportDTO> KpiProductGroupingReport_ExportDTOs = new List<KpiProductGroupingReport_ExportDTO>();
            List<KpiProductGroupingCriteria> KpiProductGroupingCriterias = await KpiProductGroupingCriteriaService.List(new KpiProductGroupingCriteriaFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiProductGroupingCriteriaSelect.Id,
            });
            List<long> KpiProductGroupingCriteriaIds = KpiProductGroupingCriterias.Select(x => x.Id).ToList();

            foreach (var KpiProductGroupingReport_KpiProductGroupingReportDTO in KpiProductGroupingReport_KpiProductGroupingReportDTOs)
            {
                KpiProductGroupingReport_ExportDTO KpiProductGroupingReport_ExportDTO = new KpiProductGroupingReport_ExportDTO();
                KpiProductGroupingReport_ExportDTO.OrganizationId = KpiProductGroupingReport_KpiProductGroupingReportDTO.OrganizationId;
                KpiProductGroupingReport_ExportDTO.OrganizationName = KpiProductGroupingReport_KpiProductGroupingReportDTO.OrganizationName;
                KpiProductGroupingReport_ExportDTO.Employees = new List<KpiProductGroupingReport_KpiSaleEmployeeReportDTO>();

                foreach (var saleEmployee in KpiProductGroupingReport_KpiProductGroupingReportDTO.SaleEmployees)
                {
                    foreach (var content in saleEmployee.Contents)
                    {
                        KpiProductGroupingReport_KpiSaleEmployeeReportDTO KpiProductGroupingReport_KpiSaleEmployeeReportDTO = new KpiProductGroupingReport_KpiSaleEmployeeReportDTO();
                        KpiProductGroupingReport_KpiSaleEmployeeReportDTO.STT = content.STT;
                        KpiProductGroupingReport_KpiSaleEmployeeReportDTO.UserName = saleEmployee.UserName;
                        KpiProductGroupingReport_KpiSaleEmployeeReportDTO.DisplayName = saleEmployee.DisplayName;
                        KpiProductGroupingReport_KpiSaleEmployeeReportDTO.ProductGroupingCode = content.ProductGroupingCode;
                        KpiProductGroupingReport_KpiSaleEmployeeReportDTO.ProductGroupingName = content.ProductGroupingName;
                        KpiProductGroupingReport_KpiSaleEmployeeReportDTO.CriteriaContents = new List<KpiProductGroupingReport_KpiProductGroupingContentExportDTO>();

                        if (KpiProductGroupingCriteriaIds.Contains(KpiProductGroupingCriteriaEnum.INDIRECT_REVENUE.Id))
                        {
                            KpiProductGroupingReport_KpiSaleEmployeeReportDTO.CriteriaContents.Add(new KpiProductGroupingReport_KpiProductGroupingContentExportDTO
                            {
                                CriteriaId = KpiProductGroupingCriteriaEnum.INDIRECT_REVENUE.Id,
                                CriteriaName = KpiProductGroupingCriteriaEnum.INDIRECT_REVENUE.Name,
                                Planned = content.IndirectRevenuePlanned,
                                Result = content.IndirectRevenue,
                                Ratio = content.IndirectRevenueRatio
                            });
                        }
                        if (KpiProductGroupingCriteriaIds.Contains(KpiProductGroupingCriteriaEnum.INDIRECT_STORE.Id))
                        {
                            KpiProductGroupingReport_KpiSaleEmployeeReportDTO.CriteriaContents.Add(new KpiProductGroupingReport_KpiProductGroupingContentExportDTO
                            {
                                CriteriaId = KpiProductGroupingCriteriaEnum.INDIRECT_STORE.Id,
                                CriteriaName = KpiProductGroupingCriteriaEnum.INDIRECT_STORE.Name,
                                Planned = content.IndirectStorePlanned,
                                Result = content.IndirectStore,
                                Ratio = content.IndirectStoreRatio
                            });
                        }
                        if (KpiProductGroupingCriteriaIds.Contains(KpiProductGroupingCriteriaEnum.DIRECT_REVENUE.Id))
                        {
                            KpiProductGroupingReport_KpiSaleEmployeeReportDTO.CriteriaContents.Add(new KpiProductGroupingReport_KpiProductGroupingContentExportDTO
                            {
                                CriteriaId = KpiProductGroupingCriteriaEnum.DIRECT_REVENUE.Id,
                                CriteriaName = KpiProductGroupingCriteriaEnum.DIRECT_REVENUE.Name,
                                Planned = content.DirectRevenuePlanned,
                                Result = content.DirectRevenue,
                                Ratio = content.DirectRevenueRatio
                            });
                        }
                        if (KpiProductGroupingCriteriaIds.Contains(KpiProductGroupingCriteriaEnum.DIRECT_STORE.Id))
                        {
                            KpiProductGroupingReport_KpiSaleEmployeeReportDTO.CriteriaContents.Add(new KpiProductGroupingReport_KpiProductGroupingContentExportDTO
                            {
                                CriteriaId = KpiProductGroupingCriteriaEnum.DIRECT_STORE.Id,
                                CriteriaName = KpiProductGroupingCriteriaEnum.DIRECT_STORE.Name,
                                Planned = content.DirectStorePlanned,
                                Result = content.DirectStore,
                                Ratio = content.DirectStoreRatio
                            });
                        }

                        KpiProductGroupingReport_ExportDTO.Employees.Add(KpiProductGroupingReport_KpiSaleEmployeeReportDTO);
                    }
                }

                KpiProductGroupingReport_ExportDTOs.Add(KpiProductGroupingReport_ExportDTO);
            }

            return KpiProductGroupingReport_ExportDTOs;
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

        public async Task<ActionResult<List<KpiProductGroupingReport_KpiProductGroupingReportDTO>>> List_Old([FromBody] KpiProductGroupingReport_KpiProductGroupingReportFilterDTO KpiProductGroupingReport_KpiProductGroupingReportFilterDTO)
        {
            try
            {
                #region validate dữ liệu filter bắt buộc có 
                if (!ModelState.IsValid)
                    throw new BindException(ModelState);
                if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiPeriodId?.Equal.HasValue == false)
                    return BadRequest(new { message = "Chưa chọn kì KPI" });
                if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiYearId?.Equal.HasValue == false)
                    return BadRequest(new { message = "Chưa chọn năm KPI" });
                if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiProductGroupingTypeId?.Equal.HasValue == false)
                    return BadRequest(new { message = "Chưa chọn loại KPI" });
                #endregion

                #region tính thời gian bắt đầu, kết thúc, lấy ra Id nhân viên và orgUnit từ filter
                DateTime StartDate, EndDate;
                long? SaleEmployeeId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.AppUserId?.Equal;
                long? ProductGroupingId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.ProductGroupingId?.Equal;
                long? KpiPeriodId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiPeriodId?.Equal.Value;
                long? KpiYearId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiYearId?.Equal.Value;
                long? KpiProductGroupingTypeId = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.KpiProductGroupingTypeId?.Equal.Value;
                (StartDate, EndDate) = DateTimeConvert(KpiPeriodId.Value, KpiYearId.Value);
                (StartDate, EndDate) = DateTimeConvert(KpiPeriodId.Value, KpiYearId.Value);
                if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrderDate != null)
                {
                    if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrderDate.GreaterEqual != null)
                    {
                        var StartDateFilter = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrderDate.GreaterEqual.Value.AddHours(0 - CurrentContext.TimeZone);
                        StartDate = StartDateFilter >= StartDate ? StartDateFilter : StartDate; // Don't allow get date out of Kpi period range
                    }
                    if (KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrderDate.LessEqual != null)
                    {
                        var EndDateFilter = KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrderDate.LessEqual.Value.AddDays(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                        EndDate = EndDateFilter <= EndDate ? EndDateFilter : EndDate; // Don't allow get date out of Kpi period range
                    }
                }

                List<long> AppUserIds, OrganizationIds;
                (AppUserIds, OrganizationIds) = await FilterOrganizationAndUser(KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrganizationId,
                    AppUserService, OrganizationService, CurrentContext, DataContext);
                #endregion

                #region lấy dữ liệu báo cáo
                var query = from kpg in DataContext.KpiProductGrouping
                            join au in DataContext.AppUser on kpg.EmployeeId equals au.Id
                            join kpgc in DataContext.KpiProductGroupingContent on kpg.Id equals kpgc.KpiProductGroupingId
                            join pg in DataContext.ProductGrouping on kpgc.ProductGroupingId equals pg.Id
                            where OrganizationIds.Contains(kpg.OrganizationId) &&
                            AppUserIds.Contains(kpg.EmployeeId) &&
                            (SaleEmployeeId.HasValue == false || kpg.EmployeeId == SaleEmployeeId.Value) &&
                            (ProductGroupingId.HasValue == false || pg.Id == ProductGroupingId.Value) &&
                            (kpg.KpiPeriodId == KpiPeriodId.Value) &&
                            (kpg.KpiYearId == KpiYearId) &&
                            (KpiProductGroupingTypeId.HasValue == false || kpg.KpiProductGroupingTypeId == KpiProductGroupingTypeId.Value) &&
                            kpg.DeletedAt == null &&
                            kpg.StatusId == StatusEnum.ACTIVE.Id
                            select new
                            {
                                KpiProductGroupingId = kpg.Id,
                                OrganizationId = kpg.OrganizationId,
                                SaleEmployeeId = kpg.EmployeeId,
                                Username = au.Username,
                                DisplayName = au.DisplayName,
                                ProductGroupingId = pg.Id,
                                ProductGroupingCode = pg.Code,
                                ProductGroupingName = pg.Name,
                            }; // grouping kpi nhóm sản phẩm theo Organization và ProductGrouping

                var datas = await query.Distinct()
                    .OrderBy(x => x.OrganizationId)
                    .ThenBy(x => x.DisplayName)
                    .Skip(KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.Skip)
                    .Take(KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.Take)
                    .ToListWithNoLockAsync(); // lấy ra toàn bộ dữ liệu theo filter
                var KpiProductGroupingIds = datas
                    .Select(x => x.KpiProductGroupingId)
                    .Distinct().ToList();
                OrganizationIds = datas
                    .Select(x => x.OrganizationId)
                    .Distinct().ToList();
                AppUserIds = datas
                    .Select(x => x.SaleEmployeeId)
                    .Distinct().ToList();
                var OrganizationDAOs = await DataContext.Organization.AsNoTracking()
                    .Where(x => OrganizationIds.Contains(x.Id))
                    .OrderBy(x => x.Id)
                    .Select(x => new OrganizationDAO
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToListWithNoLockAsync(); // lấy ra toàn bộ Org trong danh sách phân trang
                var AppUserDAOs = await DataContext.AppUser.AsNoTracking()
                    .Where(x => AppUserIds.Contains(x.Id))
                    .OrderBy(x => x.Id)
                    .Select(x => new AppUserDAO
                    {
                        Id = x.Id,
                        Username = x.Username,
                        DisplayName = x.DisplayName,
                        OrganizationId = x.OrganizationId
                    }).ToListWithNoLockAsync(); // lấy ra toàn bộ Nhân viên trong danh sách phân trang


                var query_content = from km in DataContext.KpiProductGroupingContentCriteriaMapping
                                    join kc in DataContext.KpiProductGroupingContent on km.KpiProductGroupingContentId equals kc.Id
                                    join k in DataContext.KpiProductGrouping on kc.KpiProductGroupingId equals k.Id
                                    join pg in DataContext.ProductGrouping on kc.ProductGroupingId equals pg.Id
                                    where KpiProductGroupingIds.Contains(k.Id) &&
                                    (ProductGroupingId.HasValue == false || pg.Id == ProductGroupingId.Value)
                                    select new
                                    {
                                        SaleEmployeeId = k.EmployeeId,
                                        ProductGroupingId = kc.ProductGroupingId,
                                        KpiProductGroupingContentId = kc.Id,
                                        KpiProductGroupingCriteriaId = km.KpiProductGroupingCriteriaId,
                                        Value = km.Value,
                                    };

                List<KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTO> KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTOs =
                    await query_content.Distinct().Select(x => new KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTO
                    {
                        SaleEmployeeId = x.SaleEmployeeId,
                        ProductGroupingId = x.ProductGroupingId,
                        KpiProductGroupingContentId = x.KpiProductGroupingContentId,
                        KpiProductGroupingCriteriaId = x.KpiProductGroupingCriteriaId,
                        Value = x.Value,
                    }).ToListWithNoLockAsync();

                List<long> KpiProductGroupingContentIds = KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTOs
                    .Select(x => x.KpiProductGroupingContentId)
                    .Distinct()
                    .ToList();
                List<long> ProductGroupingIds = KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTOs
                    .Select(x => x.ProductGroupingId)
                    .Distinct().ToList();
                List<ProductGroupingDAO> ProductGroupingDAOs = await DataContext.ProductGrouping.AsNoTracking()
                    .Where(x => ProductGroupingIds.Contains(x.Id))
                    .Select(x => new ProductGroupingDAO
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                    })
                    .ToListWithNoLockAsync();
                List<KpiProductGroupingContentItemMappingDAO> KpiProductGroupingContentItemMappingDAOs = await DataContext.KpiProductGroupingContentItemMapping.AsNoTracking()
                    .Where(x => KpiProductGroupingContentIds.Contains(x.KpiProductGroupingContentId))
                    .Select(x => new KpiProductGroupingContentItemMappingDAO
                    {
                        ItemId = x.ItemId,
                        KpiProductGroupingContentId = x.KpiProductGroupingContentId,
                    })
                    .ToListWithNoLockAsync(); // lay ra toan bo itemId duoc map voi contentIds
                List<long> ItemIds = KpiProductGroupingContentItemMappingDAOs
                    .Select(x => x.ItemId)
                    .Distinct()
                    .ToList();

                var indirect_sales_order_query = from transaction in DataContext.IndirectSalesOrderTransaction
                                                 join ind in DataContext.IndirectSalesOrder on transaction.IndirectSalesOrderId equals ind.Id
                                                 where AppUserIds.Contains(transaction.SalesEmployeeId) &&
                                                 (transaction.OrderDate >= StartDate) &&
                                                 (transaction.OrderDate <= EndDate) &&
                                                 (ind.RequestStateId == RequestStateEnum.APPROVED.Id) &&
                                                 (ItemIds.Contains(transaction.ItemId))
                                                 select transaction;
                List<IndirectSalesOrderTransactionDAO> IndirectTransactionDAOs = await indirect_sales_order_query
                .Distinct()
                .Select(x => new IndirectSalesOrderTransactionDAO
                {
                    Id = x.Id,
                    SalesEmployeeId = x.SalesEmployeeId,
                    OrderDate = x.OrderDate,
                    BuyerStoreId = x.BuyerStoreId,
                    ItemId = x.ItemId,
                    Revenue = x.Revenue,
                }).ToListWithNoLockAsync();
                #endregion

                #region tổng hợp dữ liệu báo cáo
                List<KpiProductGroupingReport_KpiProductGroupingReportDTO> KpiProductGroupingReport_KpiProductGroupingReportDTOs = new List<KpiProductGroupingReport_KpiProductGroupingReportDTO>();
                foreach (var Organization in OrganizationDAOs)
                {
                    KpiProductGroupingReport_KpiProductGroupingReportDTO KpiProductGroupingReport_KpiProductGroupingReportDTO = new KpiProductGroupingReport_KpiProductGroupingReportDTO
                    {
                        OrganizationId = Organization.Id,
                        OrganizationName = Organization.Name,
                        SaleEmployees = new List<KpiProductGroupingReport_KpiSaleEmployeetDTO>(),
                    };
                    KpiProductGroupingReport_KpiProductGroupingReportDTO.SaleEmployees = datas.Where(x => x.OrganizationId == Organization.Id).Select(x => new KpiProductGroupingReport_KpiSaleEmployeetDTO
                    {
                        Id = x.SaleEmployeeId,
                        UserName = x.Username,
                        DisplayName = x.DisplayName,
                        OrganizationId = x.OrganizationId,
                    }).Distinct().ToList();
                    KpiProductGroupingReport_KpiProductGroupingReportDTOs.Add(KpiProductGroupingReport_KpiProductGroupingReportDTO);
                }

                foreach (var Organization in KpiProductGroupingReport_KpiProductGroupingReportDTOs)
                {
                    foreach (var Employee in Organization.SaleEmployees)
                    {

                        Employee.Contents = new List<KpiProductGroupingReport_KpiProductGroupingContentDTO>();
                        List<KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTO> ContentCriteriaMappings = KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTOs
                            .Where(x => x.SaleEmployeeId == Employee.Id)
                            .ToList();
                        List<long> SubProductGroupingIds = ContentCriteriaMappings.Select(x => x.ProductGroupingId)
                            .Distinct()
                            .ToList();
                        List<long> SubkpiProductGroupingContentIds = ContentCriteriaMappings.Select(x => x.KpiProductGroupingContentId)
                            .Distinct()
                            .ToList();
                        List<ProductGroupingDAO> SubProductGroupings = ProductGroupingDAOs
                            .Where(x => SubProductGroupingIds.Contains(x.Id))
                            .ToList();
                        foreach (var ProductGrouping in SubProductGroupings)
                        {
                            KpiProductGroupingReport_KpiProductGroupingContentDTO Content = new KpiProductGroupingReport_KpiProductGroupingContentDTO();
                            Content.ProductGroupingId = ProductGrouping.Id;
                            Content.ProductGroupingCode = ProductGrouping.Code;
                            Content.ProductGroupingName = ProductGrouping.Name;
                            Content.SaleEmployeeId = Employee.Id;

                            List<KpiProductGroupingReport_KpiProductGroupingContentCriteriaMappingDTO> SubContentCriteriaMappings = ContentCriteriaMappings
                                .Where(x => x.ProductGroupingId == ProductGrouping.Id)
                                .ToList();
                            List<long> SubContentIds = SubContentCriteriaMappings.Select(x => x.KpiProductGroupingContentId)
                                .Distinct()
                                .ToList();
                            List<long> SubItemIds = KpiProductGroupingContentItemMappingDAOs.Where(x => SubContentIds.Contains(x.KpiProductGroupingContentId))
                                .Select(x => x.ItemId)
                                .Distinct()
                                .ToList();
                            List<IndirectSalesOrderTransactionDAO> SubIndirectTransactions = IndirectTransactionDAOs
                                .Where(x => x.SalesEmployeeId == Employee.Id)
                                .Where(x => SubItemIds.Contains(x.ItemId))
                                .ToList(); // lay transaction don gian tiep theo SalesEmployee va Item

                            #region thống kê doanh thu đơn gián tiếp
                            Content.IndirectRevenuePlanned = ContentCriteriaMappings
                                .Where(x => x.ProductGroupingId == ProductGrouping.Id)
                                .Where(x => x.KpiProductGroupingCriteriaId == KpiProductGroupingCriteriaEnum.INDIRECT_REVENUE.Id)
                                .Where(x => x.SaleEmployeeId == Employee.Id)
                                .Select(x => x.Value)
                                .FirstOrDefault(); // lấy ra giá trị kế hoạch theo nhóm sản phẩm, user và loại kpi
                            if (Content.IndirectRevenuePlanned.HasValue)
                            {
                                Content.IndirectRevenue = SubIndirectTransactions
                                    .Where(x => x.Revenue.HasValue)
                                    .Sum(x => x.Revenue);
                                Content.IndirectRevenue = Math.Round(Content.IndirectRevenue.Value, 0);
                                Content.IndirectRevenueRatio = Content.IndirectRevenuePlanned == 0 || Content.IndirectRevenuePlanned == null || Content.IndirectRevenue == null ?
                                    null : (decimal?)
                                    Math.Round((Content.IndirectRevenue.Value / Content.IndirectRevenuePlanned.Value) * 100, 2);
                            }
                            #endregion

                            #region thống kê số đại lý đơn gián tiếp
                            Content.IndirectStorePlanned = ContentCriteriaMappings
                                .Where(x => x.ProductGroupingId == ProductGrouping.Id)
                                .Where(x => x.KpiProductGroupingCriteriaId == KpiProductGroupingCriteriaEnum.INDIRECT_STORE.Id)
                                .Where(x => x.SaleEmployeeId == Employee.Id)
                                .Select(x => x.Value)
                                .FirstOrDefault(); // lấy ra giá trị kế hoạch theo nhóm sản phẩm, user và loại kpi

                            if (Content.IndirectStorePlanned.HasValue)
                            {
                                var BuyStoreIds = SubIndirectTransactions
                                    .Select(x => x.BuyerStoreId)
                                    .Distinct().ToList();
                                Content.StoreIndirectIds = new HashSet<long>(BuyStoreIds);
                                Content.IndirectStoreRatio = Content.IndirectStorePlanned == 0 || Content.IndirectStorePlanned == null || Content.IndirectStore == null ?
                                    null : (decimal?)
                                    Math.Round((Content.IndirectStore.Value / Content.IndirectStorePlanned.Value) * 100, 2);
                            }
                            #endregion

                            Employee.Contents.Add(Content); // thêm content
                        }
                    }
                }
                #endregion

                return KpiProductGroupingReport_KpiProductGroupingReportDTOs;
            }
            catch (Exception Exception)
            {

                throw Exception;
            }
        }

    }
}
