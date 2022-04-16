using TrueSight.Common;
using System;
using System.Collections.Generic;
using DMS.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DMS.Entities
{
    public class LuckyDrawWinner : DataEntity, IEquatable<LuckyDrawWinner>
    {
        public long Id { get; set; }
        public long LuckyDrawId { get; set; }
        public long? LuckyDrawStructureId { get; set; }
        public long LuckyDrawRegistrationId { get; set; }
        public long? LuckyDrawNumberId { get; set; }
        public DateTime Time { get; set; }
        public LuckyDraw LuckyDraw { get; set; }
        public LuckyDrawRegistration LuckyDrawRegistration { get; set; }
        public LuckyDrawStructure LuckyDrawStructure { get; set; }
        public LuckyDrawNumber LuckyDrawNumber { get; set; }
        public Guid RowId { get; set; }

        public bool Equals(LuckyDrawWinner other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.LuckyDrawId != other.LuckyDrawId) return false;
            if (this.LuckyDrawStructureId != other.LuckyDrawStructureId) return false;
            if (this.LuckyDrawRegistrationId != other.LuckyDrawRegistrationId) return false;
            if (this.LuckyDrawNumberId != other.LuckyDrawNumberId) return false;
            if (this.Time != other.Time) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class LuckyDrawWinnerFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter LuckyDrawId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter LuckyDrawStructureId { get; set; }
        public IdFilter LuckyDrawRegistrationId { get; set; }
        public IdFilter LuckyDrawNumberId { get; set; }
        public DateFilter Time { get; set; }
        public bool? IsDrawnByStore { get; set; }
        public bool? Used { get; set; }
        public List<LuckyDrawWinnerFilter> OrFilter { get; set; }
        public LuckyDrawWinnerOrder OrderBy { get; set; }
        public LuckyDrawWinnerSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LuckyDrawWinnerOrder
    {
        Id = 0,
        LuckyDraw = 1,
        LuckyDrawStructure = 2,
        LuckyDrawRegistration = 3,
        Time = 4,
        Store = 5,
        LuckyDrawNumber = 6,
    }

    [Flags]
    public enum LuckyDrawWinnerSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        LuckyDraw = E._1,
        LuckyDrawStructure = E._2,
        LuckyDrawRegistration = E._3,
        Time = E._4,
        LuckyDrawNumber = E._5,
    }
}
