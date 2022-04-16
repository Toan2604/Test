using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.workflow_direction
{
    public class WorkflowDirection_WorkflowParameterDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long WorkflowParameterTypeId { get; set; }
        public long WorkflowTypeId { get; set; }

        public WorkflowDirection_WorkflowParameterDTO() { }
        public WorkflowDirection_WorkflowParameterDTO(WorkflowParameter WorkflowParameter)
        {
            this.Id = WorkflowParameter.Id;
            this.Code = WorkflowParameter.Code;
            this.Name = WorkflowParameter.Name;
            this.WorkflowParameterTypeId = WorkflowParameter.WorkflowParameterTypeId;
            this.WorkflowTypeId = WorkflowParameter.WorkflowTypeId;
        }
    }

    public class WorkflowDirection_WorkflowParameterFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter WorkflowTypeId { get; set; }
        public IdFilter WorkflowParameterTypeId { get; set; }
        public WorkflowParameterOrder OrderBy { get; set; }
    }
}
