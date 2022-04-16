using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionStoreTypeDTO : DataDTO
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

        public Promotion_PromotionStoreTypeDTO() { }
        public Promotion_PromotionStoreTypeDTO(PromotionStoreType PromotionStoreType)
        {
            this.Id = PromotionStoreType.Id;
            this.PromotionPolicyId = PromotionStoreType.PromotionPolicyId;
            this.PromotionId = PromotionStoreType.PromotionId;
            this.Note = PromotionStoreType.Note;
            this.FromValue = PromotionStoreType.FromValue;
            this.ToValue = PromotionStoreType.ToValue;
            this.PromotionDiscountTypeId = PromotionStoreType.PromotionDiscountTypeId;
            this.DiscountPercentage = PromotionStoreType.DiscountPercentage;
            this.DiscountValue = PromotionStoreType.DiscountValue;
            this.Price = PromotionStoreType.Price;
            this.PromotionDiscountType = PromotionStoreType.PromotionDiscountType == null ? null : new Promotion_PromotionDiscountTypeDTO(PromotionStoreType.PromotionDiscountType);
            this.PromotionPolicy = PromotionStoreType.PromotionPolicy == null ? null : new Promotion_PromotionPolicyDTO(PromotionStoreType.PromotionPolicy);
            this.Errors = PromotionStoreType.Errors;
        }
    }

    public class Promotion_PromotionStoreTypeFilterDTO : FilterDTO
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

        public PromotionStoreTypeOrder OrderBy { get; set; }
    }
}