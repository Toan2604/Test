using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class CodeGeneratorRuleEntityComponentMapping : DataEntity, IEquatable<CodeGeneratorRuleEntityComponentMapping>
    {
        public long CodeGeneratorRuleId { get; set; }
        public long EntityComponentId { get; set; }
        public long Sequence { get; set; }
        public string Value { get; set; }
        public DateTime? DeletedAt { get; set; }
        public CodeGeneratorRule CodeGeneratorRule { get; set; }
        public EntityComponent EntityComponent { get; set; }

        public bool Equals(CodeGeneratorRuleEntityComponentMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class CodeGeneratorRuleEntityComponentMappingFilter : FilterEntity
    {
        public IdFilter CodeGeneratorRuleId { get; set; }
        public IdFilter EntityComponentId { get; set; }
        public List<CodeGeneratorRuleEntityComponentMappingFilter> OrFilter { get; set; }
        public CodeGeneratorRuleEntityComponentMappingOrder OrderBy { get; set; }
        public CodeGeneratorRuleEntityComponentMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CodeGeneratorRuleEntityComponentMappingOrder
    {
        CodeGeneratorRule = 0,
        EntityComponent = 1,
        Sequence = 2,
        Value = 3
    }

    [Flags]
    public enum CodeGeneratorRuleEntityComponentMappingSelect : long
    {
        ALL = E.ALL,
        CodeGeneratorRule = E._0,
        EntityComponent = E._1,
        Sequence = E._2,
        Value = E._3
    }
}
