using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class DirectSalesOrder : DataEntity, IEquatable<DirectSalesOrder>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string ErpCode { get; set; }
        public long OrganizationId { get; set; }
        public long BuyerStoreId { get; set; }
        public long BuyerStoreTypeId { get; set; }
        public string PhoneNumber { get; set; }
        public string StoreAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public long SaleEmployeeId { get; set; }
        public long? CreatorId { get; set; }
        public long? StoreApprovalStateId { get; set; }
        public long? ErpApprovalStateId { get; set; }
        public long? StoreUserCreatorId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public long RequestStateId { get; set; }
        public long EditedPriceStatusId { get; set; }
        public string Note { get; set; }
        public decimal SubTotal { get; set; }
        public decimal? GeneralDiscountPercentage { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalAfterTax { get; set; }
        public long? PromotionCodeId { get; set; }
        public string PromotionCode { get; set; }
        public decimal? PromotionValue { get; set; }
        public decimal Total { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalRequestedQuantity { get; set; }
        public long GeneralApprovalStateId { get; set; }
        public long? StoreBalanceCheckStateId { get; set; }
        public long? InventoryCheckStateId { get; set; }
        public GeneralApprovalState GeneralApprovalState { get; set; }
        public Store BuyerStore { get; set; }
        public DirectSalesOrderSourceType DirectSalesOrderSourceType { get; set; }
        public EditedPriceStatus EditedPriceStatus { get; set; }
        public Organization Organization { get; set; }
        public RequestState RequestState { get; set; }
        public StoreApprovalState StoreApprovalState { get; set; }
        public CheckState StoreBalanceCheckState { get; set; }
        public CheckState InventoryCheckState { get; set; }
        public ErpApprovalState ErpApprovalState { get; set; }
        public AppUser SaleEmployee { get; set; }
        public AppUser Creator { get; set; }
        public StoreUser StoreUserCreator { get; set; }
        public Guid RowId { get; set; }
        public long? StoreCheckingId { get; set; }
        public long? DirectSalesOrderSourceTypeId { get; set; }
        public List<DirectSalesOrderContent> DirectSalesOrderContents { get; set; }
        public List<DirectSalesOrderPromotion> DirectSalesOrderPromotions { get; set; }
        public List<RequestWorkflowStepMapping> RequestWorkflowStepMappings { get; set; }
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
        public IdFilter OrganizationId { get; set; }
        public StringFilter Code { get; set; }
        public IdFilter BuyerStoreId { get; set; }
        public StringFilter BuyerStoreCode { get; set; }
        public StringFilter PhoneNumber { get; set; }
        public StringFilter StoreAddress { get; set; }
        public StringFilter DeliveryAddress { get; set; }
        public IdFilter AppUserId { get; set; }
        public DateFilter OrderDate { get; set; }
        public DateFilter DeliveryDate { get; set; }
        public IdFilter RequestStateId { get; set; }
        public IdFilter EditedPriceStatusId { get; set; }
        public IdFilter DirectSalesOrderSourceTypeId { get; set; }
        public StringFilter Note { get; set; }
        public StringFilter PromotionCode { get; set; }
        public DecimalFilter SubTotal { get; set; }
        public DecimalFilter GeneralDiscountPercentage { get; set; }
        public DecimalFilter GeneralDiscountAmount { get; set; }
        public DecimalFilter TotalTaxAmount { get; set; }
        public DecimalFilter Total { get; set; }
        public IdFilter StoreCheckingId { get; set; }
        public IdFilter StoreApprovalStateId { get; set; }
        public IdFilter ErpApprovalStateId { get; set; }
        public IdFilter StoreStatusId { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public IdFilter GeneralApprovalStateId { get; set; }
        public IdFilter StoreBalanceCheckStateId { get; set; }
        public IdFilter InventoryCheckStateId { get; set; }
        public string Search { get; set; }
        public List<DirectSalesOrderFilter> OrFilter { get; set; }
        public DirectSalesOrderOrder OrderBy { get; set; }
        public DirectSalesOrderSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DirectSalesOrderOrder
    {
        Id = 0,
        Code = 1,
        BuyerStore = 2,
        PhoneNumber = 3,
        StoreAddress = 4,
        DeliveryAddress = 5,
        SaleEmployee = 7,
        OrderDate = 8,
        DeliveryDate = 9,
        RequestState = 10,
        EditedPriceStatus = 11,
        Note = 12,
        SubTotal = 13,
        GeneralDiscountPercentage = 14,
        GeneralDiscountAmount = 15,
        TotalTaxAmount = 16,
        Total = 17,
        Organization = 18,
        CreatedAt = 19,
        UpdatedAt = 20,
        StoreApprovalStateId = 21,
        ErpApprovalStateId = 22,
        GeneralApprovalStateId = 27,
        StoreBalanceCheckStateId = 28,
        InventoryCheckStateId = 29,
    }

    [Flags]
    public enum DirectSalesOrderSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        BuyerStore = E._2,
        PhoneNumber = E._3,
        StoreAddress = E._4,
        DeliveryAddress = E._5,
        SaleEmployee = E._7,
        OrderDate = E._8,
        DeliveryDate = E._9,
        RequestState = E._10,
        EditedPriceStatus = E._11,
        Note = E._12,
        SubTotal = E._13,
        GeneralDiscountPercentage = E._14,
        GeneralDiscountAmount = E._15,
        TotalTaxAmount = E._16,
        Total = E._17,
        Organization = E._18,
        PromotionCode = E._19,
        PromotionValue = E._20,
        TotalAfterTax = E._21,
        UpdatedAt = E._22,
        DirectSalesOrderContents = E._23,
        DirectSalesOrderPromotions = E._24,
        StoreApprovalState = E._25,
        ErpApprovalState = E._26,
        GeneralApprovalState = E._27,
        StoreBalanceCheckState = E._28,
        InventoryCheckState = E._29,
        CreatedAt = E._30,
        DirectSalesOrderSourceType = E._31,
        StoreChecking = E._32,
    }
}
