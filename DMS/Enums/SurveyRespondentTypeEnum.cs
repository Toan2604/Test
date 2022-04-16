using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class SurveyRespondentTypeEnum
    {
        public static GenericEnum STORE_SCOUTING = new GenericEnum { Id = 1, Code = "STORE_SCOUTING", Name = "Cửa hàng cắm cờ" };
        public static GenericEnum STORE = new GenericEnum { Id = 2, Code = "STORE", Name = "Đại lý" };
        public static GenericEnum OTHER = new GenericEnum { Id = 3, Code = "OTHER", Name = "Đối tượng khác" };
        public static List<GenericEnum> SurveyRespondentTypeEnumList = new List<GenericEnum>
        {
            STORE_SCOUTING,STORE,OTHER,
        };
    }
}
