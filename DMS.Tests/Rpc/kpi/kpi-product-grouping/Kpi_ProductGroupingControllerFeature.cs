using DMS.Models;
using DMS.Rpc.kpi_item;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LightBDD.NUnit3;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using DMS.Rpc.kpi_product_grouping;
using NUnit.Framework;

namespace DMS.Tests.Rpc.kpi.kpi_product_grouping
{
    [TestFixture]
    [FeatureDescription(@"")]
    [Label("Story-1")]
    public partial class Kpi_ProductGroupingControllerFeature :BaseTests
    {
        private const string DATA_DMS_PATH = "/files/DMS/bachlx/DMS_1121.xlsx";
        private const string DW_DMS_PATH = "/files/DMS/bachlx/DW_DMS_1121.xlsx";
        public const string INIT_DMS_PATH = "/files/DMS/DMS_Script_1121.txt";
        public const string INIT_DWDMS_PATH = "/files/DMS/DW_DMS_Script_1121.txt";

        KpiProductGroupingController KpiProductGroupingController;
        KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO;
        IFormFile FormFile;

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
            KpiProductGroupingController = ServiceProvider.GetService<KpiProductGroupingController>();
        }
        [Scenario]
        [Label("Create")]
        public async Task Create_KpiProductGrouping()
        {
            string payload_path = "rpc/kpi/kpi-product-grouping/KpiProductGrouping_Payload.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, KpiProductGroupingRoute.Create),
                _ => When_UserInput(payload_path),
                _ => When_Create(),
                _ => Then_Success()
                );
        }

        [Scenario]
        [Label("Update")]
        public async Task Update_KpiProductGrouping()
        {
            string payload_path = "rpc/kpi/kpi-product-grouping/KpiProductGrouping_Payload.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, KpiProductGroupingRoute.Update),
                _ => When_UserInput(payload_path),
                _ => When_Update(),
                _ => Then_Success()
                );
        }

        [Scenario]
        [Label("Delete")]
        public async Task Delete_KpiProductGrouping()
        {
            string payload_path = "rpc/kpi/kpi-product-grouping/KpiProductGrouping_Payload.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, KpiProductGroupingRoute.Delete),
                _ => When_UserInput(payload_path),
                _ => When_Delete(),
                _ => Then_Success()
                );
        }

        [Scenario]
        [Label("Import")]
        public async Task Import_KpiProductGrouping()
        {
            string import_excel = "link file excel";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, KpiProductGroupingRoute.Import),
                _ => When_UserImportExcel(import_excel),
                _ => When_Import(),
                _ => Then_ImportSuccess(1)
                );
        }
    }
}
