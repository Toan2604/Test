using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class StoreCheckingImageMappingDAO
    {
        public long ImageId { get; set; }
        public long StoreCheckingId { get; set; }
        public long AlbumId { get; set; }
        public long StoreId { get; set; }
        public long SaleEmployeeId { get; set; }
        public DateTime ShootingAt { get; set; }
        public long? Distance { get; set; }

        public virtual AlbumDAO Album { get; set; }
        public virtual ImageDAO Image { get; set; }
        public virtual AppUserDAO SaleEmployee { get; set; }
        public virtual StoreDAO Store { get; set; }
        public virtual StoreCheckingDAO StoreChecking { get; set; }
    }
}
