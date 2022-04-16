using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion_code
{
    public class PromotionCode_PromotionCodeStoreMappingDTO : DataDTO
    {
        public long PromotionCodeId { get; set; }
        public long StoreId { get; set; }
        public PromotionCode_StoreDTO Store { get; set; }

        public PromotionCode_PromotionCodeStoreMappingDTO() { }
        public PromotionCode_PromotionCodeStoreMappingDTO(PromotionCodeStoreMapping PromotionCodeStoreMapping)
        {
            this.PromotionCodeId = PromotionCodeStoreMapping.PromotionCodeId;
            this.StoreId = PromotionCodeStoreMapping.StoreId;
            this.Store = PromotionCodeStoreMapping.Store == null ? null : new PromotionCode_StoreDTO(PromotionCodeStoreMapping.Store);
            this.Errors = PromotionCodeStoreMapping.Errors;
        }
    }

    public class PromotionCode_PromotionCodeStoreMappingFilterDTO : FilterDTO
    {

        public IdFilter PromotionCodeId { get; set; }

        public IdFilter StoreId { get; set; }

        public PromotionCodeStoreMappingOrder OrderBy { get; set; }
    }
}