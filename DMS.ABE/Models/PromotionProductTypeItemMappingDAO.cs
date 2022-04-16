using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class PromotionProductTypeItemMappingDAO
    {
        public long PromotionProductTypeId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }

        public virtual ItemDAO Item { get; set; }
        public virtual PromotionProductTypeDAO PromotionProductType { get; set; }
    }
}
