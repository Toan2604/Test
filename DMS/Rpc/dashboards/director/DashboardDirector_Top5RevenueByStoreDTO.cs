using TrueSight.Common;

namespace DMS.Rpc.dashboards.director
{
    public class DashboardDirector_Top5RevenueByEmployeeDTO : DataDTO
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public decimal Revenue { get; set; }
    }

    public class DashboardDirector_Top5RevenueByEmployeeFilterDTO : DashboardDirector_FilterDTO
    {

    }
}
