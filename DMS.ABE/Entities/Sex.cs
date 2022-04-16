using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class Sex : DataEntity, IEquatable<Sex>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public bool Equals(Sex other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class SexFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public List<SexFilter> OrFilter { get; set; }
        public SexOrder OrderBy { get; set; }
        public SexSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SexOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Status = 3,
    }

    [Flags]
    public enum SexSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Status = E._3,
    }
}
