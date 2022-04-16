using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class PromotionDirectSalesOrderItemMappingDAO
    {
        public long PromotionDirectSalesOrderId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }

        public virtual ItemDAO Item { get; set; }
        public virtual PromotionDirectSalesOrderDAO PromotionDirectSalesOrder { get; set; }
    }
}
