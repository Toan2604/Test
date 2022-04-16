using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.workflow_direction
{
    public class WorkflowDirection_WorkflowDirectionDTO : DataDTO
    {
        public long Id { get; set; }
        public long WorkflowDefinitionId { get; set; }
        public long FromStepId { get; set; }
        public long ToStepId { get; set; }
        public string SubjectMailForCreator { get; set; }
        public string SubjectMailForCurrentStep { get; set; }
        public string SubjectMailForNextStep { get; set; }
        public string BodyMailForCreator { get; set; }
        public string BodyMailForCurrentStep { get; set; }
        public string BodyMailForNextStep { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public WorkflowDirection_StatusDTO Status { get; set; }
        public WorkflowDirection_WorkflowStepDTO FromStep { get; set; }
        public WorkflowDirection_WorkflowStepDTO ToStep { get; set; }
        public WorkflowDirection_WorkflowDefinitionDTO WorkflowDefinition { get; set; }
        public List<WorkflowDirection_WorkflowParameterDTO> WorkflowParameters { get; set; }
        public List<WorkflowDirection_WorkflowDirectionConditionDTO> WorkflowDirectionConditions { get; set; }
        public WorkflowDirection_WorkflowDirectionDTO() { }
        public WorkflowDirection_WorkflowDirectionDTO(WorkflowDirection WorkflowDirection)
        {
            this.Id = WorkflowDirection.Id;
            this.WorkflowDefinitionId = WorkflowDirection.WorkflowDefinitionId;
            this.FromStepId = WorkflowDirection.FromStepId;
            this.ToStepId = WorkflowDirection.ToStepId;
            this.SubjectMailForCreator = WorkflowDirection.SubjectMailForCreator;
            this.SubjectMailForCurrentStep = WorkflowDirection.SubjectMailForCurrentStep;
            this.SubjectMailForNextStep = WorkflowDirection.SubjectMailForNextStep;
            this.BodyMailForCreator = WorkflowDirection.BodyMailForCreator;
            this.BodyMailForCurrentStep = WorkflowDirection.BodyMailForCurrentStep;
            this.BodyMailForNextStep = WorkflowDirection.BodyMailForNextStep;
            this.UpdatedAt = WorkflowDirection.UpdatedAt;
            this.StatusId = WorkflowDirection.StatusId;
            this.Used = WorkflowDirection.Used;
            this.Status = WorkflowDirection.Status == null ? null : new WorkflowDirection_StatusDTO(WorkflowDirection.Status);
            this.FromStep = WorkflowDirection.FromStep == null ? null : new WorkflowDirection_WorkflowStepDTO(WorkflowDirection.FromStep);
            this.ToStep = WorkflowDirection.ToStep == null ? null : new WorkflowDirection_WorkflowStepDTO(WorkflowDirection.ToStep);
            this.WorkflowDefinition = WorkflowDirection.WorkflowDefinition == null ? null : new WorkflowDirection_WorkflowDefinitionDTO(WorkflowDirection.WorkflowDefinition);
            this.WorkflowParameters = WorkflowDirection.WorkflowParameters?.Select(p => new WorkflowDirection_WorkflowParameterDTO(p)).ToList();
            this.WorkflowDirectionConditions = WorkflowDirection.WorkflowDirectionConditions?.Select(p => new WorkflowDirection_WorkflowDirectionConditionDTO(p)).ToList();
            this.Errors = WorkflowDirection.Errors;
        }
    }

    public class WorkflowDirection_WorkflowDirectionFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter WorkflowDefinitionId { get; set; }
        public IdFilter FromStepId { get; set; }
        public IdFilter ToStepId { get; set; }
        public IdFilter StatusId { get; set; }
        public StringFilter SubjectMailForCreator { get; set; }
        public StringFilter SubjectMailForNextStep { get; set; }
        public StringFilter BodyMailForCreator { get; set; }
        public StringFilter BodyMailForNextStep { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public WorkflowDirectionOrder OrderBy { get; set; }
    }
}
