using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PromotionStoreItemMapping : DataEntity, IEquatable<PromotionStoreItemMapping>
    {
        public long PromotionStoreId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public Item Item { get; set; }
        public PromotionStore PromotionStore { get; set; }

        public bool Equals(PromotionStoreItemMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionStoreItemMappingFilter : FilterEntity
    {
        public IdFilter PromotionStoreId { get; set; }
        public IdFilter ItemId { get; set; }
        public LongFilter Quantity { get; set; }
        public List<PromotionStoreItemMappingFilter> OrFilter { get; set; }
        public PromotionStoreItemMappingOrder OrderBy { get; set; }
        public PromotionStoreItemMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionStoreItemMappingOrder
    {
        PromotionStore = 0,
        Item = 1,
        Quantity = 2,
    }

    [Flags]
    public enum PromotionStoreItemMappingSelect : long
    {
        ALL = E.ALL,
        PromotionStore = E._0,
        Item = E._1,
        Quantity = E._2,
    }
}
