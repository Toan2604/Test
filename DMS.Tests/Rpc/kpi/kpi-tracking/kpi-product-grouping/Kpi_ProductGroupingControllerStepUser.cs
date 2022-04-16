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
using DMS.Rpc.kpi_tracking.kpi_product_grouping_report;
namespace DMS.Tests.Rpc.kpi.kpi_tracking.kpi_product_grouping
{
    public partial class Kpi_ProductGroupingControllerFeature
    {
        private async Task When_UserInputFilter_NoOrderDate()
        {
            KpiProductGroupingReport_KpiProductGroupingReportFilterDTO = new KpiProductGroupingReport_KpiProductGroupingReportFilterDTO()
            {
                AppUserId = new IdFilter() { Equal = 2855 }, // Nhung Le
                KpiPeriodId = new IdFilter() { Equal = 105 }, // Thang 5
                KpiYearId = new IdFilter() { Equal = 2021 }, // 2021
                KpiProductGroupingTypeId = new IdFilter() { Equal = 2}, // Kpi nhóm sản phẩm trọng tâm
                ProductGroupingId = new IdFilter() { Equal = 20585}, // Dép
            };
        }
        private async Task When_UserInputFilter_OrderDate()
        {
            KpiProductGroupingReport_KpiProductGroupingReportFilterDTO = new KpiProductGroupingReport_KpiProductGroupingReportFilterDTO()
            {
                AppUserId = new IdFilter() { Equal = 2855 }, // Nhung Le
                KpiPeriodId = new IdFilter() { Equal = 105 }, // Thang 5
                KpiYearId = new IdFilter() { Equal = 2021 }, // 2021
                KpiProductGroupingTypeId = new IdFilter() { Equal = 2 }, // Kpi nhóm sản phẩm trọng tâm
                ProductGroupingId = new IdFilter() { Equal = 20585 }, // Dép
                OrderDate = new DateFilter()
                {
                    GreaterEqual = new DateTime(2021, 3, 1),
                    LessEqual = new DateTime(2021, 3, 31),
                }
            };
        }

        private async Task Get_KpiProductGroupingReport()
        {
            var Result = await KpiProductGroupingReportController.List(KpiProductGroupingReport_KpiProductGroupingReportFilterDTO);
            if (Result == null) Assert.Fail();
            else KpiProductGroupingReport_KpiProductGroupingReportDTOs = Result.Value;
        }
    }
}
