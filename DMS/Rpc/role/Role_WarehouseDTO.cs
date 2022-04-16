using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public class Role_WarehouseDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Role_WarehouseDTO() { }
        public Role_WarehouseDTO(Warehouse Warehouse)
        {
            this.Id = Warehouse.Id;
            this.Code = Warehouse.Code;
            this.Name = Warehouse.Name;
            this.Address = Warehouse.Address;
            this.Errors = Warehouse.Errors;
        }
    }

    public class Role_WarehouseFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Address { get; set; }
        public WarehouseOrder OrderBy { get; set; }
    }
}
