using DMS.Common;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public class PermissionMobileRoute : Root
    {
        private const string Default = Rpc + Module + "/permission-mobile";

        // dashboard kpi
        public const string CountAppUser = Default + "/count-app-user";
        public const string ListAppUser = Default + "/list-app-user";

        public const string ListCurrentKpiGeneral = Default + "/list-current-kpi-general";
        public const string ListCurrentKpiItem = Default + "/list-current-kpi-item";
        public const string ListCurrentKpiNewItem = Default + "/list-current-kpi-new-item";
        public const string ListCurrentKpiProductGrouping = Default + "/list-current-kpi-product-grouping";
        public const string ListCurrentKpiNewProductGrouping = Default + "/list-current-kpi-new-product-grouping";

        public const string SalesQuantity = Default + "/sales-quantity";
        public const string KpiGeneral = Default + "/kpi-general";


        // dashboard order
        public const string CountStoreChecking = Default + "/count-store-checking"; // số lượt viếng thăm 
        public const string CountStore = Default + "/count-store"; // số đại lý viếng thăm

        public const string CountIndirectSalesOrder = Default + "/count-indirect-sales-order"; // đơn gián tiếp
        public const string IndirectSalesOrderRevenue = Default + "/indirect-sales-order-revenue"; // doanh thu đơn gián tiếp
        public const string IndirectSalesOrderTotalQuantity = Default + "/indirect-sales-order-total-quantity"; // số lượng bán đơn gián tiếp
        public const string IndirectSalesOrderItemAmount = Default + "/indirect-sales-order-item-amount"; // số sản phẩm bán đơn gián tiếp

        public const string CountDirectSalesOrder = Default + "/count-direct-sales-order"; // đơn trục tiếp
        public const string DirectSalesOrderRevenue = Default + "/direct-sales-order-revenue"; // doanh thu đơn trực tiếp

        public const string TopIndirectSaleEmployeeRevenue = Default + "/top-indirect-sale-employee-revenue"; // top 5 doanh thu đơn gián tiếp theo nhân viên
        public const string TopIndirecProductRevenue = Default + "/top-indirect-product-revenue"; // top 5 doanh thu đơn gián tiếp theo nhân viên

        public const string TopDirectSaleEmployeeRevenue = Default + "/top-direct-sale-employee-revenue"; // top 5 doanh thu đơn trực tiếp theo nhân viên
        public const string TopDirecProductRevenue = Default + "/top-direct-product-revenue"; // top 5 doanh thu đơn trực tiếp theo nhân viên

        public const string IndirectRevenueGrowth = Default + "/indirect-revenue-growth"; // bieu do tang truong doanh thu gian tiep
        public const string IndirectQuantityGrowth = Default + "/indirect-quantity-growth"; // bieu do tang truong so luong gian tiep

        public const string DirectRevenueGrowth = Default + "/direct-revenue-growth"; // bieu do tang truong doanh thu truc tiep
        public const string DirectQuantityGrowth = Default + "/direct-quantity-growth"; // bieu do tang truong so luong truc tiep
        public const string DirectSalesOrderTotalQuantity = Default + "/direct-total-quantity"; // so luong truc tiep
        public const string DirectSalesOrderItemAmount = Default + "/direct-item-amount"; // so luong san pham truc tiep

        public const string SingleListPeriod = Default + "/single-list-period";

        //FIlter list
        public const string FilterListStore = Default + "/filter-list-store";
        public const string FilterCountStore = Default + "/filter-count-store";
        public const string FilterListStoreType = Default + "/filter-list-store-type";
        public const string FilterCountStoreType = Default + "/filter-count-store-type";
        public const string FilterListStoreGrouping = Default + "/filter-list-store-grouping";
        public const string FilterCountStoreGrouping = Default + "/filter-count-store-grouping";
        public const string FilterListProvince = Default + "/filter-list-province";
        public const string FilterCountProvince = Default + "/filter-count-province";


        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { "OrganizationId", FieldTypeEnum.ID.Id },
            { "AppUserId", FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Thống kê Kpi nhân viên theo tháng", new List<string>{
                ListCurrentKpiGeneral, ListCurrentKpiItem, ListCurrentKpiNewItem, ListCurrentKpiProductGrouping, ListCurrentKpiNewProductGrouping, CountAppUser, ListAppUser,
            } },
            { "Quyền Dashboard nhân viên theo đơn gián tiếp", new List<string> {
                CountStoreChecking, CountStore, CountIndirectSalesOrder, IndirectSalesOrderRevenue,
                IndirectSalesOrderTotalQuantity, IndirectSalesOrderItemAmount,
                SingleListPeriod, FilterListStore, FilterListStoreType, FilterListStoreGrouping, FilterListProvince,
                FilterCountStore, FilterCountStoreType, FilterCountStoreGrouping, FilterCountProvince,
            } },
            { "Quyền Dashboard nhân viên theo đơn trực tiếp", new List<string> {
                CountStoreChecking, CountStore, CountDirectSalesOrder, DirectSalesOrderRevenue, 
                DirectSalesOrderTotalQuantity, DirectSalesOrderItemAmount,
                SingleListPeriod, FilterListStore, FilterListStoreType, FilterListStoreGrouping, FilterListProvince,
                FilterCountStore, FilterCountStoreType, FilterCountStoreGrouping, FilterCountProvince,
            } },
            { "Quyền Dashboard quản lý theo đơn gián tiếp", new List<string> {
                CountStoreChecking, CountStore, CountIndirectSalesOrder, IndirectSalesOrderRevenue, 
                IndirectSalesOrderTotalQuantity, IndirectSalesOrderItemAmount,
                TopIndirectSaleEmployeeRevenue, TopIndirecProductRevenue,
                IndirectRevenueGrowth, IndirectQuantityGrowth,
                SingleListPeriod, FilterListStore, FilterListStoreType, FilterListStoreGrouping, FilterListProvince,
                FilterCountStore, FilterCountStoreType, FilterCountStoreGrouping, FilterCountProvince,
            } },
            { "Quyền Dashboard quản lý theo đơn trực tiếp", new List<string> {
                CountStoreChecking, CountStore, CountDirectSalesOrder, DirectSalesOrderRevenue,
                DirectSalesOrderTotalQuantity, DirectSalesOrderItemAmount,
                TopDirectSaleEmployeeRevenue, TopDirecProductRevenue,
                DirectRevenueGrowth, DirectQuantityGrowth,
                SingleListPeriod, FilterListStore, FilterListStoreType, FilterListStoreGrouping, FilterListProvince,
                FilterCountStore, FilterCountStoreType, FilterCountStoreGrouping, FilterCountProvince,
            } },
        };
    }
}
