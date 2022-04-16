using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Rpc.monitor_store_problems;
using System;
using TrueSight.Common;

namespace DMS.Services.MProblem
{
    public interface IProblemTemplate : IServiceScoped
    {
        GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, Problem Problem, AppUser AppUser, NotificationType NotificationType);
        GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, Problem Problem, AppUser AppUser, NotificationType NotificationType);
    }
    public class ProblemTemplate : IProblemTemplate
    {
        public GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, Problem Problem, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.TOCREATOR => $"Bạn vừa thêm mới vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name}",
                NotificationType.TOUPDATER => $"Bạn vừa cập nhật vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name}",
                NotificationType.TODELETER => $"Bạn vừa xóa vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name}",
                NotificationType.CREATE => $"Vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name} đã được thêm mới lên hệ thống thành công bởi {AppUser.DisplayName}",
                NotificationType.UPDATE => $"Vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name} đã được cập nhật thành công bởi {AppUser.DisplayName}",
                NotificationType.DELETE=> $"Vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name} đã được xóa thành công bởi {AppUser.DisplayName}",
                NotificationType.SEND=> NotificationContentEnum.SEND.Value,
                NotificationType.FINISH => $"Vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name} đã được xử lý bởi {AppUser.DisplayName}",
                NotificationType.WAIT => $"Vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name} đã được đưa vào trạng thái chờ xử lý bởi {AppUser.DisplayName}",
                _ => $"Bạn vừa thêm mới vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name}",
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{MonitorStoreProblemRoute.Master}#*".Replace("*", Problem.Id.ToString()),
                LinkMobile = $"{MonitorStoreProblemRoute.Mobile}".Replace("*", Problem.Id.ToString()),
                Time = StaticParams.DateTimeNow,
                Unread = true,
                SenderRowId = SenderRowId,
                RecipientRowId = RecipientRowId,
                RowId = Guid.NewGuid(),
            };
            return GlobalUserNotification;
        }

        public GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, Problem Problem, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.TOCREATOR => $"Bạn vừa thêm mới vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name}",
                NotificationType.TOUPDATER => $"Bạn vừa cập nhật vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name}",
                NotificationType.TODELETER => $"Bạn vừa xóa vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name}",
                NotificationType.CREATE => $"Vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name} đã được thêm mới lên hệ thống thành công bởi {AppUser.DisplayName}",
                NotificationType.UPDATE => $"Vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name} đã được cập nhật thành công bởi {AppUser.DisplayName}",
                NotificationType.DELETE => $"Vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name} đã được xóa thành công bởi {AppUser.DisplayName}",
                NotificationType.SEND => NotificationContentEnum.SEND.Value,
                NotificationType.FINISH => $"Vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name} đã được xử lý bởi {AppUser.DisplayName}",
                NotificationType.WAIT => $"Vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name} đã được đưa vào trạng thái chờ xử lý bởi {AppUser.DisplayName}",
                _ => $"Bạn vừa thêm mới vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name}",
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                //TitleWeb = $"Thông báo từ DMS",
                //ContentWeb = $"Vấn đề {Problem.Code} của đại lý {Problem.Store.Code} - {Problem.Store.Name} đã được {content} bởi {AppUser.DisplayName}",
                //LinkWebsite = $"{MonitorStoreProblemRoute.Master}#*".Replace("*", Problem.Id.ToString()),
                //LinkMobile = $"{MonitorStoreProblemRoute.Mobile}".Replace("*", Problem.Id.ToString()),
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
