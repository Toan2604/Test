using TrueSight.Common;
using System;
using System.Collections.Generic;
using DMS.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DMS.Entities
{
    public class WarehouseOrganizationMapping : DataEntity,  IEquatable<WarehouseOrganizationMapping>
    {
        public long WarehouseId { get; set; }
        public long OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public Warehouse Warehouse { get; set; }
        
        public bool Equals(WarehouseOrganizationMapping other)
        {
            if (other == null) return false;
            if (this.WarehouseId != other.WarehouseId) return false;
            if (this.OrganizationId != other.OrganizationId) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class WarehouseOrganizationMappingFilter : FilterEntity
    {
        public IdFilter WarehouseId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public List<WarehouseOrganizationMappingFilter> OrFilter { get; set; }
        public WarehouseOrganizationMappingOrder OrderBy {get; set;}
        public WarehouseOrganizationMappingSelect Selects {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WarehouseOrganizationMappingOrder
    {
        Warehouse = 0,
        Organization = 1,
    }

    [Flags]
    public enum WarehouseOrganizationMappingSelect:long
    {
        ALL = E.ALL,
        Warehouse = E._0,
        Organization = E._1,
    }
}
