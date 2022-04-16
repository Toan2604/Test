using DMS.Rpc.dashboards.director;
using System.Threading.Tasks;
using TrueSight.Common;
namespace DMS.Tests.Rpc.dashboard
{
    public partial class DashboardDirectorControllerFeature
    {
        #region Indirect Sales Order
        private async Task When_UserInputFilter_Director_CountStore_NoFilter()
        {
            DashboardDirector_StoreFilterDTO = new DashboardDirector_StoreFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                Time = new IdFilter(),
                ProvinceId = new IdFilter()
            };
            Result = await DashboardDirectorController.CountStore(DashboardDirector_StoreFilterDTO);
        }
        private async Task When_UserInputFilter_Director_CountIndirectSalesOrder_ThisYear()
        {
            DashboardDirector_IndirectSalesOrderFluctuationFilterDTO = new DashboardDirector_IndirectSalesOrderFluctuationFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                Time = new IdFilter { Equal = 8 },
                ProvinceId = new IdFilter()
            };
            Result = await DashboardDirectorController.CountIndirectSalesOrder(DashboardDirector_IndirectSalesOrderFluctuationFilterDTO);
        }
        private async Task When_UserInputFilter_Director_RevenueTotal_ThisYear()
        {
            DashboardDirector_SaledItemFluctuationFilterDTO = new DashboardDirector_SaledItemFluctuationFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                Time = new IdFilter { Equal = 8 },
                ProvinceId = new IdFilter()
            };
            DecimalResult = await DashboardDirectorController.RevenueTotal(DashboardDirector_SaledItemFluctuationFilterDTO);
        }
        private async Task When_UserInputFilter_Director_StoreHasCheckedCounter_ThisYear()
        {
            DashboardDirector_SaledItemFluctuationFilterDTO = new DashboardDirector_SaledItemFluctuationFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                Time = new IdFilter { Equal = 8 },
                ProvinceId = new IdFilter()
            };
            Result = await DashboardDirectorController.StoreHasCheckedCounter(DashboardDirector_SaledItemFluctuationFilterDTO);
        }
        private async Task When_UserInputFilter_Director_CountStoreChecking_ThisYear()
        {
            DashboardDirector_StoreFilterDTO = new DashboardDirector_StoreFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                Time = new IdFilter { Equal = 8 },
                ProvinceId = new IdFilter()
            };
            Result = await DashboardDirectorController.CountStoreChecking(DashboardDirector_StoreFilterDTO);
        }
        private async Task When_UserInputFilter_Director_StatisticToday()
        {
            DashboardDirector_StoreFilterDTO = new DashboardDirector_StoreFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                ProvinceId = new IdFilter()
            };
            DashboardDirector_StatisticDailyDTO = await DashboardDirectorController.StatisticToday(DashboardDirector_StoreFilterDTO);
        }
        private async Task When_UserInputFilter_Director_StatisticYesterday()
        {
            DashboardDirector_IndirectSalesOrderFluctuationFilterDTO = new DashboardDirector_IndirectSalesOrderFluctuationFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                ProvinceId = new IdFilter()
            };
            DashboardDirector_StatisticDailyDTO = await DashboardDirectorController.StatisticYesterday(DashboardDirector_IndirectSalesOrderFluctuationFilterDTO);
        }
        private async Task When_UserInputFilter_Director_StoreCoverage_NoFilter()
        {
            DashboardDirector_StoreFilterDTO = new DashboardDirector_StoreFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                ProvinceId = new IdFilter()
            };
            DashboardDirector_StoreDTOs = await DashboardDirectorController.StoreCoverage(DashboardDirector_StoreFilterDTO);
        }
        private async Task When_UserInputFilter_Director_SaleEmployeeLocation_NoFilter()
        {
            DashboardDirector_StoreFilterDTO = new DashboardDirector_StoreFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                ProvinceId = new IdFilter(),
                Time = new IdFilter { Equal = 8 }
            };
            DashboardDirector_AppUserDTOs = await DashboardDirectorController.SaleEmployeeLocation(DashboardDirector_StoreFilterDTO);
        }
        private async Task When_UserInputFilter_Director_ListIndirectSalesOrder_NoFilter()
        {
            DashboardDirector_IndirectSalesOrderFluctuationFilterDTO = new DashboardDirector_IndirectSalesOrderFluctuationFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                ProvinceId = new IdFilter(),
            };
            DashboardDirector_IndirectSalesOrderDTOs = await DashboardDirectorController.ListIndirectSalesOrder(DashboardDirector_IndirectSalesOrderFluctuationFilterDTO);
        }
        private async Task When_UserInputFilter_Director_Top5RevenueByProduct_NoFilter()
        {
            DashboardDirector_Top5RevenueByProductFilterDTO = new DashboardDirector_Top5RevenueByProductFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                ProvinceId = new IdFilter(),
                Time = new IdFilter { Equal = 8 },
            };
            DashboardDirector_Top5RevenueByProductDTOs = await DashboardDirectorController.Top5RevenueByProduct(DashboardDirector_Top5RevenueByProductFilterDTO);
        }
        private async Task When_UserInputFilter_Director_Top5RevenueByEmployee_NoFilter()
        {
            DashboardDirector_Top5RevenueByEmployeeFilterDTO = new DashboardDirector_Top5RevenueByEmployeeFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                ProvinceId = new IdFilter(),
                Time = new IdFilter { Equal = 8 },
            };
            DashboardDirector_Top5RevenueByEmployeeDTOs = await DashboardDirectorController.Top5RevenueByEmployee(DashboardDirector_Top5RevenueByEmployeeFilterDTO);
        }
        private async Task When_UserInputFilter_Director_RevenueFluctuation_NoFilter()
        {
            DashboardDirector_RevenueFluctuationFilterDTO = new DashboardDirector_RevenueFluctuationFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                ProvinceId = new IdFilter(),
                Time = new IdFilter { Equal = 8 },
            };
            DashboardDirector_RevenueFluctuationDTO = await DashboardDirectorController.RevenueFluctuation(DashboardDirector_RevenueFluctuationFilterDTO);
        }
        private async Task When_UserInputFilter_Director_IndirectSalesOrderFluctuation_ThisYear()
        {
            DashboardDirector_IndirectSalesOrderFluctuationFilterDTO = new DashboardDirector_IndirectSalesOrderFluctuationFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                ProvinceId = new IdFilter(),
                Time = new IdFilter { Equal = 8 },
            };
            DashboardDirector_IndirectSalesOrderFluctuationDTO = await DashboardDirectorController.IndirectSalesOrderFluctuation(DashboardDirector_IndirectSalesOrderFluctuationFilterDTO);
        }
        #endregion
        #region Direct Sales Order
        private async Task When_UserInputFilter_Director_CountDirectSalesOrder_ThisYear()
        {
            DashboardDirector_DirectSalesOrderFluctuationFilterDTO = new DashboardDirector_DirectSalesOrderFluctuationFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                Time = new IdFilter { Equal = 8 },
                ProvinceId = new IdFilter()
            };
            Result = await DashboardDirectorController.CountDirectSalesOrder(DashboardDirector_DirectSalesOrderFluctuationFilterDTO);
        }
        private async Task When_UserInputFilter_Director_DirectRevenueTotal_ThisYear()
        {
            DashboardDirector_SaledItemFluctuationFilterDTO = new DashboardDirector_SaledItemFluctuationFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                Time = new IdFilter { Equal = 8 },
                ProvinceId = new IdFilter()
            };
            DecimalResult = await DashboardDirectorController.DirectRevenueTotal(DashboardDirector_SaledItemFluctuationFilterDTO);
        }
        private async Task When_UserInputFilter_Director_DirectStatisticToday()
        {
            DashboardDirector_StoreFilterDTO = new DashboardDirector_StoreFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                ProvinceId = new IdFilter()
            };
            DashboardDirector_StatisticDailyDirectSalesOrderDTO = await DashboardDirectorController.DirectStatisticToday(DashboardDirector_StoreFilterDTO);
        }
        private async Task When_UserInputFilter_Director_DirectStatisticYesterday()
        {
            DashboardDirector_DirectSalesOrderFluctuationFilterDTO = new DashboardDirector_DirectSalesOrderFluctuationFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                ProvinceId = new IdFilter()
            };
            DashboardDirector_StatisticDailyDirectSalesOrderDTO = await DashboardDirectorController.DirectStatisticYesterday(DashboardDirector_DirectSalesOrderFluctuationFilterDTO);
        }
        private async Task When_UserInputFilter_Director_Top5DirectRevenueByEmployee_NoFilter()
        {
            DashboardDirector_Top5RevenueByEmployeeFilterDTO = new DashboardDirector_Top5RevenueByEmployeeFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                ProvinceId = new IdFilter(),
                Time = new IdFilter { Equal = 8 },
            };
            DashboardDirector_Top5RevenueByEmployeeDTOs = await DashboardDirectorController.Top5DirectRevenueByEmployee(DashboardDirector_Top5RevenueByEmployeeFilterDTO);
        }
        private async Task When_UserInputFilter_Director_ListDirectSalesOrder_NoFilter()
        {
            DashboardDirector_DirectSalesOrderFluctuationFilterDTO = new DashboardDirector_DirectSalesOrderFluctuationFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                ProvinceId = new IdFilter(),
            };
            DashboardDirector_DirectSalesOrderDTOs = await DashboardDirectorController.ListDirectSalesOrder(DashboardDirector_DirectSalesOrderFluctuationFilterDTO);
        }
        private async Task When_UserInputFilter_Director_Top5DirectRevenueByProduct_NoFilter()
        {
            DashboardDirector_Top5RevenueByProductFilterDTO = new DashboardDirector_Top5RevenueByProductFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                ProvinceId = new IdFilter(),
                Time = new IdFilter { Equal = 8 },
            };
            DashboardDirector_Top5RevenueByProductDTOs = await DashboardDirectorController.Top5DirectRevenueByProduct(DashboardDirector_Top5RevenueByProductFilterDTO);
        }
        private async Task When_UserInputFilter_Director_DirectRevenueFluctuation_NoFilter()
        {
            DashboardDirector_RevenueFluctuationFilterDTO = new DashboardDirector_RevenueFluctuationFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                ProvinceId = new IdFilter(),
                Time = new IdFilter { Equal = 8 },
            };
            DashboardDirector_RevenueFluctuationDTO = await DashboardDirectorController.DirectRevenueFluctuation(DashboardDirector_RevenueFluctuationFilterDTO);
        }
        private async Task When_UserInputFilter_Director_DirectSalesOrderFluctuation_ThisYear()
        {
            DashboardDirector_DirectSalesOrderFluctuationFilterDTO = new DashboardDirector_DirectSalesOrderFluctuationFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                OrganizationId = new IdFilter(),
                AppUserId = new IdFilter(),
                ProvinceId = new IdFilter(),
                Time = new IdFilter { Equal = 8 },
            };
            DashboardDirector_DirectSalesOrderFluctuationDTO = await DashboardDirectorController.DirectSalesOrderFluctuation(DashboardDirector_DirectSalesOrderFluctuationFilterDTO);
        }
        #endregion
    }
}
