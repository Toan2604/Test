using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    /// <summary>
    /// Danh s&#225;ch t&#7845;t c&#7843; c&#225;c s&#7843;n ph&#7849;m b&#225;n v&#224; khuy&#7871;n m&#227;i
    /// </summary>
    public partial class DirectSalesOrderTransactionDAO
    {
        public long Id { get; set; }
        public long DirectSalesOrderId { get; set; }
        public long OrganizationId { get; set; }
        public long BuyerStoreId { get; set; }
        public long SalesEmployeeId { get; set; }
        public long ItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal Quantity { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Revenue { get; set; }
        public long TypeId { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual StoreDAO BuyerStore { get; set; }
        public virtual DirectSalesOrderDAO DirectSalesOrder { get; set; }
        public virtual ItemDAO Item { get; set; }
        public virtual OrganizationDAO Organization { get; set; }
        public virtual AppUserDAO SalesEmployee { get; set; }
        public virtual TransactionTypeDAO Type { get; set; }
        public virtual UnitOfMeasureDAO UnitOfMeasure { get; set; }
    }
}
