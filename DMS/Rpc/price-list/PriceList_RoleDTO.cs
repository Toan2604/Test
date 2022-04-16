using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.price_list
{
    public class PriceList_RoleDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public PriceList_RoleDTO() { }
        public PriceList_RoleDTO(Role Role)
        {
            this.Id = Role.Id;
            this.Code = Role.Code;
            this.Name = Role.Name;
        }
    }
}
