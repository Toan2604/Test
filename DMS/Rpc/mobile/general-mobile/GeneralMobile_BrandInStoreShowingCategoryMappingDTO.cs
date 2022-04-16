using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_BrandInStoreShowingCategoryMappingDTO : DataDTO
    {
        public long BrandInStoreId { get; set; }
        public long ShowingCategoryId { get; set; }

        public GeneralMobile_ShowingCategoryDTO ShowingCategory { get; set; }
        public GeneralMobile_BrandInStoreShowingCategoryMappingDTO() { }
        public GeneralMobile_BrandInStoreShowingCategoryMappingDTO(BrandInStoreShowingCategoryMapping BrandInStoreShowingCategoryMapping)
        {
            this.BrandInStoreId = BrandInStoreShowingCategoryMapping.BrandInStoreId;
            this.ShowingCategoryId = BrandInStoreShowingCategoryMapping.ShowingCategoryId;
            this.Errors = BrandInStoreShowingCategoryMapping.Errors;
            this.ShowingCategory = BrandInStoreShowingCategoryMapping.ShowingCategory == null ? null : new GeneralMobile_ShowingCategoryDTO(BrandInStoreShowingCategoryMapping.ShowingCategory);
        }

    }

    public class GeneralMobile_BrandInStoreShowingCategoryMappingFilterDTO : FilterDTO
    {
        public IdFilter BrandInStoreId { get; set; }
        public IdFilter ShowingCategoryId { get; set; }
        public BrandInStoreShowingCategoryMappingOrder OrderBy { get; set; }
    }
}
