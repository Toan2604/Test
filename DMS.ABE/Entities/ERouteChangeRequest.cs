using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class ERouteChangeRequest : DataEntity, IEquatable<ERouteChangeRequest>
    {
        public long Id { get; set; }
        public long ERouteId { get; set; }
        public long CreatorId { get; set; }
        public long RequestStateId { get; set; }
        public AppUser Creator { get; set; }
        public ERoute ERoute { get; set; }
        public RequestState RequestState { get; set; }
        public List<ERouteChangeRequestContent> ERouteChangeRequestContents { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool Equals(ERouteChangeRequest other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ERouteChangeRequestFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter SaleEmployeeId { get; set; }
        public DateFilter StartDate { get; set; }
        public DateFilter EndDate { get; set; }
        public IdFilter ERouteTypeId { get; set; }
        public IdFilter ERouteId { get; set; }
        public IdFilter CreatorId { get; set; }
        public IdFilter RequestStateId { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter StoreId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<ERouteChangeRequestFilter> OrFilter { get; set; }
        public ERouteChangeRequestOrder OrderBy { get; set; }
        public ERouteChangeRequestSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ERouteChangeRequestOrder
    {
        Id = 0,
        ERoute = 1,
        Creator = 2,
        RequestState = 3,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum ERouteChangeRequestSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        ERoute = E._1,
        Creator = E._2,
        RequestState = E._3,
    }
}
