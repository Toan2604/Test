using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class StoreStatusEnum
    {
        public static GenericEnum ALL = new GenericEnum { Id = 1, Code = "ALL", Name = "Tất cả" };
        public static GenericEnum DRAFT = new GenericEnum { Id = 2, Code = "DRAFT", Name = "Dự thảo" };
        public static GenericEnum OFFICIAL = new GenericEnum { Id = 3, Code = "OFFICIAL", Name = "Chính thức" };
        public static List<GenericEnum> StoreStatusEnumList = new List<GenericEnum>()
        {
            ALL, DRAFT, OFFICIAL
        };
    }
}
