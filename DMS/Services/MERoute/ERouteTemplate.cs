using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Rpc.direct_sales_order;
using DMS.Rpc.e_route;
using System;
using TrueSight.Common;

namespace DMS.Services.MERoute
{
    public interface IERouteTemplate : IServiceScoped
    {
        GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, ERoute ERoute, AppUser AppUser, NotificationType NotificationType);
        GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, ERoute ERoute, AppUser AppUser, NotificationType NotificationType);
    }
    public class ERouteTemplate : IERouteTemplate
    {
        public GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, ERoute ERoute, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.TOCREATOR => $"Bạn vừa thêm mới tuyến {ERoute.Code} - {ERoute.Name}",
                NotificationType.TOUPDATER => $"Bạn vừa cập nhật tuyến {ERoute.Code} - {ERoute.Name}",
                NotificationType.TODELETER=> $"Bạn vừa xóa tuyến {ERoute.Code} - {ERoute.Name}",
                NotificationType.CREATE => $"Tuyến {ERoute.Code} - {ERoute.Name} vừa được thêm mới cho anh/chị bởi {AppUser.DisplayName}",
                NotificationType.UPDATE => $"Tuyến {ERoute.Code} - {ERoute.Name} vừa được cập nhật thành công bởi {AppUser.DisplayName}",
                NotificationType.DELETE => $"Tuyến {ERoute.Code} - {ERoute.Name} vừa được xóa thành công bởi {AppUser.DisplayName}",
                _ => $"Bạn vừa thêm mới tuyến {ERoute.Code} - {ERoute.Name}"
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{ERouteRoute.Master}#*".Replace("*", ERoute.Id.ToString()),
                LinkMobile = $"{ERouteRoute.Detail}".Replace("*", ERoute.Id.ToString()),
                Time = StaticParams.DateTimeNow,
                Unread = true,
                SenderRowId = SenderRowId,
                RecipientRowId = RecipientRowId,
                RowId = Guid.NewGuid(),
            };
            return GlobalUserNotification;
        }

        public GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, ERoute ERoute, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.TOCREATOR => $"Bạn vừa thêm mới tuyến {ERoute.Code} - {ERoute.Name}",
                NotificationType.TOUPDATER => $"Bạn vừa cập nhật tuyến {ERoute.Code} - {ERoute.Name}",
                NotificationType.TODELETER => $"Bạn vừa xóa tuyến {ERoute.Code} - {ERoute.Name}",
                NotificationType.CREATE => $"Tuyến {ERoute.Code} - {ERoute.Name} vừa được thêm mới cho anh/chị bởi {AppUser.DisplayName}",
                NotificationType.UPDATE => $"Tuyến {ERoute.Code} - {ERoute.Name} vừa được cập nhật thành công bởi {AppUser.DisplayName}",
                NotificationType.DELETE => $"Tuyến {ERoute.Code} - {ERoute.Name} vừa được xóa thành công bởi {AppUser.DisplayName}",
                _ => $"Bạn vừa thêm mới tuyến {ERoute.Code} - {ERoute.Name}"
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                //TitleWeb = $"Thông báo từ DMS",
                //ContentWeb = $"Tuyên {ERoute.Code} - {ERoute.Name} đã được {content} cho anh/chị bởi {AppUser.DisplayName}",
                //LinkWebsite = $"{ERouteRoute.Master}#*".Replace("*", ERoute.Id.ToString()),
                //LinkMobile = $"{ERouteRoute.Detail}".Replace("*", ERoute.Id.ToString()),
                //Time = StaticParams.DateTimeNow,
                //Unread = true,
                //SenderRowId = SenderRowId,
                //RecipientRowId = RecipientRowId,
                //RowId = Guid.NewGuid(),
            };
            return GlobalUserNotification;
        }
    }
}
