using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class ShowingItemDAO
    {
        public ShowingItemDAO()
        {
            ShowingItemImageMappings = new HashSet<ShowingItemImageMappingDAO>();
            ShowingOrderContentWithDraws = new HashSet<ShowingOrderContentWithDrawDAO>();
            ShowingOrderContents = new HashSet<ShowingOrderContentDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long ShowingCategoryId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal SalePrice { get; set; }
        public string ERPCode { get; set; }
        public string Description { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }

        public virtual ShowingCategoryDAO ShowingCategory { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual UnitOfMeasureDAO UnitOfMeasure { get; set; }
        public virtual ICollection<ShowingItemImageMappingDAO> ShowingItemImageMappings { get; set; }
        public virtual ICollection<ShowingOrderContentWithDrawDAO> ShowingOrderContentWithDraws { get; set; }
        public virtual ICollection<ShowingOrderContentDAO> ShowingOrderContents { get; set; }
    }
}
