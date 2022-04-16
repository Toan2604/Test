using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.director
{
    public class DashboardDirector_DirectSalesOrderFluctuationDTO : DataDTO
    {
        public List<DashboardDirector_DirectSalesOrderFluctuationByMonthDTO> DirectSalesOrderFluctuationByMonths { get; set; }
        public List<DashboardDirector_DirectSalesOrderFluctuationByQuarterDTO> DirectSalesOrderFluctuationByQuaters { get; set; }
        public List<DashboardDirector_DirectSalesOrderFluctuationByYearDTO> DirectSalesOrderFluctuationByYears { get; set; }
    }

    public class DashboardDirector_DirectSalesOrderFluctuationByMonthDTO : DataDTO
    {
        public long Day { get; set; }
        public decimal DirectSalesOrderCounter { get; set; }
    }

    public class DashboardDirector_DirectSalesOrderFluctuationByQuarterDTO : DataDTO
    {
        public long Month { get; set; }
        public decimal DirectSalesOrderCounter { get; set; }
    }

    public class DashboardDirector_DirectSalesOrderFluctuationByYearDTO : DataDTO
    {
        public long Month { get; set; }
        public decimal DirectSalesOrderCounter { get; set; }
    }

    public class DashboardDirector_DirectSalesOrderFluctuationFilterDTO : DashboardDirector_FilterDTO
    {

    }
}
