using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionStoreDTO : DataDTO
    {
        public long Id { get; set; }
        public long PromotionPolicyId { get; set; }
        public long PromotionId { get; set; }
        public string Note { get; set; }
        public decimal FromValue { get; set; }
        public decimal ToValue { get; set; }
        public long PromotionDiscountTypeId { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? Price { get; set; }
        public Promotion_PromotionDiscountTypeDTO PromotionDiscountType { get; set; }
        public Promotion_PromotionPolicyDTO PromotionPolicy { get; set; }

        public Promotion_PromotionStoreDTO() { }
        public Promotion_PromotionStoreDTO(PromotionStore PromotionStore)
        {
            this.Id = PromotionStore.Id;
            this.PromotionPolicyId = PromotionStore.PromotionPolicyId;
            this.PromotionId = PromotionStore.PromotionId;
            this.Note = PromotionStore.Note;
            this.FromValue = PromotionStore.FromValue;
            this.ToValue = PromotionStore.ToValue;
            this.PromotionDiscountTypeId = PromotionStore.PromotionDiscountTypeId;
            this.DiscountPercentage = PromotionStore.DiscountPercentage;
            this.DiscountValue = PromotionStore.DiscountValue;
            this.Price = PromotionStore.Price;
            this.PromotionDiscountType = PromotionStore.PromotionDiscountType == null ? null : new Promotion_PromotionDiscountTypeDTO(PromotionStore.PromotionDiscountType);
            this.PromotionPolicy = PromotionStore.PromotionPolicy == null ? null : new Promotion_PromotionPolicyDTO(PromotionStore.PromotionPolicy);
            this.Errors = PromotionStore.Errors;
        }
    }

    public class Promotion_PromotionStoreFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter PromotionPolicyId { get; set; }

        public IdFilter PromotionId { get; set; }

        public StringFilter Note { get; set; }

        public DecimalFilter FromValue { get; set; }

        public DecimalFilter ToValue { get; set; }

        public IdFilter PromotionDiscountTypeId { get; set; }

        public DecimalFilter DiscountPercentage { get; set; }

        public DecimalFilter DiscountValue { get; set; }
        public DecimalFilter Price { get; set; }

        public PromotionStoreOrder OrderBy { get; set; }
    }
}