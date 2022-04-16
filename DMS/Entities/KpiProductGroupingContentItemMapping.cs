using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class KpiProductGroupingContentItemMapping : DataEntity, IEquatable<KpiProductGroupingContentItemMapping>
    {
        public long KpiProductGroupingContentId { get; set; }
        public long ItemId { get; set; }
        public Item Item { get; set; }
        public KpiProductGroupingContent KpiProductGroupingContent { get; set; }

        public bool Equals(KpiProductGroupingContentItemMapping other)
        {
            if (other == null) return false;
            if (this.KpiProductGroupingContentId != other.KpiProductGroupingContentId) return false;
            if (this.ItemId != other.ItemId) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class KpiProductGroupingContentItemMappingFilter : FilterEntity
    {
        public IdFilter KpiProductGroupingContentId { get; set; }
        public IdFilter ItemId { get; set; }
        public List<KpiProductGroupingContentItemMappingFilter> OrFilter { get; set; }
        public KpiProductGroupingContentItemMappingOrder OrderBy { get; set; }
        public KpiProductGroupingContentItemMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum KpiProductGroupingContentItemMappingOrder
    {
        KpiProductGroupingContent = 0,
        Item = 1,
    }

    [Flags]
    public enum KpiProductGroupingContentItemMappingSelect : long
    {
        ALL = E.ALL,
        KpiProductGroupingContent = E._0,
        Item = E._1,
    }
}
