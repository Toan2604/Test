using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class StoreImageMappingDAO
    {
        public long StoreId { get; set; }
        public long ImageId { get; set; }

        public virtual ImageDAO Image { get; set; }
        public virtual StoreDAO Store { get; set; }
    }
}
