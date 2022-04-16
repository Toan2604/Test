using DMS.Rpc.reports.report_store.report_statistic_problem;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_store.report_statistic_problem
{
    public partial class ReportStatisticProblemControllerFeature : BaseTests
    {
        public async Task Then_ReportStatisticProblem_Result(string path)
        {
            var Expected = ReadFileFromJson<List<ReportStatisticProblem_ReportStatisticProblemDTO>>(path);

            Assert.AreEqual(Expected.Count, ReportStatisticProblemDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].OrganizationName, ReportStatisticProblemDTOs[i].OrganizationName);

                Assert.AreEqual(Expected[i].Stores.Count, ReportStatisticProblemDTOs[i].Stores.Count);

                for (int j = 0; j < Expected[i].Stores.Count; j++)
                {

                    Assert.AreEqual(Expected[i].Stores[j].Code, ReportStatisticProblemDTOs[i].Stores[j].Code);
                    Assert.AreEqual(Expected[i].Stores[j].Name, ReportStatisticProblemDTOs[i].Stores[j].Name);
                    Assert.AreEqual(Expected[i].Stores[j].StoreStatusId, ReportStatisticProblemDTOs[i].Stores[j].StoreStatusId);
                    //Assert.AreEqual(Expected[i].Stores[j].CodeDraft, ReportStatisticProblemDTOs[i].Stores[j].CodeDraft);
                    Assert.AreEqual(Expected[i].Stores[j].StoreTypeId, ReportStatisticProblemDTOs[i].Stores[j].StoreTypeId);
                    Assert.AreEqual(Expected[i].Stores[j].Address, ReportStatisticProblemDTOs[i].Stores[j].Address);

                    Assert.AreEqual(Expected[i].Stores[j].Contents.Count, ReportStatisticProblemDTOs[i].Stores[j].Contents.Count);

                    for (int k = 0; k < Expected[i].Stores[j].Contents.Count; k++)
                    {
                        Assert.AreEqual(Expected[i].Stores[j].Contents[k].ProblemTypeId, ReportStatisticProblemDTOs[i].Stores[j].Contents[k].ProblemTypeId);
                        Assert.AreEqual(Expected[i].Stores[j].Contents[k].ProblemTypeName, ReportStatisticProblemDTOs[i].Stores[j].Contents[k].ProblemTypeName);
                        Assert.AreEqual(Expected[i].Stores[j].Contents[k].WaitingCounter, ReportStatisticProblemDTOs[i].Stores[j].Contents[k].WaitingCounter);
                        Assert.AreEqual(Expected[i].Stores[j].Contents[k].ProcessCounter, ReportStatisticProblemDTOs[i].Stores[j].Contents[k].ProcessCounter);
                        Assert.AreEqual(Expected[i].Stores[j].Contents[k].CompletedCounter, ReportStatisticProblemDTOs[i].Stores[j].Contents[k].CompletedCounter);
                        Assert.AreEqual(Expected[i].Stores[j].Contents[k].Total, ReportStatisticProblemDTOs[i].Stores[j].Contents[k].Total);

                    }
                }
            }
        }
    }
}
