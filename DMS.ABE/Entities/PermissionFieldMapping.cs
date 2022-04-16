using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PermissionContent : DataEntity, IEquatable<PermissionContent>
    {
        public long Id { get; set; }
        public long PermissionId { get; set; }
        public long FieldId { get; set; }
        public long PermissionOperatorId { get; set; }
        public string Value { get; set; }
        public Field Field { get; set; }
        public PermissionOperator PermissionOperator { get; set; }

        public bool Equals(PermissionContent other)
        {
            if (other == null) return false;
            return other.Id == this.Id;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PermissionFieldMappingFilter : FilterEntity
    {
        public IdFilter PermissionId { get; set; }
        public IdFilter FieldId { get; set; }
        public StringFilter Value { get; set; }
        public List<PermissionFieldMappingFilter> OrFilter { get; set; }
        public PermissionFieldMappingOrder OrderBy { get; set; }
        public PermissionFieldMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PermissionFieldMappingOrder
    {
        Permission = 0,
        Field = 1,
        Value = 2,
    }

    [Flags]
    public enum PermissionFieldMappingSelect : long
    {
        ALL = E.ALL,
        Permission = E._0,
        Field = E._1,
        Value = E._2,
    }
}
