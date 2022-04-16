using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Helpers;
using DMS.ABE.Rpc.direct_sales_order;
using System;
using TrueSight.Common;

namespace DMS.ABE.Services.MComment
{
    public interface ICommentTemplate : IServiceScoped
    {
        GlobalUserNotification CreateNotification(Guid SenderRowId, Guid RecipientRowId, string Code, string Name, string link_website, string link_mobile, GlobalUser GlobalUser, NotificationType NotificationType);
    }
    public class CommentTemplate : ICommentTemplate
    {
        public GlobalUserNotification CreateNotification(
            Guid SenderRowId, 
            Guid RecipientRowId, 
            string Code,
            string Name,
            string link_website,
            string link_mobile,
            GlobalUser GlobalUser, 
            NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.COMMENT_ORDER => $"{GlobalUser.Username} - {GlobalUser.DisplayName} đã nhắc đến bạn trong một bình luận tại đơn hàng số {Code}",
                NotificationType.COMMENT_STORESCOUTING => $"{GlobalUser.Username} - {GlobalUser.DisplayName} đã nhắc đến bạn trong một bình luận tại Đại lý cắm cờ {Code} - {Name}",
                NotificationType.COMMENT_PROBLEM => $"Bạn vừa được nhắc đến trong một bình luận tại Vấn đề {Code}",


                NotificationType.COMMENT_ORDER_RELATED => $"{GlobalUser.Username} - {GlobalUser.DisplayName} vừa bình luận vào đơn hàng số {Code}",
                NotificationType.COMMENT_PROBLEM_RELATED => $"{GlobalUser.Username} - {GlobalUser.DisplayName} vừa bình luận vào Vấn đề {Code}",
                NotificationType.COMMENT_STORESCOUTING_RELATED => $"{GlobalUser.Username} - {GlobalUser.DisplayName} vừa bình luận vào đai lý cắm cờ {Code} - {Name}",
                _ => ""
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = link_website,
                LinkMobile = link_mobile,
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
