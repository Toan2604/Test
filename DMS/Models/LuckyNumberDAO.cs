using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class LuckyNumberDAO
    {
        public LuckyNumberDAO()
        {
            RewardHistoryContents = new HashSet<RewardHistoryContentDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public long LuckyNumberGroupingId { get; set; }
        public long RewardStatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }
        public DateTime? UsedAt { get; set; }

        public virtual LuckyNumberGroupingDAO LuckyNumberGrouping { get; set; }
        public virtual RewardStatusDAO RewardStatus { get; set; }
        public virtual ICollection<RewardHistoryContentDAO> RewardHistoryContents { get; set; }
    }
}
