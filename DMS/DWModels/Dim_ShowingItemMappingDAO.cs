using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Dim_ShowingItemMappingDAO
    {
        public long ShowingItemMappingId { get; set; }
        public long? ShowingItemId { get; set; }
        public long? UnitOfMeasureId { get; set; }
        public long? ProductGroupingId { get; set; }
        public long? ProductTypeId { get; set; }
        public long? CategoryId { get; set; }
        public long? BrandId { get; set; }
        public long? SupplierId { get; set; }
    }
}
