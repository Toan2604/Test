using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.monitor
{
    public class DashboardMonitor_StatisticIndirectSalesOrderDTO : DataDTO
    {
        public long Sum => StatisticIndirectSalesOrderHours.Sum(x => x.Counter);
        public List<DashboardMonitor_StatisticIndirectSalesOrderHourDTO> StatisticIndirectSalesOrderHours { get; set; }
    }

    public class DashboardMonitor_StatisticIndirectSalesOrderHourDTO : DataDTO
    {
        public string Hour { get; set; }
        public long Counter { get; set; }
    }
}
