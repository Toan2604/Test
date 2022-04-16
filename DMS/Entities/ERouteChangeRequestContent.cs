using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class ERouteChangeRequestContent : DataEntity, IEquatable<ERouteChangeRequestContent>
    {
        public long Id { get; set; }
        public long ERouteChangeRequestId { get; set; }
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
        public ERouteChangeRequest ERouteChangeRequest { get; set; }
        public Store Store { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool Equals(ERouteChangeRequestContent other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ERouteChangeRequestContentFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter ERouteChangeRequestId { get; set; }
        public IdFilter StoreId { get; set; }
        public LongFilter OrderNumber { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<ERouteChangeRequestContentFilter> OrFilter { get; set; }
        public ERouteChangeRequestContentOrder OrderBy { get; set; }
        public ERouteChangeRequestContentSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ERouteChangeRequestContentOrder
    {
        Id = 0,
        ERouteChangeRequest = 1,
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
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum ERouteChangeRequestContentSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        ERouteChangeRequest = E._1,
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
