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
using DMS.Rpc.kpi_tracking.kpi_product_grouping_report;
namespace DMS.Tests.Rpc.kpi.kpi_tracking.kpi_product_grouping
{
    public partial class Kpi_ProductGroupingControllerFeature
    {
        private async Task KpiItemReport_KpiItemContentDTO_Result(string path)
        {
            var Expected = ReadFileFromJson<List<KpiProductGroupingReport_KpiProductGroupingReportDTO>>(path);

            Assert.AreEqual(Expected.Count, KpiProductGroupingReport_KpiProductGroupingReportDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].OrganizationId, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].OrganizationId);
                Assert.AreEqual(Expected[i].OrganizationName, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].OrganizationName);

                Assert.AreEqual(Expected[i].SaleEmployees.Count, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees.Count);

                for (int j = 0; j < Expected[i].SaleEmployees.Count; j++)
                {
                    Assert.AreEqual(Expected[i].SaleEmployees[j].UserName, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].UserName);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].DisplayName, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].DisplayName);

                    Assert.AreEqual(Expected[i].SaleEmployees[j].Contents.Count, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].Contents.Count);

                    for (int k = 0; k < Expected[i].SaleEmployees[j].Contents.Count; k++)
                    {
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Contents[k].ProductGroupingId, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].Contents[k].ProductGroupingId);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Contents[k].ProductGroupingCode, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].Contents[k].ProductGroupingCode);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Contents[k].ProductGroupingName, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].Contents[k].ProductGroupingName);
                       
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Contents[k].DirectRevenue, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].Contents[k].DirectRevenue);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Contents[k].DirectRevenuePlanned, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].Contents[k].DirectRevenuePlanned);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Contents[k].DirectRevenueRatio, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].Contents[k].DirectRevenueRatio);

                        Assert.AreEqual(Expected[i].SaleEmployees[j].Contents[k].DirectStore, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].Contents[k].DirectStore);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Contents[k].DirectStorePlanned, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].Contents[k].DirectStorePlanned);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Contents[k].DirectStoreRatio, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].Contents[k].DirectStoreRatio);

                        Assert.AreEqual(Expected[i].SaleEmployees[j].Contents[k].IndirectRevenue, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].Contents[k].IndirectRevenue);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Contents[k].IndirectRevenuePlanned, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].Contents[k].IndirectRevenuePlanned);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Contents[k].IndirectRevenueRatio, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].Contents[k].IndirectRevenueRatio);

                        Assert.AreEqual(Expected[i].SaleEmployees[j].Contents[k].IndirectStore, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].Contents[k].IndirectStore);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Contents[k].IndirectStorePlanned, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].Contents[k].IndirectStorePlanned);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Contents[k].IndirectStoreRatio, KpiProductGroupingReport_KpiProductGroupingReportDTOs[i].SaleEmployees[j].Contents[k].IndirectStoreRatio);
                    }
                }
            }
        }
    }
}
