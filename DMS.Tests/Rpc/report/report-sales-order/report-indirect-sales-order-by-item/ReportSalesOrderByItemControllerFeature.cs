using DMS.Common;
using DMS.DWModels;
using DMS.Models;
using DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_item;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.NUnit3;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_indirect_sales_order_by_item
{
    [TestFixture]
    [FeatureDescription(@"Báo cáo đơn hàng gián tiếp theo sản phẩm")]
    [Label("Story-1")]
    public partial class ReportSalesOrderByItemControllerFeature : BaseTests
    {
        private const string DATA_DMS_PATH = "/files/DMS/bachlx/DMS_1121.xlsx";
        private const string DW_DMS_PATH = "/files/DMS/bachlx/DW_DMS_1121.xlsx";
        public const string INIT_DMS_PATH = "/files/DMS/DMS_Script_1121.txt";
        public const string INIT_DWDMS_PATH = "/files/DMS/DW_DMS_Script_1121.txt";
        ReportSalesOrderByItemController ReportSalesOrderByItemController;
        List<ReportSalesOrderByItem_ReportSalesOrderByItemDTO> ReportSalesOrderByItemDTOs;
        ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO ReportSalesOrderByItemFilterDTO;

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
            ReportSalesOrderByItemController = ServiceProvider.GetService<ReportSalesOrderByItemController>();
        }

        [Scenario]
        [Label("Báo cáo đơn hàng gián tiếp theo sản phẩm")]
        public async Task Get_Report_SalesOrder_ByItem()
        {
            string json_path = "/files/DMS/bachlx/ExpectedJson/report/report-sales-order/report-indirect-sales-order-by-item.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, ReportSalesOrderByItemRoute.List),
                _ => When_UserInput_Filter(),
                _ => Get_ReportSalesOrderByItem(),
                _ => Then_ReportSalesOrderByItem_Result(json_path)
                );
        }
    }
}
