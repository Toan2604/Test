using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class BannerDAO
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual AppUserDAO Creator { get; set; }
        public virtual ImageDAO Image { get; set; }
        public virtual OrganizationDAO Organization { get; set; }
        public virtual StatusDAO Status { get; set; }
    }
}
