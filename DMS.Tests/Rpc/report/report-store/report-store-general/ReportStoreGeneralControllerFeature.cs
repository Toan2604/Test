using DMS.Common;
using DMS.DWModels;
using DMS.Models;
using DMS.Rpc.reports.report_store.report_store_general;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.NUnit3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_store.report_store_general
{
    [TestFixture]
    [FeatureDescription(@"Báo cáo tổng hợp đại lý")]
    [Label("Story-1")]
    public partial class ReportStoreGeneralControllerFeature : BaseTests
    {
        private const string DATA_DMS_PATH = "/files/DMS/bachlx/DMS_1121.xlsx";
        private const string DW_DMS_PATH = "/files/DMS/bachlx/DW_DMS_1121.xlsx";
        public const string INIT_DMS_PATH = "/files/DMS/InitDMS1.txt";
        public const string INIT_DWDMS_PATH = "/files/DMS/InitDWDMS.txt";
        ReportStoreGeneralController ReportStoreGeneralController;
        List<ReportStoreGeneral_ReportStoreGeneralDTO> ReportStoreGeneralDTOs;
        ReportStoreGeneral_ReportStoreGeneralFilterDTO ReportStoreGeneralFilterDTO;

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
            ReportStoreGeneralController = ServiceProvider.GetService<ReportStoreGeneralController>();
        }

        [Scenario]
        [Label("Báo cáo tổng hợp đại lý")]
        public async Task Get_Report_Store_General()
        {
            string json_path = "/files/DMS/bachlx/ExpectedJson/report/report-store/report-store-general.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, ReportStoreGeneralRoute.List),
                _ => When_UserInput_Filter(),
                _ => Get_ReportStoreGeneral(),
                _ => Then_ReportStoreGeneral_Result(json_path)
                );
        }
    }
}
