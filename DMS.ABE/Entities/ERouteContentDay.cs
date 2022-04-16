using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace DMS.ABE.Entities
{
    public class ERouteContentDay : DataEntity, IEquatable<ERouteContentDay>
    {
        public long Id { get; set; }
        public long ERouteContentId { get; set; }
        public long OrderDay { get; set; }
        public bool Planned { get; set; }
        public bool Equals(ERouteContentDay other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ERouteContentDayFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter ERouteContentId { get; set; }
        public LongFilter OrderDay { get; set; }
        public bool? Planned { get; set; }
        public ERouteContentDayOrder OrderBy { get; set; }
        public ERouteContentDaySelect Select { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ERouteContentDayOrder
    {
        Id = 0,
        ERouteContent = 1,
        OrderDay = 2,
        Planned = 3,
    }

    [Flags]
    public enum ERouteContentDaySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        ERouteContent = E._1,
        OrderDay = E._2,
        Planned = E._3,
    }
}
