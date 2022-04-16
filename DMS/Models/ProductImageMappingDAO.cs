using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class ProductImageMappingDAO
    {
        public long ProductId { get; set; }
        public long ImageId { get; set; }

        public virtual ImageDAO Image { get; set; }
        public virtual ProductDAO Product { get; set; }
    }
}
