using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Dim_DateMappingDAO
    {
        public long DateMappingId { get; set; }
        public DateTime Date { get; set; }
        public long DateId { get; set; }
        public long MonthId { get; set; }
        public long QuarterId { get; set; }
        public long YearId { get; set; }
    }
}
