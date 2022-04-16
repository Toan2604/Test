using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.export_template.dto
{
    public class ExportTemplate_ProductDTO : DataDTO
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
        public ExportTemplate_ProductTypeDTO ProductType { get; set; }
        public ExportTemplate_CategoryDTO Category { get; set; }
        public ExportTemplate_TaxTypeDTO TaxType { get; set; }
        public ExportTemplate_UnitOfMeasureDTO UnitOfMeasure { get; set; }
        public List<ExportTemplate_ProductProductGroupingMappingDTO> ProductProductGroupingMappings { get; set; }
    }
}
