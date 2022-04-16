using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class PromotionCodeProductMappingDAO
    {
        public long PromotionCodeId { get; set; }
        public long ProductId { get; set; }

        public virtual ProductDAO Product { get; set; }
        public virtual PromotionCodeDAO PromotionCode { get; set; }
    }
}
