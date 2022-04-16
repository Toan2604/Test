using TrueSight.Common;

namespace DMS.Rpc.dashboards.director
{
    public class DashboardDirector_FilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter Time { get; set; }
        public DateFilter OrderDate { get; set; }
    }
}
