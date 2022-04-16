using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class Ward : DataEntity, IEquatable<Ward>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? Priority { get; set; }
        public long DistrictId { get; set; }
        public long StatusId { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public District District { get; set; }
        public Status Status { get; set; }

        public bool Equals(Ward other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class WardFilter : FilterEntity
    {
        public string Search { get; set; }
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public LongFilter Priority { get; set; }
        public IdFilter DistrictId { get; set; }
        public IdFilter StatusId { get; set; }
        public List<WardFilter> OrFilter { get; set; }
        public WardOrder OrderBy { get; set; }
        public WardSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WardOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Priority = 3,
        District = 4,
        Status = 5,
    }

    [Flags]
    public enum WardSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Priority = E._3,
        District = E._4,
        Status = E._5,
        RowId = E._6
    }
}
