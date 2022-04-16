using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class KpiItemTypeDAO
    {
        public KpiItemTypeDAO()
        {
            KpiItems = new HashSet<KpiItemDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<KpiItemDAO> KpiItems { get; set; }
    }
}
