using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class WarehouseDAO
    {
        public WarehouseDAO()
        {
            Inventories = new HashSet<InventoryDAO>();
            WarehouseOrganizationMappings = new HashSet<WarehouseOrganizationMappingDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? WardId { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }

        public virtual DistrictDAO District { get; set; }
        public virtual ProvinceDAO Province { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual WardDAO Ward { get; set; }
        public virtual ICollection<InventoryDAO> Inventories { get; set; }
        public virtual ICollection<WarehouseOrganizationMappingDAO> WarehouseOrganizationMappings { get; set; }
    }
}
