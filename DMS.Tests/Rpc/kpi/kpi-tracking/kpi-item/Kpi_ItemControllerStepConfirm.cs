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
using DMS.Rpc.kpi_tracking.kpi_item_report;
namespace DMS.Tests.Rpc.kpi.kpi_tracking.kpi_item
{
    public partial class Kpi_ItemControllerFeature
    {
        private async Task KpiItemReport_KpiItemContentDTO_Result(string path)
        {
            var Expected = ReadFileFromJson<List<KpiItemReport_KpiItemReportDTO>>(path);

            Assert.AreEqual(Expected.Count, KpiItemReport_KpiItemReportDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].SaleEmployeeId, KpiItemReport_KpiItemReportDTOs[i].SaleEmployeeId);
                Assert.AreEqual(Expected[i].Username, KpiItemReport_KpiItemReportDTOs[i].Username);
                Assert.AreEqual(Expected[i].DisplayName, KpiItemReport_KpiItemReportDTOs[i].DisplayName);

                Assert.AreEqual(Expected[i].ItemContents.Count, Expected[i].ItemContents.Count);

                for (int j = 0; j < Expected[i].ItemContents.Count; j++)
                {
                    Assert.AreEqual(Expected[i].ItemContents[j].ItemId, Expected[i].ItemContents[j].ItemId);
                    Assert.AreEqual(Expected[i].ItemContents[j].ItemCode, Expected[i].ItemContents[j].ItemCode);
                    Assert.AreEqual(Expected[i].ItemContents[j].ItemName, Expected[i].ItemContents[j].ItemName);

                    Assert.AreEqual(Expected[i].ItemContents[j].DirectRevenue, Expected[i].ItemContents[j].DirectRevenue);
                    Assert.AreEqual(Expected[i].ItemContents[j].DirectRevenuePlanned, Expected[i].ItemContents[j].DirectRevenuePlanned);
                    Assert.AreEqual(Expected[i].ItemContents[j].DirectRevenueRatio, Expected[i].ItemContents[j].DirectRevenueRatio);

                    Assert.AreEqual(Expected[i].ItemContents[j].DirectStore, Expected[i].ItemContents[j].DirectStore);
                    Assert.AreEqual(Expected[i].ItemContents[j].DirectStorePlanned, Expected[i].ItemContents[j].DirectStorePlanned);
                    Assert.AreEqual(Expected[i].ItemContents[j].DirectStoreRatio, Expected[i].ItemContents[j].DirectStoreRatio);

                    Assert.AreEqual(Expected[i].ItemContents[j].IndirectRevenue, Expected[i].ItemContents[j].IndirectRevenue);
                    Assert.AreEqual(Expected[i].ItemContents[j].IndirectRevenuePlanned, Expected[i].ItemContents[j].IndirectRevenuePlanned);
                    Assert.AreEqual(Expected[i].ItemContents[j].IndirectRevenueRatio, Expected[i].ItemContents[j].IndirectRevenueRatio);

                    Assert.AreEqual(Expected[i].ItemContents[j].IndirectStore, Expected[i].ItemContents[j].IndirectStore);
                    Assert.AreEqual(Expected[i].ItemContents[j].IndirectStorePlanned, Expected[i].ItemContents[j].IndirectStorePlanned);
                    Assert.AreEqual(Expected[i].ItemContents[j].IndirectStoreRatio, Expected[i].ItemContents[j].IndirectStoreRatio);
                }

            }
        }
    }
}
