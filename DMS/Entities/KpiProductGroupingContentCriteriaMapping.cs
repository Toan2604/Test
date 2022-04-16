using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class KpiProductGroupingContentCriteriaMapping : DataEntity, IEquatable<KpiProductGroupingContentCriteriaMapping>
    {
        public long KpiProductGroupingContentId { get; set; }
        public long KpiProductGroupingCriteriaId { get; set; }
        public long? Value { get; set; }
        public KpiProductGroupingContent KpiProductGroupingContent { get; set; }
        public KpiProductGroupingCriteria KpiProductGroupingCriteria { get; set; }

        public bool Equals(KpiProductGroupingContentCriteriaMapping other)
        {
            if (other == null) return false;
            if (this.KpiProductGroupingContentId != other.KpiProductGroupingContentId) return false;
            if (this.KpiProductGroupingCriteriaId != other.KpiProductGroupingCriteriaId) return false;
            if (this.Value != other.Value) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class KpiProductGroupingContentCriteriaMappingFilter : FilterEntity
    {
        public IdFilter KpiProductGroupingContentId { get; set; }
        public IdFilter KpiProductGroupingCriteriaId { get; set; }
        public LongFilter Value { get; set; }
        public List<KpiProductGroupingContentCriteriaMappingFilter> OrFilter { get; set; }
        public KpiProductGroupingContentCriteriaMappingOrder OrderBy { get; set; }
        public KpiProductGroupingContentCriteriaMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum KpiProductGroupingContentCriteriaMappingOrder
    {
        KpiProductGroupingContent = 0,
        KpiProductGroupingCriteria = 1,
        Value = 2,
    }

    [Flags]
    public enum KpiProductGroupingContentCriteriaMappingSelect : long
    {
        ALL = E.ALL,
        KpiProductGroupingContent = E._0,
        KpiProductGroupingCriteria = E._1,
        Value = E._2,
    }
}
