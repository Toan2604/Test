using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.workflow_step
{
    public class WorkflowStep_StatusDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long StatusId { get; set; }


        public WorkflowStep_StatusDTO() { }
        public WorkflowStep_StatusDTO(Status Status)
        {

            this.Id = Status.Id;

            this.Code = Status.Code;

            this.Name = Status.Name;

            this.Errors = Status.Errors;
        }
    }

    public class WorkflowStep_StatusFilterDTO : FilterDTO
    {

        public IdFilter WorkflowDefinitionId { get; set; }
        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StatusOrder OrderBy { get; set; }
    }
}