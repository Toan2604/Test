using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.store
{
    public class Store_StoreExportDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public string CodeDraft { get; set; }
        public string Name { get; set; }
        public long? ParentStoreId { get; set; }
        public long OrganizationId { get; set; }
        public long StoreTypeId { get; set; }
        public string Telephone { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? WardId { get; set; }
        public string Address { get; set; }
        public string DeliveryAddress { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string DeliveryLatitude { get; set; }
        public string DeliveryLongitude { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerEmail { get; set; }
        public string TaxCode { get; set; }
        public string LegalEntity { get; set; }
        public long StatusId { get; set; }
        public bool HasEroute { get; set; }
        public bool HasChecking { get; set; }
        public bool Used { get; set; }
        public long? AppUserId { get; set; }
        public long CreatorId { get; set; }
        public long? StoreScoutingId { get; set; }
        public long StoreStatusId { get; set; }
        public string Description { get; set; }
        public decimal? DebtLimited { get; set; }
        public Store_AppUserDTO AppUser { get; set; }
        public Store_AppUserDTO Creator { get; set; }
        public Store_DistrictDTO District { get; set; }
        public Store_OrganizationDTO Organization { get; set; }
        public Store_StoreDTO ParentStore { get; set; }
        public Store_ProvinceDTO Province { get; set; }
        public Store_StatusDTO Status { get; set; }
        public Store_StoreGroupingDTO StoreGrouping { get; set; }
        public Store_StoreTypeDTO StoreType { get; set; }
        public Store_WardDTO Ward { get; set; }
        public Store_StoreScoutingDTO StoreScouting { get; set; }
        public Store_StoreStatusDTO StoreStatus { get; set; }
        public List<Store_BrandInStoreDTO> BrandInStores { get; set; }
        public string StoreGroupingName { get; set; }
        public string AppUserName { get; set; }
        public List<Store_StoreStoreGroupingMappingDTO> StoreStoreGroupingMappings { get; set; }
        public List<Store_AppUserStoreMappingDTO> AppUserStoreMappings { get; set; }
        public Store_BrandInStoreDTO BrandInStoreTop1 { get; set; }
        public Store_BrandInStoreDTO BrandInStoreTop2 { get; set; }
        public Store_BrandInStoreDTO BrandInStoreTop3 { get; set; }
        public Store_BrandInStoreDTO BrandInStoreTop4 { get; set; }
        public Store_BrandInStoreDTO BrandInStoreTop5 { get; set; }
        public Store_StoreExportDTO() { }
        public DateTime CreatedAt { get; set; }
        public string CreateAtString { get; set; }
        public Store_StoreExportDTO(Store Store)
        {
            var culture = System.Globalization.CultureInfo.GetCultureInfo("en-EN");
            this.Id = Store.Id;
            this.Code = Store.Code;
            this.CodeDraft = Store.CodeDraft;
            this.Name = Store.Name;
            this.ParentStoreId = Store.ParentStoreId;
            this.OrganizationId = Store.OrganizationId;
            this.StoreTypeId = Store.StoreTypeId;
            this.Telephone = Store.Telephone;
            this.ProvinceId = Store.ProvinceId;
            this.DistrictId = Store.DistrictId;
            this.WardId = Store.WardId;
            this.Address = Store.Address;
            this.DeliveryAddress = Store.DeliveryAddress;
            this.Latitude = Store.Latitude.ToString("F015", culture);
            this.Longitude = Store.Longitude.ToString("F015", culture);
            this.DeliveryLatitude = Store.DeliveryLatitude == null ? null : Store.DeliveryLatitude.Value.ToString("F015", culture);
            this.DeliveryLongitude = Store.DeliveryLongitude == null ? null : Store.DeliveryLongitude.Value.ToString("F015", culture);
            this.OwnerName = Store.OwnerName;
            this.OwnerPhone = Store.OwnerPhone;
            this.OwnerEmail = Store.OwnerEmail;
            this.TaxCode = Store.TaxCode;
            this.LegalEntity = Store.LegalEntity;
            this.StatusId = Store.StatusId;
            this.HasEroute = Store.HasEroute;
            this.HasChecking = Store.HasChecking;
            this.Used = Store.Used;
            this.StoreScoutingId = Store.StoreScoutingId;
            this.AppUserId = Store.AppUserId;
            this.CreatorId = Store.CreatorId;
            this.StoreStatusId = Store.StoreStatusId;
            this.Description = Store.Description;
            this.DebtLimited = Store.DebtLimited;
            this.District = Store.District == null ? null : new Store_DistrictDTO(Store.District);
            this.Organization = Store.Organization == null ? null : new Store_OrganizationDTO(Store.Organization);
            this.ParentStore = Store.ParentStore == null ? null : new Store_StoreDTO(Store.ParentStore);
            this.Province = Store.Province == null ? null : new Store_ProvinceDTO(Store.Province);
            this.Status = Store.Status == null ? null : new Store_StatusDTO(Store.Status);
            this.StoreType = Store.StoreType == null ? null : new Store_StoreTypeDTO(Store.StoreType);
            this.Ward = Store.Ward == null ? null : new Store_WardDTO(Store.Ward);
            this.StoreScouting = Store.StoreScouting == null ? null : new Store_StoreScoutingDTO(Store.StoreScouting);
            this.AppUser = Store.AppUser == null ? null : new Store_AppUserDTO(Store.AppUser);
            this.Creator = Store.Creator == null ? null : new Store_AppUserDTO(Store.Creator);
            this.StoreStatus = Store.StoreStatus == null ? null : new Store_StoreStatusDTO(Store.StoreStatus);
            this.BrandInStores = Store.BrandInStores?.Select(x => new Store_BrandInStoreDTO(x)).ToList();
            this.StoreStoreGroupingMappings = Store.StoreStoreGroupingMappings?.Select(x => new Store_StoreStoreGroupingMappingDTO(x)).ToList();
            this.AppUserStoreMappings = Store.AppUserStoreMappings?.Select(x => new Store_AppUserStoreMappingDTO(x)).ToList();
            this.Errors = Store.Errors;
            this.CreatedAt = Store.CreatedAt;
        }
    }
}
