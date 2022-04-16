using DMS.Rpc.dashboards.director;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
namespace DMS.Controllers.dashboards.director
{
    public class DashboardDirectorViewController : RpcViewController
    {
        [Route(DashboardDirectorViewRoute.CountStore), HttpGet]
        public IActionResult CountStore(long? organizationId, long? appUserId, long? provinceId, long? time)
        {
            DashboardDirector_StoreFilterDTO DashboardDirector_StoreFilterDTO = new DashboardDirector_StoreFilterDTO
            {
                AppUserId = new IdFilter { Equal = appUserId },
                OrganizationId = new IdFilter { Equal = organizationId },
                ProvinceId = new IdFilter { Equal = provinceId },
                Time = new IdFilter { Equal = time }
            };

            ViewBag.body = JsonSerialize(DashboardDirector_StoreFilterDTO);
            return View();
        }
        [Route(DashboardDirectorViewRoute.CountIndirectSalesOrder), HttpGet]
        public IActionResult CountIndirectSalesOrder(long? organizationId, long? appUserId, long? provinceId, long? time)
        {
            DashboardDirector_IndirectSalesOrderFluctuationFilterDTO DashboardDirector_IndirectSalesOrderFluctuationFilterDTO = new DashboardDirector_IndirectSalesOrderFluctuationFilterDTO
            {
                AppUserId = new IdFilter { Equal = appUserId },
                OrganizationId = new IdFilter { Equal = organizationId },
                ProvinceId = new IdFilter { Equal = provinceId },
                Time = new IdFilter { Equal = time }
            };

            ViewBag.body = JsonSerialize(DashboardDirector_IndirectSalesOrderFluctuationFilterDTO);
            return View();
        }

        [Route(DashboardDirectorViewRoute.RevenueTotal), HttpGet]
        public IActionResult RevenueTotal(long? organizationId, long? appUserId, long? provinceId, long? time)
        {
            DashboardDirector_SaledItemFluctuationFilterDTO DashboardDirector_SaledItemFluctuationFilterDTO = new DashboardDirector_SaledItemFluctuationFilterDTO
            {
                AppUserId = new IdFilter { Equal = appUserId },
                OrganizationId = new IdFilter { Equal = organizationId },
                ProvinceId = new IdFilter { Equal = provinceId },
                Time = new IdFilter { Equal = time }
            };

            ViewBag.body = JsonSerialize(DashboardDirector_SaledItemFluctuationFilterDTO);
            return View();
        }

        [Route(DashboardDirectorViewRoute.StoreHasCheckedCounter), HttpGet]
        public IActionResult StoreHasCheckedCounter(long? organizationId, long? appUserId, long? provinceId, long? time)
        {
            DashboardDirector_SaledItemFluctuationFilterDTO DashboardDirector_SaledItemFluctuationFilterDTO = new DashboardDirector_SaledItemFluctuationFilterDTO
            {
                AppUserId = new IdFilter { Equal = appUserId },
                OrganizationId = new IdFilter { Equal = organizationId },
                ProvinceId = new IdFilter { Equal = provinceId },
                Time = new IdFilter { Equal = time }
            };

            ViewBag.body = JsonSerialize(DashboardDirector_SaledItemFluctuationFilterDTO);
            return View();
        }

