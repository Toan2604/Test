using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class AppUserGpsDAO
    {
        public long Id { get; set; }
        public long AppUserId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime GPSUpdatedAt { get; set; }

        public virtual AppUserDAO AppUser { get; set; }
    }
}
