using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class BrandInStoreHistoryDAO
    {
        public long Id { get; set; }
        public long BrandInStoreId { get; set; }
        public long StoreId { get; set; }
        public long BrandId { get; set; }
        public long Top { get; set; }
        public long CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
