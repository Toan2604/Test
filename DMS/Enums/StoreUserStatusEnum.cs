using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class StoreUserStatusEnum
    {
        public static GenericEnum NOT_YET_CREATED = new GenericEnum { Id = 1, Code = "NOT_YET_CREATED", Name = "Chưa mở" };
        public static GenericEnum ALREADY_CREATED = new GenericEnum { Id = 2, Code = "ALREADY_CREATED", Name = "Đã mở" };
        public static GenericEnum ALREADY_LOCKED = new GenericEnum { Id = 3, Code = "ALREADY_LOCKED", Name = "Đã khóa" };

        public static List<GenericEnum> StoreUserStatusEnumList = new List<GenericEnum>
        {
            NOT_YET_CREATED, ALREADY_CREATED, ALREADY_LOCKED
        };
    }
}
