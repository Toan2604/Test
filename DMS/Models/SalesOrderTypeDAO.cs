using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class SalesOrderTypeDAO
    {
        public SalesOrderTypeDAO()
        {
            PriceLists = new HashSet<PriceListDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PriceListDAO> PriceLists { get; set; }
    }
}
