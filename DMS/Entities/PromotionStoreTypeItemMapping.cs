using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class PromotionStoreTypeItemMapping : DataEntity, IEquatable<PromotionStoreTypeItemMapping>
    {
        public long PromotionStoreTypeId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public Item Item { get; set; }
        public PromotionStoreType PromotionStoreType { get; set; }

        public bool Equals(PromotionStoreTypeItemMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionStoreTypeItemMappingFilter : FilterEntity
    {
        public IdFilter PromotionStoreTypeId { get; set; }
        public IdFilter ItemId { get; set; }
        public LongFilter Quantity { get; set; }
        public List<PromotionStoreTypeItemMappingFilter> OrFilter { get; set; }
        public PromotionStoreTypeItemMappingOrder OrderBy { get; set; }
        public PromotionStoreTypeItemMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionStoreTypeItemMappingOrder
    {
        PromotionStoreType = 0,
        Item = 1,
        Quantity = 2,
    }

    [Flags]
    public enum PromotionStoreTypeItemMappingSelect : long
    {
        ALL = E.ALL,
        PromotionStoreType = E._0,
        Item = E._1,
        Quantity = E._2,
    }
}
