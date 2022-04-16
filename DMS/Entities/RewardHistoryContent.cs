using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class RewardHistoryContent : DataEntity, IEquatable<RewardHistoryContent>
    {
        public long Id { get; set; }
        public long RewardHistoryId { get; set; }
        public long LuckyNumberId { get; set; }
        public LuckyNumber LuckyNumber { get; set; }
        public RewardHistory RewardHistory { get; set; }

        public bool Equals(RewardHistoryContent other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class RewardHistoryContentFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter RewardHistoryId { get; set; }
        public IdFilter LuckyNumberId { get; set; }
        public List<RewardHistoryContentFilter> OrFilter { get; set; }
        public RewardHistoryContentOrder OrderBy { get; set; }
        public RewardHistoryContentSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RewardHistoryContentOrder
    {
        Id = 0,
        RewardHistory = 1,
        LuckyNumber = 2,
    }

    [Flags]
    public enum RewardHistoryContentSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        RewardHistory = E._1,
        LuckyNumber = E._2,
    }
}
