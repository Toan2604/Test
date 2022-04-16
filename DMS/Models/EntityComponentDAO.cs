using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class EntityComponentDAO
    {
        public EntityComponentDAO()
        {
            CodeGeneratorRuleEntityComponentMappings = new HashSet<CodeGeneratorRuleEntityComponentMappingDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CodeGeneratorRuleEntityComponentMappingDAO> CodeGeneratorRuleEntityComponentMappings { get; set; }
    }
}
