using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class PromotionDAO
    {
        public PromotionDAO()
        {
            PromotionCombos = new HashSet<PromotionComboDAO>();
            PromotionDirectSalesOrders = new HashSet<PromotionDirectSalesOrderDAO>();
            PromotionProductGroupings = new HashSet<PromotionProductGroupingDAO>();
            PromotionProductTypes = new HashSet<PromotionProductTypeDAO>();
            PromotionProducts = new HashSet<PromotionProductDAO>();
            PromotionPromotionPolicyMappings = new HashSet<PromotionPromotionPolicyMappingDAO>();
            PromotionSamePrices = new HashSet<PromotionSamePriceDAO>();
            PromotionStoreGroupingMappings = new HashSet<PromotionStoreGroupingMappingDAO>();
            PromotionStoreGroupings = new HashSet<PromotionStoreGroupingDAO>();
            PromotionStoreMappings = new HashSet<PromotionStoreMappingDAO>();
            PromotionStoreTypeMappings = new HashSet<PromotionStoreTypeMappingDAO>();
            PromotionStoreTypes = new HashSet<PromotionStoreTypeDAO>();
            PromotionStores = new HashSet<PromotionStoreDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long OrganizationId { get; set; }
        public long PromotionTypeId { get; set; }
        public string Note { get; set; }
        public long Priority { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }

        public virtual OrganizationDAO Organization { get; set; }
        public virtual PromotionTypeDAO PromotionType { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<PromotionComboDAO> PromotionCombos { get; set; }
        public virtual ICollection<PromotionDirectSalesOrderDAO> PromotionDirectSalesOrders { get; set; }
        public virtual ICollection<PromotionProductGroupingDAO> PromotionProductGroupings { get; set; }
        public virtual ICollection<PromotionProductTypeDAO> PromotionProductTypes { get; set; }
        public virtual ICollection<PromotionProductDAO> PromotionProducts { get; set; }
        public virtual ICollection<PromotionPromotionPolicyMappingDAO> PromotionPromotionPolicyMappings { get; set; }
        public virtual ICollection<PromotionSamePriceDAO> PromotionSamePrices { get; set; }
        public virtual ICollection<PromotionStoreGroupingMappingDAO> PromotionStoreGroupingMappings { get; set; }
        public virtual ICollection<PromotionStoreGroupingDAO> PromotionStoreGroupings { get; set; }
        public virtual ICollection<PromotionStoreMappingDAO> PromotionStoreMappings { get; set; }
        public virtual ICollection<PromotionStoreTypeMappingDAO> PromotionStoreTypeMappings { get; set; }
        public virtual ICollection<PromotionStoreTypeDAO> PromotionStoreTypes { get; set; }
        public virtual ICollection<PromotionStoreDAO> PromotionStores { get; set; }
    }
}
