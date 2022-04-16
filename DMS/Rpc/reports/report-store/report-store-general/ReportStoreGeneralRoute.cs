using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store.report_store_general
{
    [DisplayName("Báo cáo tổng hợp đại lý")]
    public class ReportStoreGeneralRoute : Root
    {
        public const string Parent = Module + "/store-report";
        public const string Master = Module + "/store-report/store-general-report-master";

        private const string Default = Rpc + Module + "/store-general-report";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Export = Default + "/export";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListStore = Default + "/filter-list-store";
        public const string FilterListStoreType = Default + "/filter-list-store-type";
        public const string FilterListStoreGrouping = Default + "/filter-list-store-grouping";
        public const string FilterListStoreStatus = Default + "/filter-list-store-status";
        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListProvince = Default + "/filter-list-province";
        public const string FilterListDistrict = Default + "/filter-list-district";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(ReportStoreGeneral_ReportStoreGeneralFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(ReportStoreGeneral_ReportStoreGeneralFilterDTO.StoreGroupingId), FieldTypeEnum.ID.Id },
            { nameof(ReportStoreGeneral_ReportStoreGeneralFilterDTO.StoreTypeId), FieldTypeEnum.ID.Id },
            { nameof(ReportStoreGeneral_ReportStoreGeneralFilterDTO.StoreId), FieldTypeEnum.ID.Id },
            { nameof(ReportStoreGeneral_ReportStoreGeneralFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(ReportStoreGeneral_ReportStoreGeneralFilterDTO.ProvinceId), FieldTypeEnum.ID.Id },
            { nameof(ReportStoreGeneral_ReportStoreGeneralFilterDTO.DistrictId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Export,
                FilterListOrganization, FilterListStore, FilterListStoreType, FilterListStoreGrouping, FilterListStoreStatus, FilterListAppUser,
                FilterListProvince, FilterListDistrict} },

        };
    }
}
