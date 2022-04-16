using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public class Role_RoleDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public Role_StatusDTO Status { get; set; }
        public List<Role_PermissionDTO> Permissions { get; set; }
        public List<Role_AppUserRoleMappingDTO> AppUserRoleMappings { get; set; }
        public Role_RoleDTO() { }
        public Role_RoleDTO(Role Role)
        {
            this.Id = Role.Id;
            this.Code = Role.Code;
            this.Name = Role.Name;
            this.StatusId = Role.StatusId;
            this.Used = Role.Used;
            this.Status = Role.Status == null ? null : new Role_StatusDTO(Role.Status);
            this.Permissions = Role.Permissions?.Select(x => new Role_PermissionDTO(x)).ToList();
            this.AppUserRoleMappings = Role.AppUserRoleMappings?.Select(x => new Role_AppUserRoleMappingDTO(x)).ToList();
            this.Errors = Role.Errors;
        }
    }

    public class Role_RoleFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public RoleOrder OrderBy { get; set; }
    }
}
