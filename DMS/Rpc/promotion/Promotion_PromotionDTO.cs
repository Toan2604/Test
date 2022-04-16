using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long OrganizationId { get; set; }
        public long PromotionTypeId { get; set; }
        public string Note { get; set; }
        public long Priority { get; set; }
        public long StatusId { get; set; }
        public Promotion_OrganizationDTO Organization { get; set; }
        public Promotion_PromotionTypeDTO PromotionType { get; set; }
        public Promotion_StatusDTO Status { get; set; }

        public List<Promotion_PromotionPromotionPolicyMappingDTO> PromotionPromotionPolicyMappings { get; set; }
        public List<Promotion_PromotionStoreMappingDTO> PromotionStoreMappings { get; set; }
        public List<Promotion_PromotionStoreGroupingMappingDTO> PromotionStoreGroupingMappings { get; set; }
        public List<Promotion_PromotionStoreTypeMappingDTO> PromotionStoreTypeMappings { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Promotion_PromotionDTO() { }
        public Promotion_PromotionDTO(Promotion Promotion)
        {
            this.Id = Promotion.Id;
            this.Code = Promotion.Code;
            this.Name = Promotion.Name;
            this.StartDate = Promotion.StartDate;
            this.EndDate = Promotion.EndDate;
            this.OrganizationId = Promotion.OrganizationId;
            this.PromotionTypeId = Promotion.PromotionTypeId;
            this.Note = Promotion.Note;
            this.Priority = Promotion.Priority;
            this.StatusId = Promotion.StatusId;
            this.Organization = Promotion.Organization == null ? null : new Promotion_OrganizationDTO(Promotion.Organization);
            this.PromotionType = Promotion.PromotionType == null ? null : new Promotion_PromotionTypeDTO(Promotion.PromotionType);
            this.Status = Promotion.Status == null ? null : new Promotion_StatusDTO(Promotion.Status);
            this.PromotionPromotionPolicyMappings = Promotion.PromotionPromotionPolicyMappings?.Select(x => new Promotion_PromotionPromotionPolicyMappingDTO(x)).ToList();
            this.PromotionStoreGroupingMappings = Promotion.PromotionStoreGroupingMappings?.Select(x => new Promotion_PromotionStoreGroupingMappingDTO(x)).ToList();
            this.PromotionStoreMappings = Promotion.PromotionStoreMappings?.Select(x => new Promotion_PromotionStoreMappingDTO(x)).ToList();
            this.PromotionStoreTypeMappings = Promotion.PromotionStoreTypeMappings?.Select(x => new Promotion_PromotionStoreTypeMappingDTO(x)).ToList();

            this.CreatedAt = Promotion.CreatedAt;
            this.UpdatedAt = Promotion.UpdatedAt;
            this.Errors = Promotion.Errors;
        }
    }

    public class Promotion_PromotionFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public DateFilter StartDate { get; set; }
        public DateFilter EndDate { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter PromotionTypeId { get; set; }
        public StringFilter Note { get; set; }
        public LongFilter Priority { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public PromotionOrder OrderBy { get; set; }
    }
}
