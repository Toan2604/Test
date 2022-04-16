using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.director
{
    public class DashboardDirector_TimeDetailDTO : DataDTO
    {
        public DateTime StartDate{ get; set; }
        public DateTime EndDate{ get; set; }
    }

    public class DashboardDirector_TimeDetailFilterDTO : FilterDTO
    {
        public IdFilter PeriodTimeId { get; set; }
    }
}
