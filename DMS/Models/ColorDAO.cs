using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class ColorDAO
    {
        public ColorDAO()
        {
            StoreTypes = new HashSet<StoreTypeDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<StoreTypeDAO> StoreTypes { get; set; }
    }
}
