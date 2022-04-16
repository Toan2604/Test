using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PromotionCodeOrganizationMapping : DataEntity, IEquatable<PromotionCodeOrganizationMapping>
    {
        public long PromotionCodeId { get; set; }
        public long OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public PromotionCode PromotionCode { get; set; }

        public bool Equals(PromotionCodeOrganizationMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionCodeOrganizationMappingFilter : FilterEntity
    {
        public IdFilter PromotionCodeId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public List<PromotionCodeOrganizationMappingFilter> OrFilter { get; set; }
        public PromotionCodeOrganizationMappingOrder OrderBy { get; set; }
        public PromotionCodeOrganizationMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionCodeOrganizationMappingOrder
    {
        PromotionCode = 0,
        Organization = 1,
    }

    [Flags]
    public enum PromotionCodeOrganizationMappingSelect : long
    {
        ALL = E.ALL,
        PromotionCode = E._0,
        Organization = E._1,
    }
}
