using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.workflow_definition
{
    public class WorkflowDefinition_WorkflowStepDTO : DataDTO
    {
        public long Id { get; set; }
        public long WorkflowDefinitionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long RoleId { get; set; }
        public string SubjectMailForReject { get; set; }
        public string BodyMailForReject { get; set; }
        public WorkflowDefinition_RoleDTO Role { get; set; }

        public WorkflowDefinition_WorkflowStepDTO() { }
        public WorkflowDefinition_WorkflowStepDTO(WorkflowStep WorkflowStep)
        {
            this.Id = WorkflowStep.Id;
            this.WorkflowDefinitionId = WorkflowStep.WorkflowDefinitionId;
            this.Code = WorkflowStep.Code;
            this.Name = WorkflowStep.Name;
            this.RoleId = WorkflowStep.RoleId;
            this.SubjectMailForReject = WorkflowStep.SubjectMailForReject;
            this.BodyMailForReject = WorkflowStep.BodyMailForReject;
            this.Role = WorkflowStep.Role == null ? null : new WorkflowDefinition_RoleDTO(WorkflowStep.Role);
            this.Errors = WorkflowStep.Errors;
        }
    }

    public class WorkflowDefinition_WorkflowStepFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter WorkflowDefinitionId { get; set; }

        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }

        public IdFilter RoleId { get; set; }

        public StringFilter SubjectMailForReject { get; set; }

        public StringFilter BodyMailForReject { get; set; }

        public WorkflowStepOrder OrderBy { get; set; }
    }
}