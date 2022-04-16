using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.director
{
    public class DashboardDirector_IndirectSalesOrderFluctuationDTO : DataDTO
    {
        public List<DashboardDirector_IndirectSalesOrderFluctuationByMonthDTO> IndirectSalesOrderFluctuationByMonths { get; set; }
        public List<DashboardDirector_IndirectSalesOrderFluctuationByQuarterDTO> IndirectSalesOrderFluctuationByQuaters { get; set; }
        public List<DashboardDirector_IndirectSalesOrderFluctuationByYearDTO> IndirectSalesOrderFluctuationByYears { get; set; }
    }

    public class DashboardDirector_IndirectSalesOrderFluctuationByMonthDTO : DataDTO
    {
        public long Day { get; set; }
        public decimal IndirectSalesOrderCounter { get; set; }
    }

    public class DashboardDirector_IndirectSalesOrderFluctuationByQuarterDTO : DataDTO
    {
        public long Month { get; set; }
        public decimal IndirectSalesOrderCounter { get; set; }
    }

    public class DashboardDirector_IndirectSalesOrderFluctuationByYearDTO : DataDTO
    {
        public long Month { get; set; }
        public decimal IndirectSalesOrderCounter { get; set; }
    }

    public class DashboardDirector_IndirectSalesOrderFluctuationFilterDTO : DashboardDirector_FilterDTO
    {

    }
}
