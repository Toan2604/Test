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
namespace DMS.Tests.Rpc.kpi_tracking.kpi_period
{
    public partial class Kpi_PeriodControllerFeature
    {
        private async Task Then_KpiGeneralPeriodReport_Result(string path)
        {
            var Expected = ReadFileFromJson<List<KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO>>(path);

            Assert.AreEqual(Expected.Count, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].OrganizationId, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].OrganizationId);
                Assert.AreEqual(Expected[i].OrganizationName, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].OrganizationName);

                Assert.AreEqual(Expected[i].SaleEmployees.Count, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees.Count);

                for (int j = 0; j < Expected[i].SaleEmployees.Count; j++)
                {
                    Assert.AreEqual(Expected[i].SaleEmployees[j].Username, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].Username);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].DisplayName, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].DisplayName);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].OrganizationName, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].OrganizationName);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].SaleEmployeeId, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].SaleEmployeeId);
                    
                    Assert.AreEqual(Expected[i].SaleEmployees[j].TotalDirectOrders, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].TotalDirectOrders);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].TotalDirectOrdersPlanned, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].TotalDirectOrdersPlanned);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].TotalDirectOrdersRatio, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].TotalDirectOrdersRatio);

                    Assert.AreEqual(Expected[i].SaleEmployees[j].TotalDirectSalesAmount, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].TotalDirectSalesAmount);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].TotalDirectSalesAmountPlanned, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].TotalDirectSalesAmountPlanned);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].TotalDirectSalesAmountRatio, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].TotalDirectSalesAmountRatio);

                    Assert.AreEqual(Expected[i].SaleEmployees[j].NewStoreC2Created, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].NewStoreC2Created);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].NewStoreC2CreatedPlanned, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].NewStoreC2CreatedPlanned);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].NewStoreC2CreatedRatio, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].NewStoreC2CreatedRatio);

                    Assert.AreEqual(Expected[i].SaleEmployees[j].NewStoreCreated, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].NewStoreCreated);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].NewStoreCreatedPlanned, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].NewStoreCreatedPlanned);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].NewStoreCreatedRatio, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].NewStoreCreatedRatio);

                    Assert.AreEqual(Expected[i].SaleEmployees[j].NumberOfStoreVisits, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].NumberOfStoreVisits);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].NumberOfStoreVisitsPlanned, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].NumberOfStoreVisitsPlanned);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].NumberOfStoreVisitsRatio, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].NumberOfStoreVisitsRatio);

                    Assert.AreEqual(Expected[i].SaleEmployees[j].RevenueC2, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].RevenueC2);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].RevenueC2Planned, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].RevenueC2Planned);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].RevenueC2Ratio, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].RevenueC2Ratio);

                    Assert.AreEqual(Expected[i].SaleEmployees[j].RevenueC2SL, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].RevenueC2SL);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].RevenueC2SLPlanned, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].RevenueC2SLPlanned);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].RevenueC2SLRatio, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].RevenueC2SLRatio);

                    Assert.AreEqual(Expected[i].SaleEmployees[j].RevenueC2TD, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].RevenueC2TD);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].RevenueC2TDPlanned, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].RevenueC2TDPlanned);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].RevenueC2TDRatio, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].RevenueC2TDRatio);

                    Assert.AreEqual(Expected[i].SaleEmployees[j].StoresVisited, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].StoresVisited);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].StoresVisitedPlanned, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].StoresVisitedPlanned);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].StoresVisitedRatio, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].StoresVisitedRatio);

                    Assert.AreEqual(Expected[i].SaleEmployees[j].TotalImage, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].TotalImage);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].TotalImagePlanned, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].TotalImagePlanned);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].TotalImageRatio, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].TotalImageRatio);

                    Assert.AreEqual(Expected[i].SaleEmployees[j].TotalIndirectSalesAmount, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].TotalIndirectSalesAmount);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].TotalIndirectSalesAmountPlanned, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].TotalIndirectSalesAmountPlanned);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].TotalIndirectSalesAmountRatio, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].TotalIndirectSalesAmountRatio);

                    Assert.AreEqual(Expected[i].SaleEmployees[j].TotalProblem, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].TotalProblem);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].TotalProblemPlanned, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].TotalProblemPlanned);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].TotalProblemRatio, KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs[i].SaleEmployees[j].TotalProblemRatio);
                }
            }
        }
    }
}
