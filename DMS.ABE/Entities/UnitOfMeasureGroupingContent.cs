using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class UnitOfMeasureGroupingContent : DataEntity, IEquatable<UnitOfMeasureGroupingContent>
    {
        public long Id { get; set; }
        public long UnitOfMeasureGroupingId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public long? Factor { get; set; }
        public Guid RowId { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public UnitOfMeasureGrouping UnitOfMeasureGrouping { get; set; }

        public bool Equals(UnitOfMeasureGroupingContent other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class UnitOfMeasureGroupingContentFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter UnitOfMeasureGroupingId { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public LongFilter Factor { get; set; }
        public List<UnitOfMeasureGroupingContentFilter> OrFilter { get; set; }
        public UnitOfMeasureGroupingContentOrder OrderBy { get; set; }
        public UnitOfMeasureGroupingContentSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UnitOfMeasureGroupingContentOrder
    {
        Id = 0,
        UnitOfMeasureGrouping = 1,
        UnitOfMeasure = 2,
        Factor = 3,
    }

    [Flags]
    public enum UnitOfMeasureGroupingContentSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        UnitOfMeasureGrouping = E._1,
        UnitOfMeasure = E._2,
        Factor = E._3,
    }
}
