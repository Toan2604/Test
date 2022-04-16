using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionComboDTO : DataDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long PromotionPolicyId { get; set; }
        public long PromotionId { get; set; }
        public string Note { get; set; }
        public long PromotionDiscountTypeId { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? Price { get; set; }
        public Promotion_PromotionDiscountTypeDTO PromotionDiscountType { get; set; }
        public Promotion_PromotionPolicyDTO PromotionPolicy { get; set; }
        public List<Promotion_PromotionComboInItemMappingDTO> PromotionComboInItemMappings { get; set; }
        public List<Promotion_PromotionComboOutItemMappingDTO> PromotionComboOutItemMappings { get; set; }
        public Promotion_PromotionComboDTO() { }
        public Promotion_PromotionComboDTO(PromotionCombo PromotionCombo)
        {
            this.Id = PromotionCombo.Id;
            this.Name = PromotionCombo.Name;
            this.PromotionPolicyId = PromotionCombo.PromotionPolicyId;
            this.PromotionId = PromotionCombo.PromotionId;
            this.Note = PromotionCombo.Note;
            this.PromotionDiscountTypeId = PromotionCombo.PromotionDiscountTypeId;
            this.DiscountPercentage = PromotionCombo.DiscountPercentage;
            this.DiscountValue = PromotionCombo.DiscountValue;
            this.Price = PromotionCombo.Price;
            this.PromotionDiscountType = PromotionCombo.PromotionDiscountType == null ? null : new Promotion_PromotionDiscountTypeDTO(PromotionCombo.PromotionDiscountType);
            this.PromotionPolicy = PromotionCombo.PromotionPolicy == null ? null : new Promotion_PromotionPolicyDTO(PromotionCombo.PromotionPolicy);
            this.PromotionComboInItemMappings = PromotionCombo.PromotionComboInItemMappings?.Select(x => new Promotion_PromotionComboInItemMappingDTO(x)).ToList();
            this.PromotionComboOutItemMappings = PromotionCombo.PromotionComboOutItemMappings?.Select(x => new Promotion_PromotionComboOutItemMappingDTO(x)).ToList();
            this.Errors = PromotionCombo.Errors;
        }
    }

    public class Promotion_PromotionComboFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter PromotionPolicyId { get; set; }

        public IdFilter PromotionId { get; set; }

        public StringFilter Note { get; set; }

        public IdFilter PromotionDiscountTypeId { get; set; }

        public DecimalFilter DiscountPercentage { get; set; }

        public DecimalFilter DiscountValue { get; set; }

        public DecimalFilter Price { get; set; }

        public PromotionComboOrder OrderBy { get; set; }
    }
}