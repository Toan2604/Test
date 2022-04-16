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
using DMS.Rpc.kpi_tracking.kpi_general_employee_report;

namespace DMS.Tests.Rpc.kpi_tracking.kpi_employee
{
    public partial class Kpi_EmployeeControllerFeature
    {
        private async Task When_UserInputFilter_NoOrderDate_DW()
        {
            KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTOs = new KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO()
            {
                AppUserId = new IdFilter() { Equal = 2855 }, //Nhung Le
                KpiPeriodId = new IdFilter() { Equal = 103 }, // Thang 3
                KpiYearId = new IdFilter() { Equal = 2021 }, // 2021
            };
        }
        private async Task When_UserInputFilter_OrderDate_DW()
        {
            KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTOs = new KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO()
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
        private async Task Get_KpiGeneralEmployeeReport()
        {
            var Result = await KpiGeneralEmployeeReportController.List(KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTOs);
            if (Result == null) Assert.Fail();
            else KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs = Result.Value;
        }
    }
}
