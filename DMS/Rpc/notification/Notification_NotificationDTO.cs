using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.notification
{
    public class Notification_NotificationDTO : DataDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public long? OrganizationId { get; set; }
        public long NotificationStatusId { get; set; }
        public Notification_NotificationStatusDTO NotificationStatus { get; set; }
        public Notification_OrganizationDTO Organization { get; set; }
        public Notification_NotificationDTO() { }
        public Notification_NotificationDTO(Notification Notification)
        {
            this.Id = Notification.Id;
            this.Title = Notification.Title;
            this.Content = Notification.Content;
            this.OrganizationId = Notification.OrganizationId;
            this.NotificationStatusId = Notification.NotificationStatusId;
            this.NotificationStatus = Notification.NotificationStatus == null ? null : new Notification_NotificationStatusDTO(Notification.NotificationStatus);
            this.Organization = Notification.Organization == null ? null : new Notification_OrganizationDTO(Notification.Organization);
            this.Errors = Notification.Errors;
        }
    }

    public class Notification_NotificationFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Title { get; set; }
        public StringFilter Content { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter NotificationStatusId { get; set; }
        public NotificationOrder OrderBy { get; set; }
    }
}
