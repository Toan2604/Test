using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class LuckyNumberGrouping : DataEntity, IEquatable<LuckyNumberGrouping>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long OrganizationId { get; set; }
        public long StatusId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Organization Organization { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<LuckyNumber> LuckyNumbers { get; set; }
        public bool Equals(LuckyNumberGrouping other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class LuckyNumberGroupingFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter StartDate { get; set; }
        public DateFilter EndDate { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<LuckyNumberGroupingFilter> OrFilter { get; set; }
        public LuckyNumberGroupingOrder OrderBy { get; set; }
        public LuckyNumberGroupingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LuckyNumberGroupingOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Organization = 3,
        Status = 4,
        StartDate = 5,
        EndDate = 6,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum LuckyNumberGroupingSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Organization = E._3,
        Status = E._4,
        StartDate = E._5,
        EndDate = E._6,
    }
}
