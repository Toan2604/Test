using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.promotion_code
{
    [DisplayName("Mã khuyến mại")]
    public class PromotionCodeRoute : Root
    {
        public const string Master = Module + "/price-list-and-promotion/promotion-code/promotion-code-master";
        public const string Detail = Module + "/price-list-and-promotion/promotion-code/promotion-code-detail/*";
        public const string Preview = Module + "/price-list-and-promotion/promotion-code/promotion-code-preview/*";
        private const string Default = Rpc + Module + "/promotion-code";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string GetPreview = Default + "/get-preview";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string Import = Default + "/import";
        public const string Export = Default + "/export";
        public const string ExportTemplate = Default + "/export-template";
        public const string BulkDelete = Default + "/bulk-delete";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListPromotionDiscountType = Default + "/filter-list-promotion-discount-type";
        public const string FilterListPromotionProductAppliedType = Default + "/filter-list-promotion-product-applied-type";
        public const string FilterListPromotionType = Default + "/filter-list-promotion-type";
        public const string FilterListStatus = Default + "/filter-list-status";
        public const string FilterListProduct = Default + "/filter-list-product";
        public const string FilterListStore = Default + "/filter-list-store";

        public const string SingleListOrganization = Default + "/single-list-organization";
        public const string SingleListPromotionDiscountType = Default + "/single-list-promotion-discount-type";
        public const string SingleListPromotionProductAppliedType = Default + "/single-list-promotion-product-applied-type";
        public const string SingleListPromotionType = Default + "/single-list-promotion-type";
        public const string SingleListStatus = Default + "/single-list-status";
        public const string SingleListSupplier = Default + "/single-list-supplier";
        public const string SingleListProduct = Default + "/single-list-product";
        public const string SingleListStore = Default + "/single-list-store";
        public const string SingleListStoreType = Default + "/single-list-store-type";
        public const string SingleListStoreGrouping = Default + "/single-list-store-grouping";

        public const string CountOrganization = Default + "/count-organization";
        public const string ListOrganization = Default + "/list-organization";
        public const string CountProduct = Default + "/count-product";
        public const string ListProduct = Default + "/list-product";
        public const string CountStore = Default + "/count-store";
        public const string ListStore = Default + "/list-store";
        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(PromotionCodeFilter.Id), FieldTypeEnum.ID.Id },
            { nameof(PromotionCodeFilter.Code), FieldTypeEnum.STRING.Id },
            { nameof(PromotionCodeFilter.Name), FieldTypeEnum.STRING.Id },
            { nameof(PromotionCodeFilter.Quantity), FieldTypeEnum.LONG.Id },
            { nameof(PromotionCodeFilter.PromotionDiscountTypeId), FieldTypeEnum.ID.Id },
            { nameof(PromotionCodeFilter.Value), FieldTypeEnum.DECIMAL.Id },
            { nameof(PromotionCodeFilter.MaxValue), FieldTypeEnum.DECIMAL.Id },
            { nameof(PromotionCodeFilter.PromotionTypeId), FieldTypeEnum.ID.Id },
            { nameof(PromotionCodeFilter.PromotionProductAppliedTypeId), FieldTypeEnum.ID.Id },
            { nameof(PromotionCodeFilter.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(PromotionCodeFilter.StartDate), FieldTypeEnum.DATE.Id },
            { nameof(PromotionCodeFilter.EndDate), FieldTypeEnum.DATE.Id },
            { nameof(PromotionCodeFilter.StatusId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Master, Count, List,
                Get, GetPreview, Preview,
                FilterListOrganization, FilterListPromotionDiscountType, FilterListPromotionProductAppliedType, FilterListPromotionType, FilterListStatus, FilterListProduct, FilterListStore, } },
            { "Thêm", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListPromotionDiscountType, FilterListPromotionProductAppliedType, FilterListPromotionType, FilterListStatus, FilterListProduct, FilterListStore,
                Detail, Create,
                SingleListOrganization, SingleListPromotionDiscountType, SingleListPromotionProductAppliedType, SingleListPromotionType, SingleListStatus, SingleListProduct, SingleListStore,
                SingleListStoreType, SingleListStoreGrouping, SingleListSupplier,
                CountOrganization, ListOrganization, CountProduct, ListProduct, CountStore, ListStore, } },

            { "Sửa", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListPromotionDiscountType, FilterListPromotionProductAppliedType, FilterListPromotionType, FilterListStatus, FilterListProduct, FilterListStore,
                Detail, Update,
                SingleListOrganization, SingleListPromotionDiscountType, SingleListPromotionProductAppliedType, SingleListPromotionType, SingleListStatus, SingleListProduct, SingleListStore,
                SingleListStoreType, SingleListStoreGrouping, SingleListSupplier,
                CountOrganization, ListOrganization, CountProduct, ListProduct, CountStore, ListStore, } },

            { "Xoá", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListPromotionDiscountType, FilterListPromotionProductAppliedType, FilterListPromotionType, FilterListStatus, FilterListProduct, FilterListStore,
                Delete,
                SingleListOrganization, SingleListPromotionDiscountType, SingleListPromotionProductAppliedType, SingleListPromotionType, SingleListStatus, SingleListProduct, SingleListStore,
                SingleListStoreType, SingleListStoreGrouping, SingleListSupplier } },

            { "Xoá nhiều", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListPromotionDiscountType, FilterListPromotionProductAppliedType, FilterListPromotionType, FilterListStatus, FilterListProduct, FilterListStore,
                BulkDelete } },

            { "Xuất excel", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListPromotionDiscountType, FilterListPromotionProductAppliedType, FilterListPromotionType, FilterListStatus, FilterListProduct, FilterListStore,
                Export } },

            { "Nhập excel", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListPromotionDiscountType, FilterListPromotionProductAppliedType, FilterListPromotionType, FilterListStatus, FilterListProduct, FilterListStore,
                ExportTemplate, Import } },
        };
    }
}
