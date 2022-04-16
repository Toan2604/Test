using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_StoreScoutingDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string OwnerPhone { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? WardId { get; set; }
        public long OrganizationId { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public long CreatorId { get; set; }
        public long StoreScoutingStatusId { get; set; }
        public long StoreScoutingTypeId { get; set; }
        public string Description { get; set; }
        public Guid RowId { get; set; }
        public GeneralMobile_OrganizationDTO Organization { get; set; }
        public GeneralMobile_AppUserDTO Creator { get; set; }
        public GeneralMobile_DistrictDTO District { get; set; }
        public GeneralMobile_ProvinceDTO Province { get; set; }
        public GeneralMobile_StoreScoutingStatusDTO StoreScoutingStatus { get; set; }
        public GeneralMobile_StoreScoutingTypeDTO StoreScoutingType { get; set; }
        public GeneralMobile_WardDTO Ward { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<GeneralMobile_StoreScoutingImageMappingDTO> StoreScoutingImageMappings { get; set; }
        public GeneralMobile_StoreScoutingDTO() { }
        public GeneralMobile_StoreScoutingDTO(StoreScouting StoreScouting)
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
            this.Description = StoreScouting.Description;
            this.RowId = StoreScouting.RowId;
            this.Creator = StoreScouting.Creator == null ? null : new GeneralMobile_AppUserDTO(StoreScouting.Creator);
            this.Organization = StoreScouting.Organization == null ? null : new GeneralMobile_OrganizationDTO(StoreScouting.Organization);
            this.District = StoreScouting.District == null ? null : new GeneralMobile_DistrictDTO(StoreScouting.District);
            this.Province = StoreScouting.Province == null ? null : new GeneralMobile_ProvinceDTO(StoreScouting.Province);
            this.StoreScoutingStatus = StoreScouting.StoreScoutingStatus == null ? null : new GeneralMobile_StoreScoutingStatusDTO(StoreScouting.StoreScoutingStatus);
            this.StoreScoutingType = StoreScouting.StoreScoutingType == null ? null : new GeneralMobile_StoreScoutingTypeDTO(StoreScouting.StoreScoutingType);
            this.Ward = StoreScouting.Ward == null ? null : new GeneralMobile_WardDTO(StoreScouting.Ward);
            this.StoreScoutingImageMappings = StoreScouting.StoreScoutingImageMappings?.Select(x => new GeneralMobile_StoreScoutingImageMappingDTO(x)).ToList();
            this.CreatedAt = StoreScouting.CreatedAt;
            this.UpdatedAt = StoreScouting.UpdatedAt;
            this.Errors = StoreScouting.Errors;
        }
    }

    public class GeneralMobile_StoreScoutingFilterDTO : FilterDTO
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
        public IdFilter CreatorId { get; set; }
        public IdFilter StoreScoutingStatusId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public StoreScoutingOrder OrderBy { get; set; }
    }
}
