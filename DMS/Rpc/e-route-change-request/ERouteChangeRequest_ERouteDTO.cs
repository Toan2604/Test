using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.e_route_change_request
{
    public class ERouteChangeRequest_ERouteDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long SaleEmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long ERouteTypeId { get; set; }
        public long RequestStateId { get; set; }
        public long StatusId { get; set; }
        public long CreatorId { get; set; }
        public ERouteChangeRequest_AppUserDTO Creator { get; set; }
        public ERouteChangeRequest_ERouteTypeDTO ERouteType { get; set; }
        public ERouteChangeRequest_RequestStateDTO RequestState { get; set; }
        public ERouteChangeRequest_AppUserDTO SaleEmployee { get; set; }
        public ERouteChangeRequest_StatusDTO Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<ERouteChangeRequest_ERouteContentDTO> ERouteContents { get; set; }
        public ERouteChangeRequest_ERouteDTO() { }
        public ERouteChangeRequest_ERouteDTO(ERoute ERoute)
        {
            this.Id = ERoute.Id;
            this.Code = ERoute.Code;
            this.Name = ERoute.Name;
            this.SaleEmployeeId = ERoute.SaleEmployeeId;
            this.StartDate = ERoute.StartDate;
            this.EndDate = ERoute.EndDate;
            this.ERouteTypeId = ERoute.ERouteTypeId;
            this.RequestStateId = ERoute.RequestStateId;
            this.StatusId = ERoute.StatusId;
            this.CreatorId = ERoute.CreatorId;
            this.Creator = ERoute.Creator == null ? null : new ERouteChangeRequest_AppUserDTO(ERoute.Creator);
            this.ERouteType = ERoute.ERouteType == null ? null : new ERouteChangeRequest_ERouteTypeDTO(ERoute.ERouteType);
            this.RequestState = ERoute.RequestState == null ? null : new ERouteChangeRequest_RequestStateDTO(ERoute.RequestState);
            this.SaleEmployee = ERoute.SaleEmployee == null ? null : new ERouteChangeRequest_AppUserDTO(ERoute.SaleEmployee);
            this.Status = ERoute.Status == null ? null : new ERouteChangeRequest_StatusDTO(ERoute.Status);
            this.CreatedAt = ERoute.CreatedAt;
            this.UpdatedAt = ERoute.UpdatedAt;
            this.ERouteContents = ERoute.ERouteContents?.Select(x => new ERouteChangeRequest_ERouteContentDTO(x)).ToList();
            this.Errors = ERoute.Errors;
        }
    }

    public class ERouteChangeRequest_ERouteFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter SaleEmployeeId { get; set; }

        public DateFilter StartDate { get; set; }

        public DateFilter EndDate { get; set; }

        public IdFilter ERouteTypeId { get; set; }

        public IdFilter RequestStateId { get; set; }

        public IdFilter StatusId { get; set; }

        public IdFilter CreatorId { get; set; }

        public ERouteOrder OrderBy { get; set; }
    }
}