using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class PromotionStoreItemMappingDAO
    {
        public long PromotionStoreId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }

        public virtual ItemDAO Item { get; set; }
        public virtual PromotionStoreDAO PromotionStore { get; set; }
    }
}
