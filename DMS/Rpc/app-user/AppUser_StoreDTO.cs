using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.app_user
{
    public class AppUser_StoreDTO : DataDTO
    {

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
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? DeliveryLatitude { get; set; }
        public decimal? DeliveryLongitude { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerEmail { get; set; }
        public string TaxCode { get; set; }
        public string LegalEntity { get; set; }
        public long StatusId { get; set; }
        public bool HasEroute { get; set; }
        public long? WorkflowDefinitionId { get; set; }
        public long? RequestStateId { get; set; }
        public AppUser_ProvinceDTO Province { get; set; }
        public AppUser_DistrictDTO District { get; set; }
        public AppUser_WardDTO Ward { get; set; }
        public AppUser_StoreStatusDTO StoreStatus { get; set; }
        public AppUser_OrganizationDTO Organization { get; set; }
        public AppUser_StoreTypeDTO StoreType { get; set; }
        public AppUser_StoreDTO() { }
        public AppUser_StoreDTO(Store Store)
        {

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
            this.Latitude = Store.Latitude;
            this.Longitude = Store.Longitude;
            this.DeliveryLatitude = Store.DeliveryLatitude;
            this.DeliveryLongitude = Store.DeliveryLongitude;
            this.OwnerName = Store.OwnerName;
            this.OwnerPhone = Store.OwnerPhone;
            this.OwnerEmail = Store.OwnerEmail;
            this.TaxCode = Store.TaxCode;
            this.LegalEntity = Store.LegalEntity;
            this.StatusId = Store.StatusId;
            this.HasEroute = Store.HasEroute;
            this.Province = Store.Province == null ? null : new AppUser_ProvinceDTO(Store.Province);
            this.District = Store.District == null ? null : new AppUser_DistrictDTO(Store.District);
            this.Ward = Store.Ward == null ? null : new AppUser_WardDTO(Store.Ward);
            this.StoreStatus = Store.StoreStatus == null ? null : new AppUser_StoreStatusDTO(Store.StoreStatus);
            this.Organization = Store.Organization == null ? null : new AppUser_OrganizationDTO(Store.Organization);
            this.StoreType = Store.StoreType == null ? null : new AppUser_StoreTypeDTO(Store.StoreType);
            this.Errors = Store.Errors;
        }
    }

    public class AppUser_StoreFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }
        public StringFilter CodeDraft { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter ParentStoreId { get; set; }

        public IdFilter OrganizationId { get; set; }

        public IdFilter StoreTypeId { get; set; }

        public IdFilter ResellerId { get; set; }

        public StringFilter Telephone { get; set; }

        public IdFilter ProvinceId { get; set; }

        public IdFilter DistrictId { get; set; }

        public IdFilter WardId { get; set; }

        public StringFilter Address { get; set; }

        public StringFilter DeliveryAddress { get; set; }

        public DecimalFilter Latitude { get; set; }

        public DecimalFilter Longitude { get; set; }

        public DecimalFilter DeliveryLatitude { get; set; }

        public DecimalFilter DeliveryLongitude { get; set; }

        public StringFilter OwnerName { get; set; }

        public StringFilter OwnerPhone { get; set; }

        public StringFilter OwnerEmail { get; set; }

        public IdFilter StatusId { get; set; }

        public IdFilter WorkflowDefinitionId { get; set; }

        public IdFilter RequestStateId { get; set; }

        public StoreOrder OrderBy { get; set; }
    }
}
