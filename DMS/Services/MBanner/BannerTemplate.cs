using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Rpc.banner;
using System;
using TrueSight.Common;

namespace DMS.Services.MBanner
{
    public interface IBannerTemplate : IServiceScoped
    {
        GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, Banner Banner, AppUser AppUser, NotificationType NotificationType);
        GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, Banner Banner, AppUser AppUser, NotificationType NotificationType);
    }
    public class BannerTemplate : IBannerTemplate
    {
        public GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, Banner Banner, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.TOCREATOR => $"Bạn vừa thêm mới banner {Banner.Code} - {Banner.Title}",
                NotificationType.TOUPDATER => $"Bạn vừa cập nhật banner {Banner.Code} - {Banner.Title}",
                NotificationType.TODELETER => $"Bạn vừa xóa banner {Banner.Code} - {Banner.Title}",
                NotificationType.CREATE => $"Banner {Banner.Code} - {Banner.Title} vừa được thêm mới thành công lên hệ thống bởi {AppUser.DisplayName}",
                NotificationType.UPDATE => $"Banner {Banner.Code} - {Banner.Title} vừa được cập nhật thành công lên hệ thống bởi {AppUser.DisplayName}",
                NotificationType.DELETE => $"Banner {Banner.Code} - {Banner.Title} vừa được xóa thành công lên hệ thống bởi {AppUser.DisplayName}",
                _ => $"Bạn vừa thêm mới banner {Banner.Code} - {Banner.Title}",
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{BannerRoute.Master}#*".Replace("*", Banner.Id.ToString()),
                LinkMobile = null,
                Time = StaticParams.DateTimeNow,
                Unread = true,
                SenderRowId = SenderRowId,
                RecipientRowId = RecipientRowId
            };
            return GlobalUserNotification;
        }

        public GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, Banner Banner, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.CREATE => $"Banner {Banner.Code} - {Banner.Title} vừa được thêm mới thành công lên hệ thống bởi {AppUser.DisplayName}",
                NotificationType.UPDATE => $"Banner {Banner.Code} - {Banner.Title} vừa được cập nhật thành công lên hệ thống bởi {AppUser.DisplayName}",
                NotificationType.DELETE => $"Banner {Banner.Code} - {Banner.Title} vừa được xóa thành công lên hệ thống bởi {AppUser.DisplayName}",
                _ => NotificationContentEnum.CREATE.Value
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{BannerRoute.Master}#*".Replace("*", Banner.Id.ToString()),
                LinkMobile = $"{BannerRoute.MobileABE}".Replace("*", Banner.Id.ToString()),
                Time = StaticParams.DateTimeNow,
                Unread = true,
                SenderRowId = SenderRowId,
                RecipientRowId = RecipientRowId
            };
            return GlobalUserNotification;
        }
    }
}
