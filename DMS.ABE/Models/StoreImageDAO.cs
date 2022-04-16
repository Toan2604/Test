using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class StoreImageDAO
    {
        public long ImageId { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public long AlbumId { get; set; }
        public string AlbumName { get; set; }
        public long StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public DateTime ShootingAt { get; set; }
        public long? SaleEmployeeId { get; set; }
        public string DisplayName { get; set; }
        public long OrganizationId { get; set; }
        public long? Distance { get; set; }
    }
}
