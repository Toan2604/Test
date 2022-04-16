using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.warehouse
{
    public class Warehouse_WarehouseDTO : DataDTO
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Warehouse_DistrictDTO District { get; set; }
        public Warehouse_ProvinceDTO Province { get; set; }
        public Warehouse_StatusDTO Status { get; set; }
        public Warehouse_WardDTO Ward { get; set; }
        public List<Warehouse_InventoryDTO> Inventories { get; set; }
        public List<Warehouse_WarehouseOrganizationMappingDTO> WarehouseOrganizationMappings { get; set; }
        public Warehouse_WarehouseDTO() { }
        public Warehouse_WarehouseDTO(Warehouse Warehouse)
        {
            this.Id = Warehouse.Id;
            this.Code = Warehouse.Code;
            this.Name = Warehouse.Name;
            this.Address = Warehouse.Address;
            this.ProvinceId = Warehouse.ProvinceId;
            this.DistrictId = Warehouse.DistrictId;
            this.WardId = Warehouse.WardId;
            this.StatusId = Warehouse.StatusId;
            this.Used = Warehouse.Used;
            this.CreatedAt = Warehouse.CreatedAt;
            this.UpdatedAt = Warehouse.UpdatedAt;
            this.District = Warehouse.District == null ? null : new Warehouse_DistrictDTO(Warehouse.District);
            this.Province = Warehouse.Province == null ? null : new Warehouse_ProvinceDTO(Warehouse.Province);
            this.Status = Warehouse.Status == null ? null : new Warehouse_StatusDTO(Warehouse.Status);
            this.Ward = Warehouse.Ward == null ? null : new Warehouse_WardDTO(Warehouse.Ward);
            this.Inventories = Warehouse.Inventories?.Select(x => new Warehouse_InventoryDTO(x)).ToList();
            this.WarehouseOrganizationMappings = Warehouse.WarehouseOrganizationMappings?.Select(x => new Warehouse_WarehouseOrganizationMappingDTO(x)).ToList();
            this.Errors = Warehouse.Errors;
        }
    }

    public class Warehouse_WarehouseFilterDTO : FilterDTO
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
        public WarehouseOrder OrderBy { get; set; }
    }
}
