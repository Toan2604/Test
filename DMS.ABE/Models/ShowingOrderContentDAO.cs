using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class ShowingOrderContentDAO
    {
        public long Id { get; set; }
        public long ShowingOrderId { get; set; }
        public long ShowingItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal SalePrice { get; set; }
        public long Quantity { get; set; }
        public decimal Amount { get; set; }

        public virtual ShowingItemDAO ShowingItem { get; set; }
        public virtual ShowingOrderDAO ShowingOrder { get; set; }
        public virtual UnitOfMeasureDAO UnitOfMeasure { get; set; }
    }
}
