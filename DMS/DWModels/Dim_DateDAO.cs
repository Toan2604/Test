using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Dim_DateDAO
    {
        public long DateId { get; set; }
        public DateTime Date { get; set; }
        public long Day { get; set; }
        public string DayOfWeekName { get; set; }
        public long DayOfWeek { get; set; }
        public long DayOfYear { get; set; }
    }
}
