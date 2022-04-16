using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class StoreStatusDAO
    {
        public StoreStatusDAO()
        {
            Stores = new HashSet<StoreDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<StoreDAO> Stores { get; set; }
    }
}
