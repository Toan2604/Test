using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class Category : DataEntity, IEquatable<Category>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string Path { get; set; }
        public long Level { get; set; }
        public long StatusId { get; set; }
        public long? ImageId { get; set; }
        public string Description { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }
        public bool IsDisplay { get; set; }
        internal bool HasChildren { get; set; }
        public long ProductCounter { get; set; }
        public long OrderNumber { get; set; }
        public Image Image { get; set; }
        public Category Parent { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public bool Equals(Category other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class CategoryFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter OrderNumber { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter ParentId { get; set; }
        public StringFilter Path { get; set; }
        public LongFilter Level { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter ImageId { get; set; }
        public bool? IsDisplay { get; set; }
        public GuidFilter RowId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public bool? HasChildren { get; set; }
        public List<CategoryFilter> OrFilter { get; set; }
        public CategoryOrder OrderBy { get; set; }
        public CategorySelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CategoryOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Parent = 3,
        Path = 4,
        Level = 5,
        Status = 6,
        Image = 7,
        Description = 8,
        Row = 11,
        Used = 12,
        CreatedAt = 50,
        UpdatedAt = 51,
        OrderNumber = 52,
    }

    [Flags]
    public enum CategorySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Parent = E._3,
        Path = E._4,
        Level = E._5,
        Status = E._6,
        Image = E._7,
        Description = E._8,
        Row = E._11,
        Used = E._12,
        OrderNumber = E._13,
    }
}
