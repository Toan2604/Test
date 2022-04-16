using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class LuckyDrawStoreMappingDAO
    {
        public long LuckyDrawId { get; set; }
        public long StoreId { get; set; }

        public virtual LuckyDrawDAO LuckyDraw { get; set; }
        public virtual StoreDAO Store { get; set; }
    }
}
