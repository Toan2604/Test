using DMS.Common;
using DMS.DWModels;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MOrganization;
using DMS.Services.MProduct;
using DMS.Services.MProvince;
using DMS.Services.MStore;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreType;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public partial class PermissionMobileController : RpcController
    {
        private const long TODAY = 1;
        private const long THIS_WEEK = 2;
        private const long THIS_MONTH = 3;
        private const long LAST_MONTH = 4;
        private const long THIS_QUARTER = 5;
        private const long LAST_QUATER = 6;
        private const long YEAR = 7;

        private IProvinceService ProvinceService;
        private IStoreGroupingService StoreGroupingService;
        private IStoreTypeService StoreTypeService;
        private IAppUserService AppUserService;
        private IOrganizationService OrganizationService;
        private IItemService ItemService;
        private IProductService ProductService;
        private IStoreService StoreService;

        private ICurrentContext CurrentContext;
        private DataContext DataContext;
        private DWContext DWContext;

        public PermissionMobileController(
            IProvinceService ProvinceService,
            IStoreGroupingService StoreGroupingService,
            IStoreTypeService StoreTypeService,
            IAppUserService AppUserService,
            IOrganizationService OrganizationService,
            IItemService ItemService,
            IProductService ProductService,
            IStoreService StoreService,
            ICurrentContext CurrentContext,
            DWContext DWContext,
            DataContext DataContext)
        {
            this.ProvinceService = ProvinceService;
            this.StoreGroupingService = StoreGroupingService;
            this.StoreTypeService = StoreTypeService;
            this.AppUserService = AppUserService;
            this.OrganizationService = OrganizationService;
            this.ItemService = ItemService;
            this.ProductService = ProductService;
            this.StoreService = StoreService;
            this.CurrentContext = CurrentContext;
            this.DataContext = DataContext;
            this.DWContext = DWContext;
        }

        #region Dashboard KPI
        [Route(PermissionMobileRoute.ListCurrentKpiGeneral), HttpPost]
        public async Task<List<PermissionMobile_EmployeeKpiGeneralReportDTO>> ListCurrentKpiGeneral([FromBody] PermissionMobile_EmployeeKpiGeneralReportFilterDTO PermissionMobile_EmployeeKpiFilterDTO)
        {
            // lây ra tháng hiện tại + năm hiện tại
            long KpiYearId = StaticParams.DateTimeNow.Year;
            long KpiPeriodId = StaticParams.DateTimeNow.Month + 100;
            GenericEnum CurrentMonth;
            GenericEnum CurrentQuarter;
            GenericEnum CurrentYear;
            (CurrentMonth, CurrentQuarter, CurrentYear) = ConvertDateTime(StaticParams.DateTimeNow);
            DateTime Start = new DateTime(StaticParams.DateTimeNow.Year, StaticParams.DateTimeNow.Month, 1);
            Start = Start.AddHours(0 - CurrentContext.TimeZone);
            DateTime End = new DateTime(StaticParams.DateTimeNow.Year, StaticParams.DateTimeNow.Month, 1).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);

            List<long> AppUserIds = new List<long>();
            if (PermissionMobile_EmployeeKpiFilterDTO.EmployeeId.Equal.HasValue)
            {
                AppUserIds.Add(PermissionMobile_EmployeeKpiFilterDTO.EmployeeId.Equal.Value);
            }
            else
            {
                List<long> Ids = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
                AppUserIds.AddRange(Ids);
            }
            IdFilter AppUserFilter = new IdFilter() { In = AppUserIds };
            DateFilter OrderDateFilter = new DateFilter() { GreaterEqual = Start, LessEqual = End };

            // lấy ra số KpiGeneral theo kế hoạch
            // lấy ra KpiGeneral bằng filter theo AppUserId, trạng thái = true, kpiYearId
            var kpigeneral_query = DataContext.KpiGeneral.AsNoTracking();
            kpigeneral_query = kpigeneral_query.Where(x => x.EmployeeId, AppUserFilter);
            kpigeneral_query = kpigeneral_query.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });
            kpigeneral_query = kpigeneral_query.Where(x => x.KpiYearId, new IdFilter { Equal = CurrentYear.Id });
            kpigeneral_query = kpigeneral_query.Where(x => x.DeletedAt == null);
            List<long> KpiGeneralIds = await kpigeneral_query.Select(p => p.Id).ToListWithNoLockAsync();

            if (KpiGeneralIds.Count == 0)
                return new List<PermissionMobile_EmployeeKpiGeneralReportDTO>();

            var kpigeneralcontent_query = DataContext.KpiGeneralContent.AsNoTracking();
            kpigeneralcontent_query = kpigeneralcontent_query.Where(x => x.KpiGeneralId, new IdFilter { In = KpiGeneralIds });
            kpigeneralcontent_query = kpigeneralcontent_query.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });
            List<long> KpiGeneralContentIds = await kpigeneralcontent_query.Select(x => x.Id).ToListWithNoLockAsync();

            // lấy ra toàn bộ KpiGeneralContentKpiPeriodMappings bằng filter theo KpiGeneralContent và theo kì
            var kpigeneralcontent_kpiperiodmapping_query = DataContext.KpiGeneralContentKpiPeriodMapping.AsNoTracking();
            kpigeneralcontent_kpiperiodmapping_query = kpigeneralcontent_kpiperiodmapping_query.Where(x => x.KpiGeneralContentId, new IdFilter { In = KpiGeneralContentIds });
            kpigeneralcontent_kpiperiodmapping_query = kpigeneralcontent_kpiperiodmapping_query.Where(x => x.KpiPeriodId, new IdFilter { Equal = KpiPeriodId });
            List<KpiGeneralContentKpiPeriodMappingDAO> KpiGeneralContentKpiPeriodMappingDAOs = await kpigeneralcontent_kpiperiodmapping_query
                .Select(x => new KpiGeneralContentKpiPeriodMappingDAO
                {
                    KpiPeriodId = x.KpiPeriodId,
                    KpiGeneralContentId = x.KpiGeneralContentId,
                    Value = x.Value,
                    KpiGeneralContent = x.KpiGeneralContent == null ? null : new KpiGeneralContentDAO
                    {
                        KpiCriteriaGeneralId = x.KpiGeneralContent.KpiCriteriaGeneralId
                    }
                }).ToListWithNoLockAsync();

            // lấy ra toàn bộ storeChecking để tính số liệu thực hiện bằng filter SaleEmployeeId
            var storechecking_query = DWContext.Fact_StoreChecking.AsNoTracking();
            storechecking_query = storechecking_query.Where(x => x.SaleEmployeeId, AppUserFilter);
            storechecking_query = storechecking_query.Where(x => x.CheckOutAt, OrderDateFilter);
            var StoreCheckingDAOs = await storechecking_query
                .Select(x => new StoreCheckingDAO
                {
                    SaleEmployeeId = x.SaleEmployeeId,
                    Id = x.StoreCheckingId,
                    CheckInAt = x.CheckInAt,
                    CheckOutAt = x.CheckOutAt,
                    StoreId = x.StoreId
                })
                .ToListWithNoLockAsync();

            // All stores
            var allstore_query = DWContext.Dim_Store.AsNoTracking();
            var AllStores = await allstore_query.ToListWithNoLockAsync();

            // store type
            var StoreTypeDAOs = await DWContext.Dim_StoreType.ToListWithNoLockAsync();
            long C2TD_ID = StoreTypeDAOs.Where(x => x.Code == StaticParams.C2TD).Select(x => x.StoreTypeId).FirstOrDefault();
            long C2SL_ID = StoreTypeDAOs.Where(x => x.Code == StaticParams.C2SL).Select(x => x.StoreTypeId).FirstOrDefault();
            long C2_ID = StoreTypeDAOs.Where(x => x.Code == StaticParams.C2).Select(x => x.StoreTypeId).FirstOrDefault();


            // DirectSalesOrder
            var directsalesorder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            directsalesorder_query = directsalesorder_query.Where(q => q.SaleEmployeeId, AppUserFilter);
            directsalesorder_query = directsalesorder_query.Where(q => q.OrderDate, OrderDateFilter);
            directsalesorder_query = directsalesorder_query.Where(q => q.GeneralApprovalStateId, new IdFilter()
            {
                In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });
            var DirectSalesOrderDAOs = await directsalesorder_query.ToListWithNoLockAsync();
            List<long> DirectSalesOrderIds = DirectSalesOrderDAOs.Select(x => x.DirectSalesOrderId).ToList();

            // DirectSalesOrder transaction
            var DirectSalesOrderTransaction_query = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
            DirectSalesOrderTransaction_query = DirectSalesOrderTransaction_query.Where(x => x.DeletedAt == null);
            DirectSalesOrderTransaction_query = DirectSalesOrderTransaction_query.Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds });
            DirectSalesOrderTransaction_query = DirectSalesOrderTransaction_query.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id });
            var DirectSalesOrderTransactionDAOs = await DirectSalesOrderTransaction_query.Select(x => new
            {
                x.DirectSalesOrderId,
                x.Quantity
            }).ToListWithNoLockAsync();

            // IndirectSalesOrder
            var indirectsalesorder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            indirectsalesorder_query = indirectsalesorder_query.Where(q => q.SaleEmployeeId, AppUserFilter);
            indirectsalesorder_query = indirectsalesorder_query.Where(q => q.OrderDate, OrderDateFilter);
            indirectsalesorder_query = indirectsalesorder_query.Where(q => q.RequestStateId, new IdFilter() { Equal = RequestStateEnum.APPROVED.Id });

            var IndirectSalesOrderDAOs = await indirectsalesorder_query.ToListWithNoLockAsync();
            List<long> IndirectSalesOrderIds = IndirectSalesOrderDAOs.Select(x => x.IndirectSalesOrderId).ToList();

            // IndirectSalesOrder transaction
            var IndirectSalesOrderTransaction_query = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            IndirectSalesOrderTransaction_query = IndirectSalesOrderTransaction_query.Where(x => x.DeletedAt == null);
            IndirectSalesOrderTransaction_query = IndirectSalesOrderTransaction_query.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
            IndirectSalesOrderTransaction_query = IndirectSalesOrderTransaction_query.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id });
            var IndirectSalesOrderTransactionDAOs = await IndirectSalesOrderTransaction_query.Select(x => new
            {
                x.IndirectSalesOrderId,
                x.Quantity
            }).ToListWithNoLockAsync();

            // IndirectSalesOrder C2TD
            List<long> StoreC2TDIds = AllStores.Where(s => s.StoreTypeId == C2TD_ID).Select(s => s.StoreId).ToList();
            var StoreC2TDIdFilter = new IdFilter() { In = StoreC2TDIds };
            var IndirectSalesOrderC2TDDAOs = await indirectsalesorder_query.Where(q => q.BuyerStoreId, StoreC2TDIdFilter).ToListWithNoLockAsync();

            // IndirectSalesOrder C2SL
            List<long> StoreC2SLIds = AllStores.Where(s => s.StoreTypeId == C2SL_ID).Select(s => s.StoreId).ToList();
            var StoreC2SLFilter = new IdFilter() { In = StoreC2SLIds };
            var IndirectSalesOrderC2SLDAOs = await indirectsalesorder_query.Where(q => q.BuyerStoreId, StoreC2SLFilter).ToListWithNoLockAsync();

            // IndirectSalesOrder C2
            List<long> StoreC2Ids = AllStores.Where(s => s.StoreTypeId == C2_ID).Select(s => s.StoreId).ToList();
            var StoreC2IdFilter = new IdFilter() { In = StoreC2Ids };
            var IndirectSalesOrderC2DAOs = await indirectsalesorder_query.Where(q => q.BuyerStoreId, StoreC2IdFilter).ToListWithNoLockAsync();

            var store_query = allstore_query.Where(x => x.CreatorId, AppUserFilter);
            store_query = store_query.Where(s => s.DeletedAt == null);
            store_query = store_query.Where(x => x.CreatedAt, OrderDateFilter);
            var StoreDAOs = await store_query.ToListWithNoLockAsync();

            var problem_query = DWContext.Fact_Problem.AsNoTracking();
            problem_query = problem_query.Where(x => x.CreatorId, AppUserFilter);
            problem_query = problem_query.Where(x => x.NoteAt, OrderDateFilter);
            var Problems = await problem_query.ToListWithNoLockAsync();

            var storeimage_query = DWContext.Fact_Image.AsNoTracking();
            storeimage_query = storeimage_query.Where(x => x.SaleEmployeeId, AppUserFilter);
            storeimage_query = storeimage_query.Where(x => x.ShootingAt, OrderDateFilter);
            var StoreImages = await storeimage_query.ToListWithNoLockAsync();

            var KpiCriteriaGeneralDAOs = await DataContext.KpiCriteriaGeneral.Where(x => x.StatusId == StatusEnum.ACTIVE.Id).ToListWithNoLockAsync();

            var KpiGenerals = new List<PermissionMobile_EmployeeKpiGeneralReportDTO>();
            foreach (var KpiCriteriaGeneral in KpiCriteriaGeneralDAOs)
            {
                List<KpiGeneralContentKpiPeriodMappingDAO>
                    KpiGeneralContentKpiPeriodMappings = KpiGeneralContentKpiPeriodMappingDAOs
                    .Where(x => x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneral.Id)
                    .ToList();

                PermissionMobile_EmployeeKpiGeneralReportDTO PermissionMobile_EmployeeKpiGeneralReportDTO = new PermissionMobile_EmployeeKpiGeneralReportDTO();
                PermissionMobile_EmployeeKpiGeneralReportDTO.KpiCriteriaGeneralId = KpiCriteriaGeneral.Id;
                PermissionMobile_EmployeeKpiGeneralReportDTO.KpiCriteriaGeneralName = KpiCriteriaGeneral.Name;

                PermissionMobile_EmployeeKpiGeneralReportDTO.PlannedValue = KpiGeneralContentKpiPeriodMappingDAOs
                    .Where(x => x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneral.Id)
                    .Select(x => x.Value).FirstOrDefault();

                if (KpiCriteriaGeneral.Id == KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_AMOUNT.Id)
                {
                    PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue = IndirectSalesOrderDAOs.Sum(iso => iso.Total);
                }
                if (KpiCriteriaGeneral.Id == KpiCriteriaGeneralEnum.REVENUE_C2_TD.Id)
                {
                    PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue = IndirectSalesOrderC2TDDAOs
                    .Sum(iso => iso.Total);
                }
                if (KpiCriteriaGeneral.Id == KpiCriteriaGeneralEnum.REVENUE_C2_SL.Id)
                {
                    PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue = IndirectSalesOrderC2SLDAOs
                    .Sum(iso => iso.Total);
                }
                if (KpiCriteriaGeneral.Id == KpiCriteriaGeneralEnum.REVENUE_C2.Id)
                {
                    PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue = IndirectSalesOrderC2DAOs
                    .Sum(iso => iso.Total);
                }
                if (KpiCriteriaGeneral.Id == KpiCriteriaGeneralEnum.NEW_STORE_CREATED.Id)
                {
                    PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue = StoreDAOs.Count();
                }
                if (KpiCriteriaGeneral.Id == KpiCriteriaGeneralEnum.NEW_STORE_C2_CREATED.Id)
                {
                    PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue = StoreDAOs
                    .Where(x => x.StoreTypeId == C2TD_ID)
                    .Count();
                }
                if (KpiCriteriaGeneral.Id == KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_AMOUNT.Id)
                {
                    PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue = DirectSalesOrderDAOs.Sum(iso => iso.Total);
                }
                if (KpiCriteriaGeneral.Id == KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_ORDER.Id)
                {
                    PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue = DirectSalesOrderDAOs.Distinct().Count();
                }
                if (KpiCriteriaGeneral.Id == KpiCriteriaGeneralEnum.STORE_VISITED.Id)
                {
                    PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue = StoreCheckingDAOs
                        .Select(x => x.StoreId)
                        .Distinct()
                        .Count();
                }
                if (KpiCriteriaGeneral.Id == KpiCriteriaGeneralEnum.NUMBER_OF_STORE_VISIT.Id)
                {
                    PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue = StoreCheckingDAOs.Count();
                }
                if (KpiCriteriaGeneral.Id == KpiCriteriaGeneralEnum.TOTAL_PROBLEM.Id)
                {
                    PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue = Problems.Count();
                }
                if (KpiCriteriaGeneral.Id == KpiCriteriaGeneralEnum.TOTAL_IMAGE.Id)
                {
                    PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue = StoreImages.Count();
                }
                if (KpiCriteriaGeneral.Id == KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_QUANTITY.Id)
                {
                    PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue = DirectSalesOrderTransactionDAOs.Sum(x => x.Quantity);
                }
                if (KpiCriteriaGeneral.Id == KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_QUANTITY.Id)
                {
                    PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue = IndirectSalesOrderTransactionDAOs.Sum(x => x.Quantity);
                }
                if (KpiCriteriaGeneral.Id == KpiCriteriaGeneralEnum.DIRECT_SALES_BUYER_STORE.Id)
                {
                    PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue = DirectSalesOrderDAOs.Select(x => x.BuyerStoreId).Distinct().Count();
                }
                if (KpiCriteriaGeneral.Id == KpiCriteriaGeneralEnum.INDIRECT_SALES_BUYER_STORE.Id)
                {
                    PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue = IndirectSalesOrderDAOs.Select(x => x.BuyerStoreId).Distinct().Count();
                }

                PermissionMobile_EmployeeKpiGeneralReportDTO.Percentage = PermissionMobile_EmployeeKpiGeneralReportDTO.PlannedValue == null || PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue == null || PermissionMobile_EmployeeKpiGeneralReportDTO.PlannedValue.Value == 0
                            ? null
                            : (decimal?)Math.Round((PermissionMobile_EmployeeKpiGeneralReportDTO.CurrentValue.Value / PermissionMobile_EmployeeKpiGeneralReportDTO.PlannedValue.Value) * 100, 2);

                KpiGenerals.Add(PermissionMobile_EmployeeKpiGeneralReportDTO);
            }
            return KpiGenerals;
        }

        [Route(PermissionMobileRoute.ListCurrentKpiItem), HttpPost]
        public async Task<List<PermissionMobile_EmployeeKpiItemReportDTO>> ListCurrentKpiItem([FromBody] PermissionMobile_EmployeeKpiItemReportFilterDTO PermissionMobile_EmployeeKpiFilterDTO)
        {
            // lây ra tháng hiện tại + năm hiện tại
            long KpiYearId = StaticParams.DateTimeNow.Year;
            long KpiPeriodId = StaticParams.DateTimeNow.Month + 100;
            GenericEnum CurrentMonth;
            GenericEnum CurrentQuarter;
            GenericEnum CurrentYear;
            (CurrentMonth, CurrentQuarter, CurrentYear) = ConvertDateTime(StaticParams.DateTimeNow);
            DateTime Start = new DateTime(StaticParams.DateTimeNow.Year, StaticParams.DateTimeNow.Month, 1);
            Start = Start.AddHours(0 - CurrentContext.TimeZone);
            DateTime End = new DateTime(StaticParams.DateTimeNow.Year, StaticParams.DateTimeNow.Month, 1).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);

            List<long> AppUserIds = new List<long>();
            if (PermissionMobile_EmployeeKpiFilterDTO.EmployeeId.Equal.HasValue)
            {
                AppUserIds.Add(PermissionMobile_EmployeeKpiFilterDTO.EmployeeId.Equal.Value);
            }
            else
            {
                List<long> Ids = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
                AppUserIds.AddRange(Ids);
            }
            IdFilter AppUserFilter = new IdFilter() { In = AppUserIds };
            DateFilter OrderDateFilter = new DateFilter() { GreaterEqual = Start, LessEqual = End };

            var kpiitem_query = DataContext.KpiItem.AsNoTracking();
            kpiitem_query = kpiitem_query.Where(x => x.EmployeeId, AppUserFilter);
            kpiitem_query = kpiitem_query.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });
            kpiitem_query = kpiitem_query.Where(x => x.KpiPeriodId, new IdFilter { Equal = KpiPeriodId });
            kpiitem_query = kpiitem_query.Where(x => x.KpiYearId, new IdFilter { Equal = KpiYearId });
            kpiitem_query = kpiitem_query.Where(x => x.KpiItemTypeId, new IdFilter { Equal = KpiItemTypeEnum.ALL_PRODUCT.Id });
            kpiitem_query = kpiitem_query.Where(x => x.DeletedAt == null);
            List<KpiItemDAO> KpiItems = await kpiitem_query.Select(x => new KpiItemDAO
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
            }).ToListWithNoLockAsync();
            var KpiItemIds = KpiItems.Select(x => x.Id).ToList();
            if (KpiItems.Count == 0)
                return new List<PermissionMobile_EmployeeKpiItemReportDTO>();

            var kpiitemcontent_query = DataContext.KpiItemContent.AsNoTracking();
            kpiitemcontent_query = kpiitemcontent_query.Where(x => x.KpiItemId, new IdFilter { In = KpiItemIds });
            List<KpiItemContentDAO> KpiItemContentDAOs = await kpiitemcontent_query.ToListWithNoLockAsync();
            List<long> KpiItemContentIds = KpiItemContentDAOs.Select(x => x.Id).ToList();

            if (KpiItemContentIds.Count == 0)
                return new List<PermissionMobile_EmployeeKpiItemReportDTO>();

            var kpiitemcontent_kpicriteriaitem_mapping_query = DataContext.KpiItemContentKpiCriteriaItemMapping.AsNoTracking();
            kpiitemcontent_kpicriteriaitem_mapping_query = kpiitemcontent_kpicriteriaitem_mapping_query.Where(x => x.KpiItemContentId, new IdFilter { In = KpiItemContentIds });
            List<KpiItemContentKpiCriteriaItemMappingDAO> KpiItemContentKpiCriteriaItemMappingDAOs = await kpiitemcontent_kpicriteriaitem_mapping_query.ToListWithNoLockAsync();

            List<PermissionMobile_EmployeeKpiItem> PermissionMobile_EmployeeKpiItems = new List<PermissionMobile_EmployeeKpiItem>();
            if (KpiItemContentKpiCriteriaItemMappingDAOs.Count == 0)
                return new List<PermissionMobile_EmployeeKpiItemReportDTO>();

            List<long> ItemIds = KpiItemContentDAOs.Select(x => x.ItemId).Distinct().ToList(); // lẩy ra list itemId theo chỉ tiêu
            List<ItemDAO> ItemDAOs = await DataContext.Item.Where(x => ItemIds.Contains(x.Id)).Select(x => new ItemDAO
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToListWithNoLockAsync();

            //var indirectsalesorder_transaction_query = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            //indirectsalesorder_transaction_query = indirectsalesorder_transaction_query.Where(x => x.OrderDate, OrderDateFilter);
            //indirectsalesorder_transaction_query = indirectsalesorder_transaction_query.Where(x => x.SalesEmployeeId, AppUserFilter);
            //indirectsalesorder_transaction_query = indirectsalesorder_transaction_query.Where(x => x.ItemId, new IdFilter { In = ItemIds });
            //indirectsalesorder_transaction_query = indirectsalesorder_transaction_query.Where(x => x.Item.DeletedAt == null);

            //List<IndirectSalesOrderTransactionDAO> IndirectSalesOrderTransactionDAOs = await indirectsalesorder_transaction_query.ToListWithNoLockAsync();

            // DirectSalesOrderTransaction
            var direct_sales_order_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            direct_sales_order_query = direct_sales_order_query.Where(q => q.OrderDate, OrderDateFilter);
            direct_sales_order_query = direct_sales_order_query.Where(q => q.SaleEmployeeId, AppUserFilter);
            direct_sales_order_query = direct_sales_order_query.Where(q => q.GeneralApprovalStateId, new IdFilter()
            {
                In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });
            var direct_sales_order_transaction_query = from tr in DWContext.Fact_DirectSalesOrderTransaction
                                                       join dr in direct_sales_order_query on tr.DirectSalesOrderId equals dr.DirectSalesOrderId
                                                       select tr;
            var DirectSalesOrderTransactionDAOs = await direct_sales_order_transaction_query.ToListWithNoLockAsync();

            // IndirectSalesOrderTransaction
            var indirect_sales_order_transaction_query = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            indirect_sales_order_transaction_query = indirect_sales_order_transaction_query.Where(q => q.OrderDate, OrderDateFilter);
            indirect_sales_order_transaction_query = indirect_sales_order_transaction_query.Where(q => q.SalesEmployeeId, AppUserFilter);
            indirect_sales_order_transaction_query = indirect_sales_order_transaction_query.Where(q => q.RequestStateId, new IdFilter() { Equal = RequestStateEnum.APPROVED.Id });
            indirect_sales_order_transaction_query = indirect_sales_order_transaction_query.Where(x => !x.DeletedAt.HasValue);

            var IndirectSalesOrderTransactionDAOs = await indirect_sales_order_transaction_query.ToListWithNoLockAsync();

            var KpiCriteriaItems = await DataContext.KpiCriteriaItem.Where(x => x.StatusId == StatusEnum.ACTIVE.Id).ToListWithNoLockAsync();

            var PermissionMobile_EmployeeKpiItemReportDTOs = new List<PermissionMobile_EmployeeKpiItemReportDTO>();

            foreach (var item in ItemDAOs)
            {
                var PermissionMobile_EmployeeKpiItemReportDTO = new PermissionMobile_EmployeeKpiItemReportDTO();

                var SubKpiItemContentIds = KpiItemContentDAOs.Where(x => x.ItemId == item.Id).Select(x => x.Id).ToList();
                var SubKpiItemContentKpiCriteriaItemMappingDAOs = KpiItemContentKpiCriteriaItemMappingDAOs
                    .Where(x => SubKpiItemContentIds.Contains(x.KpiItemContentId));

                var SubIndirectSalesOrderTransactionDAOs = IndirectSalesOrderTransactionDAOs.Where(x => x.ItemId == item.Id).ToList();
                var SubDirectSalesOrderTransactionDAOs = DirectSalesOrderTransactionDAOs.Where(x => x.ItemId == item.Id).ToList();

                PermissionMobile_EmployeeKpiItemReportDTO.ItemId = item.Id;
                PermissionMobile_EmployeeKpiItemReportDTO.ItemName = item.Name;
                PermissionMobile_EmployeeKpiItemReportDTO.CurrentKpiItems = new List<PermissionMobile_EmployeeKpiItem>();

                foreach (var kpiCriteriaItem in KpiCriteriaItems)
                {
                    PermissionMobile_EmployeeKpiItem PermissionMobile_EmployeeKpiItem = new PermissionMobile_EmployeeKpiItem();
                    PermissionMobile_EmployeeKpiItem.ItemId = item.Id;
                    PermissionMobile_EmployeeKpiItem.KpiCriteriaItemName = kpiCriteriaItem.Name;
                    PermissionMobile_EmployeeKpiItem.PlannedValue = SubKpiItemContentKpiCriteriaItemMappingDAOs
                        .Where(x => x.KpiCriteriaItemId == kpiCriteriaItem.Id)
                        .Select(x => x.Value).FirstOrDefault();

                    if (kpiCriteriaItem.Id == KpiCriteriaItemEnum.INDIRECT_REVENUE.Id)
                        PermissionMobile_EmployeeKpiItem.CurrentValue = SubIndirectSalesOrderTransactionDAOs.Sum(x => (x.Amount - x.GeneralDiscountAmount ?? 0));
                    if (kpiCriteriaItem.Id == KpiCriteriaItemEnum.INDIRECT_STORE.Id)
                        PermissionMobile_EmployeeKpiItem.CurrentValue = SubIndirectSalesOrderTransactionDAOs.Select(x => x.BuyerStoreId).Distinct().Count();
                    if (kpiCriteriaItem.Id == KpiCriteriaItemEnum.DIRECT_REVENUE.Id)
                        PermissionMobile_EmployeeKpiItem.CurrentValue = SubDirectSalesOrderTransactionDAOs.Sum(x => (x.Amount - x.GeneralDiscountAmount ?? 0));
                    if (kpiCriteriaItem.Id == KpiCriteriaItemEnum.DIRECT_STORE.Id)
                        PermissionMobile_EmployeeKpiItem.CurrentValue = SubDirectSalesOrderTransactionDAOs.Select(x => x.BuyerStoreId).Distinct().Count();
                    PermissionMobile_EmployeeKpiItem.Percentage = PermissionMobile_EmployeeKpiItem.PlannedValue == null || PermissionMobile_EmployeeKpiItem.CurrentValue == null || PermissionMobile_EmployeeKpiItem.PlannedValue.Value == 0
                            ? null
                            : (decimal?)Math.Round((PermissionMobile_EmployeeKpiItem.CurrentValue.Value / PermissionMobile_EmployeeKpiItem.PlannedValue.Value) * 100, 2);
                    PermissionMobile_EmployeeKpiItemReportDTO.CurrentKpiItems.Add(PermissionMobile_EmployeeKpiItem);
                }
                PermissionMobile_EmployeeKpiItemReportDTOs.Add(PermissionMobile_EmployeeKpiItemReportDTO);
            }

            return PermissionMobile_EmployeeKpiItemReportDTOs;
        }

        [Route(PermissionMobileRoute.ListCurrentKpiNewItem), HttpPost]
        public async Task<List<PermissionMobile_EmployeeKpiItemReportDTO>> ListCurrentKpiNewItem([FromBody] PermissionMobile_EmployeeKpiItemReportFilterDTO PermissionMobile_EmployeeKpiFilterDTO)
        {
            // lây ra tháng hiện tại + năm hiện tại
            long KpiYearId = StaticParams.DateTimeNow.Year;
            long KpiPeriodId = StaticParams.DateTimeNow.Month + 100;
            GenericEnum CurrentMonth;
            GenericEnum CurrentQuarter;
            GenericEnum CurrentYear;
            (CurrentMonth, CurrentQuarter, CurrentYear) = ConvertDateTime(StaticParams.DateTimeNow);
            DateTime Start = new DateTime(StaticParams.DateTimeNow.Year, StaticParams.DateTimeNow.Month, 1);
            Start = Start.AddHours(0 - CurrentContext.TimeZone);
            DateTime End = new DateTime(StaticParams.DateTimeNow.Year, StaticParams.DateTimeNow.Month, 1).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);

            List<long> AppUserIds = new List<long>();
            if (PermissionMobile_EmployeeKpiFilterDTO.EmployeeId.Equal.HasValue)
            {
                AppUserIds.Add(PermissionMobile_EmployeeKpiFilterDTO.EmployeeId.Equal.Value);
            }
            else
            {
                List<long> Ids = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
                AppUserIds.AddRange(Ids);
            }
            IdFilter AppUserFilter = new IdFilter() { In = AppUserIds };
            DateFilter OrderDateFilter = new DateFilter() { GreaterEqual = Start, LessEqual = End };

            var kpiitem_query = DataContext.KpiItem.AsNoTracking();
            kpiitem_query = kpiitem_query.Where(x => x.EmployeeId, AppUserFilter);
            kpiitem_query = kpiitem_query.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });
            kpiitem_query = kpiitem_query.Where(x => x.KpiPeriodId, new IdFilter { Equal = KpiPeriodId });
            kpiitem_query = kpiitem_query.Where(x => x.KpiYearId, new IdFilter { Equal = KpiYearId });
            kpiitem_query = kpiitem_query.Where(x => x.KpiItemTypeId, new IdFilter { Equal = KpiItemTypeEnum.NEW_PRODUCT.Id });
            kpiitem_query = kpiitem_query.Where(x => x.DeletedAt == null);
            List<KpiItemDAO> KpiItems = await kpiitem_query.Select(x => new KpiItemDAO
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
            }).ToListWithNoLockAsync();
            var KpiItemIds = KpiItems.Select(x => x.Id).ToList();
            if (KpiItems.Count == 0)
                return new List<PermissionMobile_EmployeeKpiItemReportDTO>();

            var kpiitemcontent_query = DataContext.KpiItemContent.AsNoTracking();
            kpiitemcontent_query = kpiitemcontent_query.Where(x => x.KpiItemId, new IdFilter { In = KpiItemIds });
            List<KpiItemContentDAO> KpiItemContentDAOs = await kpiitemcontent_query.ToListWithNoLockAsync();
            List<long> KpiItemContentIds = KpiItemContentDAOs.Select(x => x.Id).ToList();

            if (KpiItemContentIds.Count == 0)
                return new List<PermissionMobile_EmployeeKpiItemReportDTO>();

            var kpiitemcontent_kpicriteriaitem_mapping_query = DataContext.KpiItemContentKpiCriteriaItemMapping.AsNoTracking();
            kpiitemcontent_kpicriteriaitem_mapping_query = kpiitemcontent_kpicriteriaitem_mapping_query.Where(x => x.KpiItemContentId, new IdFilter { In = KpiItemContentIds });
            List<KpiItemContentKpiCriteriaItemMappingDAO> KpiItemContentKpiCriteriaItemMappingDAOs = await kpiitemcontent_kpicriteriaitem_mapping_query.ToListWithNoLockAsync();

            List<PermissionMobile_EmployeeKpiItem> PermissionMobile_EmployeeKpiItems = new List<PermissionMobile_EmployeeKpiItem>();
            if (KpiItemContentKpiCriteriaItemMappingDAOs.Count == 0)
                return new List<PermissionMobile_EmployeeKpiItemReportDTO>();

            List<long> ItemIds = KpiItemContentDAOs.Select(x => x.ItemId).Distinct().ToList(); // lẩy ra list itemId theo chỉ tiêu
            List<ItemDAO> ItemDAOs = await DataContext.Item.Where(x => ItemIds.Contains(x.Id)).Select(x => new ItemDAO
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToListWithNoLockAsync();

            // DirectSalesOrderTransaction
            var direct_sales_order_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            direct_sales_order_query = direct_sales_order_query.Where(q => q.OrderDate, OrderDateFilter);
            direct_sales_order_query = direct_sales_order_query.Where(q => q.SaleEmployeeId, AppUserFilter);
            direct_sales_order_query = direct_sales_order_query.Where(q => q.GeneralApprovalStateId, new IdFilter()
            {
                In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });
            var direct_sales_order_transaction_query = from tr in DWContext.Fact_DirectSalesOrderTransaction
                                                       join dr in direct_sales_order_query on tr.DirectSalesOrderId equals dr.DirectSalesOrderId
                                                       select tr;
            var DirectSalesOrderTransactionDAOs = await direct_sales_order_transaction_query.ToListWithNoLockAsync();

            // IndirectSalesOrderTransaction
            var indirect_sales_order_transaction_query = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            indirect_sales_order_transaction_query = indirect_sales_order_transaction_query.Where(q => q.OrderDate, OrderDateFilter);
            indirect_sales_order_transaction_query = indirect_sales_order_transaction_query.Where(q => q.SalesEmployeeId, AppUserFilter);
            indirect_sales_order_transaction_query = indirect_sales_order_transaction_query.Where(q => q.RequestStateId, new IdFilter() { Equal = RequestStateEnum.APPROVED.Id });
            indirect_sales_order_transaction_query = indirect_sales_order_transaction_query.Where(x => !x.DeletedAt.HasValue);

            var IndirectSalesOrderTransactionDAOs = await indirect_sales_order_transaction_query.ToListWithNoLockAsync();

            var KpiCriteriaItems = await DataContext.KpiCriteriaItem.Where(x => x.StatusId == StatusEnum.ACTIVE.Id).ToListWithNoLockAsync();

            var PermissionMobile_EmployeeKpiItemReportDTOs = new List<PermissionMobile_EmployeeKpiItemReportDTO>();

            foreach (var item in ItemDAOs)
            {
                var PermissionMobile_EmployeeKpiItemReportDTO = new PermissionMobile_EmployeeKpiItemReportDTO();

                var SubKpiItemContentIds = KpiItemContentDAOs.Where(x => x.ItemId == item.Id).Select(x => x.Id).ToList();
                var SubKpiItemContentKpiCriteriaItemMappingDAOs = KpiItemContentKpiCriteriaItemMappingDAOs
                    .Where(x => SubKpiItemContentIds.Contains(x.KpiItemContentId));

                var SubIndirectSalesOrderTransactionDAOs = IndirectSalesOrderTransactionDAOs.Where(x => x.ItemId == item.Id).ToList();
                var SubDirectSalesOrderTransactionDAOs = DirectSalesOrderTransactionDAOs.Where(x => x.ItemId == item.Id).ToList();

                PermissionMobile_EmployeeKpiItemReportDTO.ItemId = item.Id;
                PermissionMobile_EmployeeKpiItemReportDTO.ItemName = item.Name;
                PermissionMobile_EmployeeKpiItemReportDTO.CurrentKpiItems = new List<PermissionMobile_EmployeeKpiItem>();

                foreach (var kpiCriteriaItem in KpiCriteriaItems)
                {
                    PermissionMobile_EmployeeKpiItem PermissionMobile_EmployeeKpiItem = new PermissionMobile_EmployeeKpiItem();
                    PermissionMobile_EmployeeKpiItem.ItemId = item.Id;
                    PermissionMobile_EmployeeKpiItem.KpiCriteriaItemName = kpiCriteriaItem.Name;
                    PermissionMobile_EmployeeKpiItem.PlannedValue = SubKpiItemContentKpiCriteriaItemMappingDAOs
                        .Where(x => x.KpiCriteriaItemId == kpiCriteriaItem.Id)
                        .Select(x => x.Value).FirstOrDefault();

                    if (kpiCriteriaItem.Id == KpiCriteriaItemEnum.INDIRECT_REVENUE.Id)
                        PermissionMobile_EmployeeKpiItem.CurrentValue = SubIndirectSalesOrderTransactionDAOs.Sum(x => (x.Amount - x.GeneralDiscountAmount ?? 0));
                    if (kpiCriteriaItem.Id == KpiCriteriaItemEnum.INDIRECT_STORE.Id)
                        PermissionMobile_EmployeeKpiItem.CurrentValue = SubIndirectSalesOrderTransactionDAOs.Select(x => x.BuyerStoreId).Distinct().Count();
                    if (kpiCriteriaItem.Id == KpiCriteriaItemEnum.DIRECT_REVENUE.Id)
                        PermissionMobile_EmployeeKpiItem.CurrentValue = SubDirectSalesOrderTransactionDAOs.Sum(x => (x.Amount - x.GeneralDiscountAmount ?? 0));
                    if (kpiCriteriaItem.Id == KpiCriteriaItemEnum.DIRECT_STORE.Id)
                        PermissionMobile_EmployeeKpiItem.CurrentValue = SubDirectSalesOrderTransactionDAOs.Select(x => x.BuyerStoreId).Distinct().Count();
                    PermissionMobile_EmployeeKpiItem.Percentage = PermissionMobile_EmployeeKpiItem.PlannedValue == null || PermissionMobile_EmployeeKpiItem.CurrentValue == null || PermissionMobile_EmployeeKpiItem.PlannedValue.Value == 0
                            ? null
                            : (decimal?)Math.Round((PermissionMobile_EmployeeKpiItem.CurrentValue.Value / PermissionMobile_EmployeeKpiItem.PlannedValue.Value) * 100, 2);
                    PermissionMobile_EmployeeKpiItemReportDTO.CurrentKpiItems.Add(PermissionMobile_EmployeeKpiItem);
                }
                PermissionMobile_EmployeeKpiItemReportDTOs.Add(PermissionMobile_EmployeeKpiItemReportDTO);
            }

            return PermissionMobile_EmployeeKpiItemReportDTOs;
        }

        [Route(PermissionMobileRoute.ListCurrentKpiProductGrouping), HttpPost]
        public async Task<List<PermissionMobile_EmployeeKpiProductGroupingReportDTO>> ListCurrentKpiProductGrouping([FromBody] PermissionMobile_EmployeeKpiProductGroupingReportFilterDTO PermissionMobile_EmployeeKpiProductGroupingReportFilterDTO)
        {
            var PermissionMobile_EmployeeKpiProductGroupingReportDTOs = new List<PermissionMobile_EmployeeKpiProductGroupingReportDTO>();

            #region chuẩn bị dữ liệu filter: thời gian tính kpi, AppUserIds
            // lây ra tháng hiện tại + năm hiện tại
            long KpiYearId = StaticParams.DateTimeNow.Year;
            long KpiPeriodId = StaticParams.DateTimeNow.Month + 100;
            GenericEnum CurrentMonth;
            GenericEnum CurrentQuarter;
            GenericEnum CurrentYear;
            (CurrentMonth, CurrentQuarter, CurrentYear) = ConvertDateTime(StaticParams.DateTimeNow);
            DateTime Start = new DateTime(StaticParams.DateTimeNow.Year, StaticParams.DateTimeNow.Month, 1);
            Start = Start.AddHours(0 - CurrentContext.TimeZone);
            DateTime End = new DateTime(StaticParams.DateTimeNow.Year, StaticParams.DateTimeNow.Month, 1).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);

            List<long> AppUserIds = new List<long>();
            if (PermissionMobile_EmployeeKpiProductGroupingReportFilterDTO.EmployeeId.Equal.HasValue)
            {
                AppUserIds.Add(PermissionMobile_EmployeeKpiProductGroupingReportFilterDTO.EmployeeId.Equal.Value);
            }
            else
            {
                List<long> Ids = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
                AppUserIds.AddRange(Ids);
            }
            IdFilter AppUserFilter = new IdFilter() { In = AppUserIds };
            DateFilter OrderDateFilter = new DateFilter() { GreaterEqual = Start, LessEqual = End };
            #endregion

            #region lấy giữ liệu kpi
            var kpiproductgrouping_query = DataContext.KpiProductGrouping.AsNoTracking();
            kpiproductgrouping_query = kpiproductgrouping_query.Where(x => x.EmployeeId, AppUserFilter);
            kpiproductgrouping_query = kpiproductgrouping_query.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });
            kpiproductgrouping_query = kpiproductgrouping_query.Where(x => x.KpiPeriodId, new IdFilter { Equal = KpiPeriodId });
            kpiproductgrouping_query = kpiproductgrouping_query.Where(x => x.KpiYearId, new IdFilter { Equal = KpiYearId });
            kpiproductgrouping_query = kpiproductgrouping_query.Where(x => x.KpiProductGroupingTypeId, new IdFilter { Equal = KpiItemTypeEnum.ALL_PRODUCT.Id });
            kpiproductgrouping_query = kpiproductgrouping_query.Where(x => x.DeletedAt == null);
            List<KpiProductGroupingDAO> KpiProductGroupingDAOs = await kpiproductgrouping_query
                .Select(x => new KpiProductGroupingDAO
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId
                }).ToListWithNoLockAsync();
            #endregion

            #region tổng hợp dữ liệu
            if (KpiProductGroupingDAOs.Count == 0)
                return PermissionMobile_EmployeeKpiProductGroupingReportDTOs;

            List<long> KpiProductGroupingIds = KpiProductGroupingDAOs.Select(x => x.Id).ToList();
            var kpiproductgroupingcontent_query = DataContext.KpiProductGroupingContent.AsNoTracking();
            kpiproductgroupingcontent_query = kpiproductgroupingcontent_query.Where(x => x.KpiProductGroupingId, new IdFilter { In = KpiProductGroupingIds });
            List<KpiProductGroupingContentDAO> KpiProductGroupingContentDAOs = await kpiproductgroupingcontent_query.ToListWithNoLockAsync(); // lấy ra toàn bộ content của kpi
            if (KpiProductGroupingContentDAOs.Count == 0)
                return PermissionMobile_EmployeeKpiProductGroupingReportDTOs;

            List<long> KpiProductGroupingContentIds = KpiProductGroupingContentDAOs.Select(x => x.Id).ToList();

            var kpiproductgroupingcontent_criteriamapping_query = DataContext.KpiProductGroupingContentCriteriaMapping.AsNoTracking();
            kpiproductgroupingcontent_criteriamapping_query = kpiproductgroupingcontent_criteriamapping_query.Where(x => x.KpiProductGroupingContentId, new IdFilter { In = KpiProductGroupingContentIds });
            List<KpiProductGroupingContentCriteriaMappingDAO> KpiProductGroupingContentCriteriaMappingDAOs = await kpiproductgroupingcontent_criteriamapping_query.ToListWithNoLockAsync(); // lấy ra toàn bộ mapping content với chỉ tiêu

            var kpiproductgroupingcontent_itemmapping = DataContext.KpiProductGroupingContentItemMapping.AsNoTracking();
            kpiproductgroupingcontent_itemmapping = kpiproductgroupingcontent_itemmapping.Where(x => x.KpiProductGroupingContentId, new IdFilter { In = KpiProductGroupingContentIds });
            List<KpiProductGroupingContentItemMappingDAO> KpiProductGroupingContentItemMappingDAOs = await kpiproductgroupingcontent_itemmapping.ToListWithNoLockAsync(); // lấy ra toàn bộ mapping content với Item

            if (KpiProductGroupingContentCriteriaMappingDAOs.Count == 0 && KpiProductGroupingContentItemMappingDAOs.Count == 0)
                return PermissionMobile_EmployeeKpiProductGroupingReportDTOs;

            List<long> ProductGroupingIds = KpiProductGroupingContentDAOs.Select(x => x.ProductGroupingId)
                .Distinct()
                .ToList();
            List<long> ProductGroupingSelectAllItemIds = KpiProductGroupingContentDAOs
                .Where(x => x.SelectAllCurrentItem).Select(x => x.ProductGroupingId)
                .Distinct()
                .ToList();
            var ProductProductGroupingMapping = await DataContext.ProductProductGroupingMapping.AsNoTracking()
                    .Where(x => x.ProductGroupingId, new IdFilter { In = ProductGroupingSelectAllItemIds })
                    .Select(x => new { x.ProductGroupingId, x.ProductId }).ToListWithNoLockAsync();
            var ItemOfProductGroupingSelectAllItems = await DataContext.Item.AsNoTracking()
                .Where(x => x.ProductId, new IdFilter { In = ProductProductGroupingMapping.Select(x => x.ProductId).Distinct().ToList() })
                .Select(x => new
                {
                    x.ProductId,
                    x.Id
                }).Distinct().ToListWithNoLockAsync();
            List<long> ItemIds = KpiProductGroupingContentItemMappingDAOs.Select(x => x.ItemId)
                .Distinct()
                .ToList();
            ItemIds.AddRange(ItemOfProductGroupingSelectAllItems.Select(x => x.Id).Distinct().ToList());
            var productgrouping_query = DataContext.ProductGrouping.AsNoTracking();
            productgrouping_query = productgrouping_query.Where(x => x.Id, new IdFilter { In = ProductGroupingIds });
            List<ProductGroupingDAO> ProductGroupingDAOs = await productgrouping_query.ToListWithNoLockAsync();

            // DirectSalesOrderTransaction
            var direct_sales_order_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            direct_sales_order_query = direct_sales_order_query.Where(q => q.OrderDate, OrderDateFilter);
            direct_sales_order_query = direct_sales_order_query.Where(q => q.SaleEmployeeId, AppUserFilter);
            direct_sales_order_query = direct_sales_order_query.Where(q => q.GeneralApprovalStateId, new IdFilter()
            {
                In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });
            var direct_sales_order_transaction_query = from tr in DWContext.Fact_DirectSalesOrderTransaction
                                                       join dr in direct_sales_order_query on tr.DirectSalesOrderId equals dr.DirectSalesOrderId
                                                       select tr;
            var DirectSalesOrderTransactionDAOs = await direct_sales_order_transaction_query.ToListWithNoLockAsync();

            // IndirectSalesOrderTransaction
            IdFilter ItemIdFilter = new IdFilter() { In = ItemIds };
            var indirect_sales_order_transaction = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            indirect_sales_order_transaction = indirect_sales_order_transaction.Where(q => q.OrderDate, OrderDateFilter);
            indirect_sales_order_transaction = indirect_sales_order_transaction.Where(q => q.SalesEmployeeId, AppUserFilter);
            indirect_sales_order_transaction = indirect_sales_order_transaction.Where(q => q.RequestStateId, new IdFilter() { Equal = RequestStateEnum.APPROVED.Id });
            indirect_sales_order_transaction = indirect_sales_order_transaction.Where(q => q.ItemId, ItemIdFilter);
            indirect_sales_order_transaction = indirect_sales_order_transaction.Where(x => !x.DeletedAt.HasValue);

            var InDirectSalesOrderTransactionDAOs = await indirect_sales_order_transaction.Distinct().ToListWithNoLockAsync(); // lấy ra toàn bộ đơn hàng thỏa mãn điều kiện thời gian, nhân viên, Item
            List<KpiProductGroupingCriteriaDAO> KpiProductGroupingCriterias = await DataContext.KpiProductGroupingCriteria.
                Where(x => x.StatusId == StatusEnum.ACTIVE.Id).ToListWithNoLockAsync(); // lấy ra toàn bộ chỉ tiêu kpi

            List<PermissionMobile_EmployeeKpiProductGroupingReportDTO> SubResults = new List<PermissionMobile_EmployeeKpiProductGroupingReportDTO>(); // lấy ra kết quả chưa group theo ProductGrouping


            foreach (var productgrouping in ProductGroupingDAOs)
            {
                var subKpiProductGroupingContents = KpiProductGroupingContentDAOs.Where(x => x.ProductGroupingId == productgrouping.Id).ToList();
                List<long> subKpiProductGroupingContentIds = subKpiProductGroupingContents.Select(x => x.Id).ToList();
                var subKpiProductGroupingContentCriteriaMappings = KpiProductGroupingContentCriteriaMappingDAOs
                    .Where(x => subKpiProductGroupingContentIds.Contains(x.KpiProductGroupingContentId));

                List<long> SubProductIds = ProductProductGroupingMapping.Where(x => x.ProductGroupingId == productgrouping.Id)
                                .Select(x => x.ProductId).ToList();
                var subItemOfProductGroupingSelectAllItems = ItemOfProductGroupingSelectAllItems.Where(x => SubProductIds.Contains(x.ProductId)).ToList();

                var subKpiProductGroupingContentItemMappings = KpiProductGroupingContentItemMappingDAOs
                    .Where(x => subKpiProductGroupingContentIds.Contains(x.KpiProductGroupingContentId));

                List<long> SubItemIds = new List<long>();
                if (subKpiProductGroupingContents[0].SelectAllCurrentItem)
                {
                    SubItemIds = subItemOfProductGroupingSelectAllItems.Select(x => x.Id).Distinct().ToList();
                }
                else
                {
                    SubItemIds = subKpiProductGroupingContentItemMappings.Select(x => x.ItemId).Distinct().ToList();
                }


                var subIndirectSalesOrderTransactions = InDirectSalesOrderTransactionDAOs.Where(x => SubItemIds.Contains(x.ItemId)).ToList();
                var subDirectSalesOrderTransactions = DirectSalesOrderTransactionDAOs.Where(x => SubItemIds.Contains(x.ItemId)).ToList();

                var PermissionMobile_EmployeeKpiProductGroupingReportDTO = new PermissionMobile_EmployeeKpiProductGroupingReportDTO();
                PermissionMobile_EmployeeKpiProductGroupingReportDTO.ProductGroupingId = productgrouping.Id;
                PermissionMobile_EmployeeKpiProductGroupingReportDTO.ProductGroupingName = productgrouping.Name;
                PermissionMobile_EmployeeKpiProductGroupingReportDTO.CurrentKpiProductGroupings = new List<PermissionMobile_KpiProductGroupingContent>();

                foreach (var KpiProductGroupingCriteria in KpiProductGroupingCriterias)
                {
                    PermissionMobile_KpiProductGroupingContent PermissionMobile_KpiProductGroupingContent = new PermissionMobile_KpiProductGroupingContent();
                    PermissionMobile_KpiProductGroupingContent.ProductGroupingId = productgrouping.Id;
                    PermissionMobile_KpiProductGroupingContent.KpiProductGroupingCriteriaName = KpiProductGroupingCriteria.Name;
                    PermissionMobile_KpiProductGroupingContent.PlannedValue = subKpiProductGroupingContentCriteriaMappings
                        .Where(x => x.KpiProductGroupingCriteriaId == KpiProductGroupingCriteria.Id).Select(x => x.Value).FirstOrDefault();

                    if (KpiProductGroupingCriteria.Id == KpiProductGroupingCriteriaEnum.INDIRECT_REVENUE.Id)
                    {
                        PermissionMobile_KpiProductGroupingContent.CurrentValue = subIndirectSalesOrderTransactions.Sum(x => (x.Amount - x.GeneralDiscountAmount ?? 0));
                    }
                    if (KpiProductGroupingCriteria.Id == KpiProductGroupingCriteriaEnum.INDIRECT_STORE.Id)
                    {
                        PermissionMobile_KpiProductGroupingContent.CurrentValue = subIndirectSalesOrderTransactions.Select(x => x.BuyerStoreId).Distinct().Count();
                    }
                    if (KpiProductGroupingCriteria.Id == KpiProductGroupingCriteriaEnum.DIRECT_REVENUE.Id)
                    {
                        PermissionMobile_KpiProductGroupingContent.CurrentValue = subDirectSalesOrderTransactions.Sum(x => (x.Amount - x.GeneralDiscountAmount ?? 0));
                    }
                    if (KpiProductGroupingCriteria.Id == KpiProductGroupingCriteriaEnum.DIRECT_STORE.Id)
                    {
                        PermissionMobile_KpiProductGroupingContent.CurrentValue = subDirectSalesOrderTransactions.Select(x => x.BuyerStoreId).Distinct().Count();
                    }
                    PermissionMobile_KpiProductGroupingContent.Percentage = PermissionMobile_KpiProductGroupingContent.PlannedValue == null || PermissionMobile_KpiProductGroupingContent.CurrentValue == null || PermissionMobile_KpiProductGroupingContent.PlannedValue.Value == 0
                            ? null
                            : (decimal?)Math.Round((PermissionMobile_KpiProductGroupingContent.CurrentValue.Value / PermissionMobile_KpiProductGroupingContent.PlannedValue.Value) * 100, 2);

                    PermissionMobile_EmployeeKpiProductGroupingReportDTO.CurrentKpiProductGroupings.Add(PermissionMobile_KpiProductGroupingContent);
                }
                PermissionMobile_EmployeeKpiProductGroupingReportDTOs.Add(PermissionMobile_EmployeeKpiProductGroupingReportDTO);
            }
            #endregion

            return PermissionMobile_EmployeeKpiProductGroupingReportDTOs;
        }

        [Route(PermissionMobileRoute.ListCurrentKpiNewProductGrouping), HttpPost]
        public async Task<List<PermissionMobile_EmployeeKpiProductGroupingReportDTO>> ListCurrentKpiNewProductGrouping([FromBody] PermissionMobile_EmployeeKpiProductGroupingReportFilterDTO PermissionMobile_EmployeeKpiProductGroupingReportFilterDTO)
        {
            var PermissionMobile_EmployeeKpiProductGroupingReportDTOs = new List<PermissionMobile_EmployeeKpiProductGroupingReportDTO>();

            #region chuẩn bị dữ liệu filter: thời gian tính kpi, AppUserIds
            // lây ra tháng hiện tại + năm hiện tại
            long KpiYearId = StaticParams.DateTimeNow.Year;
            long KpiPeriodId = StaticParams.DateTimeNow.Month + 100;
            GenericEnum CurrentMonth;
            GenericEnum CurrentQuarter;
            GenericEnum CurrentYear;
            (CurrentMonth, CurrentQuarter, CurrentYear) = ConvertDateTime(StaticParams.DateTimeNow);
            DateTime Start = new DateTime(StaticParams.DateTimeNow.Year, StaticParams.DateTimeNow.Month, 1);
            Start = Start.AddHours(0 - CurrentContext.TimeZone);
            DateTime End = new DateTime(StaticParams.DateTimeNow.Year, StaticParams.DateTimeNow.Month, 1).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);

            List<long> AppUserIds = new List<long>();
            if (PermissionMobile_EmployeeKpiProductGroupingReportFilterDTO.EmployeeId.Equal.HasValue)
            {
                AppUserIds.Add(PermissionMobile_EmployeeKpiProductGroupingReportFilterDTO.EmployeeId.Equal.Value);
            }
            else
            {
                List<long> Ids = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
                AppUserIds.AddRange(Ids);
            }
            IdFilter AppUserFilter = new IdFilter() { In = AppUserIds };
            DateFilter OrderDateFilter = new DateFilter() { GreaterEqual = Start, LessEqual = End };
            #endregion

            #region lấy giữ liệu kpi
            var kpiproductgrouping_query = DataContext.KpiProductGrouping.AsNoTracking();
            kpiproductgrouping_query = kpiproductgrouping_query.Where(x => x.EmployeeId, AppUserFilter);
            kpiproductgrouping_query = kpiproductgrouping_query.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });
            kpiproductgrouping_query = kpiproductgrouping_query.Where(x => x.KpiPeriodId, new IdFilter { Equal = KpiPeriodId });
            kpiproductgrouping_query = kpiproductgrouping_query.Where(x => x.KpiYearId, new IdFilter { Equal = KpiYearId });
            kpiproductgrouping_query = kpiproductgrouping_query.Where(x => x.KpiProductGroupingTypeId, new IdFilter { Equal = KpiItemTypeEnum.NEW_PRODUCT.Id });
            kpiproductgrouping_query = kpiproductgrouping_query.Where(x => x.DeletedAt == null);
            List<KpiProductGroupingDAO> KpiProductGroupingDAOs = await kpiproductgrouping_query
                .Select(x => new KpiProductGroupingDAO
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId
                }).ToListWithNoLockAsync();
            #endregion

            #region tổng hợp dữ liệu
            if (KpiProductGroupingDAOs.Count == 0)
                return PermissionMobile_EmployeeKpiProductGroupingReportDTOs;

            List<long> KpiProductGroupingIds = KpiProductGroupingDAOs.Select(x => x.Id).ToList();
            var kpiproductgroupingcontent_query = DataContext.KpiProductGroupingContent.AsNoTracking();
            kpiproductgroupingcontent_query = kpiproductgroupingcontent_query.Where(x => x.KpiProductGroupingId, new IdFilter { In = KpiProductGroupingIds });
            List<KpiProductGroupingContentDAO> KpiProductGroupingContentDAOs = await kpiproductgroupingcontent_query.ToListWithNoLockAsync(); // lấy ra toàn bộ content của kpi
            if (KpiProductGroupingContentDAOs.Count == 0)
                return PermissionMobile_EmployeeKpiProductGroupingReportDTOs;

            List<long> KpiProductGroupingContentIds = KpiProductGroupingContentDAOs.Select(x => x.Id).ToList();

            var kpiproductgroupingcontent_criteriamapping_query = DataContext.KpiProductGroupingContentCriteriaMapping.AsNoTracking();
            kpiproductgroupingcontent_criteriamapping_query = kpiproductgroupingcontent_criteriamapping_query.Where(x => x.KpiProductGroupingContentId, new IdFilter { In = KpiProductGroupingContentIds });
            List<KpiProductGroupingContentCriteriaMappingDAO> KpiProductGroupingContentCriteriaMappingDAOs = await kpiproductgroupingcontent_criteriamapping_query.ToListWithNoLockAsync(); // lấy ra toàn bộ mapping content với chỉ tiêu

            var kpiproductgroupingcontent_itemmapping = DataContext.KpiProductGroupingContentItemMapping.AsNoTracking();
            kpiproductgroupingcontent_itemmapping = kpiproductgroupingcontent_itemmapping.Where(x => x.KpiProductGroupingContentId, new IdFilter { In = KpiProductGroupingContentIds });
            List<KpiProductGroupingContentItemMappingDAO> KpiProductGroupingContentItemMappingDAOs = await kpiproductgroupingcontent_itemmapping.ToListWithNoLockAsync(); // lấy ra toàn bộ mapping content với Item

            if (KpiProductGroupingContentCriteriaMappingDAOs.Count == 0 && KpiProductGroupingContentItemMappingDAOs.Count == 0)
                return PermissionMobile_EmployeeKpiProductGroupingReportDTOs;

            List<long> ProductGroupingIds = KpiProductGroupingContentDAOs.Select(x => x.ProductGroupingId)
                .Distinct()
                .ToList();
            List<long> ProductGroupingSelectAllItemIds = KpiProductGroupingContentDAOs
                .Where(x => x.SelectAllCurrentItem).Select(x => x.ProductGroupingId)
                .Distinct()
                .ToList();
            var ProductProductGroupingMapping = await DataContext.ProductProductGroupingMapping.AsNoTracking()
                    .Where(x => x.ProductGroupingId, new IdFilter { In = ProductGroupingSelectAllItemIds })
                    .Where(x => x.Product.IsNew)
                    .Select(x => new { x.ProductGroupingId, x.ProductId }).ToListWithNoLockAsync();
            var ItemOfProductGroupingSelectAllItems = await DataContext.Item.AsNoTracking()
                .Where(x => x.ProductId, new IdFilter { In = ProductProductGroupingMapping.Select(x => x.ProductId).Distinct().ToList() })
                .Select(x => new
                {
                    x.ProductId,
                    x.Id
                }).Distinct().ToListWithNoLockAsync();
            List<long> ItemIds = KpiProductGroupingContentItemMappingDAOs.Select(x => x.ItemId)
                .Distinct()
                .ToList();
            ItemIds.AddRange(ItemOfProductGroupingSelectAllItems.Select(x => x.Id).Distinct().ToList());
            var productgrouping_query = DataContext.ProductGrouping.AsNoTracking();
            productgrouping_query = productgrouping_query.Where(x => x.Id, new IdFilter { In = ProductGroupingIds });
            List<ProductGroupingDAO> ProductGroupingDAOs = await productgrouping_query.ToListWithNoLockAsync();

            // DirectSalesOrderTransaction
            var direct_sales_order_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            direct_sales_order_query = direct_sales_order_query.Where(q => q.OrderDate, OrderDateFilter);
            direct_sales_order_query = direct_sales_order_query.Where(q => q.SaleEmployeeId, AppUserFilter);
            direct_sales_order_query = direct_sales_order_query.Where(q => q.GeneralApprovalStateId, new IdFilter()
            {
                In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });
            var direct_sales_order_transaction_query = from tr in DWContext.Fact_DirectSalesOrderTransaction
                                                       join dr in direct_sales_order_query on tr.DirectSalesOrderId equals dr.DirectSalesOrderId
                                                       select tr;
            var DirectSalesOrderTransactionDAOs = await direct_sales_order_transaction_query.ToListWithNoLockAsync();

            // IndirectSalesOrderTransaction
            IdFilter ItemIdFilter = new IdFilter() { In = ItemIds };
            var indirect_sales_order_transaction = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            indirect_sales_order_transaction = indirect_sales_order_transaction.Where(q => q.OrderDate, OrderDateFilter);
            indirect_sales_order_transaction = indirect_sales_order_transaction.Where(q => q.SalesEmployeeId, AppUserFilter);
            indirect_sales_order_transaction = indirect_sales_order_transaction.Where(q => q.RequestStateId, new IdFilter() { Equal = RequestStateEnum.APPROVED.Id });
            indirect_sales_order_transaction = indirect_sales_order_transaction.Where(q => q.ItemId, ItemIdFilter);
            indirect_sales_order_transaction = indirect_sales_order_transaction.Where(x => !x.DeletedAt.HasValue);

            var InDirectSalesOrderTransactionDAOs = await indirect_sales_order_transaction.Distinct().ToListWithNoLockAsync(); // lấy ra toàn bộ đơn hàng thỏa mãn điều kiện thời gian, nhân viên, Item
            List<KpiProductGroupingCriteriaDAO> KpiProductGroupingCriterias = await DataContext.KpiProductGroupingCriteria.
                Where(x => x.StatusId == StatusEnum.ACTIVE.Id).ToListWithNoLockAsync(); // lấy ra toàn bộ chỉ tiêu kpi

            List<PermissionMobile_EmployeeKpiProductGroupingReportDTO> SubResults = new List<PermissionMobile_EmployeeKpiProductGroupingReportDTO>(); // lấy ra kết quả chưa group theo ProductGrouping


            foreach (var productgrouping in ProductGroupingDAOs)
            {
                var subKpiProductGroupingContents = KpiProductGroupingContentDAOs.Where(x => x.ProductGroupingId == productgrouping.Id).ToList();
                List<long> subKpiProductGroupingContentIds = subKpiProductGroupingContents.Select(x => x.Id).ToList();
                var subKpiProductGroupingContentCriteriaMappings = KpiProductGroupingContentCriteriaMappingDAOs
                    .Where(x => subKpiProductGroupingContentIds.Contains(x.KpiProductGroupingContentId));

                List<long> SubProductIds = ProductProductGroupingMapping.Where(x => x.ProductGroupingId == productgrouping.Id)
                                .Select(x => x.ProductId).ToList();
                var subItemOfProductGroupingSelectAllItems = ItemOfProductGroupingSelectAllItems.Where(x => SubProductIds.Contains(x.ProductId)).ToList();

                var subKpiProductGroupingContentItemMappings = KpiProductGroupingContentItemMappingDAOs
                    .Where(x => subKpiProductGroupingContentIds.Contains(x.KpiProductGroupingContentId));

                List<long> SubItemIds = new List<long>();
                if (subKpiProductGroupingContents[0].SelectAllCurrentItem)
                {
                    SubItemIds = subItemOfProductGroupingSelectAllItems.Select(x => x.Id).Distinct().ToList();
                }
                else
                {
                    SubItemIds = subKpiProductGroupingContentItemMappings.Select(x => x.ItemId).Distinct().ToList();
                }


                var subIndirectSalesOrderTransactions = InDirectSalesOrderTransactionDAOs.Where(x => SubItemIds.Contains(x.ItemId)).ToList();
                var subDirectSalesOrderTransactions = DirectSalesOrderTransactionDAOs.Where(x => SubItemIds.Contains(x.ItemId)).ToList();

                var PermissionMobile_EmployeeKpiProductGroupingReportDTO = new PermissionMobile_EmployeeKpiProductGroupingReportDTO();
                PermissionMobile_EmployeeKpiProductGroupingReportDTO.ProductGroupingId = productgrouping.Id;
                PermissionMobile_EmployeeKpiProductGroupingReportDTO.ProductGroupingName = productgrouping.Name;
                PermissionMobile_EmployeeKpiProductGroupingReportDTO.CurrentKpiProductGroupings = new List<PermissionMobile_KpiProductGroupingContent>();

                foreach (var KpiProductGroupingCriteria in KpiProductGroupingCriterias)
                {
                    PermissionMobile_KpiProductGroupingContent PermissionMobile_KpiProductGroupingContent = new PermissionMobile_KpiProductGroupingContent();
                    PermissionMobile_KpiProductGroupingContent.ProductGroupingId = productgrouping.Id;
                    PermissionMobile_KpiProductGroupingContent.KpiProductGroupingCriteriaName = KpiProductGroupingCriteria.Name;
                    PermissionMobile_KpiProductGroupingContent.PlannedValue = subKpiProductGroupingContentCriteriaMappings
                        .Where(x => x.KpiProductGroupingCriteriaId == KpiProductGroupingCriteria.Id).Select(x => x.Value).FirstOrDefault();

                    if (KpiProductGroupingCriteria.Id == KpiProductGroupingCriteriaEnum.INDIRECT_REVENUE.Id)
                    {
                        PermissionMobile_KpiProductGroupingContent.CurrentValue = subIndirectSalesOrderTransactions.Sum(x => (x.Amount - x.GeneralDiscountAmount ?? 0));
                    }
                    if (KpiProductGroupingCriteria.Id == KpiProductGroupingCriteriaEnum.INDIRECT_STORE.Id)
                    {
                        PermissionMobile_KpiProductGroupingContent.CurrentValue = subIndirectSalesOrderTransactions.Select(x => x.BuyerStoreId).Distinct().Count();
                    }
                    if (KpiProductGroupingCriteria.Id == KpiProductGroupingCriteriaEnum.DIRECT_REVENUE.Id)
                    {
                        PermissionMobile_KpiProductGroupingContent.CurrentValue = subDirectSalesOrderTransactions.Sum(x => (x.Amount - x.GeneralDiscountAmount ?? 0));
                    }
                    if (KpiProductGroupingCriteria.Id == KpiProductGroupingCriteriaEnum.DIRECT_STORE.Id)
                    {
                        PermissionMobile_KpiProductGroupingContent.CurrentValue = subDirectSalesOrderTransactions.Select(x => x.BuyerStoreId).Distinct().Count();
                    }
                    PermissionMobile_KpiProductGroupingContent.Percentage = PermissionMobile_KpiProductGroupingContent.PlannedValue == null || PermissionMobile_KpiProductGroupingContent.CurrentValue == null || PermissionMobile_KpiProductGroupingContent.PlannedValue.Value == 0
                            ? null
                            : (decimal?)Math.Round((PermissionMobile_KpiProductGroupingContent.CurrentValue.Value / PermissionMobile_KpiProductGroupingContent.PlannedValue.Value) * 100, 2);

                    PermissionMobile_EmployeeKpiProductGroupingReportDTO.CurrentKpiProductGroupings.Add(PermissionMobile_KpiProductGroupingContent);
                }
                PermissionMobile_EmployeeKpiProductGroupingReportDTOs.Add(PermissionMobile_EmployeeKpiProductGroupingReportDTO);
            }
            #endregion

            return PermissionMobile_EmployeeKpiProductGroupingReportDTOs;
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
        #endregion

        #region Dashboard Order: Count Store and StoreChecking
        [Route(PermissionMobileRoute.SingleListPeriod), HttpPost]
        public async Task<List<PermissionMobile_EnumList>> SingleListPeriod()
        {
            List<PermissionMobile_EnumList> Periods = new List<PermissionMobile_EnumList>();
            Periods.Add(new PermissionMobile_EnumList { Id = TODAY, Name = "Hôm nay" });
            Periods.Add(new PermissionMobile_EnumList { Id = THIS_WEEK, Name = "Tuần này" });
            Periods.Add(new PermissionMobile_EnumList { Id = THIS_MONTH, Name = "Tháng này" });
            //Periods.Add(new PermissionMobile_EnumList { Id = LAST_MONTH, Name = "Tháng trước" });
            Periods.Add(new PermissionMobile_EnumList { Id = THIS_QUARTER, Name = "Quý này" });
            //Periods.Add(new PermissionMobile_EnumList { Id = LAST_QUATER, Name = "Quý trước" });
            Periods.Add(new PermissionMobile_EnumList { Id = YEAR, Name = "Năm" });
            return Periods;
        }
        [Route(PermissionMobileRoute.CountStoreChecking), HttpPost]
        public async Task<long> CountStoreChecking([FromBody] PermissionMobile_FilterDTO filter)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            var storechecking_query = DWContext.Fact_StoreChecking.AsNoTracking();
            storechecking_query = storechecking_query.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            storechecking_query = storechecking_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            storechecking_query = storechecking_query.Where(x => x.CheckOutAt, DateFilter);

            var count = await storechecking_query.CountWithNoLockAsync();
            return count;
        } // số lượt viếng thăm theo nhân viên được phân quyền

        [Route(PermissionMobileRoute.CountStore), HttpPost]
        public async Task<long> CountStore([FromBody] PermissionMobile_FilterDTO filter)
        {
            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            var storechecking_query = DWContext.Fact_StoreChecking.AsNoTracking();
            storechecking_query = storechecking_query.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            storechecking_query = storechecking_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            storechecking_query = storechecking_query.Where(x => x.CheckOutAt, DateFilter);

            var count = await storechecking_query.Select(x => x.StoreId)
                .Distinct()
                .CountWithNoLockAsync();
            return count;
        } // số đại lý ghé thăm
        #endregion

        #region Dashboard Order: IndirectSalesOrder
        [Route(PermissionMobileRoute.IndirectSalesOrderTotalQuantity), HttpPost]
        public async Task<decimal> IndirectSalesOrderTotalQuantity([FromBody] PermissionMobile_FilterDTO filter)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            var IndirectSalesOrder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });
            List<long> IndirectSalesOrderIds = await IndirectSalesOrder_query.Select(x => x.IndirectSalesOrderId).ToListWithNoLockAsync();
            var IndirectSalesOrderTransaction_query = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            IndirectSalesOrderTransaction_query = IndirectSalesOrderTransaction_query.Where(x => x.DeletedAt == null);
            IndirectSalesOrderTransaction_query = IndirectSalesOrderTransaction_query.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
            IndirectSalesOrderTransaction_query = IndirectSalesOrderTransaction_query.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id });

            return await IndirectSalesOrderTransaction_query.SumAsync(x => x.Quantity);
        } // Tổng sản lượng ĐGT

        [Route(PermissionMobileRoute.IndirectSalesOrderItemAmount), HttpPost]
        public async Task<long> IndirectSalesOrderItemAmount([FromBody] PermissionMobile_FilterDTO filter)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            var IndirectSalesOrder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });
            List<long> IndirectSalesOrderIds = await IndirectSalesOrder_query.Select(x => x.IndirectSalesOrderId).ToListWithNoLockAsync();
            var IndirectSalesOrderTransaction_query = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            IndirectSalesOrderTransaction_query = IndirectSalesOrderTransaction_query.Where(x => x.DeletedAt == null);
            IndirectSalesOrderTransaction_query = IndirectSalesOrderTransaction_query.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
            IndirectSalesOrderTransaction_query = IndirectSalesOrderTransaction_query.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id });

            return await IndirectSalesOrderTransaction_query.Select(x => x.ItemId).Distinct().CountWithNoLockAsync();
        } // Tổng Sản phẩm bán ĐGT

        [Route(PermissionMobileRoute.CountIndirectSalesOrder), HttpPost]
        public async Task<long> CountIndirectSalesOrder([FromBody] PermissionMobile_FilterDTO filter)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            var IndirectSalesOrder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });

            return await IndirectSalesOrder_query.CountWithNoLockAsync();
        } // Tổng số ĐGT

        [Route(PermissionMobileRoute.IndirectSalesOrderRevenue), HttpPost]
        public async Task<decimal> IndirectSalesOrderRevenue([FromBody] PermissionMobile_FilterDTO filter)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            var IndirectSalesOrder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });

            var results = await IndirectSalesOrder_query.Select(x => new IndirectSalesOrderDAO { Total = x.Total }).ToListWithNoLockAsync();
            return results.Select(x => x.Total)
                .DefaultIfEmpty(0)
                .Sum();
        } // Tổng doanh thu ĐGT

        [Route(PermissionMobileRoute.TopIndirectSaleEmployeeRevenue), HttpPost]
        public async Task<List<PermissionMobile_TopRevenueBySalesEmployeeDTO>> TopIndirectSaleEmployeeRevenue([FromBody] PermissionMobile_FilterDTO filter)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            var IndirectSalesOrder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });

            var transactionGroups = await IndirectSalesOrder_query.GroupBy(x => x.SaleEmployeeId).Select(x => new
            {
                SalesEmployeeId = x.Key,
                Revenue = x.Sum(x => x.Total)
            }).ToListWithNoLockAsync();
            //var query = from transaction in DataContext.IndirectSalesOrderTransaction
            //            join ind in DataContext.IndirectSalesOrder on transaction.IndirectSalesOrderId equals ind.Id
            //            where ind.RequestStateId == RequestStateEnum.APPROVED.Id
            //            && AppUserIds.Contains(ind.SaleEmployeeId)
            //            && transaction.OrderDate >= Start
            //            && transaction.OrderDate <= End
            //            group transaction by transaction.SalesEmployeeId into transGroup
            //            select transGroup; // query tu transaction don hang gian tiep co trang thai phe duyet hoan thanh

            //var transactionGroups = await query
            //    .Select(x => new
            //    {
            //        SalesEmployeeId = x.Key,
            //        Revenue = x.Sum(x => x.Revenue)
            //    })
            //    .ToListWithNoLockAsync();

            List<PermissionMobile_TopRevenueBySalesEmployeeDTO> Result = new List<PermissionMobile_TopRevenueBySalesEmployeeDTO>();
            List<long> UserIds = transactionGroups
                .Select(x => x.SalesEmployeeId)
                .ToList();
            List<AppUserDAO> AppUserDAOs = await DataContext.AppUser
                .Where(x => UserIds.Contains(x.Id))
                .ToListWithNoLockAsync();

            foreach (var groupItem in transactionGroups)
            {
                long SaleEmployeeId = groupItem.SalesEmployeeId;
                AppUserDAO SaleEmpolyee = AppUserDAOs
                    .Where(x => x.Id == SaleEmployeeId)
                    .FirstOrDefault();
                PermissionMobile_TopRevenueBySalesEmployeeDTO Item = new PermissionMobile_TopRevenueBySalesEmployeeDTO();
                Item.SaleEmployeeId = SaleEmployeeId;
                Item.SaleEmployeeName = SaleEmpolyee.DisplayName;
                Item.Revenue = groupItem.Revenue;
                Result.Add(Item);
            }
            Result = Result
                .Where(x => x.Revenue > 0)
                .OrderByDescending(x => x.Revenue)
                .Take(5)
                .ToList();

            return Result;
        } // top 5 doanh thu đơn gián tiếp theo nhân viên

        [Route(PermissionMobileRoute.TopIndirecProductRevenue), HttpPost]
        public async Task<List<PermissionMobile_TopRevenueByItemDTO>> TopIndirecProductRevenue([FromBody] PermissionMobile_FilterDTO filter)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            var IndirectSalesOrder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });
            List<long> IndirectSalesOrderIds = await IndirectSalesOrder_query.Select(x => x.IndirectSalesOrderId).ToListWithNoLockAsync();
            var IndirectSalesOrderTransaction_query = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            IndirectSalesOrderTransaction_query = IndirectSalesOrderTransaction_query.Where(x => x.DeletedAt == null);
            IndirectSalesOrderTransaction_query = IndirectSalesOrderTransaction_query.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });

            var transactionGroups = await IndirectSalesOrderTransaction_query.GroupBy(x => x.ItemId).Select(x => new
            {
                ItemId = x.Key,
                Revenue = x.Sum(x => x.Amount)
            }).ToListWithNoLockAsync();

            //var query = from transaction in DataContext.IndirectSalesOrderTransaction
            //            join ind in DataContext.IndirectSalesOrder on transaction.IndirectSalesOrderId equals ind.Id
            //            where ind.RequestStateId == RequestStateEnum.APPROVED.Id
            //            && AppUserIds.Contains(ind.SaleEmployeeId)
            //            && transaction.OrderDate >= Start
            //            && transaction.OrderDate <= End
            //            group transaction by transaction.ItemId into transGroup
            //            select transGroup; // query tu transaction don hang gian tiep co trang thai phe duyet hoan thanh

            //var transactionGroups = await query
            //    .Select(x => new DirectSalesOrderTransactionDAO
            //    {
            //        ItemId = x.Key,
            //        Revenue = x.Sum(x => x.Revenue)
            //    })
            //    .ToListWithNoLockAsync();

            List<PermissionMobile_TopRevenueByItemDTO> Result = new List<PermissionMobile_TopRevenueByItemDTO>();

            List<long> ItemIds = transactionGroups.Select(x => x.ItemId).ToList();
            List<Item> Items = await ItemService.MobileList(new ItemFilter
            {
                Id = new IdFilter { In = ItemIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = ItemSelect.Id | ItemSelect.Product
            });
            List<long> ProductIds = Items.Select(x => x.Product.Id).Distinct().ToList();
            List<Product> Products = await ProductService.MobileList(new ProductFilter
            {
                Id = new IdFilter { In = ProductIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProductSelect.Id | ProductSelect.Name
            });

            foreach (var groupItem in transactionGroups)
            {
                long ItemId = groupItem.ItemId;
                Item Item = Items
                    .Where(x => x.Id == ItemId)
                    .FirstOrDefault();
                if (Item != null)
                {
                    PermissionMobile_TopRevenueByItemDTO ResultItem = new PermissionMobile_TopRevenueByItemDTO();
                    ResultItem.ProductId = Item.Product.Id;
                    ResultItem.Revenue = groupItem.Revenue;
                    Result.Add(ResultItem);
                }
            }
            Result = Result
                .GroupBy(x => x.ProductId)
                .Select(y => new PermissionMobile_TopRevenueByItemDTO
                {
                    ProductId = y.Key,
                    Revenue = y.Sum(r => r.Revenue)
                })
                .ToList(); // group doanh thu theo id san pham
            foreach (var ResultItem in Result)
            {
                Product Product = Products.Where(x => x.Id == ResultItem.ProductId).FirstOrDefault();
                ResultItem.ProductName = Product.Name;
            }

            Result = Result
                .Where(x => x.Revenue > 0)
                .OrderByDescending(x => x.Revenue)
                .Take(5)
                .ToList();

            return Result;
        } // top 5 doanh thu đơn gián tiếp theo item

        [Route(PermissionMobileRoute.IndirectRevenueGrowth), HttpPost]
        public async Task<PermissionMobile_RevenueGrowthDTO> IndirectRevenueGrowth([FromBody] PermissionMobile_FilterDTO filter)
        {
            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            var IndirectSalesOrder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });

            var Transactions = await IndirectSalesOrder_query.Select(x => new
            {
                OrderDate = x.OrderDate,
                Revenue = x.Total
            }).ToListWithNoLockAsync();

            //var query = from t in DataContext.IndirectSalesOrderTransaction
            //            join i in DataContext.IndirectSalesOrder on t.IndirectSalesOrderId equals i.Id
            //            where i.RequestStateId == RequestStateEnum.APPROVED.Id
            //            && AppUserIds.Contains(i.SaleEmployeeId)
            //            && t.OrderDate >= Start
            //            && t.OrderDate <= End
            //            select new IndirectSalesOrderTransactionDAO
            //            {
            //                OrderDate = t.OrderDate,
            //                Revenue = t.Revenue
            //            };

            //var Transactions = await query.ToListWithNoLockAsync();

            int StartDay = Start.AddHours(CurrentContext.TimeZone).Day;
            int EndDay = End.AddHours(CurrentContext.TimeZone).Day;
            int StartMonth = Start.AddHours(CurrentContext.TimeZone).Month;
            int EndMonth = End.AddHours(CurrentContext.TimeZone).Month;

            if (filter.Time.Equal.HasValue == false
                || filter.Time.Equal.Value == THIS_MONTH)
            {
                PermissionMobile_RevenueGrowthDTO RevenueGrowthDTO = new PermissionMobile_RevenueGrowthDTO();
                RevenueGrowthDTO.IndirectRevenueGrowthByMonths = new List<PermissionMobile_RevenueGrowthByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = StartDay; i <= EndDay; i++)
                {
                    PermissionMobile_RevenueGrowthByMonthDTO RevenueGrowthByMonth = new PermissionMobile_RevenueGrowthByMonthDTO
                    {
                        Day = i,
                        Revenue = 0
                    };
                    RevenueGrowthDTO.IndirectRevenueGrowthByMonths.Add(RevenueGrowthByMonth);
                }

                foreach (var RevenueGrowthByMonth in RevenueGrowthDTO.IndirectRevenueGrowthByMonths)
                {
                    DateTime LocalStart = new DateTime(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Year, StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Month, (int)RevenueGrowthByMonth.Day).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddDays(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueGrowthByMonth.Revenue = Transactions.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Select(x => x.Revenue)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return RevenueGrowthDTO;
            }
            else if (filter.Time.Equal.Value == LAST_MONTH)
            {
                PermissionMobile_RevenueGrowthDTO RevenueGrowthDTO = new PermissionMobile_RevenueGrowthDTO();
                RevenueGrowthDTO.IndirectRevenueGrowthByMonths = new List<PermissionMobile_RevenueGrowthByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = StartDay; i <= EndDay; i++)
                {
                    PermissionMobile_RevenueGrowthByMonthDTO RevenueGrowthByMonth = new PermissionMobile_RevenueGrowthByMonthDTO
                    {
                        Day = i,
                        Revenue = 0
                    };
                    RevenueGrowthDTO.IndirectRevenueGrowthByMonths.Add(RevenueGrowthByMonth);
                }

                foreach (var RevenueGrowthByMonth in RevenueGrowthDTO.IndirectRevenueGrowthByMonths)
                {
                    DateTime LocalStart = new DateTime(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Year, StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).AddMonths(-1).Month, (int)RevenueGrowthByMonth.Day).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddDays(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueGrowthByMonth.Revenue = Transactions.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Select(x => x.Revenue)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return RevenueGrowthDTO;
            }
            else if (filter.Time.Equal.Value == THIS_QUARTER)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Month / 3m));
                PermissionMobile_RevenueGrowthDTO RevenueGrowthDTO = new PermissionMobile_RevenueGrowthDTO();
                RevenueGrowthDTO.IndirectRevenueGrowthByQuaters = new List<PermissionMobile_RevenueGrowthByQuarterDTO>();
                int start = 3 * (this_quarter - 1) + 1;
                int end = start + 3;
                for (int i = StartMonth; i <= EndMonth; i++)
                {
                    PermissionMobile_RevenueGrowthByQuarterDTO RevenueGrowthByQuarter = new PermissionMobile_RevenueGrowthByQuarterDTO
                    {
                        Month = i,
                        Revenue = 0
                    };
                    RevenueGrowthDTO.IndirectRevenueGrowthByQuaters.Add(RevenueGrowthByQuarter);
                }

                foreach (var RevenueGrowthByQuarter in RevenueGrowthDTO.IndirectRevenueGrowthByQuaters)
                {
                    DateTime LocalStart = new DateTime(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Year, (int)RevenueGrowthByQuarter.Month, 1).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueGrowthByQuarter.Revenue = Transactions.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Select(x => x.Revenue)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return RevenueGrowthDTO;
            }
            else if (filter.Time.Equal.Value == LAST_QUATER)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Month / 3m));
                var last_quarter = (this_quarter + 3) % 4;
                PermissionMobile_RevenueGrowthDTO RevenueGrowthDTO = new PermissionMobile_RevenueGrowthDTO();
                RevenueGrowthDTO.IndirectRevenueGrowthByQuaters = new List<PermissionMobile_RevenueGrowthByQuarterDTO>();
                int start = 3 * (last_quarter - 1) + 1;
                int end = start + 3;
                for (int i = StartMonth; i <= EndMonth; i++)
                {
                    PermissionMobile_RevenueGrowthByQuarterDTO RevenueGrowthByQuarter = new PermissionMobile_RevenueGrowthByQuarterDTO
                    {
                        Month = i,
                        Revenue = 0
                    };
                    RevenueGrowthDTO.IndirectRevenueGrowthByQuaters.Add(RevenueGrowthByQuarter);
                }

                foreach (var RevenueGrowthByQuarter in RevenueGrowthDTO.IndirectRevenueGrowthByQuaters)
                {
                    DateTime LocalStart = new DateTime(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Year, (int)RevenueGrowthByQuarter.Month, 1).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueGrowthByQuarter.Revenue = Transactions.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Select(x => x.Revenue)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return RevenueGrowthDTO;
            }
            else if (filter.Time.Equal.Value == YEAR)
            {
                PermissionMobile_RevenueGrowthDTO RevenueGrowthDTO = new PermissionMobile_RevenueGrowthDTO();
                RevenueGrowthDTO.IndirectRevenueGrowthByYears = new List<PermissionMobile_RevenueGrowthByYearDTO>();
                for (int i = StartMonth; i <= EndMonth; i++)
                {
                    PermissionMobile_RevenueGrowthByYearDTO RevenueGrowthByYear = new PermissionMobile_RevenueGrowthByYearDTO
                    {
                        Month = i,
                        Revenue = 0
                    };
                    RevenueGrowthDTO.IndirectRevenueGrowthByYears.Add(RevenueGrowthByYear);
                }

                foreach (var RevenueGrowthByYear in RevenueGrowthDTO.IndirectRevenueGrowthByYears)
                {
                    DateTime LocalStart = new DateTime(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Year, (int)RevenueGrowthByYear.Month, 1).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueGrowthByYear.Revenue = Transactions.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Select(x => x.Revenue)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return RevenueGrowthDTO;
            }
            return new PermissionMobile_RevenueGrowthDTO();
        } // tăng trưởng doanh thu gián tiếp

        [Route(PermissionMobileRoute.IndirectQuantityGrowth), HttpPost]
        public async Task<PermissionMobile_QuantityGrowthDTO> IndirectSalesOrderGrowth([FromBody] PermissionMobile_FilterDTO filter)
        {
            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            var IndirectSalesOrder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });

            int StartDay = Start.AddHours(CurrentContext.TimeZone).Day;
            int EndDay = End.AddHours(CurrentContext.TimeZone).Day;
            int StartMonth = Start.AddHours(CurrentContext.TimeZone).Month;
            int EndMonth = End.AddHours(CurrentContext.TimeZone).Month;

            if (filter.Time.Equal.HasValue == false
                || filter.Time.Equal.Value == THIS_MONTH)
            {
                //var query = from i in FilterIndirectSalesOrder(AppUserIds, Start, End)
                //            group i by i.OrderDate.Day into x
                //            select new PermissionMobile_QuantityGrowthByMonthDTO
                //            {
                //                Day = x.Key,
                //                IndirectSalesOrderCounter = x.Count()
                //            };

                var OrderGrowthByMonthDTOs = await IndirectSalesOrder_query.GroupBy(x => x.OrderDate.Day).Select(x => new
                {
                    Day = x.Key,
                    IndirectSalesOrderCounter = x.Count()
                }).ToListWithNoLockAsync();

                PermissionMobile_QuantityGrowthDTO QuantityGrowthDTO = new PermissionMobile_QuantityGrowthDTO();
                QuantityGrowthDTO.IndirectSalesOrderQuantityGrowthByMonths = new List<PermissionMobile_QuantityGrowthByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = StartDay; i <= EndDay; i++)
                {
                    PermissionMobile_QuantityGrowthByMonthDTO IndirectSalesOrderQuantityGrowthByMonth = new PermissionMobile_QuantityGrowthByMonthDTO
                    {
                        Day = i,
                        IndirectSalesOrderCounter = 0
                    };
                    QuantityGrowthDTO.IndirectSalesOrderQuantityGrowthByMonths.Add(IndirectSalesOrderQuantityGrowthByMonth);
                }

                foreach (var IndirectSalesOrderGrowthByMonth in QuantityGrowthDTO.IndirectSalesOrderQuantityGrowthByMonths)
                {
                    var data = OrderGrowthByMonthDTOs.Where(x => x.Day == IndirectSalesOrderGrowthByMonth.Day).FirstOrDefault();
                    if (data != null)
                        IndirectSalesOrderGrowthByMonth.IndirectSalesOrderCounter = data.IndirectSalesOrderCounter;
                }

                return QuantityGrowthDTO;
            }
            else if (filter.Time.Equal.Value == LAST_MONTH)
            {
                var OrderGrowthByMonthDTOs = await IndirectSalesOrder_query.GroupBy(x => x.OrderDate.Day).Select(x => new
                {
                    Day = x.Key,
                    IndirectSalesOrderCounter = x.Count()
                }).ToListWithNoLockAsync();

                PermissionMobile_QuantityGrowthDTO QuantityGrowthDTO = new PermissionMobile_QuantityGrowthDTO();
                QuantityGrowthDTO.IndirectSalesOrderQuantityGrowthByMonths = new List<PermissionMobile_QuantityGrowthByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = StartDay; i <= EndDay; i++)
                {
                    PermissionMobile_QuantityGrowthByMonthDTO IndirectSalesOrderQuantityGrowthByMonth = new PermissionMobile_QuantityGrowthByMonthDTO
                    {
                        Day = i,
                        IndirectSalesOrderCounter = 0
                    };
                    QuantityGrowthDTO.IndirectSalesOrderQuantityGrowthByMonths.Add(IndirectSalesOrderQuantityGrowthByMonth);
                }

                foreach (var IndirectSalesOrderGrowthByMonth in QuantityGrowthDTO.IndirectSalesOrderQuantityGrowthByMonths)
                {
                    var data = OrderGrowthByMonthDTOs.Where(x => x.Day == IndirectSalesOrderGrowthByMonth.Day).FirstOrDefault();
                    if (data != null)
                        IndirectSalesOrderGrowthByMonth.IndirectSalesOrderCounter = data.IndirectSalesOrderCounter;
                }

                return QuantityGrowthDTO;
            }
            else if (filter.Time.Equal.Value == THIS_QUARTER)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Month / 3m));

                var OrderGrowthByMonthDTOs = await IndirectSalesOrder_query.GroupBy(x => x.OrderDate.Month).Select(x => new
                {
                    Month = x.Key,
                    IndirectSalesOrderCounter = x.Count()
                }).ToListWithNoLockAsync();

                PermissionMobile_QuantityGrowthDTO QuantityGrowthDTO = new PermissionMobile_QuantityGrowthDTO();
                QuantityGrowthDTO.IndirectSalesOrderQuantityGrowthByQuaters = new List<PermissionMobile_QuantityGrowthByQuarterDTO>();
                int start = 3 * (this_quarter - 1) + 1;
                int end = start + 3;
                for (int i = StartMonth; i <= EndMonth; i++)
                {
                    PermissionMobile_QuantityGrowthByQuarterDTO IndirectSalesOrderQuantityGrowthByQuarter = new PermissionMobile_QuantityGrowthByQuarterDTO
                    {
                        Month = i,
                        IndirectSalesOrderCounter = 0
                    };
                    QuantityGrowthDTO.IndirectSalesOrderQuantityGrowthByQuaters.Add(IndirectSalesOrderQuantityGrowthByQuarter);
                }

                foreach (var IndirectSalesOrderGrowthByQuater in QuantityGrowthDTO.IndirectSalesOrderQuantityGrowthByQuaters)
                {
                    var data = OrderGrowthByMonthDTOs.Where(x => x.Month == IndirectSalesOrderGrowthByQuater.Month).FirstOrDefault();
                    if (data != null)
                        IndirectSalesOrderGrowthByQuater.IndirectSalesOrderCounter = data.IndirectSalesOrderCounter;
                }

                return QuantityGrowthDTO;
            }
            else if (filter.Time.Equal.Value == LAST_QUATER)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Month / 3m));
                var last_quarter = (this_quarter + 3) % 4;

                var OrderGrowthByMonthDTOs = await IndirectSalesOrder_query.GroupBy(x => x.OrderDate.Month).Select(x => new
                {
                    Month = x.Key,
                    IndirectSalesOrderCounter = x.Count()
                }).ToListWithNoLockAsync();

                PermissionMobile_QuantityGrowthDTO QuantityGrowthDTO = new PermissionMobile_QuantityGrowthDTO();
                QuantityGrowthDTO.IndirectSalesOrderQuantityGrowthByQuaters = new List<PermissionMobile_QuantityGrowthByQuarterDTO>();
                int start = 3 * (last_quarter - 1) + 1;
                int end = start + 3;
                for (int i = StartMonth; i <= EndMonth; i++)
                {
                    PermissionMobile_QuantityGrowthByQuarterDTO IndirectSalesOrderGrowthByQuarter = new PermissionMobile_QuantityGrowthByQuarterDTO
                    {
                        Month = i,
                        IndirectSalesOrderCounter = 0
                    };
                    QuantityGrowthDTO.IndirectSalesOrderQuantityGrowthByQuaters.Add(IndirectSalesOrderGrowthByQuarter);
                }

                foreach (var IndirectSalesOrderGrowthByQuater in QuantityGrowthDTO.IndirectSalesOrderQuantityGrowthByQuaters)
                {
                    var data = OrderGrowthByMonthDTOs.Where(x => x.Month == IndirectSalesOrderGrowthByQuater.Month).FirstOrDefault();
                    if (data != null)
                        IndirectSalesOrderGrowthByQuater.IndirectSalesOrderCounter = data.IndirectSalesOrderCounter;
                }

                return QuantityGrowthDTO;
            }
            else if (filter.Time.Equal.Value == YEAR)
            {
                var OrderGrowthByMonthDTOs = await IndirectSalesOrder_query.GroupBy(x => x.OrderDate.Month).Select(x => new
                {
                    Month = x.Key,
                    IndirectSalesOrderCounter = x.Count()
                }).ToListWithNoLockAsync();

                PermissionMobile_QuantityGrowthDTO QuantityGrowthDTO = new PermissionMobile_QuantityGrowthDTO();
                QuantityGrowthDTO.IndirectSalesOrderQuantityGrowthByYears = new List<PermissionMobile_QuantityGrowthByYearDTO>();
                for (int i = StartMonth; i <= EndMonth; i++)
                {
                    PermissionMobile_QuantityGrowthByYearDTO IndirectSalesOrderGrowthByYear = new PermissionMobile_QuantityGrowthByYearDTO
                    {
                        Month = i,
                        IndirectSalesOrderCounter = 0
                    };
                    QuantityGrowthDTO.IndirectSalesOrderQuantityGrowthByYears.Add(IndirectSalesOrderGrowthByYear);
                }

                foreach (var IndirectSalesOrderGrowthByYear in QuantityGrowthDTO.IndirectSalesOrderQuantityGrowthByYears)
                {
                    var data = OrderGrowthByMonthDTOs.Where(x => x.Month == IndirectSalesOrderGrowthByYear.Month).FirstOrDefault();
                    if (data != null)
                        IndirectSalesOrderGrowthByYear.IndirectSalesOrderCounter = data.IndirectSalesOrderCounter;
                }

                return QuantityGrowthDTO;
            }
            return new PermissionMobile_QuantityGrowthDTO();
        } //  tăng trưởng số lượng đơn gián tiếp
        #endregion

        #region Dashboard Order: DirectSalesOrder
        [Route(PermissionMobileRoute.DirectSalesOrderTotalQuantity), HttpPost]
        public async Task<decimal> DirectSalesOrderTotalQuantity([FromBody] PermissionMobile_FilterDTO filter)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            var DirectSalesOrder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long> {
                GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });
            List<long> DirectSalesOrderIds = await DirectSalesOrder_query.Select(x => x.DirectSalesOrderId).ToListWithNoLockAsync();
            var DirectSalesOrderTransaction_query = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
            DirectSalesOrderTransaction_query = DirectSalesOrderTransaction_query.Where(x => x.DeletedAt == null);
            DirectSalesOrderTransaction_query = DirectSalesOrderTransaction_query.Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds });
            DirectSalesOrderTransaction_query = DirectSalesOrderTransaction_query.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id });

            return await DirectSalesOrderTransaction_query.SumAsync(x => x.Quantity);
        } // Tổng sản lượng bán ĐTT

        [Route(PermissionMobileRoute.DirectSalesOrderItemAmount), HttpPost]
        public async Task<long> DirectSalesOrderItemAmount([FromBody] PermissionMobile_FilterDTO filter)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            var DirectSalesOrder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long> {
                GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });
            List<long> DirectSalesOrderIds = await DirectSalesOrder_query.Select(x => x.DirectSalesOrderId).ToListWithNoLockAsync();
            var DirectSalesOrderTransaction_query = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
            DirectSalesOrderTransaction_query = DirectSalesOrderTransaction_query.Where(x => x.DeletedAt == null);
            DirectSalesOrderTransaction_query = DirectSalesOrderTransaction_query.Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds });
            DirectSalesOrderTransaction_query = DirectSalesOrderTransaction_query.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id });

            return await DirectSalesOrderTransaction_query.Select(x => x.ItemId).Distinct().CountWithNoLockAsync();
        } // Tổng sản phẩm bán ĐTT

        [Route(PermissionMobileRoute.CountDirectSalesOrder), HttpPost]
        public async Task<long> DirectSalesOrder([FromBody] PermissionMobile_FilterDTO filter)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            //var query = from di in DataContext.DirectSalesOrder
            //            where
            //            (di.GeneralApprovalStateId == GeneralApprovalStateEnum.APPROVED.Id || di.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_APPROVED.Id)
            //            && AppUserIds.Contains(di.SaleEmployeeId)
            //            && di.OrderDate >= Start
            //            && di.OrderDate <= End
            //            select di;

            var DirectSalesOrder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long> {
                GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });

            return await DirectSalesOrder_query.CountWithNoLockAsync();
        }

        [Route(PermissionMobileRoute.DirectSalesOrderRevenue), HttpPost]
        public async Task<decimal> DirectSalesOrderRevenue([FromBody] PermissionMobile_FilterDTO filter)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            //var query = from di in DataContext.DirectSalesOrder
            //            where
            //            (di.GeneralApprovalStateId == GeneralApprovalStateEnum.APPROVED.Id || di.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_APPROVED.Id)
            //            && AppUserIds.Contains(di.SaleEmployeeId)
            //            && di.OrderDate >= Start
            //            && di.OrderDate <= End
            //            select di;

            var DirectSalesOrder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long> {
                GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });

            var results = await DirectSalesOrder_query.ToListWithNoLockAsync();
            return results.Select(x => x.Total)
                .DefaultIfEmpty(0)
                .Sum();
        }

        [Route(PermissionMobileRoute.TopDirectSaleEmployeeRevenue), HttpPost]
        public async Task<List<PermissionMobile_TopRevenueBySalesEmployeeDTO>> TopDirectSaleEmployeeRevenue([FromBody] PermissionMobile_FilterDTO filter)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            //var query = from transaction in DataContext.DirectSalesOrderTransaction
            //            join di in DataContext.DirectSalesOrder on transaction.DirectSalesOrderId equals di.Id
            //            where
            //            (di.GeneralApprovalStateId == GeneralApprovalStateEnum.APPROVED.Id || di.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_APPROVED.Id)
            //            && AppUserIds.Contains(di.SaleEmployeeId)
            //            && transaction.OrderDate >= Start
            //            && transaction.OrderDate <= End
            //            group transaction by transaction.SalesEmployeeId into transGroup
            //            select transGroup; // query tu transaction don hang gian tiep co trang thai phe duyet hoan thanh

            //var transactionGroups = await query
            //    .Select(x => new DirectSalesOrderTransactionDAO
            //    {
            //        SalesEmployeeId = x.Key,
            //        Revenue = x.Sum(x => x.Revenue)
            //    })
            //    .ToListWithNoLockAsync();
            var DirectSalesOrder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long> {
                GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });

            var transactionGroups = await DirectSalesOrder_query.GroupBy(x => x.SaleEmployeeId).Select(x => new
            {
                SalesEmployeeId = x.Key,
                Revenue = x.Sum(x => x.Total)
            }).ToListWithNoLockAsync();

            List<PermissionMobile_TopRevenueBySalesEmployeeDTO> Result = new List<PermissionMobile_TopRevenueBySalesEmployeeDTO>();
            List<long> UserIds = transactionGroups
                .Select(x => x.SalesEmployeeId)
                .ToList();
            List<AppUserDAO> AppUserDAOs = await DataContext.AppUser
                .Where(x => UserIds.Contains(x.Id))
                .ToListWithNoLockAsync();

            foreach (var groupItem in transactionGroups)
            {
                long SaleEmployeeId = groupItem.SalesEmployeeId;
                AppUserDAO SaleEmpolyee = AppUserDAOs
                    .Where(x => x.Id == SaleEmployeeId)
                    .FirstOrDefault();
                PermissionMobile_TopRevenueBySalesEmployeeDTO Item = new PermissionMobile_TopRevenueBySalesEmployeeDTO();
                Item.SaleEmployeeId = SaleEmployeeId;
                Item.SaleEmployeeName = SaleEmpolyee.DisplayName;
                Item.Revenue = groupItem.Revenue;
                Result.Add(Item);
            }
            Result = Result
                .Where(x => x.Revenue > 0)
                .OrderByDescending(x => x.Revenue)
                .Take(5)
                .ToList();

            return Result;
        } // top 5 doanh thu đơn trực tiếp theo nhân viên

        [Route(PermissionMobileRoute.TopDirecProductRevenue), HttpPost]
        public async Task<List<PermissionMobile_TopRevenueByItemDTO>> TopDirecProductRevenue([FromBody] PermissionMobile_FilterDTO filter)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            //var query = from transaction in DataContext.DirectSalesOrderTransaction
            //            join di in DataContext.DirectSalesOrder on transaction.DirectSalesOrderId equals di.Id
            //            where
            //            (di.GeneralApprovalStateId == GeneralApprovalStateEnum.APPROVED.Id || di.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_APPROVED.Id)
            //            && AppUserIds.Contains(di.SaleEmployeeId)
            //            && transaction.OrderDate >= Start
            //            && transaction.OrderDate <= End
            //            group transaction by transaction.ItemId into transGroup
            //            select transGroup; // query tu transaction don hang gian tiep co trang thai phe duyet hoan thanh

            //var transactionGroups = await query
            //    .Select(x => new DirectSalesOrderTransactionDAO
            //    {
            //        ItemId = x.Key,
            //        Revenue = x.Sum(x => x.Revenue)
            //    })
            //    .ToListWithNoLockAsync();

            var DirectSalesOrder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long> {
                GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });
            List<long> DirectSalesOrderIds = await DirectSalesOrder_query.Select(x => x.DirectSalesOrderId).Distinct().ToListWithNoLockAsync();

            var DirectSalesOrderTransaction_query = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
            DirectSalesOrderTransaction_query = DirectSalesOrderTransaction_query.Where(x => x.DeletedAt == null);
            DirectSalesOrderTransaction_query = DirectSalesOrderTransaction_query.Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds });

            var transactionGroups = await DirectSalesOrderTransaction_query.GroupBy(x => x.ItemId).Select(x => new
            {
                ItemId = x.Key,
                Revenue = x.Sum(x => x.Amount + x.TaxAmount.GetValueOrDefault(0))
            }).ToListWithNoLockAsync();

            List<PermissionMobile_TopRevenueByItemDTO> Result = new List<PermissionMobile_TopRevenueByItemDTO>();
            List<long> ItemIds = transactionGroups.Select(x => x.ItemId).ToList();
            List<Item> Items = await ItemService.MobileList(new ItemFilter
            {
                Id = new IdFilter { In = ItemIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = ItemSelect.Id | ItemSelect.Product
            });

            List<long> ProductIds = Items.Select(x => x.Product.Id).Distinct().ToList();
            List<Product> Products = await ProductService.MobileList(new ProductFilter
            {
                Id = new IdFilter { In = ProductIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProductSelect.Id | ProductSelect.Name
            });

            foreach (var groupItem in transactionGroups)
            {
                long ItemId = groupItem.ItemId;
                Item Item = Items
                    .Where(x => x.Id == ItemId)
                    .FirstOrDefault();
                if (Item != null)
                {
                    PermissionMobile_TopRevenueByItemDTO ResultItem = new PermissionMobile_TopRevenueByItemDTO();
                    ResultItem.ProductId = Item.Product.Id;
                    ResultItem.Revenue = groupItem.Revenue;
                    Result.Add(ResultItem);
                }
            }
            Result = Result
                .GroupBy(x => x.ProductId)
                .Select(y => new PermissionMobile_TopRevenueByItemDTO
                {
                    ProductId = y.Key,
                    Revenue = y.Sum(r => r.Revenue)
                })
                .ToList(); // group doanh thu theo id san pham
            foreach (var ResultItem in Result)
            {
                Product Product = Products.Where(x => x.Id == ResultItem.ProductId).FirstOrDefault();
                ResultItem.ProductName = Product.Name;
            }

            Result = Result
                .Where(x => x.Revenue > 0)
                .OrderByDescending(x => x.Revenue)
                .Take(5)
                .ToList();

            return Result;
        } // top 5 doanh thu đơn gián tiếp theo item

        [Route(PermissionMobileRoute.DirectRevenueGrowth), HttpPost]
        public async Task<PermissionMobile_RevenueGrowthDTO> DirectRevenueGrowth([FromBody] PermissionMobile_FilterDTO filter)
        {
            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;
            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            //var query = from t in DataContext.DirectSalesOrderTransaction
            //            join i in DataContext.DirectSalesOrder on t.DirectSalesOrderId equals i.Id
            //            where
            //            (i.GeneralApprovalStateId == GeneralApprovalStateEnum.APPROVED.Id || i.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_APPROVED.Id)
            //            && AppUserIds.Contains(i.SaleEmployeeId)
            //            && t.OrderDate >= Start
            //            && t.OrderDate <= End
            //            select new DirectSalesOrderTransactionDAO
            //            {
            //                OrderDate = t.OrderDate,
            //                Revenue = t.Revenue
            //            };

            var DirectSalesOrder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long> {
                GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });

            var DirectSalesOrderTransactionDAOs = await DirectSalesOrder_query.Select(x => new
            {
                OrderDate = x.OrderDate,
                Revenue = x.Total
            }).ToListWithNoLockAsync();

            int StartDay = Start.AddHours(CurrentContext.TimeZone).Day;
            int EndDay = End.AddHours(CurrentContext.TimeZone).Day;
            int StartMonth = Start.AddHours(CurrentContext.TimeZone).Month;
            int EndMonth = End.AddHours(CurrentContext.TimeZone).Month;

            if (filter.Time.Equal.HasValue == false
                || filter.Time.Equal.Value == THIS_MONTH)
            {
                PermissionMobile_RevenueGrowthDTO PermissionMobile_RevenueGrowthDTO = new PermissionMobile_RevenueGrowthDTO();
                PermissionMobile_RevenueGrowthDTO.DirectRevenueGrowthByMonths = new List<PermissionMobile_RevenueGrowthByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = StartDay; i <= EndDay; i++)
                {
                    PermissionMobile_RevenueGrowthByMonthDTO RevenueGrowthByMonth = new PermissionMobile_RevenueGrowthByMonthDTO
                    {
                        Day = i,
                        Revenue = 0
                    };
                    PermissionMobile_RevenueGrowthDTO.DirectRevenueGrowthByMonths.Add(RevenueGrowthByMonth);
                }

                foreach (var RevenueGrowthByMonth in PermissionMobile_RevenueGrowthDTO.DirectRevenueGrowthByMonths)
                {
                    DateTime LocalStart = new DateTime(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Year, StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Month, (int)RevenueGrowthByMonth.Day).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddDays(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueGrowthByMonth.Revenue = DirectSalesOrderTransactionDAOs.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Select(x => x.Revenue)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return PermissionMobile_RevenueGrowthDTO;
            }
            else if (filter.Time.Equal.Value == LAST_MONTH)
            {
                PermissionMobile_RevenueGrowthDTO PermissionMobile_RevenueGrowthDTO = new PermissionMobile_RevenueGrowthDTO();
                PermissionMobile_RevenueGrowthDTO.DirectRevenueGrowthByMonths = new List<PermissionMobile_RevenueGrowthByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = StartDay; i <= EndDay; i++)
                {
                    PermissionMobile_RevenueGrowthByMonthDTO RevenueGrowthByMonth = new PermissionMobile_RevenueGrowthByMonthDTO
                    {
                        Day = i,
                        Revenue = 0
                    };
                    PermissionMobile_RevenueGrowthDTO.DirectRevenueGrowthByMonths.Add(RevenueGrowthByMonth);
                }

                foreach (var RevenueGrowthByMonth in PermissionMobile_RevenueGrowthDTO.DirectRevenueGrowthByMonths)
                {
                    DateTime LocalStart = new DateTime(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Year, StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).AddMonths(-1).Month, (int)RevenueGrowthByMonth.Day).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddDays(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueGrowthByMonth.Revenue = DirectSalesOrderTransactionDAOs.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Select(x => x.Revenue)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return PermissionMobile_RevenueGrowthDTO;
            }
            else if (filter.Time.Equal.Value == THIS_QUARTER)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Month / 3m));
                PermissionMobile_RevenueGrowthDTO PermissionMobile_RevenueGrowthDTO = new PermissionMobile_RevenueGrowthDTO();
                PermissionMobile_RevenueGrowthDTO.DirectRevenueGrowthByQuaters = new List<PermissionMobile_RevenueGrowthByQuarterDTO>();
                int start = 3 * (this_quarter - 1) + 1;
                int end = start + 3;
                for (int i = StartMonth; i <= EndMonth; i++)
                {
                    PermissionMobile_RevenueGrowthByQuarterDTO RevenueGrowthByQuarter = new PermissionMobile_RevenueGrowthByQuarterDTO
                    {
                        Month = i,
                        Revenue = 0
                    };
                    PermissionMobile_RevenueGrowthDTO.DirectRevenueGrowthByQuaters.Add(RevenueGrowthByQuarter);
                }

                foreach (var RevenueGrowthByQuarter in PermissionMobile_RevenueGrowthDTO.DirectRevenueGrowthByQuaters)
                {
                    DateTime LocalStart = new DateTime(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Year, (int)RevenueGrowthByQuarter.Month, 1).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueGrowthByQuarter.Revenue = DirectSalesOrderTransactionDAOs.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Select(x => x.Revenue)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return PermissionMobile_RevenueGrowthDTO;
            }
            else if (filter.Time.Equal.Value == LAST_QUATER)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Month / 3m));
                var last_quarter = (this_quarter + 3) % 4;
                PermissionMobile_RevenueGrowthDTO PermissionMobile_RevenueGrowthDTO = new PermissionMobile_RevenueGrowthDTO();
                PermissionMobile_RevenueGrowthDTO.DirectRevenueGrowthByQuaters = new List<PermissionMobile_RevenueGrowthByQuarterDTO>();
                int start = 3 * (last_quarter - 1) + 1;
                int end = start + 3;
                for (int i = StartMonth; i <= EndMonth; i++)
                {
                    PermissionMobile_RevenueGrowthByQuarterDTO RevenueGrowthByQuarter = new PermissionMobile_RevenueGrowthByQuarterDTO
                    {
                        Month = i,
                        Revenue = 0
                    };
                    PermissionMobile_RevenueGrowthDTO.DirectRevenueGrowthByQuaters.Add(RevenueGrowthByQuarter);
                }

                foreach (var RevenueGrowthByQuarter in PermissionMobile_RevenueGrowthDTO.DirectRevenueGrowthByQuaters)
                {
                    DateTime LocalStart = new DateTime(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Year, (int)RevenueGrowthByQuarter.Month, 1).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueGrowthByQuarter.Revenue = DirectSalesOrderTransactionDAOs.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Select(x => x.Revenue)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return PermissionMobile_RevenueGrowthDTO;
            }
            else if (filter.Time.Equal.Value == YEAR)
            {
                PermissionMobile_RevenueGrowthDTO PermissionMobile_RevenueGrowthDTO = new PermissionMobile_RevenueGrowthDTO();
                PermissionMobile_RevenueGrowthDTO.DirectRevenueGrowthByYears = new List<PermissionMobile_RevenueGrowthByYearDTO>();
                for (int i = StartMonth; i <= EndMonth; i++)
                {
                    PermissionMobile_RevenueGrowthByYearDTO RevenueGrowthByYear = new PermissionMobile_RevenueGrowthByYearDTO
                    {
                        Month = i,
                        Revenue = 0
                    };
                    PermissionMobile_RevenueGrowthDTO.DirectRevenueGrowthByYears.Add(RevenueGrowthByYear);
                }

                foreach (var RevenueGrowthByYear in PermissionMobile_RevenueGrowthDTO.DirectRevenueGrowthByYears)
                {
                    DateTime LocalStart = new DateTime(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Year, (int)RevenueGrowthByYear.Month, 1).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueGrowthByYear.Revenue = DirectSalesOrderTransactionDAOs.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Select(x => x.Revenue)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return PermissionMobile_RevenueGrowthDTO;
            }
            return new PermissionMobile_RevenueGrowthDTO();
        } // tăng trưởng doanh thu truc tiep

        [Route(PermissionMobileRoute.DirectQuantityGrowth), HttpPost]
        public async Task<PermissionMobile_QuantityGrowthDTO> DirectQuantityGrowth([FromBody] PermissionMobile_FilterDTO filter)
        {
            DateTime Start; DateTime End;
            (Start, End) = ConvertTime(filter); // lấy ra startDate và EndDate theo filter time
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> AppUserIds; List<long> StoreIds;

            (AppUserIds, StoreIds) = await DynamicFilter(filter);

            var DirectSalesOrder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long> {
                GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });

            int StartDay = Start.AddHours(CurrentContext.TimeZone).Day;
            int EndDay = End.AddHours(CurrentContext.TimeZone).Day;
            int StartMonth = Start.AddHours(CurrentContext.TimeZone).Month;
            int EndMonth = End.AddHours(CurrentContext.TimeZone).Month;

            if (filter.Time.Equal.HasValue == false
                || filter.Time.Equal.Value == THIS_MONTH)
            {
                var PermissionMobile_DirectSalesOrderGrowthByMonthDTOs = await DirectSalesOrder_query.GroupBy(x => x.OrderDate.Day).Select(x => new
                {
                    Day = x.Key,
                    DirectSalesOrderCounter = x.Count()
                }).ToListWithNoLockAsync();

                PermissionMobile_QuantityGrowthDTO PermissionMobile_QuantityGrowthDTO = new PermissionMobile_QuantityGrowthDTO();
                PermissionMobile_QuantityGrowthDTO.DirectSalesOrderQuantityGrowthByMonths = new List<PermissionMobile_QuantityGrowthByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = StartDay; i <= EndDay; i++)
                {
                    PermissionMobile_QuantityGrowthByMonthDTO DirectSalesOrderQuantityGrowthByMonth = new PermissionMobile_QuantityGrowthByMonthDTO
                    {
                        Day = i,
                        DirectSalesOrderCounter = 0
                    };
                    PermissionMobile_QuantityGrowthDTO.DirectSalesOrderQuantityGrowthByMonths.Add(DirectSalesOrderQuantityGrowthByMonth);
                }

                foreach (var DirectSalesOrderGrowthByMonth in PermissionMobile_QuantityGrowthDTO.DirectSalesOrderQuantityGrowthByMonths)
                {
                    var data = PermissionMobile_DirectSalesOrderGrowthByMonthDTOs.Where(x => x.Day == DirectSalesOrderGrowthByMonth.Day).FirstOrDefault();
                    if (data != null)
                        DirectSalesOrderGrowthByMonth.DirectSalesOrderCounter = data.DirectSalesOrderCounter;
                }

                return PermissionMobile_QuantityGrowthDTO;
            }
            else if (filter.Time.Equal.Value == LAST_MONTH)
            {
                var PermissionMobile_DirectSalesOrderGrowthByMonthDTOs = await DirectSalesOrder_query.GroupBy(x => x.OrderDate.Day).Select(x => new
                {
                    Day = x.Key,
                    DirectSalesOrderCounter = x.Count()
                }).ToListWithNoLockAsync();

                PermissionMobile_QuantityGrowthDTO PermissionMobile_QuantityGrowthDTO = new PermissionMobile_QuantityGrowthDTO();
                PermissionMobile_QuantityGrowthDTO.DirectSalesOrderQuantityGrowthByMonths = new List<PermissionMobile_QuantityGrowthByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = StartDay; i <= EndDay; i++)
                {
                    PermissionMobile_QuantityGrowthByMonthDTO DirectSalesOrderQuantityGrowthByMonth = new PermissionMobile_QuantityGrowthByMonthDTO
                    {
                        Day = i,
                        DirectSalesOrderCounter = 0
                    };
                    PermissionMobile_QuantityGrowthDTO.DirectSalesOrderQuantityGrowthByMonths.Add(DirectSalesOrderQuantityGrowthByMonth);
                }

                foreach (var DirectSalesOrderGrowthByMonth in PermissionMobile_QuantityGrowthDTO.DirectSalesOrderQuantityGrowthByMonths)
                {
                    var data = PermissionMobile_DirectSalesOrderGrowthByMonthDTOs.Where(x => x.Day == DirectSalesOrderGrowthByMonth.Day).FirstOrDefault();
                    if (data != null)
                        DirectSalesOrderGrowthByMonth.DirectSalesOrderCounter = data.DirectSalesOrderCounter;
                }

                return PermissionMobile_QuantityGrowthDTO;
            }
            else if (filter.Time.Equal.Value == THIS_QUARTER)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Month / 3m));

                var PermissionMobile_DirectSalesOrderGrowthByQuarterDTOs = await DirectSalesOrder_query.GroupBy(x => x.OrderDate.Month).Select(x => new
                {
                    Month = x.Key,
                    DirectSalesOrderCounter = x.Count()
                }).ToListWithNoLockAsync();
                PermissionMobile_QuantityGrowthDTO PermissionMobile_QuantityGrowthDTO = new PermissionMobile_QuantityGrowthDTO();
                PermissionMobile_QuantityGrowthDTO.DirectSalesOrderQuantityGrowthByQuaters = new List<PermissionMobile_QuantityGrowthByQuarterDTO>();
                int start = 3 * (this_quarter - 1) + 1;
                int end = start + 3;
                for (int i = StartMonth; i <= EndMonth; i++)
                {
                    PermissionMobile_QuantityGrowthByQuarterDTO DirectSalesOrderQuantityGrowthByQuarter = new PermissionMobile_QuantityGrowthByQuarterDTO
                    {
                        Month = i,
                        DirectSalesOrderCounter = 0
                    };
                    PermissionMobile_QuantityGrowthDTO.DirectSalesOrderQuantityGrowthByQuaters.Add(DirectSalesOrderQuantityGrowthByQuarter);
                }

                foreach (var DirectSalesOrderGrowthByQuater in PermissionMobile_QuantityGrowthDTO.DirectSalesOrderQuantityGrowthByQuaters)
                {
                    var data = PermissionMobile_DirectSalesOrderGrowthByQuarterDTOs.Where(x => x.Month == DirectSalesOrderGrowthByQuater.Month).FirstOrDefault();
                    if (data != null)
                        DirectSalesOrderGrowthByQuater.DirectSalesOrderCounter = data.DirectSalesOrderCounter;
                }

                return PermissionMobile_QuantityGrowthDTO;
            }
            else if (filter.Time.Equal.Value == LAST_QUATER)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Month / 3m));
                var last_quarter = (this_quarter + 3) % 4;
                var PermissionMobile_DirectSalesOrderGrowthByQuarterDTOs = await DirectSalesOrder_query.GroupBy(x => x.OrderDate.Month).Select(x => new
                {
                    Month = x.Key,
                    DirectSalesOrderCounter = x.Count()
                }).ToListWithNoLockAsync();

                PermissionMobile_QuantityGrowthDTO PermissionMobile_QuantityGrowthDTO = new PermissionMobile_QuantityGrowthDTO();
                PermissionMobile_QuantityGrowthDTO.DirectSalesOrderQuantityGrowthByQuaters = new List<PermissionMobile_QuantityGrowthByQuarterDTO>();
                int start = 3 * (last_quarter - 1) + 1;
                int end = start + 3;
                for (int i = StartMonth; i <= EndMonth; i++)
                {
                    PermissionMobile_QuantityGrowthByQuarterDTO DirectSalesOrderGrowthByQuarter = new PermissionMobile_QuantityGrowthByQuarterDTO
                    {
                        Month = i,
                        DirectSalesOrderCounter = 0
                    };
                    PermissionMobile_QuantityGrowthDTO.DirectSalesOrderQuantityGrowthByQuaters.Add(DirectSalesOrderGrowthByQuarter);
                }

                foreach (var DirectSalesOrderGrowthByQuater in PermissionMobile_QuantityGrowthDTO.DirectSalesOrderQuantityGrowthByQuaters)
                {
                    var data = PermissionMobile_DirectSalesOrderGrowthByQuarterDTOs.Where(x => x.Month == DirectSalesOrderGrowthByQuater.Month).FirstOrDefault();
                    if (data != null)
                        DirectSalesOrderGrowthByQuater.DirectSalesOrderCounter = data.DirectSalesOrderCounter;
                }

                return PermissionMobile_QuantityGrowthDTO;
            }
            else if (filter.Time.Equal.Value == YEAR)
            {
                var PermissionMobile_DirectSalesOrderGrowthByYearDTO = await DirectSalesOrder_query.GroupBy(x => x.OrderDate.Month).Select(x => new
                {
                    Month = x.Key,
                    DirectSalesOrderCounter = x.Count()
                }).ToListWithNoLockAsync();

                PermissionMobile_QuantityGrowthDTO PermissionMobile_QuantityGrowthDTO = new PermissionMobile_QuantityGrowthDTO();
                PermissionMobile_QuantityGrowthDTO.DirectSalesOrderQuantityGrowthByYears = new List<PermissionMobile_QuantityGrowthByYearDTO>();
                for (int i = StartMonth; i <= EndMonth; i++)
                {
                    PermissionMobile_QuantityGrowthByYearDTO DirectSalesOrderGrowthByYear = new PermissionMobile_QuantityGrowthByYearDTO
                    {
                        Month = i,
                        DirectSalesOrderCounter = 0
                    };
                    PermissionMobile_QuantityGrowthDTO.DirectSalesOrderQuantityGrowthByYears.Add(DirectSalesOrderGrowthByYear);
                }

                foreach (var DirectSalesOrderGrowthByYear in PermissionMobile_QuantityGrowthDTO.DirectSalesOrderQuantityGrowthByYears)
                {
                    var data = PermissionMobile_DirectSalesOrderGrowthByYearDTO.Where(x => x.Month == DirectSalesOrderGrowthByYear.Month).FirstOrDefault();
                    if (data != null)
                        DirectSalesOrderGrowthByYear.DirectSalesOrderCounter = data.DirectSalesOrderCounter;
                }

                return PermissionMobile_QuantityGrowthDTO;
            }
            return new PermissionMobile_QuantityGrowthDTO();
        } //  tăng trưởng số lượng đơn truc tiếp
        private Tuple<DateTime, DateTime> ConvertTime(PermissionMobile_FilterDTO filter)
        {
            IdFilter Time = filter.Time;
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            DateTime Now = StaticParams.DateTimeNow;
            if (Time.Equal.HasValue == false)
            {
                Time.Equal = 0;
                Start = LocalStartDay(CurrentContext);
                End = LocalEndDay(CurrentContext);
                if (filter.OrderDate?.GreaterEqual != null)
                    Start = filter.OrderDate.GreaterEqual.Value;
                if (filter.OrderDate?.LessEqual != null)
                    End = filter.OrderDate.LessEqual.Value;
            }
            else if (Time.Equal.Value == TODAY)
            {
                Start = LocalStartDay(CurrentContext);
                End = LocalEndDay(CurrentContext);
            }
            else if (Time.Equal.Value == THIS_WEEK)
            {
                int diff = (7 + (Now.AddHours(CurrentContext.TimeZone).DayOfWeek - DayOfWeek.Monday)) % 7;
                Start = LocalStartDay(CurrentContext).AddDays(-1 * diff);
                End = Start.AddDays(7).AddSeconds(-1);
            }
            else if (Time.Equal.Value == THIS_MONTH)
            {
                Start = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddHours(0 - CurrentContext.TimeZone);
                End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
            }
            else if (Time.Equal.Value == LAST_MONTH)
            {
                Start = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddMonths(-1).AddHours(0 - CurrentContext.TimeZone);
                End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
            }
            else if (Time.Equal.Value == THIS_QUARTER)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.AddHours(CurrentContext.TimeZone).Month / 3m));
                Start = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, (this_quarter - 1) * 3 + 1, 1).AddHours(0 - CurrentContext.TimeZone);
                End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, (this_quarter - 1) * 3 + 1, 1).AddMonths(3).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
            }
            else if (Time.Equal.Value == LAST_QUATER)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.AddHours(CurrentContext.TimeZone).Month / 3m));
                Start = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, (this_quarter - 1) * 3 + 1, 1).AddMonths(-3).AddHours(0 - CurrentContext.TimeZone);
                End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, (this_quarter - 1) * 3 + 1, 1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
            }
            else if (Time.Equal.Value == YEAR)
            {
                Start = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, 1, 1).AddHours(0 - CurrentContext.TimeZone);
                End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, 1, 1).AddYears(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
            }
            if (filter.OrderDate?.GreaterEqual != null && filter.OrderDate.GreaterEqual > Start)
                Start = filter.OrderDate.GreaterEqual.Value;
            if (filter.OrderDate?.LessEqual != null && filter.OrderDate.LessEqual < End)
                End = filter.OrderDate.LessEqual.Value;
            return Tuple.Create(Start, End);
        } // lấy ra thời điểm bắt đầu và kết thúc của ngày, tuần, tháng, quý năm từ một thời điểm cho trước
        #endregion

        private async Task<Tuple<List<long>, List<long>>> DynamicFilter(PermissionMobile_FilterDTO filter)
        {
            List<long> AppUserIds = new List<long>();

            AppUserIds = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            if (filter.EmployeeId.Equal != null)
            {
                AppUserIds = AppUserIds.Intersect(new List<long> { filter.EmployeeId.Equal.Value }).ToList();
            }

            List<long> StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            if (filter.StoreId != null && filter.StoreId.Equal != null)
                StoreIds = StoreIds.Intersect(new List<long> { filter.StoreId.Equal.Value }).ToList();

            if (filter.StoreGroupingId != null && filter.StoreGroupingId.HasValue)
            {
                var StoreStoreGroupingMapping_query = DataContext.StoreStoreGroupingMapping.AsNoTracking();
                StoreStoreGroupingMapping_query = StoreStoreGroupingMapping_query.Where(x => x.StoreId, new IdFilter { In = StoreIds });
                StoreStoreGroupingMapping_query = StoreStoreGroupingMapping_query.Where(x => x.StoreGroupingId, filter.StoreGroupingId);
                StoreIds = await StoreStoreGroupingMapping_query.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();
            }

            var Store_query = DWContext.Dim_Store.AsNoTracking();
            Store_query = Store_query.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            Store_query = Store_query.Where(x => x.StoreTypeId, filter.StoreTypeId);
            Store_query = Store_query.Where(x => x.ProvinceId, filter.ProvinceId);
            StoreIds = await Store_query.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();

            return Tuple.Create(AppUserIds, StoreIds);
        }
    }
}
