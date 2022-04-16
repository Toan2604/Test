using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace DMS.ABE.Entities
{
    public class StoreStatusHistory : DataEntity
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public DateTime CreatedAt { get; set; }
        public long AppUserId { get; set; }
        public long? PreviousStoreStatusId { get; set; }
        public long StoreStatusId { get; set; }
        public AppUser AppUser { get; set; }
        public Store Store { get; set; }
        public StoreStatus PreviousStoreStatus { get; set; }
        public StoreStatus StoreStatus { get; set; }

        public bool Equals(StoreStatusHistory other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class StoreStatusHistoryFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter StoreId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter StoreStatusId { get; set; }
        public IdFilter PreviousStoreStatusId { get; set; }
        public StoreStatusHistoryOrder OrderBy { get; set; }
        public StoreStatusHistorySelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StoreStatusHistoryOrder
    {
        Id = 0,
        Store = 1,
        AppUser = 3,
        StoreStatus = 4,
        PreviousStoreStatus = 5,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum StoreStatusHistorySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Store = E._1,
        AppUser = E._3,
        StoreStatus = E._4,
        PreviousStoreStatus = E._5,
    }
}
