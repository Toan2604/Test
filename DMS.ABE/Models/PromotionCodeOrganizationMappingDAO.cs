using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class PromotionCodeOrganizationMappingDAO
    {
        public long PromotionCodeId { get; set; }
        public long OrganizationId { get; set; }

        public virtual OrganizationDAO Organization { get; set; }
        public virtual PromotionCodeDAO PromotionCode { get; set; }
    }
}
