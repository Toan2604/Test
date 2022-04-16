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
using DMS.Rpc.kpi_tracking.kpi_product_grouping_report;
namespace DMS.Tests.Rpc.kpi.kpi_tracking.kpi_product_grouping
{
    [TestFixture]
    [FeatureDescription(@"")] 
    [Label("Story-1")]
    public partial class Kpi_ProductGroupingControllerFeature : BaseTests
    {
        private const string DATA_DMS_PATH = "/files/DMS/bachlx/DMS_1121.xlsx";
        private const string DW_DMS_PATH = "/files/DMS/bachlx/DW_DMS_1121.xlsx";
        public const string INIT_DMS_PATH = "/files/DMS/DMS_Script_1121.txt";
        public const string INIT_DWDMS_PATH = "/files/DMS/DW_DMS_Script_1121.txt";

        KpiProductGroupingReportController KpiProductGroupingReportController;
        List<KpiProductGroupingReport_KpiProductGroupingReportDTO> KpiProductGroupingReport_KpiProductGroupingReportDTOs;
        KpiProductGroupingReport_KpiProductGroupingReportFilterDTO KpiProductGroupingReport_KpiProductGroupingReportFilterDTO;
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
            KpiProductGroupingReportController = ServiceProvider.GetService<KpiProductGroupingReportController>();
        }
        [Scenario]
        [Label("List report without order date")]
        public async Task Get_KpiProductGroupingReport_NoOrderdate() 
        {
            string json_path = "";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, KpiProductGroupingReportRoute.List),
                _ => When_UserInputFilter_NoOrderDate(),
                _ => Get_KpiProductGroupingReport(),
                _ => KpiItemReport_KpiItemContentDTO_Result(json_path)
                ) ;
        }
        [Scenario]
        [Label("List report with order date")]
        public async Task Get_KpiProductGroupingReport_Orderdate()
        {
            string json_path = "";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, "rpc/dms/kpi-product-grouping-report/list"),
                _ => When_UserInputFilter_OrderDate(),
                _ => Get_KpiProductGroupingReport(),
                _ => KpiItemReport_KpiItemContentDTO_Result(json_path)
                );
        }
    }
}
