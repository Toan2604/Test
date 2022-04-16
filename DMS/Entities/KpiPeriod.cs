using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class KpiPeriod : DataEntity, IEquatable<KpiPeriod>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<KpiItem> ItemSpecificKpis { get; set; }

        public bool Equals(KpiPeriod other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class KpiPeriodFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public List<KpiPeriodFilter> OrFilter { get; set; }
        public KpiPeriodOrder OrderBy { get; set; }
        public KpiPeriodSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum KpiPeriodOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
    }

    [Flags]
    public enum KpiPeriodSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
    }
}
