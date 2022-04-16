using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using DMS.Enums;
using DMS.Rpc.store;
using Microsoft.AspNetCore.Http;
using System.IO;
using DMS.Rpc.dashboards.store_information;
using TrueSight.Common;
namespace DMS.Tests.Rpc.dashboard
{
    public partial class DashboardControllerFeature
    {
        #region Store counter
        private async Task When_UserInput_StoreCounter_NoFilter()
        {
            DashboardStoreInformation_StoreCounterFilterDTO = new DashboardStoreInformation_StoreCounterFilterDTO
            {
                OrganizationId = null,
                BrandId = null,
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue
            };
        }
        private async Task When_UserInput_StoreCounter_WithFilter()
        {
            DashboardStoreInformation_StoreCounterFilterDTO = new DashboardStoreInformation_StoreCounterFilterDTO
            {
                OrganizationId = new IdFilter { Equal = 213 },
                BrandId = new IdFilter { Equal = 51 },
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue
            };
        }
        private async Task When_UserInput_StoreCounter_WithFilter_OrderDate()
        {
            DashboardStoreInformation_StoreCounterFilterDTO = new DashboardStoreInformation_StoreCounterFilterDTO
            {
                OrganizationId = null,
                BrandId = null,
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue,
                OrderDate = new DateFilter { LessEqual = new DateTime(2021, 10, 6).AddSeconds(-1).AddHours(-7) }
            };
        }
        private async Task Get_Store_Counter()
        {
            DashboardStoreInformation_StoreCounterDTO = await DashboardStoreInformationController.StoreCounter(DashboardStoreInformation_StoreCounterFilterDTO);
        }
        #endregion

        #region Brand statistics
        private async Task When_UserInput_BrandStatistics_NoFilter()
        {
            DashboardStoreInformation_BrandStatisticsFilterDTO = new DashboardStoreInformation_BrandStatisticsFilterDTO
            {
                OrganizationId = null,
                BrandId = null,
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue
            };
        }
        private async Task When_UserInput_BrandStatistics_WithFilter()
        {
            DashboardStoreInformation_BrandStatisticsFilterDTO = new DashboardStoreInformation_BrandStatisticsFilterDTO
            {
                OrganizationId = new IdFilter { Equal = 213 },
                BrandId = new IdFilter { Equal = 51 },
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue
            };
        }
        private async Task When_UserInput_BrandStatistics_WithFilter_OrderDate()
        {
            DashboardStoreInformation_BrandStatisticsFilterDTO = new DashboardStoreInformation_BrandStatisticsFilterDTO
            {
                OrganizationId = null,
                BrandId = null,
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue,
                OrderDate = new DateFilter { LessEqual = new DateTime(2021, 10, 6).AddSeconds(-1).AddHours(-7) }
            };
        }
        private async Task Get_BrandStatistics()
        {
            DashboardStoreInformation_BrandStatisticsDTOs = await DashboardStoreInformationController.BrandStatistic(DashboardStoreInformation_BrandStatisticsFilterDTO);
        }
        #endregion

        #region Brand unstatistics
        private async Task When_UserInput_BrandUnStatistics_NoFilter()
        {
            DashboardStoreInformation_BrandUnStatisticsFilterDTO = new DashboardStoreInformation_BrandStatisticsFilterDTO
            {
                OrganizationId = null,
                BrandId = null,
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue
            };
        }
        private async Task When_UserInput_BrandUnStatistics_WithFilter()
        {
            DashboardStoreInformation_BrandUnStatisticsFilterDTO = new DashboardStoreInformation_BrandStatisticsFilterDTO
            {
                OrganizationId = new IdFilter { Equal = 213 },
                BrandId = new IdFilter { Equal = 51 },
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue
            };
        }
        private async Task When_UserInput_BrandUnStatistics_WithFilter_OrderDate()
        {
            DashboardStoreInformation_BrandUnStatisticsFilterDTO = new DashboardStoreInformation_BrandStatisticsFilterDTO
            {
                OrganizationId = null,
                BrandId = null,
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue,
                OrderDate = new DateFilter { LessEqual = new DateTime(2021, 10, 6).AddSeconds(-1).AddHours(-7) }
            };
        }
        private async Task Get_BrandUnStatistics()
        {
            DashboardStoreInformation_BrandUnStatisticsDTOs = await DashboardStoreInformationController.BrandUnStatistic(DashboardStoreInformation_BrandUnStatisticsFilterDTO);
        }
        #endregion

        #region Store coverage
        private async Task When_UserInput_StoreCoverage_NoFilter()
        {
            DashboardStoreInformation_StoreFilterDTO = new DashboardStoreInformation_StoreFilterDTO
            {
                OrganizationId = null,
                BrandId = null,
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue
            };
        }
        private async Task When_UserInput_StoreCoverage_WithFilter()
        {
            DashboardStoreInformation_StoreFilterDTO = new DashboardStoreInformation_StoreFilterDTO
            {
                OrganizationId = new IdFilter { Equal = 213 },
                BrandId = new IdFilter { Equal = 51 },
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue
            };
        }
        private async Task When_UserInput_StoreCoverage_WithFilter_OrderDate()
        {
            DashboardStoreInformation_StoreFilterDTO = new DashboardStoreInformation_StoreFilterDTO
            {
                OrganizationId = null,
                BrandId = null,
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue,
                OrderDate = new DateFilter { LessEqual = new DateTime(2021, 10, 6).AddSeconds(-1).AddHours(-7) }
            };
        }
        private async Task Get_StoreCoverage()
        {
            DashboardStoreInformation_StoreDTOs = await DashboardStoreInformationController.StoreCoverage(DashboardStoreInformation_StoreFilterDTO);
        }
        #endregion

