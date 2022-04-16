using DMS.Common;
using DMS.DWModels;
using DMS.Models;
using DMS.Rpc.dashboards.director;
using DMS.Rpc.dashboards.store_information;
using DMS.Rpc.dashboards.user;
using DMS.Rpc.store;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.NUnit3;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;
namespace DMS.Tests.Rpc.dashboard
{
    [TestFixture]
    [FeatureDescription(@"Báo cáo tổng số đại lý")]
    [Label("Story-1")]
    public partial class DashboardDirectorControllerFeature : BaseTests
    {
        private const string DATA_DMS_PATH = "/files/DMS/bachlx/DMS_1121.xlsx";
        private const string DW_DMS_PATH = "/files/DMS/bachlx/DW_DMS_1121.xlsx";
        public const string INIT_DMS_PATH = "/files/DMS/DMS_Script_1121.txt";
        public const string INIT_DWDMS_PATH = "/files/DMS/DW_DMS_Script_1121.txt";
        DashboardDirectorController DashboardDirectorController;

        #region DTO
        DashboardDirector_StoreFilterDTO DashboardDirector_StoreFilterDTO;
        DashboardDirector_IndirectSalesOrderFluctuationFilterDTO DashboardDirector_IndirectSalesOrderFluctuationFilterDTO;
        DashboardDirector_SaledItemFluctuationFilterDTO DashboardDirector_SaledItemFluctuationFilterDTO;
        DashboardDirector_Top5RevenueByProductFilterDTO DashboardDirector_Top5RevenueByProductFilterDTO;
        DashboardDirector_RevenueFluctuationFilterDTO DashboardDirector_RevenueFluctuationFilterDTO;
        DashboardDirector_DirectSalesOrderFluctuationFilterDTO DashboardDirector_DirectSalesOrderFluctuationFilterDTO;
        DashboardDirector_Top5RevenueByEmployeeFilterDTO DashboardDirector_Top5RevenueByEmployeeFilterDTO;
        DashboardDirector_StatisticDailyDTO DashboardDirector_StatisticDailyDTO;
        List<DashboardDirector_StoreDTO> DashboardDirector_StoreDTOs;
        List<DashboardDirector_AppUserDTO> DashboardDirector_AppUserDTOs;
        List<DashboardDirector_IndirectSalesOrderDTO> DashboardDirector_IndirectSalesOrderDTOs;
        List<DashboardDirector_Top5RevenueByEmployeeDTO> DashboardDirector_Top5RevenueByEmployeeDTOs;
        List<DashboardDirector_Top5RevenueByProductDTO> DashboardDirector_Top5RevenueByProductDTOs;
        DashboardDirector_RevenueFluctuationDTO DashboardDirector_RevenueFluctuationDTO;
        DashboardDirector_IndirectSalesOrderFluctuationDTO DashboardDirector_IndirectSalesOrderFluctuationDTO;
        DashboardDirector_StatisticDailyDirectSalesOrderDTO DashboardDirector_StatisticDailyDirectSalesOrderDTO;
        List<DashboardDirector_DirectSalesOrderDTO> DashboardDirector_DirectSalesOrderDTOs;
        DashboardDirector_DirectSalesOrderFluctuationDTO DashboardDirector_DirectSalesOrderFluctuationDTO;
        #endregion
        long Result;
        decimal DecimalResult;

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
            DashboardDirectorController = ServiceProvider.GetService<DashboardDirectorController>();
        }
        #region Indirect Sales Order
        [Scenario]
        [Label("Tổng số đại lý có đơn hàng gián tiếp")]
        public async Task Get_Dashboard_Director_Indirect_CountStore_NoFilter()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.CountStore),
                _ => When_UserInputFilter_Director_CountStore_NoFilter(),
                _ => Then_Result(97137)
                );
        }
        [Scenario]
        [Label("Tổng số đơn hàng gián tiếp")]
        public async Task Get_Dashboard_Director_Indirect_CountIndirectSalesOrder_ThisYear()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.CountIndirectSalesOrder),
                _ => When_UserInputFilter_Director_CountIndirectSalesOrder_ThisYear(),
                _ => Then_Result(166)
                );
        }
        [Scenario]
        [Label("Tổng số doanh thu cửa hàng từ đơn gián tiếp")]
        public async Task Get_Dashboard_Director_Indirect_RevenueTotal_ThisYear()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.RevenueTotal),
                _ => When_UserInputFilter_Director_RevenueTotal_ThisYear(),
                _ => Then_DecimalResult(279513300263.125m)
                );
        }
        [Scenario]
        [Label("Số cửa hàng dự thảo đã check out")]
        public async Task Get_Dashboard_Director_Indirect_StoreHasCheckedCounter_ThisYear()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.StoreHasCheckedCounter),
                _ => When_UserInputFilter_Director_StoreHasCheckedCounter_ThisYear(),
                _ => Then_Result(125)
                );
        }
        [Scenario]
        [Label("Số cửa hàng dự thảo")]
        public async Task Get_Dashboard_Director_Indirect_CountStoreChecking_ThisYear()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.CountStoreChecking),
                _ => When_UserInputFilter_Director_CountStoreChecking_ThisYear(),
                _ => Then_Result(954)
                );
        }
        [Scenario]
        [Label("Thống kê hôm nay đơn gián tiếp")]
        public async Task Get_Dashboard_Director_Indirect_StatisticToday()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.StatisticToday),
                _ => When_UserInputFilter_Director_StatisticToday(),
                _ => Then_StatisticDailyResult(0,0,0,0)
                );
        }
        [Scenario]
        [Label("Thống kê hôm qua đơn gián tiếp")]
        public async Task Get_Dashboard_Director_Indirect_StatisticYesterday()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.StatisticYesterday),
                _ => When_UserInputFilter_Director_StatisticYesterday(),
                _ => Then_StatisticDailyResult(0, 0, 0, 0)
                );
        }
        [Scenario]
        [Label("Bản đồ hệ thống phân phối")]
        public async Task Get_Dashboard_Director_Indirect_StoreCoverage_NoFilter()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.StoreCoverage),
                _ => When_UserInputFilter_Director_StoreCoverage_NoFilter(),
                _ => Then_StoreCoverage_NoFilter_Result()
                );
        }
        [Scenario]
        [Label("Bản đồ hệ thống tiếp thị")]
        public async Task Get_Dashboard_Director_Indirect_SaleEmployeeLocation_NoFilter()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.SaleEmployeeLocation),
                _ => When_UserInputFilter_Director_SaleEmployeeLocation_NoFilter(),
                _ => Then_SaleEmployeeLocation_NoFilter_Result()
                );
        }
        [Scenario]
        [Label("Danh sách đơn hàng gián tiếp")]
        public async Task Get_Dashboard_Director_Indirect_ListIndirectSalesOrder_NoFilter()
        {
            string json_path = "/files/DMS/bachlx/ExpectedJson/....json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.ListIndirectSalesOrder),
                _ => When_UserInputFilter_Director_ListIndirectSalesOrder_NoFilter(),
                _ => Then_ListIndirectSalesOrder_NoFilter_Result(json_path)
                );
        }
        [Scenario]
        [Label("Top 5 doanh thu gián tiếp theo sản phẩm")]
        public async Task Get_Dashboard_Director_Indirect_Top5RevenueByProduct_NoFilter()
        {
            string json_path = "/files/DMS/bachlx/ExpectedJson/....json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.Top5RevenueByProduct),
                _ => When_UserInputFilter_Director_Top5RevenueByProduct_NoFilter(),
                _ => Then_Top5RevenueByProduct_NoFilter_Result(json_path)
                );
        }
        [Scenario]
        [Label("Top 5 doanh thu gián tiếp theo sản phẩm")]
        public async Task Get_Dashboard_Director_Indirect_Top5RevenueByEmployee_NoFilter()
        {
            string json_path = "/files/DMS/bachlx/ExpectedJson/....json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.Top5RevenueByEmployee),
                _ => When_UserInputFilter_Director_Top5RevenueByEmployee_NoFilter(),
                _ => Then_Top5RevenueByEmployee_NoFilter_Result(json_path)
                );
        }
        [Scenario]
        [Label("Biểu đồ tăng trưởng doanh thu đơn hàng gián tiếp trong năm nay")]
        public async Task Get_Dashboard_Director_Indirect_RevenueFluctuation_ThisYear()
        {
            string json_path = "/files/DMS/bachlx/ExpectedJson/....json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.RevenueFluctuation),
                _ => When_UserInputFilter_Director_RevenueFluctuation_NoFilter(),
                _ => Then_RevenueFluctuation_ThisYear_Result(json_path)
                );
        }
        [Scenario]
        [Label("Biểu đồ tăng trưởng đơn hàng gián tiếp trong năm nay")]
        public async Task Get_Dashboard_Director_Indirect_SalesOrderFluctuation_ThisYear()
        {
            string json_path = "/files/DMS/bachlx/ExpectedJson/....json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.IndirectSalesOrderFluctuation),
                _ => When_UserInputFilter_Director_IndirectSalesOrderFluctuation_ThisYear(),
                _ => Then_IndirectSalesOrderFluctuation_ThisYear_Result(json_path)
                );
        }
        #endregion
        #region Direct Sales Order
        [Scenario]
        [Label("Tổng số đơn hàng trực tiếp")]
        public async Task Get_Dashboard_Director_Direct_CountDirectSalesOrder_ThisYear()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.CountDirectSalesOrder),
                _ => When_UserInputFilter_Director_CountDirectSalesOrder_ThisYear(),
                _ => Then_Result(26)
                );
        }
        [Scenario]
        [Label("Tổng số doanh thu cửa hàng từ đơn trực tiếp")]
        public async Task Get_Dashboard_Director_Direct_DirectRevenueTotal_ThisYear()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.DirectRevenueTotal),
                _ => When_UserInputFilter_Director_DirectRevenueTotal_ThisYear(),
                _ => Then_DecimalResult(49162000.0000M)
                );
        }
        [Scenario]
        [Label("Thống kê hôm nay đơn trực tiếp")]
        public async Task Get_Dashboard_Director_Direct_DirectStatisticToday_28072021()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.DirectStatisticToday),
                _ => When_UserInputFilter_Director_DirectStatisticToday(),
                _ => Then_DirectStatisticDailyResult(39402000.0000M, 4, 4, 10)
                );
        }
        [Scenario]
        [Label("Thống kê hôm qua đơn trực tiếp")]
        public async Task Get_Dashboard_Director_Direct_DirectStatisticYesterday_28072021()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.DirectStatisticYesterday),
                _ => When_UserInputFilter_Director_DirectStatisticYesterday(),
                _ => Then_DirectStatisticDailyResult(33000.0000M, 3, 3, 2)
                );
        }
        [Scenario]
        [Label("Danh sách đơn hàng trực tiếp")]
        public async Task Get_Dashboard_Director_Direct_ListDirectSalesOrder_NoFilter()
        {
            string json_path = "/files/DMS/bachlx/ExpectedJson/....json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.CountDirectSalesOrder),
                _ => When_UserInputFilter_Director_ListDirectSalesOrder_NoFilter(),
                _ => Then_ListDirectSalesOrder_NoFilter_Result(json_path)
                );
        }
        [Scenario]
        [Label("Top 5 doanh thu trực tiếp theo sản phẩm")]
        public async Task Get_Dashboard_Director_Direct_Top5DirectRevenueByProduct_NoFilter()
        {
            string json_path = "/files/DMS/bachlx/ExpectedJson/....json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.Top5DirectRevenueByProduct),
                _ => When_UserInputFilter_Director_Top5DirectRevenueByProduct_NoFilter(),
                _ => Then_Top5DirectRevenueByProduct_NoFilter_Result(json_path)
                );
        }
        [Scenario]
        [Label("Top 5 doanh thu trực tiếp theo nhân viên")]
        public async Task Get_Dashboard_Director_Direct_Top5RevenueByEmployee_NoFilter()
        {
            string json_path = "/files/DMS/bachlx/ExpectedJson/....json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.Top5DirectRevenueByEmployee),
                _ => When_UserInputFilter_Director_Top5DirectRevenueByEmployee_NoFilter(),
                _ => Then_Top5DirectRevenueByEmployee_NoFilter_Result(json_path)
                );
        }
        [Scenario]
        [Label("Biểu đồ tăng trưởng doanh thu đơn hàng trực tiếp trong năm nay")]
        public async Task Get_Dashboard_Director_Direct_DirectRevenueFluctuation_ThisYear()
        {
            string json_path = "/files/DMS/bachlx/ExpectedJson/....json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.DirectRevenueFluctuation),
                _ => When_UserInputFilter_Director_DirectRevenueFluctuation_NoFilter(),
                _ => Then_DirectRevenueFluctuation_ThisYear_Result(json_path)
                );
        }
        [Scenario]
        [Label("Biểu đồ tăng trưởng đơn hàng trực tiếp trong năm nay")]
        public async Task Get_Dashboard_Director_Direct_DirectSalesOrderFluctuation_ThisYear()
        {
            string json_path = "/files/DMS/bachlx/ExpectedJson/....json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardDirectorRoute.DirectSalesOrderFluctuation),
                _ => When_UserInputFilter_Director_DirectSalesOrderFluctuation_ThisYear(),
                _ => Then_DirectSalesOrderFluctuation_ThisYear_Result(json_path)
                );
        }
        #endregion
    }
}
