using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_store_images
{
    [DisplayName("Giám sát hình ảnh")]
    public class MonitorStoreImageRoute : Root
    {
        public const string Parent = Module + "/monitor";
        public const string Master = Module + "/monitor/monitor-store-image/monitor-store-image-master";

        private const string Default = Rpc + Module + "/monitor-store-image";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string UpdateAlbum = Default + "/update-album";
        public const string Export = Default + "/export";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListStore = Default + "/filter-list-store";
        public const string FilterListHasImage = Default + "/filter-list-has-image";
        public const string FilterListHasOrder = Default + "/filter-list-has-order";

        public const string SingleListAlbum = Default + "/single-list-album";
        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(MonitorStoreImage_MonitorStoreImageFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(MonitorStoreImage_MonitorStoreImageFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListOrganization, FilterListAppUser, FilterListStore, FilterListHasImage, FilterListHasOrder,
                SingleListAlbum} },
            { "Cập nhật Album", new List<string> {
                Parent,
                Master, Count, List, Get,
                UpdateAlbum,
                FilterListOrganization, FilterListAppUser, FilterListStore, FilterListHasImage, FilterListHasOrder,
                SingleListAlbum} },
            { "Xuất Excel", new List<string> {
                Parent,
                Master, Count, List, Get, Export,
                FilterListOrganization, FilterListAppUser, FilterListStore, FilterListHasImage, FilterListHasOrder,
                SingleListAlbum} },
        };
    }
}
