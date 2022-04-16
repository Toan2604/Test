using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;

namespace DMS.ABE.Rpc.product
{
    public class Product_ProductProductGroupingMappingDTO : DataDTO
    {
        public long ProductId { get; set; }
        public long ProductGroupingId { get; set; }
        public Product_ProductGroupingDTO ProductGrouping { get; set; }

        public Product_ProductProductGroupingMappingDTO() { }
        public Product_ProductProductGroupingMappingDTO(ProductProductGroupingMapping ProductProductGroupingMapping)
        {
            this.ProductId = ProductProductGroupingMapping.ProductId;
            this.ProductGroupingId = ProductProductGroupingMapping.ProductGroupingId;
            this.ProductGrouping = ProductProductGroupingMapping.ProductGrouping == null ? null : new Product_ProductGroupingDTO(ProductProductGroupingMapping.ProductGrouping);
        }
    }

    public class Product_ProductProductGroupingMappingFilterDTO : FilterDTO
    {

        public IdFilter ProductId { get; set; }

        public IdFilter ProductGroupingId { get; set; }

        public ProductProductGroupingMappingOrder OrderBy { get; set; }
    }
}
