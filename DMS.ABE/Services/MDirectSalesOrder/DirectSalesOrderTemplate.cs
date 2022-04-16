using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Helpers;
using DMS.ABE.Rpc.direct_sales_order;
using DMS.ABE.Rpc.web.direct_sales_order;
using System;
using TrueSight.Common;

namespace DMS.ABE.Services.MDirectSalesOrder
{
    public interface IDirectSalesOrderTemplate : IServiceScoped
    {
        GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, DirectSalesOrder DirectSalesOrder, Store Store, NotificationType NotificationType);
        GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, DirectSalesOrder DirectSalesOrder, Store Store, NotificationType NotificationType);
    }
    public class DirectSalesOrderTemplate : IDirectSalesOrderTemplate
    {
        public GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, DirectSalesOrder DirectSalesOrder, Store Store, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.WAIT => $"Bạn vừa nhận được yêu cầu phê duyệt Đơn hàng số {DirectSalesOrder.Code} của cửa hàng {Store.Code} - {Store.Name}",
                NotificationType.SEND => $"Đơn hàng số {DirectSalesOrder.Code} vừa được thêm mới thành công bởi Đại lý {Store.Code} - {Store.Name}",
                NotificationType.APPROVE => $"Đơn hàng số {DirectSalesOrder.Code} vừa được phê duyệt thành công bởi đại lý {Store.Code} - {Store.Name}",
                NotificationType.REJECT => $"Đơn hàng số {DirectSalesOrder.Code} vừa bị từ chối bởi đại lý {Store.Code} - {Store.Name}",
                _ => $"Đại lý {Store.Code} - {Store.Name} vừa thêm mới đơn hàng số {DirectSalesOrder.Code}"
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{WebDirectSalesOrderRoute.DMSMaster}#*".Replace("*", DirectSalesOrder.Id.ToString()),
                LinkMobile = $"{WebDirectSalesOrderRoute.DMSMobile}".Replace("*", DirectSalesOrder.Id.ToString()),
                Time = StaticParams.DateTimeNow,
                Unread = true,
                SenderRowId = SenderRowId,
                RecipientRowId = RecipientRowId,
                RowId = Guid.NewGuid()
            };
            return GlobalUserNotification;
        }

        public GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, DirectSalesOrder DirectSalesOrder, Store Store, NotificationType NotificationType)
        {
            var content = NotificationType switch
            {
                NotificationType.TOSENDER => $"Bạn vừa gửi thành công đơn hàng số {DirectSalesOrder.Code}",
                NotificationType.TOAPPROVER => $"Bạn vừa phê duyệt thành công đơn hàng số {DirectSalesOrder.Code}",
                NotificationType.TOREJECTER => $"Bạn vừa từ chối đơn hàng số {DirectSalesOrder.Code}",
                _ => $"Bạn vừa gửi thành công đơn hàng số {DirectSalesOrder.Code}",
            };
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = content,
                LinkWebsite = $"{WebDirectSalesOrderRoute.Preview}?id=*".Replace("*", DirectSalesOrder.Id.ToString()),
                LinkMobile = $"{WebDirectSalesOrderRoute.Mobile}".Replace("*", DirectSalesOrder.Id.ToString()),
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
