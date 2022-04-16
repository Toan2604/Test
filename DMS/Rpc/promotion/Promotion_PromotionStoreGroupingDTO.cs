using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionStoreGroupingDTO : DataDTO
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

        public Promotion_PromotionStoreGroupingDTO() { }
        public Promotion_PromotionStoreGroupingDTO(PromotionStoreGrouping PromotionStoreGrouping)
        {
            this.Id = PromotionStoreGrouping.Id;
            this.PromotionPolicyId = PromotionStoreGrouping.PromotionPolicyId;
            this.PromotionId = PromotionStoreGrouping.PromotionId;
            this.Note = PromotionStoreGrouping.Note;
            this.FromValue = PromotionStoreGrouping.FromValue;
            this.ToValue = PromotionStoreGrouping.ToValue;
            this.PromotionDiscountTypeId = PromotionStoreGrouping.PromotionDiscountTypeId;
            this.DiscountPercentage = PromotionStoreGrouping.DiscountPercentage;
            this.DiscountValue = PromotionStoreGrouping.DiscountValue;
            this.Price = PromotionStoreGrouping.Price;
            this.PromotionDiscountType = PromotionStoreGrouping.PromotionDiscountType == null ? null : new Promotion_PromotionDiscountTypeDTO(PromotionStoreGrouping.PromotionDiscountType);
            this.PromotionPolicy = PromotionStoreGrouping.PromotionPolicy == null ? null : new Promotion_PromotionPolicyDTO(PromotionStoreGrouping.PromotionPolicy);
            this.Errors = PromotionStoreGrouping.Errors;
        }
    }

    public class Promotion_PromotionStoreGroupingFilterDTO : FilterDTO
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

        public PromotionStoreGroupingOrder OrderBy { get; set; }
    }
}