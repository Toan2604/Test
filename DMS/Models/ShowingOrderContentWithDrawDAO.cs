using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class ShowingOrderContentWithDrawDAO
    {
        public long Id { get; set; }
        public long ShowingOrderWithDrawId { get; set; }
        public long ShowingItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal SalePrice { get; set; }
        public long Quantity { get; set; }
        public decimal Amount { get; set; }

        public virtual ShowingItemDAO ShowingItem { get; set; }
        public virtual ShowingOrderWithDrawDAO ShowingOrderWithDraw { get; set; }
        public virtual UnitOfMeasureDAO UnitOfMeasure { get; set; }
    }
}
