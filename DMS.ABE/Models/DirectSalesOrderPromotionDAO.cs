using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    /// <summary>
    /// Danh s&#225;ch s&#7843;n ph&#7849;m khuy&#7871;n m&#227;i c&#7911;a 1 &#273;&#417;n h&#224;ng tr&#7921;c ti&#7871;p
    /// </summary>
    public partial class DirectSalesOrderPromotionDAO
    {
        public long Id { get; set; }
        public long DirectSalesOrderId { get; set; }
        public long ItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal Quantity { get; set; }
        public long PrimaryUnitOfMeasureId { get; set; }
        public decimal RequestedQuantity { get; set; }
        public string Note { get; set; }
        public long? Factor { get; set; }
        public long? InventoryCheckStateId { get; set; }

        public virtual DirectSalesOrderDAO DirectSalesOrder { get; set; }
        public virtual CheckStateDAO InventoryCheckState { get; set; }
        public virtual ItemDAO Item { get; set; }
        public virtual UnitOfMeasureDAO PrimaryUnitOfMeasure { get; set; }
        public virtual UnitOfMeasureDAO UnitOfMeasure { get; set; }
    }
}
