using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Fact_StoreHistoryDAO
    {
        public long StoreHistoryId { get; set; }
        public long StoreId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? AppUserId { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public long StoreStatusId { get; set; }
        public long? EstimatedRevenueId { get; set; }
    }
}
