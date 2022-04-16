using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class NationDAO
    {
        public NationDAO()
        {
            Suppliers = new HashSet<SupplierDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? Priority { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }

        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<SupplierDAO> Suppliers { get; set; }
    }
}
