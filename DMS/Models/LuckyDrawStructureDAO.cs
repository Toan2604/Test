using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class LuckyDrawStructureDAO
    {
        public LuckyDrawStructureDAO()
        {
            LuckyDrawNumbers = new HashSet<LuckyDrawNumberDAO>();
            LuckyDrawWinners = new HashSet<LuckyDrawWinnerDAO>();
        }

        public long Id { get; set; }
        public long LuckyDrawId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public long Quantity { get; set; }

        public virtual LuckyDrawDAO LuckyDraw { get; set; }
        public virtual ICollection<LuckyDrawNumberDAO> LuckyDrawNumbers { get; set; }
        public virtual ICollection<LuckyDrawWinnerDAO> LuckyDrawWinners { get; set; }
    }
}
