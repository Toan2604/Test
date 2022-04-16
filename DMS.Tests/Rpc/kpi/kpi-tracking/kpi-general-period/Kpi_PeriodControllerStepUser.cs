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
namespace DMS.Tests.Rpc.kpi_tracking.kpi_period
{
    public partial class Kpi_PeriodControllerFeature
    {
        private async Task When_UserInputFilter_NoOrderDate()
        {
            KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO = new KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO()
            {
                AppUserId = new IdFilter() { Equal = 2855 }, //Nhung Le
                KpiPeriodId = new IdFilter() { Equal = 103 }, // Thang 3
                KpiYearId = new IdFilter() { Equal = 2021 }, // 2021
            };
        }
        private async Task When_UserInputFilter_OrderDate()
        {
            KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO = new KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO()
            {
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
        private async Task Get_KpiGeneralPeriodReport()
        {
            var Result = await KpiGeneralPeriodReportController.List(KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO);
            if (Result == null) Assert.Fail();
            else KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs = Result.Value;
        }
    }
}
