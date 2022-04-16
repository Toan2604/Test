using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.album
{
    [DisplayName("Album ảnh")]
    public class AlbumRoute : Root
    {
        public const string Parent = Module + "/gallery";
        public const string Master = Module + "/gallery/album/album-master";
        public const string Detail = Module + "/gallery/album/album-detail/*";
        private const string Default = Rpc + Module + "/album";
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

        public const string FilterListStatus = Default + "/filter-list-status";
        public const string SingleListStatus = Default + "/single-list-status";
        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(AlbumFilter.Name), FieldTypeEnum.STRING.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListStatus, } },
            { "Thêm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListStatus, Detail, Create,  SingleListStatus, } },
            { "Sửa", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListStatus, Detail, Update,  SingleListStatus, } },
            { "Xoá", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListStatus, Detail, Delete,  } },
            { "Xoá nhiều", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListStatus, BulkDelete } },
            { "Xuất excel", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListStatus, Export } },
            { "Nhập excel", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListStatus, ExportTemplate, Import } },
        };
    }
}
