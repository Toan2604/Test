using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class PageDAO
    {
        public PageDAO()
        {
            ActionPageMappings = new HashSet<ActionPageMappingDAO>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<ActionPageMappingDAO> ActionPageMappings { get; set; }
    }
}
