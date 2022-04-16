using DMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Common
{
    public interface ITimeService : IServiceScoped
    {
        public Tuple<DateTime, DateTime>  ConvertDashboardTime(IdFilter Time, ICurrentContext CurrentContext);
        public DateTime LocalStartDay(ICurrentContext CurrentContext);
        public DateTime LocalEndDay(ICurrentContext CurrentContext);
    }
    public class TimeService : ITimeService
    {
        public Tuple<DateTime, DateTime> ConvertDashboardTime(IdFilter Time, ICurrentContext CurrentContext)
        {
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            DateTime Now = StaticParams.DateTimeNow;
            if (Time.Equal.HasValue == false)
            {
                Time.Equal = 0;
                Start = LocalStartDay(CurrentContext);
                End = LocalEndDay(CurrentContext);
            }
            else if (Time.Equal.Value == DashboardPeriodTimeEnum.TODAY.Id)
            {
                Start = LocalStartDay(CurrentContext);
                End = LocalEndDay(CurrentContext);
            }
            else if (Time.Equal.Value == DashboardPeriodTimeEnum.THIS_WEEK.Id)
            {
                int diff = (7 + (Now.AddHours(0 - CurrentContext.TimeZone).DayOfWeek - DayOfWeek.Monday)) % 7;
                Start = LocalStartDay(CurrentContext).AddDays(-1 * diff);
                End = Start.AddDays(7).AddSeconds(-1);
            }
            else if (Time.Equal.Value == DashboardPeriodTimeEnum.THIS_MONTH.Id)
            {
                Start = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddHours(0 - CurrentContext.TimeZone);
                End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
            }
            else if (Time.Equal.Value == DashboardPeriodTimeEnum.LAST_MONTH.Id)
            {
                Start = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddMonths(-1).AddHours(0 - CurrentContext.TimeZone);
                End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
            }
            else if (Time.Equal.Value == DashboardPeriodTimeEnum.THIS_QUARTER.Id)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.AddHours(CurrentContext.TimeZone).Month / 3m));
                Start = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, (this_quarter - 1) * 3 + 1, 1).AddHours(0 - CurrentContext.TimeZone);
                End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, (this_quarter - 1) * 3 + 1, 1).AddMonths(3).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
            }
            else if (Time.Equal.Value == DashboardPeriodTimeEnum.LAST_QUATER.Id)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.AddHours(CurrentContext.TimeZone).Month / 3m));
                Start = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, (this_quarter - 1) * 3 + 1, 1).AddMonths(-3).AddHours(0 - CurrentContext.TimeZone);
                End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, (this_quarter - 1) * 3 + 1, 1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
            }
            else if (Time.Equal.Value == DashboardPeriodTimeEnum.YEAR.Id)
            {
                Start = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, 1, 1).AddHours(0 - CurrentContext.TimeZone);
                End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, 1, 1).AddYears(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
            }
            else if (Time.Equal.Value == DashboardPeriodTimeEnum.LAST_WEEK.Id)
            {
                int diff = (7 + (Now.AddHours(0 - CurrentContext.TimeZone).DayOfWeek - DayOfWeek.Monday)) % 7;
                Start = LocalStartDay(CurrentContext).AddDays(-1 * diff);
                Start = Start.AddDays(-7);
                End = Start.AddDays(7).AddSeconds(-1);
            }
            return Tuple.Create(Start, End);
        }

        public DateTime LocalStartDay(ICurrentContext CurrentContext)
        {
            DateTime Start = StaticParams.DateTimeNow.Date.AddHours(0 - CurrentContext.TimeZone);
            return Start;
        }

        public DateTime LocalEndDay(ICurrentContext CurrentContext)
        {
            DateTime End = StaticParams.DateTimeNow.Date.AddHours(0 - CurrentContext.TimeZone).AddDays(1).AddSeconds(-1);
            return End;
        }
    }



    public class DashboardPeriodTimeEnum
    {
        public static GenericEnum TODAY = new GenericEnum { Id = 1, Code = "TODAY", Name = "Hôm nay" };
        public static GenericEnum THIS_WEEK = new GenericEnum { Id = 2, Code = "THIS_WEEK", Name = "Tuần này" };
        public static GenericEnum LAST_WEEK = new GenericEnum { Id = 3, Code = "LAST_WEEK", Name = "Tuần trước" };
        public static GenericEnum THIS_MONTH = new GenericEnum { Id = 4, Code = "THIS_MONTH", Name = "Tháng này" };
        public static GenericEnum LAST_MONTH = new GenericEnum { Id = 5, Code = "LAST_MONTH", Name = "Tháng trước" };
        public static GenericEnum THIS_QUARTER = new GenericEnum { Id = 6, Code = "THIS_QUARTER", Name = "Quý này" };
        public static GenericEnum LAST_QUATER = new GenericEnum { Id = 7, Code = "LAST_QUATER", Name = "Quý trước" };
        public static GenericEnum YEAR = new GenericEnum { Id = 8, Code = "YEAR", Name = "Năm" };
    }
}
