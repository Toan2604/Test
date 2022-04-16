using DMS.Entities;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_WarehouseDTO : DataDTO
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
        public List<GeneralMobile_InventoryDTO> Inventories { get; set; }
        public GeneralMobile_WarehouseDTO() { }
        public GeneralMobile_WarehouseDTO(Warehouse Warehouse)
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
            this.Errors = Warehouse.Errors;
        }
    }
}
