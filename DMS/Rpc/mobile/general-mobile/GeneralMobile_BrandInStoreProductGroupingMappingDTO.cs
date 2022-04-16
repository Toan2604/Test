using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_BrandInStoreProductGroupingMappingDTO : DataDTO
    {
        public long BrandInStoreId { get; set; }
        public long ProductGroupingId { get; set; }

        public GeneralMobile_ProductGroupingDTO ProductGrouping { get; set; }
        public GeneralMobile_BrandInStoreProductGroupingMappingDTO() { }
        public GeneralMobile_BrandInStoreProductGroupingMappingDTO(BrandInStoreProductGroupingMapping BrandInStoreProductGroupingMapping)
        {
            this.BrandInStoreId = BrandInStoreProductGroupingMapping.BrandInStoreId;
            this.ProductGroupingId = BrandInStoreProductGroupingMapping.ProductGroupingId;
            this.Errors = BrandInStoreProductGroupingMapping.Errors;
            this.ProductGrouping = BrandInStoreProductGroupingMapping.ProductGrouping == null ? null : new GeneralMobile_ProductGroupingDTO(BrandInStoreProductGroupingMapping.ProductGrouping);
        }

    }

    public class GeneralMobile_BrandInStoreProductGroupingMappingFilterDTO : FilterDTO
    {
        public IdFilter BrandInStoreId { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public BrandInStoreProductGroupingMappingOrder OrderBy { get; set; }
    }
}
