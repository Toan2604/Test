using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class District : DataEntity, IEquatable<District>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? Priority { get; set; }
        public long ProvinceId { get; set; }
        public long StatusId { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Province Province { get; set; }
        public Status Status { get; set; }
        public List<Ward> Wards { get; set; }

        public bool Equals(District other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class DistrictFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public LongFilter Priority { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter StatusId { get; set; }
        public List<DistrictFilter> OrFilter { get; set; }
        public DistrictOrder OrderBy { get; set; }
        public DistrictSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DistrictOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Priority = 3,
        Province = 4,
        Status = 5,
    }

    [Flags]
    public enum DistrictSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Priority = E._3,
        Province = E._4,
        Status = E._5,
        RowId = E._6,
    }
}
