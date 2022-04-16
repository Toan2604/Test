using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class Store : DataEntity, IEquatable<Store>
    {
        public long Stt { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public string CodeDraft { get; set; }
        public string Name { get; set; }
        public string UnsignName { get; set; }
        public long? ParentStoreId { get; set; }
        public long OrganizationId { get; set; }
        public long StoreTypeId { get; set; }
        public long? StoreGroupingId { get; set; }
        public string Telephone { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? WardId { get; set; }
        public string Address { get; set; }
        public string UnsignAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal? DeliveryLatitude { get; set; }
        public decimal? DeliveryLongitude { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerEmail { get; set; }
        public string TaxCode { get; set; }
        public string LegalEntity { get; set; }
        public long? AppUserId { get; set; }
        public long CreatorId { get; set; }
        public long StatusId { get; set; }
        public Guid RowId { get; set; }
        public long? StoreUserId { get; set; }
        public long? StoreScoutingId { get; set; }
        public long StoreStatusId { get; set; }
        public bool HasEroute { get; set; }
        public bool HasChecking { get; set; }
        public bool Used { get; set; }
        public string? Description { get; set; }
        public decimal? DebtLimited { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public AppUser AppUser { get; set; }
        public District District { get; set; }
        public Organization Organization { get; set; }
        public Store ParentStore { get; set; }
        public Province Province { get; set; }
        public Status Status { get; set; }
        public StoreGrouping StoreGrouping { get; set; }
        public StoreUser StoreUser { get; set; }
        public AppUser Creator { get; set; }
        public StoreType StoreType { get; set; }
        public Ward Ward { get; set; }
        public StoreStatus StoreStatus { get; set; }
        public StoreScouting StoreScouting { get; set; }
        public List<AlbumImageMapping> AlbumImageMappings { get; set; }
        public List<StoreStoreGroupingMapping> StoreStoreGroupingMappings { get; set; }
        public List<StoreImageMapping> StoreImageMappings { get; set; }
        public List<AppUserStoreMapping> StoreAppUserMappings { get; set; }
        public List<BrandInStore> BrandInStores { get; set; }
        public double Distance { get; set; }
        public bool Equals(Store other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.Code != other.Code) return false;
            if (this.CodeDraft != other.CodeDraft) return false;
            if (this.Name != other.Name) return false;
            if (this.UnsignName != other.UnsignName) return false;
            if (this.ParentStoreId != other.ParentStoreId) return false;
            if (this.OrganizationId != other.OrganizationId) return false;
            if (this.StoreTypeId != other.StoreTypeId) return false;
            if (this.StoreGroupingId != other.StoreGroupingId) return false;
            if (this.Telephone != other.Telephone) return false;
            if (this.ProvinceId != other.ProvinceId) return false;
            if (this.DistrictId != other.DistrictId) return false;
            if (this.WardId != other.WardId) return false;
            if (this.Address != other.Address) return false;
            if (this.UnsignAddress != other.UnsignAddress) return false;
            if (this.DeliveryAddress != other.DeliveryAddress) return false;
            if (this.Latitude != other.Latitude) return false;
            if (this.Longitude != other.Longitude) return false;
            if (this.DeliveryLatitude != other.DeliveryLatitude) return false;
            if (this.DeliveryLongitude != other.DeliveryLongitude) return false;
            if (this.OwnerName != other.OwnerName) return false;
            if (this.OwnerPhone != other.OwnerPhone) return false;
            if (this.OwnerEmail != other.OwnerEmail) return false;
            if (this.TaxCode != other.TaxCode) return false;
            if (this.LegalEntity != other.LegalEntity) return false;
            if (this.AppUserId != other.AppUserId) return false;
            if (this.StatusId != other.StatusId) return false;
            if (this.StoreStatusId != other.StoreStatusId) return false;
            if (this.RowId != other.RowId) return false;
            if (this.Used != other.Used) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class StoreFilter : FilterEntity
    {
        public string Search { get; set; }
        public long TimeZone { get; set; }
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter CodeDraft { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter UnsignName { get; set; }
        public IdFilter ParentStoreId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter StoreTypeId { get; set; }
        public IdFilter StoreGroupingId { get; set; }
        public IdFilter StoreCheckingStatusId { get; set; }
        public IdFilter SalesEmployeeId { get; set; }
        public StringFilter Telephone { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        public IdFilter WardId { get; set; }
        public StringFilter Address { get; set; }
        public StringFilter UnsignAddress { get; set; }
        public StringFilter DeliveryAddress { get; set; }
        public DecimalFilter Latitude { get; set; }
        public DecimalFilter Longitude { get; set; }
        public DecimalFilter DeliveryLatitude { get; set; }
        public DecimalFilter DeliveryLongitude { get; set; }
        public StringFilter OwnerName { get; set; }
        public StringFilter OwnerPhone { get; set; }
        public StringFilter OwnerEmail { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter CreatorId { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter StoreUserStatusId { get; set; }
        public IdFilter WorkflowDefinitionId { get; set; }
        public IdFilter StoreStatusId { get; set; }
        public IdFilter StoreDraftTypeId { get; set; }
        public IdFilter LuckyDrawId { get; set; }
        public List<StoreFilter> OrFilter { get; set; }
        public StoreOrder OrderBy { get; set; }
        public StoreSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StoreOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        ParentStore = 3,
        Organization = 4,
        StoreType = 5,
        StoreGrouping = 6,
        Telephone = 8,
        Province = 9,
        District = 10,
        Ward = 11,
        Address = 12,
        DeliveryAddress = 13,
        Latitude = 14,
        Longitude = 15,
        DeliveryLatitude = 16,
        DeliveryLongitude = 17,
        OwnerName = 18,
        OwnerPhone = 19,
        OwnerEmail = 20,
        Status = 21,
        UnsignName = 23,
        UnsignAddress = 24,
        AppUser = 25,
        StoreStatus = 26,
        CodeDraft = 27,
    }

    [Flags]
    public enum StoreSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        ParentStore = E._3,
        Organization = E._4,
        StoreType = E._5,
        StoreGrouping = E._6,
        Telephone = E._8,
        Province = E._9,
        District = E._10,
        Ward = E._11,
        Address = E._12,
        DeliveryAddress = E._13,
        Latitude = E._14,
        Longitude = E._15,
        DeliveryLatitude = E._16,
        DeliveryLongitude = E._17,
        OwnerName = E._18,
        OwnerPhone = E._19,
        OwnerEmail = E._20,
        Status = E._21,
        StoreImageMappings = E._23,
        TaxCode = E._24,
        LegalEntity = E._25,
        StoreScouting = E._26,
        UnsignName = E._27,
        UnsignAddress = E._28,
        AppUser = E._30,
        StoreStatus = E._31,
        HasChecking = E._32,
        CodeDraft = E._33,
        StoreAppUserMappings = E._34,
        Creator = E._35,
        StoreStoreGroupingMapping = E._36,
        Description = E._37,
        DebtLimited = E._38
    }
}
