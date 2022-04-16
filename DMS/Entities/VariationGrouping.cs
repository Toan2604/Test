using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class VariationGrouping : DataEntity, IEquatable<VariationGrouping>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ProductId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }
        public Product Product { get; set; }
        public List<Variation> Variations { get; set; }

        public bool Equals(VariationGrouping other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class VariationGroupingFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter ProductId { get; set; }
        public List<VariationGroupingFilter> OrFilter { get; set; }
        public VariationGroupingOrder OrderBy { get; set; }
        public VariationGroupingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum VariationGroupingOrder
    {
        Id = 0,
        Name = 1,
        Product = 2,
    }

    [Flags]
    public enum VariationGroupingSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Name = E._1,
        Product = E._2,
    }
}
