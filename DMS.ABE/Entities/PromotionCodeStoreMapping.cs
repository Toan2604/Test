using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PromotionCodeStoreMapping : DataEntity, IEquatable<PromotionCodeStoreMapping>
    {
        public long PromotionCodeId { get; set; }
        public long StoreId { get; set; }
        public PromotionCode PromotionCode { get; set; }
        public Store Store { get; set; }

        public bool Equals(PromotionCodeStoreMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionCodeStoreMappingFilter : FilterEntity
    {
        public IdFilter PromotionCodeId { get; set; }
        public IdFilter StoreId { get; set; }
        public List<PromotionCodeStoreMappingFilter> OrFilter { get; set; }
        public PromotionCodeStoreMappingOrder OrderBy { get; set; }
        public PromotionCodeStoreMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionCodeStoreMappingOrder
    {
        PromotionCode = 0,
        Store = 1,
    }

    [Flags]
    public enum PromotionCodeStoreMappingSelect : long
    {
        ALL = E.ALL,
        PromotionCode = E._0,
        Store = E._1,
    }
}
