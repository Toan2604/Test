using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class StoreCheckingDAO
    {
        public StoreCheckingDAO()
        {
            DirectSalesOrders = new HashSet<DirectSalesOrderDAO>();
            Problems = new HashSet<ProblemDAO>();
            StoreCheckingImageMappings = new HashSet<StoreCheckingImageMappingDAO>();
        }

        public long Id { get; set; }
        public long StoreId { get; set; }
        public long SaleEmployeeId { get; set; }
        public long OrganizationId { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? CheckOutLongitude { get; set; }
        public decimal? CheckOutLatitude { get; set; }
        public DateTime? CheckInAt { get; set; }
        public DateTime? CheckOutAt { get; set; }
        public long? CheckInDistance { get; set; }
        public long? CheckOutDistance { get; set; }
        public long? IndirectSalesOrderCounter { get; set; }
        public long? ImageCounter { get; set; }
        public bool Planned { get; set; }
        public bool IsOpenedStore { get; set; }
        public string DeviceName { get; set; }

        public virtual OrganizationDAO Organization { get; set; }
        public virtual AppUserDAO SaleEmployee { get; set; }
        public virtual StoreDAO Store { get; set; }
        public virtual ICollection<DirectSalesOrderDAO> DirectSalesOrders { get; set; }
        public virtual ICollection<ProblemDAO> Problems { get; set; }
        public virtual ICollection<StoreCheckingImageMappingDAO> StoreCheckingImageMappings { get; set; }
    }
}
