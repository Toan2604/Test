using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public class Role_PermissionOperatorDTO : DataDTO
    {

        public long Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }
        public long FieldTypeId { get; set; }

        public Role_PermissionOperatorDTO() { }
        public Role_PermissionOperatorDTO(PermissionOperator PermissionOperator)
        {
            this.Id = PermissionOperator.Id;
            this.Code = PermissionOperator.Code;
            this.Name = PermissionOperator.Name;
            this.FieldTypeId = PermissionOperator.FieldTypeId;
        }
    }

    public class Role_PermissionOperatorFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Name { get; set; }

        public StringFilter Code { get; set; }

        public IdFilter FieldTypeId { get; set; }

    }
}
