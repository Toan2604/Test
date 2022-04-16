using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class StoreHistoryDAO
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? AppUserId { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public long StoreStatusId { get; set; }
        public long? EstimatedRevenueId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
