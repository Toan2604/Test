using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.workflow_definition
{
    public class WorkflowDefinition_WorkflowDefinitionDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long CreatorId { get; set; }
        public long ModifierId { get; set; }
        public long WorkflowTypeId { get; set; }
        public long OrganizationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long StatusId { get; set; }
        public bool HasConflict { get; set; }
        public WorkflowDefinition_OrganizationDTO Organization { get; set; }
        public WorkflowDefinition_AppUserDTO Creator { get; set; }
        public WorkflowDefinition_AppUserDTO Modifier { get; set; }
        public WorkflowDefinition_StatusDTO Status { get; set; }
        public WorkflowDefinition_WorkflowTypeDTO WorkflowType { get; set; }
        public List<WorkflowDefinition_WorkflowDirectionDTO> WorkflowDirections { get; set; }
        public List<WorkflowDefinition_WorkflowParameterDTO> WorkflowParameters { get; set; }
        public List<WorkflowDefinition_WorkflowStepDTO> WorkflowSteps { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Used { get; set; }
        public WorkflowDefinition_WorkflowDefinitionDTO() { }
        public WorkflowDefinition_WorkflowDefinitionDTO(WorkflowDefinition WorkflowDefinition)
        {
            this.Id = WorkflowDefinition.Id;
            this.Code = WorkflowDefinition.Code;
            this.Name = WorkflowDefinition.Name;
            this.CreatorId = WorkflowDefinition.CreatorId;
            this.ModifierId = WorkflowDefinition.ModifierId;
            this.WorkflowTypeId = WorkflowDefinition.WorkflowTypeId;
            this.StartDate = WorkflowDefinition.StartDate;
            this.EndDate = WorkflowDefinition.EndDate;
            this.StatusId = WorkflowDefinition.StatusId;
            this.OrganizationId = WorkflowDefinition.OrganizationId;
            this.CreatedAt = WorkflowDefinition.CreatedAt;
            this.UpdatedAt = WorkflowDefinition.UpdatedAt;
            this.Used = WorkflowDefinition.Used;
            this.HasConflict = WorkflowDefinition.HasConflict;
            this.Organization = WorkflowDefinition.Organization == null ? null : new WorkflowDefinition_OrganizationDTO(WorkflowDefinition.Organization);
            this.Creator = WorkflowDefinition.Creator == null ? null : new WorkflowDefinition_AppUserDTO(WorkflowDefinition.Creator);
            this.Modifier = WorkflowDefinition.Modifier == null ? null : new WorkflowDefinition_AppUserDTO(WorkflowDefinition.Modifier);
            this.Status = WorkflowDefinition.Status == null ? null : new WorkflowDefinition_StatusDTO(WorkflowDefinition.Status);
            this.WorkflowType = WorkflowDefinition.WorkflowType == null ? null : new WorkflowDefinition_WorkflowTypeDTO(WorkflowDefinition.WorkflowType);
            this.WorkflowDirections = WorkflowDefinition.WorkflowDirections?.Select(x => new WorkflowDefinition_WorkflowDirectionDTO(x)).ToList();
            this.WorkflowParameters = WorkflowDefinition.WorkflowParameters?.Select(x => new WorkflowDefinition_WorkflowParameterDTO(x)).ToList();
            this.WorkflowSteps = WorkflowDefinition.WorkflowSteps?.Select(x => new WorkflowDefinition_WorkflowStepDTO(x)).ToList();
            this.Errors = WorkflowDefinition.Errors;
        }
    }

    public class WorkflowDefinition_WorkflowDefinitionFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter CreatorId { get; set; }
        public IdFilter ModifierId { get; set; }
        public IdFilter WorkflowTypeId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public DateFilter StartDate { get; set; }
        public DateFilter EndDate { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public WorkflowDefinitionOrder OrderBy { get; set; }
    }
}
