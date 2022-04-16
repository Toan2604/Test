using System;
using System.Collections.Generic;

namespace DMS.Models
{
    /// <summary>
    /// C&#225;c s&#7843;n ph&#7849;m khuy&#7871;n m&#227;i c&#7911;a &#273;&#417;n h&#224;ng gi&#225;n ti&#7871;p
    /// </summary>
    public partial class IndirectSalesOrderPromotionDAO
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Id đơn hàng gián tiếp
        /// </summary>
        public long IndirectSalesOrderId { get; set; }
        /// <summary>
        /// Sản phẩm khuyến mãi
        /// </summary>
        public long ItemId { get; set; }
        /// <summary>
        /// Đơn vị xuất kho
        /// </summary>
        public long UnitOfMeasureId { get; set; }
        /// <summary>
        /// Số lượng bán
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Đơn vị lưu kho
        /// </summary>
        public long PrimaryUnitOfMeasureId { get; set; }
        /// <summary>
        /// Số lượng yêu cầu xuất kho
        /// </summary>
        public decimal RequestedQuantity { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }
        public long? Factor { get; set; }

        public virtual IndirectSalesOrderDAO IndirectSalesOrder { get; set; }
        public virtual ItemDAO Item { get; set; }
        public virtual UnitOfMeasureDAO PrimaryUnitOfMeasure { get; set; }
        public virtual UnitOfMeasureDAO UnitOfMeasure { get; set; }
    }
}
