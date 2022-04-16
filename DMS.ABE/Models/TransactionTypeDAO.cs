using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class TransactionTypeDAO
    {
        public TransactionTypeDAO()
        {
            DirectSalesOrderTransactions = new HashSet<DirectSalesOrderTransactionDAO>();
            IndirectSalesOrderTransactions = new HashSet<IndirectSalesOrderTransactionDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DirectSalesOrderTransactionDAO> DirectSalesOrderTransactions { get; set; }
        public virtual ICollection<IndirectSalesOrderTransactionDAO> IndirectSalesOrderTransactions { get; set; }
    }
}
