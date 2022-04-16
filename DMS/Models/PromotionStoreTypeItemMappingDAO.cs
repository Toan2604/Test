using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class PromotionStoreTypeItemMappingDAO
    {
        public long PromotionStoreTypeId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }

        public virtual ItemDAO Item { get; set; }
        public virtual PromotionStoreTypeDAO PromotionStoreType { get; set; }
    }
}
