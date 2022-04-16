using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class StoreScouting : DataEntity, IEquatable<StoreScouting>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string OwnerPhone { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? WardId { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public long CreatorId { get; set; }
        public long OrganizationId { get; set; }
        public long StoreScoutingStatusId { get; set; }
        public long StoreScoutingTypeId { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public long? StoreId { get; set; }
        public Guid RowId { get; set; }
        public AppUser Creator { get; set; }
        public Organization Organization { get; set; }
        public District District { get; set; }
        public Province Province { get; set; }
        public StoreScoutingStatus StoreScoutingStatus { get; set; }
        public StoreScoutingType StoreScoutingType { get; set; }
        public Ward Ward { get; set; }
        public Store Store { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<StoreScoutingImageMapping> StoreScoutingImageMappings { get; set; }
        public bool Equals(StoreScouting other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class StoreScoutingFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter OwnerPhone { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        public IdFilter WardId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public StringFilter Address { get; set; }
        public DecimalFilter Latitude { get; set; }
        public DecimalFilter Longitude { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter StoreScoutingStatusId { get; set; }
        public IdFilter StoreScoutingTypeId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<StoreScoutingFilter> OrFilter { get; set; }
        public StoreScoutingOrder OrderBy { get; set; }
        public StoreScoutingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StoreScoutingOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        OwnerPhone = 3,
        Province = 4,
        District = 5,
        Ward = 6,
        Address = 8,
        Latitude = 9,
        Longitude = 10,
        Store = 11,
        Creator = 12,
        StoreScoutingStatus = 13,
        Organization = 14,
        StoreScoutingType = 15,
        CreatedAt = 50,
        UpdatedAt = 51,

    }

    [Flags]
    public enum StoreScoutingSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        OwnerPhone = E._3,
        Province = E._4,
        District = E._5,
        Ward = E._6,
        Address = E._8,
        Latitude = E._9,
        Longitude = E._10,
        Store = E._11,
        Creator = E._12,
        StoreScoutingStatus = E._13,
        Organization = E._14,
        StoreScoutingType = E._15,
        Description = E._16,
    }
}
