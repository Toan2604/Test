using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class PromotionCodeHistoryDAO
    {
        public long Id { get; set; }
        public long PromotionCodeId { get; set; }
        public DateTime AppliedAt { get; set; }
        public Guid RowId { get; set; }

        public virtual PromotionCodeDAO PromotionCode { get; set; }
    }
}
