using DMS.DWModels;
using DMS.Models;
using DMS.Rpc.reports.report_store_checking.report_store_unchecked;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.NUnit3;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_store_checking.report_store_unchecked
{
    [TestFixture]
    [FeatureDescription(@"Báo cáo đại lý không ghé thăm")]
    [Label("Story-1")]
    public partial class ReportStoreUncheckedControllerFeature : BaseTests
    {
        private const string DATA_DMS_PATH = "/files/DMS/bachlx/DMS_1121.xlsx";
        private const string DW_DMS_PATH = "/files/DMS/bachlx/DW_DMS_1121.xlsx";
        public const string INIT_DMS_PATH = "/files/DMS/DMS_Script_1121.txt";
        public const string INIT_DWDMS_PATH = "/files/DMS/DW_DMS_Script_1121.txt";
        ReportStoreUncheckedController ReportStoreUncheckedController;
        List<ReportStoreUnchecked_ReportStoreUncheckedDTO> ReportStoreUnchecked_ReportStoreUncheckedDTOs;
        ReportStoreUnchecked_ReportStoreUncheckedFilterDTO ReportStoreUnchecked_ReportStoreUncheckedFilterDTO;

        [SetUp]
        public async Task Setup()
        {
            string DatabaseName = DATA_DMS_PATH.Split('/')[DATA_DMS_PATH.Split('/').Length - 1].Split('.')[0];
            string DWName = DW_DMS_PATH.Split('/')[DATA_DMS_PATH.Split('/').Length - 1].Split('.')[0];

            InitDatabase(DatabaseName, INIT_DMS_PATH);
            InitDatabase(DWName, INIT_DWDMS_PATH);

            Init(DatabaseName, DWName);

            await LoadExcel(DATA_DMS_PATH, DatabaseName);
            await LoadDWExcel(DW_DMS_PATH, DWName);

            DataContext = ServiceProvider.GetService<DataContext>();
            DWContext = ServiceProvider.GetService<DWContext>();
            ReportStoreUncheckedController = ServiceProvider.GetService<ReportStoreUncheckedController>();
        }

        [Scenario]
        [Label("Báo cáo thống kê Đại lý cắm cờ")]
        public async Task Get_Report_StatisticStoreScouting_General()
        {
            string json_path = "/files/DMS/bachlx/ExpectedJson/report/report-store/report-store-checking.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, ReportStoreUncheckedRoute.List),
                _ => When_UserInput_Filter(),
                _ => Get_ReportStoreUnchecked(),
                _ => Then_ReportStoreUnchecked_Result(json_path)
                );
        }
    }
}
