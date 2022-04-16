using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.product
{
    [DisplayName("Sản phẩm mới")]
    public class NewProductRoute : Root
    {
        public const string Parent = Module + "/product-category";
        public const string Master = Module + "/product-category/new-product/new-product-master";
        private const string Default = Rpc + Module + "/new-product";
        public const string Mobile = Module + ".new-products.*";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string Delete = Default + "/delete";
        public const string BulkDelete = Default + "/bulk-delete";

        public const string MobileABE = MobileABEModule + ".products.*";

        public const string FilterListProductType = Default + "/filter-list-product-type";
        public const string FilterListStatus = Default + "/filter-list-status";
        public const string FilterListSupplier = Default + "/filter-list-supplier";
        public const string FilterListProductGrouping = Default + "/filter-list-product-grouping";
        public const string FilterListBrand = Default + "/filter-list-brand";
        public const string FilterListCategory = Default + "/filter-list-category";

        public const string CountProduct = Default + "/count-product";
        public const string ListProduct = Default + "/list-product";
        public const string ExportTemplate = Default + "/export-template";
        public const string Import = Default + "/import";
        public const string Export = Default + "/export";
        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(ProductFilter.Code), FieldTypeEnum.STRING.Id },
            { nameof(ProductFilter.SupplierCode), FieldTypeEnum.STRING.Id },
            { nameof(ProductFilter.Name), FieldTypeEnum.STRING.Id },
            { nameof(ProductFilter.Description), FieldTypeEnum.STRING.Id },
            { nameof(ProductFilter.ScanCode), FieldTypeEnum.STRING.Id },
            { nameof(ProductFilter.ProductTypeId), FieldTypeEnum.ID.Id },
            { nameof(ProductFilter.SupplierId), FieldTypeEnum.ID.Id },
            { nameof(ProductFilter.BrandId), FieldTypeEnum.ID.Id },
            { nameof(ProductFilter.UnitOfMeasureId), FieldTypeEnum.ID.Id },
            { nameof(ProductFilter.UnitOfMeasureGroupingId), FieldTypeEnum.ID.Id },
            { nameof(ProductFilter.StatusId), FieldTypeEnum.ID.Id },
        };
        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListProductType, FilterListStatus, FilterListSupplier, FilterListProductGrouping, FilterListBrand, FilterListCategory,
                CountProduct, ListProduct} },
            { "Thêm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListProductType, FilterListStatus, FilterListSupplier, FilterListProductGrouping, FilterListBrand, FilterListCategory,
                Create,
                CountProduct, ListProduct} },
            { "Xoá", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListProductType, FilterListStatus, FilterListSupplier, FilterListProductGrouping, FilterListBrand, FilterListCategory,
                Delete, BulkDelete,
                CountProduct, ListProduct} },
             { "Xoá nhiều", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListProductType, FilterListStatus, FilterListSupplier, FilterListProductGrouping, FilterListBrand, FilterListCategory,
                BulkDelete,
                CountProduct, ListProduct} },
              { "Nhập excel", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListProductType, FilterListStatus, FilterListSupplier, FilterListProductGrouping, FilterListBrand, FilterListCategory,
                Import, ExportTemplate,
                CountProduct, ListProduct} },
              { "Xuất excel", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListProductType, FilterListStatus, FilterListSupplier, FilterListProductGrouping, FilterListBrand, FilterListCategory,
                Export,
                CountProduct, ListProduct} },
        };
    }
}
