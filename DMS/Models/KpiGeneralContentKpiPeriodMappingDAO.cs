using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class KpiGeneralContentKpiPeriodMappingDAO
    {
        public long KpiGeneralContentId { get; set; }
        public long KpiPeriodId { get; set; }
        public decimal? Value { get; set; }

        public virtual KpiGeneralContentDAO KpiGeneralContent { get; set; }
        public virtual KpiPeriodDAO KpiPeriod { get; set; }
    }
}
