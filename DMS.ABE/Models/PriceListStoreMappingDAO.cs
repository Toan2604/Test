using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class PriceListStoreMappingDAO
    {
        public long PriceListId { get; set; }
        public long StoreId { get; set; }

        public virtual PriceListDAO PriceList { get; set; }
        public virtual StoreDAO Store { get; set; }
    }
}
