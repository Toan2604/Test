using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class DirectSalesOrderPromotion : DataEntity, IEquatable<DirectSalesOrderPromotion>
    {
        public long Id { get; set; }
        public long DirectSalesOrderId { get; set; }
        public long ItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal Quantity { get; set; }
        public long PrimaryUnitOfMeasureId { get; set; }
        public decimal RequestedQuantity { get; set; }
        public string Note { get; set; }
        public long? Factor { get; set; }
        public long? InventoryCheckStateId { get; set; }
        public DirectSalesOrder DirectSalesOrder { get; set; }
        public Item Item { get; set; }
        public UnitOfMeasure PrimaryUnitOfMeasure { get; set; }
        public CheckState InventoryCheckState { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public bool Equals(DirectSalesOrderPromotion other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class DirectSalesOrderPromotionFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter DirectSalesOrderId { get; set; }
        public IdFilter ProductId { get; set; }
        public IdFilter ItemId { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public DecimalFilter Quantity { get; set; }
        public IdFilter PrimaryUnitOfMeasureId { get; set; }
        public DecimalFilter RequestedQuantity { get; set; }
        public StringFilter Note { get; set; }
        public List<DirectSalesOrderPromotionFilter> OrFilter { get; set; }
        public DirectSalesOrderPromotionOrder OrderBy { get; set; }
        public DirectSalesOrderPromotionSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DirectSalesOrderPromotionOrder
    {
        Id = 0,
        DirectSalesOrder = 1,
        Item = 2,
        UnitOfMeasure = 3,
        Quantity = 4,
        PrimaryUnitOfMeasure = 5,
        RequestedQuantity = 6,
        Note = 7,
    }

    [Flags]
    public enum DirectSalesOrderPromotionSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        DirectSalesOrder = E._1,
        Item = E._2,
        UnitOfMeasure = E._3,
        Quantity = E._4,
        PrimaryUnitOfMeasure = E._5,
        RequestedQuantity = E._6,
        Note = E._7,
        Factor = E._8
    }
}
