using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_store_checker
{
    [DisplayName("Giám sát viếng thăm")]
    public class MonitorStoreCheckerRoute : Root
    {
        public const string Parent = Module + "/monitor";
        public const string Master = Module + "/monitor/monitor-store-checker/monitor-store-checker-master";

        private const string Default = Rpc + Module + "/monitor-store-checker";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string ListImage = Default + "/list-image";
        public const string Get = Default + "/get";
        public const string Export = Default + "/export";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListChecking = Default + "/filter-list-checking";
        public const string FilterListImage = Default + "/filter-list-image";
        public const string FilterListSalesOrder = Default + "/filter-list-sales-order";
        public const string FilterListProvince = Default + "/filter-list-province";
        public const string FilterListDistrict = Default + "/filter-list-district";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(MonitorStoreChecker_MonitorStoreCheckerFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(MonitorStoreChecker_MonitorStoreCheckerFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(MonitorStoreChecker_MonitorStoreCheckerFilterDTO.ProvinceId), FieldTypeEnum.ID.Id },
            { nameof(MonitorStoreChecker_MonitorStoreCheckerFilterDTO.DistrictId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, ListImage, Get, Export,
                FilterListOrganization,FilterListAppUser,FilterListChecking, FilterListImage,FilterListSalesOrder,
                FilterListProvince, FilterListDistrict} },

        };
    }
}
