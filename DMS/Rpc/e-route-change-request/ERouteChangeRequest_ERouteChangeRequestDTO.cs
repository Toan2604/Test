using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.e_route_change_request
{
    public class ERouteChangeRequest_ERouteChangeRequestDTO : DataDTO
    {
        public long Id { get; set; }
        public long ERouteId { get; set; }
        public long CreatorId { get; set; }
        public long RequestStateId { get; set; }
        public ERouteChangeRequest_AppUserDTO Creator { get; set; }
        public ERouteChangeRequest_ERouteDTO ERoute { get; set; }
        public ERouteChangeRequest_RequestStateDTO RequestState { get; set; }
        public List<ERouteChangeRequest_ERouteChangeRequestContentDTO> ERouteChangeRequestContents { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ERouteChangeRequest_ERouteChangeRequestDTO() { }
        public ERouteChangeRequest_ERouteChangeRequestDTO(ERouteChangeRequest ERouteChangeRequest)
        {
            this.Id = ERouteChangeRequest.Id;
            this.ERouteId = ERouteChangeRequest.ERouteId;
            this.CreatorId = ERouteChangeRequest.CreatorId;
            this.RequestStateId = ERouteChangeRequest.RequestStateId;
            this.Creator = ERouteChangeRequest.Creator == null ? null : new ERouteChangeRequest_AppUserDTO(ERouteChangeRequest.Creator);
            this.ERoute = ERouteChangeRequest.ERoute == null ? null : new ERouteChangeRequest_ERouteDTO(ERouteChangeRequest.ERoute);
            this.RequestState = ERouteChangeRequest.RequestState == null ? null : new ERouteChangeRequest_RequestStateDTO(ERouteChangeRequest.RequestState);
            this.ERouteChangeRequestContents = ERouteChangeRequest.ERouteChangeRequestContents?.Select(x => new ERouteChangeRequest_ERouteChangeRequestContentDTO(x)).ToList();
            this.CreatedAt = ERouteChangeRequest.CreatedAt;
            this.UpdatedAt = ERouteChangeRequest.UpdatedAt;
            this.Errors = ERouteChangeRequest.Errors;
        }
    }

    public class ERouteChangeRequest_ERouteChangeRequestFilterDTO : FilterDTO
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
        public ERouteChangeRequestOrder OrderBy { get; set; }
    }
}
