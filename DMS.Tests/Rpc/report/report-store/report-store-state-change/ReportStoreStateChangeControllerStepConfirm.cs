using DMS.Rpc.reports.report_store.report_store_state_change;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_store.report_store_state_change
{
    public partial class ReportStoreStateChangeControllerFeature : BaseTests
    {
        public async Task Then_ReportStoreStateChange_Result(string path)
        {
            var Expected = ReadFileFromJson<List<ReportStoreStateChange_ReportStoreStateChangeDTO>>(path);

            Assert.AreEqual(Expected.Count, ReportStoreStateChangeDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].Total, ReportStoreStateChangeDTOs[i].Total);
                Assert.AreEqual(Expected[i].OrganizationName, ReportStoreStateChangeDTOs[i].OrganizationName);

                Assert.AreEqual(Expected[i].Details.Count, ReportStoreStateChangeDTOs[i].Details.Count);

                for (int j = 0; j < Expected[i].Details.Count; j++)
                {
                    Assert.AreEqual(Expected[i].Details[j].CreatedAt, ReportStoreStateChangeDTOs[i].Details[j].CreatedAt);
                    Assert.AreEqual(Expected[i].Details[j].StringCreatedAt, ReportStoreStateChangeDTOs[i].Details[j].StringCreatedAt);
                    Assert.AreEqual(Expected[i].Details[j].StoreCode, ReportStoreStateChangeDTOs[i].Details[j].StoreCode);
                    Assert.AreEqual(Expected[i].Details[j].StoreName, ReportStoreStateChangeDTOs[i].Details[j].StoreName);
                    Assert.AreEqual(Expected[i].Details[j].StoreAddress, ReportStoreStateChangeDTOs[i].Details[j].StoreAddress);
                    Assert.AreEqual(Expected[i].Details[j].StorePhoneNumber, ReportStoreStateChangeDTOs[i].Details[j].StorePhoneNumber);
                    Assert.AreEqual(Expected[i].Details[j].ApproverName, ReportStoreStateChangeDTOs[i].Details[j].ApproverName);
                    Assert.AreEqual(Expected[i].Details[j].PreviousCreatedAt, ReportStoreStateChangeDTOs[i].Details[j].PreviousCreatedAt);
                    Assert.AreEqual(Expected[i].Details[j].StringPreviousCreatedAt, ReportStoreStateChangeDTOs[i].Details[j].StringPreviousCreatedAt);
                    Assert.AreEqual(Expected[i].Details[j].StoreStatus, ReportStoreStateChangeDTOs[i].Details[j].StoreStatus);
                }
            }
        }
    }
}
