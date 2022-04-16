using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class JobQueueDAO
    {
        public int Id { get; set; }
        public long JobId { get; set; }
        public string Queue { get; set; }
        public DateTime? FetchedAt { get; set; }
    }
}
