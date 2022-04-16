using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Fact_BrandHistoryDAO
    {
        public long BrandHistoryId { get; set; }
        public long BrandId { get; set; }
        public long? AppUserId { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? Used { get; set; }
    }
}
