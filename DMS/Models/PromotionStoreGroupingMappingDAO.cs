using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class PromotionStoreGroupingMappingDAO
    {
        public long PromotionId { get; set; }
        public long StoreGroupingId { get; set; }

        public virtual PromotionDAO Promotion { get; set; }
        public virtual StoreGroupingDAO StoreGrouping { get; set; }
    }
}
