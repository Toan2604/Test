using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class KpiProductGroupingContentCriteriaMappingDAO
    {
        public long KpiProductGroupingContentId { get; set; }
        public long KpiProductGroupingCriteriaId { get; set; }
        public long? Value { get; set; }

        public virtual KpiProductGroupingContentDAO KpiProductGroupingContent { get; set; }
        public virtual KpiProductGroupingCriteriaDAO KpiProductGroupingCriteria { get; set; }
    }
}
