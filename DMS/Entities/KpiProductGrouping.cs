using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class KpiProductGrouping : DataEntity, IEquatable<KpiProductGrouping>
    {
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long KpiYearId { get; set; }
        public long KpiPeriodId { get; set; }
        public long KpiProductGroupingTypeId { get; set; }
        public long StatusId { get; set; }
        public long EmployeeId { get; set; }
        public long CreatorId { get; set; }
        public Guid RowId { get; set; }
        public AppUser Creator { get; set; }
        public AppUser Employee { get; set; }
        public KpiPeriod KpiPeriod { get; set; }
        public KpiYear KpiYear { get; set; }
        public KpiProductGroupingType KpiProductGroupingType { get; set; }
        public Organization Organization { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public List<AppUser> Employees { get; set; }
        public List<KpiProductGroupingContent> KpiProductGroupingContents { get; set; }

        public bool Equals(KpiProductGrouping other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.OrganizationId != other.OrganizationId) return false;
            if (this.KpiYearId != other.KpiYearId) return false;
            if (this.KpiPeriodId != other.KpiPeriodId) return false;
            if (this.KpiProductGroupingTypeId != other.KpiProductGroupingTypeId) return false;
            if (this.StatusId != other.StatusId) return false;
            if (this.EmployeeId != other.EmployeeId) return false;
            if (this.CreatorId != other.CreatorId) return false;
            if (this.RowId != other.RowId) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class KpiProductGroupingFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter KpiYearId { get; set; }
        public IdFilter KpiPeriodId { get; set; }
        public IdFilter KpiProductGroupingTypeId { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter EmployeeId { get; set; }
        public IdFilter CreatorId { get; set; }
        public GuidFilter RowId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<KpiProductGroupingFilter> OrFilter { get; set; }
        public KpiProductGroupingOrder OrderBy { get; set; }
        public KpiProductGroupingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum KpiProductGroupingOrder
    {
        Id = 0,
        Organization = 1,
        KpiYear = 2,
        KpiPeriod = 3,
        KpiProductGroupingType = 4,
        Status = 5,
        Employee = 6,
        Creator = 7,
        Row = 11,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum KpiProductGroupingSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Organization = E._1,
        KpiYear = E._2,
        KpiPeriod = E._3,
        KpiProductGroupingType = E._4,
        Status = E._5,
        Employee = E._6,
        Creator = E._7,
        Row = E._11,
    }
}
