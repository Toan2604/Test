using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class ProductGroupingHistoryDAO
    {
        public long Id { get; set; }
        public long ProductGroupingId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public long? AppUserId { get; set; }
    }
}
