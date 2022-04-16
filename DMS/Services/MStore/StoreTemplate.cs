using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Rpc.store;
using System;
using TrueSight.Common;

namespace DMS.Services.MStore
{
    public interface IStoreTemplate : IServiceScoped
    {
        GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, Store Store, AppUser AppUser, NotificationType NotificationType);
        GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, Store Store, AppUser AppUser, NotificationType NotificationType);
        GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, StoreScouting StoreScouting, Store Store, AppUser AppUser, NotificationType NotificationType);
    }
    public class StoreTemplate : IStoreTemplate
    {
        public GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, Store Store, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.TOCREATOR => $"Bạn vừa thêm mới đại lý {Store.Code} - {Store.Name}",
                NotificationType.TOUPDATER => $"Bạn vừa cập nhật đại lý {Store.Code} - {Store.Name}",
                NotificationType.TODELETER => $"Bạn vừa xóa đại lý {Store.Code} - {Store.Name}",
                NotificationType.TOAPPROVER => $"Bạn vừa phê duyệt đại lý {Store.Code} - {Store.Name} thành đại lý chính thức",
                NotificationType.TOCHANGER => $"Bạn vừa thay đổi loại đại lý của {Store.Code} - {Store.Name} thành {Store.StoreType.Name}",
                NotificationType.CREATE => $"Đại lý {Store.Code} - {Store.Name} vừa được thêm mới trên hệ thống bởi {AppUser.DisplayName}",
                NotificationType.UPDATE => $"Đại lý {Store.Code} - {Store.Name} vừa được cập nhật trên hệ thống bởi {AppUser.DisplayName}",
                NotificationType.DELETE => $"Đại lý {Store.Code} - {Store.Name} vừa được xóa trên hệ thống bởi {AppUser.DisplayName}",
                NotificationType.APPROVE => $"Đại lý {Store.Code} - {Store.Name} vừa được phê duyệt thành đại lý chính thức bởi {AppUser.DisplayName}",
                NotificationType.CHANGE => $"Đại lý {Store.Code} - {Store.Name} vừa được thay đổi thành loại đại lý {Store.StoreType.Name} bởi {AppUser.DisplayName}",
                _ => $"Đại lý {Store.Code} - {Store.Name} vừa được thêm mới trên hệ thống bởi {AppUser.DisplayName}"
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{StoreRoute.Master}/#*".Replace("*", Store.Id.ToString()),
                LinkMobile = $"{StoreRoute.Mobile}".Replace("*", Store.Id.ToString()),
                Time = StaticParams.DateTimeNow,
                Unread = true,
                SenderRowId = SenderRowId,
                RecipientRowId = RecipientRowId,
                RowId = Guid.NewGuid()
            };
            return GlobalUserNotification;
        }

        public GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, Store Store, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.CREATE => $"Đại lý {Store.Code} - {Store.Name} vừa được thêm mới trên hệ thống bởi {AppUser.DisplayName}",
                NotificationType.UPDATE => $"Đại lý {Store.Code} - {Store.Name} vừa được cập nhật trên hệ thống bởi {AppUser.DisplayName}",
                NotificationType.DELETE => $"Đại lý {Store.Code} - {Store.Name} vừa được xóa trên hệ thống bởi {AppUser.DisplayName}",
                NotificationType.APPROVE => $"Đại lý {Store.Code} - {Store.Name} vừa được phê duyệt thành đại lý chính thức bởi {AppUser.DisplayName}",
                _ => $"Đại lý {Store.Code} - {Store.Name} vừa được thêm mới trên hệ thống bởi {AppUser.DisplayName}",
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{StoreRoute.Master}/#*".Replace("*", Store.Id.ToString()),
                LinkMobile = $"{StoreRoute.Mobile}".Replace("*", Store.Id.ToString()),
                Time = StaticParams.DateTimeNow,
                Unread = true,
                SenderRowId = SenderRowId,
                RecipientRowId = RecipientRowId,
                RowId = Guid.NewGuid()
            };
            return GlobalUserNotification;
        }

        public GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, StoreScouting StoreScouting, Store Store, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.CREATE => $"Đại lý cắm cờ {StoreScouting.Code} - {StoreScouting.Name} vừa được mở thành đại lý {Store.Code} - {Store.Name} bởi {AppUser.DisplayName}",
                NotificationType.TOCREATOR => $"Bạn vừa tạo thành công Đại lý {Store.Code} - {Store.Name} từ đại lý cắm cờ {StoreScouting.Code} - {StoreScouting.Name}",
                _ => $"Đại lý cắm cờ {StoreScouting.Code} - {StoreScouting.Name} vừa được mở thành đại lý {Store.Code} - {Store.Name} bởi {AppUser.DisplayName}",
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{StoreRoute.Master}/#*".Replace("*", Store.Id.ToString()),
                LinkMobile = $"{StoreRoute.Mobile}".Replace("*", Store.Id.ToString()),
                RecipientRowId = RecipientRowId,
                SenderRowId = SenderRowId,
                Time = StaticParams.DateTimeNow,
                Unread = true,
                RowId = Guid.NewGuid()
            };
            return GlobalUserNotification;
        }
    }
}
