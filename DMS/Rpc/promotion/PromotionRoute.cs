using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    [DisplayName("Khuyến mại")]
    public class PromotionRoute : Root
    {
        public const string Master = Module + "/price-list-and-promotion/promotion/promotion-master";
        public const string Detail = Module + "/price-list-and-promotion/promotion/promotion-detail/*";
        public const string SalesOrderPromotion = Module + "/price-list-and-promotion/promotion/promotion-detail/sales-order-promotion/*";
        public const string StorePromotion = Module + "/price-list-and-promotion/promotion/promotion-detail/store-promotion/*";
        public const string ProductPromotion = Module + "/price-list-and-promotion/promotion/promotion-detail/product-promotion/*";
        public const string ProductGroupingPromotion = Module + "/price-list-and-promotion/promotion/promotion-detail/product-grouping-promotion/*";
        public const string ProductTypePromotion = Module + "/price-list-and-promotion/promotion/promotion-detail/product-type-promotion/*";
        public const string ComboPromotion = Module + "/price-list-and-promotion/promotion/promotion-detail/combo-promotion/*";
        public const string SamePricePromotion = Module + "/price-list-and-promotion/promotion/promotion-detail/same-price-promotion/*";
        private const string Default = Rpc + Module + "/promotion";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string GetPreview = Default + "/get-preview";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string CreateDraft = Default + "/create-draft";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string ImportStore = Default + "/import-store";
        public const string ExportStore = Default + "/export-store";
        public const string ExportTemplateStore = Default + "/export-template-store";
        public const string BulkDelete = Default + "/bulk-delete";

        public const string GetMpping = Default + "/get-mapping";
        public const string UpdateDirectSalesOrder = Default + "/update-sales-order";
        public const string UpdateStore = Default + "/update-store";
        public const string UpdateStoreGrouping = Default + "/update-store-grouping";
        public const string UpdateStoreType = Default + "/update-store-type";
        public const string UpdateProduct = Default + "/update-product";
        public const string UpdateProductGrouping = Default + "/update-product-grouping";
        public const string UpdateProductType = Default + "/update-product-type";
        public const string UpdateCombo = Default + "/update-combo";
        public const string UpdateSamePrice = Default + "/update-same-price";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListPromotionType = Default + "/filter-list-promotion-type";
        public const string FilterListStatus = Default + "/filter-list-status";
        public const string FilterListPromotionPolicy = Default + "/filter-list-promotion-policy";
        public const string FilterListPromotionDiscountType = Default + "/filter-list-promotion-discount-type";
        public const string FilterListProductGrouping = Default + "/filter-list-product-grouping";
        public const string FilterListProductType = Default + "/filter-list-product-type";
        public const string FilterListProduct = Default + "/filter-list-product";
        public const string FilterListStoreGrouping = Default + "/filter-list-store-grouping";
        public const string FilterListStore = Default + "/filter-list-store";
        public const string FilterListStoreType = Default + "/filter-list-store-type";

        public const string SingleListOrganization = Default + "/single-list-organization";
        public const string SingleListPromotionType = Default + "/single-list-promotion-type";
        public const string SingleListStatus = Default + "/single-list-status";
        public const string SingleListPromotionPolicy = Default + "/single-list-promotion-policy";
        public const string SingleListPromotionDiscountType = Default + "/single-list-promotion-discount-type";
        public const string SingleListProductGrouping = Default + "/single-list-product-grouping";
        public const string SingleListProductType = Default + "/single-list-product-type";
        public const string SingleListProduct = Default + "/single-list-product";
        public const string SingleListStoreGrouping = Default + "/single-list-store-grouping";
        public const string SingleListStore = Default + "/single-list-store";
        public const string SingleListStoreType = Default + "/single-list-store-type";

        public const string CountStoreGrouping = Default + "/count-store-grouping";
        public const string ListStoreGrouping = Default + "/list-store-grouping";
        public const string CountStore = Default + "/count-store";
        public const string ListStore = Default + "/list-store";
        public const string CountStoreType = Default + "/count-store-type";
        public const string ListStoreType = Default + "/list-store-type";
        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(PromotionFilter.Id), FieldTypeEnum.ID.Id },
            { nameof(PromotionFilter.Code), FieldTypeEnum.STRING.Id },
            { nameof(PromotionFilter.Name), FieldTypeEnum.STRING.Id },
            { nameof(PromotionFilter.StartDate), FieldTypeEnum.DATE.Id },
            { nameof(PromotionFilter.EndDate), FieldTypeEnum.DATE.Id },
            { nameof(PromotionFilter.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(PromotionFilter.PromotionTypeId), FieldTypeEnum.ID.Id },
            { nameof(PromotionFilter.Note), FieldTypeEnum.STRING.Id },
            { nameof(PromotionFilter.Priority), FieldTypeEnum.LONG.Id },
            { nameof(PromotionFilter.StatusId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Master, Count, List,
                Get, GetPreview,

                 FilterListOrganization, FilterListPromotionType, FilterListStatus, FilterListPromotionPolicy, FilterListPromotionDiscountType, FilterListProductGrouping, FilterListProductType, FilterListProduct, FilterListStoreGrouping, FilterListStore, FilterListStoreType, } },
            { "Thêm", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListPromotionType, FilterListStatus, FilterListPromotionPolicy, FilterListPromotionDiscountType, FilterListProductGrouping, FilterListProductType, FilterListProduct, FilterListStoreGrouping, FilterListStore, FilterListStoreType,
                Detail, Create, CreateDraft, UpdateDirectSalesOrder, UpdateStore, UpdateStoreGrouping, UpdateStoreType, UpdateProduct, UpdateProductGrouping, UpdateProductType, UpdateCombo, UpdateSamePrice,
                SingleListOrganization, SingleListPromotionType, SingleListStatus, SingleListPromotionPolicy, SingleListPromotionDiscountType, SingleListProductGrouping, SingleListProductType, SingleListProduct, SingleListStoreGrouping, SingleListStore, SingleListStoreType,
                CountStoreGrouping, ListStoreGrouping, CountStore, ListStore, CountStoreType, ListStoreType } },

            { "Sửa", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListPromotionType, FilterListStatus, FilterListPromotionPolicy, FilterListPromotionDiscountType, FilterListProductGrouping, FilterListProductType, FilterListProduct, FilterListStoreGrouping, FilterListStore, FilterListStoreType,
                Detail, Update, CreateDraft, UpdateDirectSalesOrder, UpdateStore, UpdateStoreGrouping, UpdateStoreType, UpdateProduct, UpdateProductGrouping, UpdateProductType, UpdateCombo, UpdateSamePrice,
                SingleListOrganization, SingleListPromotionType, SingleListStatus, SingleListPromotionPolicy, SingleListPromotionDiscountType, SingleListProductGrouping, SingleListProductType, SingleListProduct, SingleListStoreGrouping, SingleListStore, SingleListStoreType,
                CountStoreGrouping, ListStoreGrouping, CountStore, ListStore, CountStoreType, ListStoreType } },

            { "Xoá", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListPromotionType, FilterListStatus, FilterListPromotionPolicy, FilterListPromotionDiscountType, FilterListProductGrouping, FilterListProductType, FilterListProduct, FilterListStoreGrouping, FilterListStore, FilterListStoreType,
                Delete,
                SingleListOrganization, SingleListPromotionType, SingleListStatus, SingleListPromotionPolicy, SingleListPromotionDiscountType, SingleListProductGrouping, SingleListProductType, SingleListProduct, SingleListStoreGrouping, SingleListStore, SingleListStoreType,  } },

            { "Xoá nhiều", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListPromotionType, FilterListStatus, FilterListPromotionPolicy, FilterListPromotionDiscountType, FilterListProductGrouping, FilterListProductType, FilterListProduct, FilterListStoreGrouping, FilterListStore, FilterListStoreType,
                BulkDelete } },

            { "Xuất excel", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListPromotionType, FilterListStatus, FilterListPromotionPolicy, FilterListPromotionDiscountType, FilterListProductGrouping, FilterListProductType, FilterListProduct, FilterListStoreGrouping, FilterListStore, FilterListStoreType,
                ExportStore } },

            { "Nhập excel", new List<string> {
                Master, Count, List, Get, GetPreview,
                FilterListOrganization, FilterListPromotionType, FilterListStatus, FilterListPromotionPolicy, FilterListPromotionDiscountType, FilterListProductGrouping, FilterListProductType, FilterListProduct, FilterListStoreGrouping, FilterListStore, FilterListStoreType,
                ExportTemplateStore, ImportStore } },
        };
    }
}
