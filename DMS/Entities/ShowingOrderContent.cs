using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class ShowingOrderContent : DataEntity, IEquatable<ShowingOrderContent>
    {
        public long Id { get; set; }
        public long ShowingOrderId { get; set; }
        public long ShowingItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal SalePrice { get; set; }
        public long Quantity { get; set; }
        public decimal Amount { get; set; }
        public ShowingItem ShowingItem { get; set; }
        public ShowingOrder ShowingOrder { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public bool Equals(ShowingOrderContent other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.ShowingOrderId != other.ShowingOrderId) return false;
            if (this.ShowingItemId != other.ShowingItemId) return false;
            if (this.UnitOfMeasureId != other.UnitOfMeasureId) return false;
            if (this.SalePrice != other.SalePrice) return false;
            if (this.Quantity != other.Quantity) return false;
            if (this.Amount != other.Amount) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class ShowingOrderContentFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter ShowingOrderId { get; set; }
        public IdFilter ShowingItemId { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public DecimalFilter SalePrice { get; set; }
        public LongFilter Quantity { get; set; }
        public DecimalFilter Amount { get; set; }
        public List<ShowingOrderContentFilter> OrFilter { get; set; }
        public ShowingOrderContentOrder OrderBy { get; set; }
        public ShowingOrderContentSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShowingOrderContentOrder
    {
        Id = 0,
        ShowingOrder = 1,
        ShowingItem = 2,
        UnitOfMeasure = 3,
        SalePrice = 4,
        Quantity = 5,
        Amount = 6,
    }

    [Flags]
    public enum ShowingOrderContentSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        ShowingOrder = E._1,
        ShowingItem = E._2,
        UnitOfMeasure = E._3,
        SalePrice = E._4,
        Quantity = E._5,
        Amount = E._6,
    }
}
