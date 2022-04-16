using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class KpiGeneral : DataEntity, IEquatable<KpiGeneral>
    {
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long EmployeeId { get; set; }
        public long KpiYearId { get; set; }
        public long StatusId { get; set; }
        public long CreatorId { get; set; }
        public AppUser Creator { get; set; }
        public AppUser Employee { get; set; }
        public KpiYear KpiYear { get; set; }
        public Organization Organization { get; set; }
        public Status Status { get; set; }
        public List<KpiGeneralContent> KpiGeneralContents { get; set; }
        public List<long> EmployeeIds { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid RowId { get; set; }

        public bool Equals(KpiGeneral other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class KpiGeneralFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter KpiYearId { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter CreatorId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<KpiGeneralFilter> OrFilter { get; set; }
        public KpiGeneralOrder OrderBy { get; set; }
        public KpiGeneralSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum KpiGeneralOrder
    {
        Id = 0,
        Organization = 1,
        Employee = 2,
        KpiYear = 3,
        Status = 4,
        Creator = 5,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum KpiGeneralSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Organization = E._1,
        Employee = E._2,
        KpiYear = E._3,
        Status = E._4,
        Creator = E._5,
    }
}
