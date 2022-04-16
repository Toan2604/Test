using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.monitor
{
    public class DashboardMonitor_StoreDTO : DataDTO
    {
        public long Id { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public bool IsScouting { get; set; }
        public DashboardMonitor_StoreDTO() { }
        public DashboardMonitor_StoreDTO(Store Store)
        {
            this.Id = Store.Id;
            this.Longitude = Store.Longitude;
            this.Latitude = Store.Latitude;
            this.Name = Store.Name;
            this.Telephone = Store.Telephone;
        }
    }

    public class DashboardMonitor_StoreFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
    }
}
