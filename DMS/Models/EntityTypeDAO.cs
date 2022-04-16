using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class EntityTypeDAO
    {
        public EntityTypeDAO()
        {
            CodeGeneratorRules = new HashSet<CodeGeneratorRuleDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CodeGeneratorRuleDAO> CodeGeneratorRules { get; set; }
    }
}
