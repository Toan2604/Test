using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class KpiCriteriaGeneralDAO
    {
        public KpiCriteriaGeneralDAO()
        {
            KpiGeneralContents = new HashSet<KpiGeneralContentDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }

        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<KpiGeneralContentDAO> KpiGeneralContents { get; set; }
    }
}
