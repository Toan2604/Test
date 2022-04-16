using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class LuckyDrawWinnerDAO
    {
        public long Id { get; set; }
        public long LuckyDrawId { get; set; }
        public long? LuckyDrawStructureId { get; set; }
        public long LuckyDrawRegistrationId { get; set; }
        public DateTime Time { get; set; }
        public long? LuckyDrawNumberId { get; set; }
        public Guid RowId { get; set; }

        public virtual LuckyDrawDAO LuckyDraw { get; set; }
        public virtual LuckyDrawNumberDAO LuckyDrawNumber { get; set; }
        public virtual LuckyDrawRegistrationDAO LuckyDrawRegistration { get; set; }
        public virtual LuckyDrawStructureDAO LuckyDrawStructure { get; set; }
    }
}
