using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.price_list
{
    public class PriceList_ProductProductGroupingMappingDTO : DataDTO
    {
        public long ProductId { get; set; }
        public long ProductGroupingId { get; set; }
        public PriceList_ProductGroupingDTO ProductGrouping { get; set; }

        public PriceList_ProductProductGroupingMappingDTO() { }
        public PriceList_ProductProductGroupingMappingDTO(ProductProductGroupingMapping ProductProductGroupingMapping)
        {
            this.ProductId = ProductProductGroupingMapping.ProductId;
            this.ProductGroupingId = ProductProductGroupingMapping.ProductGroupingId;
            this.ProductGrouping = ProductProductGroupingMapping.ProductGrouping == null ? null : new PriceList_ProductGroupingDTO(ProductProductGroupingMapping.ProductGrouping);
        }
    }

    public class Mobile_ProductProductGroupingMappingFilterDTO : FilterDTO
    {

        public IdFilter ProductId { get; set; }

        public IdFilter ProductGroupingId { get; set; }

        public ProductProductGroupingMappingOrder OrderBy { get; set; }
    }
}
