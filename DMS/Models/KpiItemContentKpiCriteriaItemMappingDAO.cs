using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class KpiItemContentKpiCriteriaItemMappingDAO
    {
        public long KpiItemContentId { get; set; }
        public long KpiCriteriaItemId { get; set; }
        public long? Value { get; set; }

        public virtual KpiCriteriaItemDAO KpiCriteriaItem { get; set; }
        public virtual KpiItemContentDAO KpiItemContent { get; set; }
    }
}
