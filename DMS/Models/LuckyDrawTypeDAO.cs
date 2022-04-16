using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class LuckyDrawTypeDAO
    {
        public LuckyDrawTypeDAO()
        {
            LuckyDraws = new HashSet<LuckyDrawDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LuckyDrawDAO> LuckyDraws { get; set; }
    }
}
