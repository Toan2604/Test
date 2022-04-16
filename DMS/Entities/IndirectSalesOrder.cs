using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class IndirectSalesOrder : DataEntity, IEquatable<IndirectSalesOrder>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public long OrganizationId { get; set; }
        public long BuyerStoreId { get; set; }
        public string PhoneNumber { get; set; }
        public string StoreAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public long SellerStoreId { get; set; }
        public long BuyerStoreTypeId { get; set; }
        public long SaleEmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public long RequestStateId { get; set; }
        public long EditedPriceStatusId { get; set; }
        public string Note { get; set; }
        public decimal SubTotal { get; set; }
        public decimal? GeneralDiscountPercentage { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public decimal Total { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalRequestedQuantity { get; set; }
        public long StoreStatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long CreatorId { get; set; }
        public Store BuyerStore { get; set; }
        public EditedPriceStatus EditedPriceStatus { get; set; }
        public Organization Organization { get; set; }
        public RequestState RequestState { get; set; }
        public AppUser SaleEmployee { get; set; }
        public AppUser Creator { get; set; }
        public Store SellerStore { get; set; }
        public Guid RowId { get; set; }
        public long? StoreCheckingId { get; set; }
        public List<IndirectSalesOrderContent> IndirectSalesOrderContents { get; set; }
        public List<IndirectSalesOrderPromotion> IndirectSalesOrderPromotions { get; set; }
        public List<RequestWorkflowStepMapping> RequestWorkflowStepMappings { get; set; }
        public bool Equals(IndirectSalesOrder other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class IndirectSalesOrderFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter OrganizationId { get; set; }
        public StringFilter Code { get; set; }
        public IdFilter BuyerStoreId { get; set; }
        public StringFilter BuyerStoreCode { get; set; }
        public StringFilter PhoneNumber { get; set; }
        public StringFilter StoreAddress { get; set; }
        public StringFilter DeliveryAddress { get; set; }
        public IdFilter SellerStoreId { get; set; }
        public IdFilter BuyerStoreTypeId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter ApproverId { get; set; }
        public DateFilter OrderDate { get; set; }
        public DateFilter DeliveryDate { get; set; }
        public IdFilter RequestStateId { get; set; }
        public IdFilter EditedPriceStatusId { get; set; }
        public StringFilter Note { get; set; }
        public DecimalFilter SubTotal { get; set; }
        public DecimalFilter GeneralDiscountPercentage { get; set; }
        public DecimalFilter GeneralDiscountAmount { get; set; }
        public DecimalFilter Total { get; set; }
        public IdFilter StoreCheckingId { get; set; }
        public IdFilter StoreStatusId { get; set; }
        public string Search { get; set; }
        public List<IndirectSalesOrderFilter> OrFilter { get; set; }
        public IndirectSalesOrderOrder OrderBy { get; set; }
        public IndirectSalesOrderSelect Selects { get; set; }

        public long? UserId { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum IndirectSalesOrderOrder
    {
        Id = 0,
        Code = 1,
        BuyerStore = 2,
        PhoneNumber = 3,
        StoreAddress = 4,
        DeliveryAddress = 5,
        SellerStore = 6,
        SaleEmployee = 7,
        OrderDate = 8,
        DeliveryDate = 9,
        RequestState = 10,
        EditedPriceStatus = 11,
        Note = 12,
        SubTotal = 13,
        GeneralDiscountPercentage = 14,
        GeneralDiscountAmount = 15,
        Total = 17,
        Organization = 18,
        UpdatedAt = 19,
        CreatedAt = 20,
        BuyerStoreType = 21,
    }

    [Flags]
    public enum IndirectSalesOrderSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        BuyerStore = E._2,
        PhoneNumber = E._3,
        StoreAddress = E._4,
        DeliveryAddress = E._5,
        SellerStore = E._6,
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
        StoreStatusId = E._19,
        CreatedAt = E._20,
        BuyerStoreType = E._21,
        StoreChecking = E._22,
    }
}
