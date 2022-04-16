using System;
using System.Collections.Generic;

namespace DMS.Models
{
    /// <summary>
    /// &#160;T&#7893;ng h&#7907;p c&#225;c s&#7843;n ph&#7849;m c&#7911;a &#273;&#417;n h&#224;ng gi&#225;n ti&#7871;p
    /// </summary>
    public partial class IndirectSalesOrderTransactionDAO
    {
        public long Id { get; set; }
        public long IndirectSalesOrderId { get; set; }
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
        public virtual IndirectSalesOrderDAO IndirectSalesOrder { get; set; }
        public virtual ItemDAO Item { get; set; }
        public virtual OrganizationDAO Organization { get; set; }
        public virtual AppUserDAO SalesEmployee { get; set; }
        public virtual TransactionTypeDAO Type { get; set; }
        public virtual UnitOfMeasureDAO UnitOfMeasure { get; set; }
    }
}
