using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class StoreCheckingStatusEnum
    {
        public static GenericEnum CHECKED = new GenericEnum { Id = 1, Code = "CHECKED", Name = "Đã viếng thăm" };
        public static GenericEnum NOTCHECKED = new GenericEnum { Id = 0, Code = "NOT_CHECKED", Name = "Chưa viếng thăm" };
        public static GenericEnum ALL = new GenericEnum { Id = 2, Code = "ALL", Name = "Tất cả" };

        public static List<GenericEnum> StoreCheckingStatusEnumList = new List<GenericEnum>()
        {
            CHECKED, NOTCHECKED
        };
    }
}
