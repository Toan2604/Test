using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.workflow_direction
{
    public class WorkflowDirection_WorkflowDirectionConditionDTO : DataDTO
    {
        public long Id { get; set; }
        public long WorkflowDirectionId { get; set; }
        public long WorkflowParameterId { get; set; }
        public long WorkflowOperatorId { get; set; }
        public string Value { get; set; }
        public string ValueString { get; set; }
        public bool Used { get; set; }
        public WorkflowDirection_WorkflowDirectionDTO WorkflowDirection { get; set; }
        public WorkflowDirection_WorkflowOperatorDTO WorkflowOperator { get; set; }
        public WorkflowDirection_WorkflowParameterDTO WorkflowParameter { get; set; }
        public WorkflowDirection_WorkflowDirectionConditionDTO() { }
        public WorkflowDirection_WorkflowDirectionConditionDTO(WorkflowDirectionCondition WorkflowDirectionCondition)
        {
            this.Id = WorkflowDirectionCondition.Id;
            this.WorkflowDirectionId = WorkflowDirectionCondition.WorkflowDirectionId;
            this.WorkflowParameterId = WorkflowDirectionCondition.WorkflowParameterId;
            this.WorkflowOperatorId = WorkflowDirectionCondition.WorkflowOperatorId;
            this.Value = WorkflowDirectionCondition.Value;
            this.ValueString = WorkflowDirectionCondition.ValueString;
            this.Used = WorkflowDirectionCondition.Used;

            this.WorkflowDirection = WorkflowDirectionCondition.WorkflowDirection == null ? null : new WorkflowDirection_WorkflowDirectionDTO(WorkflowDirectionCondition.WorkflowDirection);
            this.WorkflowOperator = WorkflowDirectionCondition.WorkflowOperator == null ? null : new WorkflowDirection_WorkflowOperatorDTO(WorkflowDirectionCondition.WorkflowOperator);
            this.WorkflowParameter = WorkflowDirectionCondition.WorkflowParameter == null ? null : new WorkflowDirection_WorkflowParameterDTO(WorkflowDirectionCondition.WorkflowParameter);
            this.Errors = WorkflowDirectionCondition.Errors;
        }
    }
}
