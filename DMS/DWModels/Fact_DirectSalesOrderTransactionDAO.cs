using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Fact_DirectSalesOrderTransactionDAO
    {
        public long DirectSalesOrderTransactionId { get; set; }
        public long DirectSalesOrderId { get; set; }
        public long OrganizationId { get; set; }
        public long BuyerStoreId { get; set; }
        public long SalesEmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public long RequestStateId { get; set; }
        /// <summary>
        /// Ngày giao hàng
        /// </summary>
        public DateTime? DeliveryDate { get; set; }
        public long TransactionTypeId { get; set; }
        public long ItemId { get; set; }
        /// <summary>
        /// Giá bán theo đơn vị xuất hàng
        /// </summary>
        public decimal? SalePrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal? RequestedQuantity { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? GeneralDiscountPercentage { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal Amount { get; set; }
        public long? Factor { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
