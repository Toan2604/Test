using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class KpiPeriodEnum
    {
        public static GenericEnum PERIOD_MONTH01 = new GenericEnum { Id = 101, Code = "M01", Name = "Tháng 1" };
        public static GenericEnum PERIOD_MONTH02 = new GenericEnum { Id = 102, Code = "M02", Name = "Tháng 2" };
        public static GenericEnum PERIOD_MONTH03 = new GenericEnum { Id = 103, Code = "M03", Name = "Tháng 3" };
        public static GenericEnum PERIOD_MONTH04 = new GenericEnum { Id = 104, Code = "M04", Name = "Tháng 4" };
        public static GenericEnum PERIOD_MONTH05 = new GenericEnum { Id = 105, Code = "M05", Name = "Tháng 5" };
        public static GenericEnum PERIOD_MONTH06 = new GenericEnum { Id = 106, Code = "M06", Name = "Tháng 6" };
        public static GenericEnum PERIOD_MONTH07 = new GenericEnum { Id = 107, Code = "M07", Name = "Tháng 7" };
        public static GenericEnum PERIOD_MONTH08 = new GenericEnum { Id = 108, Code = "M08", Name = "Tháng 8" };
        public static GenericEnum PERIOD_MONTH09 = new GenericEnum { Id = 109, Code = "M09", Name = "Tháng 9" };
        public static GenericEnum PERIOD_MONTH10 = new GenericEnum { Id = 110, Code = "M10", Name = "Tháng 10" };
        public static GenericEnum PERIOD_MONTH11 = new GenericEnum { Id = 111, Code = "M11", Name = "Tháng 11" };
        public static GenericEnum PERIOD_MONTH12 = new GenericEnum { Id = 112, Code = "M12", Name = "Tháng 12" };

        public static GenericEnum PERIOD_QUATER01 = new GenericEnum { Id = 201, Code = "Q01", Name = "Quý 1" };
        public static GenericEnum PERIOD_QUATER02 = new GenericEnum { Id = 202, Code = "Q02", Name = "Quý 2" };
        public static GenericEnum PERIOD_QUATER03 = new GenericEnum { Id = 203, Code = "Q03", Name = "Quý 3" };
        public static GenericEnum PERIOD_QUATER04 = new GenericEnum { Id = 204, Code = "Q04", Name = "Quý 4" };

        public static GenericEnum PERIOD_YEAR01 = new GenericEnum { Id = 401, Code = "Y01", Name = "Năm" };


        public static List<GenericEnum> KpiPeriodEnumList = new List<GenericEnum>()
        {
           PERIOD_MONTH01,
           PERIOD_MONTH02,
           PERIOD_MONTH03,
           PERIOD_MONTH04,
           PERIOD_MONTH05,
           PERIOD_MONTH06,
           PERIOD_MONTH07,
           PERIOD_MONTH08,
           PERIOD_MONTH09,
           PERIOD_MONTH10,
           PERIOD_MONTH11,
           PERIOD_MONTH12,

           PERIOD_QUATER01,
           PERIOD_QUATER02,
           PERIOD_QUATER03,
           PERIOD_QUATER04,

           PERIOD_YEAR01,
        };
    }
}
