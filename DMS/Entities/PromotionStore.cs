using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class PromotionStore : DataEntity, IEquatable<PromotionStore>
    {
        public long Id { get; set; }
        public long PromotionPolicyId { get; set; }
        public long PromotionId { get; set; }
        public string Note { get; set; }
        public decimal FromValue { get; set; }
        public decimal ToValue { get; set; }
        public long PromotionDiscountTypeId { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? Price { get; set; }
        public Guid RowId { get; set; }
        public Promotion Promotion { get; set; }
        public PromotionDiscountType PromotionDiscountType { get; set; }
        public PromotionPolicy PromotionPolicy { get; set; }
        public List<PromotionStoreItemMapping> PromotionStoreItemMappings { get; set; }

        public bool Equals(PromotionStore other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class PromotionStoreFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter PromotionPolicyId { get; set; }
        public IdFilter PromotionId { get; set; }
        public StringFilter Note { get; set; }
        public DecimalFilter FromValue { get; set; }
        public DecimalFilter ToValue { get; set; }
        public IdFilter PromotionDiscountTypeId { get; set; }
        public DecimalFilter DiscountPercentage { get; set; }
        public DecimalFilter DiscountValue { get; set; }
        public DecimalFilter Price { get; set; }
        public List<PromotionStoreFilter> OrFilter { get; set; }
        public PromotionStoreOrder OrderBy { get; set; }
        public PromotionStoreSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionStoreOrder
    {
        Id = 0,
        PromotionPolicy = 1,
        Promotion = 2,
        Note = 3,
        FromValue = 4,
        ToValue = 5,
        PromotionDiscountType = 6,
        DiscountPercentage = 7,
        DiscountValue = 8,
        Price = 9,
    }

    [Flags]
    public enum PromotionStoreSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        PromotionPolicy = E._1,
        Promotion = E._2,
        Note = E._3,
        FromValue = E._4,
        ToValue = E._5,
        PromotionDiscountType = E._6,
        DiscountPercentage = E._7,
        DiscountValue = E._8,
        Price = E._9,
    }
}
