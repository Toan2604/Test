using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class Album : DataEntity, IEquatable<Album>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<AlbumImageMapping> AlbumImageMappings { get; set; }
        public bool Equals(Album other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class AlbumFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter StoreId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public DateFilter ShootingAt { get; set; }
        public List<AlbumFilter> OrFilter { get; set; }
        public AlbumOrder OrderBy { get; set; }
        public AlbumSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AlbumOrder
    {
        Id = 0,
        Name = 1,
        Status = 2,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum AlbumSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Name = E._1,
        Status = E._2,
        Mapping = E._3,
    }
}
