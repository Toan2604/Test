using DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_store_and_item;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_indirect_sales_order_by_store_and_item
{
    public partial class ReportSalesOrderByStoreAndItemControllerFeature
    {
        public async Task When_UserInput_Filter()
        {
            ReportSalesOrderByStoreAndItemFilterDTO = new ReportSalesOrderByStoreAndItem_ReportSalesOrderByStoreAndItemFilterDTO
            {

            };
        }

        public async Task Get_ReportSalesOrderByStoreAndItem()
        {
            var Result = await ReportSalesOrderByStoreAndItemController.List(ReportSalesOrderByStoreAndItemFilterDTO);
            if (Result.Value != null) ReportSalesOrderByStoreAndItemDTOs = Result.Value;
            if (Result.Result != null) ReportSalesOrderByStoreAndItemDTOs =
                     (List<ReportSalesOrderByStoreAndItem_ReportSalesOrderByStoreAndItemDTO>)((BadRequestObjectResult)Result.Result).Value;

        }
    }
}
