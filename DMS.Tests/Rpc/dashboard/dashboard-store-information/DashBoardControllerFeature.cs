using DMS.Common;
using DMS.DWModels;
using DMS.Models;
using DMS.Rpc.dashboards.store_information;
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
    public partial class DashboardControllerFeature : BaseTests
    {
        private const string DATA_DMS_PATH = "/files/DMS/bachlx/DMS_1121.xlsx";
        private const string DW_DMS_PATH = "/files/DMS/bachlx/DW_DMS_1121.xlsx";
        public const string INIT_DMS_PATH = "/files/DMS/DMS_Script_1121.txt";
        public const string INIT_DWDMS_PATH = "/files/DMS/DW_DMS_Script_1121.txt";
        //StoreController StoreController;
        DashboardStoreInformationController DashboardStoreInformationController;

        #region DTO
        List<DashboardStoreInformation_BrandStatisticsDTO> DashboardStoreInformation_BrandStatisticsDTOs;
        List<DashboardStoreInformation_StoreDTO> DashboardStoreInformation_StoreDTOs;
        List<DashboardStoreInformation_BrandStatisticsDTO> DashboardStoreInformation_BrandUnStatisticsDTOs;
        List<DashboardStoreInformation_EstimatedRevenueStatisticsDTO> DashboardStoreInformation_EstimatedRevenueStatisticsDTOs;
        List<DashboardStoreInformation_ProductGroupingStatisticsDTO> DashboardStoreInformation_ProductGroupingStatisticsDTOs;
        List<DashboardStoreInformation_ProductGroupingNumberStatisticsDTO> DashboardStoreInformation_ProductGroupingNumberStatisticsDTOs;
        DashboardStoreInformation_StoreCounterDTO DashboardStoreInformation_StoreCounterDTO;
        List<DashboardStoreInformation_TopBrandDTO> DashboardStoreInformation_TopBrandDTOs;
        DashboardStoreInformation_StoreCounterFilterDTO DashboardStoreInformation_StoreCounterFilterDTO;
        DashboardStoreInformation_BrandStatisticsFilterDTO DashboardStoreInformation_BrandStatisticsFilterDTO;
        DashboardStoreInformation_BrandStatisticsFilterDTO DashboardStoreInformation_BrandUnStatisticsFilterDTO;
        DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO;
        DashboardStoreInformation_ProductGroupingStatisticsFilterDTO DashboardStoreInformation_ProductGroupingStatisticsFilterDTO;
        DashboardStoreInformation_ProductGroupingNumberStatisticsFilterDTO DashboardStoreInformation_ProductGroupingNumberStatisticsFilterDTO;
        DashboardStoreInformation_StoreFilterDTO DashboardStoreInformation_StoreFilterDTO;
        DashboardStoreInformation_TopBrandFilterDTO DashboardStoreInformation_TopBrandFilterDTO;
        #endregion

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
            //StoreController = ServiceProvider.GetService<StoreController>();
            DashboardStoreInformationController = ServiceProvider.GetService<DashboardStoreInformationController>();
        }
        #region Store counter
        [Scenario]
        [Label("Tổng số đại lý không filter")]
        public async Task Get_Dashboard_DWStoreInformation_StoreCounter_NoFilter()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.StoreCounter),
                _ => When_UserInput_StoreCounter_NoFilter(),
                _ => Get_Store_Counter(),
                _ => Then_StoreCounter_Result(15, 21932)
                );
        }
        [Scenario]
        [Label("Tổng số đại lý có filter Org và Brand")]
        public async Task Get_Dashboard_DWStoreInformation_StoreCounter_FilterOrganization_Brand()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.StoreCounter),
                _ => When_UserInput_StoreCounter_WithFilter(),
                _ => Get_Store_Counter(),
                _ => Then_StoreCounter_Result(13, 10380)
                );
        }
        [Scenario]
        [Label("Tổng số đại lý có filter Date")]
        public async Task Get_Dashboard_DWStoreInformation_StoreCounter_FilterOrderDate()
        {
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.StoreCounter),
                _ => When_UserInput_StoreCounter_WithFilter_OrderDate(),
                _ => Get_Store_Counter(),
                _ => Then_StoreCounter_Result(10, 21920)
                );
        }
        #endregion

        #region Brand statistics
        [Scenario]
        [Label("Hiện diện theo hãng không filter")]
        public async Task Get_Dashboard_DWStoreInformation_BrandStatistics_NoFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/brand-statistics-nofilter.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.BrandStatistic),
                _ => When_UserInput_BrandStatistics_NoFilter(),
                _ => Get_BrandStatistics(),
                _ => Then_BrandStatistics_Result(json_path)
                );
        }
        [Scenario]
        [Label("Hiện diện theo hãng có filter Org và Brand")]
        public async Task Get_Dashboard_DWStoreInformation_BrandStatistics_WithFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/brand-statistics-withfilter.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.BrandStatistic),
                _ => When_UserInput_BrandStatistics_WithFilter(),
                _ => Get_BrandStatistics(),
                _ => Then_BrandStatistics_Result(json_path)
                );
        }
        [Scenario]
        [Label("Hiện diện theo hãng có filter Đến ngày")]
        public async Task Get_Dashboard_DWStoreInformation_BrandStatistics_WithOrderDateFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/brand-statistics-orderdate.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.BrandStatistic),
                _ => When_UserInput_BrandStatistics_WithFilter_OrderDate(),
                _ => Get_BrandStatistics(),
                _ => Then_BrandStatistics_Result(json_path)
                );
        }
        #endregion

        #region Brand unstatistics
        [Scenario]
        [Label("Hiện diện theo hãng không filter")]
        public async Task Get_Dashboard_DWStoreInformation_BrandUnStatistics_NoFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/brand-unstatistics-nofilter.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.BrandUnStatistic),
                _ => When_UserInput_BrandUnStatistics_NoFilter(),
                _ => Get_BrandUnStatistics(),
                _ => Then_BrandUnStatistics_Result(json_path)
                );
        }
        [Scenario]
        [Label("Hiện diện theo hãng có filter Org và Brand")]
        public async Task Get_Dashboard_DWStoreInformation_BrandUnStatistics_WithFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/brand-unstatistics-withfilter.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.BrandUnStatistic),
                _ => When_UserInput_BrandUnStatistics_WithFilter(),
                _ => Get_BrandUnStatistics(),
                _ => Then_BrandUnStatistics_Result(json_path)
                );
        }
        [Scenario]
        [Label("Hiện diện theo hãng có filter Đến ngày")]
        public async Task Get_Dashboard_DWStoreInformation_BrandUnStatistics_WithOrderDateFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/brand-unstatistics-orderdate.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.BrandUnStatistic),
                _ => When_UserInput_BrandUnStatistics_WithFilter_OrderDate(),
                _ => Get_BrandUnStatistics(),
                _ => Then_BrandUnStatistics_Result(json_path)
                );
        }
        #endregion

        #region Store coverage
        [Scenario]
        [Label("Bản đồ hệ thống không filter")]
        public async Task Get_Dashboard_DWStoreInformation_StoreCoverage_NoFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/store-coverage-nofilter.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.StoreCoverage),
                _ => When_UserInput_StoreCoverage_NoFilter(),
                _ => Get_StoreCoverage(),
                _ => Then_StoreCoverage_Result(json_path)
                );
        }
        [Scenario]
        [Label("Bản đồ hệ thống có filter")]
        public async Task Get_Dashboard_DWStoreInformation_StoreCoverage_WithFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/store-coverage-withfilter.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.StoreCoverage),
                _ => When_UserInput_StoreCoverage_WithFilter(),
                _ => Get_StoreCoverage(),
                _ => Then_StoreCoverage_Result(json_path)
                );
        }
        [Scenario]
        [Label("Bản đồ hệ thống có filter theo ngày")]
        public async Task Get_Dashboard_DWStoreInformation_StoreCoverage_WithOrderDateFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/store-coverage-orderdate.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.StoreCoverage),
                _ => When_UserInput_StoreCoverage_WithFilter_OrderDate(),
                _ => Get_StoreCoverage(),
                _ => Then_StoreCoverage_Result(json_path)
                );
        }
        #endregion

        #region Product grouping number
        [Scenario]
        [Label("Hiện diện theo số nhóm sản phẩm không filter")]
        public async Task Get_Dashboard_DWStoreInformation_ProductGroupingNumberStatistics_NoFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/product-grouping-number-nofilter.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.ProductGroupingNumberStatistic),
                _ => When_UserInput_ProductGroupingNumberStatistics_NoFilter(),
                _ => Get_ProductGroupingNumberStatistics(),
                _ => Then_ProductGroupingNumberStatistics_Result(json_path)
                );
        }
        [Scenario]
        [Label("Hiện diện theo số nhóm sản phẩm có filter")]
        public async Task Get_Dashboard_DWStoreInformation_ProductGroupingNumberStatistics_WithFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/product-grouping-number-withfilter.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.ProductGroupingNumberStatistic),
                _ => When_UserInput_ProductGroupingNumberStatistics_WithFilter(),
                _ => Get_ProductGroupingNumberStatistics(),
                _ => Then_ProductGroupingNumberStatistics_Result(json_path)
                );
        }
        [Scenario]
        [Label("Hiện diện theo số nhóm sản phẩm filter theo ngày")]
        public async Task Get_Dashboard_DWStoreInformation_ProductGroupingNumberStatistics_WithOrderDateFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/product-grouping-number-orderdate.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.ProductGroupingNumberStatistic),
                _ => When_UserInput_ProductGroupingNumberStatistics_WithFilter_OrderDate(),
                _ => Get_ProductGroupingNumberStatistics(),
                _ => Then_ProductGroupingNumberStatistics_Result(json_path)
                );
        }
        #endregion

        #region Product grouping
        [Scenario]
        [Label("Hiện diện theo nhóm sản phẩm không filter")]
        public async Task Get_Dashboard_DWStoreInformation_ProductGroupingStatistics_NoFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/product-grouping-nofilter.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.ProductGroupingStatistic),
                _ => When_UserInput_ProductGroupingStatistics_NoFilter(),
                _ => Get_ProductGroupingStatistics(),
                _ => Then_ProductGroupingStatistics_Result(json_path)
                );
        }
        [Scenario]
        [Label("Hiện diện theo nhóm sản phẩm có filter")]
        public async Task Get_Dashboard_DWStoreInformation_ProductGroupingStatistics_WithFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/product-grouping-withfilter.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.ProductGroupingStatistic),
                _ => When_UserInput_ProductGroupingStatistics_WithFilter(),
                _ => Get_ProductGroupingStatistics(),
                _ => Then_ProductGroupingStatistics_Result(json_path)
                );
        }
        [Scenario]
        [Label("Hiện diện theo nhóm sản phẩm filter theo ngày")]
        public async Task Get_Dashboard_DWStoreInformation_ProductGroupingStatistics_WithOrderDateFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/product-grouping-orderdate.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.ProductGroupingStatistic),
                _ => When_UserInput_ProductGroupingStatistics_WithFilter_OrderDate(),
                _ => Get_ProductGroupingStatistics(),
                _ => Then_ProductGroupingStatistics_Result(json_path)
                );
        }
        #endregion

        #region Estimated revenue
        [Scenario]
        [Label("Hiện diện theo ước doanh thu ngành đèn không filter")]
        public async Task Get_Dashboard_DWStoreInformation_EstimatedRevenueStatistics_NoFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/estimated-revenue-nofilter.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.EstimatedRevenueStatistic),
                _ => When_UserInput_EstimatedRevenueStatistics_NoFilter(),
                _ => Get_EstimatedRevenueStatistics(),
                _ => Then_EstimatedRevenueStatistics_Result(json_path)
                );
        }
        [Scenario]
        [Label("Hiện diện theo ước doanh thu ngành đèn có filter")]
        public async Task Get_Dashboard_DWStoreInformation_EstimatedRevenueStatistics_WithFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/estimated-revenue-withfilter.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.EstimatedRevenueStatistic),
                _ => When_UserInput_EstimatedRevenueStatistics_WithFilter(),
                _ => Get_EstimatedRevenueStatistics(),
                _ => Then_EstimatedRevenueStatistics_Result(json_path)
                );
        }
        [Scenario]
        [Label("Hiện diện theo ước doanh thu ngành đèn filter theo ngày")]
        public async Task Get_Dashboard_DWStoreInformation_EstimatedRevenueStatistics_WithOrderDateFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/estimated-revenue-orderdate.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.EstimatedRevenueStatistic),
                _ => When_UserInput_EstimatedRevenueStatistics_WithFilter_OrderDate(),
                _ => Get_EstimatedRevenueStatistics(),
                _ => Then_EstimatedRevenueStatistics_Result(json_path)
                );
        }
        #endregion

        #region Estimated revenue
        [Scenario]
        [Label("Thứ tự theo hạng không filter")]
        public async Task Get_Dashboard_DWStoreInformation_TopBrand_NoFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/top-brand-nofilter.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.TopBrand),
                _ => When_UserInput_TopBrand_NoFilter(),
                _ => Get_TopBrand(),
                _ => Then_TopBrand_Result(json_path)
                );
        }
        [Scenario]
        [Label("Thứ tự theo hạng có filter")]
        public async Task Get_Dashboard_DWStoreInformation_TopBrand_WithFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/top-brand-withfilter.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.TopBrand),
                _ => When_UserInput_TopBrand_WithFilter(),
                _ => Get_TopBrand(),
                _ => Then_TopBrand_Result(json_path)
                );
        }
        [Scenario]
        [Label("Thứ tự theo hạng filter theo ngày")]
        public async Task Get_Dashboard_DWStoreInformation_TopBrand_WithOrderDateFilter()
        {
            string json_path = "rpc/dashboard/dashboard-store-information/result-json/top-brand-orderdate.json";
            await Runner.RunScenarioAsync(
                _ => GetCurrentContext(2, DashboardStoreInformationRoute.TopBrand),
                _ => When_UserInput_TopBrand_WithFilter_OrderDate(),
                _ => Get_TopBrand(),
                _ => Then_TopBrand_Result(json_path)
                );
        }
        #endregion
    }
}