        [Route(DashboardDirectorViewRoute.CountStoreChecking), HttpGet]
        public IActionResult CountStoreChecking(long? organizationId, long? appUserId, long? provinceId, long? time)
        {
            DashboardDirector_StoreFilterDTO DashboardDirector_StoreFilterDTO = new DashboardDirector_StoreFilterDTO
            {
                AppUserId = new IdFilter { Equal = appUserId },
                OrganizationId = new IdFilter { Equal = organizationId },
                ProvinceId = new IdFilter { Equal = provinceId },
                Time = new IdFilter { Equal = time }
            };

            ViewBag.body = JsonSerialize(DashboardDirector_StoreFilterDTO);
            return View();
        }
        [Route(DashboardDirectorViewRoute.StatisticToday), HttpGet]
        public IActionResult StatisticToday(long? organizationId, long? appUserId, long? provinceId, long? time)
        {
            DashboardDirector_StoreFilterDTO DashboardDirector_StoreFilterDTO = new DashboardDirector_StoreFilterDTO
            {
                AppUserId = new IdFilter { Equal = appUserId },
                OrganizationId = new IdFilter { Equal = organizationId },
                ProvinceId = new IdFilter { Equal = provinceId },
                Time = new IdFilter { Equal = time }
            };

            ViewBag.body = JsonSerialize(DashboardDirector_StoreFilterDTO);
            return View();
        }

        [Route(DashboardDirectorViewRoute.StatisticYesterday), HttpGet]
        public IActionResult StatisticYesterday(long? organizationId, long? appUserId, long? provinceId, long? time)
        {
            DashboardDirector_IndirectSalesOrderFluctuationFilterDTO DashboardDirector_IndirectSalesOrderFluctuationFilterDTO = new DashboardDirector_IndirectSalesOrderFluctuationFilterDTO
            {
                AppUserId = new IdFilter { Equal = appUserId },
                OrganizationId = new IdFilter { Equal = organizationId },
                ProvinceId = new IdFilter { Equal = provinceId },
                Time = new IdFilter { Equal = time }
            };

            ViewBag.body = JsonSerialize(DashboardDirector_IndirectSalesOrderFluctuationFilterDTO);
            return View();
        }

        [Route(DashboardDirectorViewRoute.StoreCoverage), HttpGet]
        public IActionResult StoreCoverage(long? organizationId, long? appUserId, long? provinceId, long? time)
        {
            DashboardDirector_StoreFilterDTO DashboardDirector_StoreFilterDTO = new DashboardDirector_StoreFilterDTO
            {
                AppUserId = new IdFilter { Equal = appUserId },
                OrganizationId = new IdFilter { Equal = organizationId },
                ProvinceId = new IdFilter { Equal = provinceId },
                Time = new IdFilter { Equal = time }
            };

            ViewBag.body = JsonSerialize(DashboardDirector_StoreFilterDTO);
            return View();
        }

        [Route(DashboardDirectorViewRoute.SaleEmployeeLocation), HttpGet]
        public IActionResult SaleEmployeeLocation(long? organizationId, long? appUserId, long? provinceId, long? time)
        {
            DashboardDirector_StoreFilterDTO DashboardDirector_StoreFilterDTO = new DashboardDirector_StoreFilterDTO
            {
                AppUserId = new IdFilter { Equal = appUserId },
                OrganizationId = new IdFilter { Equal = organizationId },
                ProvinceId = new IdFilter { Equal = provinceId },
                Time = new IdFilter { Equal = time }
            };

            ViewBag.body = JsonSerialize(DashboardDirector_StoreFilterDTO);
            return View();
        }

        [Route(DashboardDirectorViewRoute.ListIndirectSalesOrder), HttpGet]
        public IActionResult ListIndirectSalesOrder(long? organizationId, long? appUserId, long? provinceId, long? time)
        {
            DashboardDirector_IndirectSalesOrderFluctuationFilterDTO DashboardDirector_IndirectSalesOrderFluctuationFilterDTO = new DashboardDirector_IndirectSalesOrderFluctuationFilterDTO
            {
                AppUserId = new IdFilter { Equal = appUserId },
                OrganizationId = new IdFilter { Equal = organizationId },
                ProvinceId = new IdFilter { Equal = provinceId },
                Time = new IdFilter { Equal = time }
            };

            ViewBag.body = JsonSerialize(DashboardDirector_IndirectSalesOrderFluctuationFilterDTO);
            return View();
        }

