using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class DirectSalesOrder : DataEntity, IEquatable<DirectSalesOrder>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public long OrganizationId { get; set; }
        public long BuyerStoreId { get; set; }
        public long BuyerStoreTypeId { get; set; }
        public string PhoneNumber { get; set; }
        public string StoreAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public long SaleEmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public long? ErpApprovalStateId { get; set; }
        public long? StoreApprovalStateId { get; set; }
        public long RequestStateId { get; set; }
        public long EditedPriceStatusId { get; set; }
        public string Note { get; set; }
        public decimal SubTotal { get; set; }
        public decimal? GeneralDiscountPercentage { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalAfterTax { get; set; }
        public string PromotionCode { get; set; }
        public decimal? PromotionValue { get; set; }
        public decimal Total { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalRequestedQuantity { get; set; }
        public Guid RowId { get; set; }
        public long? StoreCheckingId { get; set; }
        public long? CreatorId { get; set; }
        public long? StoreUserCreatorId { get; set; }
        public long? DirectSalesOrderSourceTypeId { get; set; }
        public long GeneralApprovalStateId { get; set; }
        public long? StoreBalanceCheckStateId { get; set; }
        public long? InventoryCheckStateId { get; set; }
        public GeneralApprovalState GeneralApprovalState { get; set; }
        public Store BuyerStore { get; set; }
        public AppUser Creator { get; set; }
        public StoreUser StoreUserCreator { get; set; }
        public DirectSalesOrderSourceType DirectSalesOrderSourceType { get; set; }
        public EditedPriceStatus EditedPriceStatus { get; set; }
        public ErpApprovalState ErpApprovalState { get; set; }
        public Organization Organization { get; set; }
        public RequestState RequestState { get; set; }
        public AppUser SaleEmployee { get; set; }
        public StoreApprovalState StoreApprovalState { get; set; }
        public CheckState StoreBalanceCheckState { get; set; }
        public CheckState InventoryCheckState { get; set; }
        public StoreChecking StoreChecking { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<DirectSalesOrderContent> DirectSalesOrderContents { get; set; }
        public List<DirectSalesOrderPromotion> DirectSalesOrderPromotions { get; set; }

        public bool Equals(DirectSalesOrder other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class DirectSalesOrderFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter ApproverId { get; set; }
        public StringFilter Code { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter BuyerStoreId { get; set; }
        public StringFilter PhoneNumber { get; set; }
        public StringFilter StoreAddress { get; set; }
        public StringFilter DeliveryAddress { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter StoreStatusId { get; set; }
        public IdFilter SaleEmployeeId { get; set; }
        public DateFilter OrderDate { get; set; }
        public DateFilter DeliveryDate { get; set; }
        public IdFilter ErpApprovalStateId { get; set; }
        public IdFilter StoreApprovalStateId { get; set; }
        public IdFilter RequestStateId { get; set; }
        public IdFilter EditedPriceStatusId { get; set; }
        public StringFilter Note { get; set; }
        public DecimalFilter SubTotal { get; set; }
        public DecimalFilter GeneralDiscountPercentage { get; set; }
        public DecimalFilter GeneralDiscountAmount { get; set; }
        public DecimalFilter TotalTaxAmount { get; set; }
        public DecimalFilter TotalAfterTax { get; set; }
        public StringFilter PromotionCode { get; set; }
        public DecimalFilter PromotionValue { get; set; }
        public DecimalFilter Total { get; set; }
        public GuidFilter RowId { get; set; }
        public IdFilter StoreCheckingId { get; set; }
        public IdFilter CreatorId { get; set; }
        public IdFilter StoreUserCreatorId { get; set; }
        public IdFilter DirectSalesOrderSourceTypeId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public IdFilter GeneralApprovalStateId { get; set; }
        public List<DirectSalesOrderFilter> OrFilter { get; set; }
        public DirectSalesOrderOrder OrderBy { get; set; }
        public DirectSalesOrderSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DirectSalesOrderOrder
    {
        Id = 0,
        Code = 1,
        Organization = 2,
        BuyerStore = 3,
        PhoneNumber = 4,
        StoreAddress = 5,
        DeliveryAddress = 6,
        SaleEmployee = 7,
        OrderDate = 8,
        DeliveryDate = 9,
        ErpApprovalState = 10,
        StoreApprovalState = 11,
        RequestState = 12,
        EditedPriceStatus = 13,
        Note = 14,
        SubTotal = 15,
        GeneralDiscountPercentage = 16,
        GeneralDiscountAmount = 17,
        TotalTaxAmount = 18,
        TotalAfterTax = 19,
        PromotionCode = 20,
        PromotionValue = 21,
        Total = 22,
        Row = 23,
        StoreChecking = 24,
        Creator = 25,
        StoreUserCreator = 26,
        DirectSalesOrderSourceType = 29,
        GeneralApprovalState = 30,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum DirectSalesOrderSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Organization = E._2,
        BuyerStore = E._3,
        PhoneNumber = E._4,
        StoreAddress = E._5,
        DeliveryAddress = E._6,
        SaleEmployee = E._7,
        OrderDate = E._8,
        DeliveryDate = E._9,
        ErpApprovalState = E._10,
        StoreApprovalState = E._11,
        RequestState = E._12,
        EditedPriceStatus = E._13,
        Note = E._14,
        SubTotal = E._15,
        GeneralDiscountPercentage = E._16,
        GeneralDiscountAmount = E._17,
        TotalTaxAmount = E._18,
        TotalAfterTax = E._19,
        PromotionCode = E._20,
        PromotionValue = E._21,
        Total = E._22,
        Row = E._23,
        StoreChecking = E._24,
        Creator = E._25,
        StoreUserCreator = E._26,
        DirectSalesOrderSourceType = E._29,
        GeneralApprovalState = E._30,
        InventoryCheckState = E._31,
        StoreBalanceCheckState = E._32,
    }
}
