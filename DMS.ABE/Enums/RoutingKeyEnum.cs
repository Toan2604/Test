using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class RoutingKeyEnum
    {
        public static GenericEnum AppUserSync = new GenericEnum { Id = 1, Code = "AppUser.Sync", Name = "Đồng bộ AppUser" };
        public static GenericEnum OrganizationSync = new GenericEnum { Id = 2, Code = "Organization.Sync", Name = "Đồng bộ Organization" };
        public static GenericEnum StoreSync = new GenericEnum { Id = 3, Code = "Store.Sync", Name = "Đồng bộ Store" };
        public static GenericEnum ProductSync = new GenericEnum { Id = 4, Code = "Product.Sync", Name = "Đồng bộ Product" };
        public static GenericEnum StoreStatusSync = new GenericEnum { Id = 5, Code = "StoreStatus.Sync", Name = "Đồng bộ StoreStatus" };
        public static GenericEnum StoreGroupingSync = new GenericEnum { Id = 6, Code = "StoreGrouping.Sync", Name = "Đồng bộ StoreGrouping" };
        public static GenericEnum StoreTypeSync = new GenericEnum { Id = 7, Code = "StoreType.Sync", Name = "Đồng bộ StoreType" };
        public static GenericEnum DirectSalesOrderSync = new GenericEnum { Id = 8, Code = "DirectSalesOrder.Sync", Name = "Đồng bộ DirectSalesOrder" };
        public static GenericEnum StoreUserSync = new GenericEnum { Id = 9, Code = "StoreUser.Sync", Name = "Đồng bộ StoreUser" };
        public static GenericEnum GlobalUserSync = new GenericEnum {  Code = "GlobalUser.Sync", Name = "Đồng bộ GlobalUser" };

        public static GenericEnum MailSend = new GenericEnum { Id = 4, Code = "Mail.Send", Name = "Gửi Mail" };
        public static GenericEnum AuditLogSend = new GenericEnum { Id = 5, Code = "AuditLog.Send", Name = "Audit Log" };
        public static GenericEnum SystemLogSend = new GenericEnum { Id = 6, Code = "SystemLog.Send", Name = "System Log" };
        public static GenericEnum UserNotificationSend = new GenericEnum { Id = 7, Code = "UserNotification.Send", Name = "UserNotification" };
        public static List<GenericEnum> RoutingKeyEnumList = new List<GenericEnum>()
        {
            AppUserSync, OrganizationSync, StoreSync, MailSend, AuditLogSend, SystemLogSend
        };

        public static GenericEnum AppUserUsed = new GenericEnum { Id = 1001, Code = "AppUser.Used", Name = "AppUser Used" };
        public static GenericEnum OrganizationUsed = new GenericEnum { Id = 1002, Code = "Organization.Used", Name = "Organization Used" };

        public static GenericEnum AlbumUsed = new GenericEnum { Id = 101, Code = "Album.Used", Name = "Album Used" };
        public static GenericEnum BrandUsed = new GenericEnum { Id = 102, Code = "Brand.Used", Name = "Brand Used" };
        public static GenericEnum ItemUsed = new GenericEnum { Id = 103, Code = "Item.Used", Name = "Item Used" };
        public static GenericEnum ProductUsed = new GenericEnum { Id = 104, Code = "Product.Used", Name = "Product Used" };
        public static GenericEnum ProductTypeUsed = new GenericEnum { Id = 105, Code = "ProductType.Used", Name = "ProductType Used" };
        public static GenericEnum ProblemTypeUsed = new GenericEnum { Id = 106, Code = "ProblemType.Used", Name = "ProblemType Used" };
        public static GenericEnum StoreUsed = new GenericEnum { Id = 107, Code = "DMS.ABE.Store.Used", Name = "Store Used" };
        public static GenericEnum StoreTypeUsed = new GenericEnum { Id = 108, Code = "StoreType.Used", Name = "StoreType Used" };
        public static GenericEnum SupplierUsed = new GenericEnum { Id = 109, Code = "Supplier.Used", Name = "Supplier Used" };
        public static GenericEnum SurveyUsed = new GenericEnum { Id = 110, Code = "Survey.Used", Name = "Survey Used" };
        public static GenericEnum TaxTypeUsed = new GenericEnum { Id = 111, Code = "TaxType.Used", Name = "TaxType Used" };
        public static GenericEnum UnitOfMeasureUsed = new GenericEnum { Id = 112, Code = "UnitOfMeasure.Used", Name = "UnitOfMeasure Used" };
        public static GenericEnum WorkflowDefinitionUsed = new GenericEnum { Id = 113, Code = "WorkflowDefinition.Used", Name = "WorkflowDefinition Used" };
        public static GenericEnum RoleUsed = new GenericEnum { Id = 114, Code = "Role.Used", Name = "Role Used" };
        public static GenericEnum PromotionCodeUsed = new GenericEnum { Id = 115, Code = "PromotionCode.Used", Name = "PromotionCode Used" };
        public static GenericEnum StoreGroupingUsed = new GenericEnum { Id = 116, Code = "StoreGrouping.Used", Name = "StoreGrouping Used" };
        public static GenericEnum ProvinceUsed = new GenericEnum { Id = 117, Code = "Province.Used", Name = "Province Used" };
        public static GenericEnum DistrictUsed = new GenericEnum { Id = 118, Code = "District.Used", Name = "District Used" };
        public static GenericEnum WardUsed = new GenericEnum { Id = 119, Code = "Ward.Used", Name = "Ward Used" };
        public static GenericEnum StoreUserUsed = new GenericEnum { Id = 120, Code = "DMS.ABE.StoreUser.Used", Name = "StoreUser Used" };
        public static GenericEnum ShowingItemUsed = new GenericEnum { Id = 121, Code = "DMS.ABE.ShowingItem.Used", Name = "ShowingItem Used" };
        public static GenericEnum ShowingCategoryUsed = new GenericEnum { Id = 121, Code = "DMS.ABE.ShowingCategory.Used", Name = "ShowingCategory Used" };
        public static GenericEnum GlobalUserNotificationSend = new GenericEnum { Id = 122, Code = "GlobalUserNotification.Send", Name = "GlobalUserNotificationSend" };
    }
}