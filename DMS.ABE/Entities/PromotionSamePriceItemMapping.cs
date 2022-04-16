using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PromotionSamePriceItemMapping : DataEntity, IEquatable<PromotionSamePriceItemMapping>
    {
        public long PromotionSamePriceId { get; set; }
        public long ItemId { get; set; }
        public string Quantity { get; set; }
        public Item Item { get; set; }
        public PromotionSamePrice PromotionSamePrice { get; set; }

        public bool Equals(PromotionSamePriceItemMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionSamePriceItemMappingFilter : FilterEntity
    {
        public IdFilter PromotionSamePriceId { get; set; }
        public IdFilter ItemId { get; set; }
        public StringFilter Quantity { get; set; }
        public List<PromotionSamePriceItemMappingFilter> OrFilter { get; set; }
        public PromotionSamePriceItemMappingOrder OrderBy { get; set; }
        public PromotionSamePriceItemMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionSamePriceItemMappingOrder
    {
        PromotionSamePrice = 0,
        Item = 1,
        Quantity = 2,
    }

    [Flags]
    public enum PromotionSamePriceItemMappingSelect : long
    {
        ALL = E.ALL,
        PromotionSamePrice = E._0,
        Item = E._1,
        Quantity = E._2,
    }
}
