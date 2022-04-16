using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class NotificationStatusDAO
    {
        public NotificationStatusDAO()
        {
            Notifications = new HashSet<NotificationDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<NotificationDAO> Notifications { get; set; }
    }
}
