using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.workflow_parameter
{
    public class WorkflowParameter_WorkflowTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public WorkflowParameter_WorkflowTypeDTO() { }
        public WorkflowParameter_WorkflowTypeDTO(WorkflowType WorkflowType)
        {

            this.Id = WorkflowType.Id;

            this.Code = WorkflowType.Code;

            this.Name = WorkflowType.Name;

            this.Errors = WorkflowType.Errors;
        }
    }

    public class WorkflowParameter_WorkflowTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public WorkflowTypeOrder OrderBy { get; set; }
    }
}