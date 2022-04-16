using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class RewardStatusDAO
    {
        public RewardStatusDAO()
        {
            LuckyNumbers = new HashSet<LuckyNumberDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LuckyNumberDAO> LuckyNumbers { get; set; }
    }
}
