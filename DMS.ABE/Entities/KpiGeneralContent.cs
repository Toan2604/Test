using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class KpiGeneralContent : DataEntity, IEquatable<KpiGeneralContent>
    {
        public long Id { get; set; }
        public long KpiGeneralId { get; set; }
        public long KpiCriteriaGeneralId { get; set; }
        public long StatusId { get; set; }
        public KpiCriteriaGeneral KpiCriteriaGeneral { get; set; }
        public KpiGeneral KpiGeneral { get; set; }
        public Status Status { get; set; }
        public Guid RowId { get; set; }
        public List<KpiGeneralContentKpiPeriodMapping> KpiGeneralContentKpiPeriodMappings { get; set; }


        public bool Equals(KpiGeneralContent other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class KpiGeneralContentFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter KpiGeneralId { get; set; }
        public IdFilter KpiCriteriaGeneralId { get; set; }
        public IdFilter StatusId { get; set; }
        public List<KpiGeneralContentFilter> OrFilter { get; set; }
        public KpiGeneralContentOrder OrderBy { get; set; }
        public KpiGeneralContentSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum KpiGeneralContentOrder
    {
        Id = 0,
        KpiGeneral = 1,
        KpiCriteriaGeneral = 2,
        Status = 3,
    }

    [Flags]
    public enum KpiGeneralContentSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        KpiGeneral = E._1,
        KpiCriteriaGeneral = E._2,
        Status = E._3,
    }
}
