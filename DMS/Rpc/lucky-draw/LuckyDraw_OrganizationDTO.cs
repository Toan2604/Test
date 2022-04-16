using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.lucky_draw
{
    public class LuckyDraw_OrganizationDTO : DataDTO
    {
        
        public long Id { get; set; }
        
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public long? ParentId { get; set; }
        
        public string Path { get; set; }
        
        public long Level { get; set; }
        
        public long StatusId { get; set; }
        
        public string Phone { get; set; }
        
        public string Email { get; set; }
        
        public string Address { get; set; }
        
        public bool IsDisplay { get; set; }
        
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public LuckyDraw_OrganizationDTO() {}
        public LuckyDraw_OrganizationDTO(Organization Organization)
        {
            
            this.Id = Organization.Id;
            
            this.Code = Organization.Code;
            
            this.Name = Organization.Name;
            
            this.ParentId = Organization.ParentId;
            
            this.Path = Organization.Path;
            
            this.Level = Organization.Level;
            
            this.StatusId = Organization.StatusId;
            
            this.Phone = Organization.Phone;
            
            this.Email = Organization.Email;
            
            this.Address = Organization.Address;
            
            this.IsDisplay = Organization.IsDisplay;
            
            this.RowId = Organization.RowId;
            this.CreatedAt = Organization.CreatedAt;
            this.UpdatedAt = Organization.UpdatedAt;
            this.Informations = Organization.Informations;
            this.Warnings = Organization.Warnings;
            this.Errors = Organization.Errors;
        }
    }

    public class LuckyDraw_OrganizationFilterDTO : FilterDTO
    {
        
        public IdFilter Id { get; set; }
        
        public StringFilter Code { get; set; }
        
        public StringFilter Name { get; set; }
        
        public IdFilter ParentId { get; set; }
        
        public StringFilter Path { get; set; }
        
        public LongFilter Level { get; set; }
        
        public IdFilter StatusId { get; set; }
        
        public StringFilter Phone { get; set; }
        
        public StringFilter Email { get; set; }
        
        public StringFilter Address { get; set; }
        
        public OrganizationOrder OrderBy { get; set; }
    }
}