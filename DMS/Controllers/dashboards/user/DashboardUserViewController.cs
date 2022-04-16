using DMS.Common;
using DMS.DWModels;
using DMS.Helpers;
using DMS.Models;
using DMS.Rpc.dashboards.user;
using DMS.Services.MAppUser;
using DMS.Services.MIndirectSalesOrder;
using DMS.Services.MOrganization;
using DMS.Services.MStoreChecking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Controllers.dashboards.user
{
    public partial class DashboardUserViewController : RpcViewController
    {
        [Route(DashboardUserViewRoute.ListIndirectSalesOrder), HttpPost]
        public IActionResult ListIndirectSalesOrder()
        {
            return View();
        }

        [Route(DashboardUserViewRoute.ListComment), HttpPost]
        public IActionResult ListComment()
        {
            return View();
        }
        [Route(DashboardUserViewRoute.SalesQuantity), HttpPost]
        public IActionResult IndirectSalesQuantity(long? time)
        {
            DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO = new DashboardUser_DashboardUserFilterDTO
            {
                Time = new IdFilter { Equal = time }
            };
            ViewBag.body = JsonSerialize(DashboardUser_DashboardUserFilterDTO);
            return View();
        }

        [Route(DashboardUserViewRoute.StoreChecking), HttpPost]
        public IActionResult StoreChecking(long? time)
        {
            DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO = new DashboardUser_DashboardUserFilterDTO
            {
                Time = new IdFilter { Equal = time }
            };
            ViewBag.body = JsonSerialize(DashboardUser_DashboardUserFilterDTO);
            return View();
        }

        [Route(DashboardUserViewRoute.Revenue), HttpPost]
        public IActionResult IndirectRevenue(long? time)
        {
            DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO = new DashboardUser_DashboardUserFilterDTO
            {
                Time = new IdFilter { Equal = time }
            };
            ViewBag.body = JsonSerialize(DashboardUser_DashboardUserFilterDTO);
            return View();
        }

        [Route(DashboardUserViewRoute.StatisticIndirectSalesOrder), HttpPost]
        public IActionResult StatisticIndirectSalesOrder(long? time)
        {
            DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO = new DashboardUser_DashboardUserFilterDTO
            {
                Time = new IdFilter { Equal = time }
            };
            ViewBag.body = JsonSerialize(DashboardUser_DashboardUserFilterDTO);
            return View();
        }

        [Route(DashboardUserViewRoute.ListDirectSalesOrder), HttpPost]
        public IActionResult ListDirectSalesOrder()
        {
            return View();
        }
        [Route(DashboardUserViewRoute.DirectSalesQuantity), HttpPost]
        public IActionResult SalesQuantity(long? time)
        {
            DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO = new DashboardUser_DashboardUserFilterDTO
            {
                Time = new IdFilter { Equal = time }
            };
            ViewBag.body = JsonSerialize(DashboardUser_DashboardUserFilterDTO);
            return View();
        }

        [Route(DashboardUserViewRoute.DirectRevenue), HttpPost]
        public IActionResult Revenue(long? time)
        {
            DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO = new DashboardUser_DashboardUserFilterDTO
            {
                Time = new IdFilter { Equal = time }
            };
            ViewBag.body = JsonSerialize(DashboardUser_DashboardUserFilterDTO);
            return View();
        }

        [Route(DashboardUserViewRoute.StatisticDirectSalesOrder), HttpPost]
        public IActionResult StatisticDirectSalesOrder(long? time)
        {
            DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO = new DashboardUser_DashboardUserFilterDTO
            {
                Time = new IdFilter { Equal = time }
            };
            ViewBag.body = JsonSerialize(DashboardUser_DashboardUserFilterDTO);
            return View();
        }

    }
}
