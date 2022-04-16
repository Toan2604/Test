using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PromotionStoreGroupingItemMapping : DataEntity, IEquatable<PromotionStoreGroupingItemMapping>
    {
        public long PromotionStoreGroupingId { get; set; }
        public long itemId { get; set; }
        public string Quantity { get; set; }
        public PromotionStoreGrouping PromotionStoreGrouping { get; set; }
        public Item item { get; set; }

        public bool Equals(PromotionStoreGroupingItemMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionStoreGroupingItemMappingFilter : FilterEntity
    {
        public IdFilter PromotionStoreGroupingId { get; set; }
        public IdFilter itemId { get; set; }
        public StringFilter Quantity { get; set; }
        public List<PromotionStoreGroupingItemMappingFilter> OrFilter { get; set; }
        public PromotionStoreGroupingItemMappingOrder OrderBy { get; set; }
        public PromotionStoreGroupingItemMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionStoreGroupingItemMappingOrder
    {
        PromotionStoreGrouping = 0,
        item = 1,
        Quantity = 2,
    }

    [Flags]
    public enum PromotionStoreGroupingItemMappingSelect : long
    {
        ALL = E.ALL,
        PromotionStoreGrouping = E._0,
        item = E._1,
        Quantity = E._2,
    }
}
