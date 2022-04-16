using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.e_route
{
    public class ERoute_ERouteDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long SaleEmployeeId { get; set; }
        public long OrganizationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long ERouteTypeId { get; set; }
        public long RequestStateId { get; set; }
        public Guid RowId { get; set; }
        public long StatusId { get; set; }
        public long CreatorId { get; set; }
        public bool Used { get; set; }
        public ERoute_AppUserDTO Creator { get; set; }
        public ERoute_ERouteTypeDTO ERouteType { get; set; }
        public ERoute_OrganizationDTO Organization { get; set; }
        public ERoute_RequestStateDTO RequestState { get; set; }
        public ERoute_AppUserDTO SaleEmployee { get; set; }
        public ERoute_StatusDTO Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<ERoute_ERouteContentDTO> ERouteContents { get; set; }
        public List<ERoute_RequestWorkflowStepMappingDTO> RequestWorkflowStepMappings { get; set; }
        public ERoute_ERouteDTO() { }
        public ERoute_ERouteDTO(ERoute ERoute)
        {
            this.Id = ERoute.Id;
            this.Code = ERoute.Code;
            this.Name = ERoute.Name;
            this.SaleEmployeeId = ERoute.SaleEmployeeId;
            this.OrganizationId = ERoute.OrganizationId;
            this.StartDate = ERoute.StartDate;
            this.EndDate = ERoute.EndDate;
            this.ERouteTypeId = ERoute.ERouteTypeId;
            this.RequestStateId = ERoute.RequestStateId;
            this.StatusId = ERoute.StatusId;
            this.CreatorId = ERoute.CreatorId;
            this.RowId = ERoute.RowId;
            this.Used = ERoute.Used;
            this.Creator = ERoute.Creator == null ? null : new ERoute_AppUserDTO(ERoute.Creator);
            this.ERouteType = ERoute.ERouteType == null ? null : new ERoute_ERouteTypeDTO(ERoute.ERouteType);
            this.RequestState = ERoute.RequestState == null ? null : new ERoute_RequestStateDTO(ERoute.RequestState);
            this.SaleEmployee = ERoute.SaleEmployee == null ? null : new ERoute_AppUserDTO(ERoute.SaleEmployee);
            this.Organization = ERoute.Organization == null ? null : new ERoute_OrganizationDTO(ERoute.Organization);
            this.Status = ERoute.Status == null ? null : new ERoute_StatusDTO(ERoute.Status);
            this.CreatedAt = ERoute.CreatedAt;
            this.UpdatedAt = ERoute.UpdatedAt;
            this.ERouteContents = ERoute.ERouteContents?.Select(x => new ERoute_ERouteContentDTO(x)).ToList();
            this.RequestWorkflowStepMappings = ERoute.RequestWorkflowStepMappings?.Select(x => new ERoute_RequestWorkflowStepMappingDTO(x)).ToList();
            this.Errors = ERoute.Errors;
        }
    }

    public class ERoute_ERouteFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter AppUserId { get; set; }
        public DateFilter StartDate { get; set; }
        public DateFilter EndDate { get; set; }
        public IdFilter ERouteTypeId { get; set; }
        public IdFilter RequestStateId { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter CreatorId { get; set; }
        public IdFilter StoreId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public ERouteOrder OrderBy { get; set; }
    }
}
