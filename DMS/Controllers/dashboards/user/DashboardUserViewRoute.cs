using System.ComponentModel;

namespace DMS.Controllers.dashboards.user
{
    [DisplayName("Dashboard của tôi")]
    public class DashboardUserViewRoute : ViewRoot
    {
        public const string Parent = Module + "/dashboards";
        public const string Master = Module + "/dashboards/store-information";
        private const string Default = Rpc + Module + "/dashboards/user";


        public const string StoreChecking = Default + "/store-checking";
        public const string ListComment = Default + "/list-comment";

        private const string IndirectSalesOrder = Default + "/indirect-sales-order";
        public const string SalesQuantity = IndirectSalesOrder + "/sales-quantity";
        public const string Revenue = IndirectSalesOrder + "/revenue";
        public const string StatisticIndirectSalesOrder = IndirectSalesOrder + "/statistic-sales-order";
        public const string ListIndirectSalesOrder = IndirectSalesOrder + "/list-sales-order";

        private const string DirectSalesOrder = Default + "/direct-sales-order";
        public const string DirectSalesQuantity = DirectSalesOrder + "/sales-quantity";
        public const string DirectRevenue = DirectSalesOrder + "/revenue";
        public const string StatisticDirectSalesOrder = DirectSalesOrder + "/statistic-sales-order";
        public const string ListDirectSalesOrder = DirectSalesOrder + "/list-sales-order";
    }
}
