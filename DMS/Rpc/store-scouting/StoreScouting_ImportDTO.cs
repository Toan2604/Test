using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.store_scouting
{
    public class StoreScouting_ImportDTO : DataDTO
    {
        public long Stt { get; set; }
        public string CodeValue { get; set; }
        public string NameValue { get; set; }
        public string StoreScoutingTypeCodeValue { get; set; }
        public string OrganizationCodeValue { get; set; }
        public string ProvinceCodeValue { get; set; }
        public string DistrictCodeValue { get; set; }
        public string WardCodeValue { get; set; }
        public string AddressValue { get; set; }
        public string LongitudeValue { get; set; }
        public string LatitudeValue { get; set; }
        public string OwnerPhoneValue { get; set; }
        public string SalesEmployeeUsernameValue { get; set; }
        public string Description { get; set; }
        public string StoreScoutingStatusNameValue { get; set; }
        public bool IsNew { get; set; }
        public StoreScouting StoreScouting { get; set; }
        public long OrganizationId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public long StoreScoutingTypeId { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? WardId { get; set; }
        public long SalesEmployeeId { get; set; }
        public long StoreScoutingStatusId { get; set; }
    }
}
