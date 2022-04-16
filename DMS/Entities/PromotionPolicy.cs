using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class PromotionPolicy : DataEntity, IEquatable<PromotionPolicy>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public List<PromotionDirectSalesOrder> PromotionDirectSalesOrders { get; set; }
        public List<PromotionStore> PromotionStores { get; set; }
        public List<PromotionStoreGrouping> PromotionStoreGroupings { get; set; }
        public List<PromotionStoreType> PromotionStoreTypes { get; set; }
        public List<PromotionProduct> PromotionProducts { get; set; }
        public List<PromotionProductGrouping> PromotionProductGroupings { get; set; }
        public List<PromotionProductType> PromotionProductTypes { get; set; }
        public List<PromotionSamePrice> PromotionSamePrices { get; set; }
        public List<PromotionCombo> PromotionCombos { get; set; }

        public List<PromotionPromotionPolicyMapping> PromotionPromotionPolicyMappings { get; set; }
        public bool Equals(PromotionPolicy other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class PromotionPolicyFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public List<PromotionPolicyFilter> OrFilter { get; set; }
        public PromotionPolicyOrder OrderBy { get; set; }
        public PromotionPolicySelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionPolicyOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
    }

    [Flags]
    public enum PromotionPolicySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
    }
}
