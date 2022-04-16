using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class KpiItemContentDAO
    {
        public KpiItemContentDAO()
        {
            KpiItemContentKpiCriteriaItemMappings = new HashSet<KpiItemContentKpiCriteriaItemMappingDAO>();
        }

        public long Id { get; set; }
        public long KpiItemId { get; set; }
        public long ItemId { get; set; }
        public Guid RowId { get; set; }

        public virtual ItemDAO Item { get; set; }
        public virtual KpiItemDAO KpiItem { get; set; }
        public virtual ICollection<KpiItemContentKpiCriteriaItemMappingDAO> KpiItemContentKpiCriteriaItemMappings { get; set; }
    }
}
