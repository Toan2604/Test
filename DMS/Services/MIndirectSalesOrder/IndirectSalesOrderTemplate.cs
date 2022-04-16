using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Rpc.direct_sales_order;
using DMS.Rpc.indirect_sales_order;
using System;
using TrueSight.Common;

namespace DMS.Services.MIndirectSalesOrder
{
    public interface IIndirectSalesOrderTemplate : IServiceScoped
    {
        GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, IndirectSalesOrder IndirectSalesOrder, AppUser AppUser, NotificationType NotificationType);
        GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, IndirectSalesOrder IndirectSalesOrder, AppUser AppUser, NotificationType NotificationType);
    }
    public class IndirectSalesOrderTemplate : IIndirectSalesOrderTemplate
    {
        public GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, IndirectSalesOrder IndirectSalesOrder, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.SEND => $"Bạn vừa nhận được yêu cầu phê duyệt Đơn hàng số {IndirectSalesOrder.Code} của nhân viên {AppUser.Username} - {AppUser.DisplayName}",
                NotificationType.APPROVE => $"{AppUser.Username} - {AppUser.DisplayName} đã phê duyệt đơn hàng số {IndirectSalesOrder.Code}",
                NotificationType.REJECT => $"{AppUser.Username} - {AppUser.DisplayName} đã từ chối đơn hàng số {IndirectSalesOrder.Code}",
                NotificationType.TOSENDER => $"Bạn vừa gửi thành công đơn hàng số {IndirectSalesOrder.Code}",
                NotificationType.TOAPPROVER => $"Bạn vừa phê duyệt đơn hàng số {IndirectSalesOrder.Code}",
                NotificationType.TOREJECTER => $"Bạn vừa từ chối đơn hàng số {IndirectSalesOrder.Code}",
                NotificationType.TOSALEEMPLOYEE => $"Bạn đã đươc tạo đơn hàng số {IndirectSalesOrder.Code} bởi {AppUser.Username} - {AppUser.DisplayName}",
                _ => $"Bạn vừa gửi thành công đơn hàng số {IndirectSalesOrder.Code}",
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{IndirectSalesOrderRoute.Master}#*".Replace("*", IndirectSalesOrder.Id.ToString()),
                LinkMobile = $"{IndirectSalesOrderRoute.Mobile}".Replace("*", IndirectSalesOrder.Id.ToString()),
                Time = StaticParams.DateTimeNow,
                Unread = true,
                SenderRowId = SenderRowId,
                RecipientRowId = RecipientRowId,
                RowId = Guid.NewGuid()
            };
            return GlobalUserNotification;
        }

        public GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, IndirectSalesOrder IndirectSalesOrder, AppUser AppUser, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.SEND => $"Bạn vừa nhận được yêu cầu phê duyệt Đơn hàng số {IndirectSalesOrder.Code} của nhân viên {AppUser.Username} - {AppUser.DisplayName}",
                NotificationType.APPROVE => $"{AppUser.Username} - {AppUser.DisplayName} đã phê duyệt đơn hàng số {IndirectSalesOrder.Code}",
                NotificationType.REJECT => $"{AppUser.Username} - {AppUser.DisplayName} đã từ chối đơn hàng số {IndirectSalesOrder.Code}",
                NotificationType.TOSENDER => $"Bạn vừa gửi thành công đơn hàng số {IndirectSalesOrder.Code}",
                NotificationType.TOAPPROVER => $"Bạn vừa phê duyệt đơn hàng số {IndirectSalesOrder.Code}",
                NotificationType.TOREJECTER => $"Bạn vừa từ chối đơn hàng số {IndirectSalesOrder.Code}",
                _ => $"Bạn vừa gửi thành công đơn hàng số {IndirectSalesOrder.Code}",
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                //TitleWeb = $"Thông báo từ DMS",
                //ContentWeb = $"Đơn hàng {IndirectSalesOrder.Code} đã được {content} bởi {AppUser.DisplayName}",
                //LinkWebsite = $"{IndirectSalesOrderRoute.Master}#*".Replace("*", IndirectSalesOrder.Id.ToString()),
                //LinkMobile = $"{DirectSalesOrderRoute.Mobile}".Replace("*", IndirectSalesOrder.Id.ToString()),
                //Time = StaticParams.DateTimeNow,
                //Unread = true,
                //SenderRowId = SenderRowId,
                //RecipientRowId = RecipientRowId,
                //RowId = Guid.NewGuid()
            };
            return GlobalUserNotification;
        }
    }
}
