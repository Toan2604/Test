using DMS.Entities;
using System;
using System.Collections.Generic;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_BannerDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public long? Priority { get; set; }
        public string Content { get; set; }
        public long CreatorId { get; set; }
        public long? ImageId { get; set; }
        public long StatusId { get; set; }
        public MobileSync_AppUserDTO Creator { get; set; }
        public List<MobileSync_ImageDTO> Images { get; set; }
        public MobileSync_StatusDTO Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public MobileSync_BannerDTO() { }
        public MobileSync_BannerDTO(Banner Banner)
        {
            this.Id = Banner.Id;
            this.Code = Banner.Code;
            this.Title = Banner.Title;
            this.Priority = Banner.Priority;
            this.Content = Banner.Content;
            this.CreatorId = Banner.CreatorId;
            this.ImageId = Banner.ImageId;
            this.StatusId = Banner.StatusId;
            this.Creator = Banner.Creator == null ? null : new MobileSync_AppUserDTO(Banner.Creator);
            this.Images = new List<MobileSync_ImageDTO> { Banner.Image == null ? null : new MobileSync_ImageDTO(Banner.Image) };
            this.Status = Banner.Status == null ? null : new MobileSync_StatusDTO(Banner.Status);
            this.CreatedAt = Banner.CreatedAt;
            this.UpdatedAt = Banner.UpdatedAt;
        }
    }
}
