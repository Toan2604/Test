using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class LuckyDrawRegistrationDAO
    {
        public LuckyDrawRegistrationDAO()
        {
            LuckyDrawWinners = new HashSet<LuckyDrawWinnerDAO>();
        }

        public long Id { get; set; }
        public long LuckyDrawId { get; set; }
        public long AppUserId { get; set; }
        public long StoreId { get; set; }
        public decimal Revenue { get; set; }
        public long TurnCounter { get; set; }
        public bool IsDrawnByStore { get; set; }
        public DateTime Time { get; set; }

        public virtual AppUserDAO AppUser { get; set; }
        public virtual LuckyDrawDAO LuckyDraw { get; set; }
        public virtual StoreDAO Store { get; set; }
        public virtual ICollection<LuckyDrawWinnerDAO> LuckyDrawWinners { get; set; }
    }
}
