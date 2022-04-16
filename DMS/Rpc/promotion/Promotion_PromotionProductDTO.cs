using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionProductDTO : DataDTO
    {
        public long Id { get; set; }
        public long PromotionPolicyId { get; set; }
        public long PromotionId { get; set; }
        public long ProductId { get; set; }
        public string Note { get; set; }
        public decimal FromValue { get; set; }
        public decimal ToValue { get; set; }
        public long PromotionDiscountTypeId { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? Price { get; set; }
        public Promotion_ProductDTO Product { get; set; }
        public Promotion_PromotionDiscountTypeDTO PromotionDiscountType { get; set; }
        public Promotion_PromotionPolicyDTO PromotionPolicy { get; set; }
        public List<Promotion_PromotionProductItemMappingDTO> PromotionProductItemMappings { get; set; }
        public Promotion_PromotionProductDTO() { }
        public Promotion_PromotionProductDTO(PromotionProduct PromotionProduct)
        {
            this.Id = PromotionProduct.Id;
            this.PromotionPolicyId = PromotionProduct.PromotionPolicyId;
            this.PromotionId = PromotionProduct.PromotionId;
            this.ProductId = PromotionProduct.ProductId;
            this.Note = PromotionProduct.Note;
            this.FromValue = PromotionProduct.FromValue;
            this.ToValue = PromotionProduct.ToValue;
            this.PromotionDiscountTypeId = PromotionProduct.PromotionDiscountTypeId;
            this.DiscountPercentage = PromotionProduct.DiscountPercentage;
            this.DiscountValue = PromotionProduct.DiscountValue;
            this.Price = PromotionProduct.Price;
            this.Product = PromotionProduct.Product == null ? null : new Promotion_ProductDTO(PromotionProduct.Product);
            this.PromotionDiscountType = PromotionProduct.PromotionDiscountType == null ? null : new Promotion_PromotionDiscountTypeDTO(PromotionProduct.PromotionDiscountType);
            this.PromotionPolicy = PromotionProduct.PromotionPolicy == null ? null : new Promotion_PromotionPolicyDTO(PromotionProduct.PromotionPolicy);
            this.PromotionProductItemMappings = PromotionProduct.PromotionProductItemMappings?.Select(x => new Promotion_PromotionProductItemMappingDTO(x)).ToList();
            this.Errors = PromotionProduct.Errors;
        }
    }

    public class Promotion_PromotionProductFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter PromotionPolicyId { get; set; }

        public IdFilter PromotionId { get; set; }

        public IdFilter ProductId { get; set; }

        public StringFilter Note { get; set; }

        public DecimalFilter FromValue { get; set; }

        public DecimalFilter ToValue { get; set; }

        public IdFilter PromotionDiscountTypeId { get; set; }

        public DecimalFilter DiscountPercentage { get; set; }

        public DecimalFilter DiscountValue { get; set; }
        public DecimalFilter Price { get; set; }

        public PromotionProductOrder OrderBy { get; set; }
    }
}