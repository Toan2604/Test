using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    /// <summary>
    /// Danh s&#225;ch s&#7843;n ph&#7849;m b&#225;n c&#7911;a 1 &#273;&#417;n h&#224;ng tr&#7921;c ti&#7871;p
    /// </summary>
    public partial class DirectSalesOrderContentDAO
    {
        public long Id { get; set; }
        public long DirectSalesOrderId { get; set; }
        public long ItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal Quantity { get; set; }
        public long PrimaryUnitOfMeasureId { get; set; }
        public decimal RequestedQuantity { get; set; }
        /// <summary>
        /// Giá theo đơn vị lưu kho
        /// </summary>
        public decimal PrimaryPrice { get; set; }
        /// <summary>
        /// Giá bán theo đơn vị xuất hàng
        /// </summary>
        public decimal SalePrice { get; set; }
        public long EditedPriceStatusId { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GeneralDiscountPercentage { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal Amount { get; set; }
        public long? Factor { get; set; }
        public long? InventoryCheckStateId { get; set; }

        public virtual DirectSalesOrderDAO DirectSalesOrder { get; set; }
        public virtual EditedPriceStatusDAO EditedPriceStatus { get; set; }
        public virtual CheckStateDAO InventoryCheckState { get; set; }
        public virtual ItemDAO Item { get; set; }
        public virtual UnitOfMeasureDAO PrimaryUnitOfMeasure { get; set; }
        public virtual UnitOfMeasureDAO UnitOfMeasure { get; set; }
    }
}