        #region Product grouping number statistics
        private async Task When_UserInput_ProductGroupingNumberStatistics_NoFilter()
        {
            DashboardStoreInformation_ProductGroupingNumberStatisticsFilterDTO = new DashboardStoreInformation_ProductGroupingNumberStatisticsFilterDTO
            {
                OrganizationId = null,
                BrandId = null,
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue
            };
        }
        private async Task When_UserInput_ProductGroupingNumberStatistics_WithFilter()
        {
            DashboardStoreInformation_ProductGroupingNumberStatisticsFilterDTO = new DashboardStoreInformation_ProductGroupingNumberStatisticsFilterDTO
            {
                OrganizationId = new IdFilter { Equal = 213 },
                BrandId = new IdFilter { Equal = 51 },
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue
            };
        }
        private async Task When_UserInput_ProductGroupingNumberStatistics_WithFilter_OrderDate()
        {
            DashboardStoreInformation_ProductGroupingNumberStatisticsFilterDTO = new DashboardStoreInformation_ProductGroupingNumberStatisticsFilterDTO
            {
                OrganizationId = null,
                BrandId = null,
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue,
                OrderDate = new DateFilter { LessEqual = new DateTime(2021, 10, 6).AddSeconds(-1).AddHours(-7) }
            };
        }
        private async Task Get_ProductGroupingNumberStatistics()
        {
            DashboardStoreInformation_ProductGroupingNumberStatisticsDTOs = await DashboardStoreInformationController.ProductGroupingNumberStatistic(DashboardStoreInformation_ProductGroupingNumberStatisticsFilterDTO);
        }
        #endregion

        #region Product grouping statistics
        private async Task When_UserInput_ProductGroupingStatistics_NoFilter()
        {
            DashboardStoreInformation_ProductGroupingStatisticsFilterDTO = new DashboardStoreInformation_ProductGroupingStatisticsFilterDTO
            {
                OrganizationId = null,
                BrandId = null,
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue
            };
        }
        private async Task When_UserInput_ProductGroupingStatistics_WithFilter()
        {
            DashboardStoreInformation_ProductGroupingStatisticsFilterDTO = new DashboardStoreInformation_ProductGroupingStatisticsFilterDTO
            {
                OrganizationId = new IdFilter { Equal = 213 },
                BrandId = new IdFilter { Equal = 51 },
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue
            };
        }
        private async Task When_UserInput_ProductGroupingStatistics_WithFilter_OrderDate()
        {
            DashboardStoreInformation_ProductGroupingStatisticsFilterDTO = new DashboardStoreInformation_ProductGroupingStatisticsFilterDTO
            {
                OrganizationId = null,
                BrandId = null,
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue,
                OrderDate = new DateFilter { LessEqual = new DateTime(2021, 10, 6).AddSeconds(-1).AddHours(-7) }
            };
        }
        private async Task Get_ProductGroupingStatistics()
        {
            DashboardStoreInformation_ProductGroupingStatisticsDTOs = await DashboardStoreInformationController.ProductGroupingStatistic(DashboardStoreInformation_ProductGroupingStatisticsFilterDTO);
        }
        #endregion

        #region Estimated Revenue statistics
        private async Task When_UserInput_EstimatedRevenueStatistics_NoFilter()
        {
            DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO = new DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO
            {
                OrganizationId = null,
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue
            };
        }
        private async Task When_UserInput_EstimatedRevenueStatistics_WithFilter()
        {
            DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO = new DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO
            {
                OrganizationId = new IdFilter { Equal = 213 },
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue
            };
        }
        private async Task When_UserInput_EstimatedRevenueStatistics_WithFilter_OrderDate()
        {
            DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO = new DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO
            {
                OrganizationId = null,
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue,
                OrderDate = new DateFilter { LessEqual = new DateTime(2021, 10, 6).AddSeconds(-1).AddHours(-7) }
            };
        }
        private async Task Get_EstimatedRevenueStatistics()
        {
            DashboardStoreInformation_EstimatedRevenueStatisticsDTOs = await DashboardStoreInformationController.EstimatedRevenueStatistic(DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO);
        }
        #endregion

        #region Top brand
        private async Task When_UserInput_TopBrand_NoFilter()
        {
            DashboardStoreInformation_TopBrandFilterDTO = new DashboardStoreInformation_TopBrandFilterDTO
            {
                OrganizationId = null,
                BrandId = null,
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue,
            };
        }
        private async Task When_UserInput_TopBrand_WithFilter()
        {
            DashboardStoreInformation_TopBrandFilterDTO = new DashboardStoreInformation_TopBrandFilterDTO
            {
                OrganizationId = new IdFilter { Equal = 213 },
                BrandId = new IdFilter { Equal = 51 },
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue,
            };
        }
        private async Task When_UserInput_TopBrand_WithFilter_OrderDate()
        {
            DashboardStoreInformation_TopBrandFilterDTO = new DashboardStoreInformation_TopBrandFilterDTO
            {
                OrganizationId = null,
                BrandId = null,
                ProvinceId = null,
                DistrictId = null,
                Skip = 0,
                Take = int.MaxValue,
                OrderDate = new DateFilter { LessEqual = new DateTime(2021, 10, 6).AddSeconds(-1).AddHours(-7) }
            };
        }
        private async Task Get_TopBrand()
        {
            DashboardStoreInformation_TopBrandDTOs = await DashboardStoreInformationController.TopBrand(DashboardStoreInformation_TopBrandFilterDTO);
        }
        #endregion
    }
}
