using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class KpiCriteriaGeneral : DataEntity, IEquatable<KpiCriteriaGeneral>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public bool Equals(KpiCriteriaGeneral other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class KpiCriteriaGeneralFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public List<KpiCriteriaGeneralFilter> OrFilter { get; set; }
        public KpiCriteriaGeneralOrder OrderBy { get; set; }
        public KpiCriteriaGeneralSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum KpiCriteriaGeneralOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
    }

    [Flags]
    public enum KpiCriteriaGeneralSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
    }
}
