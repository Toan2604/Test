using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class KpiProductGroupingTypeDAO
    {
        public KpiProductGroupingTypeDAO()
        {
            KpiProductGroupings = new HashSet<KpiProductGroupingDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<KpiProductGroupingDAO> KpiProductGroupings { get; set; }
    }
}
