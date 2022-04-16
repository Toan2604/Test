using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class BrandHistory : DataEntity, IEquatable<BrandHistory>
    {
        public long Id { get; set; }
        public long BrandId { get; set; }
        public long? AppUserId { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? Used { get; set; }
        public Status Status { get; set; }

        public bool Equals(BrandHistory other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class BrandHistoryFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter BrandId { get; set; }
        public IdFilter StatusId { get; set; }
        public List<BrandHistoryFilter> OrFilter { get; set; }
        public BrandHistoryOrder OrderBy { get; set; }
        public BrandHistorySelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BrandHistoryOrder
    {
        Id = 0,
        BrandId = 1,
        Status = 2,
    }

    [Flags]
    public enum BrandHistorySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        BrandId = E._1,
        Status = E._2,
    }
}
