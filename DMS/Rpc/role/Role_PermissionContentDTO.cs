using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public class Role_PermissionContentDTO : DataDTO
    {
        public long Id { get; set; }
        public long PermissionId { get; set; }
        public long PermissionOperatorId { get; set; }
        public long FieldId { get; set; }
        public string Value { get; set; }
        public Role_FieldDTO Field { get; set; }
        public Role_PermissionOperatorDTO PermissionOperator { get; set; }

        public Role_PermissionContentDTO() { }
        public Role_PermissionContentDTO(PermissionContent PermissionContent)
        {
            this.Id = PermissionContent.Id;
            this.PermissionOperatorId = PermissionContent.PermissionOperatorId;
            this.PermissionId = PermissionContent.PermissionId;
            this.FieldId = PermissionContent.FieldId;
            this.Value = PermissionContent.Value;
            this.Field = PermissionContent.Field == null ? null : new Role_FieldDTO(PermissionContent.Field);
            this.PermissionOperator = PermissionContent.PermissionOperator == null ? null : new Role_PermissionOperatorDTO(PermissionContent.PermissionOperator);
            this.Errors = PermissionContent.Errors;
        }
    }

    public class Role_PermissionFieldMappingFilterDTO : FilterDTO
    {

        public IdFilter PermissionId { get; set; }

        public IdFilter FieldId { get; set; }

        public StringFilter Value { get; set; }

        public PermissionFieldMappingOrder OrderBy { get; set; }
    }
}
