using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class IndirectSalesOrderPromotion : DataEntity, IEquatable<IndirectSalesOrderPromotion>
    {
        public long Id { get; set; }
        public long IndirectSalesOrderId { get; set; }
        public long ItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal Quantity { get; set; }
        public long PrimaryUnitOfMeasureId { get; set; }
        public decimal RequestedQuantity { get; set; }
        public string Note { get; set; }
        public long? Factor { get; set; }
        public IndirectSalesOrder IndirectSalesOrder { get; set; }
        public Item Item { get; set; }
        public UnitOfMeasure PrimaryUnitOfMeasure { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public bool Equals(IndirectSalesOrderPromotion other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class IndirectSalesOrderPromotionFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter IndirectSalesOrderId { get; set; }
        public IdFilter ProductId { get; set; }
        public IdFilter ItemId { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public DecimalFilter Quantity { get; set; }
        public IdFilter PrimaryUnitOfMeasureId { get; set; }
        public DecimalFilter RequestedQuantity { get; set; }
        public StringFilter Note { get; set; }
        public List<IndirectSalesOrderPromotionFilter> OrFilter { get; set; }
        public IndirectSalesOrderPromotionOrder OrderBy { get; set; }
        public IndirectSalesOrderPromotionSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum IndirectSalesOrderPromotionOrder
    {
        Id = 0,
        IndirectSalesOrder = 1,
        Item = 2,
        UnitOfMeasure = 3,
        Quantity = 4,
        PrimaryUnitOfMeasure = 5,
        RequestedQuantity = 6,
        Note = 7,
    }

    [Flags]
    public enum IndirectSalesOrderPromotionSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        IndirectSalesOrder = E._1,
        Item = E._2,
        UnitOfMeasure = E._3,
        Quantity = E._4,
        PrimaryUnitOfMeasure = E._5,
        RequestedQuantity = E._6,
        Note = E._7,
        Factor = E._8
    }
}
