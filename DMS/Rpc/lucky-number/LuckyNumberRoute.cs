using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.lucky_number
{
    [DisplayName("Quay thưởng")]
    public class LuckyNumberRoute : Root
    {
        public const string Master = Module + "/price-list-and-promotion/lucky-number/lucky-number-master";
        public const string Detail = Module + "/price-list-and-promotion/lucky-number/lucky-number-detail";
        private const string Default = Rpc + Module + "/lucky-number";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string GetPreview = Default + "/get-preview";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string Import = Default + "/import";
        public const string Export = Default + "/export";
        public const string ExportStore = Default + "/export-store";
        public const string ExportTemplate = Default + "/export-template";
        public const string BulkDelete = Default + "/bulk-delete";


        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListRewardStatus = Default + "/filter-list-reward-status";

        public const string SingleListRewardStatus = Default + "/single-list-reward-status";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(LuckyNumberFilter.Id), FieldTypeEnum.ID.Id },
            { nameof(LuckyNumberFilter.Code), FieldTypeEnum.STRING.Id },
            { nameof(LuckyNumberFilter.Name), FieldTypeEnum.STRING.Id },
            { nameof(LuckyNumberFilter.RewardStatusId), FieldTypeEnum.ID.Id },
            { nameof(LuckyNumberFilter.RowId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Master, Count, List,
                Get, GetPreview,

                 FilterListRewardStatus, } },
            { "Thêm", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListRewardStatus,
                Detail, Create,
                SingleListRewardStatus,
                 } },

            { "Sửa", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListRewardStatus,
                Detail, Update,
                SingleListRewardStatus,
                 } },

            { "Xoá", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListRewardStatus,
                Delete,
                SingleListRewardStatus,  } },

            { "Xoá nhiều", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListRewardStatus,
                BulkDelete } },

            { "Xuất excel", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListRewardStatus,
                Export, ExportStore } },

            { "Nhập excel", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListRewardStatus,
                ExportTemplate, Import } },
        };
    }
}
