using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.workflow_parameter
{
    public class WorkflowParameter_WorkflowParameterTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public WorkflowParameter_WorkflowParameterTypeDTO() { }
        public WorkflowParameter_WorkflowParameterTypeDTO(WorkflowParameterType WorkflowParameterType)
        {

            this.Id = WorkflowParameterType.Id;

            this.Code = WorkflowParameterType.Code;

            this.Name = WorkflowParameterType.Name;

            this.Errors = WorkflowParameterType.Errors;
        }
    }

    public class WorkflowParameter_WorkflowParameterTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public WorkflowParameterTypeOrder OrderBy { get; set; }
    }
}