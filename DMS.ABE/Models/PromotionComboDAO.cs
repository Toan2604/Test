using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class PromotionComboDAO
    {
        public PromotionComboDAO()
        {
            PromotionComboInItemMappings = new HashSet<PromotionComboInItemMappingDAO>();
            PromotionComboOutItemMappings = new HashSet<PromotionComboOutItemMappingDAO>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long PromotionPolicyId { get; set; }
        public long PromotionId { get; set; }
        public string Note { get; set; }
        public long PromotionDiscountTypeId { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? Price { get; set; }
        public Guid RowId { get; set; }

        public virtual PromotionDAO Promotion { get; set; }
        public virtual PromotionDiscountTypeDAO PromotionDiscountType { get; set; }
        public virtual PromotionPolicyDAO PromotionPolicy { get; set; }
        public virtual ICollection<PromotionComboInItemMappingDAO> PromotionComboInItemMappings { get; set; }
        public virtual ICollection<PromotionComboOutItemMappingDAO> PromotionComboOutItemMappings { get; set; }
    }
}
