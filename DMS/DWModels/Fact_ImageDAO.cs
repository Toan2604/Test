using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Fact_ImageDAO
    {
        public long ImageId { get; set; }
        public long? StoreCheckingId { get; set; }
        public long AlbumId { get; set; }
        public long StoreId { get; set; }
        public DateTime ShootingAt { get; set; }
        public string Url { get; set; }
        public long? OrganizationId { get; set; }
        public long? SaleEmployeeId { get; set; }
        public long? Distance { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
