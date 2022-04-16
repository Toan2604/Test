using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store.report_store_state_change
{
    [DisplayName("Báo cáo chuyển đổi trạng thái đại lý")]
    public class ReportStoreStateChangeRoute : Root
    {
        public const string Parent = Module + "/store-report";
        public const string Master = Module + "/store-report/report-store-state-change-master";

        private const string Default = Rpc + Module + "/report-store-state-change";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Export = Default + "/export";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListStore = Default + "/filter-list-store";
        public const string FilterListStoreStatusHistoryType = Default + "/filter-list-store-status-history-type";
        public const string FilterListProvince = Default + "/filter-list-province";
        public const string FilterListDistrict = Default + "/filter-list-district";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(ReportStoreStateChange_ReportStoreStateChangeFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(ReportStoreStateChange_ReportStoreStateChangeFilterDTO.StoreId), FieldTypeEnum.ID.Id },
            { nameof(ReportStoreStateChange_ReportStoreStateChangeFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(ReportStoreStateChange_ReportStoreStateChangeFilterDTO.ProvinceId), FieldTypeEnum.ID.Id },
            { nameof(ReportStoreStateChange_ReportStoreStateChangeFilterDTO.DistrictId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Export,
                FilterListOrganization, FilterListStore, FilterListStoreStatusHistoryType,
                FilterListProvince, FilterListDistrict } },

        };
    }
}
