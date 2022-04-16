using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class PromotionComboInItemMappingDAO
    {
        public long PromotionComboId { get; set; }
        public long ItemId { get; set; }
        public long From { get; set; }
        public long? To { get; set; }

        public virtual ItemDAO Item { get; set; }
        public virtual PromotionComboDAO PromotionCombo { get; set; }
    }
}
