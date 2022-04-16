using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class UsedVariationDAO
    {
        public UsedVariationDAO()
        {
            Products = new HashSet<ProductDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProductDAO> Products { get; set; }
    }
}
