using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    /// <summary>
    /// &#272;&#417;n h&#224;ng gi&#225;n ti&#7871;p
    /// </summary>
    public partial class IndirectSalesOrderDAO
    {
        public IndirectSalesOrderDAO()
        {
            IndirectSalesOrderContents = new HashSet<IndirectSalesOrderContentDAO>();
            IndirectSalesOrderPromotions = new HashSet<IndirectSalesOrderPromotionDAO>();
            IndirectSalesOrderTransactions = new HashSet<IndirectSalesOrderTransactionDAO>();
        }

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Mã đơn hàng
        /// </summary>
        public string Code { get; set; }
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
        /// Cửa hàng bán
        /// </summary>
        public long SellerStoreId { get; set; }
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
        /// Tổng tiền sau thuế
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// Id global cho phê duyệt
        /// </summary>
        public Guid RowId { get; set; }
        public long? StoreCheckingId { get; set; }
        public long RequestStateId { get; set; }
        public long CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long BuyerStoreTypeId { get; set; }

        public virtual StoreDAO BuyerStore { get; set; }
        public virtual StoreTypeDAO BuyerStoreType { get; set; }
        public virtual AppUserDAO Creator { get; set; }
        public virtual EditedPriceStatusDAO EditedPriceStatus { get; set; }
        public virtual OrganizationDAO Organization { get; set; }
        public virtual RequestStateDAO RequestState { get; set; }
        public virtual AppUserDAO SaleEmployee { get; set; }
        public virtual StoreDAO SellerStore { get; set; }
        public virtual ICollection<IndirectSalesOrderContentDAO> IndirectSalesOrderContents { get; set; }
        public virtual ICollection<IndirectSalesOrderPromotionDAO> IndirectSalesOrderPromotions { get; set; }
        public virtual ICollection<IndirectSalesOrderTransactionDAO> IndirectSalesOrderTransactions { get; set; }
    }
}
