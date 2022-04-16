using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class VariationGroupingDAO
    {
        public VariationGroupingDAO()
        {
            Variations = new HashSet<VariationDAO>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long ProductId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }

        public virtual ProductDAO Product { get; set; }
        public virtual ICollection<VariationDAO> Variations { get; set; }
    }
}
