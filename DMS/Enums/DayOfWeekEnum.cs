using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class DayOfWeekEnum
    {
        public static GenericEnum MONDAY = new GenericEnum { Id = 1, Code = DayOfWeek.Monday.ToString(), Name = "Thứ 2" };
        public static GenericEnum TUESDAY = new GenericEnum { Id = 2, Code = DayOfWeek.Tuesday.ToString(), Name = "Thứ 3" };
        public static GenericEnum WEDNESDAY = new GenericEnum { Id = 3, Code = DayOfWeek.Wednesday.ToString(), Name = "Thứ 4" };
        public static GenericEnum THURSDAY = new GenericEnum { Id = 4, Code = DayOfWeek.Thursday.ToString(), Name = "Thứ 5" };
        public static GenericEnum FRIDAY = new GenericEnum { Id = 5, Code = DayOfWeek.Friday.ToString(), Name = "Thứ 6" };
        public static GenericEnum SATURDAY = new GenericEnum { Id = 6, Code = DayOfWeek.Saturday.ToString(), Name = "Thứ 7" };
        public static GenericEnum SUNDAY = new GenericEnum { Id = 7, Code = DayOfWeek.Sunday.ToString(), Name = "Chủ nhật" };
        public static List<GenericEnum> DayOfWeekEnumList = new List<GenericEnum>
        {
            MONDAY,TUESDAY,WEDNESDAY,THURSDAY,FRIDAY,SATURDAY,SUNDAY
        };
    }
}
