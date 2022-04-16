using DMS.Models;
using DMS.Rpc.direct_sales_order;
using DMS.Rpc.kpi_general;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using LightBDD.NUnit3;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using DMS.Rpc.store_balance;
using DMS.Rpc.warehouse;
using DMS.Rpc.store;

namespace DMS.Tests.Rpc.direct_sales_order
{
    public partial class DirectSalesOrderControllerFeature : BaseTests
    {
        private const string DATA_DMS_PATH = "/files/DMS/bachlx/DMS_1121.xlsx";
        private const string DW_DMS_PATH = "/files/DMS/bachlx/DW_DMS_1121.xlsx";
        public const string INIT_DMS_PATH = "/files/DMS/DMS_Script_1121.txt";
        public const string INIT_DWDMS_PATH = "/files/DMS/DW_DMS_Script_1121.txt";

        DirectSalesOrderController DirectSalesOrderController;
        DirectSalesOrder_DirectSalesOrderDTO DirectSalesOrder_DirectSalesOrderDTO;
        List<DirectSalesOrder_DirectSalesOrderDTO> DirectSalesOrder_DirectSalesOrderDTOs;
        DirectSalesOrder_DirectSalesOrderFilterDTO DirectSalesOrder_DirectSalesOrderFilterDTO;
        Store_StoreDTO Store_StoreDTO;
        StoreBalance_StoreBalanceDTO StoreBalance_StoreBalanceDTO;
        Warehouse_WarehouseDTO Warehouse_WarehouseDTO;

        StoreController StoreController;
        StoreBalanceController StoreBalanceController;
        WarehouseController WarehouseController;
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
            DirectSalesOrderController = ServiceProvider.GetService<DirectSalesOrderController>();
            StoreBalanceController = ServiceProvider.GetService<StoreBalanceController>();
            WarehouseController = ServiceProvider.GetService<WarehouseController>();
            StoreController = ServiceProvider.GetService<StoreController>();
        }

