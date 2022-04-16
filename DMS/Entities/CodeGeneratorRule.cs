using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class CodeGeneratorRule : DataEntity, IEquatable<CodeGeneratorRule>
    {
        public long Id { get; set; }
        public long EntityTypeId { get; set; }
        public long AutoNumberLenth { get; set; }
        public long StatusId { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }
        public EntityType EntityType { get; set; }
        public Status Status { get; set; }
        public List<CodeGeneratorRuleEntityComponentMapping> CodeGeneratorRuleEntityComponentMappings { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public bool Equals(CodeGeneratorRule other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class CodeGeneratorRuleFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter EntityTypeId { get; set; }
        public IdFilter EntityComponentId { get; set; }
        public LongFilter AutoNumberLenth { get; set; }
        public IdFilter StatusId { get; set; }
        public GuidFilter RowId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<CodeGeneratorRuleFilter> OrFilter { get; set; }
        public CodeGeneratorRuleOrder OrderBy { get; set; }
        public CodeGeneratorRuleSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CodeGeneratorRuleOrder
    {
        Id = 0,
        EntityType = 1,
        AutoNumberLenth = 2,
        Status = 3,
        Row = 7,
        Used = 8,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum CodeGeneratorRuleSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        EntityType = E._1,
        AutoNumberLenth = E._2,
        Status = E._3,
        Row = E._7,
        Used = E._8,
    }
}
