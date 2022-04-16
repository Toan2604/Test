using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class PromotionStoreTypeMapping : DataEntity, IEquatable<PromotionStoreTypeMapping>
    {
        public long PromotionId { get; set; }
        public long StoreTypeId { get; set; }
        public Promotion Promotion { get; set; }
        public StoreType StoreType { get; set; }

        public bool Equals(PromotionStoreTypeMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionStoreTypeMappingFilter : FilterEntity
    {
        public IdFilter PromotionId { get; set; }
        public IdFilter StoreTypeId { get; set; }
        public List<PromotionStoreTypeMappingFilter> OrFilter { get; set; }
        public PromotionStoreTypeMappingOrder OrderBy { get; set; }
        public PromotionStoreTypeMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionStoreTypeMappingOrder
    {
        Promotion = 0,
        StoreType = 1,
    }

    [Flags]
    public enum PromotionStoreTypeMappingSelect : long
    {
        ALL = E.ALL,
        Promotion = E._0,
        StoreType = E._1,
    }
}
