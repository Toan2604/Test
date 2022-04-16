using DMS.Rpc.kpi_general;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DMS.Models;
using LightBDD.NUnit3;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using DMS.Rpc.kpi_tracking.kpi_general_period_report;
using DMS.Common;
using System.Collections.Generic;
using DMS.DWModels;
using DMS.Rpc.kpi_tracking.kpi_item_report;
namespace DMS.Tests.Rpc.kpi.kpi_tracking.kpi_item
{
    [TestFixture]
    [FeatureDescription(@"")] 
    [Label("Story-1")]
    public partial class Kpi_ItemControllerFeature : BaseTests
    {
        private const string DATA_DMS_PATH = "/files/DMS/bachlx/DMS_1121.xlsx";
        private const string DW_DMS_PATH = "/files/DMS/bachlx/DW_DMS_1121.xlsx";
        public const string INIT_DMS_PATH = "/files/DMS/DMS_Script_1121.txt";
        public const string INIT_DWDMS_PATH = "/files/DMS/DW_DMS_Script_1121.txt";

        KpiItemReportController KpiItemReportController;
        List<KpiItemReport_KpiItemReportDTO> KpiItemReport_KpiItemReportDTOs;
        KpiItemReport_KpiItemReportFilterDTO KpiItemReport_KpiItemReportFilterDTO;
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
            KpiItemReportController = ServiceProvider.GetService<KpiItemReportController>();
        }
        [Scenario]
        [Label("List report without order date")]
        public async Task Get_KpiItemReport_NoOrderdate() 
        {
            string json_path = "";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, KpiItemReportRoute.List),
                _ => When_UserInputFilter_NoOrderDate(),
                _ => KpiItemReport_KpiItemContentDTO_Result(json_path)
                ) ;
        }
        [Scenario]
        [Label("List report with order date")]
        public async Task Get_KpiItemReport_Orderdate()
        {
            string json_path = "";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, KpiItemReportRoute.List),
                _ => When_UserInputFilter_OrderDate(),
                _ => KpiItemReport_KpiItemContentDTO_Result(json_path)
                );
        }
    }
}
