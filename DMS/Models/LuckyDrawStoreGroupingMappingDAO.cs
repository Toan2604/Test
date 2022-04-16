using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class LuckyDrawStoreGroupingMappingDAO
    {
        public long LuckyDrawId { get; set; }
        public long StoreGroupingId { get; set; }

        public virtual LuckyDrawDAO LuckyDraw { get; set; }
        public virtual StoreGroupingDAO StoreGrouping { get; set; }
    }
}
