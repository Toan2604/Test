using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.kpi_product_grouping
{
    public class KpiProductGrouping_ProductProductGroupingMappingDTO : DataDTO
    {
        public long ProductId { get; set; }
        public long ProductGroupingId { get; set; }
        public KpiProductGrouping_ProductGroupingDTO ProductGrouping { get; set; }

        public KpiProductGrouping_ProductProductGroupingMappingDTO() { }
        public KpiProductGrouping_ProductProductGroupingMappingDTO(ProductProductGroupingMapping ProductProductGroupingMapping)
        {
            this.ProductId = ProductProductGroupingMapping.ProductId;
            this.ProductGroupingId = ProductProductGroupingMapping.ProductGroupingId;
            this.ProductGrouping = ProductProductGroupingMapping.ProductGrouping == null ? null : new KpiProductGrouping_ProductGroupingDTO(ProductProductGroupingMapping.ProductGrouping);
        }
    }

    public class KpiProductGrouping_ProductProductGroupingMappingFilterDTO : FilterDTO
    {

        public IdFilter ProductId { get; set; }

        public IdFilter ProductGroupingId { get; set; }

        public ProductProductGroupingMappingOrder OrderBy { get; set; }
    }
}
