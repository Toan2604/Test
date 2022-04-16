using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
namespace DMS.ABE.Rpc.web.direct_sales_order
{
    public class WebDirectSalesOrder_ProductProductGroupingMappingDTO : DataDTO
    {
        public long ProductId { get; set; }
        public long ProductGroupingId { get; set; }
        public WebDirectSalesOrder_ProductGroupingDTO ProductGrouping { get; set; }
        public WebDirectSalesOrder_ProductProductGroupingMappingDTO() { }
        public WebDirectSalesOrder_ProductProductGroupingMappingDTO(ProductProductGroupingMapping ProductProductGroupingMapping)
        {
            this.ProductId = ProductProductGroupingMapping.ProductId;
            this.ProductGroupingId = ProductProductGroupingMapping.ProductGroupingId;
            this.ProductGrouping = ProductProductGroupingMapping.ProductGrouping == null ? null : new WebDirectSalesOrder_ProductGroupingDTO(ProductProductGroupingMapping.ProductGrouping);
        }
    }
    public class WebDirectSalesOrder_ProductProductGroupingMappingFilterDTO : FilterDTO
    {
        public IdFilter ProductId { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public ProductProductGroupingMappingOrder OrderBy { get; set; }
    }
}
