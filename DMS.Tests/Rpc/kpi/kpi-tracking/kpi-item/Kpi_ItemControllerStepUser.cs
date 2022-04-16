using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using DMS.Enums;
using DMS.Rpc.kpi_general;
using DMS.Rpc.kpi_tracking.kpi_general_period_report;
using TrueSight.Common;
using DMS.Rpc.kpi_tracking.kpi_item_report;
namespace DMS.Tests.Rpc.kpi.kpi_tracking.kpi_item
{
    public partial class Kpi_ItemControllerFeature
    {
        private async Task When_UserInputFilter_NoOrderDate()
        {
            KpiItemReport_KpiItemReportFilterDTO = new KpiItemReport_KpiItemReportFilterDTO()
            {
                KpiItemTypeId = new IdFilter() { Equal = 1}, // Sản phẩm mới
                AppUserId = new IdFilter() { Equal = 2855 }, // Nhung Le
                KpiPeriodId = new IdFilter() { Equal = 103 }, // Thang 3
                KpiYearId = new IdFilter() { Equal = 2021 }, // 2021
            };
        }
        private async Task When_UserInputFilter_OrderDate()
        {
            KpiItemReport_KpiItemReportFilterDTO = new KpiItemReport_KpiItemReportFilterDTO()
            {
                KpiItemTypeId = new IdFilter() { Equal = 1 }, // Sản phẩm mới
                AppUserId = new IdFilter() { Equal = 2855 }, // Nhung Le
                KpiPeriodId = new IdFilter() { Equal = 103 }, // Thang 3
                KpiYearId = new IdFilter() { Equal = 2021 }, // 2021
                OrderDate = new DateFilter()
                {
                    GreaterEqual = new DateTime(2021, 3, 1),
                    LessEqual = new DateTime(2021, 3, 31),
                }
            };
        }
        private async Task Get_KpiItemReport()
        {
            var Result = await KpiItemReportController.List(KpiItemReport_KpiItemReportFilterDTO);
            if (Result == null) Assert.Fail();
            else KpiItemReport_KpiItemReportDTOs = Result.Value;
        }
    }
}
