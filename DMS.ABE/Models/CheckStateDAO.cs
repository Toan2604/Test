using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class CheckStateDAO
    {
        public CheckStateDAO()
        {
            DirectSalesOrderContents = new HashSet<DirectSalesOrderContentDAO>();
            DirectSalesOrderInventoryCheckStates = new HashSet<DirectSalesOrderDAO>();
            DirectSalesOrderPromotions = new HashSet<DirectSalesOrderPromotionDAO>();
            DirectSalesOrderStoreBalanceCheckStates = new HashSet<DirectSalesOrderDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DirectSalesOrderContentDAO> DirectSalesOrderContents { get; set; }
        public virtual ICollection<DirectSalesOrderDAO> DirectSalesOrderInventoryCheckStates { get; set; }
        public virtual ICollection<DirectSalesOrderPromotionDAO> DirectSalesOrderPromotions { get; set; }
        public virtual ICollection<DirectSalesOrderDAO> DirectSalesOrderStoreBalanceCheckStates { get; set; }
    }
}
