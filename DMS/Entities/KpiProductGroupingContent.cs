using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class KpiProductGroupingContent : DataEntity, IEquatable<KpiProductGroupingContent>
    {
        public long Id { get; set; }
        public long KpiProductGroupingId { get; set; }
        public long ProductGroupingId { get; set; }
        public Guid RowId { get; set; }
        public bool SelectAllCurrentItem { get; set; }
        public KpiProductGrouping KpiProductGrouping { get; set; }
        public ProductGrouping ProductGrouping { get; set; }

        public List<KpiProductGroupingContentCriteriaMapping> KpiProductGroupingContentCriteriaMappings { get; set; }
        public List<KpiProductGroupingContentItemMapping> KpiProductGroupingContentItemMappings { get; set; }

        public bool Equals(KpiProductGroupingContent other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.KpiProductGroupingId != other.KpiProductGroupingId) return false;
            if (this.ProductGroupingId != other.ProductGroupingId) return false;
            if (this.RowId != other.RowId) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class KpiProductGroupingContentFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter KpiProductGroupingId { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public bool SelectAllCurrentItem { get; set; }
        public GuidFilter RowId { get; set; }
        public List<KpiProductGroupingContentFilter> OrFilter { get; set; }
        public KpiProductGroupingContentOrder OrderBy { get; set; }
        public KpiProductGroupingContentSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum KpiProductGroupingContentOrder
    {
        Id = 0,
        KpiProductGrouping = 1,
        ProductGrouping = 2,
        Row = 3,
        SelectAllCurrentItem = 4,
    }

    [Flags]
    public enum KpiProductGroupingContentSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        KpiProductGrouping = E._1,
        ProductGrouping = E._2,
        Row = E._3,
        SelectAllCurrentItem = E._4,
    }
}
