using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Rpc.survey;
using System;
using TrueSight.Common;

namespace DMS.Services.MSurvey
{
    public interface ISurveyTemplate : IServiceScoped
    {
        GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, Survey Survey, AppUser AppUser, NotificationType NotificationType);
        GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, Survey Survey, AppUser AppUser, NotificationType NotificationType);
    }
    public class SurveyTemplate : ISurveyTemplate
    {
        public GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, Survey Survey, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.TOCREATOR => $"Bạn vừa thêm mới khảo sát {Survey.Title}, có hiệu lực từ {Survey.StartAt} - {Survey.EndAt}",
                NotificationType.TOUPDATER => $"Bạn vừa cập nhật khảo sát {Survey.Title}, có hiệu lực từ {Survey.StartAt} - {Survey.EndAt}",
                NotificationType.TODELETER => $"Bạn vừa xóa khảo sát {Survey.Title}",
                NotificationType.CREATE => $"Khảo sát {Survey.Title} đã được thêm mới trên hệ thống bởi {AppUser.DisplayName}, có hiệu lực từ {Survey.StartAt} - {Survey.EndAt}",
                NotificationType.UPDATE => $"Khảo sát {Survey.Title} đã được cập nhật trên hệ thống bởi {AppUser.DisplayName}, có hiệu lực từ {Survey.StartAt} - {Survey.EndAt}",
                NotificationType.DELETE=> $"Khảo sát {Survey.Title} đã được xóa trên hệ thống bởi {AppUser.DisplayName}",
                _ => $"Khảo sát {Survey.Title} đã được thêm mới trên hệ thống bởi {AppUser.DisplayName}, có hiệu lực từ {Survey.StartAt} - {Survey.EndAt}",
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{SurveyRoute.Master}#*".Replace("*", Survey.Id.ToString()),
                LinkMobile = $"{SurveyRoute.Mobile}".Replace("*", Survey.Id.ToString()),
                RecipientRowId = RecipientRowId,
                SenderRowId = SenderRowId,
                Time = StaticParams.DateTimeNow,
                Unread = true,
                RowId = Guid.NewGuid(),
            };
            return GlobalUserNotification;
        }

        public GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, Survey Survey, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.TOCREATOR => $"Bạn vừa thêm mới khảo sát {Survey.Title}, có hiệu lực từ {Survey.StartAt} - {Survey.EndAt}",
                NotificationType.TOUPDATER => $"Bạn vừa cập nhật khảo sát {Survey.Title}, có hiệu lực từ {Survey.StartAt} - {Survey.EndAt}",
                NotificationType.TODELETER => $"Bạn vừa xóa khảo sát {Survey.Title}",
                NotificationType.CREATE => $"Khảo sát {Survey.Title} đã được thêm mới trên hệ thống bởi {AppUser.DisplayName}, có hiệu lực từ {Survey.StartAt} - {Survey.EndAt}",
                NotificationType.UPDATE => $"Khảo sát {Survey.Title} đã được cập nhật trên hệ thống bởi {AppUser.DisplayName}, có hiệu lực từ {Survey.StartAt} - {Survey.EndAt}",
                NotificationType.DELETE => $"Khảo sát {Survey.Title} đã được xóa trên hệ thống bởi {AppUser.DisplayName}",
                _ => $"Khảo sát {Survey.Title} đã được thêm mới trên hệ thống bởi {AppUser.DisplayName}, có hiệu lực từ {Survey.StartAt} - {Survey.EndAt}",
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{SurveyRoute.Master}#*".Replace("*", Survey.Id.ToString()),
                LinkMobile = $"{SurveyRoute.Mobile}".Replace("*", Survey.Id.ToString()),
                RecipientRowId = RecipientRowId,
                SenderRowId = SenderRowId,
                Time = StaticParams.DateTimeNow,
                Unread = true,
                RowId = Guid.NewGuid(),
            };
            return GlobalUserNotification;
        }
    }
}
