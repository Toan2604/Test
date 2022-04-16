using DMS.Rpc.reports.report_store.report_store_general;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_store.report_store_general
{
    public partial class ReportStoreGeneralControllerFeature : BaseTests
    {
        public async Task Then_ReportStoreGeneral_Result(string path)
        {
            var Expected = ReadFileFromJson<List<ReportStoreGeneral_ReportStoreGeneralDTO>>(path);

            Assert.AreEqual(Expected.Count, ReportStoreGeneralDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].OrganizationId, ReportStoreGeneralDTOs[i].OrganizationId);
                Assert.AreEqual(Expected[i].OrganizationName, ReportStoreGeneralDTOs[i].OrganizationName);

                Assert.AreEqual(Expected[i].Stores.Count, ReportStoreGeneralDTOs[i].Stores.Count);

                for (int j = 0; j < Expected[i].Stores.Count; j++)
                {
                    Assert.AreEqual(Expected[i].Stores[j].Id, ReportStoreGeneralDTOs[i].Stores[j].Id);
                    Assert.AreEqual(Expected[i].Stores[j].Code, ReportStoreGeneralDTOs[i].Stores[j].Code);
                    //Assert.AreEqual(Expected[i].Stores[j].CodeDraft, ReportStoreGeneralDTOs[i].Stores[j].CodeDraft);
                    Assert.AreEqual(Expected[i].Stores[j].Name, ReportStoreGeneralDTOs[i].Stores[j].Name);
                    Assert.AreEqual(Expected[i].Stores[j].StoreStatusName, ReportStoreGeneralDTOs[i].Stores[j].StoreStatusName);
                    Assert.AreEqual(Expected[i].Stores[j].Address, ReportStoreGeneralDTOs[i].Stores[j].Address);
                    Assert.AreEqual(Expected[i].Stores[j].Phone, ReportStoreGeneralDTOs[i].Stores[j].Phone);
                    Assert.AreEqual(Expected[i].Stores[j].CheckingPlannedCounter, ReportStoreGeneralDTOs[i].Stores[j].CheckingPlannedCounter);
                    Assert.AreEqual(Expected[i].Stores[j].CheckingUnPlannedCounter, ReportStoreGeneralDTOs[i].Stores[j].CheckingUnPlannedCounter);
                    Assert.AreEqual(Expected[i].Stores[j].TotalCheckingTime, ReportStoreGeneralDTOs[i].Stores[j].TotalCheckingTime);
                    Assert.AreEqual(Expected[i].Stores[j].eFirstChecking, ReportStoreGeneralDTOs[i].Stores[j].eFirstChecking);
                    Assert.AreEqual(Expected[i].Stores[j].eLastChecking, ReportStoreGeneralDTOs[i].Stores[j].eLastChecking);
                    Assert.AreEqual(Expected[i].Stores[j].IndirectSalesOrderCounter, ReportStoreGeneralDTOs[i].Stores[j].IndirectSalesOrderCounter);
                    Assert.AreEqual(Expected[i].Stores[j].SKUCounter, ReportStoreGeneralDTOs[i].Stores[j].SKUCounter);
                    Assert.AreEqual(Expected[i].Stores[j].TotalRevenue, ReportStoreGeneralDTOs[i].Stores[j].TotalRevenue);
                }
            }
        }
    }
}
