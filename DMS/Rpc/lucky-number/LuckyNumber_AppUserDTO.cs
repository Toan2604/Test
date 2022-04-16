using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.lucky_number
{
    public class LuckyNumber_AppUserDTO : DataDTO
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

        public string Avatar { get; set; }

        public DateTime? Birthday { get; set; }

        public long? ProvinceId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public LuckyNumber_AppUserDTO() { }
        public LuckyNumber_AppUserDTO(AppUser AppUser)
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

            this.Avatar = AppUser.Avatar;

            this.Birthday = AppUser.Birthday;

            this.ProvinceId = AppUser.ProvinceId;
            this.Latitude = AppUser.Latitude;
            this.Longitude = AppUser.Longitude;
            this.Errors = AppUser.Errors;
        }
    }

    public class Mobile_AppUserFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Username { get; set; }

        public StringFilter Password { get; set; }

        public StringFilter DisplayName { get; set; }

        public StringFilter Address { get; set; }

        public StringFilter Email { get; set; }

        public StringFilter Phone { get; set; }

        public IdFilter PositionId { get; set; }

        public StringFilter Department { get; set; }

        public IdFilter OrganizationId { get; set; }

        public IdFilter SexId { get; set; }

        public IdFilter StatusId { get; set; }

        public StringFilter Avatar { get; set; }

        public DateFilter Birthday { get; set; }

        public IdFilter ProvinceId { get; set; }

        public AppUserOrder OrderBy { get; set; }
    }
}