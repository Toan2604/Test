using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class StoreScoutingImageMappingDAO
    {
        public long StoreScoutingId { get; set; }
        public long ImageId { get; set; }

        public virtual ImageDAO Image { get; set; }
        public virtual StoreScoutingDAO StoreScouting { get; set; }
    }
}
