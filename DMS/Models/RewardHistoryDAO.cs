using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class RewardHistoryDAO
    {
        public RewardHistoryDAO()
        {
            RewardHistoryContents = new HashSet<RewardHistoryContentDAO>();
        }

        public long Id { get; set; }
        public long AppUserId { get; set; }
        public long StoreId { get; set; }
        public long TurnCounter { get; set; }
        public decimal Revenue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }

        public virtual AppUserDAO AppUser { get; set; }
        public virtual StoreDAO Store { get; set; }
        public virtual ICollection<RewardHistoryContentDAO> RewardHistoryContents { get; set; }
    }
}
