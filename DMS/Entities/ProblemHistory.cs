using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class ProblemHistory : DataEntity, IEquatable<ProblemHistory>
    {
        public long Id { get; set; }
        public long ProblemId { get; set; }
        public DateTime Time { get; set; }
        public long ModifierId { get; set; }
        public long ProblemStatusId { get; set; }
        public AppUser Modifier { get; set; }
        public Problem Problem { get; set; }
        public ProblemStatus ProblemStatus { get; set; }

        public bool Equals(ProblemHistory other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ProblemHistoryFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter ProblemId { get; set; }
        public DateFilter Time { get; set; }
        public IdFilter ModifierId { get; set; }
        public IdFilter ProblemStatusId { get; set; }
        public List<ProblemHistoryFilter> OrFilter { get; set; }
        public ProblemHistoryOrder OrderBy { get; set; }
        public ProblemHistorySelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProblemHistoryOrder
    {
        Id = 0,
        Problem = 1,
        Time = 2,
        Modifier = 3,
        ProblemStatus = 4,
    }

    [Flags]
    public enum ProblemHistorySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Problem = E._1,
        Time = E._2,
        Modifier = E._3,
        ProblemStatus = E._4,
    }
}
