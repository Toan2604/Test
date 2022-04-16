using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.director
{
    public class DashboardDirector_RevenueFluctuationDTO : DataDTO
    {
        public List<DashboardDirector_RevenueFluctuationByMonthDTO> RevenueFluctuationByMonths { get; set; }
        public List<DashboardDirector_RevenueFluctuationByQuarterDTO> RevenueFluctuationByQuaters { get; set; }
        public List<DashboardDirector_RevenueFluctuationByYearDTO> RevenueFluctuationByYears { get; set; }
    }

    public class DashboardDirector_RevenueFluctuationByMonthDTO : DataDTO
    {
        public long Day { get; set; }
        public decimal Revenue { get; set; }
    }

    public class DashboardDirector_RevenueFluctuationByQuarterDTO : DataDTO
    {
        public long Month { get; set; }
        public decimal Revenue { get; set; }
    }

    public class DashboardDirector_RevenueFluctuationByYearDTO : DataDTO
    {
        public long Month { get; set; }
        public decimal Revenue { get; set; }
    }

    public class DashboardDirector_RevenueFluctuationFilterDTO : DashboardDirector_FilterDTO
    {

    }
}
