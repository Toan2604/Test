using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class PriceListStoreGroupingMappingDAO
    {
        public long PriceListId { get; set; }
        public long StoreGroupingId { get; set; }

        public virtual PriceListDAO PriceList { get; set; }
        public virtual StoreGroupingDAO StoreGrouping { get; set; }
    }
}
