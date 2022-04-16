using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class IndirectSalesOrderTransaction : DataEntity, IEquatable<IndirectSalesOrderTransaction>
    {
        public long Id { get; set; }
        public long IndirectSalesOrderId { get; set; }
        public long OrganizationId { get; set; }
        public long ItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal Quantity { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Revenue { get; set; }
        public long TypeId { get; set; }
        public IndirectSalesOrder IndirectSalesOrder { get; set; }
        public Item Item { get; set; }
        public Organization Organization { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public bool Equals(IndirectSalesOrderTransaction other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class IndirectSalesOrderTransactionFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter IndirectSalesOrderId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter ItemId { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public DecimalFilter Quantity { get; set; }
        public DecimalFilter Discount { get; set; }
        public DecimalFilter Revenue { get; set; }
        public IdFilter TypeId { get; set; }
        public List<IndirectSalesOrderTransactionFilter> OrFilter { get; set; }
        public IndirectSalesOrderTransactionOrder OrderBy { get; set; }
        public IndirectSalesOrderTransactionSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum IndirectSalesOrderTransactionOrder
    {
        Id = 0,
        IndirectSalesOrder = 1,
        Organization = 2,
        Item = 3,
        UnitOfMeasure = 4,
        Quantity = 5,
        Discount = 6,
        Revenue = 7,
        Type = 8,
    }

    [Flags]
    public enum IndirectSalesOrderTransactionSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        IndirectSalesOrder = E._1,
        Organization = E._2,
        Item = E._3,
        UnitOfMeasure = E._4,
        Quantity = E._5,
        Discount = E._6,
        Revenue = E._7,
        Type = E._8,
    }
}
