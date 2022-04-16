using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class ItemImageMappingDAO
    {
        public long ItemId { get; set; }
        public long ImageId { get; set; }

        public virtual ImageDAO Image { get; set; }
        public virtual ItemDAO Item { get; set; }
    }
}
