using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class ERoute : DataEntity, IEquatable<ERoute>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long SaleEmployeeId { get; set; }
        public long OrganizationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime RealStartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long ERouteTypeId { get; set; }
        public long RequestStateId { get; set; }
        public long StatusId { get; set; }
        public long CreatorId { get; set; }
        public long TotalStoreCounter { get; set; }
        public Guid RowId { get; set; }
        public AppUser Creator { get; set; }
        public ERouteType ERouteType { get; set; }
        public Organization Organization { get; set; }
        public RequestState RequestState { get; set; }
        public AppUser SaleEmployee { get; set; }
        public Status Status { get; set; }
        public List<ERouteContent> ERouteContents { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<RequestWorkflowStepMapping> RequestWorkflowStepMappings { get; set; }
        public bool Equals(ERoute other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ERouteFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter ApproverId { get; set; }
        public DateFilter StartDate { get; set; }
        public DateFilter EndDate { get; set; }
        public IdFilter ERouteTypeId { get; set; }
        public IdFilter RequestStateId { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter CreatorId { get; set; }
        public IdFilter StoreId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<ERouteFilter> OrFilter { get; set; }
        public ERouteOrder OrderBy { get; set; }
        public ERouteSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ERouteOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        SaleEmployee = 3,
        StartDate = 4,
        EndDate = 5,
        ERouteType = 6,
        RequestState = 7,
        Status = 8,
        Organization = 9,
        Creator = 12,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum ERouteSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        SaleEmployee = E._3,
        StartDate = E._4,
        EndDate = E._5,
        ERouteType = E._6,
        RequestState = E._7,
        Status = E._8,
        RealStartDate = E._9,
        Organization = E._10,
        Creator = E._12,
        ERouteContent = E._13,
    }
}
