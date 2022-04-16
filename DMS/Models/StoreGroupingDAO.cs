using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class StoreGroupingDAO
    {
        public StoreGroupingDAO()
        {
            InverseParent = new HashSet<StoreGroupingDAO>();
            LuckyDrawStoreGroupingMappings = new HashSet<LuckyDrawStoreGroupingMappingDAO>();
            PriceListStoreGroupingMappings = new HashSet<PriceListStoreGroupingMappingDAO>();
            PromotionStoreGroupingMappings = new HashSet<PromotionStoreGroupingMappingDAO>();
            StoreStoreGroupingMappings = new HashSet<StoreStoreGroupingMappingDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string Path { get; set; }
        public long Level { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }

        public virtual StoreGroupingDAO Parent { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<StoreGroupingDAO> InverseParent { get; set; }
        public virtual ICollection<LuckyDrawStoreGroupingMappingDAO> LuckyDrawStoreGroupingMappings { get; set; }
        public virtual ICollection<PriceListStoreGroupingMappingDAO> PriceListStoreGroupingMappings { get; set; }
        public virtual ICollection<PromotionStoreGroupingMappingDAO> PromotionStoreGroupingMappings { get; set; }
        public virtual ICollection<StoreStoreGroupingMappingDAO> StoreStoreGroupingMappings { get; set; }
    }
}
