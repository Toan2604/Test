using DMS.Common;
using DMS.DWModels;
using DMS.Models;
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
    public partial class DashboardUserControllerFeature : BaseTests
    {
        private const string DATA_DMS_PATH = "/files/DMS/bachlx/DMS_1121.xlsx";
        private const string DW_DMS_PATH = "/files/DMS/bachlx/DW_DMS_1121.xlsx";
        public const string INIT_DMS_PATH = "/files/DMS/DMS_Script_1121.txt";
        public const string INIT_DWDMS_PATH = "/files/DMS/DW_DMS_Script_1121.txt";

        DashboardUserController DashboardUserController;
        DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO;
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
            DashboardUserController = ServiceProvider.GetService<DashboardUserController>();
        }
        [Scenario]
        [Label("Tổng số đơn gián tiếp tháng trước")]
        public async Task Get_Dashboard_User_SaleQuality_LastMonth()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardUserRoute.SalesQuantity),
                _ => When_UserInputFilter_User_SaleQuality_LastMonth(),
                _ => Then_Result(40)
                );
        }
        [Scenario]
        [Label("Tổng số đơn gián tiếp tuần này")]
        public async Task Get_Dashboard_User_SaleQuality_ThisWeek()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardUserRoute.SalesQuantity),
                _ => When_UserInputFilter_User_SaleQuality_ThisWeek(),
                _ => Then_Result(0)
                );
        }
        [Scenario]
        [Label("Tổng cửa hàng checking số đơn gián tiếp tháng trước")]
        public async Task Get_Dashboard_User_StoreChecking_LastMonth()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardUserRoute.StoreChecking),
                _ => When_UserInputFilter_User_StoreChecking_LastMonth(),
                _ => Then_Result(9)
                );
        }
        [Scenario]
        [Label("Tổng doanh thu đơn hàng gián tiếp tháng trước")]
        public async Task Get_Dashboard_User_Revenue_LastMonth()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardUserRoute.Revenue),
                _ => When_UserInputFilter_User_Revenue_LastMonth(),
                _ => Then_DecimalResult(3367000)
                );
        }
        [Scenario]
        [Label("Thống kê đơn hàng gián tiếp tháng trước")]
        public async Task Get_Dashboard_User_StatisticIndirectSalesOrder_LastMonth()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardUserRoute.StatisticIndirectSalesOrder),
                _ => When_UserInputFilter_User_StatisticIndirectSalesOrder_LastMonth(),
                _ => Then_Result(19)
                );
        }
        [Scenario]
        [Label("Tổng số đơn trực tiếp tháng trước")]
        public async Task Get_Dashboard_User_DirectSaleQuality_LastMonth()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardUserRoute.DirectSalesQuantity),
                _ => When_UserInputFilter_User_DirectSaleQuality_LastMonth(),
                _ => Then_Result(123)
                );
        }
        [Scenario]
        [Label("Tổng số đơn gián tiếp tuần này")]
        public async Task Get_Dashboard_User_DirectSaleQuality_ThisWeek()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardUserRoute.DirectSalesQuantity),
                _ => When_UserInputFilter_User_DirectSaleQuality_ThisWeek(),
                _ => Then_Result(0)
                );
        }
        [Scenario]
        [Label("Tổng doanh thu đơn hàng gián tiếp tháng trước")]
        public async Task Get_Dashboard_User_DirectRevenue_LastMonth()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardUserRoute.DirectRevenue),
                _ => When_UserInputFilter_User_DirectRevenue_LastMonth(),
                _ => Then_DecimalResult(28257500)
                );
        }
        [Scenario]
        [Label("Thống kê đơn hàng gián tiếp tháng trước")]
        public async Task Get_Dashboard_User_StatisticDirectSalesOrder_LastMonth()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardUserRoute.StatisticDirectSalesOrder),
                _ => When_UserInputFilter_User_StatisticDirectSalesOrder_LastMonth(),
                _ => Then_Result(19)
                );
        }
    }
}
