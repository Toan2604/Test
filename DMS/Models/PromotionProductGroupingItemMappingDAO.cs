using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class PromotionProductGroupingItemMappingDAO
    {
        public long PromotionProductGroupingId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }

        public virtual ItemDAO Item { get; set; }
        public virtual PromotionProductGroupingDAO PromotionProductGrouping { get; set; }
    }
}
