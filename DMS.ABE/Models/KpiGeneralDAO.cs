using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    /// <summary>
    /// KPI chung
    /// </summary>
    public partial class KpiGeneralDAO
    {
        public KpiGeneralDAO()
        {
            KpiGeneralContents = new HashSet<KpiGeneralContentDAO>();
        }

        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long EmployeeId { get; set; }
        public long KpiYearId { get; set; }
        public long StatusId { get; set; }
        public long CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }

        public virtual AppUserDAO Creator { get; set; }
        public virtual AppUserDAO Employee { get; set; }
        public virtual KpiYearDAO KpiYear { get; set; }
        public virtual OrganizationDAO Organization { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<KpiGeneralContentDAO> KpiGeneralContents { get; set; }
    }
}
