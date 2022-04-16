using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_BrandInStoreDTO : DataDTO
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public long BrandId { get; set; }
        public long Top { get; set; }
        public long CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }

        public GeneralMobile_BrandDTO Brand { get; set; }
        public GeneralMobile_AppUserDTO Creator { get; set; }
        public List<GeneralMobile_BrandInStoreProductGroupingMappingDTO> BrandInStoreProductGroupingMappings { get; set; }
        public List<GeneralMobile_BrandInStoreShowingCategoryMappingDTO> BrandInStoreShowingCategoryMappings { get; set; }

        public GeneralMobile_BrandInStoreDTO() { }
        public GeneralMobile_BrandInStoreDTO(BrandInStore BrandInStore)
        {
            this.Id = BrandInStore.Id;
            this.StoreId = BrandInStore.StoreId;
            this.BrandId = BrandInStore.BrandId;
            this.CreatorId = BrandInStore.CreatorId;
            this.Top = BrandInStore.Top;
            this.CreatedAt = BrandInStore.CreatedAt;
            this.UpdatedAt = BrandInStore.UpdatedAt;
            this.Errors = BrandInStore.Errors;
            this.Brand = BrandInStore.Brand == null ? null : new GeneralMobile_BrandDTO(BrandInStore.Brand);
            this.Creator = BrandInStore.Creator == null ? null : new GeneralMobile_AppUserDTO(BrandInStore.Creator);
            this.BrandInStoreProductGroupingMappings = BrandInStore.BrandInStoreProductGroupingMappings?.Select(x => new GeneralMobile_BrandInStoreProductGroupingMappingDTO(x)).ToList();
            this.BrandInStoreShowingCategoryMappings = BrandInStore.BrandInStoreShowingCategoryMappings?.Select(x => new GeneralMobile_BrandInStoreShowingCategoryMappingDTO(x)).ToList();
        }
    }

    public class GeneralMobile_BrandInStoreDTOFilter : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter BrandId { get; set; }
        public LongFilter Top { get; set; }
        public IdFilter CreatorId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public BrandInStoreOrder OrderBy { get; set; }
    }
}
