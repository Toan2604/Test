using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class ItemHistoryDAO
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public DateTime Time { get; set; }
        public long ModifierId { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }

        public virtual ItemDAO Item { get; set; }
        public virtual AppUserDAO Modifier { get; set; }
    }
}
