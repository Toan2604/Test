using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class PromotionCode : DataEntity, IEquatable<PromotionCode>
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
        public Organization Organization { get; set; }
        public PromotionDiscountType PromotionDiscountType { get; set; }
        public PromotionProductAppliedType PromotionProductAppliedType { get; set; }
        public PromotionType PromotionType { get; set; }
        public Status Status { get; set; }
        public List<PromotionCodeHistory> PromotionCodeHistories { get; set; }
        public List<PromotionCodeOrganizationMapping> PromotionCodeOrganizationMappings { get; set; }
        public List<PromotionCodeProductMapping> PromotionCodeProductMappings { get; set; }
        public List<PromotionCodeStoreMapping> PromotionCodeStoreMappings { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Used { get; set; }


        public bool Equals(PromotionCode other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class PromotionCodeFilter : FilterEntity
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
        public List<PromotionCodeFilter> OrFilter { get; set; }
        public PromotionCodeOrder OrderBy { get; set; }
        public PromotionCodeSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionCodeOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Quantity = 3,
        PromotionDiscountType = 4,
        Value = 5,
        MaxValue = 6,
        PromotionType = 7,
        PromotionProductAppliedType = 8,
        Organization = 9,
        StartDate = 10,
        EndDate = 11,
        Status = 12,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum PromotionCodeSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Quantity = E._3,
        PromotionDiscountType = E._4,
        Value = E._5,
        MaxValue = E._6,
        PromotionType = E._7,
        PromotionProductAppliedType = E._8,
        Organization = E._9,
        StartDate = E._10,
        EndDate = E._11,
        Status = E._12,
    }
}
