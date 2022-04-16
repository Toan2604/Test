using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionStoreMappingDTO : DataDTO
    {
        public long PromotionId { get; set; }
        public long StoreId { get; set; }
        public Promotion_StoreDTO Store { get; set; }

        public Promotion_PromotionStoreMappingDTO() { }
        public Promotion_PromotionStoreMappingDTO(PromotionStoreMapping PromotionStoreMapping)
        {
            this.PromotionId = PromotionStoreMapping.PromotionId;
            this.StoreId = PromotionStoreMapping.StoreId;
            this.Store = PromotionStoreMapping.Store == null ? null : new Promotion_StoreDTO(PromotionStoreMapping.Store);
            this.Errors = PromotionStoreMapping.Errors;
        }
    }

    public class Promotion_PromotionStoreMappingFilterDTO : FilterDTO
    {

        public IdFilter PromotionId { get; set; }

        public IdFilter StoreId { get; set; }

        public PromotionStoreMappingOrder OrderBy { get; set; }
    }
}