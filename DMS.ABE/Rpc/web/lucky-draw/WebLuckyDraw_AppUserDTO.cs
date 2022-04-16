using TrueSight.Common;
using DMS.ABE.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.ABE.Entities;

namespace DMS.ABE.Rpc.web.lucky_draw
{
    public class WebLuckyDraw_AppUserDTO : DataDTO
    {
        
        public long Id { get; set; }
        
        public string Username { get; set; }
        
        public string DisplayName { get; set; }
        
        public string Address { get; set; }
        
        public string Email { get; set; }
        
        public string Phone { get; set; }
        
        public long SexId { get; set; }
        
        public DateTime? Birthday { get; set; }
        
        public string Avatar { get; set; }
        
        public long? PositionId { get; set; }
        
        public string Department { get; set; }
        
        public long OrganizationId { get; set; }
        
        public long? ProvinceId { get; set; }
        
        public long StatusId { get; set; }
        
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public WebLuckyDraw_AppUserDTO() {}
        public WebLuckyDraw_AppUserDTO(AppUser AppUser)
        {
            
            this.Id = AppUser.Id;
            
            this.Username = AppUser.Username;
            
            this.DisplayName = AppUser.DisplayName;
            
            this.Address = AppUser.Address;
            
            this.Email = AppUser.Email;
            
            this.Phone = AppUser.Phone;
            
            this.SexId = AppUser.SexId;
            
            this.Birthday = AppUser.Birthday;
            
            this.Avatar = AppUser.Avatar;
            
            this.PositionId = AppUser.PositionId;
            
            this.Department = AppUser.Department;
            
            this.OrganizationId = AppUser.OrganizationId;
            
            this.ProvinceId = AppUser.ProvinceId;
            
            this.StatusId = AppUser.StatusId;
            
            this.RowId = AppUser.RowId;
            this.CreatedAt = AppUser.CreatedAt;
            this.UpdatedAt = AppUser.UpdatedAt;
            this.Informations = AppUser.Informations;
            this.Warnings = AppUser.Warnings;
            this.Errors = AppUser.Errors;
        }
    }

    public class WebLuckyDraw_AppUserFilterDTO : FilterDTO
    {
        
        public IdFilter Id { get; set; }
        
        public StringFilter Username { get; set; }
        
        public StringFilter DisplayName { get; set; }
        
        public StringFilter Address { get; set; }
        
        public StringFilter Email { get; set; }
        
        public StringFilter Phone { get; set; }
        
        public IdFilter SexId { get; set; }
        
        public DateFilter Birthday { get; set; }
        
        public StringFilter Avatar { get; set; }
        
        public IdFilter PositionId { get; set; }
        
        public StringFilter Department { get; set; }
        
        public IdFilter OrganizationId { get; set; }
        
        public IdFilter ProvinceId { get; set; }
        
        public IdFilter StatusId { get; set; }
        
        public AppUserOrder OrderBy { get; set; }
    }
}