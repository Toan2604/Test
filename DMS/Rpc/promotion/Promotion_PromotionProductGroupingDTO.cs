using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionProductGroupingDTO : DataDTO
    {
        public long Id { get; set; }
        public long PromotionPolicyId { get; set; }
        public long PromotionId { get; set; }
        public long ProductGroupingId { get; set; }
        public string Note { get; set; }
        public decimal FromValue { get; set; }
        public decimal ToValue { get; set; }
        public long PromotionDiscountTypeId { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? Price { get; set; }
        public Promotion_ProductGroupingDTO ProductGrouping { get; set; }
        public Promotion_PromotionDiscountTypeDTO PromotionDiscountType { get; set; }
        public Promotion_PromotionPolicyDTO PromotionPolicy { get; set; }
        public List<Promotion_PromotionProductGroupingItemMappingDTO> PromotionProductGroupingItemMappings { get; set; }
        public Promotion_PromotionProductGroupingDTO() { }
        public Promotion_PromotionProductGroupingDTO(PromotionProductGrouping PromotionProductGrouping)
        {
            this.Id = PromotionProductGrouping.Id;
            this.PromotionPolicyId = PromotionProductGrouping.PromotionPolicyId;
            this.PromotionId = PromotionProductGrouping.PromotionId;
            this.ProductGroupingId = PromotionProductGrouping.ProductGroupingId;
            this.Note = PromotionProductGrouping.Note;
            this.FromValue = PromotionProductGrouping.FromValue;
            this.ToValue = PromotionProductGrouping.ToValue;
            this.PromotionDiscountTypeId = PromotionProductGrouping.PromotionDiscountTypeId;
            this.DiscountPercentage = PromotionProductGrouping.DiscountPercentage;
            this.DiscountValue = PromotionProductGrouping.DiscountValue;
            this.Price = PromotionProductGrouping.Price;
            this.ProductGrouping = PromotionProductGrouping.ProductGrouping == null ? null : new Promotion_ProductGroupingDTO(PromotionProductGrouping.ProductGrouping);
            this.PromotionDiscountType = PromotionProductGrouping.PromotionDiscountType == null ? null : new Promotion_PromotionDiscountTypeDTO(PromotionProductGrouping.PromotionDiscountType);
            this.PromotionPolicy = PromotionProductGrouping.PromotionPolicy == null ? null : new Promotion_PromotionPolicyDTO(PromotionProductGrouping.PromotionPolicy);
            this.PromotionProductGroupingItemMappings = PromotionProductGrouping.PromotionProductGroupingItemMappings?.Select(x => new Promotion_PromotionProductGroupingItemMappingDTO(x)).ToList();
            this.Errors = PromotionProductGrouping.Errors;
        }
    }

    public class Promotion_PromotionProductGroupingFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter PromotionPolicyId { get; set; }

        public IdFilter PromotionId { get; set; }

        public IdFilter ProductGroupingId { get; set; }

        public StringFilter Note { get; set; }

        public DecimalFilter FromValue { get; set; }

        public DecimalFilter ToValue { get; set; }

        public IdFilter PromotionDiscountTypeId { get; set; }

        public DecimalFilter DiscountPercentage { get; set; }

        public DecimalFilter DiscountValue { get; set; }
        public DecimalFilter Price { get; set; }

        public PromotionProductGroupingOrder OrderBy { get; set; }
    }
}