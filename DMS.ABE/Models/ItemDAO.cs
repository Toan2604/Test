using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class ItemDAO
    {
        public ItemDAO()
        {
            DirectSalesOrderContents = new HashSet<DirectSalesOrderContentDAO>();
            DirectSalesOrderPromotions = new HashSet<DirectSalesOrderPromotionDAO>();
            DirectSalesOrderTransactions = new HashSet<DirectSalesOrderTransactionDAO>();
            IndirectSalesOrderContents = new HashSet<IndirectSalesOrderContentDAO>();
            IndirectSalesOrderPromotions = new HashSet<IndirectSalesOrderPromotionDAO>();
            IndirectSalesOrderTransactions = new HashSet<IndirectSalesOrderTransactionDAO>();
            Inventories = new HashSet<InventoryDAO>();
            ItemHistories = new HashSet<ItemHistoryDAO>();
            ItemImageMappings = new HashSet<ItemImageMappingDAO>();
            KpiItemContents = new HashSet<KpiItemContentDAO>();
            KpiProductGroupingContentItemMappings = new HashSet<KpiProductGroupingContentItemMappingDAO>();
            PriceListItemHistories = new HashSet<PriceListItemHistoryDAO>();
            PriceListItemMappings = new HashSet<PriceListItemMappingDAO>();
            PromotionComboInItemMappings = new HashSet<PromotionComboInItemMappingDAO>();
            PromotionComboOutItemMappings = new HashSet<PromotionComboOutItemMappingDAO>();
            PromotionDirectSalesOrderItemMappings = new HashSet<PromotionDirectSalesOrderItemMappingDAO>();
            PromotionProductGroupingItemMappings = new HashSet<PromotionProductGroupingItemMappingDAO>();
            PromotionProductItemMappings = new HashSet<PromotionProductItemMappingDAO>();
            PromotionProductTypeItemMappings = new HashSet<PromotionProductTypeItemMappingDAO>();
            PromotionStoreGroupingItemMappings = new HashSet<PromotionStoreGroupingItemMappingDAO>();
            PromotionStoreItemMappings = new HashSet<PromotionStoreItemMappingDAO>();
            PromotionStoreTypeItemMappings = new HashSet<PromotionStoreTypeItemMappingDAO>();
        }

        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Code { get; set; }
        public string ERPCode { get; set; }
        public string Name { get; set; }
        public string ScanCode { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? RetailPrice { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }

        public virtual ProductDAO Product { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<DirectSalesOrderContentDAO> DirectSalesOrderContents { get; set; }
        public virtual ICollection<DirectSalesOrderPromotionDAO> DirectSalesOrderPromotions { get; set; }
        public virtual ICollection<DirectSalesOrderTransactionDAO> DirectSalesOrderTransactions { get; set; }
        public virtual ICollection<IndirectSalesOrderContentDAO> IndirectSalesOrderContents { get; set; }
        public virtual ICollection<IndirectSalesOrderPromotionDAO> IndirectSalesOrderPromotions { get; set; }
        public virtual ICollection<IndirectSalesOrderTransactionDAO> IndirectSalesOrderTransactions { get; set; }
        public virtual ICollection<InventoryDAO> Inventories { get; set; }
        public virtual ICollection<ItemHistoryDAO> ItemHistories { get; set; }
        public virtual ICollection<ItemImageMappingDAO> ItemImageMappings { get; set; }
        public virtual ICollection<KpiItemContentDAO> KpiItemContents { get; set; }
        public virtual ICollection<KpiProductGroupingContentItemMappingDAO> KpiProductGroupingContentItemMappings { get; set; }
        public virtual ICollection<PriceListItemHistoryDAO> PriceListItemHistories { get; set; }
        public virtual ICollection<PriceListItemMappingDAO> PriceListItemMappings { get; set; }
        public virtual ICollection<PromotionComboInItemMappingDAO> PromotionComboInItemMappings { get; set; }
        public virtual ICollection<PromotionComboOutItemMappingDAO> PromotionComboOutItemMappings { get; set; }
        public virtual ICollection<PromotionDirectSalesOrderItemMappingDAO> PromotionDirectSalesOrderItemMappings { get; set; }
        public virtual ICollection<PromotionProductGroupingItemMappingDAO> PromotionProductGroupingItemMappings { get; set; }
        public virtual ICollection<PromotionProductItemMappingDAO> PromotionProductItemMappings { get; set; }
        public virtual ICollection<PromotionProductTypeItemMappingDAO> PromotionProductTypeItemMappings { get; set; }
        public virtual ICollection<PromotionStoreGroupingItemMappingDAO> PromotionStoreGroupingItemMappings { get; set; }
        public virtual ICollection<PromotionStoreItemMappingDAO> PromotionStoreItemMappings { get; set; }
        public virtual ICollection<PromotionStoreTypeItemMappingDAO> PromotionStoreTypeItemMappings { get; set; }
    }
}
