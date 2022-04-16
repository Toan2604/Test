using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class RewardHistory : DataEntity, IEquatable<RewardHistory>
    {
        public long Id { get; set; }
        public long AppUserId { get; set; }
        public long StoreId { get; set; }
        public long TurnCounter { get; set; }
        public decimal Revenue { get; set; }
        public Guid RowId { get; set; }
        public AppUser AppUser { get; set; }
        public Store Store { get; set; }
        public List<RewardHistoryContent> RewardHistoryContents { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool Equals(RewardHistory other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class RewardHistoryFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter StoreId { get; set; }
        public LongFilter TurnCounter { get; set; }
        public DecimalFilter Revenue { get; set; }
        public GuidFilter RowId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public string Search { get; set; }
        public List<RewardHistoryFilter> OrFilter { get; set; }
        public RewardHistoryOrder OrderBy { get; set; }
        public RewardHistorySelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RewardHistoryOrder
    {
        Id = 0,
        AppUser = 1,
        Store = 2,
        TurnCounter = 3,
        Revenue = 4,
        Row = 7,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum RewardHistorySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        AppUser = E._1,
        Store = E._2,
        TurnCounter = E._3,
        Revenue = E._4,
        Row = E._7,
    }
}
