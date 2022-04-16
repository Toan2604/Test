using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_ProductDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string SupplierCode { get; set; }
        public string ERPCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ScanCode { get; set; }
        public long CategoryId { get; set; }
        public long ProductTypeId { get; set; }
        public long? SupplierId { get; set; }
        public long? BrandId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public long? UnitOfMeasureGroupingId { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? RetailPrice { get; set; }
        public long TaxTypeId { get; set; }
        public long StatusId { get; set; }
        public string OtherName { get; set; }
        public string TechnicalName { get; set; }
        public string Note { get; set; }
        public List<GeneralMobile_ItemDTO> Items { get; set; }
        public GeneralMobile_BrandDTO Brand { get; set; }
        public GeneralMobile_CategoryDTO Category { get; set; }
        public GeneralMobile_ProductTypeDTO ProductType { get; set; }
        public GeneralMobile_SupplierDTO Supplier { get; set; }
        public GeneralMobile_TaxTypeDTO TaxType { get; set; }
        public GeneralMobile_UnitOfMeasureDTO UnitOfMeasure { get; set; }
        public GeneralMobile_UnitOfMeasureGroupingDTO UnitOfMeasureGrouping { get; set; }
        public List<GeneralMobile_ProductProductGroupingMappingDTO> ProductProductGroupingMappings { get; set; }
        public List<GeneralMobile_VariationGroupingDTO> VariationGroupings { get; set; }
        public GeneralMobile_ProductDTO() { }
        public GeneralMobile_ProductDTO(Product Product)
        {
            this.Id = Product.Id;
            this.Code = Product.Code;
            this.SupplierCode = Product.SupplierCode;
            this.ERPCode = Product.ERPCode;
            this.Name = Product.Name;
            this.Description = Product.Description;
            this.ScanCode = Product.ScanCode;
            this.CategoryId = Product.CategoryId;
            this.ProductTypeId = Product.ProductTypeId;
            this.SupplierId = Product.SupplierId;
            this.BrandId = Product.BrandId;
            this.UnitOfMeasureId = Product.UnitOfMeasureId;
            this.UnitOfMeasureGroupingId = Product.UnitOfMeasureGroupingId;
            this.SalePrice = Product.SalePrice;
            this.RetailPrice = Product.RetailPrice;
            this.TaxTypeId = Product.TaxTypeId;
            this.StatusId = Product.StatusId;
            this.OtherName = Product.OtherName;
            this.TechnicalName = Product.TechnicalName;
            this.Note = Product.Note;
            this.Items = Product.Items?.Select(x => new GeneralMobile_ItemDTO(x)).ToList();
            this.Brand = Product.Brand == null ? null : new GeneralMobile_BrandDTO(Product.Brand);
            this.Category = Product.Category == null ? null : new GeneralMobile_CategoryDTO(Product.Category);
            this.ProductType = Product.ProductType == null ? null : new GeneralMobile_ProductTypeDTO(Product.ProductType);
            this.Supplier = Product.Supplier == null ? null : new GeneralMobile_SupplierDTO(Product.Supplier);
            this.TaxType = Product.TaxType == null ? null : new GeneralMobile_TaxTypeDTO(Product.TaxType);
            this.UnitOfMeasure = Product.UnitOfMeasure == null ? null : new GeneralMobile_UnitOfMeasureDTO(Product.UnitOfMeasure);
            this.UnitOfMeasureGrouping = Product.UnitOfMeasureGrouping == null ? null : new GeneralMobile_UnitOfMeasureGroupingDTO(Product.UnitOfMeasureGrouping);
            this.ProductProductGroupingMappings = Product.ProductProductGroupingMappings?.Select(x => new GeneralMobile_ProductProductGroupingMappingDTO(x)).ToList();
            this.VariationGroupings = Product.VariationGroupings?.Select(x => new GeneralMobile_VariationGroupingDTO(x)).ToList();
            this.Errors = Product.Errors;
        }
    }

    public class GeneralMobile_ProductFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter SupplierCode { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Description { get; set; }
        public StringFilter ScanCode { get; set; }
        public IdFilter CategoryId { get; set; }
        public IdFilter ProductTypeId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter SupplierId { get; set; }
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
        public bool? IsNew { get; set; }
        public string Search { get; set; }
        public ProductOrder OrderBy { get; set; }
    }
    public class GeneralMobile_ProductDetailDTO
    {
        public long ProductId { get; set; }
        public long? StoreId { get; set; }
        public List<long> VariationIds { get; set; }
    }
}
