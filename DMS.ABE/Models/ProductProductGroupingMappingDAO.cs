using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class ProductProductGroupingMappingDAO
    {
        public long ProductId { get; set; }
        public long ProductGroupingId { get; set; }

        public virtual ProductDAO Product { get; set; }
        public virtual ProductGroupingDAO ProductGrouping { get; set; }
    }
}
