using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.app_user
{
    [DisplayName("Tài khoản")]
    public class AppUserRoute : Root
    {
        public const string Parent = Module + "/account";
        public const string Master = Module + "/account/app-user/app-user-master";
        public const string Detail = Module + "/account/app-user/app-user-detail/*";
        private const string Default = Rpc + Module + "/app-user";
        public const string Mobile = Default + ".profile";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Update = Default + "/update";
        public const string UpdateRole = Default + "/update-role";
        public const string Export = Default + "/export";
        public const string ImportStore = Default + "/import-store";
        public const string ExportStore = Default + "/export-store";
        public const string ExportTemplateStore = Default + "/export-template-store";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListPosition = Default + "/filter-list-position";
        public const string FilterListStatus = Default + "/filter-list-status";

        public const string SingleListOrganization = Default + "/single-list-organization";
        public const string SingleListPosition = Default + "/single-list-position";
        public const string SingleListSex = Default + "/single-list-sex";
        public const string SingleListStatus = Default + "/single-list-status";
        public const string SingleListRole = Default + "/single-list-role";
        public const string SingleListStore = Default + "/single-list-store";
        public const string SingleListStoreGrouping = Default + "/single-list-store-grouping";
        public const string SingleListStoreType = Default + "/single-list-store-type";

        public const string CountRole = Default + "/count-role";
        public const string ListRole = Default + "/list-role";
        public const string CountStore = Default + "/count-store";
        public const string ListStore = Default + "/list-store";
        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(AppUserFilter.OrganizationId), FieldTypeEnum.ID.Id},
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListPosition, FilterListOrganization, FilterListStatus,
                SingleListOrganization, SingleListPosition, SingleListSex, SingleListStatus, SingleListRole, SingleListStore, SingleListStoreGrouping, SingleListStoreType,
                }},
            { "Thiết lập phạm vi đi tuyến", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListPosition,
                Detail, Get, Update, ImportStore, ExportStore, ExportTemplateStore,
                SingleListOrganization, SingleListPosition, SingleListSex, SingleListStatus, SingleListRole, SingleListStore, SingleListStoreGrouping, SingleListStoreType,
                CountStore, ListStore }},
            { "Thiết lập vai trò", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListPosition,
                Detail, Get, UpdateRole,
                SingleListOrganization, SingleListPosition, SingleListSex, SingleListStatus, SingleListRole, SingleListStore, SingleListStoreGrouping, SingleListStoreType,
                CountRole, ListRole, }},
        };
    }
}
