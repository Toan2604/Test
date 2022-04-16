using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.direct_sales_order
{
    public class MobileDirectSalesOrder_ProductDTO : DataDTO
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
        public MobileDirectSalesOrder_ProductTypeDTO ProductType { get; set; }
        public MobileDirectSalesOrder_CategoryDTO Category { get; set; }
        public MobileDirectSalesOrder_BrandDTO Brand { get; set; }
        public MobileDirectSalesOrder_SupplierDTO Supplier { get; set; }
        public MobileDirectSalesOrder_TaxTypeDTO TaxType { get; set; }
        public MobileDirectSalesOrder_UnitOfMeasureDTO UnitOfMeasure { get; set; }
        public MobileDirectSalesOrder_UnitOfMeasureGroupingDTO UnitOfMeasureGrouping { get; set; }
        public List<MobileDirectSalesOrder_ProductProductGroupingMappingDTO> ProductProductGroupingMappings { get; set; }
        public MobileDirectSalesOrder_ProductDTO() { }
        public MobileDirectSalesOrder_ProductDTO(Product Product)
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
            this.Category = Product.Category == null ? null : new MobileDirectSalesOrder_CategoryDTO(Product.Category);
            this.Brand = Product.Brand == null ? null : new MobileDirectSalesOrder_BrandDTO(Product.Brand);
            this.ProductType = Product.ProductType == null ? null : new MobileDirectSalesOrder_ProductTypeDTO(Product.ProductType);
            this.Supplier = Product.Supplier == null ? null : new MobileDirectSalesOrder_SupplierDTO(Product.Supplier);
            this.TaxType = Product.TaxType == null ? null : new MobileDirectSalesOrder_TaxTypeDTO(Product.TaxType);
            this.UnitOfMeasure = Product.UnitOfMeasure == null ? null : new MobileDirectSalesOrder_UnitOfMeasureDTO(Product.UnitOfMeasure);
            this.UnitOfMeasureGrouping = Product.UnitOfMeasureGrouping == null ? null : new MobileDirectSalesOrder_UnitOfMeasureGroupingDTO(Product.UnitOfMeasureGrouping);
            this.ProductProductGroupingMappings = Product.ProductProductGroupingMappings?.Select(x => new MobileDirectSalesOrder_ProductProductGroupingMappingDTO(x)).ToList();
            this.Errors = Product.Errors;
        }
    }

    public class MobileDirectSalesOrder_ProductFilterDTO : FilterDTO
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
