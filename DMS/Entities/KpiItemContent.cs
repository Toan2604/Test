using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class KpiItemContent : DataEntity, IEquatable<KpiItemContent>
    {
        public long Id { get; set; }
        public long KpiItemId { get; set; }
        public long ItemId { get; set; }
        public Guid RowId { get; set; }
        public Item Item { get; set; }
        public KpiItem KpiItem { get; set; }
        public List<KpiItemContentKpiCriteriaItemMapping> KpiItemContentKpiCriteriaItemMappings { get; set; }
        public bool Equals(KpiItemContent other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class KpiItemContentFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter KpiItemId { get; set; }
        public IdFilter ItemId { get; set; }
        public List<KpiItemContentFilter> OrFilter { get; set; }
        public KpiItemContentOrder OrderBy { get; set; }
        public KpiItemContentSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum KpiItemContentOrder
    {
        Id = 0,
        KpiItem = 1,
        Item = 2,
    }

    [Flags]
    public enum KpiItemContentSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        KpiItem = E._1,
        Item = E._2,
    }
}