        [Route(DashboardDirectorViewRoute.Top5RevenueByProduct), HttpGet]
        public IActionResult Top5RevenueByProduct(long? organizationId, long? appUserId, long? provinceId, long? time)
        {
            DashboardDirector_Top5RevenueByProductFilterDTO DashboardDirector_Top5RevenueByProductFilterDTO = new DashboardDirector_Top5RevenueByProductFilterDTO
            {
                AppUserId = new IdFilter { Equal = appUserId },
                OrganizationId = new IdFilter { Equal = organizationId },
                ProvinceId = new IdFilter { Equal = provinceId },
                Time = new IdFilter { Equal = time }
            };

            ViewBag.body = JsonSerialize(DashboardDirector_Top5RevenueByProductFilterDTO);
            return View();
        }

        [Route(DashboardDirectorViewRoute.Top5RevenueByEmployee), HttpGet]
        public IActionResult Top5RevenueByEmployee(long? organizationId, long? appUserId, long? provinceId, long? time)
        {
            DashboardDirector_Top5RevenueByEmployeeFilterDTO DashboardDirector_Top5RevenueByEmployeeFilterDTO = new DashboardDirector_Top5RevenueByEmployeeFilterDTO
            {
                AppUserId = new IdFilter { Equal = appUserId },
                OrganizationId = new IdFilter { Equal = organizationId },
                ProvinceId = new IdFilter { Equal = provinceId },
                Time = new IdFilter { Equal = time }
            };

            ViewBag.body = JsonSerialize(DashboardDirector_Top5RevenueByEmployeeFilterDTO);
            return View();
        }

        [Route(DashboardDirectorViewRoute.RevenueFluctuation), HttpGet]
        public IActionResult RevenueFluctuation(long? organizationId, long? appUserId, long? provinceId, long? time)
        {
            DashboardDirector_RevenueFluctuationFilterDTO DashboardDirector_RevenueFluctuationFilterDTO = new DashboardDirector_RevenueFluctuationFilterDTO
            {
                AppUserId = new IdFilter { Equal = appUserId },
                OrganizationId = new IdFilter { Equal = organizationId },
                ProvinceId = new IdFilter { Equal = provinceId },
                Time = new IdFilter { Equal = time }
            };

            ViewBag.body = JsonSerialize(DashboardDirector_RevenueFluctuationFilterDTO);
            return View();
        }

        [Route(DashboardDirectorViewRoute.IndirectSalesOrderFluctuation), HttpGet]
        public IActionResult IndirectSalesOrderFluctuation(long? organizationId, long? appUserId, long? provinceId, long? time)
        {
            DashboardDirector_IndirectSalesOrderFluctuationFilterDTO DashboardDirector_IndirectSalesOrderFluctuationFilterDTO = new DashboardDirector_IndirectSalesOrderFluctuationFilterDTO
            {
                AppUserId = new IdFilter { Equal = appUserId },
                OrganizationId = new IdFilter { Equal = organizationId },
                ProvinceId = new IdFilter { Equal = provinceId },
                Time = new IdFilter { Equal = time }
            };

            ViewBag.body = JsonSerialize(DashboardDirector_IndirectSalesOrderFluctuationFilterDTO);
            return View();
        }

        [Route(DashboardDirectorViewRoute.SaledItemFluctuation), HttpGet]
        public IActionResult SaledItemFluctuation(long? organizationId, long? appUserId, long? provinceId, long? time)
        {
            DashboardDirector_SaledItemFluctuationFilterDTO DashboardDirector_SaledItemFluctuationFilterDTO = new DashboardDirector_SaledItemFluctuationFilterDTO
            {
                AppUserId = new IdFilter { Equal = appUserId },
                OrganizationId = new IdFilter { Equal = organizationId },
                ProvinceId = new IdFilter { Equal = provinceId },
                Time = new IdFilter { Equal = time }
            };

            ViewBag.body = JsonSerialize(DashboardDirector_SaledItemFluctuationFilterDTO);
            return View();
        }

    }
}
