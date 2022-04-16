using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class Role : DataEntity, IEquatable<Role>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public Status Status { get; set; }
        public List<AppUserRoleMapping> AppUserRoleMappings { get; set; }
        public List<Permission> Permissions { get; set; }

        public bool Equals(Role other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class RoleFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public List<RoleFilter> OrFilter { get; set; }
        public RoleOrder OrderBy { get; set; }
        public RoleSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RoleOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Status = 3,
    }

    [Flags]
    public enum RoleSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Status = E._3,
    }
}
