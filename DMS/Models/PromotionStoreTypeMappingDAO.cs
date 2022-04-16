using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class PromotionStoreTypeMappingDAO
    {
        public long PromotionId { get; set; }
        public long StoreTypeId { get; set; }

        public virtual PromotionDAO Promotion { get; set; }
        public virtual StoreTypeDAO StoreType { get; set; }
    }
}
