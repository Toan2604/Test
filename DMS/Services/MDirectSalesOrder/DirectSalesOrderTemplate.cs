using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Rpc.direct_sales_order;
using System;
using TrueSight.Common;

namespace DMS.Services.MDirectSalesOrder
{
    public interface IDirectSalesOrderTemplate : IServiceScoped
    {
        GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, DirectSalesOrder DirectSalesOrder, AppUser AppUser, NotificationType NotificationType, DirectSalesOrder old = null);
        GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, DirectSalesOrder DirectSalesOrder, AppUser AppUser, NotificationType NotificationType, DirectSalesOrder old = null);
    }
    public class DirectSalesOrderTemplate : IDirectSalesOrderTemplate
    {
        public GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, DirectSalesOrder DirectSalesOrder, AppUser AppUser, NotificationType NotificationType, DirectSalesOrder old = null)
        {
            var content = NotificationType switch
            {
                NotificationType.SEND => $"Bạn vừa nhận được yêu cầu phê duyệt Đơn hàng số {DirectSalesOrder.Code} của nhân viên {AppUser.Username} - {AppUser.DisplayName}",
                NotificationType.WAIT => $"Bạn vừa nhận được yêu cầu phê duyệt Đơn hàng số {DirectSalesOrder.Code} của nhân viên {AppUser.Username} - {AppUser.DisplayName}",
                NotificationType.APPROVE => $"{AppUser.Username} - {AppUser.DisplayName} đã phê duyệt đơn hàng số {DirectSalesOrder.Code}",
                NotificationType.REJECT => $"{AppUser.Username} - {AppUser.DisplayName} đã từ chối đơn hàng số {DirectSalesOrder.Code}",
                NotificationType.TOSENDER => $"Bạn vừa gửi thành công đơn hàng số {DirectSalesOrder.Code}",
                NotificationType.TOUPDATER => $"Bạn vừa cập nhật thành công đơn hàng số {DirectSalesOrder.Code}",
                NotificationType.TOAPPROVER => $"Bạn vừa phê duyệt đơn hàng số {DirectSalesOrder.Code}",
                NotificationType.TOREJECTER => $"Bạn vừa từ chối đơn hàng số {DirectSalesOrder.Code}",
                NotificationType.UPDATE => $"Đơn hàng số {DirectSalesOrder.Code} vừa được cập nhật",
                NotificationType.UPDATE_ERP => $"Đơn hàng số {DirectSalesOrder.Code} vừa được cập nhật từ trạng thái {old.ErpApprovalState.Name} thành trạng thái thành {DirectSalesOrder.ErpApprovalState.Name}",
                _ => NotificationContentEnum.CREATE.Value
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{DirectSalesOrderRoute.Master}#*".Replace("*", DirectSalesOrder.Id.ToString()),
                LinkMobile = $"{DirectSalesOrderRoute.Mobile}".Replace("*", DirectSalesOrder.Id.ToString()),
                Time = StaticParams.DateTimeNow,
                Unread = true,
                SenderRowId = SenderRowId,
                RecipientRowId = RecipientRowId,
                RowId = Guid.NewGuid()
            };
            return GlobalUserNotification;
        }

        public GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, DirectSalesOrder DirectSalesOrder, AppUser AppUser, NotificationType NotificationType, DirectSalesOrder old = null)
        {
            var content = NotificationType switch
            {
                NotificationType.SEND => $"Bạn vừa có đơn hàng {DirectSalesOrder.Code} được tạo bởi nhân viên {AppUser.Username} - {AppUser.DisplayName}",
                NotificationType.APPROVE => $"Đơn hàng số {DirectSalesOrder.Code} đã được phê duyệt bởi {AppUser.Username} - {AppUser.DisplayName}",
                NotificationType.REJECT => $"Đơn hàng số {DirectSalesOrder.Code} đã bị từ chối bởi {AppUser.Username} - {AppUser.DisplayName}",
                NotificationType.UPDATE => $"Đơn hàng số {DirectSalesOrder.Code} vừa được cập nhật",
                NotificationType.UPDATE_ERP => $"Đơn hàng số {DirectSalesOrder.Code} vừa được cập nhật từ trạng thái {old.ErpApprovalState.Name} thành trạng thái thành {DirectSalesOrder.ErpApprovalState.Name}",
                _ => NotificationContentEnum.CREATE.Value
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{DirectSalesOrderRoute.PreviewABE}#*".Replace("*", DirectSalesOrder.Id.ToString()),
                LinkMobile = $"{DirectSalesOrderRoute.MobileABE}".Replace("*", DirectSalesOrder.Id.ToString()),
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
