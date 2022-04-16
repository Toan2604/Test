using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class Problem : DataEntity, IEquatable<Problem>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public long? StoreCheckingId { get; set; }
        public long StoreId { get; set; }
        public long CreatorId { get; set; }
        public long ProblemTypeId { get; set; }
        public DateTime NoteAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Content { get; set; }
        public long ProblemStatusId { get; set; }
        public Guid RowId { get; set; }
        public AppUser Creator { get; set; }
        public ProblemStatus ProblemStatus { get; set; }
        public ProblemType ProblemType { get; set; }
        public Store Store { get; set; }
        public StoreChecking StoreChecking { get; set; }
        public List<ProblemImageMapping> ProblemImageMappings { get; set; }
        public List<ProblemHistory> ProblemHistories { get; set; }
        public bool Equals(Problem other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ProblemFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter StoreCheckingId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter ProblemTypeId { get; set; }
        public DateFilter NoteAt { get; set; }
        public DateFilter CompletedAt { get; set; }
        public StringFilter Content { get; set; }
        public IdFilter ProblemStatusId { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        public List<ProblemFilter> OrFilter { get; set; }
        public ProblemOrder OrderBy { get; set; }
        public ProblemSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProblemOrder
    {
        Id = 0,
        StoreChecking = 1,
        Store = 2,
        Creator = 3,
        ProblemType = 4,
        NoteAt = 5,
        CompletedAt = 6,
        Content = 7,
        ProblemStatus = 8,
        Code = 9,
        Organization = 10
    }

    [Flags]
    public enum ProblemSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        StoreChecking = E._1,
        Store = E._2,
        Creator = E._3,
        ProblemType = E._4,
        NoteAt = E._5,
        CompletedAt = E._6,
        Content = E._7,
        ProblemStatus = E._8,
        Code = E._9,
    }
}
