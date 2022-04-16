using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class StoreStatusHistoryTypeDAO
    {
        public StoreStatusHistoryTypeDAO()
        {
            StoreStatusHistoryPreviousStoreStatuses = new HashSet<StoreStatusHistoryDAO>();
            StoreStatusHistoryStoreStatuses = new HashSet<StoreStatusHistoryDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<StoreStatusHistoryDAO> StoreStatusHistoryPreviousStoreStatuses { get; set; }
        public virtual ICollection<StoreStatusHistoryDAO> StoreStatusHistoryStoreStatuses { get; set; }
    }
}
