using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class PriceListStoreTypeMappingDAO
    {
        public long PriceListId { get; set; }
        public long StoreTypeId { get; set; }

        public virtual PriceListDAO PriceList { get; set; }
        public virtual StoreTypeDAO StoreType { get; set; }
    }
}
