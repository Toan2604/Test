using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PromotionComboInItemMapping : DataEntity, IEquatable<PromotionComboInItemMapping>
    {
        public long PromotionComboId { get; set; }
        public long ItemId { get; set; }
        public long From { get; set; }
        public long? To { get; set; }
        public Item Item { get; set; }
        public PromotionCombo PromotionCombo { get; set; }

        public bool Equals(PromotionComboInItemMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionComboInItemMappingFilter : FilterEntity
    {
        public IdFilter PromotionComboId { get; set; }
        public IdFilter ItemId { get; set; }
        public LongFilter From { get; set; }
        public LongFilter To { get; set; }
        public List<PromotionComboInItemMappingFilter> OrFilter { get; set; }
        public PromotionComboInItemMappingOrder OrderBy { get; set; }
        public PromotionComboInItemMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionComboInItemMappingOrder
    {
        PromotionCombo = 0,
        Item = 1,
        From = 2,
        To = 3,
    }

    [Flags]
    public enum PromotionComboInItemMappingSelect : long
    {
        ALL = E.ALL,
        PromotionCombo = E._0,
        Item = E._1,
        From = E._2,
        To = E._3,
    }
}
