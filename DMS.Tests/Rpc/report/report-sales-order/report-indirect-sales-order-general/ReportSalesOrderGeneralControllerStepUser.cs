using DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_general;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_indirect_sales_order_general
{
    public partial class ReportSalesOrderGeneralControllerFeature
    {
        public async Task When_UserInput_Filter()
        {
            ReportSalesOrderGeneralFilterDTO = new ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO
            {

            };
        }

        public async Task Get_ReportSalesOrderGeneral()
        {
            var Result = await ReportSalesOrderGeneralController.List(ReportSalesOrderGeneralFilterDTO);
            if (Result.Value != null) ReportSalesOrderGeneralDTOs = Result.Value;
            if (Result.Result != null) ReportSalesOrderGeneralDTOs =
                     (List<ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO>)((BadRequestObjectResult)Result.Result).Value;

        }
    }
}
