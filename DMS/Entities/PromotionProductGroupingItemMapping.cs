using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class PromotionProductGroupingItemMapping : DataEntity, IEquatable<PromotionProductGroupingItemMapping>
    {
        public long PromotionProductGroupingId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public Item Item { get; set; }
        public PromotionProductGrouping PromotionProductGrouping { get; set; }

        public bool Equals(PromotionProductGroupingItemMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionProductGroupingItemMappingFilter : FilterEntity
    {
        public IdFilter PromotionProductGroupingId { get; set; }
        public IdFilter ItemId { get; set; }
        public LongFilter Quantity { get; set; }
        public List<PromotionProductGroupingItemMappingFilter> OrFilter { get; set; }
        public PromotionProductGroupingItemMappingOrder OrderBy { get; set; }
        public PromotionProductGroupingItemMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionProductGroupingItemMappingOrder
    {
        PromotionProductGrouping = 0,
        Item = 1,
        Quantity = 2,
    }

    [Flags]
    public enum PromotionProductGroupingItemMappingSelect : long
    {
        ALL = E.ALL,
        PromotionProductGrouping = E._0,
        Item = E._1,
        Quantity = E._2,
    }
}
