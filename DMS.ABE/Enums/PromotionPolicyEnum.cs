using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class PromotionPolicyEnum
    {
        public static GenericEnum SALES_ORDER = new GenericEnum { Id = 1, Code = "SALES_ORDER", Name = "Khuyến mại theo giá trị đơn hàng" };
        public static GenericEnum STORE = new GenericEnum { Id = 2, Code = "STORE", Name = "Khuyến mại theo khách hàng" };
        public static GenericEnum STORE_GROUPING = new GenericEnum { Id = 3, Code = "STORE_GROUPING", Name = "Khuyến mại theo nhóm khách hàng" };
        public static GenericEnum STORE_TYPE = new GenericEnum { Id = 4, Code = "STORE_TYPE", Name = "Khuyến mại theo loại khách hàng" };
        public static GenericEnum PRODUCT = new GenericEnum { Id = 5, Code = "PRODUCT", Name = "Khuyến mại theo sản phẩm" };
        public static GenericEnum PRODUCT_GROUPING = new GenericEnum { Id = 6, Code = "PRODUCT_GROUPING", Name = "Khuyến mại theo nhóm sản phẩm" };
        public static GenericEnum PRODUCT_TYPE = new GenericEnum { Id = 7, Code = "PRODUCT_TYPE", Name = "Khuyến mại theo loại sản phẩm" };
        public static GenericEnum COMBO = new GenericEnum { Id = 8, Code = "COMBO", Name = "Khuyến mại theo combo sản phẩm" };
        public static GenericEnum SAME_PRICE = new GenericEnum { Id = 9, Code = "SAME_PRICE", Name = "Khuyến mại đồng giá" };

        public static List<GenericEnum> PromotionPolicyEnumList = new List<GenericEnum>()
        {
            SALES_ORDER, STORE, STORE_GROUPING, STORE_TYPE,
            PRODUCT, PRODUCT_GROUPING, PRODUCT_TYPE, COMBO, SAME_PRICE
        };
    }
}
