using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class UnitOfMeasureDAO
    {
        public UnitOfMeasureDAO()
        {
            DirectSalesOrderContentPrimaryUnitOfMeasures = new HashSet<DirectSalesOrderContentDAO>();
            DirectSalesOrderContentUnitOfMeasures = new HashSet<DirectSalesOrderContentDAO>();
            DirectSalesOrderPromotionPrimaryUnitOfMeasures = new HashSet<DirectSalesOrderPromotionDAO>();
            DirectSalesOrderPromotionUnitOfMeasures = new HashSet<DirectSalesOrderPromotionDAO>();
            DirectSalesOrderTransactions = new HashSet<DirectSalesOrderTransactionDAO>();
            IndirectSalesOrderContentPrimaryUnitOfMeasures = new HashSet<IndirectSalesOrderContentDAO>();
            IndirectSalesOrderContentUnitOfMeasures = new HashSet<IndirectSalesOrderContentDAO>();
            IndirectSalesOrderPromotionPrimaryUnitOfMeasures = new HashSet<IndirectSalesOrderPromotionDAO>();
            IndirectSalesOrderPromotionUnitOfMeasures = new HashSet<IndirectSalesOrderPromotionDAO>();
            IndirectSalesOrderTransactions = new HashSet<IndirectSalesOrderTransactionDAO>();
            Products = new HashSet<ProductDAO>();
            ShowingItems = new HashSet<ShowingItemDAO>();
            ShowingOrderContentWithDraws = new HashSet<ShowingOrderContentWithDrawDAO>();
            ShowingOrderContents = new HashSet<ShowingOrderContentDAO>();
            UnitOfMeasureGroupingContents = new HashSet<UnitOfMeasureGroupingContentDAO>();
            UnitOfMeasureGroupings = new HashSet<UnitOfMeasureGroupingDAO>();
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
        public bool IsDecimal { get; set; }

        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<DirectSalesOrderContentDAO> DirectSalesOrderContentPrimaryUnitOfMeasures { get; set; }
        public virtual ICollection<DirectSalesOrderContentDAO> DirectSalesOrderContentUnitOfMeasures { get; set; }
        public virtual ICollection<DirectSalesOrderPromotionDAO> DirectSalesOrderPromotionPrimaryUnitOfMeasures { get; set; }
        public virtual ICollection<DirectSalesOrderPromotionDAO> DirectSalesOrderPromotionUnitOfMeasures { get; set; }
        public virtual ICollection<DirectSalesOrderTransactionDAO> DirectSalesOrderTransactions { get; set; }
        public virtual ICollection<IndirectSalesOrderContentDAO> IndirectSalesOrderContentPrimaryUnitOfMeasures { get; set; }
        public virtual ICollection<IndirectSalesOrderContentDAO> IndirectSalesOrderContentUnitOfMeasures { get; set; }
        public virtual ICollection<IndirectSalesOrderPromotionDAO> IndirectSalesOrderPromotionPrimaryUnitOfMeasures { get; set; }
        public virtual ICollection<IndirectSalesOrderPromotionDAO> IndirectSalesOrderPromotionUnitOfMeasures { get; set; }
        public virtual ICollection<IndirectSalesOrderTransactionDAO> IndirectSalesOrderTransactions { get; set; }
        public virtual ICollection<ProductDAO> Products { get; set; }
        public virtual ICollection<ShowingItemDAO> ShowingItems { get; set; }
        public virtual ICollection<ShowingOrderContentWithDrawDAO> ShowingOrderContentWithDraws { get; set; }
        public virtual ICollection<ShowingOrderContentDAO> ShowingOrderContents { get; set; }
        public virtual ICollection<UnitOfMeasureGroupingContentDAO> UnitOfMeasureGroupingContents { get; set; }
        public virtual ICollection<UnitOfMeasureGroupingDAO> UnitOfMeasureGroupings { get; set; }
    }
}
