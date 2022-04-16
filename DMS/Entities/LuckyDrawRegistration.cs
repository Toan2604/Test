using TrueSight.Common;
using System;
using System.Collections.Generic;
using DMS.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DMS.Entities
{
    public class LuckyDrawRegistration : DataEntity, IEquatable<LuckyDrawRegistration>
    {
        public long Id { get; set; }
        public long LuckyDrawId { get; set; }
        public long AppUserId { get; set; }
        public long StoreId { get; set; }
        public decimal Revenue { get; set; }
        public long TurnCounter { get; set; }
        public long RemainingTurnCounter { get; set; }
        public long UsedTurnCounter { get; set; }
        public bool IsDrawnByStore { get; set; }
        public DateTime Time { get; set; }
        public AppUser AppUser { get; set; }
        public LuckyDraw LuckyDraw { get; set; }
        public Store Store { get; set; }
        public List<LuckyDrawWinner> LuckyDrawWinners { get; set; }

        public bool Equals(LuckyDrawRegistration other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.LuckyDrawId != other.LuckyDrawId) return false;
            if (this.AppUserId != other.AppUserId) return false;
            if (this.StoreId != other.StoreId) return false;
            if (this.Revenue != other.Revenue) return false;
            if (this.TurnCounter != other.TurnCounter) return false;
            if (this.RemainingTurnCounter != other.RemainingTurnCounter) return false;
            if (this.IsDrawnByStore != other.IsDrawnByStore) return false;
            if (this.Time != other.Time) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class LuckyDrawRegistrationFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter LuckyDrawId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter StoreId { get; set; }
        public DecimalFilter Revenue { get; set; }
        public LongFilter TurnCounter { get; set; }
        public LongFilter RemainingTurnCounter { get; set; }
        public bool? IsDrawnByStore { get; set; }
        public string Search { get; set; }
        public DateFilter Time { get; set; }
        public List<LuckyDrawRegistrationFilter> OrFilter { get; set; }
        public LuckyDrawRegistrationOrder OrderBy { get; set; }
        public LuckyDrawRegistrationSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LuckyDrawRegistrationOrder
    {
        Id = 0,
        LuckyDraw = 1,
        AppUser = 2,
        Store = 3,
        Revenue = 4,
        TurnCounter = 5,
        RemainingTurnCounter = 6,
        IsDrawnByStore = 7,
        Time = 8,
    }

    [Flags]
    public enum LuckyDrawRegistrationSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        LuckyDraw = E._1,
        AppUser = E._2,
        Store = E._3,
        Revenue = E._4,
        TurnCounter = E._5,
        RemainingTurnCounter = E._6,
        IsDrawnByStore = E._7,
        Time = E._8,
    }
}
