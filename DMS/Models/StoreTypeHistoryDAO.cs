using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class StoreTypeHistoryDAO
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public DateTime CreatedAt { get; set; }
        public long AppUserId { get; set; }
        public DateTime? PreviousCreatedAt { get; set; }
        public long? PreviousStoreTypeId { get; set; }
        public long StoreTypeId { get; set; }

        public virtual AppUserDAO AppUser { get; set; }
        public virtual StoreTypeDAO PreviousStoreType { get; set; }
        public virtual StoreDAO Store { get; set; }
        public virtual StoreTypeDAO StoreType { get; set; }
    }
}
