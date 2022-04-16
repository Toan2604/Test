using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PromotionStoreMapping : DataEntity, IEquatable<PromotionStoreMapping>
    {
        public long PromotionId { get; set; }
        public long StoreId { get; set; }
        public Promotion Promotion { get; set; }
        public Store Store { get; set; }

        public bool Equals(PromotionStoreMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionStoreMappingFilter : FilterEntity
    {
        public IdFilter PromotionId { get; set; }
        public IdFilter StoreId { get; set; }
        public List<PromotionStoreMappingFilter> OrFilter { get; set; }
        public PromotionStoreMappingOrder OrderBy { get; set; }
        public PromotionStoreMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionStoreMappingOrder
    {
        Promotion = 0,
        Store = 1,
    }

    [Flags]
    public enum PromotionStoreMappingSelect : long
    {
        ALL = E.ALL,
        Promotion = E._0,
        Store = E._1,
    }
}
