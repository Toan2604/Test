using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class KpiProductGroupingContentItemMappingDAO
    {
        public long KpiProductGroupingContentId { get; set; }
        public long ItemId { get; set; }

        public virtual ItemDAO Item { get; set; }
        public virtual KpiProductGroupingContentDAO KpiProductGroupingContent { get; set; }
    }
}
