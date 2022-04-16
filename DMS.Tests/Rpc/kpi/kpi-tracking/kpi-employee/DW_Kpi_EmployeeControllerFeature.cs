using DMS.Rpc.kpi_general;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DMS.Models;
using LightBDD.NUnit3;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using DMS.Common;
using System.Collections.Generic;
using DMS.DWModels;
using DMS.Rpc.kpi_tracking.kpi_general_employee_report;

namespace DMS.Tests.Rpc.kpi_tracking.kpi_employee
{
    [TestFixture]
    [FeatureDescription(@"")]
    [Label("Story-1")]
    public partial class Kpi_EmployeeControllerFeature : BaseTests
    {
        private const string DATA_DMS_PATH = "/files/DMS/bachlx/DMS_1121.xlsx";
        private const string DW_DMS_PATH = "/files/DMS/bachlx/DW_DMS_1121.xlsx";
        public const string INIT_DMS_PATH = "/files/DMS/DMS_Script_1121.txt";
        public const string INIT_DWDMS_PATH = "/files/DMS/DW_DMS_Script_1121.txt";

        KpiGeneralEmployeeReportController KpiGeneralEmployeeReportController;
        List<KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO> KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs;
        KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTOs;

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
            KpiGeneralEmployeeReportController = ServiceProvider.GetService<KpiGeneralEmployeeReportController>();
        }

        [Scenario]
        [Label("List report without order date from DW.DMS")]
        public async Task Get_DW_KpiEmployeeReport_NoOrderdate()
        {
            string json_path = "";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, KpiGeneralEmployeeReportRoute.List),
                _ => When_UserInputFilter_NoOrderDate_DW(),
                _ => Get_KpiGeneralEmployeeReport(),
                _ => Then_KpiEmployeeReport_SaleEmployee_Result(json_path)
                );
        }
        [Scenario]
        [Label("List report with order date from DW.DMS")]
        public async Task Get_DW_KpiEmployeeReport_Orderdate()
        {
            string json_path = "";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, KpiGeneralEmployeeReportRoute.List),
                _ => When_UserInputFilter_OrderDate_DW(),
                _ => Get_KpiGeneralEmployeeReport(),
                _ => Then_KpiEmployeeReport_SaleEmployee_Result(json_path)
                );
        }
    }
}
