using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.organization
{
    public class Organization_OrganizationDTO : DataDTO
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
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Organization_OrganizationDTO Parent { get; set; }
        public Organization_StatusDTO Status { get; set; }

        public List<Organization_AppUserDTO> AppUsers { get; set; }
        public Organization_OrganizationDTO() { }
        public Organization_OrganizationDTO(Organization Organization)
        {
            this.Id = Organization.Id;
            this.Code = Organization.Code;
            this.Name = Organization.Name;
            this.ParentId = Organization.ParentId;
            this.Path = Organization.Path;
            this.Level = Organization.Level;
            this.StatusId = Organization.StatusId;
            this.Phone = Organization.Phone;
            this.Address = Organization.Address;
            this.Email = Organization.Email;
            this.Parent = Organization.Parent == null ? null : new Organization_OrganizationDTO(Organization.Parent);
            this.Status = Organization.Status == null ? null : new Organization_StatusDTO(Organization.Status);

            this.AppUsers = Organization.AppUsers?.Select(x => new Organization_AppUserDTO(x)).ToList();
            this.IsDisplay = Organization.IsDisplay;
            this.UpdatedAt = Organization.UpdatedAt;
            this.CreatedAt = Organization.CreatedAt;
            this.DeletedAt = Organization.DeletedAt;
            this.Errors = Organization.Errors;
        }
    }

    public class Organization_OrganizationFilterDTO : FilterDTO
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
        public bool? IsDisplay { get; set; }
        public OrganizationOrder OrderBy { get; set; }
        public DateFilter UpdatedAt { get; set; }
    }
}
