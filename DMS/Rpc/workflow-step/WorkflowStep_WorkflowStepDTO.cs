using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.workflow_step
{
    public class WorkflowStep_WorkflowStepDTO : DataDTO
    {
        public long Id { get; set; }
        public long WorkflowDefinitionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long RoleId { get; set; }
        public string SubjectMailForReject { get; set; }
        public string BodyMailForReject { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public WorkflowStep_RoleDTO Role { get; set; }
        public WorkflowStep_StatusDTO Status { get; set; }
        public WorkflowStep_WorkflowDefinitionDTO WorkflowDefinition { get; set; }
        public List<WorkflowStep_WorkflowParameterDTO> WorkflowParameters { get; set; }
        public WorkflowStep_WorkflowStepDTO() { }
        public WorkflowStep_WorkflowStepDTO(WorkflowStep WorkflowStep)
        {
            this.Id = WorkflowStep.Id;
            this.WorkflowDefinitionId = WorkflowStep.WorkflowDefinitionId;
            this.Code = WorkflowStep.Code;
            this.Name = WorkflowStep.Name;
            this.RoleId = WorkflowStep.RoleId;
            this.StatusId = WorkflowStep.StatusId;
            this.SubjectMailForReject = WorkflowStep.SubjectMailForReject;
            this.BodyMailForReject = WorkflowStep.BodyMailForReject;
            this.Used = WorkflowStep.Used;
            this.Role = WorkflowStep.Role == null ? null : new WorkflowStep_RoleDTO(WorkflowStep.Role);
            this.Status = WorkflowStep.Status == null ? null : new WorkflowStep_StatusDTO(WorkflowStep.Status);
            this.WorkflowDefinition = WorkflowStep.WorkflowDefinition == null ? null : new WorkflowStep_WorkflowDefinitionDTO(WorkflowStep.WorkflowDefinition);
            this.WorkflowParameters = WorkflowStep.WorkflowParameters?.Select(p => new WorkflowStep_WorkflowParameterDTO(p)).ToList();
            this.Errors = WorkflowStep.Errors;
        }
    }

    public class WorkflowStep_WorkflowStepFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter WorkflowDefinitionId { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter RoleId { get; set; }
        public IdFilter StatusId { get; set; }
        public StringFilter SubjectMailForReject { get; set; }
        public StringFilter BodyMailForReject { get; set; }
        public WorkflowStepOrder OrderBy { get; set; }
    }
}
