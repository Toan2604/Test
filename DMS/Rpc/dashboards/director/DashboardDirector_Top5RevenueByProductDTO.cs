using TrueSight.Common;

namespace DMS.Rpc.dashboards.director
{
    public class DashboardDirector_Top5RevenueByProductDTO : DataDTO
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public decimal Revenue { get; set; }
    }

    public class DashboardDirector_Top5RevenueByProductFilterDTO : DashboardDirector_FilterDTO
    {

    }
}
