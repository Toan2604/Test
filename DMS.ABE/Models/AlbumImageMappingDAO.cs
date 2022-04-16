using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    /// <summary>
    /// B&#7843;ng l&#432;u danh s&#225;ch &#7843;nh c&#7911;a 1 album
    /// </summary>
    public partial class AlbumImageMappingDAO
    {
        public long ImageId { get; set; }
        public long AlbumId { get; set; }
        public long StoreId { get; set; }
        public DateTime ShootingAt { get; set; }
        public long OrganizationId { get; set; }
        public long? SaleEmployeeId { get; set; }
        public long? Distance { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual AlbumDAO Album { get; set; }
        public virtual ImageDAO Image { get; set; }
        public virtual StoreDAO Store { get; set; }
    }
}
