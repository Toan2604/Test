using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class CodeGeneratorRuleEntityComponentMappingDAO
    {
        public long CodeGeneratorRuleId { get; set; }
        public long EntityComponentId { get; set; }
        public long Sequence { get; set; }
        public string Value { get; set; }

        public virtual CodeGeneratorRuleDAO CodeGeneratorRule { get; set; }
        public virtual EntityComponentDAO EntityComponent { get; set; }
    }
}
