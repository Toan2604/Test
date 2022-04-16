using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store_checking.report_store_unchecked
{
    [DisplayName("Báo cáo đại lý không viếng thăm")]
    public class ReportStoreUncheckedRoute : Root
    {
        public const string Parent = Module + "/store-checking-report";
        public const string Master = Module + "/store-checking-report/store-unchecked-report-master";

        private const string Default = Rpc + Module + "/store-unchecked-report";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Export = Default + "/export";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListERoute = Default + "/filter-list-e-route";
        public const string FilterListStoreStatus = Default + "/filter-list-store-status";
        public const string FilterListProvince = Default + "/filter-list-province";
        public const string FilterListDistrict = Default + "/filter-list-district";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(ReportStoreUnchecked_ReportStoreUncheckedFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(ReportStoreUnchecked_ReportStoreUncheckedFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(ReportStoreUnchecked_ReportStoreUncheckedFilterDTO.ERouteId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Export,
                FilterListOrganization, FilterListAppUser, FilterListERoute, FilterListStoreStatus,
                FilterListProvince, FilterListDistrict} },

        };
    }
}
