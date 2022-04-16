using DMS.Entities;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_StoreDTO
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

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public decimal? DeliveryLatitude { get; set; }

        public decimal? DeliveryLongitude { get; set; }

        public string OwnerName { get; set; }

        public string OwnerPhone { get; set; }

        public string OwnerEmail { get; set; }
        public string TaxCode { get; set; }
        public string LegalEntity { get; set; }

        public long StoreStatusId { get; set; }

        public long StatusId { get; set; }
        public MobileSync_StoreDTO ParentStore { get; set; }
        public MobileSync_StoreGroupingDTO StoreGrouping { get; set; }
        public MobileSync_StoreTypeDTO StoreType { get; set; }
        public MobileSync_StoreDTO() { }
        public MobileSync_StoreDTO(Store Store)
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

            this.ParentStore = Store.ParentStore == null ? null : new MobileSync_StoreDTO(Store.ParentStore);
            this.StoreType = Store.StoreType == null ? null : new MobileSync_StoreTypeDTO(Store.StoreType);
        }
    }
}
