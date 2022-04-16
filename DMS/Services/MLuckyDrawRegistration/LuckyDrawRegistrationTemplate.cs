using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Rpc.lucky_draw;
using System;
using TrueSight.Common;

namespace DMS.Services.MLuckyDrawRegistration
{
    public interface ILuckyDrawRegistrationTemplate : IServiceScoped
    {
        GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, LuckyDraw LuckyDraw, AppUser AppUser, NotificationType NotificationType);
        GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, LuckyDraw LuckyDraw, AppUser AppUser, NotificationType NotificationType);
    }
    public class LuckyDrawRegistrationTemplate : ILuckyDrawRegistrationTemplate
    {
        public GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, LuckyDraw LuckyDraw, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.TOCREATOR => $"Bạn vừa thêm mới lượt quay thưởng của {LuckyDraw.Name}",
                NotificationType.CREATE => $"Lượt quay thưởng của {LuckyDraw.Name} đã được thêm mới bởi {AppUser.DisplayName}",
                _ => $"Lượt quay thưởng của {LuckyDraw.Name} đã được thêm mới bởi {AppUser.DisplayName}"
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

        public GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, LuckyDraw LuckyDraw, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.CREATE => $"Bạn vừa nhận được lượt quay thưởng của chương trình {LuckyDraw.Code} - {LuckyDraw.Name} được gửi bởi {AppUser.DisplayName}",
                _ => $"Bạn vừa nhận được lượt quay thưởng của chương trình {LuckyDraw.Code} - {LuckyDraw.Name} được gửi bởi {AppUser.DisplayName}",
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
