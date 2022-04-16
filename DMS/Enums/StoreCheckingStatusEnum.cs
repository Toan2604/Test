using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class StoreCheckingStatusEnum
    {
        public static GenericEnum CHECKED = new GenericEnum { Id = 1, Code = "CHECKED", Name = "Đã ghé thăm trong ngày" };
        public static GenericEnum NOTCHECKED = new GenericEnum { Id = 0, Code = "NOT_CHECKED", Name = "Chưa ghé thăm trong ngày" };
        public static GenericEnum ALL = new GenericEnum { Id = 2, Code = "ALL", Name = "Tất cả" };
        public static GenericEnum CHECKED_IN_MONTH = new GenericEnum { Id = 3, Code = "CHECKED_IN_MONTH", Name = "Đã ghé thăm trong tháng" };
        public static GenericEnum NOTCHECKED_IN_MONTH = new GenericEnum { Id = 4, Code = "NOT_CHECKED_IN_MONTH", Name = "Chưa ghé thăm trong tháng" };

        public static List<GenericEnum> StoreCheckingStatusEnumList = new List<GenericEnum>()
        {
            CHECKED, NOTCHECKED, CHECKED_IN_MONTH, NOTCHECKED_IN_MONTH
        };
    }
}
