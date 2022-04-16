using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class CategoryDAO
    {
        public CategoryDAO()
        {
            InverseParent = new HashSet<CategoryDAO>();
            Products = new HashSet<ProductDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string Path { get; set; }
        public long Level { get; set; }
        public long StatusId { get; set; }
        public long? ImageId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }
        public long OrderNumber { get; set; }

        public virtual ImageDAO Image { get; set; }
        public virtual CategoryDAO Parent { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<CategoryDAO> InverseParent { get; set; }
        public virtual ICollection<ProductDAO> Products { get; set; }
    }
}
