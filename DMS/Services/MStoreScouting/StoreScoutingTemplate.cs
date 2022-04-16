using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Rpc.store;
using DMS.Rpc.store_scouting;
using System;
using TrueSight.Common;

namespace DMS.Services.MStoreScouting
{
    public interface IStoreScoutingTemplate : IServiceScoped
    {
        GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, StoreScouting StoreScouting, AppUser AppUser, NotificationType NotificationType);
    }
    public class StoreScoutingTemplate : IStoreScoutingTemplate
    {


        public GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, StoreScouting StoreScouting, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.TOCREATOR => $"Bạn vừa tạo thành công đại lý cắm cờ {StoreScouting.Code} - {StoreScouting.Name}",
                NotificationType.TOUPDATER => $"Bạn vừa cập nhật thành công đại lý cắm cờ {StoreScouting.Code} - {StoreScouting.Name}",
                NotificationType.TODELETER => $"Bạn vừa xóa thành công đại lý cắm cờ {StoreScouting.Code} - {StoreScouting.Name}",
                NotificationType.TOREJECTER => $"Bạn vừa từ chối đại lý cắm cờ {StoreScouting.Code} - {StoreScouting.Name}",
                NotificationType.CREATE => $"Đại lý cắm cờ {StoreScouting.Code} - {StoreScouting.Name} vừa được thêm mới trên hệ thống bởi {AppUser.DisplayName}",
                NotificationType.UPDATE => $"Đại lý cắm cờ {StoreScouting.Code} - {StoreScouting.Name} vừa được cập nhật trên hệ thống bởi {AppUser.DisplayName}",
                NotificationType.DELETE => $"Đại lý cắm cờ {StoreScouting.Code} - {StoreScouting.Name} vừa bị xóa trên hệ thống bởi {AppUser.DisplayName}",
                NotificationType.REJECT => $"Đại lý cắm cờ {StoreScouting.Code} - {StoreScouting.Name} vừa bị từ chối trên hệ thống bởi {AppUser.DisplayName}",
                _ => $"Đại lý cắm cờ {StoreScouting.Code} - {StoreScouting.Name} vừa được thêm mới trên hệ thống bởi {AppUser.DisplayName}"
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{StoreScoutingRoute.Master}#*".Replace("*", StoreScouting.Id.ToString()),
                LinkMobile = $"{StoreScoutingRoute.Mobile}".Replace("*", StoreScouting.Id.ToString()),
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
