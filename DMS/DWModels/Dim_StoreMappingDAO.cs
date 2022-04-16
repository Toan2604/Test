using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Dim_StoreMappingDAO
    {
        public long StoreMappingId { get; set; }
        public long StoreId { get; set; }
        public long? OrganizationId { get; set; }
        public long? StoreGroupingId { get; set; }
        public long? StoreStatusId { get; set; }
        public long? StoreTypeId { get; set; }
        public long? WardId { get; set; }
    }
}
