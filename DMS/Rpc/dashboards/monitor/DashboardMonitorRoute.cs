using DMS.Common;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.monitor
{
    public class DashboardMonitorRoute : Root
    {
        public const string Parent = Module + "/dashboards";
        public const string Master = Module + "/dashboards/monitor";
        private const string Default = Rpc + Module + "/dashboards/monitor";
        public const string StoreChecking = Default + "/store-checking";
        public const string SaleEmployeeOnline = Default + "/sale-employee-online";
        public const string StatisticIndirectSalesOrder = Default + "/statistic-indirect-sales-order";
        public const string ImageStoreCheking = Default + "/image-store-checking";
        public const string StoreCoverage = Default + "/store-coverage";
        public const string SaleEmployeeLocation = Default + "/sale-employee-location";
        public const string ListIndirectSalesOrder = Default + "/list-indirect-sales-order";
        public const string TopSaleEmployeeStoreChecking = Default + "/top-sale-employee-store-checking";

        public const string FilterListTime = Default + "/filter-list-time";
        public const string FilterListOrganization = Default + "/filter-list-organization";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
            { nameof(DashboardMonitor_StoreFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Hiển thị", new List<string> {
                Parent,
                Master,
                StoreChecking, SaleEmployeeOnline, StatisticIndirectSalesOrder, ImageStoreCheking, StoreCoverage, SaleEmployeeLocation,
                ListIndirectSalesOrder, TopSaleEmployeeStoreChecking, FilterListTime, FilterListOrganization
            } },
        };
    }
}
