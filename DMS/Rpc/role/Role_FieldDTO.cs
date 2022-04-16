using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public class Role_FieldDTO : DataDTO
    {

        public long Id { get; set; }

        public string Name { get; set; }

        public long FieldTypeId { get; set; }

        public long MenuId { get; set; }

        public bool IsDeleted { get; set; }


        public Role_FieldDTO() { }
        public Role_FieldDTO(Field Field)
        {

            this.Id = Field.Id;

            this.Name = Field.Name;

            this.FieldTypeId = Field.FieldTypeId;

            this.MenuId = Field.MenuId;

            this.IsDeleted = Field.IsDeleted;

        }
    }

    public class Role_FieldFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Name { get; set; }

        public StringFilter Type { get; set; }

        public IdFilter MenuId { get; set; }

        public FieldOrder OrderBy { get; set; }
    }
}
