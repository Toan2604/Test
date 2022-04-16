using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class CheckStateEnum
    {
        public static GenericEnum NOT_PASS = new GenericEnum { Id = 0, Code = "NotPass", Name = "Không đạt" };
        public static GenericEnum PASS = new GenericEnum { Id = 1, Code = "Pass", Name = "Đạt" };
        public static List<GenericEnum> CheckStateEnumList = new List<GenericEnum>
        {
            NOT_PASS,PASS,
        };
    }
}
