using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PromotionProductItemMapping : DataEntity, IEquatable<PromotionProductItemMapping>
    {
        public long PromotionProductId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public Item Item { get; set; }
        public PromotionProduct PromotionProduct { get; set; }

        public bool Equals(PromotionProductItemMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionProductItemMappingFilter : FilterEntity
    {
        public IdFilter PromotionProductId { get; set; }
        public IdFilter ItemId { get; set; }
        public LongFilter Quantity { get; set; }
        public List<PromotionProductItemMappingFilter> OrFilter { get; set; }
        public PromotionProductItemMappingOrder OrderBy { get; set; }
        public PromotionProductItemMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionProductItemMappingOrder
    {
        PromotionProduct = 0,
        Item = 1,
        Quantity = 2,
    }

    [Flags]
    public enum PromotionProductItemMappingSelect : long
    {
        ALL = E.ALL,
        PromotionProduct = E._0,
        Item = E._1,
        Quantity = E._2,
    }
}
