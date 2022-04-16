using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.promotion_code
{
    public class PromotionCode_PromotionCodeDTO : DataDTO
    {
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
        public PromotionCode_OrganizationDTO Organization { get; set; }
        public PromotionCode_PromotionDiscountTypeDTO PromotionDiscountType { get; set; }
        public PromotionCode_PromotionProductAppliedTypeDTO PromotionProductAppliedType { get; set; }
        public PromotionCode_PromotionTypeDTO PromotionType { get; set; }
        public PromotionCode_StatusDTO Status { get; set; }
        public List<PromotionCode_PromotionCodeHistoryDTO> PromotionCodeHistories { get; set; }
        public List<PromotionCode_PromotionCodeOrganizationMappingDTO> PromotionCodeOrganizationMappings { get; set; }
        public List<PromotionCode_PromotionCodeProductMappingDTO> PromotionCodeProductMappings { get; set; }
        public List<PromotionCode_PromotionCodeStoreMappingDTO> PromotionCodeStoreMappings { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Used { get; set; }
        public long SalesOrderApplied => PromotionCodeHistories == null ? 0 : PromotionCodeHistories.Count();
        public PromotionCode_PromotionCodeDTO() { }
        public PromotionCode_PromotionCodeDTO(PromotionCode PromotionCode)
        {
            this.Id = PromotionCode.Id;
            this.Code = PromotionCode.Code;
            this.Name = PromotionCode.Name;
            this.Quantity = PromotionCode.Quantity;
            this.PromotionDiscountTypeId = PromotionCode.PromotionDiscountTypeId;
            this.Value = PromotionCode.Value;
            this.MaxValue = PromotionCode.MaxValue;
            this.PromotionTypeId = PromotionCode.PromotionTypeId;
            this.PromotionProductAppliedTypeId = PromotionCode.PromotionProductAppliedTypeId;
            this.OrganizationId = PromotionCode.OrganizationId;
            this.StartDate = PromotionCode.StartDate;
            this.EndDate = PromotionCode.EndDate;
            this.StatusId = PromotionCode.StatusId;
            this.Organization = PromotionCode.Organization == null ? null : new PromotionCode_OrganizationDTO(PromotionCode.Organization);
            this.PromotionDiscountType = PromotionCode.PromotionDiscountType == null ? null : new PromotionCode_PromotionDiscountTypeDTO(PromotionCode.PromotionDiscountType);
            this.PromotionProductAppliedType = PromotionCode.PromotionProductAppliedType == null ? null : new PromotionCode_PromotionProductAppliedTypeDTO(PromotionCode.PromotionProductAppliedType);
            this.PromotionType = PromotionCode.PromotionType == null ? null : new PromotionCode_PromotionTypeDTO(PromotionCode.PromotionType);
            this.Status = PromotionCode.Status == null ? null : new PromotionCode_StatusDTO(PromotionCode.Status);
            this.PromotionCodeHistories = PromotionCode.PromotionCodeHistories?.Select(x => new PromotionCode_PromotionCodeHistoryDTO(x)).ToList();
            this.PromotionCodeOrganizationMappings = PromotionCode.PromotionCodeOrganizationMappings?.Select(x => new PromotionCode_PromotionCodeOrganizationMappingDTO(x)).ToList();
            this.PromotionCodeProductMappings = PromotionCode.PromotionCodeProductMappings?.Select(x => new PromotionCode_PromotionCodeProductMappingDTO(x)).ToList();
            this.PromotionCodeStoreMappings = PromotionCode.PromotionCodeStoreMappings?.Select(x => new PromotionCode_PromotionCodeStoreMappingDTO(x)).ToList();
            this.CreatedAt = PromotionCode.CreatedAt;
            this.UpdatedAt = PromotionCode.UpdatedAt;
            this.Used = PromotionCode.Used;
            this.Errors = PromotionCode.Errors;
        }
    }

    public class PromotionCode_PromotionCodeFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public LongFilter Quantity { get; set; }
        public IdFilter PromotionDiscountTypeId { get; set; }
        public DecimalFilter Value { get; set; }
        public DecimalFilter MaxValue { get; set; }
        public IdFilter PromotionTypeId { get; set; }
        public IdFilter PromotionProductAppliedTypeId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public DateFilter StartDate { get; set; }
        public DateFilter EndDate { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public PromotionCodeOrder OrderBy { get; set; }
    }
}
