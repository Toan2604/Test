using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public class Role_ActionDTO : DataDTO
    {

        public long Id { get; set; }

        public string Name { get; set; }

        public long MenuId { get; set; }

        public bool IsDeleted { get; set; }


        public Role_ActionDTO() { }
        public Role_ActionDTO(Action Action)
        {

            this.Id = Action.Id;

            this.Name = Action.Name;

            this.MenuId = Action.MenuId;

            this.IsDeleted = Action.IsDeleted;

        }
    }

    public class Role_PageFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Name { get; set; }

        public StringFilter Path { get; set; }

        public IdFilter MenuId { get; set; }

        public PageOrder OrderBy { get; set; }
    }
}
