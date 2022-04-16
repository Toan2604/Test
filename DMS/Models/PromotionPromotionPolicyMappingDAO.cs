using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class PromotionPromotionPolicyMappingDAO
    {
        public long PromotionId { get; set; }
        public long PromotionPolicyId { get; set; }
        public string Note { get; set; }
        public long StatusId { get; set; }

        public virtual PromotionDAO Promotion { get; set; }
        public virtual PromotionPolicyDAO PromotionPolicy { get; set; }
        public virtual StatusDAO Status { get; set; }
    }
}
