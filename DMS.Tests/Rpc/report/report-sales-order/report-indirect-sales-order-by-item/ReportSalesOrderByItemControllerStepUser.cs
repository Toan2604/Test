using DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_item;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_indirect_sales_order_by_item
{
    public partial class ReportSalesOrderByItemControllerFeature
    {
        public async Task When_UserInput_Filter()
        {
            ReportSalesOrderByItemFilterDTO = new ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO
            {

            };
        }

        public async Task Get_ReportSalesOrderByItem()
        {
            var Result = await ReportSalesOrderByItemController.List(ReportSalesOrderByItemFilterDTO);
            if (Result.Value != null) ReportSalesOrderByItemDTOs = Result.Value;
            if (Result.Result != null) ReportSalesOrderByItemDTOs =
                     (List<ReportSalesOrderByItem_ReportSalesOrderByItemDTO>)((BadRequestObjectResult)Result.Result).Value;

        }
    }
}
