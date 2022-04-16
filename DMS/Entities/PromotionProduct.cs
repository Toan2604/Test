using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class PromotionProduct : DataEntity, IEquatable<PromotionProduct>
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
        public Guid RowId { get; set; }
        public Product Product { get; set; }
        public Promotion Promotion { get; set; }
        public PromotionDiscountType PromotionDiscountType { get; set; }
        public PromotionPolicy PromotionPolicy { get; set; }
        public List<PromotionProductItemMapping> PromotionProductItemMappings { get; set; }

        public bool Equals(PromotionProduct other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class PromotionProductFilter : FilterEntity
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
        public List<PromotionProductFilter> OrFilter { get; set; }
        public PromotionProductOrder OrderBy { get; set; }
        public PromotionProductSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionProductOrder
    {
        Id = 0,
        PromotionPolicy = 1,
        Promotion = 2,
        Product = 3,
        Note = 4,
        FromValue = 5,
        ToValue = 6,
        PromotionDiscountType = 7,
        DiscountPercentage = 8,
        DiscountValue = 9,
        Price = 10,
    }

    [Flags]
    public enum PromotionProductSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        PromotionPolicy = E._1,
        Promotion = E._2,
        Product = E._3,
        Note = E._4,
        FromValue = E._5,
        ToValue = E._6,
        PromotionDiscountType = E._7,
        DiscountPercentage = E._8,
        DiscountValue = E._9,
        Price = E._10,
    }
}
