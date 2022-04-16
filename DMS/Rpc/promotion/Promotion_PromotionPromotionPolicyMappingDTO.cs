using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionPromotionPolicyMappingDTO : DataDTO
    {
        public long PromotionId { get; set; }
        public long PromotionPolicyId { get; set; }
        public string Note { get; set; }
        public long StatusId { get; set; }
        public Promotion_PromotionPolicyDTO PromotionPolicy { get; set; }
        public Promotion_StatusDTO Status { get; set; }

        public Promotion_PromotionPromotionPolicyMappingDTO() { }
        public Promotion_PromotionPromotionPolicyMappingDTO(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            this.PromotionId = PromotionPromotionPolicyMapping.PromotionId;
            this.PromotionPolicyId = PromotionPromotionPolicyMapping.PromotionPolicyId;
            this.Note = PromotionPromotionPolicyMapping.Note;
            this.StatusId = PromotionPromotionPolicyMapping.StatusId;
            this.PromotionPolicy = PromotionPromotionPolicyMapping.PromotionPolicy == null ? null : new Promotion_PromotionPolicyDTO(PromotionPromotionPolicyMapping.PromotionPolicy);
            this.Status = PromotionPromotionPolicyMapping.Status == null ? null : new Promotion_StatusDTO(PromotionPromotionPolicyMapping.Status);
            this.Errors = PromotionPromotionPolicyMapping.Errors;
        }
    }

    public class Promotion_PromotionPromotionPolicyMappingFilterDTO : FilterDTO
    {

        public IdFilter PromotionId { get; set; }

        public IdFilter PromotionPolicyId { get; set; }

        public StringFilter Note { get; set; }

        public IdFilter StatusId { get; set; }

        public PromotionPromotionPolicyMappingOrder OrderBy { get; set; }
    }
}