using DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_item;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_direct_sales_order_by_item
{
    public partial class ReportDirectSalesOrderByItemControllerFeature
    {
        public async Task When_UserInput_Filter()
        {
            ReportDirectSalesOrderByItemFilterDTO = new ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO
            {

            };
        }

        public async Task Get_ReportDirectSalesOrderByItem()
        {
            var Result = await ReportDirectSalesOrderByItemController.List(ReportDirectSalesOrderByItemFilterDTO);
            if (Result.Value != null) ReportDirectSalesOrderByItemDTOs = Result.Value;
            if (Result.Result != null) ReportDirectSalesOrderByItemDTOs =
                     (List<ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO>)((BadRequestObjectResult)Result.Result).Value;

        }
    }
}
