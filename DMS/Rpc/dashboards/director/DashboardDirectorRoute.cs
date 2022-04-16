using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.director
{
    [DisplayName("Dashboard quản lý")]
    public class DashboardDirectorRoute : Root
    {
        public const string Parent = Module + "/dashboards";
        public const string Master = Module + "/dashboards/director";
        private const string Default = Rpc + Module + "/dashboards/director";
        public const string CountStore = Default + "/count-store";
        public const string StoreHasCheckedCounter = Default + "/store-has-checked-counter";
        public const string CountStoreChecking = Default + "/store-checking-couter";
        public const string StoreCoverage = Default + "/store-coverage";
        public const string SaleEmployeeLocation = Default + "/sale-employee-location";

        public const string IndirectSalesOrder = Default + "/indirect-sales-order";
        public const string CountIndirectSalesOrder = IndirectSalesOrder + "/sales-order-counter";
        public const string RevenueTotal = IndirectSalesOrder + "/revenue-total";
        public const string StatisticToday = IndirectSalesOrder + "/statistic-today";
        public const string StatisticYesterday = IndirectSalesOrder + "/statistic-yesterday";
        public const string ListIndirectSalesOrder = IndirectSalesOrder + "/list-sales-order";
        public const string Top5RevenueByProduct = IndirectSalesOrder + "/top-5-revenue-by-product";
        public const string Top5RevenueByEmployee = IndirectSalesOrder + "/top-5-revenue-by-employee";
        public const string RevenueFluctuation = IndirectSalesOrder + "/revenue-fluctuation";
        public const string SaledItemFluctuation = IndirectSalesOrder + "/saled-item-fluctuation";
        public const string IndirectSalesOrderFluctuation = IndirectSalesOrder + "/sales-order-fluctuation";

        public const string DirectSalesOrder = Default + "/direct-sales-order";
        public const string CountDirectSalesOrder = DirectSalesOrder + "/sales-order-counter";
        public const string DirectRevenueTotal = DirectSalesOrder + "/revenue-total";
        public const string DirectStatisticToday = DirectSalesOrder + "/statistic-today";
        public const string DirectStatisticYesterday = DirectSalesOrder + "/statistic-yesterday";
        public const string ListDirectSalesOrder = DirectSalesOrder + "/list-sales-order";
        public const string Top5DirectRevenueByProduct = DirectSalesOrder + "/top-5-revenue-by-product";
        public const string Top5DirectRevenueByEmployee = DirectSalesOrder + "/top-5-revenue-by-employee";
        public const string DirectRevenueFluctuation = DirectSalesOrder + "/revenue-fluctuation";
        public const string DirectSaledItemFluctuation = DirectSalesOrder + "/saled-item-fluctuation";
        public const string DirectSalesOrderFluctuation = DirectSalesOrder + "/sales-order-fluctuation";


        public const string FilterListTime = Default + "/filter-list-time";
        public const string FilterListTimeDetail = Default + "/filter-list-time-detail";
        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListProvince = Default + "/filter-list-province";
        public const string FilterListDistrict = Default + "/filter-list-district";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {

            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
            { nameof(DashboardDirector_StoreFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(DashboardDirector_StoreFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Hiển thị", new List<string> {
                Parent,
                Master,
                CountStore, CountIndirectSalesOrder, RevenueTotal, StoreHasCheckedCounter, CountStoreChecking, StatisticToday, StatisticYesterday,
                StoreCoverage, SaleEmployeeLocation, ListIndirectSalesOrder, Top5RevenueByProduct, Top5RevenueByEmployee,
                RevenueFluctuation, SaledItemFluctuation, IndirectSalesOrderFluctuation,
                DirectSalesOrder, CountDirectSalesOrder, DirectRevenueTotal, DirectStatisticToday, DirectStatisticYesterday,
                ListDirectSalesOrder, Top5DirectRevenueByProduct, Top5DirectRevenueByEmployee, DirectRevenueFluctuation,
                DirectSaledItemFluctuation, DirectSalesOrderFluctuation,
                FilterListTime, FilterListAppUser, FilterListOrganization, FilterListProvince, FilterListDistrict, FilterListTimeDetail
            } },
        };
    }
}
