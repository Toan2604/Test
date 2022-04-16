using System.Collections.Generic;
using System.ComponentModel;
using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.banner
{
    [DisplayName("Banner thông tin")]
    public class BannerRoute : Root
    {
        public const string Parent = Module + "/application-banner";
        public const string Master = Module + "/application-banner/banner/banner-master";
        public const string Detail = Module + "/application-banner/banner/banner-detail/*";
        private const string Default = Rpc + Module + "/banner";

        public const string MobileABE = MobileABEModule + ".banner.*";


        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string Import = Default + "/import";
        public const string Export = Default + "/export";
        public const string ExportTemplate = Default + "/export-tempate";
        public const string BulkDelete = Default + "/bulk-delete";
        public const string SaveImage = Default + "/save-image";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListStatus = Default + "/filter-list-status";

        public const string SingleListOrganization = Default + "/single-list-organization";
        public const string SingleListAppUser = Default + "/single-list-app-user";
        public const string SingleListStatus = Default + "/single-list-status";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(BannerFilter.OrganizationId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master,
                Detail,Count,List,Get,
                FilterListAppUser, FilterListOrganization, FilterListStatus}
            },
            { "Thêm mới", new List<string> {
                Parent,
                Master,
                Detail, Count, List, Get, FilterListAppUser, FilterListOrganization, FilterListStatus, Detail, Create,
                SingleListAppUser, SingleListAppUser, SingleListOrganization, SingleListStatus}
            },
            { "Sửa", new List<string> {
                Parent,
                Master,
                Detail, Count, List, Get, FilterListAppUser, FilterListOrganization, FilterListStatus, Detail, Update,
                SingleListAppUser, SingleListAppUser, SingleListOrganization, SingleListStatus}
            },
            { "Lưu ảnh", new List<string> {
                Parent,
                SaveImage}
            },
            { "Xoá", new List<string> {
                Parent,
                Master,
                Detail, Count, List, Get, FilterListAppUser, FilterListOrganization, FilterListStatus, Detail, Delete,
                SingleListAppUser, SingleListAppUser, SingleListOrganization, SingleListStatus}
            },
            { "Xoá nhiều", new List<string> {
                Parent,
                Master,
                Detail,Count,List,Get, BulkDelete,
                FilterListAppUser, FilterListOrganization, FilterListStatus}
            },
            { "Xuất excel", new List<string> {
                Parent,
                Master,
                Detail,Count,List,Get, Export,
                FilterListAppUser, FilterListOrganization, FilterListStatus}
            },
            { "Nhập excel", new List<string> {
                Parent,
                Master,
                Detail,Count,List,Get, ExportTemplate, Import,
                FilterListAppUser, FilterListOrganization, FilterListStatus}
            },
        };
    }
}
