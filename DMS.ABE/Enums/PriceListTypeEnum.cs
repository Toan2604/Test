using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class PriceListTypeEnum
    {
        public static GenericEnum ALLSTORE = new GenericEnum { Id = 1, Code = "ALLSTORE", Name = "Tất cả đại lý" };
        public static GenericEnum STORETYPE = new GenericEnum { Id = 2, Code = "STORETYPE", Name = "Theo loại đại lý" };
        public static GenericEnum STOREGROUPING = new GenericEnum { Id = 3, Code = "STOREGROUPING", Name = "Theo nhóm đại lý" };
        public static GenericEnum DETAILS = new GenericEnum { Id = 4, Code = "DETAILS", Name = "Chọn đại lý" };
        public static List<GenericEnum> PriceListTypeEnumList = new List<GenericEnum>()
        {
            ALLSTORE, STORETYPE, STOREGROUPING, DETAILS
        };
    }
}
