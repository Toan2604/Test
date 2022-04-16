using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PriceList : DataEntity, IEquatable<PriceList>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long StatusId { get; set; }
        public long CreatorId { get; set; }
        public long OrganizationId { get; set; }
        public long PriceListTypeId { get; set; }
        public long SalesOrderTypeId { get; set; }
        public Guid RowId { get; set; }
        public long RequestStateId { get; set; }
        public AppUser Creator { get; set; }
        public RequestState RequestState { get; set; }
        public Organization Organization { get; set; }
        public PriceListType PriceListType { get; set; }
        public SalesOrderType SalesOrderType { get; set; }
        public Status Status { get; set; }
        public List<PriceListItemMapping> PriceListItemMappings { get; set; }
        public List<PriceListStoreMapping> PriceListStoreMappings { get; set; }
        public List<PriceListStoreTypeMapping> PriceListStoreTypeMappings { get; set; }
        public List<PriceListStoreGroupingMapping> PriceListStoreGroupingMappings { get; set; }
        public List<RequestWorkflowStepMapping> RequestWorkflowStepMappings { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool Equals(PriceList other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class PriceListFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter CreatorId { get; set; }
        public DateFilter StartDate { get; set; }
        public DateFilter EndDate { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter PriceListTypeId { get; set; }
        public IdFilter SalesOrderTypeId { get; set; }
        public IdFilter RequestStateId { get; set; }
        public IdFilter ApproverId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<PriceListFilter> OrFilter { get; set; }
        public PriceListOrder OrderBy { get; set; }
        public PriceListSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PriceListOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        StartDate = 3,
        EndDate = 4,
        Status = 5,
        Organization = 6,
        PriceListType = 7,
        SalesOrderType = 8,
        RequestState = 9,
        Creator = 10,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum PriceListSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        StartDate = E._3,
        EndDate = E._4,
        Status = E._5,
        Organization = E._6,
        PriceListType = E._7,
        SalesOrderType = E._8,
        RequestState = E._9,
        Creator = E._10,
    }
}
