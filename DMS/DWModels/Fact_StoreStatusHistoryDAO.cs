using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Fact_StoreStatusHistoryDAO
    {
        public long StoreStatusHistoryId { get; set; }
        public long StoreId { get; set; }
        public DateTime CreatedAt { get; set; }
        public long AppUserId { get; set; }
        public DateTime? PreviousCreatedAt { get; set; }
        public long? PreviousStoreStatusId { get; set; }
        public long StoreStatusId { get; set; }
    }
}