        [Scenario]
        [Label("Tạo mới đơn hàng thành công")]
        public async Task Create_Success()
        {
            string payload = "/files/DMS/20210927/direct-sales-order-payload/payload_create.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInput(payload),
                _ => When_UserCreate(),
                _ => Then_Create_Success()
                );
        }
        [Scenario]
        [Label("Tạo mới đơn hàng thất bại do thiếu các trường bắt buộc")]
        public async Task Create_MissingRequiredField_Failed()
        {
            string payload = "/files/DMS/20210927/direct-sales-order-payload/payload_create_missingfield_failed.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInput(payload),
                _ => When_UserCreate(),
                _ => Then_CheckResultFailed(payload)
                );
        }
        [Scenario]
        [Label("Sửa đơn hàng thành công")]
        public async Task Update_Success()
        {
            string payload = "/files/DMS/20210927/direct-sales-order-payload/payload_update.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInput(payload),
                _ => When_UserUpdate(),
                _ => Then_CheckResult(payload)
                );
        }
        [Scenario]
        [Label("Phê duyệt đơn hàng thành công")]
        public async Task Approve_Success()
        {
            string payload = "/files/DMS/20210927/direct-sales-order-payload/payload_store_pending.json";
            string response = "/files/DMS/20210927/direct-sales-order-payload/response_approve_success.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInput(payload),
                _ => When_UserApproval(),
                _ => Then_CheckResult(response)
                );
        }
        [Scenario]
        [Label("Từ chối đơn hàng thành công")]
        public async Task Reject_Success()
        {
            string payload = "/files/DMS/20210927/direct-sales-order-payload/payload_store_pending.json";
            string response = "/files/DMS/20210927/direct-sales-order-payload/response_reject_success.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInput(payload),
                _ => When_UserReject(),
                _ => Then_CheckResult(response)
                );
        }
        [Scenario]
        [Label("Tìm kiếm đơn hàng theo ngày đơn hàng thành công")]
        public async Task SearchByDate_Success()
        {
            string response = "/files/DMS/20210927/direct-sales-order-payload/response_list_order_by_date.json";
            await Runner.RunScenarioAsync(
                _ => When_UserFilterByDate(),
                _ => When_UserSearch(),
                _ => Then_Search_Success(response)
                );
        }
        [Scenario]
        [Label("Tìm kiếm đơn hàng theo tổng giá thành công")]
        public async Task SearchByTotalPrice_Success()
        {
            string response = "/files/DMS/20210927/direct-sales-order-payload/response_list_order_by_total.json";
            await Runner.RunScenarioAsync(
                _ => When_UserFilterByTotal(),
                _ => When_UserSearch(),
                _ => Then_Search_Success(response)
                );
        }
        [Scenario]
        [Label("Tìm kiếm đơn hàng theo nhiều điều kiện thành công")]
        public async Task SearchMultiFilter_Success()
        {
            string payload = "/files/DMS/20210927/direct-sales-order-payload/payload_create.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInput(payload),
                _ => When_UserCreate(),
                _ => Then_Create_Success()
                );
        }
        [Scenario]
        [Label("Trả về danh sách sản phẩm")]
        public async Task SearchItemNoFilter_Success()
        {
            string payload = "/files/DMS/20210927/direct-sales-order-payload/payload_create.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInput(payload),
                _ => When_UserCreate(),
                _ => Then_Create_Success()
                );
        }
        [Scenario]
        [Label("Tìm kiếm sản phẩm theo tên và loại")]
        public async Task SearchItemByNameAndCategory_Success()
        {
            string payload = "/files/DMS/20210927/direct-sales-order-payload/payload_create.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInput(payload),
                _ => When_UserCreate(),
                _ => Then_Create_Success()
                );
        }
        [Scenario]
        [Label("Tính giá bán")]
        public async Task CheckAmountAfterCreateOrder_Success()
        {
            string payload = "/files/DMS/20210927/direct-sales-order-payload/payload_create.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInput(payload),
                _ => When_UserCreate(),
                _ => Then_Create_Success()
                );
        }

        [Scenario]
        [Label("Phân luồng đơn hàng khi tạo mới")]
        public async Task ClassifyOrder_WhenCreate_Success()
        {
            string payload = "/files/DMS/20210927/direct-sales-order-payload/payload_create.json";
            string response = "/files/DMS/20210927/direct-sales-order-payload/response_create_and_classify.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInput(payload),
                _ => When_UserCreate(),
                _ => Then_Classify_Success(response)
                );
        }
        [Scenario]
        [Label("Search đơn hàng có trạng thái tồn kho và công nợ")]
        public async Task Search_WithCheckState_Success()
        {
            string response = "/files/DMS/20210927/direct-sales-order-payload/response_list_pass_store_balance_check_state.json";
            await Runner.RunScenarioAsync(
                _ => When_UserFilterById(620),
                _ => When_UserSearch(),
                _ => Then_ListOrder_WithCheckState_Success(response)
                );
        }
        [Scenario]
        [Label("Chuyển trạng thái công nợ của đơn hàng thành PASS khi thay đổi công nợ")]
        public async Task ChangeStoreBalanceState_PASS_WhenChangeStoreBalance_Success()
        {
            string payload = "/files/DMS/20210927/direct-sales-order-payload/payload_update_store_balance.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInputStoreBalance(payload),
                _ => When_UserUpdateStoreBalance(),
                _ => When_UserFilterById(698),
                _ => When_UserSearch(),
                _ => Then_GetState_Success(1, 1)
                );
        }
        [Scenario]
        [Label("Chuyển trạng thái công nợ của đơn hàng thành NOT PASS khi thay đổi công nợ")]
        public async Task ChangeStoreBalanceState_NOTPASS_WhenChangeStoreBalance_Success()
        {
            string payload = "/files/DMS/20210927/direct-sales-order-payload/payload_update_store_balance_reduce.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInputStoreBalance(payload),
                _ => When_UserUpdateStoreBalance(),
                _ => When_UserFilterById(620),
                _ => When_UserSearch(),
                _ => Then_GetState_Success(1, 0)
                );
        }
        [Scenario]
        [Label("Chuyển trạng thái công nợ của đơn hàng thành PASS khi thay đổi định mức nợ")]
        public async Task ChangeStoreBalanceState_PASS_WhenChangeStoreDebtLimit_Success()
        {
            string payload = "/files/DMS/20210927/direct-sales-order-payload/payload_update_store.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInputStore(payload),
                _ => When_UserUpdateStore(),
                _ => When_UserFilterById(727),
                _ => When_UserSearch(),
                _ => Then_GetState_Success(0, 1)
                );
        }
    }
}
