using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PromotionComboOutItemMapping : DataEntity, IEquatable<PromotionComboOutItemMapping>
    {
        public long PromotionComboId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public Item Item { get; set; }
        public PromotionCombo PromotionCombo { get; set; }

        public bool Equals(PromotionComboOutItemMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionComboOutItemMappingFilter : FilterEntity
    {
        public IdFilter PromotionComboId { get; set; }
        public IdFilter ItemId { get; set; }
        public LongFilter Quantity { get; set; }
        public List<PromotionComboOutItemMappingFilter> OrFilter { get; set; }
        public PromotionComboOutItemMappingOrder OrderBy { get; set; }
        public PromotionComboOutItemMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionComboOutItemMappingOrder
    {
        PromotionCombo = 0,
        Item = 1,
        Quantity = 2,
    }

    [Flags]
    public enum PromotionComboOutItemMappingSelect : long
    {
        ALL = E.ALL,
        PromotionCombo = E._0,
        Item = E._1,
        Quantity = E._2,
    }
}
