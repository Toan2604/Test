using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class Warehouse : DataEntity, IEquatable<Warehouse>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? WardId { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }
        public District District { get; set; }
        public Province Province { get; set; }
        public Status Status { get; set; }
        public Ward Ward { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<Inventory> Inventories { get; set; }
        public List<WarehouseOrganizationMapping> WarehouseOrganizationMappings { get; set; }

        public bool Equals(Warehouse other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class WarehouseFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Address { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        public IdFilter WardId { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter ProductTypeId { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public List<WarehouseFilter> OrFilter { get; set; }
        public WarehouseOrder OrderBy { get; set; }
        public WarehouseSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WarehouseOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Address = 3,
        Organization = 4,
        Province = 5,
        District = 6,
        Ward = 7,
        Status = 8,
        UpdatedAt = 9,
        CreatedAt = 10,
    }

    [Flags]
    public enum WarehouseSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Address = E._3,
        Province = E._5,
        District = E._6,
        Ward = E._7,
        Status = E._8,
        CreatedAt = E._9,
        UpdatedAt = E._10,
        WarehouseOrganizationMappings = E._10,
    }
}
