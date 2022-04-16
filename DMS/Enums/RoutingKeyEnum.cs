using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
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
        public static GenericEnum IndirectSalesOrderSync = new GenericEnum { Id = 10, Code = "IndirectSalesOrder.Sync", Name = "Đồng bộ IndirectSalesOrder" };
        public static GenericEnum GlobalUserSync = new GenericEnum { Code = "GlobalUser.Sync", Name = "Đồng bộ GlobalUser" };
        public static GenericEnum BrandHistorySync = new GenericEnum { Code = "BrandHistory.Sync", Name = "Đồng bộ BrandHistory" };
        public static GenericEnum ProductGroupingHistorySync = new GenericEnum { Code = "ProductGroupingHistory.Sync", Name = "Đồng bộ ProductGroupingHistory" };
        //public static GenericEnum PermissionSync = new GenericEnum { Code = "Permission.Sync", Name = "Đồng bộ Permission" };

        public static GenericEnum ProductCal = new GenericEnum { Code = "Product.Cal", Name = "Tính số lượng đơn hàng" };

        public static GenericEnum MailSend = new GenericEnum { Id = 4, Code = "Mail.Send", Name = "Gửi Mail" };
        public static GenericEnum AuditLogSend = new GenericEnum { Id = 5, Code = "AuditLog.Send", Name = "Audit Log" };
        public static GenericEnum SystemLogSend = new GenericEnum { Id = 6, Code = "SystemLog.Send", Name = "System Log" };
        public static GenericEnum AppUserNotificationSend = new GenericEnum { Id = 7, Code = "AppUserNotification.Send", Name = "UserNotification" };
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
        public static GenericEnum StoreUsed = new GenericEnum { Id = 107, Code = "Store.Used", Name = "Store Used" };
        public static GenericEnum StoreTypeUsed = new GenericEnum { Id = 108, Code = "StoreType.Used", Name = "StoreType Used" };
        public static GenericEnum SupplierUsed = new GenericEnum { Id = 109, Code = "Supplier.Used", Name = "Supplier Used" };
        public static GenericEnum SurveyUsed = new GenericEnum { Id = 110, Code = "Survey.Used", Name = "Survey Used" };
        public static GenericEnum TaxTypeUsed = new GenericEnum { Id = 111, Code = "TaxType.Used", Name = "TaxType Used" };
        public static GenericEnum UnitOfMeasureUsed = new GenericEnum { Id = 112, Code = "UnitOfMeasure.Used", Name = "UnitOfMeasure Used" };
        public static GenericEnum WorkflowDefinitionUsed = new GenericEnum { Id = 113, Code = "DMS.WorkflowDefinition.Used", Name = "WorkflowDefinition Used" };
        public static GenericEnum RoleUsed = new GenericEnum { Id = 114, Code = "Role.Used", Name = "Role Used" };
        public static GenericEnum PromotionCodeUsed = new GenericEnum { Id = 115, Code = "PromotionCode.Used", Name = "PromotionCode Used" };
        public static GenericEnum StoreGroupingUsed = new GenericEnum { Id = 116, Code = "StoreGrouping.Used", Name = "StoreGrouping Used" };
        public static GenericEnum ProvinceUsed = new GenericEnum { Id = 117, Code = "Province.Used", Name = "Province Used" };
        public static GenericEnum DistrictUsed = new GenericEnum { Id = 118, Code = "District.Used", Name = "District Used" };
        public static GenericEnum WardUsed = new GenericEnum { Id = 119, Code = "Ward.Used", Name = "Ward Used" };
        public static GenericEnum StoreUserUsed = new GenericEnum { Id = 120, Code = "StoreUser.Used", Name = "StoreUser Used" };
        public static GenericEnum ShowingItemUsed = new GenericEnum { Id = 121, Code = "ShowingItem.Used", Name = "ShowingItem Used" };
        public static GenericEnum ShowingCategoryUsed = new GenericEnum { Id = 122, Code = "ShowingCategory.Used", Name = "ShowingCategory Used" };
        public static GenericEnum LuckyDrawUsed = new GenericEnum { Id = 123, Code = "LuckyDraw.Used", Name = "LuckyDraw Used" };
        public static GenericEnum ProblemSync = new GenericEnum { Id = 123, Code = "Problem.Sync", Name = "Problem Sync" };
        public static GenericEnum ProblemTypeSync = new GenericEnum { Id = 124, Code = "ProblemType.Sync", Name = "ProblemType Sync" };
        public static GenericEnum ShowingOrderSync = new GenericEnum { Id = 125, Code = "ShowingOrder.Sync", Name = "ShowingOrder Sync" };
        public static GenericEnum ShowingOrderWithDrawSync = new GenericEnum { Id = 126, Code = "ShowingOrderWithDraw.Sync", Name = "ShowingOrderWithDraw Sync" };
        public static GenericEnum ShowingItemSync = new GenericEnum { Id = 127, Code = "ShowingItem.Sync", Name = "ShowingItem Sync" };
        public static GenericEnum StoreCheckingSync = new GenericEnum { Id = 128, Code = "StoreChecking.Sync", Name = "StoreChecking Sync" };
        public static GenericEnum StoreScoutingSync = new GenericEnum { Id = 129, Code = "StoreScouting.Sync", Name = "StoreScouting Sync" };
        public static GenericEnum StoreScoutingTypeSync = new GenericEnum { Id = 130, Code = "StoreScoutingType.Sync", Name = "StoreScoutingType Sync" };
        public static GenericEnum StoreStatusHistorySync = new GenericEnum { Id = 131, Code = "StoreStatusHistory.Sync", Name = "StoreStatusHistory Sync" };
        public static GenericEnum StoreUncheckingSync = new GenericEnum { Id = 132, Code = "StoreUnchecking.Sync", Name = "StoreUnchecking Sync" };
        public static GenericEnum UnitOfMeasureSync = new GenericEnum { Id = 133, Code = "UnitOfMeasure.Sync", Name = "UnitOfMeasure Sync" };
        public static GenericEnum ShowingCategorySync = new GenericEnum { Id = 134, Code = "ShowingCategory.Sync", Name = "ShowingCategory Sync" };
        public static GenericEnum BrandInStoreSync = new GenericEnum { Id = 136, Code = "BrandInStore.Sync", Name = "BrandInStoreSync" };
        public static GenericEnum NewItemHandlerSync = new GenericEnum { Id = 137, Code = "NewItemHandler.Sync", Name = "NewItemHandlerSync" };
        public static GenericEnum KpiGeneralSync = new GenericEnum { Id = 138, Code = "KpiGeneral.Sync", Name = "KpiGeneralSync" };
        public static GenericEnum KpiCriteriaGeneralSync = new GenericEnum { Id = 139, Code = "KpiCriteriaGeneral.Sync", Name = "KpiCriteriaGeneralSync" };
        public static GenericEnum AlbumSync = new GenericEnum { Id = 140, Code = "Album.Sync", Name = "AlbumSync" };
        public static GenericEnum GlobalUserNotificationSend = new GenericEnum { Id = 141, Code = "GlobalUserNotification.Send", Name = "GlobalUserNotificationSend" };
        public static GenericEnum ProductNew = new GenericEnum { Id = 142, Code = "Product.New", Name = "Đồng bộ Product mới" };
        public static GenericEnum WarehouseCheckOrder = new GenericEnum { Id = 143, Code = "Warehouse.CheckOrder", Name = "Phân luồng đơn hàng theo tồn kho" };
        public static GenericEnum MenuSend = new GenericEnum { Id = 144, Code = "Menu.Send", Name = "Menu Send" };
        public static GenericEnum RoleSend = new GenericEnum { Id = 145, Code = "Role.Send", Name = "Role Send" };

        public static GenericEnum StoreHistorySync = new GenericEnum { Id = 146, Code = "StoreHistory.His", Name = "StoreHistorySync" };
        public static GenericEnum StoreBalanceSync = new GenericEnum { Id = 147, Code = "StoreBalance.Sync", Name = "StoreBalance Sync" };
        public static GenericEnum WarehouseSync = new GenericEnum { Id = 148, Code = "Warehouse.Sync", Name = "Warehouse Sync" };
        public static GenericEnum PriceListSync = new GenericEnum { Id = 149, Code = "PriceList.Sync", Name = "PriceList Sync" };

        public static GenericEnum MenuSync = new GenericEnum { Code = "Menu.Sync", Name = "Menu Sync" };
        public static GenericEnum PageSync = new GenericEnum { Code = "Page.Sync", Name = "Page Sync" };
        public static GenericEnum FieldSync = new GenericEnum { Code = "Field.Sync", Name = "Field Sync" };
        public static GenericEnum ActionSync = new GenericEnum { Code = "Action.Sync", Name = "Action Sync" };
        public static GenericEnum ActionPageMappingSync = new GenericEnum { Code = "ActionPageMapping.Sync", Name = "ActionPageMapping Sync" };
        public static GenericEnum AppUserGps = new GenericEnum { Code = "AppUser.Gps", Name = "AppUser Gps Update" };
        public static GenericEnum AppUserClear = new GenericEnum { Code = "AppUser.Clear", Name = "AppUser Gps Clear" };
    }
}