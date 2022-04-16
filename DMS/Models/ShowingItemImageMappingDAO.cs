using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class ShowingItemImageMappingDAO
    {
        public long ShowingItemId { get; set; }
        public long ImageId { get; set; }

        public virtual ImageDAO Image { get; set; }
        public virtual ShowingItemDAO ShowingItem { get; set; }
    }
}
