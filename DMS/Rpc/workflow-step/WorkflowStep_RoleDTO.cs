using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.workflow_step
{
    public class WorkflowStep_RoleDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long StatusId { get; set; }


        public WorkflowStep_RoleDTO() { }
        public WorkflowStep_RoleDTO(Role Role)
        {

            this.Id = Role.Id;

            this.Code = Role.Code;

            this.Name = Role.Name;

            this.StatusId = Role.StatusId;

            this.Errors = Role.Errors;
        }
    }

    public class WorkflowStep_RoleFilterDTO : FilterDTO
    {

        public IdFilter WorkflowDefinitionId { get; set; }
        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter StatusId { get; set; }

        public RoleOrder OrderBy { get; set; }
    }
}