using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class DirectSalesOrderTransaction : DataEntity, IEquatable<DirectSalesOrderTransaction>
    {
        public long Id { get; set; }
        public long DirectSalesOrderId { get; set; }
        public long OrganizationId { get; set; }
        public long BuyerStoreId { get; set; }
        public long SalesEmployeeId { get; set; }
        public long ItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal Quantity { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Revenue { get; set; }
        public long TypeId { get; set; }
        public DateTime OrderDate { get; set; }

        public DirectSalesOrder DirectSalesOrder { get; set; }
        public Item Item { get; set; }
        public Organization Organization { get; set; }
        public TransactionType TransactionType { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public bool Equals(DirectSalesOrderTransaction other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class DirectSalesOrderTransactionFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter DirectSalesOrderId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter ItemId { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public DecimalFilter Quantity { get; set; }
        public DecimalFilter Discount { get; set; }
        public DecimalFilter Revenue { get; set; }
        public IdFilter TypeId { get; set; }
        public List<DirectSalesOrderTransactionFilter> OrFilter { get; set; }
        public DirectSalesOrderTransactionOrder OrderBy { get; set; }
        public DirectSalesOrderTransactionSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DirectSalesOrderTransactionOrder
    {
        Id = 0,
        DirectSalesOrder = 1,
        Organization = 2,
        Item = 3,
        UnitOfMeasure = 4,
        Quantity = 5,
        Discount = 6,
        Revenue = 7,
        Type = 8,
    }

    [Flags]
    public enum DirectSalesOrderTransactionSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        DirectSalesOrder = E._1,
        Organization = E._2,
        Item = E._3,
        UnitOfMeasure = E._4,
        Quantity = E._5,
        Discount = E._6,
        Revenue = E._7,
        Type = E._8,
    }
}
