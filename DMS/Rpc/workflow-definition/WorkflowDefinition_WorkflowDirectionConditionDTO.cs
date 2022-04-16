using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.workflow_definition
{
    public class WorkflowDefinition_WorkflowDirectionConditionDTO : DataDTO
    {
        public long Id { get; set; }
        public long WorkflowDirectionId { get; set; }
        public long WorkflowParameterId { get; set; }
        public long WorkflowOperatorId { get; set; }
        public string Value { get; set; }

        public WorkflowDefinition_WorkflowOperatorDTO WorkflowOperator { get; set; }
        public WorkflowDefinition_WorkflowParameterDTO WorkflowParameter { get; set; }
        public WorkflowDefinition_WorkflowDirectionConditionDTO() { }
        public WorkflowDefinition_WorkflowDirectionConditionDTO(WorkflowDirectionCondition WorkflowDirectionCondition)
        {
            this.Id = WorkflowDirectionCondition.Id;
            this.WorkflowDirectionId = WorkflowDirectionCondition.WorkflowDirectionId;
            this.WorkflowParameterId = WorkflowDirectionCondition.WorkflowParameterId;
            this.WorkflowOperatorId = WorkflowDirectionCondition.WorkflowOperatorId;
            this.Value = WorkflowDirectionCondition.Value;

            this.WorkflowOperator = WorkflowDirectionCondition.WorkflowOperator == null ? null : new WorkflowDefinition_WorkflowOperatorDTO(WorkflowDirectionCondition.WorkflowOperator);
            this.WorkflowParameter = WorkflowDirectionCondition.WorkflowParameter == null ? null : new WorkflowDefinition_WorkflowParameterDTO(WorkflowDirectionCondition.WorkflowParameter);
            this.Errors = WorkflowDirectionCondition.Errors;
        }
    }
}
