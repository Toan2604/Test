using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_AppUserDTO : DataDTO
    {

        public long Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string DisplayName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public long? PositionId { get; set; }

        public string Department { get; set; }

        public long OrganizationId { get; set; }

        public long SexId { get; set; }

        public long StatusId { get; set; }


        public DateTime? Birthday { get; set; }

        public long? ProvinceId { get; set; }


        public MobileSync_AppUserDTO() { }
        public MobileSync_AppUserDTO(AppUser AppUser)
        {

            this.Id = AppUser.Id;

            this.Username = AppUser.Username;

            this.DisplayName = AppUser.DisplayName;

            this.Address = AppUser.Address;

            this.Email = AppUser.Email;

            this.Phone = AppUser.Phone;

            this.PositionId = AppUser.PositionId;

            this.Department = AppUser.Department;

            this.OrganizationId = AppUser.OrganizationId;

            this.SexId = AppUser.SexId;

            this.StatusId = AppUser.StatusId;

            this.Birthday = AppUser.Birthday;

            this.ProvinceId = AppUser.ProvinceId;
        }
    }
}