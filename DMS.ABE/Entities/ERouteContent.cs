using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class ERouteContent : DataEntity, IEquatable<ERouteContent>
    {
        public long Id { get; set; }
        public long ERouteId { get; set; }
        public long StoreId { get; set; }
        public long? OrderNumber { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public bool Week1 { get; set; }
        public bool Week2 { get; set; }
        public bool Week3 { get; set; }
        public bool Week4 { get; set; }
        public Guid RowId { get; set; }
        public ERoute ERoute { get; set; }
        public Store Store { get; set; }
        public List<ERouteContentDay> ERouteContentDays { get; set; }
        public bool Equals(ERouteContent other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ERouteContentFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter ERouteId { get; set; }
        public IdFilter StoreId { get; set; }
        public LongFilter OrderNumber { get; set; }
        public List<ERouteContentFilter> OrFilter { get; set; }
        public ERouteContentOrder OrderBy { get; set; }
        public ERouteContentSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ERouteContentOrder
    {
        Id = 0,
        ERoute = 1,
        Store = 2,
        OrderNumber = 3,
        Monday = 4,
        Tuesday = 5,
        Wednesday = 6,
        Thursday = 7,
        Friday = 8,
        Saturday = 9,
        Sunday = 10,
        Week1 = 11,
        Week2 = 12,
        Week3 = 13,
        Week4 = 14,
    }

    [Flags]
    public enum ERouteContentSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        ERoute = E._1,
        Store = E._2,
        OrderNumber = E._3,
        Monday = E._4,
        Tuesday = E._5,
        Wednesday = E._6,
        Thursday = E._7,
        Friday = E._8,
        Saturday = E._9,
        Sunday = E._10,
        Week1 = E._11,
        Week2 = E._12,
        Week3 = E._13,
        Week4 = E._14,
    }
}
