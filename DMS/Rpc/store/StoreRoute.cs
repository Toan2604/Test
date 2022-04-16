using DMS.Common;
using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.store
{
    [DisplayName("Đại lý")]
    public class StoreRoute : Root
    {
        public const string Parent = Module + "/location";
        public const string Master = Module + "/location/store/store-master";
        public const string Detail = Module + "/location/store/store-detail/*";
        public const string Mobile = Module + ".store.*";
        private const string Default = Rpc + Module + "/store";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string GetDraft = Default + "/get-draft";
        public const string Create = Default + "/create";
        public const string CreateStoreScouting = Default + "/create-store-scouting";
        public const string UpdateDraft = Default + "/update-draft";
        public const string Update = Default + "/update";
        public const string Approve = Default + "/approve";
        public const string Delete = Default + "/delete";
        public const string Import = Default + "/import";
        public const string ImportBrand = Default + "/import-brand";
        public const string ImportBrandAndProductGrouping = Default + "/import-brand-product-grouping";
        public const string Export = Default + "/export";
        public const string ExportTemplate = Default + "/export-template";
        public const string ExportStoreProfile = Default + "/export-store-profile";
        public const string BulkDelete = Default + "/bulk-delete";
        public const string SaveImage = Default + "/save-image";

        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListDistrict = Default + "/filter-list-district";
        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListParentStore = Default + "/filter-list-parent-store";
        public const string FilterListProvince = Default + "/filter-list-province";
        public const string FilterListWard = Default + "/filter-list-ward";
        public const string FilterListStoreGrouping = Default + "/filter-list-store-grouping";
        public const string FilterListStoreType = Default + "/filter-list-store-type";
        public const string FilterListStatus = Default + "/filter-list-status";
        public const string FilterListStoreStatus = Default + "/filter-list-store-status";
        public const string FilterListStoreUserStatus = Default + "/filter-list-store-user-status";

        public const string SingleListAppUser = Default + "/single-list-app-user";
        public const string SingleListBrand = Default + "/single-list-brand";
        public const string SingleListOrganization = Default + "/single-list-organization";
        public const string SingleListParentStore = Default + "/single-list-parent-store";
        public const string SingleListProvince = Default + "/single-list-province";
        public const string SingleListDistrict = Default + "/single-list-district";
        public const string SingleListWard = Default + "/single-list-ward";
        public const string SingleListStatus = Default + "/single-list-status";
        public const string SingleListStoreGrouping = Default + "/single-list-store-grouping";
        public const string SingleListStoreType = Default + "/single-list-store-type";
        public const string SingleListStoreStatus = Default + "/single-list-store-status";
        public const string SingleListEstimatedRevenue = Default + "/single-list-estimated-revenue";

        public const string ListReseller = Default + "/list-reseller";
        public const string CountReseller = Default + "/count-reseller";
        public const string ListProductGrouping = Default + "/list-product-grouping";
        public const string CountProductGrouping = Default + "/count-product-grouping";
        public const string ListShowingCategory = Default + "/list-showing-category";
        public const string CountShowingCategory = Default + "/count-showing-category";

        public const string CreateDraft = Default + "/create-draft";
        public const string CreateStoreUser = Default + "/create-store-user";
        public const string BulkCreateStoreUser = Default + "/bulk-create-store-user";
        public const string LockStoreUser = Default + "/lock-store-user";
        public const string ResetPassword = Default + "/reset-password";

        // storeSync from external
        public const string BulkCreate = Default + "/bulk-create";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(StoreFilter.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(StoreFilter.StoreTypeId), FieldTypeEnum.ID.Id },
            { nameof(StoreFilter.StoreGroupingId), FieldTypeEnum.ID.Id },
            { nameof(StoreFilter.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListDistrict, FilterListOrganization, FilterListProvince, FilterListWard, FilterListStatus, FilterListParentStore, FilterListStoreStatus, FilterListStoreGrouping, FilterListStoreType } },
            { "Thêm", new List<string> {
                Parent,
                Master, Count, List, Get, GetDraft,
                FilterListAppUser, FilterListDistrict, FilterListOrganization, FilterListProvince, FilterListWard, FilterListStatus, FilterListParentStore, FilterListStoreStatus, FilterListStoreGrouping, FilterListStoreType,
                Detail, Create, CreateStoreScouting, SaveImage, UpdateDraft,
                SingleListAppUser, SingleListBrand, SingleListDistrict, SingleListOrganization, SingleListProvince, SingleListStatus, SingleListStoreGrouping, SingleListStoreType, SingleListWard, SingleListParentStore, SingleListStoreStatus,
                CountProductGrouping, ListProductGrouping, ListShowingCategory, CountShowingCategory, SingleListEstimatedRevenue,
                BulkCreate
                } },
            { "Sửa", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListDistrict, FilterListOrganization, FilterListProvince, FilterListWard, FilterListStatus, FilterListParentStore, FilterListStoreStatus, FilterListStoreGrouping, FilterListStoreType,
                Detail, Update, SaveImage, UpdateDraft,
                SingleListAppUser, SingleListBrand, SingleListDistrict, SingleListOrganization, SingleListProvince, SingleListStatus, SingleListStoreGrouping, SingleListStoreType, SingleListWard, SingleListParentStore, SingleListStoreStatus,
                CountProductGrouping, ListProductGrouping, ListShowingCategory, CountShowingCategory, SingleListEstimatedRevenue,
                BulkCreate
                } },
            { "Xoá", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListDistrict, FilterListOrganization, FilterListProvince, FilterListWard, FilterListStatus, FilterListParentStore, FilterListStoreStatus, FilterListStoreGrouping, FilterListStoreType,
                Delete, } },
            { "Xoá nhiều", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListDistrict, FilterListOrganization, FilterListProvince, FilterListWard, FilterListStatus, FilterListParentStore, FilterListStoreStatus, FilterListStoreGrouping, FilterListStoreType,
                Detail, BulkDelete } },
            { "Xuất excel", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListDistrict, FilterListOrganization, FilterListProvince, FilterListWard, FilterListStatus, FilterListParentStore, FilterListStoreStatus, FilterListStoreGrouping, FilterListStoreType,
                Detail, Export, ExportStoreProfile } },
            { "Nhập excel", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListDistrict, FilterListOrganization, FilterListProvince, FilterListWard, FilterListStatus, FilterListParentStore, FilterListStoreStatus, FilterListStoreGrouping, FilterListStoreType,
                Detail, ExportTemplate, Import, ImportBrand, ImportBrandAndProductGrouping } },
            { "Phê duyệt", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListDistrict, FilterListOrganization, FilterListProvince, FilterListWard, FilterListStatus, FilterListParentStore, FilterListStoreStatus, FilterListStoreGrouping, FilterListStoreType,
                Detail, Approve,
                SingleListAppUser, SingleListBrand, SingleListDistrict, SingleListOrganization, SingleListProvince, SingleListStatus, SingleListStoreGrouping, SingleListStoreType, SingleListWard, SingleListParentStore, SingleListStoreStatus,
                CountProductGrouping, ListProductGrouping, ListShowingCategory, CountShowingCategory, SingleListEstimatedRevenue
                } },

             { "Tạo tài khoản cửa hàng", new List<string> {
                Parent,
                Master, Count, List, Get, CreateDraft, BulkCreateStoreUser,
                CreateStoreUser, ResetPassword, LockStoreUser,
                FilterListAppUser, FilterListDistrict, FilterListOrganization, FilterListProvince, FilterListWard, FilterListStatus, FilterListParentStore, FilterListStoreStatus, FilterListStoreUserStatus, FilterListStoreType } },
        };
    }
}
