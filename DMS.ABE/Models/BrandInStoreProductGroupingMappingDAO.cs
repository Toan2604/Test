using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    /// <summary>
    /// Nh&#243;m s&#7843;n ph&#7849;m c&#7911;a nh&#227;n hi&#7879;u trong c&#7917;a h&#224;ng
    /// </summary>
    public partial class BrandInStoreProductGroupingMappingDAO
    {
        public long BrandInStoreId { get; set; }
        public long ProductGroupingId { get; set; }

        public virtual BrandInStoreDAO BrandInStore { get; set; }
        public virtual ProductGroupingDAO ProductGrouping { get; set; }
    }
}
