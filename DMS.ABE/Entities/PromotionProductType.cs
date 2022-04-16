using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PromotionProductType : DataEntity, IEquatable<PromotionProductType>
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
        public Guid RowId { get; set; }
        public ProductType ProductType { get; set; }
        public Promotion Promotion { get; set; }
        public PromotionDiscountType PromotionDiscountType { get; set; }
        public PromotionPolicy PromotionPolicy { get; set; }
        public List<PromotionProductTypeItemMapping> PromotionProductTypeItemMappings { get; set; }

        public bool Equals(PromotionProductType other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class PromotionProductTypeFilter : FilterEntity
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
        public List<PromotionProductTypeFilter> OrFilter { get; set; }
        public PromotionProductTypeOrder OrderBy { get; set; }
        public PromotionProductTypeSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionProductTypeOrder
    {
        Id = 0,
        PromotionPolicy = 1,
        Promotion = 2,
        ProductType = 3,
        Note = 4,
        FromValue = 5,
        ToValue = 6,
        PromotionDiscountType = 7,
        DiscountPercentage = 8,
        DiscountValue = 9,
        Price = 10,
    }

    [Flags]
    public enum PromotionProductTypeSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        PromotionPolicy = E._1,
        Promotion = E._2,
        ProductType = E._3,
        Note = E._4,
        FromValue = E._5,
        ToValue = E._6,
        PromotionDiscountType = E._7,
        DiscountPercentage = E._8,
        DiscountValue = E._9,
        Price = E._10,
    }
}
