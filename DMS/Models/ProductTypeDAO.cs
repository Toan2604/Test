using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class ProductTypeDAO
    {
        public ProductTypeDAO()
        {
            Products = new HashSet<ProductDAO>();
            PromotionProductTypes = new HashSet<PromotionProductTypeDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }

        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<ProductDAO> Products { get; set; }
        public virtual ICollection<PromotionProductTypeDAO> PromotionProductTypes { get; set; }
    }
}
