using DMS.Common;
using DMS.DWModels;
using DMS.Models;
using DMS.Rpc.warehouse_report;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.NUnit3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.warehouse_report
{
    [TestFixture]
    [FeatureDescription(@"Báo cáo tồn kho có thể bán")]
    [Label("Story-1")]
    public partial class WarehouseReportControllerFeature : BaseTests
    {
        private const string DATA_DMS_PATH = "/files/DMS/bachlx/DMS_1121.xlsx";
        private const string DW_DMS_PATH = "/files/DMS/bachlx/DW_DMS_1121.xlsx";
        public const string INIT_DMS_PATH = "/files/DMS/DMS_Script_1121.txt";
        public const string INIT_DWDMS_PATH = "/files/DMS/DW_DMS_Script_1121.txt";

        WarehouseReportController WarehouseReportController;
        List<WarehouseReport_WarehouseReportDTO> WarehouseReport_WarehouseReportDTOs;
        WarehouseReport_WarehouseReportFilterDTO WarehouseReport_WarehouseReportFilterDTO;

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
            WarehouseReportController = ServiceProvider.GetService<WarehouseReportController>();
        }

        [Scenario]
        [Label("Báo cáo tồn kho có thể bán")]
        public async Task Get_Report_StatisticStoreScouting_General()
        {
            string json_path = "/files/DMS/bachlx/ExpectedJson/....json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, WarehouseReportRoute.List),
                _ => When_UserInput_Filter(),
                _ => Get_WarehouseReport(),
                _ => Then_WarehouseReport_Result(json_path)
                );
        }
    }
}
