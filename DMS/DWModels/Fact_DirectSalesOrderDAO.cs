using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Fact_DirectSalesOrderDAO
    {
        public long DirectSalesOrderId { get; set; }
        public long BuyerStoreId { get; set; }
        public long OrganizationId { get; set; }
        public long SaleEmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// Mã đơn hàng
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Ngày giao hàng
        /// </summary>
        public DateTime? DeliveryDate { get; set; }
        public long RequestStateId { get; set; }
        public long? DirectSalesOrderSourceTypeId { get; set; }
        /// <summary>
        /// Sửa giá
        /// </summary>
        public long EditedPriceStatusId { get; set; }
        /// <summary>
        /// Tổng tiền trước thuế
        /// </summary>
        public decimal SubTotal { get; set; }
        /// <summary>
        /// % chiết khấu tổng
        /// </summary>
        public decimal? GeneralDiscountPercentage { get; set; }
        /// <summary>
        /// Số tiền chiết khấu tổng
        /// </summary>
        public decimal? GeneralDiscountAmount { get; set; }
        public decimal? TotalDiscountAmount { get; set; }
        /// <summary>
        /// Tổng thuế
        /// </summary>
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalAfterTax { get; set; }
        public string PromotionCode { get; set; }
        public decimal? PromotionValue { get; set; }
        /// <summary>
        /// Tổng tiền sau thuế
        /// </summary>
        public decimal Total { get; set; }
        public long? StoreCheckingId { get; set; }
        public long? StoreUserCreatorId { get; set; }
        public long? CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long? ErpApprovalStateId { get; set; }
        public long? StoreApprovalStateId { get; set; }
        public long GeneralApprovalStateId { get; set; }
        public long BuyerStoreTypeId { get; set; }
    }
}
