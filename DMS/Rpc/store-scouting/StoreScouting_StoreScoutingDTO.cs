using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.store_scouting
{
    public class StoreScouting_StoreScoutingDTO : DataDTO
    {
        public long STT { get; set; }
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
        public StoreScouting_AppUserDTO Creator { get; set; }
        public StoreScouting_DistrictDTO District { get; set; }
        public StoreScouting_OrganizationDTO Organization { get; set; }
        public StoreScouting_ProvinceDTO Province { get; set; }
        public StoreScouting_StoreScoutingStatusDTO StoreScoutingStatus { get; set; }
        public StoreScouting_StoreScoutingTypeDTO StoreScoutingType { get; set; }
        public StoreScouting_StoreDTO Store { get; set; }
        public StoreScouting_WardDTO Ward { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedAtDisplay => CreatedAt.ToString("dd-MM-yyyy");
        public DateTime UpdatedAt { get; set; }
        public List<StoreScouting_StoreScoutingImageMappingDTO> StoreScoutingImageMappings { get; set; }
        public StoreScouting_StoreScoutingDTO() { }
        public StoreScouting_StoreScoutingDTO(StoreScouting StoreScouting)
        {
            this.Id = StoreScouting.Id;
            this.Code = StoreScouting.Code;
            this.Name = StoreScouting.Name;
            this.OwnerPhone = StoreScouting.OwnerPhone;
            this.ProvinceId = StoreScouting.ProvinceId;
            this.DistrictId = StoreScouting.DistrictId;
            this.WardId = StoreScouting.WardId;
            this.Address = StoreScouting.Address;
            this.Latitude = StoreScouting.Latitude;
            this.Longitude = StoreScouting.Longitude;
            this.CreatorId = StoreScouting.CreatorId;
            this.OrganizationId = StoreScouting.OrganizationId;
            this.StoreScoutingStatusId = StoreScouting.StoreScoutingStatusId;
            this.Link = StoreScouting.Link;
            this.RowId = StoreScouting.RowId;
            this.StoreId = StoreScouting.StoreId;
            this.Description = StoreScouting.Description;
            this.Creator = StoreScouting.Creator == null ? null : new StoreScouting_AppUserDTO(StoreScouting.Creator);
            this.Organization = StoreScouting.Organization == null ? null : new StoreScouting_OrganizationDTO(StoreScouting.Organization);
            this.District = StoreScouting.District == null ? null : new StoreScouting_DistrictDTO(StoreScouting.District);
            this.Province = StoreScouting.Province == null ? null : new StoreScouting_ProvinceDTO(StoreScouting.Province);
            this.Store = StoreScouting.Store == null ? null : new StoreScouting_StoreDTO(StoreScouting.Store);
            this.StoreScoutingStatus = StoreScouting.StoreScoutingStatus == null ? null : new StoreScouting_StoreScoutingStatusDTO(StoreScouting.StoreScoutingStatus);
            this.StoreScoutingType = StoreScouting.StoreScoutingType == null ? null : new StoreScouting_StoreScoutingTypeDTO(StoreScouting.StoreScoutingType);
            this.Ward = StoreScouting.Ward == null ? null : new StoreScouting_WardDTO(StoreScouting.Ward);
            this.CreatedAt = StoreScouting.CreatedAt;
            this.UpdatedAt = StoreScouting.UpdatedAt;
            this.StoreScoutingImageMappings = StoreScouting.StoreScoutingImageMappings?.Select(x => new StoreScouting_StoreScoutingImageMappingDTO(x)).ToList();
            this.Errors = StoreScouting.Errors;
        }
    }

    public class StoreScouting_StoreScoutingFilterDTO : FilterDTO
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
        public IdFilter StoreId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter StoreScoutingStatusId { get; set; }
        public IdFilter StoreScoutingTypeId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public StoreScoutingOrder OrderBy { get; set; }
    }
}
