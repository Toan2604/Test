using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class PromotionProductItemMappingDAO
    {
        public long PromotionProductId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }

        public virtual ItemDAO Item { get; set; }
        public virtual PromotionProductDAO PromotionProduct { get; set; }
    }
}
