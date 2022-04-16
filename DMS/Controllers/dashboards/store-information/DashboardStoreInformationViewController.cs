using DMS.Rpc.dashboards.store_information;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Controllers.dashboards.store_information
{
    public partial class DashboardStoreInformationViewController : RpcViewController
    {
        [Route(DashboardStoreInformationViewRoute.EstimatedRevenueStatistic), HttpPost]
        public IActionResult EstimatedRevenueStatistic(long? organizationId, long? appUserId, long? provinceId, long? dictrictId, long? estimatedRevenueId, DateTime? orderDate)
        {
            DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO = new DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO
            {
                OrganizationId = new IdFilter { Equal = organizationId },
                AppUserId = new IdFilter { Equal = appUserId },
                EstimatedRevenueId = new IdFilter { Equal = estimatedRevenueId },
                ProvinceId = new IdFilter { Equal = provinceId },
                DistrictId = new IdFilter { Equal = dictrictId },
                OrderDate = new DateFilter { LessEqual = orderDate },
            };

            ViewBag.body = JsonSerialize(DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO);
            return View();
        }

        [Route(DashboardStoreInformationViewRoute.StoreCounter), HttpPost]
        public IActionResult StoreCounter(long? organizationId, long? appUserId, long? provinceId, long? dictrictId, long? brandId, DateTime? orderDate)
        {
            DashboardStoreInformation_StoreCounterFilterDTO DashboardStoreInformation_StoreCounterFilterDTO = new DashboardStoreInformation_StoreCounterFilterDTO
            {
                OrganizationId = new IdFilter { Equal = organizationId },
                AppUserId = new IdFilter { Equal = appUserId },
                BrandId = new IdFilter { Equal = brandId },
                ProvinceId = new IdFilter { Equal = provinceId },
                DistrictId = new IdFilter { Equal = dictrictId },
                OrderDate = new DateFilter { LessEqual = orderDate },
            };

            ViewBag.body = JsonSerialize(DashboardStoreInformation_StoreCounterFilterDTO);
            return View();
        }

        [Route(DashboardStoreInformationViewRoute.BrandStatistic), HttpPost]
        public IActionResult BrandStatistic(long? organizationId, long? appUserId, long? provinceId, long? dictrictId, long? brandId, DateTime? orderDate)
        {
            DashboardStoreInformation_BrandStatisticsFilterDTO DashboardStoreInformation_BrandStatisticsFilterDTO = new DashboardStoreInformation_BrandStatisticsFilterDTO
            {
                OrganizationId = new IdFilter { Equal = organizationId },
                AppUserId = new IdFilter { Equal = appUserId },
                BrandId = new IdFilter { Equal = brandId },
                ProvinceId = new IdFilter { Equal = provinceId },
                DistrictId = new IdFilter { Equal = dictrictId },
                OrderDate = new DateFilter { LessEqual = orderDate },
            };

            ViewBag.body = JsonSerialize(DashboardStoreInformation_BrandStatisticsFilterDTO);
            return View();
        }

        [Route(DashboardStoreInformationViewRoute.BrandUnStatistic), HttpPost]
        public IActionResult BrandUnStatistic(long? organizationId, long? appUserId, long? provinceId, long? dictrictId, long? brandId, DateTime? orderDate)
        {
            DashboardStoreInformation_BrandStatisticsFilterDTO DashboardStoreInformation_BrandStatisticsFilterDTO = new DashboardStoreInformation_BrandStatisticsFilterDTO
            {
                OrganizationId = new IdFilter { Equal = organizationId },
                AppUserId = new IdFilter { Equal = appUserId },
                BrandId = new IdFilter { Equal = brandId },
                ProvinceId = new IdFilter { Equal = provinceId },
                DistrictId = new IdFilter { Equal = dictrictId },
                OrderDate = new DateFilter { LessEqual = orderDate },
            };

            ViewBag.body = JsonSerialize(DashboardStoreInformation_BrandStatisticsFilterDTO);
            return View();
        }

        [Route(DashboardStoreInformationViewRoute.StoreCoverage), HttpPost]
        public IActionResult StoreCoverage(long? organizationId, long? appUserId, long? provinceId, long? dictrictId, long? brandId, DateTime? orderDate)
        {
            DashboardStoreInformation_StoreFilterDTO DashboardStoreInformation_StoreFilterDTO = new DashboardStoreInformation_StoreFilterDTO
            {
                OrganizationId = new IdFilter { Equal = organizationId },
                AppUserId = new IdFilter { Equal = appUserId },
                BrandId = new IdFilter { Equal = brandId },
                ProvinceId = new IdFilter { Equal = provinceId },
                DistrictId = new IdFilter { Equal = dictrictId },
                OrderDate = new DateFilter { LessEqual = orderDate },
            };

            ViewBag.body = JsonSerialize(DashboardStoreInformation_StoreFilterDTO);
            return View();
        }

        [Route(DashboardStoreInformationViewRoute.TopBrand), HttpPost]
        public IActionResult TopBrand(long? organizationId, long? appUserId, long? provinceId, long? dictrictId, long? brandId, DateTime? orderDate, long? top)
        {
            DashboardStoreInformation_TopBrandFilterDTO DashboardStoreInformation_TopBrandFilterDTO = new DashboardStoreInformation_TopBrandFilterDTO
            {
                OrganizationId = new IdFilter { Equal = organizationId },
                AppUserId = new IdFilter { Equal = appUserId },
                BrandId = new IdFilter { Equal = brandId },
                ProvinceId = new IdFilter { Equal = provinceId },
                DistrictId = new IdFilter { Equal = dictrictId },
                OrderDate = new DateFilter { LessEqual = orderDate },
                Top = new LongFilter { Equal = top }
            };

            ViewBag.body = JsonSerialize(DashboardStoreInformation_TopBrandFilterDTO);
            return View();
        }
        [Route(DashboardStoreInformationViewRoute.ProductGroupingNumberStatistic), HttpPost]
        public IActionResult ProductGroupingNumberStatistic(long? organizationId, long? appUserId, long? provinceId, long? dictrictId, long? brandId, DateTime? orderDate)
        {
            DashboardStoreInformation_ProductGroupingNumberStatisticsFilterDTO DashboardStoreInformation_ProductGroupingNumberStatisticsFilterDTO = new DashboardStoreInformation_ProductGroupingNumberStatisticsFilterDTO
            {
                OrganizationId = new IdFilter { Equal = organizationId },
                AppUserId = new IdFilter { Equal = appUserId },
                BrandId = new IdFilter { Equal = brandId },
                ProvinceId = new IdFilter { Equal = provinceId },
                DistrictId = new IdFilter { Equal = dictrictId },
                OrderDate = new DateFilter { LessEqual = orderDate },
            };

            ViewBag.body = JsonSerialize(DashboardStoreInformation_ProductGroupingNumberStatisticsFilterDTO);
            return View();
        }

        [Route(DashboardStoreInformationViewRoute.ProductGroupingStatistic), HttpPost]
        public IActionResult ProductGroupingStatistic(long? organizationId, long? appUserId, long? provinceId, long? dictrictId, long? brandId, DateTime? orderDate)
        {
            DashboardStoreInformation_ProductGroupingStatisticsFilterDTO DashboardStoreInformation_ProductGroupingStatisticsFilterDTO = new DashboardStoreInformation_ProductGroupingStatisticsFilterDTO
            {
                OrganizationId = new IdFilter { Equal = organizationId },
                AppUserId = new IdFilter { Equal = appUserId },
                BrandId = new IdFilter { Equal = brandId },
                ProvinceId = new IdFilter { Equal = provinceId },
                DistrictId = new IdFilter { Equal = dictrictId },
                OrderDate = new DateFilter { LessEqual = orderDate },
            };

            ViewBag.body = JsonSerialize(DashboardStoreInformation_ProductGroupingStatisticsFilterDTO);
            return View();
        }

    }
}
