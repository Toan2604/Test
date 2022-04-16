using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class Permission : DataEntity, IEquatable<Permission>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long RoleId { get; set; }
        public long MenuId { get; set; }
        public long StatusId { get; set; }
        public Menu Menu { get; set; }
        public Role Role { get; set; }
        public Status Status { get; set; }
        public List<PermissionContent> PermissionContents { get; set; }
        public List<PermissionActionMapping> PermissionActionMappings { get; set; }

        public bool Equals(Permission other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class PermissionFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter RoleId { get; set; }
        public IdFilter MenuId { get; set; }
        public IdFilter StatusId { get; set; }
        public List<PermissionFilter> OrFilter { get; set; }
        public PermissionOrder OrderBy { get; set; }
        public PermissionSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PermissionOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Role = 3,
        Menu = 4,
        Status = 5
    }

    [Flags]
    public enum PermissionSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Role = E._3,
        Menu = E._4,
        Status = E._5,
        PermissionContent = E._6,
    }

    public class FieldType : DataEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class PermissionOperator : DataEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long FieldTypeId { get; set; }
    }

    public class PermissionOperatorFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter FieldTypeId { get; set; }
    }
}
