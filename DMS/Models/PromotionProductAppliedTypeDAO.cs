using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class PromotionProductAppliedTypeDAO
    {
        public PromotionProductAppliedTypeDAO()
        {
            PromotionCodes = new HashSet<PromotionCodeDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PromotionCodeDAO> PromotionCodes { get; set; }
    }
}
