using DMS.Common;
using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.store_scouting
{
    [DisplayName("Đại lý cắm cờ")]
    public class StoreScoutingRoute : Root
    {
        public const string Parent = Module + "/location";
        public const string Master = Module + "/location/store-scouting/store-scouting-master";
        public const string Detail = Module + "/location/store-scouting/store-scouting-detail/*";
        public const string Mobile = Module + ".store-scouting.*";
        private const string Default = Rpc + Module + "/store-scouting";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string Reject = Default + "/reject";
        public const string Import = Default + "/import";
        public const string ExportTemplate = Default + "/export-template";
        public const string Export = Default + "/export";

        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListDistrict = Default + "/filter-list-district";
        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListProvince = Default + "/filter-list-province";
        public const string FilterListStore = Default + "/filter-list-store";
        public const string FilterListStoreScoutingStatus = Default + "/filter-list-store-scouting-status";
        public const string FilterListStoreScoutingType = Default + "/filter-list-store-scouting-type";
        public const string FilterListWard = Default + "/filter-list-ward";

        public const string SingleListAppUser = Default + "/single-list-app-user";
        public const string SingleListDistrict = Default + "/single-list-district";
        public const string SingleListOrganization = Default + "/single-list-organization";
        public const string SingleListProvince = Default + "/single-list-province";
        public const string SingleListStore = Default + "/single-list-store";
        public const string SingleListStoreScoutingStatus = Default + "/single-list-store-scouting-status";
        public const string SingleListWard = Default + "/single-list-ward";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(StoreScoutingFilter.Id), FieldTypeEnum.ID.Id },
            { nameof(StoreScoutingFilter.ProvinceId), FieldTypeEnum.ID.Id },
            { nameof(StoreScoutingFilter.DistrictId), FieldTypeEnum.ID.Id },
            { nameof(StoreScoutingFilter.WardId), FieldTypeEnum.ID.Id },
            { nameof(StoreScoutingFilter.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(StoreScoutingFilter.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(StoreScoutingFilter.StoreScoutingStatusId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListDistrict, FilterListOrganization, FilterListProvince, FilterListStore, FilterListStoreScoutingStatus, FilterListWard, FilterListStoreScoutingType } },

            { "Thêm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListDistrict, FilterListOrganization, FilterListProvince, FilterListStore, FilterListStoreScoutingStatus, FilterListWard, FilterListStoreScoutingType,
                Detail, Create,
                SingleListAppUser, SingleListDistrict, SingleListOrganization, SingleListProvince, SingleListStore, SingleListStoreScoutingStatus, SingleListWard, } },

            { "Sửa", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListDistrict, FilterListOrganization, FilterListProvince, FilterListStore, FilterListStoreScoutingStatus, FilterListWard, FilterListStoreScoutingType,
                Detail, Update,
                SingleListAppUser, SingleListDistrict, SingleListOrganization, SingleListProvince, SingleListStore, SingleListStoreScoutingStatus, SingleListWard, } },

            { "Xoá", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListDistrict, FilterListOrganization, FilterListProvince, FilterListStore, FilterListStoreScoutingStatus, FilterListWard, FilterListStoreScoutingType,
                Detail, Delete,
                SingleListAppUser, SingleListDistrict, SingleListOrganization, SingleListProvince, SingleListStore, SingleListStoreScoutingStatus, SingleListWard, } },

            { "Từ chối", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListDistrict, FilterListOrganization, FilterListProvince, FilterListStore, FilterListStoreScoutingStatus, FilterListWard, FilterListStoreScoutingType,
                Detail, Reject,
                SingleListAppUser, SingleListDistrict, SingleListOrganization, SingleListProvince, SingleListStore, SingleListStoreScoutingStatus, SingleListWard, } },

            { "Nhập Excel", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListDistrict, FilterListOrganization, FilterListProvince, FilterListStore, FilterListStoreScoutingStatus, FilterListWard, FilterListStoreScoutingType,
                ExportTemplate, Import} },

            { "Xuất Excel", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListDistrict, FilterListOrganization, FilterListProvince, FilterListStore, FilterListStoreScoutingStatus, FilterListWard, FilterListStoreScoutingType,
                Export} },
        };
    }
}
