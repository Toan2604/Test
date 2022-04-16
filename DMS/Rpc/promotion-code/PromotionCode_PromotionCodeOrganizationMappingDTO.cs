using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion_code
{
    public class PromotionCode_PromotionCodeOrganizationMappingDTO : DataDTO
    {
        public long PromotionCodeId { get; set; }
        public long OrganizationId { get; set; }
        public PromotionCode_OrganizationDTO Organization { get; set; }

        public PromotionCode_PromotionCodeOrganizationMappingDTO() { }
        public PromotionCode_PromotionCodeOrganizationMappingDTO(PromotionCodeOrganizationMapping PromotionCodeOrganizationMapping)
        {
            this.PromotionCodeId = PromotionCodeOrganizationMapping.PromotionCodeId;
            this.OrganizationId = PromotionCodeOrganizationMapping.OrganizationId;
            this.Organization = PromotionCodeOrganizationMapping.Organization == null ? null : new PromotionCode_OrganizationDTO(PromotionCodeOrganizationMapping.Organization);
            this.Errors = PromotionCodeOrganizationMapping.Errors;
        }
    }

    public class PromotionCode_PromotionCodeOrganizationMappingFilterDTO : FilterDTO
    {

        public IdFilter PromotionCodeId { get; set; }

        public IdFilter OrganizationId { get; set; }

        public PromotionCodeOrganizationMappingOrder OrderBy { get; set; }
    }
}