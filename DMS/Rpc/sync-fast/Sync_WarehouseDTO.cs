using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.sync_fast
{
    public class Sync_WarehouseDTO : DataDTO
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
        public List<Sync_InventoryDTO> Inventories { get; set; }
        public Sync_WarehouseDTO() { }
        public Sync_WarehouseDTO(Warehouse Warehouse)
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
            this.Inventories = Warehouse.Inventories?.Select(x => new Sync_InventoryDTO(x)).ToList();
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
