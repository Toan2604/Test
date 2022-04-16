using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.workflow_definition
{
    public class WorkflowDefinition_RoleDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long StatusId { get; set; }


        public WorkflowDefinition_RoleDTO() { }
        public WorkflowDefinition_RoleDTO(Role Role)
        {

            this.Id = Role.Id;

            this.Code = Role.Code;

            this.Name = Role.Name;

            this.StatusId = Role.StatusId;

            this.Errors = Role.Errors;
        }
    }

    public class WorkflowDefinition_RoleFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter StatusId { get; set; }

        public RoleOrder OrderBy { get; set; }
    }
}