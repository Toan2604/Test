using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public class Role_StoreDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }


        public Role_StoreDTO() { }
        public Role_StoreDTO(Store Store)
        {
            this.Id = Store.Id;
            this.Code = Store.Code;
            this.Name = Store.Name;
            this.Errors = Store.Errors;
        }
    }

    public class Role_StoreFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StoreOrder OrderBy { get; set; }
    }
}
