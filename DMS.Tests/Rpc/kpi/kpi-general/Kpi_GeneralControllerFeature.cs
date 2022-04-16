using DMS.Models;
using DMS.Rpc.kpi_general;
using DMS.Rpc.kpi_tracking.kpi_general_period_report;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.NUnit3;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.kpi.kpi_general
{
    [TestFixture]
    [FeatureDescription(@"")]
    [Label("Story-1")]
    public partial class Kpi_GeneralControllerFeature : BaseTests
    {
        private const string DATA_DMS_PATH = "/files/DMS/bachlx/DMS_1121.xlsx";
        private const string DW_DMS_PATH = "/files/DMS/bachlx/DW_DMS_1121.xlsx";
        public const string INIT_DMS_PATH = "/files/DMS/DMS_Script_1121.txt";
        public const string INIT_DWDMS_PATH = "/files/DMS/DW_DMS_Script_1121.txt";

        
        KpiGeneralController KpiGeneralController;
        KpiGeneral_KpiGeneralDTO KpiGeneral_KpiGeneralDTO;
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
            KpiGeneralController = ServiceProvider.GetService<KpiGeneralController>();
        }
        [Scenario]
        [Label("Create")]
        public async Task Create_KpiGeneral()
        {
            string payload_path = "rpc/kpi/kpi-general/KPIGeneral_Payload.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, KpiGeneralRoute.Create),
                _ => When_UserInput(payload_path),
                _ => When_Create(),
                _ => Then_Success()
                );
        }

        [Scenario]
        [Label("Update")]
        public async Task Update_KpiGeneral()
        {
            string payload_path = "rpc/kpi/kpi-general/KPIGeneral_Payload.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, KpiGeneralRoute.Update),
                _ => When_UserInput(payload_path),
                _ => When_Update(),
                _ => Then_Success()
                );
        }

        [Scenario]
        [Label("Delete")]
        public async Task Delete_KpiGeneral()
        {
            string payload_path = "rpc/kpi/kpi-general/KPIGeneral_Payload.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, KpiGeneralRoute.Delete),
                _ => When_UserInput(payload_path),
                _ => When_Delete(),
                _ => Then_Success()
                );
        }

        [Scenario]
        [Label("Import")]
        public async Task Import_KpiGeneral()
        {
            string import_excel = "";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, KpiGeneralRoute.Import),
                _ => When_UserImportExcel(import_excel),
                _ => When_Import(),
                _ => Then_ImportSuccess(1)
                );
        }

    }
}
