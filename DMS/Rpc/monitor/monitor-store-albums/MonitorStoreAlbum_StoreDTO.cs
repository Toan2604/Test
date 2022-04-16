using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_store_albums
{
    public class MonitorStoreAlbum_StoreDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }

        public string Telephone { get; set; }

        public long StatusId { get; set; }


        public MonitorStoreAlbum_StoreDTO() { }
        public MonitorStoreAlbum_StoreDTO(Store Store)
        {

            this.Id = Store.Id;

            this.Code = Store.Code;

            this.Name = Store.Name;
            this.Address = Store.Address;

            this.Telephone = Store.Telephone;

            this.StatusId = Store.StatusId;

            this.Errors = Store.Errors;
        }
    }

    public class MonitorStoreAlbum_StoreFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter ParentStoreId { get; set; }

        public IdFilter OrganizationId { get; set; }

        public IdFilter StoreTypeId { get; set; }

        public IdFilter StoreGroupingId { get; set; }

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

        public StringFilter TaxCode { get; set; }

        public StringFilter LegalEntity { get; set; }

        public IdFilter StatusId { get; set; }

        public StoreOrder OrderBy { get; set; }
    }
}