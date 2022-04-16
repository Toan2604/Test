using TrueSight.Common;

namespace DMS.Entities
{
    public class WorkflowDirectionCondition : DataEntity
    {
        public long Id { get; set; }
        public long WorkflowDirectionId { get; set; }
        public long WorkflowParameterId { get; set; }
        public long WorkflowOperatorId { get; set; }
        public string Value { get; set; }
        public string ValueString { get; set; }
        public bool Used { get; set; }
        public WorkflowDirection WorkflowDirection { get; set; }
        public WorkflowOperator WorkflowOperator { get; set; }
        public WorkflowParameter WorkflowParameter { get; set; }
    }
}
