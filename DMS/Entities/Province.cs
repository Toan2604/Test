using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class Province : DataEntity, IEquatable<Province>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? Priority { get; set; }
        public long StatusId { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Status Status { get; set; }
        public List<District> Districts { get; set; }
        public bool Equals(Province other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ProvinceFilter : FilterEntity
    {
        public String Search { get; set; }
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public LongFilter Priority { get; set; }
        public IdFilter StatusId { get; set; }
        public List<ProvinceFilter> OrFilter { get; set; }
        public ProvinceOrder OrderBy { get; set; }
        public ProvinceSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProvinceOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Priority = 3,
        Status = 4,
    }

    [Flags]
    public enum ProvinceSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Priority = E._3,
        Status = E._4,
        Districts = E._5,
        RowId = E._6
    }
}
