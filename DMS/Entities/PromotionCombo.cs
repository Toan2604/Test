using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class PromotionCombo : DataEntity, IEquatable<PromotionCombo>
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
        public Promotion Promotion { get; set; }
        public PromotionDiscountType PromotionDiscountType { get; set; }
        public PromotionPolicy PromotionPolicy { get; set; }
        public Guid RowId { get; set; }
        public List<PromotionComboInItemMapping> PromotionComboInItemMappings { get; set; }
        public List<PromotionComboOutItemMapping> PromotionComboOutItemMappings { get; set; }
        public bool Equals(PromotionCombo other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class PromotionComboFilter : FilterEntity
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
        public List<PromotionComboFilter> OrFilter { get; set; }
        public PromotionComboOrder OrderBy { get; set; }
        public PromotionComboSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionComboOrder
    {
        Id = 0,
        Name = 1,
        PromotionPolicy = 2,
        Promotion = 3,
        Note = 4,
        PromotionDiscountType = 5,
        DiscountPercentage = 6,
        DiscountValue = 7,
        Price = 8,
    }

    [Flags]
    public enum PromotionComboSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Name = E._1,
        PromotionPolicy = E._2,
        Promotion = E._3,
        Note = E._4,
        PromotionDiscountType = E._5,
        DiscountPercentage = E._6,
        DiscountValue = E._7,
        Price = E._8,
    }
}
