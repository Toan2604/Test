using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.store
{
    public class Store_BrandInStoreProductGroupingMappingDTO : DataDTO
    {
        public long BrandInStoreId { get; set; }
        public long ProductGroupingId { get; set; }

        public Store_ProductGroupingDTO ProductGrouping { get; set; }
        public Store_BrandInStoreProductGroupingMappingDTO() { }
        public Store_BrandInStoreProductGroupingMappingDTO(BrandInStoreProductGroupingMapping BrandInStoreProductGroupingMapping)
        {
            this.BrandInStoreId = BrandInStoreProductGroupingMapping.BrandInStoreId;
            this.ProductGroupingId = BrandInStoreProductGroupingMapping.ProductGroupingId;
            this.Errors = BrandInStoreProductGroupingMapping.Errors;
            this.ProductGrouping = BrandInStoreProductGroupingMapping.ProductGrouping == null ? null : new Store_ProductGroupingDTO(BrandInStoreProductGroupingMapping.ProductGrouping);
        }

    }

    public class Store_BrandInStoreProductGroupingMappingFilterDTO : FilterDTO
    {
        public IdFilter BrandInStoreId { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public BrandInStoreProductGroupingMappingOrder OrderBy { get; set; }
    }
}
