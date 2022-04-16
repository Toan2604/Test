using DMS.Rpc.warehouse_report;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Tests.Rpc.warehouse_report
{
    public partial class WarehouseReportControllerFeature
    {
        public async Task When_UserInput_Filter()
        {
            WarehouseReport_WarehouseReportFilterDTO = new WarehouseReport_WarehouseReportFilterDTO
            {
                Skip = 0,
                Take = 10,
            };
        }

        public async Task Get_WarehouseReport()
        {
            WarehouseReport_WarehouseReportDTOs = await WarehouseReportController.List(WarehouseReport_WarehouseReportFilterDTO);
        }
    }
}
