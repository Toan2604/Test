using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.workflow_definition
{
    public class WorkflowDefinition_WorkflowOperatorDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long WorkflowParameterTypeId { get; set; }

        public WorkflowDefinition_WorkflowOperatorDTO() { }
        public WorkflowDefinition_WorkflowOperatorDTO(WorkflowOperator WorkflowOperator)
        {
            this.Id = WorkflowOperator.Id;
            this.Code = WorkflowOperator.Code;
            this.Name = WorkflowOperator.Name;
            this.WorkflowParameterTypeId = WorkflowOperator.WorkflowParameterTypeId;
        }
    }

    public class WorkflowDirection_WorkflowOperatorFilterDTO : DataDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter WorkflowParameterTypeId { get; set; }
    }
}
