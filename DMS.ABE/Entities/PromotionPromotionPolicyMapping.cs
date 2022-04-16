using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PromotionPromotionPolicyMapping : DataEntity, IEquatable<PromotionPromotionPolicyMapping>
    {
        public long PromotionId { get; set; }
        public long PromotionPolicyId { get; set; }
        public string Note { get; set; }
        public long StatusId { get; set; }
        public Promotion Promotion { get; set; }
        public PromotionPolicy PromotionPolicy { get; set; }
        public Status Status { get; set; }

        public bool Equals(PromotionPromotionPolicyMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionPromotionPolicyMappingFilter : FilterEntity
    {
        public IdFilter PromotionId { get; set; }
        public IdFilter PromotionPolicyId { get; set; }
        public StringFilter Note { get; set; }
        public IdFilter StatusId { get; set; }
        public List<PromotionPromotionPolicyMappingFilter> OrFilter { get; set; }
        public PromotionPromotionPolicyMappingOrder OrderBy { get; set; }
        public PromotionPromotionPolicyMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionPromotionPolicyMappingOrder
    {
        Promotion = 0,
        PromotionPolicy = 1,
        Note = 2,
        Status = 3,
    }

    [Flags]
    public enum PromotionPromotionPolicyMappingSelect : long
    {
        ALL = E.ALL,
        Promotion = E._0,
        PromotionPolicy = E._1,
        Note = E._2,
        Status = E._3,
    }
}
