using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class EntityComponentEnum
    {
        public static GenericEnum AUTO_NUMBER = new GenericEnum { Id = 1, Code = "AutoNumber", Name = "Mã số tự sinh" };
        public static GenericEnum TEXT = new GenericEnum { Id = 2, Code = "Text", Name = "Text" };
        public static GenericEnum ORGANIZATION = new GenericEnum { Id = 3, Code = "OrderOrganization", Name = "Đơn vị tổ chức" };

        public static GenericEnum ORDER_YEAR = new GenericEnum { Id = 11, Code = "OrderYear", Name = "Năm đặt hàng" };
        public static List<GenericEnum> OrderEnumList = new List<GenericEnum>
        {
            AUTO_NUMBER, TEXT, ORGANIZATION, ORDER_YEAR
        };

        public static GenericEnum PRODUCT_PRODUCT_TYPE = new GenericEnum { Id = 21, Code = "ProductType", Name = "Loại sản phẩm" };
        public static GenericEnum PRODUCT_CATEGORY = new GenericEnum { Id = 22, Code = "Category", Name = "Danh mục sản phẩm" };
        public static List<GenericEnum> ProductEnumList = new List<GenericEnum>
        {
            AUTO_NUMBER, TEXT, PRODUCT_PRODUCT_TYPE, PRODUCT_CATEGORY
        };

        public static GenericEnum STORE_STORE_TYPE = new GenericEnum { Id = 31, Code = "StoreType", Name = "Cấp đại lý" };
        public static List<GenericEnum> StoreEnumList = new List<GenericEnum>
        {
            AUTO_NUMBER, TEXT, ORGANIZATION, STORE_STORE_TYPE
        };

        public static List<GenericEnum> EntityComponentEnumList = new List<GenericEnum>
        {
            AUTO_NUMBER, TEXT, ORGANIZATION, ORDER_YEAR, PRODUCT_PRODUCT_TYPE, PRODUCT_CATEGORY, STORE_STORE_TYPE
        };
    }
}
