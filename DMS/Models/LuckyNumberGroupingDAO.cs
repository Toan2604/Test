using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class LuckyNumberGroupingDAO
    {
        public LuckyNumberGroupingDAO()
        {
            LuckyNumbers = new HashSet<LuckyNumberDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long OrganizationId { get; set; }
        public long StatusId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual OrganizationDAO Organization { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<LuckyNumberDAO> LuckyNumbers { get; set; }
    }
}
