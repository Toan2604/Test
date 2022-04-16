using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.store
{
    public class Store_BrandInStoreShowingCategoryMappingDTO : DataDTO
    {
        public long BrandInStoreId { get; set; }
        public long ShowingCategoryId { get; set; }

        public Store_ShowingCategoryDTO ShowingCategory { get; set; }
        public Store_BrandInStoreShowingCategoryMappingDTO() { }
        public Store_BrandInStoreShowingCategoryMappingDTO(BrandInStoreShowingCategoryMapping BrandInStoreShowingCategoryMapping)
        {
            this.BrandInStoreId = BrandInStoreShowingCategoryMapping.BrandInStoreId;
            this.ShowingCategoryId = BrandInStoreShowingCategoryMapping.ShowingCategoryId;
            this.Errors = BrandInStoreShowingCategoryMapping.Errors;
            this.ShowingCategory = BrandInStoreShowingCategoryMapping.ShowingCategory == null ? null : new Store_ShowingCategoryDTO(BrandInStoreShowingCategoryMapping.ShowingCategory);
        }

    }

    public class Store_BrandInStoreShowingCategoryMappingFilterDTO : FilterDTO
    {
        public IdFilter BrandInStoreId { get; set; }
        public IdFilter ShowingCategoryId { get; set; }
        public BrandInStoreShowingCategoryMappingOrder OrderBy { get; set; }
    }
}
