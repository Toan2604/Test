using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class StoreApprovalStateDAO
    {
        public StoreApprovalStateDAO()
        {
            DirectSalesOrders = new HashSet<DirectSalesOrderDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DirectSalesOrderDAO> DirectSalesOrders { get; set; }
    }
}
