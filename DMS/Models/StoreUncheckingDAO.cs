using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class StoreUncheckingDAO
    {
        public long Id { get; set; }
        public long AppUserId { get; set; }
        public long StoreId { get; set; }
        public long OrganizationId { get; set; }
        public DateTime Date { get; set; }

        public virtual AppUserDAO AppUser { get; set; }
        public virtual OrganizationDAO Organization { get; set; }
        public virtual StoreDAO Store { get; set; }
    }
}
