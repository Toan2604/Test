using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class PriceListItemHistory : DataEntity, IEquatable<PriceListItemHistory>
    {
        public long Id { get; set; }
        public long PriceListId { get; set; }
        public long ItemId { get; set; }
        public long ModifierId { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public DateTime UpdatedAt { get; set; }
        public AppUser Modifier { get; set; }
        public Item Item { get; set; }
        public PriceList PriceList { get; set; }
        public bool Equals(PriceListItemHistory other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
    public class PriceListItemHistoryFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter PriceListId { get; set; }
        public IdFilter ItemId { get; set; }
        public IdFilter ModifierId { get; set; }
        public DecimalFilter OldPrice { get; set; }
        public DecimalFilter NewPrice { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<PriceListItemHistoryFilter> OrFilter { get; set; }
        public PriceListItemHistoryOrder OrderBy { get; set; }
        public PriceListItemHistorySelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PriceListItemHistoryOrder
    {
        Id = 0,
        PriceList = 1,
        Item = 2,
        UpdatedAt = 3,
        Modifier = 4,
        OldPrice = 5,
        NewPrice = 6,
    }

    [Flags]
    public enum PriceListItemHistorySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        PriceList = E._1,
        Item = E._2,
        UpdatedAt = E._3,
        Modifier = E._4,
        OldPrice = E._5,
        NewPrice = E._6,
    }

}
