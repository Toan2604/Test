using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Dim_ItemMappingDAO
    {
        public long ItemMappingId { get; set; }
        public long ItemId { get; set; }
        public long? ProductId { get; set; }
        public long? ProductGroupingId { get; set; }
        public long? ProductTypeId { get; set; }
        public long? UnitOfMeasureId { get; set; }
        public long? CategoryId { get; set; }
        public long? BrandId { get; set; }
        public long? SupplierId { get; set; }
    }
}
