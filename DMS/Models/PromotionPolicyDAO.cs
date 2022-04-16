using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class PromotionPolicyDAO
    {
        public PromotionPolicyDAO()
        {
            PromotionCombos = new HashSet<PromotionComboDAO>();
            PromotionDirectSalesOrders = new HashSet<PromotionDirectSalesOrderDAO>();
            PromotionProductGroupings = new HashSet<PromotionProductGroupingDAO>();
            PromotionProductTypes = new HashSet<PromotionProductTypeDAO>();
            PromotionProducts = new HashSet<PromotionProductDAO>();
            PromotionPromotionPolicyMappings = new HashSet<PromotionPromotionPolicyMappingDAO>();
            PromotionSamePrices = new HashSet<PromotionSamePriceDAO>();
            PromotionStoreGroupings = new HashSet<PromotionStoreGroupingDAO>();
            PromotionStoreTypes = new HashSet<PromotionStoreTypeDAO>();
            PromotionStores = new HashSet<PromotionStoreDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PromotionComboDAO> PromotionCombos { get; set; }
        public virtual ICollection<PromotionDirectSalesOrderDAO> PromotionDirectSalesOrders { get; set; }
        public virtual ICollection<PromotionProductGroupingDAO> PromotionProductGroupings { get; set; }
        public virtual ICollection<PromotionProductTypeDAO> PromotionProductTypes { get; set; }
        public virtual ICollection<PromotionProductDAO> PromotionProducts { get; set; }
        public virtual ICollection<PromotionPromotionPolicyMappingDAO> PromotionPromotionPolicyMappings { get; set; }
        public virtual ICollection<PromotionSamePriceDAO> PromotionSamePrices { get; set; }
        public virtual ICollection<PromotionStoreGroupingDAO> PromotionStoreGroupings { get; set; }
        public virtual ICollection<PromotionStoreTypeDAO> PromotionStoreTypes { get; set; }
        public virtual ICollection<PromotionStoreDAO> PromotionStores { get; set; }
    }
}
