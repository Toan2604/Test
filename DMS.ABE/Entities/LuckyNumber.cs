using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class LuckyNumber : DataEntity, IEquatable<LuckyNumber>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public long LuckyNumberGroupingId { get; set; }
        public long RewardStatusId { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }
        public LuckyNumberGrouping LuckyNumberGrouping { get; set; }
        public RewardStatus RewardStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? UsedAt { get; set; }

        public bool Equals(LuckyNumber other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class LuckyNumberFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Value { get; set; }
        public IdFilter LuckyNumberGroupingId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter RewardStatusId { get; set; }
        public GuidFilter RowId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public DateFilter UsedAt { get; set; }
        public List<LuckyNumberFilter> OrFilter { get; set; }
        public LuckyNumberOrder OrderBy { get; set; }
        public LuckyNumberSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LuckyNumberOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        RewardStatus = 3,
        Value = 4,
        LuckyNumberGrouping = 5,
        Row = 7,
        CreatedAt = 50,
        UpdatedAt = 51,
        UsedAt = 52,
    }

    [Flags]
    public enum LuckyNumberSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        RewardStatus = E._3,
        Value = E._4,
        LuckyNumberGrouping = E._5,
        Row = E._7,
    }
}
