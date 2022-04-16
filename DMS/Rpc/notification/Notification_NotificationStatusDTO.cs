using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.notification
{
    public class Notification_NotificationStatusDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public Notification_NotificationStatusDTO() { }
        public Notification_NotificationStatusDTO(NotificationStatus NotificationStatus)
        {

            this.Id = NotificationStatus.Id;

            this.Code = NotificationStatus.Code;

            this.Name = NotificationStatus.Name;

            this.Errors = NotificationStatus.Errors;
        }
    }

    public class Notification_NotificationStatusFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public NotificationStatusOrder OrderBy { get; set; }
    }
}