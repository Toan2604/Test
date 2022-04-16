using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class ProductGroupingDAO
    {
        public ProductGroupingDAO()
        {
            BrandInStoreProductGroupingMappings = new HashSet<BrandInStoreProductGroupingMappingDAO>();
            InverseParent = new HashSet<ProductGroupingDAO>();
            KpiProductGroupingContents = new HashSet<KpiProductGroupingContentDAO>();
            ProductProductGroupingMappings = new HashSet<ProductProductGroupingMappingDAO>();
            PromotionProductGroupings = new HashSet<PromotionProductGroupingDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long? ParentId { get; set; }
        public string Path { get; set; }
        public long Level { get; set; }
        public bool HasChildren { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }

        public virtual ProductGroupingDAO Parent { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<BrandInStoreProductGroupingMappingDAO> BrandInStoreProductGroupingMappings { get; set; }
        public virtual ICollection<ProductGroupingDAO> InverseParent { get; set; }
        public virtual ICollection<KpiProductGroupingContentDAO> KpiProductGroupingContents { get; set; }
        public virtual ICollection<ProductProductGroupingMappingDAO> ProductProductGroupingMappings { get; set; }
        public virtual ICollection<PromotionProductGroupingDAO> PromotionProductGroupings { get; set; }
    }
}
