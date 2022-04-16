using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class KpiProductGroupingContentDAO
    {
        public KpiProductGroupingContentDAO()
        {
            KpiProductGroupingContentCriteriaMappings = new HashSet<KpiProductGroupingContentCriteriaMappingDAO>();
            KpiProductGroupingContentItemMappings = new HashSet<KpiProductGroupingContentItemMappingDAO>();
        }

        public long Id { get; set; }
        public long KpiProductGroupingId { get; set; }
        public long ProductGroupingId { get; set; }
        public Guid RowId { get; set; }
        public bool SelectAllCurrentItem { get; set; }

        public virtual KpiProductGroupingDAO KpiProductGrouping { get; set; }
        public virtual ProductGroupingDAO ProductGrouping { get; set; }
        public virtual ICollection<KpiProductGroupingContentCriteriaMappingDAO> KpiProductGroupingContentCriteriaMappings { get; set; }
        public virtual ICollection<KpiProductGroupingContentItemMappingDAO> KpiProductGroupingContentItemMappings { get; set; }
    }
}
