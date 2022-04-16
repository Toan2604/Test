using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class StoreStatusHistoryTypeEnum
    {
        public static GenericEnum NEW = new GenericEnum { Id = 1, Code = "NEW", Name = "Tạo mới" };
        public static GenericEnum STORE_SCOUTING = new GenericEnum { Id = 2, Code = "STORE_SCOUTING", Name = "Từ cắm cờ" };
        public static GenericEnum DRAFT = new GenericEnum { Id = 3, Code = "DRAFT", Name = "Dự thảo" };
        public static GenericEnum OFFICIAL = new GenericEnum { Id = 4, Code = "OFFICIAL", Name = "Chính thức" };
        public static List<GenericEnum> StoreStatusHistoryTypeEnumList = new List<GenericEnum>()
        {
            NEW, STORE_SCOUTING, DRAFT, OFFICIAL
        };
    }
}
