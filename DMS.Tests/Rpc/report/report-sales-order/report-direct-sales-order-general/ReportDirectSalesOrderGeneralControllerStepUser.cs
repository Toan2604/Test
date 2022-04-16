using DMS.Rpc.reports.report_sales_order.report_direct_sales_order_general;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_direct_sales_order_general
{
    public partial class ReportDirectSalesOrderGeneralControllerFeature
    {
        public async Task When_UserInput_Filter()
        {
            ReportDirectSalesOrderGeneralFilterDTO = new ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO
            {

            };
        }

        public async Task Get_ReportDirectSalesOrderGeneral()
        {
            var Result = await ReportDirectSalesOrderGeneralController.List(ReportDirectSalesOrderGeneralFilterDTO);
            if (Result.Value != null) ReportDirectSalesOrderGeneralDTOs = Result.Value;
            if (Result.Result != null) ReportDirectSalesOrderGeneralDTOs =
                     (List<ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO>)((BadRequestObjectResult)Result.Result).Value;

        }
    }
}
