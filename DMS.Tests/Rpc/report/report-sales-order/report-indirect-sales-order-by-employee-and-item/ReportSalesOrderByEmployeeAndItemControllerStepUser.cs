using DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_employee_and_item;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_indirect_sales_order_by_employee_and_item
{
    public partial class ReportSalesOrderByEmployeeAndItemControllerFeature
    {
        public async Task When_UserInput_Filter()
        {
            ReportSalesOrderByEmployeeAndItemFilterDTO = new ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO
            {

            };
        }

        public async Task Get_ReportSalesOrderByEmployeeAndItem()
        {
            var Result = await ReportSalesOrderByEmployeeAndItemController.List(ReportSalesOrderByEmployeeAndItemFilterDTO);
            if (Result.Value != null) ReportSalesOrderByEmployeeAndItemDTOs = Result.Value;
            if (Result.Result != null) ReportSalesOrderByEmployeeAndItemDTOs =
                     (List<ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO>)((BadRequestObjectResult)Result.Result).Value;

        }
    }
}
