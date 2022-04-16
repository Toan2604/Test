using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class LuckyDrawTypeEnum
    {
        public static GenericEnum ALLSTORE = new GenericEnum { Id = 1, Code = "ALLSTORE", Name = "Tất cả các đại lý" };
        public static GenericEnum STORETYPE = new GenericEnum { Id = 2, Code = "STORETYPE", Name = "Theo loại đại lý" };
        public static GenericEnum STOREGROUPING = new GenericEnum { Id = 3, Code = "STOREGROUPING", Name = "Theo nhóm đại lý" };
        public static GenericEnum STORE = new GenericEnum { Id = 4, Code = "STORE", Name = "Theo đại lý" };
        public static List<GenericEnum> LuckyDrawTypeEnumList = new List<GenericEnum>
        {
            ALLSTORE, STORETYPE, STOREGROUPING, STORE
        };
    }
}
