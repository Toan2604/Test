using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class Page : DataEntity, IEquatable<Page>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public long MenuId { get; set; }
        public bool IsDeleted { get; set; }
        public Menu Menu { get; set; }

        public bool Equals(Page other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class PageFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Path { get; set; }
        public IdFilter MenuId { get; set; }
        public List<PageFilter> OrFilter { get; set; }
        public PageOrder OrderBy { get; set; }
        public PageSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PageOrder
    {
        Id = 0,
        Name = 1,
        Path = 2,
        Menu = 3,
        IsDeleted = 4,
    }

    [Flags]
    public enum PageSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Name = E._1,
        Path = E._2,
        Menu = E._3,
        IsDeleted = E._4,
    }
}
