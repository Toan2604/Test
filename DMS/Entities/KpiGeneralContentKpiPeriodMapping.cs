using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class KpiGeneralContentKpiPeriodMapping : DataEntity, IEquatable<KpiGeneralContentKpiPeriodMapping>
    {
        public long KpiGeneralContentId { get; set; }
        public long KpiPeriodId { get; set; }
        public decimal? Value { get; set; }
        public KpiGeneralContent KpiGeneralContent { get; set; }
        public KpiPeriod KpiPeriod { get; set; }

        public bool Equals(KpiGeneralContentKpiPeriodMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class KpiGeneralContentKpiPeriodMappingFilter : FilterEntity
    {
        public IdFilter KpiGeneralContentId { get; set; }
        public IdFilter KpiPeriodId { get; set; }
        public DecimalFilter Value { get; set; }
        public List<KpiGeneralContentKpiPeriodMappingFilter> OrFilter { get; set; }
        public KpiGeneralContentKpiPeriodMappingOrder OrderBy { get; set; }
        public KpiGeneralContentKpiPeriodMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum KpiGeneralContentKpiPeriodMappingOrder
    {
        KpiGeneralContent = 0,
        KpiPeriod = 1,
        Value = 2,
    }

    [Flags]
    public enum KpiGeneralContentKpiPeriodMappingSelect : long
    {
        ALL = E.ALL,
        KpiGeneralContent = E._0,
        KpiPeriod = E._1,
        Value = E._2,
    }
}
