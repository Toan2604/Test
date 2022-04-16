using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.warehouse
{
    public class Warehouse_WarehouseOrganizationMappingDTO : DataDTO
    {
        public long WarehouseId { get; set; }
        public long OrganizationId { get; set; }
        public Warehouse_OrganizationDTO Organization { get; set; }   
        public Warehouse_WarehouseOrganizationMappingDTO() {}
        public Warehouse_WarehouseOrganizationMappingDTO(WarehouseOrganizationMapping WarehouseOrganizationMapping)
        {
            this.WarehouseId = WarehouseOrganizationMapping.WarehouseId;
            this.OrganizationId = WarehouseOrganizationMapping.OrganizationId;
            this.Organization = WarehouseOrganizationMapping.Organization == null ? null : new Warehouse_OrganizationDTO(WarehouseOrganizationMapping.Organization);
            this.Errors = WarehouseOrganizationMapping.Errors;
        }
    }

    public class Warehouse_WarehouseOrganizationMappingFilterDTO : FilterDTO
    {
        
        public IdFilter WarehouseId { get; set; }
        
        public IdFilter OrganizationId { get; set; }
        
        public WarehouseOrganizationMappingOrder OrderBy { get; set; }
    }
}