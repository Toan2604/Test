using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class StoreType : DataEntity, IEquatable<StoreType>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? ColorId { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }
        public Color Color { get; set; }
        public Status Status { get; set; }

        public bool Equals(StoreType other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class StoreTypeFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter ColorId { get; set; }
        public IdFilter StatusId { get; set; }
        public List<StoreTypeFilter> OrFilter { get; set; }
        public StoreTypeOrder OrderBy { get; set; }
        public StoreTypeSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StoreTypeOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Status = 3,
        Color = 4,
    }

    [Flags]
    public enum StoreTypeSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Status = E._3,
        Color = E._4,
    }
}
