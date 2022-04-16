using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PromotionStoreGroupingMapping : DataEntity, IEquatable<PromotionStoreGroupingMapping>
    {
        public long PromotionId { get; set; }
        public long StoreGroupingId { get; set; }
        public Promotion Promotion { get; set; }
        public StoreGrouping StoreGrouping { get; set; }

        public bool Equals(PromotionStoreGroupingMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionStoreGroupingMappingFilter : FilterEntity
    {
        public IdFilter PromotionId { get; set; }
        public IdFilter StoreGroupingId { get; set; }
        public List<PromotionStoreGroupingMappingFilter> OrFilter { get; set; }
        public PromotionStoreGroupingMappingOrder OrderBy { get; set; }
        public PromotionStoreGroupingMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionStoreGroupingMappingOrder
    {
        Promotion = 0,
        StoreGrouping = 1,
    }

    [Flags]
    public enum PromotionStoreGroupingMappingSelect : long
    {
        ALL = E.ALL,
        Promotion = E._0,
        StoreGrouping = E._1,
    }
}
