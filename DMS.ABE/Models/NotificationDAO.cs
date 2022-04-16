using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class NotificationDAO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public long? OrganizationId { get; set; }
        public long NotificationStatusId { get; set; }

        public virtual NotificationStatusDAO NotificationStatus { get; set; }
        public virtual OrganizationDAO Organization { get; set; }
    }
}
