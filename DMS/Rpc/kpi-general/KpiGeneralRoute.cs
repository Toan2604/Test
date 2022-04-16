using DMS.Common;
using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.kpi_general
{
    [DisplayName("KPI chung")]
    public class KpiGeneralRoute : Root
    {
        public const string Parent = Module + "/kpi-management";
        public const string Master = Module + "/kpi-management/kpi-general/kpi-general-master";
        public const string Detail = Module + "/kpi-management/kpi-general/kpi-general-detail/*";
        private const string Default = Rpc + Module + "/kpi-general";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string GetPreview = Default + "/get-preview";
        public const string Get = Default + "/get";
        public const string GetDraft = Default + "/get-draft";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string Import = Default + "/import";
        public const string Export = Default + "/export";
        public const string ExportTemplate = Default + "/export-template";
        public const string BulkDelete = Default + "/bulk-delete";

        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListCreator = Default + "/filter-list-creator";
        public const string FilterListKpiYear = Default + "/filter-list-kpi-year";
        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListStatus = Default + "/filter-list-status";
        public const string FilterListKpiCriteriaGeneral = Default + "/filter-list-kpi-criteria-general";

        public const string SingleListAppUser = Default + "/single-list-app-user";
        public const string SingleListKpiYear = Default + "/single-list-kpi-year";
        public const string SingleListOrganization = Default + "/single-list-organization";
        public const string SingleListStatus = Default + "/single-list-status";
        public const string SingleListKpiCriteriaGeneral = Default + "/single-list-kpi-criteria-general";
        public const string CountAppUser = Default + "/count-app-user";
        public const string ListAppUser = Default + "/list-app-user";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(KpiGeneralFilter.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(KpiGeneralFilter.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get, GetPreview,
                FilterListAppUser, FilterListCreator, FilterListKpiYear, FilterListOrganization, FilterListStatus,  FilterListKpiCriteriaGeneral, } },
            { "Thêm", new List<string> {
                Parent,
                Master, Count, List, Get, GetPreview, CountAppUser, ListAppUser,
                FilterListAppUser, FilterListCreator, FilterListKpiYear, FilterListOrganization, FilterListStatus,  FilterListKpiCriteriaGeneral,
                Detail, Create,
                SingleListAppUser, SingleListKpiYear, SingleListOrganization, SingleListStatus,  SingleListKpiCriteriaGeneral, GetDraft,
                Count, List, Count, List, Count, List, Count, List, Count, List, Count, List,  } },

            { "Sửa", new List<string> {
                Parent,
                Master, Count, List, Get, GetPreview, ListAppUser, CountAppUser,
                FilterListAppUser, FilterListCreator, FilterListKpiYear, FilterListOrganization, FilterListStatus,  FilterListKpiCriteriaGeneral,
                Detail, Update,
                SingleListAppUser, SingleListKpiYear, SingleListOrganization, SingleListStatus,  SingleListKpiCriteriaGeneral, GetDraft,
                Count, List, Count, List, Count, List, Count, List, Count, List, Count, List,  } },

            { "Xoá", new List<string> {
                Parent,
                Master, Count, List, Get, GetPreview,
                FilterListAppUser, FilterListCreator, FilterListKpiYear, FilterListOrganization, FilterListStatus,  FilterListKpiCriteriaGeneral,
                Delete,
                SingleListAppUser, SingleListKpiYear, SingleListOrganization, SingleListStatus,  SingleListKpiCriteriaGeneral,  } },

            { "Xoá nhiều", new List<string> {
                Parent,
                Master, Count, List, Get, GetPreview,
                FilterListAppUser, FilterListCreator, FilterListKpiYear, FilterListOrganization, FilterListStatus,  FilterListKpiCriteriaGeneral,
                BulkDelete } },

            { "Xuất excel", new List<string> {
                Parent,
                Master, Count, List, Get, GetPreview,
                FilterListAppUser, FilterListCreator, FilterListKpiYear, FilterListOrganization, FilterListStatus,  FilterListKpiCriteriaGeneral,
                Export } },

            { "Nhập excel", new List<string> {
                Parent,
                Master, Count, List, Get, GetPreview,
                FilterListAppUser, FilterListCreator, FilterListKpiYear, FilterListOrganization, FilterListStatus,  FilterListKpiCriteriaGeneral,
                ExportTemplate, Import } },
        };
    }
}
