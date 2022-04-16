using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    /// <summary>
    /// &#160;C&#225;c s&#7843;n ph&#7849;m b&#225;n c&#7911;a &#273;&#417;n h&#224;ng gi&#225;n ti&#7871;p
    /// </summary>
    public partial class IndirectSalesOrderContentDAO
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// FK của đơn hàng gián tiếp
        /// </summary>
        public long IndirectSalesOrderId { get; set; }
        /// <summary>
        /// Sản phẩm
        /// </summary>
        public long ItemId { get; set; }
        /// <summary>
        /// Đơn vị để xuất hàng
        /// </summary>
        public long UnitOfMeasureId { get; set; }
        /// <summary>
        /// Số lượng xuất hàng
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Đơn vị lưu kho
        /// </summary>
        public long PrimaryUnitOfMeasureId { get; set; }
        /// <summary>
        /// Số lượng xuất hàng theo đơn vị lưu kho
        /// </summary>
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
        /// <summary>
        /// % chiết khấu theo dòng
        /// </summary>
        public decimal? DiscountPercentage { get; set; }
        /// <summary>
        /// Số tiền chiết khấu theo dòng
        /// </summary>
        public decimal? DiscountAmount { get; set; }
        /// <summary>
        /// % chiết khấu tổng 
        /// </summary>
        public decimal? GeneralDiscountPercentage { get; set; }
        /// <summary>
        /// Số tiền sau chiết khấu từng dòng
        /// </summary>
        public decimal? GeneralDiscountAmount { get; set; }
        /// <summary>
        /// % thuế lấy theo sản phẩm
        /// </summary>
        public decimal? TaxPercentage { get; set; }
        /// <summary>
        /// Số tiền thuế sau tất cả các loại chiết khấu
        /// </summary>
        public decimal? TaxAmount { get; set; }
        /// <summary>
        /// Tổng số tiền từng dòng trước chiết khấu tổng
        /// </summary>
        public decimal Amount { get; set; }
        public long? Factor { get; set; }

        public virtual EditedPriceStatusDAO EditedPriceStatus { get; set; }
        public virtual IndirectSalesOrderDAO IndirectSalesOrder { get; set; }
        public virtual ItemDAO Item { get; set; }
        public virtual UnitOfMeasureDAO PrimaryUnitOfMeasure { get; set; }
        public virtual UnitOfMeasureDAO UnitOfMeasure { get; set; }
    }
}
