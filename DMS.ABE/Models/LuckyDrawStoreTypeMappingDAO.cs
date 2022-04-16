using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class LuckyDrawStoreTypeMappingDAO
    {
        public long LuckyDrawId { get; set; }
        public long StoreTypeId { get; set; }

        public virtual LuckyDrawDAO LuckyDraw { get; set; }
        public virtual StoreTypeDAO StoreType { get; set; }
    }
}
