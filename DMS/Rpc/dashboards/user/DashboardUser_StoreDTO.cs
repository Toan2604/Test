using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.user
{
    public class DashboardUser_StoreDTO : DataDTO
    {
        public long Id { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string OwnerEmail { get; set; }
        public string Address { get; set; }
        public DashboardUser_StoreDTO() { }
        public DashboardUser_StoreDTO(Store Store)
        {
            this.Id = Store.Id;
            this.Longitude = Store.Longitude;
            this.Latitude = Store.Latitude;
            this.Name = Store.Name;
            this.OwnerEmail = Store.OwnerEmail;
            this.Address = Store.Address;
            this.Telephone = Store.Telephone;
        }
    }
}
