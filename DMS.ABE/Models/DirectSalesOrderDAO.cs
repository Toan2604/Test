using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    /// <summary>
    /// Header v&#224; Footer &#273;&#417;n h&#224;ng tr&#7921;c ti&#7871;p
    /// </summary>
    public partial class DirectSalesOrderDAO
    {
        public DirectSalesOrderDAO()
        {
            DirectSalesOrderContents = new HashSet<DirectSalesOrderContentDAO>();
            DirectSalesOrderPromotions = new HashSet<DirectSalesOrderPromotionDAO>();
            DirectSalesOrderTransactions = new HashSet<DirectSalesOrderTransactionDAO>();
        }

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Mã đơn hàng
        /// </summary>
        public string Code { get; set; }
        public string ErpCode { get; set; }
        public long OrganizationId { get; set; }
        /// <summary>
        /// Cửa hàng mua
        /// </summary>
        public long BuyerStoreId { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }
        public string StoreAddress { get; set; }
        /// <summary>
        /// Địa chỉ giao hàng
        /// </summary>
        public string DeliveryAddress { get; set; }
        /// <summary>
        /// Nhân viên kinh doanh
        /// </summary>
        public long SaleEmployeeId { get; set; }
        /// <summary>
        /// Ngày đặt hàng
        /// </summary>
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// Ngày giao hàng
        /// </summary>
        public DateTime? DeliveryDate { get; set; }
        public long? ErpApprovalStateId { get; set; }
        public long? StoreApprovalStateId { get; set; }
        public long RequestStateId { get; set; }
        public long? DirectSalesOrderSourceTypeId { get; set; }
        /// <summary>
        /// Sửa giá
        /// </summary>
        public long EditedPriceStatusId { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }
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
        /// <summary>
        /// Id global cho phê duyệt
        /// </summary>
        public Guid RowId { get; set; }
        public long? StoreCheckingId { get; set; }
        public long? StoreUserCreatorId { get; set; }
        public long? CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long GeneralApprovalStateId { get; set; }
        public long? StoreBalanceCheckStateId { get; set; }
        public long? InventoryCheckStateId { get; set; }
        public long BuyerStoreTypeId { get; set; }

        public virtual StoreDAO BuyerStore { get; set; }
        public virtual StoreTypeDAO BuyerStoreType { get; set; }
        public virtual AppUserDAO Creator { get; set; }
        public virtual DirectSalesOrderSourceTypeDAO DirectSalesOrderSourceType { get; set; }
        public virtual EditedPriceStatusDAO EditedPriceStatus { get; set; }
        public virtual ErpApprovalStateDAO ErpApprovalState { get; set; }
        public virtual GeneralApprovalStateDAO GeneralApprovalState { get; set; }
        public virtual CheckStateDAO InventoryCheckState { get; set; }
        public virtual OrganizationDAO Organization { get; set; }
        public virtual RequestStateDAO RequestState { get; set; }
        public virtual AppUserDAO SaleEmployee { get; set; }
        public virtual StoreApprovalStateDAO StoreApprovalState { get; set; }
        public virtual CheckStateDAO StoreBalanceCheckState { get; set; }
        public virtual StoreCheckingDAO StoreChecking { get; set; }
        public virtual StoreUserDAO StoreUserCreator { get; set; }
        public virtual ICollection<DirectSalesOrderContentDAO> DirectSalesOrderContents { get; set; }
        public virtual ICollection<DirectSalesOrderPromotionDAO> DirectSalesOrderPromotions { get; set; }
        public virtual ICollection<DirectSalesOrderTransactionDAO> DirectSalesOrderTransactions { get; set; }
    }
}
