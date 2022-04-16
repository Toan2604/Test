using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class Field : DataEntity, IEquatable<Field>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long FieldTypeId { get; set; }
        public long MenuId { get; set; }
        public bool IsDeleted { get; set; }
        public Menu Menu { get; set; }
        public FieldType FieldType { get; set; }

        public bool Equals(Field other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class FieldFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter FieldTypeId { get; set; }
        public IdFilter MenuId { get; set; }
        public List<FieldFilter> OrFilter { get; set; }
        public FieldOrder OrderBy { get; set; }
        public FieldSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FieldOrder
    {
        Id = 0,
        Name = 1,
        FieldType = 2,
        Menu = 3,
        IsDeleted = 4,
    }

    [Flags]
    public enum FieldSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Name = E._1,
        FieldType = E._2,
        Menu = E._3,
        IsDeleted = E._4,
    }
}
