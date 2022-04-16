using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class PromotionCodeProductMapping : DataEntity, IEquatable<PromotionCodeProductMapping>
    {
        public long PromotionCodeId { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public PromotionCode PromotionCode { get; set; }

        public bool Equals(PromotionCodeProductMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionCodeProductMappingFilter : FilterEntity
    {
        public IdFilter PromotionCodeId { get; set; }
        public IdFilter ProductId { get; set; }
        public List<PromotionCodeProductMappingFilter> OrFilter { get; set; }
        public PromotionCodeProductMappingOrder OrderBy { get; set; }
        public PromotionCodeProductMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionCodeProductMappingOrder
    {
        PromotionCode = 0,
        Product = 1,
    }

    [Flags]
    public enum PromotionCodeProductMappingSelect : long
    {
        ALL = E.ALL,
        PromotionCode = E._0,
        Product = E._1,
    }
}
