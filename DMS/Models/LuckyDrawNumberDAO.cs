using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class LuckyDrawNumberDAO
    {
        public LuckyDrawNumberDAO()
        {
            LuckyDrawWinners = new HashSet<LuckyDrawWinnerDAO>();
        }

        public long Id { get; set; }
        public long LuckyDrawStructureId { get; set; }
        public bool Used { get; set; }

        public virtual LuckyDrawStructureDAO LuckyDrawStructure { get; set; }
        public virtual ICollection<LuckyDrawWinnerDAO> LuckyDrawWinners { get; set; }
    }
}
