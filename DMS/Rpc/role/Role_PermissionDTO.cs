using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public class Role_PermissionDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long RoleId { get; set; }
        public long MenuId { get; set; }
        public long StatusId { get; set; }
        public Role_MenuDTO Menu { get; set; }
        public List<Role_PermissionContentDTO> PermissionContents { get; set; }
        public List<Role_PermissionActionMappingDTO> PermissionActionMappings { get; set; }
        public Role_PermissionDTO() { }
        public Role_PermissionDTO(Permission Permission)
        {
            this.Id = Permission.Id;
            this.Code = Permission.Code;
            this.Name = Permission.Name;
            this.RoleId = Permission.RoleId;
            this.MenuId = Permission.MenuId;
            this.StatusId = Permission.StatusId;
            this.Menu = Permission.Menu == null ? null : new Role_MenuDTO(Permission.Menu);
            this.PermissionContents = Permission.PermissionContents?.Select(x => new Role_PermissionContentDTO(x)).ToList();
            this.PermissionActionMappings = Permission.PermissionActionMappings?.Select(x => new Role_PermissionActionMappingDTO(x)).ToList();
            this.Errors = Permission.Errors;
        }
    }

    public class Role_PermissionFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter RoleId { get; set; }
        public IdFilter MenuId { get; set; }
        public IdFilter StatusId { get; set; }
        public PermissionOrder OrderBy { get; set; }
    }
}