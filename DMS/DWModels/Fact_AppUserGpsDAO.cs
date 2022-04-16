using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Fact_AppUserGpsDAO
    {
        public long AppUserGpsId { get; set; }
        public long AppUserId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime GPSUpdatedAt { get; set; }
    }
}
