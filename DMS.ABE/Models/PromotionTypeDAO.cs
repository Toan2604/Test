using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class PromotionTypeDAO
    {
        public PromotionTypeDAO()
        {
            PromotionCodes = new HashSet<PromotionCodeDAO>();
            Promotions = new HashSet<PromotionDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PromotionCodeDAO> PromotionCodes { get; set; }
        public virtual ICollection<PromotionDAO> Promotions { get; set; }
    }
}
