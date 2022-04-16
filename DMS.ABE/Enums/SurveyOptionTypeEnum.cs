using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class SurveyOptionTypeEnum
    {
        public static GenericEnum SINGLE = new GenericEnum { Id = 1, Code = "SINGLE", Name = "Lựa chọn đơn" };
        public static GenericEnum ROW = new GenericEnum { Id = 2, Code = "ROW", Name = "Hàng" };
        public static GenericEnum COLUMN = new GenericEnum { Id = 3, Code = "COLUMN", Name = "Cột" };
        public static List<GenericEnum> SurveyOptionTypeEnumList = new List<GenericEnum>()
        {
            SINGLE, ROW, COLUMN
        };
    }
}
