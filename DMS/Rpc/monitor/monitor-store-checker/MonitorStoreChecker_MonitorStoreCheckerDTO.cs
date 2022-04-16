using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_store_checker
{
    public class MonitorStoreChecker_MonitorStoreCheckerDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<MonitorStoreChecker_SaleEmployeeDTO> SaleEmployees { get; set; }
    }


    public class MonitorStoreChecker_MonitorStoreCheckerFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public IdFilter AppUserId { get; set; }
        public DateFilter CheckIn { get; set; }
        public IdFilter Checking { get; set; }
        public IdFilter Image { get; set; }
        public IdFilter SalesOrder { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        public List<MonitorStoreChecker_MonitorStoreCheckerFilterDTO> OrFilters { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MonitorStoreCheckerOrder
    {
        Username = 1,
        DisplayName = 2,
        Organization = 3,
    }
}
