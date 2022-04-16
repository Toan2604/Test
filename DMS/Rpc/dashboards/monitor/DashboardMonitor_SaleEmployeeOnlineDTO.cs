using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.monitor
{
    public class DashboardMonitor_SaleEmployeeOnlineDTO : DataDTO
    {
        public long Sum => SaleEmployeeOnlineHours.Sum(x => x.Counter);
        public List<DashboardMonitor_SaleEmployeeOnlineHourDTO> SaleEmployeeOnlineHours { get; set; }
    }

    public class DashboardMonitor_SaleEmployeeOnlineHourDTO : DataDTO
    {
        public string Hour { get; set; }
        public long Counter { get; set; }
    }
}
