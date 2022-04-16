using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class PromotionCodeDAO
    {
        public PromotionCodeDAO()
        {
            PromotionCodeHistories = new HashSet<PromotionCodeHistoryDAO>();
            PromotionCodeOrganizationMappings = new HashSet<PromotionCodeOrganizationMappingDAO>();
            PromotionCodeProductMappings = new HashSet<PromotionCodeProductMappingDAO>();
            PromotionCodeStoreMappings = new HashSet<PromotionCodeStoreMappingDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? Quantity { get; set; }
        public long PromotionDiscountTypeId { get; set; }
        public decimal Value { get; set; }
        public decimal? MaxValue { get; set; }
        public long PromotionTypeId { get; set; }
        public long PromotionProductAppliedTypeId { get; set; }
        public long OrganizationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }

        public virtual OrganizationDAO Organization { get; set; }
        public virtual PromotionDiscountTypeDAO PromotionDiscountType { get; set; }
        public virtual PromotionProductAppliedTypeDAO PromotionProductAppliedType { get; set; }
        public virtual PromotionTypeDAO PromotionType { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<PromotionCodeHistoryDAO> PromotionCodeHistories { get; set; }
        public virtual ICollection<PromotionCodeOrganizationMappingDAO> PromotionCodeOrganizationMappings { get; set; }
        public virtual ICollection<PromotionCodeProductMappingDAO> PromotionCodeProductMappings { get; set; }
        public virtual ICollection<PromotionCodeStoreMappingDAO> PromotionCodeStoreMappings { get; set; }
    }
}
