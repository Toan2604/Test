using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.workflow_parameter
{
    public class WorkflowParameter_WorkflowParameterDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long WorkflowTypeId { get; set; }
        public long WorkflowParameterTypeId { get; set; }
        public WorkflowParameter_WorkflowParameterTypeDTO WorkflowParameterType { get; set; }
        public WorkflowParameter_WorkflowTypeDTO WorkflowType { get; set; }
        public WorkflowParameter_WorkflowParameterDTO() { }
        public WorkflowParameter_WorkflowParameterDTO(WorkflowParameter WorkflowParameter)
        {
            this.Id = WorkflowParameter.Id;
            this.Code = WorkflowParameter.Code;
            this.Name = WorkflowParameter.Name;
            this.WorkflowTypeId = WorkflowParameter.WorkflowTypeId;
            this.WorkflowParameterTypeId = WorkflowParameter.WorkflowParameterTypeId;
            this.WorkflowParameterType = WorkflowParameter.WorkflowParameterType == null ? null : new WorkflowParameter_WorkflowParameterTypeDTO(WorkflowParameter.WorkflowParameterType);
            this.WorkflowType = WorkflowParameter.WorkflowType == null ? null : new WorkflowParameter_WorkflowTypeDTO(WorkflowParameter.WorkflowType);
            this.Errors = WorkflowParameter.Errors;
        }
    }

    public class WorkflowParameter_WorkflowParameterFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter WorkflowTypeId { get; set; }
        public IdFilter WorkflowParameterTypeId { get; set; }
        public WorkflowParameterOrder OrderBy { get; set; }
    }
}
