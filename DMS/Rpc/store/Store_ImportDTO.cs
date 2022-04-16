using DMS.Entities;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.store
{
    public class Store_ImportDTO : DataDTO
    {
        public long Stt { get; set; }
        public string CodeValue { get; set; }
        public string CodeDraftValue { get; set; }
        public string NameValue { get; set; }
        public string OrganizationCodeValue { get; set; }
        public string ParentStoreCodeValue { get; set; }
        public string StoreTypeCodeValue { get; set; }
        public string StoreGroupingCodeValue { get; set; }
        public string LegalEntityValue { get; set; }
        public string TaxCodeValue { get; set; }
        public string ProvinceCodeValue { get; set; }
        public string DistrictCodeValue { get; set; }
        public string WardCodeValue { get; set; }
        public string AddressValue { get; set; }
        public string LongitudeValue { get; set; }
        public string LatitudeValue { get; set; }
        public string DeliveryAddressValue { get; set; }
        public string DeliveryLongitudeValue { get; set; }
        public string DeliveryLatitudeValue { get; set; }
        public string TelephoneValue { get; set; }
        public string OwnerNameValue { get; set; }
        public string OwnerPhoneValue { get; set; }
        public string OwnerEmailValue { get; set; }
        public string StoreStatusNameValue { get; set; }
        public string StatusNameValue { get; set; }
        public bool IsNew { get; set; }
        public Store Store { get; set; }
        public long OrganizationId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public decimal DeliveryLongitude { get; set; }
        public decimal DeliveryLatitude { get; set; }
        public long StoreTypeId { get; set; }
        public List<long> StoreGroupingIds { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? WardId { get; set; }
        public long StoreStatusId { get; set; }
        public long StatusId { get; set; }
    }

    public class Store_BrandInStoreImportDTO : DataDTO
    {
        public long STT { get; set; }
        public string StoreCode { get; set; }
        public string BrandCode1 { get; set; }
        public string BrandCode2 { get; set; }
        public string BrandCode3 { get; set; }
        public string BrandCode4 { get; set; }
        public string BrandCode5 { get; set; }
        public string ProductGrouping1 { get; set; }
        public string ProductGrouping2 { get; set; }
        public string ProductGrouping3 { get; set; }
        public string ProductGrouping4 { get; set; }
        public string ProductGrouping5 { get; set; }
        public long? Brand1Id { get; set; }
        public long? Brand2Id { get; set; }
        public long? Brand3Id { get; set; }
        public long? Brand4Id { get; set; }
        public long? Brand5Id { get; set; }
        public List<long> ProductGrouping1Ids { get; set; }
        public List<long> ProductGrouping2Ids { get; set; }
        public List<long> ProductGrouping3Ids { get; set; }
        public List<long> ProductGrouping4Ids { get; set; }
        public List<long> ProductGrouping5Ids { get; set; }
        public long StoreId { get; set; }
    }
}
