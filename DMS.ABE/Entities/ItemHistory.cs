using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class ItemHistory : DataEntity, IEquatable<ItemHistory>
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public DateTime Time { get; set; }
        public long ModifierId { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public Item Item { get; set; }
        public AppUser Modifier { get; set; }

        public bool Equals(ItemHistory other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ItemHistoryFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter ItemId { get; set; }
        public DateFilter Time { get; set; }
        public IdFilter ModifierId { get; set; }
        public LongFilter OldPrice { get; set; }
        public LongFilter NewPrice { get; set; }
        public List<ItemHistoryFilter> OrFilter { get; set; }
        public ItemHistoryOrder OrderBy { get; set; }
        public ItemHistorySelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ItemHistoryOrder
    {
        Id = 0,
        Item = 1,
        Time = 2,
        Modifier = 3,
        OldPrice = 4,
        NewPrice = 5,
    }

    [Flags]
    public enum ItemHistorySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Item = E._1,
        Time = E._2,
        Modifier = E._3,
        OldPrice = E._4,
        NewPrice = E._5,
    }
}
