using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class Product : DataEntity, IEquatable<Product>
    {
        public long Id { get; set; }
        public Guid RowId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }

        public string SearchName { get; set; }
        public string Description { get; set; }
        public string ScanCode { get; set; }
        public string ERPCode { get; set; }
        public string SupplierCode { get; set; }
        public long CategoryId { get; set; }
        public long ProductTypeId { get; set; }
        public long? BrandId { get; set; }
        public long? SupplierId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public long? UnitOfMeasureGroupingId { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? RetailPrice { get; set; }
        public long TaxTypeId { get; set; }
        public long StatusId { get; set; }
        public string OtherName { get; set; }
        public string TechnicalName { get; set; }
        public bool IsNew { get; set; }
        public bool IsFavorite { get; set; }
        public long UsedVariationId { get; set; }
        public long VariationCounter { get; set; }
        public bool CanDelete { get; set; }
        public bool Used { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public ProductType ProductType { get; set; }
        public Status Status { get; set; }
        public Supplier Supplier { get; set; }
        public TaxType TaxType { get; set; }
        public UsedVariation UsedVariation { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public UnitOfMeasureGrouping UnitOfMeasureGrouping { get; set; }
        public List<Item> Items { get; set; }
        public List<ProductImageMapping> ProductImageMappings { get; set; }
        public List<ProductProductGroupingMapping> ProductProductGroupingMappings { get; set; }
        public List<VariationGrouping> VariationGroupings { get; set; }

        public bool Equals(Product other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ProductFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter SupplierCode { get; set; }
        public StringFilter Description { get; set; }
        public StringFilter ScanCode { get; set; }
        public StringFilter ERPCode { get; set; }
        public IdFilter CategoryId { get; set; }
        public IdFilter SupplierId { get; set; }
        public IdFilter ProductTypeId { get; set; }
        public IdFilter BrandId { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public IdFilter UnitOfMeasureGroupingId { get; set; }
        public DecimalFilter SalePrice { get; set; }
        public DecimalFilter ItemSalePrice { get; set; }
        public DecimalFilter RetailPrice { get; set; }
        public IdFilter TaxTypeId { get; set; }
        public IdFilter StatusId { get; set; }
        public StringFilter OtherName { get; set; }
        public StringFilter TechnicalName { get; set; }
        public StringFilter Note { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public IdFilter UsedVariationId { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsFavorite { get; set; }
        public string Search { get; set; }
        public string SearchName { get; set; }

        public List<ProductFilter> OrFilter { get; set; }
        public ProductOrder OrderBy { get; set; }
        public ProductSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductOrder
    {
        Id = 0,
        Code = 1,
        Name = 3,
        Description = 4,
        ScanCode = 5,
        ProductType = 6,
        Brand = 8,
        UnitOfMeasure = 9,
        UnitOfMeasureGrouping = 10,
        SalePrice = 11,
        RetailPrice = 12,
        TaxType = 13,
        Status = 14,
        OtherName = 15,
        TechnicalName = 16,
        Note = 17,
        IsNew = 18,
        UsedVariation = 19,
        Category = 20,
        ItemSalePrice = 21,
        CreatedAt = 23,
        SalesOrderCounter = 24,
    }

    [Flags]
    public enum ProductSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._3,
        Description = E._4,
        ScanCode = E._5,
        ProductType = E._6,
        Brand = E._8,
        UnitOfMeasure = E._9,
        UnitOfMeasureGrouping = E._10,
        SalePrice = E._11,
        RetailPrice = E._12,
        TaxType = E._13,
        Status = E._14,
        OtherName = E._15,
        TechnicalName = E._16,
        Note = E._17,
        ERPCode = E._18,
        ProductProductGroupingMapping = E._19,
        IsNew = E._20,
        UsedVariation = E._21,
        VariationGrouping = E._22,
        Category = E._23,
        UsedVariationId = E._24,
        ProductImageMappings = E._25,
        SalesOrderCounter = E._26,
    }
}
