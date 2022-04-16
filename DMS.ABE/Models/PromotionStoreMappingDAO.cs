using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class PromotionStoreMappingDAO
    {
        public long PromotionId { get; set; }
        public long StoreId { get; set; }

        public virtual PromotionDAO Promotion { get; set; }
        public virtual StoreDAO Store { get; set; }
    }
}
