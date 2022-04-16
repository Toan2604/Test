using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using System.Collections.Generic;
using System.Linq;
namespace DMS.ABE.Rpc.web.direct_sales_order
{
    public class WebDirectSalesOrder_ProductDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string SupplierCode { get; set; }
        public string ERPCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ScanCode { get; set; }
        public long ProductTypeId { get; set; }
        public long CategoryId { get; set; }
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
        public WebDirectSalesOrder_ProductTypeDTO ProductType { get; set; }
        public WebDirectSalesOrder_CategoryDTO Category { get; set; }
        public WebDirectSalesOrder_SupplierDTO Supplier { get; set; }
        public WebDirectSalesOrder_TaxTypeDTO TaxType { get; set; }
        public WebDirectSalesOrder_UnitOfMeasureDTO UnitOfMeasure { get; set; }
        public WebDirectSalesOrder_UnitOfMeasureGroupingDTO UnitOfMeasureGrouping { get; set; }
        public List<WebDirectSalesOrder_ProductProductGroupingMappingDTO> ProductProductGroupingMappings { get; set; }
        public WebDirectSalesOrder_ProductDTO() { }
        public WebDirectSalesOrder_ProductDTO(Product Product)
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
            this.Category = Product.Category == null ? null : new WebDirectSalesOrder_CategoryDTO(Product.Category);
            this.ProductType = Product.ProductType == null ? null : new WebDirectSalesOrder_ProductTypeDTO(Product.ProductType);
            this.Supplier = Product.Supplier == null ? null : new WebDirectSalesOrder_SupplierDTO(Product.Supplier);
            this.TaxType = Product.TaxType == null ? null : new WebDirectSalesOrder_TaxTypeDTO(Product.TaxType);
            this.UnitOfMeasure = Product.UnitOfMeasure == null ? null : new WebDirectSalesOrder_UnitOfMeasureDTO(Product.UnitOfMeasure);
            this.UnitOfMeasureGrouping = Product.UnitOfMeasureGrouping == null ? null : new WebDirectSalesOrder_UnitOfMeasureGroupingDTO(Product.UnitOfMeasureGrouping);
            this.ProductProductGroupingMappings = Product.ProductProductGroupingMappings?.Select(x => new WebDirectSalesOrder_ProductProductGroupingMappingDTO(x)).ToList();
            this.Errors = Product.Errors;
        }
    }
    public class WebDirectSalesOrder_ProductFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter SupplierCode { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Description { get; set; }
        public StringFilter ScanCode { get; set; }
        public IdFilter CategoryId { get; set; }
        public IdFilter ProductTypeId { get; set; }
        public IdFilter SupplierId { get; set; }
        public IdFilter BrandId { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public IdFilter UnitOfMeasureGroupingId { get; set; }
        public DecimalFilter SalePrice { get; set; }
        public DecimalFilter RetailPrice { get; set; }
        public IdFilter TaxTypeId { get; set; }
        public IdFilter StatusId { get; set; }
        public StringFilter OtherName { get; set; }
        public StringFilter TechnicalName { get; set; }
        public StringFilter Note { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public string Search { get; set; }
        public ProductOrder OrderBy { get; set; }
    }
}
