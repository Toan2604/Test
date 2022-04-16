using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class PromotionDiscountTypeDAO
    {
        public PromotionDiscountTypeDAO()
        {
            PromotionCodes = new HashSet<PromotionCodeDAO>();
            PromotionCombos = new HashSet<PromotionComboDAO>();
            PromotionDirectSalesOrders = new HashSet<PromotionDirectSalesOrderDAO>();
            PromotionProductGroupings = new HashSet<PromotionProductGroupingDAO>();
            PromotionProductTypes = new HashSet<PromotionProductTypeDAO>();
            PromotionProducts = new HashSet<PromotionProductDAO>();
            PromotionStoreGroupings = new HashSet<PromotionStoreGroupingDAO>();
            PromotionStoreTypes = new HashSet<PromotionStoreTypeDAO>();
            PromotionStores = new HashSet<PromotionStoreDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PromotionCodeDAO> PromotionCodes { get; set; }
        public virtual ICollection<PromotionComboDAO> PromotionCombos { get; set; }
        public virtual ICollection<PromotionDirectSalesOrderDAO> PromotionDirectSalesOrders { get; set; }
        public virtual ICollection<PromotionProductGroupingDAO> PromotionProductGroupings { get; set; }
        public virtual ICollection<PromotionProductTypeDAO> PromotionProductTypes { get; set; }
        public virtual ICollection<PromotionProductDAO> PromotionProducts { get; set; }
        public virtual ICollection<PromotionStoreGroupingDAO> PromotionStoreGroupings { get; set; }
        public virtual ICollection<PromotionStoreTypeDAO> PromotionStoreTypes { get; set; }
        public virtual ICollection<PromotionStoreDAO> PromotionStores { get; set; }
    }
}
