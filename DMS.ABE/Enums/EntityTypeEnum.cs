using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class EntityTypeEnum
    {
        public static GenericEnum SALES_ORDER = new GenericEnum { Id = 1, Code = "SalesOrder", Name = "Đơn hàng" };
        public static GenericEnum PRODUCT = new GenericEnum { Id = 2, Code = "Product", Name = "Sản phẩm" };
        public static GenericEnum STORE = new GenericEnum { Id = 3, Code = "Store", Name = "Đại lý" };
        public static GenericEnum CUSTOMER = new GenericEnum { Id = 4, Code = "Customer", Name = "Khách hàng" };

        public static List<GenericEnum> EntityTypeEnumList = new List<GenericEnum>
        {
            SALES_ORDER, CUSTOMER, STORE
        };
    }
}
