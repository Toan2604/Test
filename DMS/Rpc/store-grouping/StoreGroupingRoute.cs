using System.Collections.Generic;
using System.ComponentModel;

namespace DMS.Rpc.store_grouping
{
    [DisplayName("Nhóm đại lý")]
    public class StoreGroupingRoute : Root
    {
        public const string Parent = Module + "/location";
        public const string Master = Module + "/location/store-grouping/store-grouping-master";
        public const string Detail = Module + "/location/store-grouping/store-grouping-detail/*";
        private const string Default = Rpc + Module + "/store-grouping";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string BulkDelete = Default + "/bulk-delete";

        public const string SingleListParentStoreGrouping = Default + "/single-list-parent-store-store";
        public const string SingleListStatus = Default + "/single-list-status";

        public const string CountStore = Default + "/count-store";
        public const string ListStore = Default + "/list-store";
        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get, } },
            { "Thêm", new List<string> {
                Parent,
                Master, Count, List, Get,
                Detail, Create,
                SingleListStatus, } },
            { "Sửa", new List<string> {
                Parent,
                Master, Count, List, Get,
                Detail, Update, SingleListStatus, } },
            { "Xoá", new List<string> {
                Parent,
                Master, Count, List, Get,
                Detail, Delete,
                SingleListStatus, } },
            { "Xoá nhiều", new List<string> {
                Parent,
                Master, Count, List, Get,
                BulkDelete } },
        };
    }
}
