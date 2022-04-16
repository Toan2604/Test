using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class KpiYearDAO
    {
        public KpiYearDAO()
        {
            KpiGenerals = new HashSet<KpiGeneralDAO>();
            KpiItems = new HashSet<KpiItemDAO>();
            KpiProductGroupings = new HashSet<KpiProductGroupingDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<KpiGeneralDAO> KpiGenerals { get; set; }
        public virtual ICollection<KpiItemDAO> KpiItems { get; set; }
        public virtual ICollection<KpiProductGroupingDAO> KpiProductGroupings { get; set; }
    }
}
