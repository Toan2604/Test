using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Rpc.lucky_draw;
using System;
using TrueSight.Common;

namespace DMS.Services.MLuckyDraw
{
    public interface ILuckyDrawTemplate : IServiceScoped
    {
        GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, LuckyDraw LuckyDraw, AppUser AppUser, NotificationType NotificationType);
        GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, LuckyDraw LuckyDraw, AppUser AppUser, NotificationType NotificationType);
    }
    public class LuckyDrawTemplate : ILuckyDrawTemplate
    {
        public GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, LuckyDraw LuckyDraw, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.TOCREATOR => $"Bạn vừa thêm mới chương trình quay thưởng số {LuckyDraw.Code} - {LuckyDraw.Name}",
                NotificationType.TOUPDATER => $"Bạn vừa cập nhật chương trình quay thưởng số {LuckyDraw.Code} - {LuckyDraw.Name}",
                NotificationType.TODELETER=> $"Bạn vừa xóa chương trình quay thưởng số {LuckyDraw.Code} - {LuckyDraw.Name}",
                NotificationType.CREATE => $"Chương trình quay thưởng số {LuckyDraw.Code} - {LuckyDraw.Name} đã bắt đầu được diễn ra. Xin mời các bạn cùng tham gia",
                NotificationType.UPDATE => $"Chương trình quay thưởng số {LuckyDraw.Code} - {LuckyDraw.Name} đã bắt đầu được diễn ra. Xin mời các bạn cùng tham gia",
                NotificationType.DELETE => $"Chương trình quay thưởng số {LuckyDraw.Code} - {LuckyDraw.Name} đã được xóa thành công lên hệ thống bởi {AppUser.DisplayName}",
                _ => $"Chương trình quay thưởng số {LuckyDraw.Code} - {LuckyDraw.Name} đã được thêm mới thành công lên hệ thống bởi {AppUser.DisplayName}",
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{LuckyDrawRoute.Master}#*".Replace("*", LuckyDraw.Id.ToString()),
                LinkMobile = $"{LuckyDrawRoute.Mobile}".Replace("*", LuckyDraw.Id.ToString()),
                Time = StaticParams.DateTimeNow,
                Unread = true,
                SenderRowId = SenderRowId,
                RecipientRowId = RecipientRowId,
                RowId = Guid.NewGuid()
            };
            return GlobalUserNotification;
        }

        public GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, LuckyDraw LuckyDraw, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.CREATE => $"Chương trình quay thưởng số {LuckyDraw.Code} - {LuckyDraw.Name} đã được thêm mới thành công lên hệ thống bởi {AppUser.DisplayName}",
                NotificationType.UPDATE => $"Chương trình quay thưởng số {LuckyDraw.Code} - {LuckyDraw.Name} đã được cập nhật thành công lên hệ thống bởi {AppUser.DisplayName}",
                NotificationType.DELETE => $"Chương trình quay thưởng số {LuckyDraw.Code} - {LuckyDraw.Name} đã được xóa thành công lên hệ thống bởi {AppUser.DisplayName}",
                _ => $"Chương trình quay thưởng số {LuckyDraw.Code} - {LuckyDraw.Name} đã được thêm mới thành công lên hệ thống bởi {AppUser.DisplayName}",
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{LuckyDrawRoute.MasterABE}#*".Replace("*", LuckyDraw.Id.ToString()),
                LinkMobile = $"{LuckyDrawRoute.MobileABE}".Replace("*", LuckyDraw.Id.ToString()),
                Time = StaticParams.DateTimeNow,
                Unread = true,
                SenderRowId = SenderRowId,
                RecipientRowId = RecipientRowId,
                RowId = Guid.NewGuid()
            };
            return GlobalUserNotification;
        }
    }
}
