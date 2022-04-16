using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class StoreChecking : DataEntity, IEquatable<StoreChecking>
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public long SaleEmployeeId { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? CheckOutLongitude { get; set; }
        public decimal? CheckOutLatitude { get; set; }
        public DateTime? CheckInAt { get; set; }
        public DateTime? CheckOutAt { get; set; }
        public long? CheckInDistance { get; set; }
        public long? CheckOutDistance { get; set; }
        public long? CountIndirectSalesOrder { get; set; }
        public long? ImageCounter { get; set; }
        public bool Planned { get; set; }
        public bool IsOpenedStore { get; set; }
        public long OrganizationId { get; set; }
        public string DeviceName { get; set; }
        public Store Store { get; set; }
        public Organization Organization { get; set; }
        public AppUser SaleEmployee { get; set; }
        public List<StoreCheckingImageMapping> StoreCheckingImageMappings { get; set; }

        public bool Equals(StoreChecking other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class StoreCheckingFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter SaleEmployeeId { get; set; }
        public DecimalFilter Longitude { get; set; }
        public DecimalFilter Latitude { get; set; }
        public DateFilter CheckInAt { get; set; }
        public DateFilter CheckOutAt { get; set; }
        public LongFilter CountIndirectSalesOrder { get; set; }
        public LongFilter CountImage { get; set; }
        public List<StoreCheckingFilter> OrFilter { get; set; }
        public StoreCheckingOrder OrderBy { get; set; }
        public StoreCheckingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StoreCheckingOrder
    {
        Id = 0,
        Store = 1,
        SaleEmployee = 2,
        Longitude = 3,
        Latitude = 4,
        CheckInAt = 5,
        CheckOutAt = 6,
        CountIndirectSalesOrder = 7,
        CountImage = 8,
        Organization = 9,
    }

    [Flags]
    public enum StoreCheckingSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Store = E._1,
        SaleEmployee = E._2,
        Longitude = E._3,
        Latitude = E._4,
        CheckInAt = E._5,
        CheckOutAt = E._6,
        CountIndirectSalesOrder = E._7,
        CountImage = E._8,
        CheckOutLongitude = E._9,
        CheckOutLatitude = E._10,
        CheckInDistance = E._11,
        CheckOutDistance = E._12,
        Organization = E._13,
    }
}
