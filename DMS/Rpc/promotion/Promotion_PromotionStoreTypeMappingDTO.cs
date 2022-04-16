using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionStoreTypeMappingDTO : DataDTO
    {
        public long PromotionId { get; set; }
        public long StoreTypeId { get; set; }
        public Promotion_StoreTypeDTO StoreType { get; set; }

        public Promotion_PromotionStoreTypeMappingDTO() { }
        public Promotion_PromotionStoreTypeMappingDTO(PromotionStoreTypeMapping PromotionStoreTypeMapping)
        {
            this.PromotionId = PromotionStoreTypeMapping.PromotionId;
            this.StoreTypeId = PromotionStoreTypeMapping.StoreTypeId;
            this.StoreType = PromotionStoreTypeMapping.StoreType == null ? null : new Promotion_StoreTypeDTO(PromotionStoreTypeMapping.StoreType);
            this.Errors = PromotionStoreTypeMapping.Errors;
        }
    }

    public class Promotion_PromotionStoreTypeMappingFilterDTO : FilterDTO
    {

        public IdFilter PromotionId { get; set; }

        public IdFilter StoreTypeId { get; set; }

        public PromotionStoreTypeMappingOrder OrderBy { get; set; }
    }
}