using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Rpc.product;
using DMS.Services.MProduct;
using System;
using TrueSight.Common;

namespace DMS.Services.MProduct
{
    public interface IProductTemplate : IServiceScoped
    {
        GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, Product Product, AppUser AppUser, NotificationType NotificationType);
        GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, Product Product, AppUser AppUser, NotificationType NotificationType);
    }
    public class ProductTemplate : IProductTemplate
    {
        public GlobalUserNotification CreateAppUserNotification(Guid SenderRowId, Guid RecipientRowId, Product Product, AppUser AppUser, NotificationType NotificationType)
        {
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = $"Sản phẩm {Product.Code} - {Product.Name} vừa được đưa vào danh sách sản phẩm mới",
                LinkWebsite = $"{NewProductRoute.Master}/?id=*".Replace("*", Product.Id.ToString()),
                LinkMobile = $"{NewProductRoute.Mobile}".Replace("*", Product.Id.ToString()),
                RecipientRowId = RecipientRowId,
                SenderRowId = SenderRowId,
                Time = StaticParams.DateTimeNow,
                Unread = true,
                RowId = Guid.NewGuid(),
            };
            return GlobalUserNotification;
        }

        public GlobalUserNotification CreateStoreUserNotification(Guid SenderRowId, Guid RecipientRowId, Product Product, AppUser AppUser, NotificationType NotificationType)
        {
            GlobalUserNotification GlobalUserNotification = new GlobalUserNotification
            {
                TitleWeb = $"Thông báo từ DMS",
                ContentWeb = $"Sản phẩm {Product.Code} - {Product.Name} vừa được đưa vào danh sách sản phẩm mới",
                LinkWebsite = $"{NewProductRoute.Master}/?id=*".Replace("*", Product.Id.ToString()),
                LinkMobile = $"{NewProductRoute.MobileABE}".Replace("*", Product.Id.ToString()),
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
