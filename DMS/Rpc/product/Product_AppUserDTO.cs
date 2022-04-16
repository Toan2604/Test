using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.product
{
    public class Product_AppUserDTO : DataDTO
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public long? OrganizationId { get; set; }
        public long? SexId { get; set; }
        public long StatusId { get; set; }
        public Product_StatusDTO Status { get; set; }

        public Product_AppUserDTO() { }
        public Product_AppUserDTO(AppUser AppUser)
        {
            this.Id = AppUser.Id;
            this.Username = AppUser.Username;
            this.DisplayName = AppUser.DisplayName;
            this.Address = AppUser.Address;
            this.Email = AppUser.Email;
            this.Phone = AppUser.Phone;
            this.Department = AppUser.Department;
            this.OrganizationId = AppUser.OrganizationId;
            this.SexId = AppUser.SexId;
            this.StatusId = AppUser.StatusId;
            this.Status = AppUser.Status == null ? null : new Product_StatusDTO(AppUser.Status);
        }
    }

    public class Product_AppUserFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Username { get; set; }

        public StringFilter Password { get; set; }

        public StringFilter DisplayName { get; set; }

        public StringFilter Address { get; set; }

        public StringFilter Email { get; set; }

        public StringFilter Phone { get; set; }

        public StringFilter Department { get; set; }

        public IdFilter OrganizationId { get; set; }

        public IdFilter SexId { get; set; }

        public IdFilter StatusId { get; set; }

        public AppUserOrder OrderBy { get; set; }
    }
}
