using DMS.DWModels;
using DMS.Models;
using DMS.Rpc.reports.report_store.report_statistic_store_scouting;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.NUnit3;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_store.report_statistic_store_scouting
{
    [TestFixture]
    [FeatureDescription(@"Báo cáo thống kê Đại lý cắm cờ")]
    [Label("Story-1")]
    public partial class ReportStatisticStoreScoutingControllerFeature : BaseTests
    {
        private const string DATA_DMS_PATH = "/files/DMS/bachlx/DMS_1121.xlsx";
        private const string DW_DMS_PATH = "/files/DMS/bachlx/DW_DMS_1121.xlsx";
        public const string INIT_DMS_PATH = "/files/DMS/DMS_Script_1121.txt";
        public const string INIT_DWDMS_PATH = "/files/DMS/DW_DMS_Script_1121.txt";
        ReportStatisticStoreScoutingController ReportStatisticStoreScoutingController;
        List<ReportStatisticStoreScouting_ReportStatisticStoreScoutingDTO> ReportStatisticStoreScoutingDTOs;
        ReportStatisticStoreScouting_ReportStatisticStoreScoutingFilterDTO ReportStatisticStoreScoutingFilterDTO;

        [SetUp]
        public async Task Setup()
        {
            string DatabaseName = DATA_DMS_PATH.Split('/')[DATA_DMS_PATH.Split('/').Length-1].Split('.')[0];
            string DWName = DW_DMS_PATH.Split('/')[DATA_DMS_PATH.Split('/').Length - 1].Split('.')[0];

            InitDatabase(DatabaseName, INIT_DMS_PATH);
            InitDatabase(DWName, INIT_DWDMS_PATH);

            Init(DatabaseName, DWName);

            await LoadExcel(DATA_DMS_PATH, DatabaseName); 
            await LoadDWExcel(DW_DMS_PATH, DWName); 

            DataContext = ServiceProvider.GetService<DataContext>();
            DWContext = ServiceProvider.GetService<DWContext>();
            ReportStatisticStoreScoutingController = ServiceProvider.GetService<ReportStatisticStoreScoutingController>();
        }

        [Scenario]
        [Label("Báo cáo thống kê Đại lý cắm cờ")]
        public async Task Get_Report_StatisticStoreScouting_General()
        {
            string json_path = "/files/DMS/bachlx/ExpectedJson/report/report-store/report-statistic-store-scouting.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, ReportStatisticStoreScoutingRoute.List),
                _ => When_UserInput_Filter(),
                _ => Get_ReportStatisticStoreScouting(),
                _ => Then_ReportStatisticStoreScouting_Result(json_path)
                );
        }
    }
}
