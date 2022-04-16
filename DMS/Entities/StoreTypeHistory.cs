using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using TrueSight.Common;

namespace DMS.Entities
{
    public class StoreTypeHistory : DataEntity
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public DateTime CreatedAt { get; set; }
        public long AppUserId { get; set; }
        public long? PreviousStoreTypeId { get; set; }
        public long StoreTypeId { get; set; }
        public AppUser AppUser { get; set; }
        public Store Store { get; set; }
        public StoreType PreviousStoreType { get; set; }
        public StoreType StoreType { get; set; }

        public bool Equals(StoreTypeHistory other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class StoreTypeHistoryFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter StoreId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter StoreTypeId { get; set; }
        public IdFilter PreviousStoreTypeId { get; set; }
        public StoreTypeHistoryOrder OrderBy { get; set; }
        public StoreTypeHistorySelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StoreTypeHistoryOrder
    {
        Id = 0,
        Store = 1,
        AppUser = 2,
        StoreType = 3,
        PreviousStoreType = 4,
        CreatedAt = 5,
    }

    [Flags]
    public enum StoreTypeHistorySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Store = E._1,
        AppUser = E._3,
        StoreType = E._4,
        PreviousStoreType = E._5,
    }
}
