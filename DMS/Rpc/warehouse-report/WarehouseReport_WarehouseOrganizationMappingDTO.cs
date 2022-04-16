using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.warehouse_report
{
    public class WarehouseReport_WarehouseOrganizationMappingDTO : DataDTO
    {
        public long WarehouseId { get; set; }
        public long OrganizationId { get; set; }
        public WarehouseReport_OrganizationDTO Organization { get; set; }
        public WarehouseReport_WarehouseOrganizationMappingDTO() { }
        public WarehouseReport_WarehouseOrganizationMappingDTO(WarehouseOrganizationMapping WarehouseOrganizationMapping)
        {
            this.WarehouseId = WarehouseOrganizationMapping.WarehouseId;
            this.OrganizationId = WarehouseOrganizationMapping.OrganizationId;
            this.Organization = WarehouseOrganizationMapping.Organization == null ? null : new WarehouseReport_OrganizationDTO(WarehouseOrganizationMapping.Organization);
            this.Errors = WarehouseOrganizationMapping.Errors;
        }
    }

    public class WarehouseReport_WarehouseOrganizationMappingFilterDTO : FilterDTO
    {

        public IdFilter WarehouseId { get; set; }

        public IdFilter OrganizationId { get; set; }

        public WarehouseOrganizationMappingOrder OrderBy { get; set; }
    }
}
