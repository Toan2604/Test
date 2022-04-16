using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionProductTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public long PromotionPolicyId { get; set; }
        public long PromotionId { get; set; }
        public long ProductTypeId { get; set; }
        public string Note { get; set; }
        public decimal FromValue { get; set; }
        public decimal ToValue { get; set; }
        public long PromotionDiscountTypeId { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? Price { get; set; }
        public Promotion_ProductTypeDTO ProductType { get; set; }
        public Promotion_PromotionDiscountTypeDTO PromotionDiscountType { get; set; }
        public Promotion_PromotionPolicyDTO PromotionPolicy { get; set; }
        public List<Promotion_PromotionProductTypeItemMappingDTO> PromotionProductTypeItemMappings { get; set; }
        public Promotion_PromotionProductTypeDTO() { }
        public Promotion_PromotionProductTypeDTO(PromotionProductType PromotionProductType)
        {
            this.Id = PromotionProductType.Id;
            this.PromotionPolicyId = PromotionProductType.PromotionPolicyId;
            this.PromotionId = PromotionProductType.PromotionId;
            this.ProductTypeId = PromotionProductType.ProductTypeId;
            this.Note = PromotionProductType.Note;
            this.FromValue = PromotionProductType.FromValue;
            this.ToValue = PromotionProductType.ToValue;
            this.PromotionDiscountTypeId = PromotionProductType.PromotionDiscountTypeId;
            this.DiscountPercentage = PromotionProductType.DiscountPercentage;
            this.DiscountValue = PromotionProductType.DiscountValue;
            this.Price = PromotionProductType.Price;
            this.ProductType = PromotionProductType.ProductType == null ? null : new Promotion_ProductTypeDTO(PromotionProductType.ProductType);
            this.PromotionDiscountType = PromotionProductType.PromotionDiscountType == null ? null : new Promotion_PromotionDiscountTypeDTO(PromotionProductType.PromotionDiscountType);
            this.PromotionPolicy = PromotionProductType.PromotionPolicy == null ? null : new Promotion_PromotionPolicyDTO(PromotionProductType.PromotionPolicy);
            this.PromotionProductTypeItemMappings = PromotionProductType.PromotionProductTypeItemMappings?.Select(x => new Promotion_PromotionProductTypeItemMappingDTO(x)).ToList();
            this.Errors = PromotionProductType.Errors;
        }
    }

    public class Promotion_PromotionProductTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter PromotionPolicyId { get; set; }

        public IdFilter PromotionId { get; set; }

        public IdFilter ProductTypeId { get; set; }

        public StringFilter Note { get; set; }

        public DecimalFilter FromValue { get; set; }

        public DecimalFilter ToValue { get; set; }

        public IdFilter PromotionDiscountTypeId { get; set; }

        public DecimalFilter DiscountPercentage { get; set; }

        public DecimalFilter DiscountValue { get; set; }
        public DecimalFilter Price { get; set; }

        public PromotionProductTypeOrder OrderBy { get; set; }
    }
}