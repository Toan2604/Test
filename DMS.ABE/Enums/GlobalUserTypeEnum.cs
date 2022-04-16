using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.ABE.Enums
{
    public class GlobalUserTypeEnum
    {
        public static GenericEnum APPUSER = new GenericEnum { Id = 1, Code = "APPUSER", Name = "Nhân viên" };
        public static GenericEnum STOREUSER = new GenericEnum { Id = 2, Code = "STOREUSER", Name = "Đại lý" };
        public static List<GenericEnum> GlobalUserTypeEnumList = new List<GenericEnum>()
        {
            APPUSER, STOREUSER
        };
    }
}
