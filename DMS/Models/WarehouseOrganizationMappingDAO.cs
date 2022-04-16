using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class WarehouseOrganizationMappingDAO
    {
        public long WarehouseId { get; set; }
        public long OrganizationId { get; set; }

        public virtual OrganizationDAO Organization { get; set; }
        public virtual WarehouseDAO Warehouse { get; set; }
    }
}
