using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class EntityComponentEnum
    {
        public static GenericEnum AUTO_NUMBER = new GenericEnum { Id = 1, Code = "AutoNumber", Name = "Mã số tự sinh" };
        public static GenericEnum TEXT = new GenericEnum { Id = 2, Code = "Text", Name = "Text" };
        public static GenericEnum ORGANIZATION = new GenericEnum { Id = 3, Code = "OrderOrganization", Name = "Đơn vị tổ chức" };

        //Đơn hàng
        public static GenericEnum ORDER_YEAR = new GenericEnum { Id = 11, Code = "OrderYear", Name = "Năm đặt hàng" };
        public static List<GenericEnum> OrderEnumList = new List<GenericEnum>
        {
            AUTO_NUMBER, TEXT, ORGANIZATION, ORDER_YEAR
        };

        //Sản phẩm
        public static GenericEnum PRODUCT_PRODUCT_TYPE = new GenericEnum { Id = 21, Code = "ProductType", Name = "Loại sản phẩm" };
        public static GenericEnum PRODUCT_CATEGORY = new GenericEnum { Id = 22, Code = "Category", Name = "Danh mục sản phẩm" };
        public static List<GenericEnum> ProductEnumList = new List<GenericEnum>
        {
            AUTO_NUMBER, TEXT, PRODUCT_PRODUCT_TYPE, PRODUCT_CATEGORY
        };

        //Đại lý
        public static GenericEnum STORE_STORE_TYPE = new GenericEnum { Id = 31, Code = "StoreType", Name = "Cấp đại lý" };
        public static List<GenericEnum> StoreEnumList = new List<GenericEnum>
        {
            AUTO_NUMBER, TEXT, ORGANIZATION, STORE_STORE_TYPE
        };

        //Khách hàng
        public static GenericEnum CUSTOMER_YEAR = new GenericEnum { Id = 41, Code = "CustomerYear", Name = "Năm" };
        public static GenericEnum CUSTOMER_TYPE = new GenericEnum { Id = 42, Code = "CustomerType", Name = "Loại khách hàng" };
        public static GenericEnum CUSTOMER_GROUPING = new GenericEnum { Id = 43, Code = "CustomerGrouping", Name = "Nhóm khách hàng" };
        public static GenericEnum CUSTOMER_RESOURCE = new GenericEnum { Id = 44, Code = "CustomerResource", Name = "Nguồn khách hàng" };
        public static GenericEnum BUSINESS_TYPE = new GenericEnum { Id = 45, Code = "BusinessType", Name = "Loại hình doanh nghiệp" };
        public static List<GenericEnum> CustomerEnumList = new List<GenericEnum>
        {
            ORGANIZATION, CUSTOMER_YEAR, AUTO_NUMBER, CUSTOMER_TYPE, CUSTOMER_GROUPING, CUSTOMER_RESOURCE, BUSINESS_TYPE
        };

        //Cơ hội/dự án
        public static GenericEnum OPPORTUNITY_YEAR = new GenericEnum { Id = 51, Code = "OpportunityYear", Name = "Năm" };
        public static List<GenericEnum> OpportunityEnumList = new List<GenericEnum>
        {
            ORGANIZATION, OPPORTUNITY_YEAR, AUTO_NUMBER
        };

        public static List<GenericEnum> EntityComponentEnumList = new List<GenericEnum>
        {
            AUTO_NUMBER, TEXT, ORGANIZATION, ORDER_YEAR, PRODUCT_PRODUCT_TYPE, PRODUCT_CATEGORY, STORE_STORE_TYPE, CUSTOMER_YEAR, CUSTOMER_TYPE, CUSTOMER_GROUPING,
            CUSTOMER_RESOURCE, BUSINESS_TYPE, OPPORTUNITY_YEAR
        };
    }
}
