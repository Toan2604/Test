using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class StoreHistory : DataEntity, IEquatable<StoreHistory>
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long AppUserId { get; set; }
        public long StatusId { get; set; }
        public long StoreStatusId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public long? EstimatedRevenueId { get; set; }
        public DateTime CreatedAt { get; set; }

        public StoreHistory() { }
        public StoreHistory(Store Store)
        {
            this.StoreId = Store.Id;
            this.Code = Store.Code;
            this.Name = Store.Name;
            this.StatusId = Store.StatusId;
            this.StoreStatusId = Store.StoreStatusId;
            this.EstimatedRevenueId = Store.EstimatedRevenueId;
            this.Latitude = Store.Latitude;
            this.Longitude = Store.Longitude;
        }

        public bool Equals(StoreHistory other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.Code != other.Code) return false;
            if (this.Name != other.Name) return false;
            if (this.StatusId != other.StatusId) return false;
            if (this.Latitude != other.Latitude) return false;
            if (this.Longitude != other.Longitude) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class StoreHistoryFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter StoreId { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter StoreStatusId { get; set; }
        public DecimalFilter Latitude { get; set; }
        public DecimalFilter Longitude { get; set; }
        public DateFilter CreatedAt { get; set; }
        public IdFilter EstimatedRevenueId { get; set; }
        public List<StoreHistoryFilter> OrFilter { get; set; }
        public IdFilter IsStoreApprovalDirectSalesOrder { get; set; }
        public StoreHistoryOrder OrderBy { get; set; }
        public StoreHistorySelect Selects { get; set; }

    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StoreHistoryOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        StoreId = 3,
        Status = 21,
        AppUser = 25,
        StoreStatus = 26,
        EstimatedRevenue = 31,
    }

    [Flags]
    public enum StoreHistorySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        StoreId = E._3,
        Status = E._4,
        StoreStatus = E._5,
        StoreStatusId = E._6,
        CreatedAt = E._7,
        EstimatedRevenue = E._8,
        Latitude = E._9,
        Longitude = E._10,
    }
}
