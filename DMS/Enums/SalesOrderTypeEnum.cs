using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class SalesOrderTypeEnum
    {
        public static GenericEnum ALL = new GenericEnum { Id = 1, Code = "ALL", Name = "Tất cả đơn hàng" };
        public static GenericEnum INDIRECT = new GenericEnum { Id = 2, Code = "INDIRECT", Name = "Đơn hàng gián tiếp" };
        public static GenericEnum DIRECT = new GenericEnum { Id = 3, Code = "DIRECT", Name = "Đơn hàng trực tiếp" };
        public static List<GenericEnum> SalesOrderTypeEnumList = new List<GenericEnum>()
        {
            ALL, INDIRECT, DIRECT
        };
    }
}
