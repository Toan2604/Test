using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.workflow_direction
{
    public class WorkflowDirection_WorkflowParameterTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public WorkflowDirection_WorkflowParameterTypeDTO() { }
        public WorkflowDirection_WorkflowParameterTypeDTO(WorkflowParameterType WorkflowParameterType)
        {
            this.Id = WorkflowParameterType.Id;
            this.Code = WorkflowParameterType.Code;
            this.Name = WorkflowParameterType.Name;
        }
    }
}
