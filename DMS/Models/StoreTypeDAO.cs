using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class StoreTypeDAO
    {
        public StoreTypeDAO()
        {
            DirectSalesOrders = new HashSet<DirectSalesOrderDAO>();
            IndirectSalesOrders = new HashSet<IndirectSalesOrderDAO>();
            LuckyDrawStoreTypeMappings = new HashSet<LuckyDrawStoreTypeMappingDAO>();
            PriceListStoreTypeMappings = new HashSet<PriceListStoreTypeMappingDAO>();
            PromotionStoreTypeMappings = new HashSet<PromotionStoreTypeMappingDAO>();
            StoreTypeHistoryPreviousStoreTypes = new HashSet<StoreTypeHistoryDAO>();
            StoreTypeHistoryStoreTypes = new HashSet<StoreTypeHistoryDAO>();
            Stores = new HashSet<StoreDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? ColorId { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }

        public virtual ColorDAO Color { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<DirectSalesOrderDAO> DirectSalesOrders { get; set; }
        public virtual ICollection<IndirectSalesOrderDAO> IndirectSalesOrders { get; set; }
        public virtual ICollection<LuckyDrawStoreTypeMappingDAO> LuckyDrawStoreTypeMappings { get; set; }
        public virtual ICollection<PriceListStoreTypeMappingDAO> PriceListStoreTypeMappings { get; set; }
        public virtual ICollection<PromotionStoreTypeMappingDAO> PromotionStoreTypeMappings { get; set; }
        public virtual ICollection<StoreTypeHistoryDAO> StoreTypeHistoryPreviousStoreTypes { get; set; }
        public virtual ICollection<StoreTypeHistoryDAO> StoreTypeHistoryStoreTypes { get; set; }
        public virtual ICollection<StoreDAO> Stores { get; set; }
    }
}
