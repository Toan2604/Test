using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class KpiItem : DataEntity, IEquatable<KpiItem>
    {
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long KpiYearId { get; set; }
        public long KpiPeriodId { get; set; }
        public long KpiItemTypeId { get; set; }
        public long StatusId { get; set; }
        public long EmployeeId { get; set; }
        public long CreatorId { get; set; }
        public bool ReadOnly { get; set; }
        public AppUser Creator { get; set; }
        public AppUser Employee { get; set; }
        public KpiYear KpiYear { get; set; }
        public KpiPeriod KpiPeriod { get; set; }
        public KpiItemType KpiItemType { get; set; }
        public Organization Organization { get; set; }
        public Status Status { get; set; }
        public List<KpiItemContent> KpiItemContents { get; set; }
        public List<AppUser> Employees { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid RowId { get; set; }
        public bool Equals(KpiItem other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class KpiItemFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter KpiYearId { get; set; }
        public IdFilter KpiPeriodId { get; set; }
        public IdFilter KpiItemTypeId { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter CreatorId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<KpiItemFilter> OrFilter { get; set; }
        public KpiItemOrder OrderBy { get; set; }
        public KpiItemSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum KpiItemOrder
    {
        Id = 0,
        Organization = 1,
        KpiPeriod = 2,
        Status = 3,
        Employee = 4,
        Creator = 5,
        KpiYear = 6,
        KpiItemType = 7,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum KpiItemSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Organization = E._1,
        KpiPeriod = E._2,
        Status = E._3,
        Employee = E._4,
        Creator = E._5,
        KpiYear = E._6,
        KpiItemType = E._7,
    }
}
