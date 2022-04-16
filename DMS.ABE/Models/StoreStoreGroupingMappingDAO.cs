using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class StoreStoreGroupingMappingDAO
    {
        public long StoreId { get; set; }
        public long StoreGroupingId { get; set; }

        public virtual StoreDAO Store { get; set; }
        public virtual StoreGroupingDAO StoreGrouping { get; set; }
    }
}
