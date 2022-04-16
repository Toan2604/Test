using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionSamePriceDTO : DataDTO
    {
        public long Id { get; set; }
        public string Note { get; set; }
        public long PromotionPolicyId { get; set; }
        public long PromotionId { get; set; }
        public decimal Price { get; set; }
        public Promotion_PromotionPolicyDTO PromotionPolicy { get; set; }

        public Promotion_PromotionSamePriceDTO() { }
        public Promotion_PromotionSamePriceDTO(PromotionSamePrice PromotionSamePrice)
        {
            this.Id = PromotionSamePrice.Id;
            this.Note = PromotionSamePrice.Note;
            this.PromotionPolicyId = PromotionSamePrice.PromotionPolicyId;
            this.PromotionId = PromotionSamePrice.PromotionId;
            this.Price = PromotionSamePrice.Price;
            this.PromotionPolicy = PromotionSamePrice.PromotionPolicy == null ? null : new Promotion_PromotionPolicyDTO(PromotionSamePrice.PromotionPolicy);
            this.Errors = PromotionSamePrice.Errors;
        }
    }

    public class Promotion_PromotionSamePriceFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Note { get; set; }

        public IdFilter PromotionPolicyId { get; set; }

        public IdFilter PromotionId { get; set; }

        public DecimalFilter Price { get; set; }

        public PromotionSamePriceOrder OrderBy { get; set; }
    }
}