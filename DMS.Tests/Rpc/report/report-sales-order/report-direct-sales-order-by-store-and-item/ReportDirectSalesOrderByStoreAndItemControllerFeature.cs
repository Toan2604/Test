using DMS.Common;
using DMS.DWModels;
using DMS.Models;
using DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_store_and_item;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.NUnit3;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_direct_sales_order_by_store_and_item
{
    [TestFixture]
    [FeatureDescription(@"Báo cáo đơn hàng trực tiếp theo đại lý và sản phẩm")]
    [Label("Story-1")]
    public partial class ReportDirectSalesOrderByStoreAndItemControllerFeature : BaseTests
    {
        private const string DATA_DMS_PATH = "/files/DMS/bachlx/DMS_1121.xlsx";
        private const string DW_DMS_PATH = "/files/DMS/bachlx/DW_DMS_1121.xlsx";
        public const string INIT_DMS_PATH = "/files/DMS/DMS_Script_1121.txt";
        public const string INIT_DWDMS_PATH = "/files/DMS/DW_DMS_Script_1121.txt";
        ReportDirectSalesOrderByStoreAndItemController ReportDirectSalesOrderByStoreAndItemController;
        List<ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO> ReportDirectSalesOrderByStoreAndItemDTOs;
        ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO ReportDirectSalesOrderByStoreAndItemFilterDTO;

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
            ReportDirectSalesOrderByStoreAndItemController = ServiceProvider.GetService<ReportDirectSalesOrderByStoreAndItemController>();
        }

        [Scenario]
        [Label("Báo cáo đơn hàng trực tiếp theo đại lý và sản phẩm")]
        public async Task Get_Report_DirectSalesOrder_ByStoreAndItem()
        {
            string json_path = "/files/DMS/bachlx/ExpectedJson/report/report-sales-order/report-direct-sales-order-by-store-and-item.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, ReportDirectSalesOrderByStoreAndItemRoute.List),
                _ => When_UserInput_Filter(),
                _ => Get_ReportDirectSalesOrderByStoreAndItem(),
                _ => Then_ReportDirectSalesOrderByStoreAndItem_Result(json_path)
                );
        }
    }
}
