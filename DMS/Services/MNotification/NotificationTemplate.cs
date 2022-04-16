using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Rpc.notification;
using System;
using TrueSight.Common;

namespace DMS.Services.MNotification
{
    public interface INotificationTemplate : IServiceScoped
    {
        GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, Notification Notification, AppUser AppUser, NotificationType NotificationType);
        GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, Notification Notification, AppUser AppUser, NotificationType NotificationType);
    }
    public class NotificationTemplate : INotificationTemplate
    {
        public GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, Notification Notification, AppUser AppUser, NotificationType NotificationType)
        {
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = Notification.Content,
                LinkWebsite = $"{NotificationRoute.Master}/?id=*".Replace("*", Notification.Id.ToString()),
                LinkMobile = $"{NotificationRoute.Mobile}".Replace("*", Notification.Id.ToString()),
                Time = StaticParams.DateTimeNow,
                Unread = true,
                SenderRowId = SenderRowId,
                RecipientRowId = RecipientRowId,
                RowId = Guid.NewGuid(),
            };
            return GlobalUserNotification;
        }

        public GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, Notification Notification, AppUser AppUser, NotificationType NotificationType)
        {
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = Notification.Content,
                LinkWebsite = $"{NotificationRoute.Master}/?id=*".Replace("*", Notification.Id.ToString()),
                LinkMobile = $"{NotificationRoute.Mobile}".Replace("*", Notification.Id.ToString()),
                Time = StaticParams.DateTimeNow,
                Unread = true,
                SenderRowId = SenderRowId,
                RecipientRowId = RecipientRowId,
                RowId = Guid.NewGuid(),
            };
            return GlobalUserNotification;
        }
    }
}
