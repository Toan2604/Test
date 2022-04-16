using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class PermissionActionMapping : DataEntity, IEquatable<PermissionActionMapping>
    {
        public long PermissionId { get; set; }
        public long ActionId { get; set; }
        public Action Action { get; set; }
        public Permission Permission { get; set; }

        public bool Equals(PermissionActionMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PermissionPageMappingFilter : FilterEntity
    {
        public IdFilter PermissionId { get; set; }
        public IdFilter ActionId { get; set; }
        public List<PermissionPageMappingFilter> OrFilter { get; set; }
        public PermissionPageMappingOrder OrderBy { get; set; }
        public PermissionPageMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PermissionPageMappingOrder
    {
        Permission = 0,
        Page = 1,
    }

    [Flags]
    public enum PermissionPageMappingSelect : long
    {
        ALL = E.ALL,
        Permission = E._0,
        Page = E._1,
    }
}
