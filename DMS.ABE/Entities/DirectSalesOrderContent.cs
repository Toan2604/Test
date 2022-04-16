using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class DirectSalesOrderContent : DataEntity, IEquatable<DirectSalesOrderContent>
    {
        public long Id { get; set; }
        public long DirectSalesOrderId { get; set; }
        public long ItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal Quantity { get; set; }
        public long PrimaryUnitOfMeasureId { get; set; }
        public decimal RequestedQuantity { get; set; }
        public decimal PrimaryPrice { get; set; }
        public decimal SalePrice { get; set; }
        public long EditedPriceStatusId { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GeneralDiscountPercentage { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal? TaxAmount { get; set; }
        public long? Factor { get; set; }
        public long? InventoryCheckStateId { get; set; }
        public EditedPriceStatus EditedPriceStatus { get; set; }
        public Item Item { get; set; }
        public DirectSalesOrder DirectSalesOrder { get; set; }
        public UnitOfMeasure PrimaryUnitOfMeasure { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public CheckState InventoryCheckState { get; set; }

        public bool Equals(DirectSalesOrderContent other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class DirectSalesOrderContentFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter DirectSalesOrderId { get; set; }
        public IdFilter ProductId { get; set; }
        public IdFilter ItemId { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public DecimalFilter Quantity { get; set; }
        public IdFilter PrimaryUnitOfMeasureId { get; set; }
        public DecimalFilter RequestedQuantity { get; set; }
        public DecimalFilter PrimaryPrice { get; set; }
        public DecimalFilter SalePrice { get; set; }
        public DecimalFilter DiscountPercentage { get; set; }
        public DecimalFilter DiscountAmount { get; set; }
        public DecimalFilter GeneralDiscountPercentage { get; set; }
        public DecimalFilter GeneralDiscountAmount { get; set; }
        public DecimalFilter Amount { get; set; }
        public DecimalFilter TaxPercentage { get; set; }
        public DecimalFilter TaxAmount { get; set; }
        public List<DirectSalesOrderContentFilter> OrFilter { get; set; }
        public DirectSalesOrderContentOrder OrderBy { get; set; }
        public DirectSalesOrderContentSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DirectSalesOrderContentOrder
    {
        Id = 0,
        DirectSalesOrder = 1,
        Item = 2,
        UnitOfMeasure = 3,
        Quantity = 4,
        PrimaryUnitOfMeasure = 5,
        RequestedQuantity = 6,
        PrimaryPrice = 7,
        SalePrice = 8,
        DiscountPercentage = 9,
        DiscountAmount = 10,
        GeneralDiscountPercentage = 11,
        GeneralDiscountAmount = 12,
        Amount = 13,
        TaxPercentage = 14,
        TaxAmount = 15,
    }

    [Flags]
    public enum DirectSalesOrderContentSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        DirectSalesOrder = E._1,
        Item = E._2,
        UnitOfMeasure = E._3,
        Quantity = E._4,
        PrimaryUnitOfMeasure = E._5,
        RequestedQuantity = E._6,
        PrimaryPrice = E._7,
        SalePrice = E._8,
        DiscountPercentage = E._9,
        DiscountAmount = E._10,
        GeneralDiscountPercentage = E._11,
        GeneralDiscountAmount = E._12,
        Amount = E._13,
        TaxPercentage = E._14,
        TaxAmount = E._15,
        Factor = E._16
    }
}
