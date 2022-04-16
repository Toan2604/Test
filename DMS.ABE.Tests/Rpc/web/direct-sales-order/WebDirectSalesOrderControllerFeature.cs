using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DMS.ABE.Models;
using LightBDD.NUnit3;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using DMS.ABE.Rpc.web.direct_sales_order;
using System.Collections.Generic;

namespace DMS.ABE.Tests.Rpc.web.direct_sales_order
{
    [TestFixture]
    [FeatureDescription(@"")]
    [Label("Story-1")]
    public partial class WebDirectSalesOrderControllerFeature : BaseTests
    {
        WebDirectSalesOrderController WebDirectSalesOrderController;
        WebDirectSalesOrder_DirectSalesOrderDTO WebDirectSalesOrder_DirectSalesOrderDTO;
        List<WebDirectSalesOrder_DirectSalesOrderDTO> WebDirectSalesOrder_DirectSalesOrderDTOs;
        WebDirectSalesOrder_DirectSalesOrderFilterDTO WebDirectSalesOrder_DirectSalesOrderFilterDTO;
        DataContext DataContext;
        string path = "/files/DMS/20210910/Database.xlsx";
        [SetUp]
        public async Task Setup()
        {
            Init();
            DataContext = ServiceProvider.GetService<DataContext>();
            WebDirectSalesOrderController = ServiceProvider.GetService<WebDirectSalesOrderController>();
        }

        [Scenario]
        [Label("Tạo mới đơn hàng thành công")]
        public async Task Create_Success()
        {
            string payload = "rpc/web/direct-sales-order/payload/create.json";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput(payload),
                _ => When_UserCreate(),
                _ => Then_Create_Success()
                ); ;
        }
        [Scenario]
        [Label("Tạo mới đơn hàng thất bại do thiếu các trường bắt buộc")]
        public async Task Create_MissingRequiredField_Failed()
        {
            string payload = "rpc/web/direct-sales-order/payload/create_missingfield_failed.json";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput(payload),
                _ => When_UserCreate(),
                _ => Then_CheckResultFailed(payload)
                ); ;
        }
        [Scenario]
        [Label("Sửa đơn hàng thành công")]
        public async Task Update_Success()
        {
            string payload = "rpc/web/direct-sales-order/payload/update.json";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput(payload),
                _ => When_UserUpdate(),
                _ => Then_CheckResult(payload)
                ); ;
        }
        [Scenario]
        [Label("Phê duyệt đơn hàng thành công")]
        public async Task Approve_Success()
        {
            string payload = "rpc/web/direct-sales-order/payload/store_pending.json";
            string response = "rpc/web/direct-sales-order/payload/approve_success.json";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput(payload),
                _ => When_UserApproval(),
                _ => Then_CheckResult(response)
                ); ;
        }
        [Scenario]
        [Label("Từ chối đơn hàng thành công")]
        public async Task Reject_Success()
        {
            string payload = "rpc/web/direct-sales-order/payload/store_pending.json";
            string response = "rpc/web/direct-sales-order/payload/reject_success.json";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput(payload),
                _ => When_UserReject(),
                _ => Then_CheckResult(response)
                ); ;
        }
        [Scenario]
        [Label("Tìm kiếm đơn hàng theo ngày đơn hàng thành công")]
        public async Task SearchByDate_Success()
        {
            string payload = "rpc/web/direct-sales-order/payload/list_order_by_date.json";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserFilterByDate(),
                _ => When_UserSearch(),
                _ => Then_Search_Success(payload)
                ); ;
        }
        [Scenario]
        [Label("Tìm kiếm đơn hàng theo tổng giá thành công")]
        public async Task SearchByTotalPrice_Success()
        {
            string payload = "rpc/web/direct-sales-order/payload/list_order_by_total.json";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserFilterByTotal(),
                _ => When_UserSearch(),
                _ => Then_Search_Success(payload)
                ); ;
        }
        [Scenario]
        [Label("Tìm kiếm đơn hàng theo nhiều điều kiện thành công")]
        public async Task SearchMultiFilter_Success()
        {
            string payload = "rpc/web/direct-sales-order/payload/create.json";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput(payload),
                _ => When_UserCreate(),
                _ => Then_Create_Success()
                ); ;
        }
        [Scenario]
        [Label("Trả về danh sách sản phẩm")]
        public async Task SearchItemNoFilter_Success()
        {
            string payload = "rpc/web/direct-sales-order/payload/create.json";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput(payload),
                _ => When_UserCreate(),
                _ => Then_Create_Success()
                ); ;
        }
        [Scenario]
        [Label("Tìm kiếm sản phẩm theo tên và loại")]
        public async Task SearchItemByNameAndCategory_Success()
        {
            string payload = "rpc/web/direct-sales-order/payload/create.json";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput(payload),
                _ => When_UserCreate(),
                _ => Then_Create_Success()
                ); ;
        }
        [Scenario]
        [Label("Tính giá bán")]
        public async Task CheckAmountAfterCreateOrder_Success()
        {
            string payload = "rpc/web/direct-sales-order/payload/create.json";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput(payload),
                _ => When_UserCreate(),
                _ => Then_Create_Success()
                ); ;
        }
    }
}
