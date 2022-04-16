using DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_store_and_item;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_direct_sales_order_by_store_and_item
{
    public partial class ReportDirectSalesOrderByStoreAndItemControllerFeature
    {
        public async Task When_UserInput_Filter()
        {
            ReportDirectSalesOrderByStoreAndItemFilterDTO = new ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO
            {

            };
        }

        public async Task Get_ReportDirectSalesOrderByStoreAndItem()
        {
            var Result = await ReportDirectSalesOrderByStoreAndItemController.List(ReportDirectSalesOrderByStoreAndItemFilterDTO);
            if (Result.Value != null) ReportDirectSalesOrderByStoreAndItemDTOs = Result.Value;
            if (Result.Result != null) ReportDirectSalesOrderByStoreAndItemDTOs =
                     (List<ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO>)((BadRequestObjectResult)Result.Result).Value;

        }
    }
}
