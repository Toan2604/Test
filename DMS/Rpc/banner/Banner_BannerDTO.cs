using DMS.Entities;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.banner
{
    public class Banner_BannerDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public long? Priority { get; set; }
        public string Content { get; set; }
        public long CreatorId { get; set; }
        public long OrganizationId { get; set; }
        public long? ImageId { get; set; }
        public long StatusId { get; set; }
        public Banner_AppUserDTO Creator { get; set; }
        public Banner_OrganizationDTO Organization { get; set; }
        public List<Banner_ImageDTO> Images { get; set; }
        public Banner_StatusDTO Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Banner_BannerDTO() { }
        public Banner_BannerDTO(Banner Banner)
        {
            this.Id = Banner.Id;
            this.Code = Banner.Code;
            this.Title = Banner.Title;
            this.Priority = Banner.Priority;
            this.Content = Banner.Content;
            this.CreatorId = Banner.CreatorId;
            this.OrganizationId = Banner.OrganizationId;
            this.ImageId = Banner.ImageId;
            this.StatusId = Banner.StatusId;
            this.Creator = Banner.Creator == null ? null : new Banner_AppUserDTO(Banner.Creator);
            this.Organization = Banner.Organization == null ? null : new Banner_OrganizationDTO(Banner.Organization);
            this.Images = new List<Banner_ImageDTO> { Banner.Image == null ? null : new Banner_ImageDTO(Banner.Image) };
            this.Status = Banner.Status == null ? null : new Banner_StatusDTO(Banner.Status);
            this.CreatedAt = Banner.CreatedAt;
            this.UpdatedAt = Banner.UpdatedAt;
            this.Errors = Banner.Errors;
        }
    }

    public class Banner_BannerFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Title { get; set; }
        public LongFilter Priority { get; set; }
        public StringFilter Content { get; set; }
        public IdFilter CreatorId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter ImageId { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public BannerOrder OrderBy { get; set; }
    }
}
