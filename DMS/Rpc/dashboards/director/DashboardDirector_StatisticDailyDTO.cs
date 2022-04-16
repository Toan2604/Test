using TrueSight.Common;

namespace DMS.Rpc.dashboards.director
{
    public class DashboardDirector_StatisticDailyDTO : DataDTO
    {
        public decimal Revenue { get; set; }
        public long IndirectSalesOrderCounter { get; set; }
        public long StoreHasCheckedCounter { get; set; }
        public long StoreCheckingCounter { get; set; }
    }

    public class DashboardDirector_StatisticDailyDirectSalesOrderDTO : DataDTO
    {
        public decimal Revenue { get; set; }
        public long DirectSalesOrderCounter { get; set; }
        public long StoreHasCheckedCounter { get; set; }
        public long StoreCheckingCounter { get; set; }
    }
}
