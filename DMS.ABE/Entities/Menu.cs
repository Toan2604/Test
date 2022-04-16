using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class Menu : DataEntity, IEquatable<Menu>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsDeleted { get; set; }
        public List<Field> Fields { get; set; }
        public List<Action> Actions { get; set; }

        public bool Equals(Menu other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class MenuFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Path { get; set; }
        public List<MenuFilter> OrFilter { get; set; }
        public MenuOrder OrderBy { get; set; }
        public MenuSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MenuOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Path = 3,
        IsDeleted = 4,
    }

    [Flags]
    public enum MenuSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Path = E._3,
        IsDeleted = E._4,
    }
}
