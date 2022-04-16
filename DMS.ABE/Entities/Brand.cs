using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class Brand : DataEntity, IEquatable<Brand>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }
        public DateTime UpdateTime { get; set; }
        public Status Status { get; set; }

        public bool Equals(Brand other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class BrandFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Description { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter UpdateTime { get; set; }
        public List<BrandFilter> OrFilter { get; set; }
        public BrandOrder OrderBy { get; set; }
        public BrandSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BrandOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Description = 3,
        Status = 4,
        UpdateTime = 5
    }

    [Flags]
    public enum BrandSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Description = E._3,
        Status = E._4,
        UpdateTime = E._5
    }
}
