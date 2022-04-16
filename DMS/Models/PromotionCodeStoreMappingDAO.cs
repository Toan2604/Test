using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class PromotionCodeStoreMappingDAO
    {
        public long PromotionCodeId { get; set; }
        public long StoreId { get; set; }

        public virtual PromotionCodeDAO PromotionCode { get; set; }
        public virtual StoreDAO Store { get; set; }
    }
}
