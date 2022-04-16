using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PromotionCodeHistory : DataEntity, IEquatable<PromotionCodeHistory>
    {
        public long Id { get; set; }
        public long PromotionCodeId { get; set; }
        public DateTime AppliedAt { get; set; }
        public Guid RowId { get; set; }
        public PromotionCode PromotionCode { get; set; }
        public DirectSalesOrder DirectSalesOrder { get; set; }
        public bool Equals(PromotionCodeHistory other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class PromotionCodeHistoryFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter PromotionCodeId { get; set; }
        public DateFilter AppliedAt { get; set; }
        public GuidFilter RowId { get; set; }
        public List<PromotionCodeHistoryFilter> OrFilter { get; set; }
        public PromotionCodeHistoryOrder OrderBy { get; set; }
        public PromotionCodeHistorySelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionCodeHistoryOrder
    {
        Id = 0,
        PromotionCode = 1,
        AppliedAt = 2,
        Row = 4,
    }

    [Flags]
    public enum PromotionCodeHistorySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        PromotionCode = E._1,
        AppliedAt = E._2,
        Row = E._4,
    }
}
