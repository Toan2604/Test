using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public class Role_AppUserDTO : DataDTO
    {

        public long Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public long? PositionId { get; set; }
        public string Department { get; set; }
        public long? OrganizationId { get; set; }
        public long SexId { get; set; }
        public long StatusId { get; set; }
        public Role_OrganizationDTO Organization { get; set; }

        public Role_AppUserDTO() { }
        public Role_AppUserDTO(AppUser AppUser)
        {
            this.Id = AppUser.Id;
            this.Username = AppUser.Username;
            this.DisplayName = AppUser.DisplayName;
            this.Address = AppUser.Address;
            this.Email = AppUser.Email;
            this.Phone = AppUser.Phone;
            this.StatusId = AppUser.StatusId;
            this.OrganizationId = AppUser.OrganizationId;
            this.Organization = AppUser.Organization == null ? null : new Role_OrganizationDTO(AppUser.Organization);
            this.Errors = AppUser.Errors;
        }
    }

    public class Role_AppUserFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Username { get; set; }
        public StringFilter DisplayName { get; set; }
        public StringFilter Address { get; set; }
        public StringFilter Email { get; set; }
        public StringFilter Phone { get; set; }
        public DateFilter Birthday { get; set; }
        public IdFilter PositionId { get; set; }
        public StringFilter Department { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter SexId { get; set; }
        public IdFilter StatusId { get; set; }
        public AppUserOrder OrderBy { get; set; }
    }
}
