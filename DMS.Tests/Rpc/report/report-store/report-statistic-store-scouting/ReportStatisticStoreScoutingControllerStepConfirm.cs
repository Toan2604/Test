using DMS.Rpc.reports.report_store.report_statistic_store_scouting;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_store.report_statistic_store_scouting
{
    public partial class ReportStatisticStoreScoutingControllerFeature : BaseTests
    {
        public async Task Then_ReportStatisticStoreScouting_Result(string path)
        {
            var Expected = ReadFileFromJson<List<ReportStatisticStoreScouting_ReportStatisticStoreScoutingDTO>>(path);

            Assert.AreEqual(Expected.Count, ReportStatisticStoreScoutingDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].OfficalName, ReportStatisticStoreScoutingDTOs[i].OfficalName);
                Assert.AreEqual(Expected[i].StoreScoutingCounter, ReportStatisticStoreScoutingDTOs[i].StoreScoutingCounter);
                Assert.AreEqual(Expected[i].StoreOpennedCounter, ReportStatisticStoreScoutingDTOs[i].StoreOpennedCounter);
                Assert.AreEqual(Expected[i].StoreScoutingUnOpen, ReportStatisticStoreScoutingDTOs[i].StoreScoutingUnOpen);
                Assert.AreEqual(Expected[i].StoreCounter, ReportStatisticStoreScoutingDTOs[i].StoreCounter);
                Assert.AreEqual(Expected[i].eStoreCoutingOpennedRate, ReportStatisticStoreScoutingDTOs[i].eStoreCoutingOpennedRate);
                Assert.AreEqual(Expected[i].eStoreCoutingRate, ReportStatisticStoreScoutingDTOs[i].eStoreCoutingRate);
                Assert.AreEqual(Expected[i].eStoreRate, ReportStatisticStoreScoutingDTOs[i].eStoreRate);
            }
        }
    }
}
