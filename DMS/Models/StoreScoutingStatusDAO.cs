using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class StoreScoutingStatusDAO
    {
        public StoreScoutingStatusDAO()
        {
            StoreScoutings = new HashSet<StoreScoutingDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<StoreScoutingDAO> StoreScoutings { get; set; }
    }
}
