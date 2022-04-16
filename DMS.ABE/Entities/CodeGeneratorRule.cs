using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class CodeGeneratorRule : DataEntity, IEquatable<CodeGeneratorRule>
    {
        public long Id { get; set; }
        public long AutoNumberLenth { get; set; }
        public bool Used { get; set; }
        public long StatusId { get; set; }
        public long EntityTypeId { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public EntityType EntityType { get; set; }
        public Status Status { get; set; }
        public List<CodeGeneratorRuleEntityComponentMapping> CodeGeneratorRuleEntityComponentMappings { get; set; }

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
        public List<CodeGeneratorRuleFilter> OrFilter { get; set; }
        public CodeGeneratorRuleOrder OrderBy { get; set; }
        public CodeGeneratorRuleSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CodeGeneratorRuleOrder
    {
        Id = 0,
        AutoNumberLenth = 1,
        Used = 2
    }

    [Flags]
    public enum CodeGeneratorRuleSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        AutoNumberLenth = E._1,
        Used = E._2
    }
}
