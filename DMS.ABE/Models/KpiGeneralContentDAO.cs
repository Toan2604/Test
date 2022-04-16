using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class KpiGeneralContentDAO
    {
        public KpiGeneralContentDAO()
        {
            KpiGeneralContentKpiPeriodMappings = new HashSet<KpiGeneralContentKpiPeriodMappingDAO>();
        }

        public long Id { get; set; }
        public long KpiGeneralId { get; set; }
        public long KpiCriteriaGeneralId { get; set; }
        public long StatusId { get; set; }
        public Guid RowId { get; set; }

        public virtual KpiCriteriaGeneralDAO KpiCriteriaGeneral { get; set; }
        public virtual KpiGeneralDAO KpiGeneral { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<KpiGeneralContentKpiPeriodMappingDAO> KpiGeneralContentKpiPeriodMappings { get; set; }
    }
}
