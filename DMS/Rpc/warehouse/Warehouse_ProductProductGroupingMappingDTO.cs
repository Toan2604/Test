using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.warehouse
{
    public class Warehouse_ProductProductGroupingMappingDTO : DataDTO
    {
        public long ProductId { get; set; }
        public long ProductGroupingId { get; set; }
        public Warehouse_ProductGroupingDTO ProductGrouping { get; set; }

        public Warehouse_ProductProductGroupingMappingDTO() { }
        public Warehouse_ProductProductGroupingMappingDTO(ProductProductGroupingMapping ProductProductGroupingMapping)
        {
            this.ProductId = ProductProductGroupingMapping.ProductId;
            this.ProductGroupingId = ProductProductGroupingMapping.ProductGroupingId;
            this.ProductGrouping = ProductProductGroupingMapping.ProductGrouping == null ? null : new Warehouse_ProductGroupingDTO(ProductProductGroupingMapping.ProductGrouping);
        }
    }

    public class Warehouse_ProductProductGroupingMappingFilterDTO : FilterDTO
    {

        public IdFilter ProductId { get; set; }

        public IdFilter ProductGroupingId { get; set; }

        public ProductProductGroupingMappingOrder OrderBy { get; set; }
    }
}
