using DMS.Rpc.kpi_tracking.kpi_general_employee_report;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.kpi_tracking.kpi_employee
{
    public partial class Kpi_EmployeeControllerFeature
    {
        public async Task Then_KpiEmployeeReport_SaleEmployee_Result(string path)
        {
            var Expected = ReadFileFromJson<List<KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO>>(path);

            Assert.AreEqual(Expected.Count, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].TotalDirectOrders, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].TotalDirectOrders);
                Assert.AreEqual(Expected[i].TotalDirectOrdersPlanned, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].TotalDirectOrdersPlanned);
                Assert.AreEqual(Expected[i].TotalDirectOrdersRatio, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].TotalDirectOrdersRatio);

                Assert.AreEqual(Expected[i].TotalDirectSalesAmount, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].TotalDirectSalesAmount);
                Assert.AreEqual(Expected[i].TotalDirectSalesAmountPlanned, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].TotalDirectSalesAmountPlanned);
                Assert.AreEqual(Expected[i].TotalDirectSalesAmountRatio, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].TotalDirectSalesAmountRatio);

                Assert.AreEqual(Expected[i].NewStoreC2Created, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].NewStoreC2Created);
                Assert.AreEqual(Expected[i].NewStoreC2CreatedPlanned, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].NewStoreC2CreatedPlanned);
                Assert.AreEqual(Expected[i].NewStoreC2CreatedRatio, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].NewStoreC2CreatedRatio);

                Assert.AreEqual(Expected[i].NewStoreCreated, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].NewStoreCreated);
                Assert.AreEqual(Expected[i].NewStoreCreatedPlanned, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].NewStoreCreatedPlanned);
                Assert.AreEqual(Expected[i].NewStoreCreatedRatio, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].NewStoreCreatedRatio);

                Assert.AreEqual(Expected[i].NumberOfStoreVisits, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].NumberOfStoreVisits);
                Assert.AreEqual(Expected[i].NumberOfStoreVisitsPlanned, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].NumberOfStoreVisitsPlanned);
                Assert.AreEqual(Expected[i].NumberOfStoreVisitsRatio, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].NumberOfStoreVisitsRatio);

                Assert.AreEqual(Expected[i].RevenueC2, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].RevenueC2);
                Assert.AreEqual(Expected[i].RevenueC2Planned, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].RevenueC2Planned);
                Assert.AreEqual(Expected[i].RevenueC2Ratio, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].RevenueC2Ratio);

                Assert.AreEqual(Expected[i].RevenueC2SL, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].RevenueC2SL);
                Assert.AreEqual(Expected[i].RevenueC2SLPlanned, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].RevenueC2SLPlanned);
                Assert.AreEqual(Expected[i].RevenueC2SLRatio, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].RevenueC2SLRatio);

                Assert.AreEqual(Expected[i].RevenueC2TD, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].RevenueC2TD);
                Assert.AreEqual(Expected[i].RevenueC2TDPlanned, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].RevenueC2TDPlanned);
                Assert.AreEqual(Expected[i].RevenueC2TDRatio, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].RevenueC2TDRatio);

                Assert.AreEqual(Expected[i].StoresVisited, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].StoresVisited);
                Assert.AreEqual(Expected[i].StoresVisitedPlanned, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].StoresVisitedPlanned);
                Assert.AreEqual(Expected[i].StoresVisitedRatio, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].StoresVisitedRatio);

                Assert.AreEqual(Expected[i].TotalImage, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].TotalImage);
                Assert.AreEqual(Expected[i].TotalImagePlanned, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].TotalImagePlanned);
                Assert.AreEqual(Expected[i].TotalImageRatio, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].TotalImageRatio);

                Assert.AreEqual(Expected[i].TotalIndirectSalesAmount, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].TotalIndirectSalesAmount);
                Assert.AreEqual(Expected[i].TotalIndirectSalesAmountPlanned, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].TotalIndirectSalesAmountPlanned);
                Assert.AreEqual(Expected[i].TotalIndirectSalesAmountRatio, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].TotalIndirectSalesAmountRatio);

                Assert.AreEqual(Expected[i].TotalProblem, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].TotalProblem);
                Assert.AreEqual(Expected[i].TotalProblemPlanned, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].TotalProblemPlanned);
                Assert.AreEqual(Expected[i].TotalProblemRatio, KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs[i].TotalProblemRatio);
            }
        }
    }
}
