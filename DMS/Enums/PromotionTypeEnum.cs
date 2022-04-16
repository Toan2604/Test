using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class PromotionTypeEnum
    {
        public static GenericEnum ALL = new GenericEnum { Id = 1, Code = "ALL", Name = "Tất cả đại lý" };
        public static GenericEnum ORGANIZATION = new GenericEnum { Id = 2, Code = "ORGANIZATION", Name = "Theo đơn vị" };
        public static GenericEnum STORE = new GenericEnum { Id = 3, Code = "STORE", Name = "Chi tiết" };

        public static List<GenericEnum> PromotionTypeEnumList = new List<GenericEnum>()
        {
            ALL, ORGANIZATION, STORE
        };
    }
}
