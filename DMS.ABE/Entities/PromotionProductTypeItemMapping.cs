using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PromotionProductTypeItemMapping : DataEntity, IEquatable<PromotionProductTypeItemMapping>
    {
        public long PromotionProductTypeId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public Item Item { get; set; }
        public PromotionProductType PromotionProductType { get; set; }

        public bool Equals(PromotionProductTypeItemMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionProductTypeItemMappingFilter : FilterEntity
    {
        public IdFilter PromotionProductTypeId { get; set; }
        public IdFilter ItemId { get; set; }
        public LongFilter Quantity { get; set; }
        public List<PromotionProductTypeItemMappingFilter> OrFilter { get; set; }
        public PromotionProductTypeItemMappingOrder OrderBy { get; set; }
        public PromotionProductTypeItemMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionProductTypeItemMappingOrder
    {
        PromotionProductType = 0,
        Item = 1,
        Quantity = 2,
    }

    [Flags]
    public enum PromotionProductTypeItemMappingSelect : long
    {
        ALL = E.ALL,
        PromotionProductType = E._0,
        Item = E._1,
        Quantity = E._2,
    }
}
