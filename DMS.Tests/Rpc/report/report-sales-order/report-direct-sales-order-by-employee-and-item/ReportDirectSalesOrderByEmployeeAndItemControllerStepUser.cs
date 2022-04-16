using DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_employee_and_item;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_direct_sales_order_by_employee_and_item
{
    public partial class ReportDirectSalesOrderByEmployeeAndItemControllerFeature
    {
        public async Task When_UserInput_Filter()
        {
            ReportDirectSalesOrderByEmployeeAndItemFilterDTO = new ReportDirectSalesOrderByEmployeeAndItem_ReportDirectSalesOrderByEmployeeAndItemFilterDTO
            {

            };
        }

        public async Task Get_ReportDirectSalesOrderByEmployeeAndItem()
        {
            var Result = await ReportDirectSalesOrderByEmployeeAndItemController.List(ReportDirectSalesOrderByEmployeeAndItemFilterDTO);
            if(Result.Value != null) ReportDirectSalesOrderByEmployeeAndItemDTOs = Result.Value;
            if (Result.Result != null) ReportDirectSalesOrderByEmployeeAndItemDTOs =
                     (List<ReportDirectSalesOrderByEmployeeAndItem_ReportDirectSalesOrderByEmployeeAndItemDTO>)((BadRequestObjectResult)Result.Result).Value;

        }
    }
}
