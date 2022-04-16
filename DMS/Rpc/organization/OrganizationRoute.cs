using System.Collections.Generic;
using System.ComponentModel;

namespace DMS.Rpc.organization
{
    [DisplayName("Cây tổ chức")]
    public class OrganizationRoute : Root
    {
        public const string Parent = Module + "/account";
        public const string Master = Module + "/account/organization/organization-master";
        public const string Detail = Module + "/account/organization/organization-detail/*";
        private const string Default = Rpc + Module + "/organization";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string UpdateIsDisplay = Default + "/update-is-display";
        public const string Export = Default + "/export";
        public const string ExportAppUser = Default + "/export-app-user";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListStatus = Default + "/filter-list-status";
        public const string FilterListAppUser = Default + "/filter-list-app-user";

        public const string SingleListOrganization = Default + "/single-list-organization";
        public const string SingleListStatus = Default + "/single-list-status";
        public const string SingleListAppUser = Default + "/single-list-app-user";
        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListOrganization, FilterListStatus, FilterListAppUser, } },
            { "Sửa", new List<string> {
                Parent,
                Master, Count, List, Get, UpdateIsDisplay,
                FilterListOrganization, FilterListStatus, FilterListAppUser, } },
            { "Xuất excel", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListOrganization, FilterListStatus, FilterListAppUser, Export, ExportAppUser } },
        };
    }
}
