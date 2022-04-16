using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class ShowingCategoryDAO
    {
        public ShowingCategoryDAO()
        {
            BrandInStoreShowingCategoryMappings = new HashSet<BrandInStoreShowingCategoryMappingDAO>();
            InverseParent = new HashSet<ShowingCategoryDAO>();
            ShowingItems = new HashSet<ShowingItemDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
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

        public virtual ImageDAO Image { get; set; }
        public virtual ShowingCategoryDAO Parent { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<BrandInStoreShowingCategoryMappingDAO> BrandInStoreShowingCategoryMappings { get; set; }
        public virtual ICollection<ShowingCategoryDAO> InverseParent { get; set; }
        public virtual ICollection<ShowingItemDAO> ShowingItems { get; set; }
    }
}
