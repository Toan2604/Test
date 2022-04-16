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
using DMS.Rpc.dashboards.user;
namespace DMS.Tests.Rpc.dashboard
{
    public partial class DashboardUserControllerFeature
    {
        private async Task When_UserInputFilter_User_SaleQuality_LastMonth()
        {
            DashboardUser_DashboardUserFilterDTO = new DashboardUser_DashboardUserFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                Time = new IdFilter { Equal = 3 }                
                };
            Result = await DashboardUserController.SalesQuantity(DashboardUser_DashboardUserFilterDTO);
        }
        private async Task When_UserInputFilter_User_SaleQuality_ThisWeek()
        {
            DashboardUser_DashboardUserFilterDTO = new DashboardUser_DashboardUserFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                Time = new IdFilter { Equal = 1 }
            };
            Result = await DashboardUserController.SalesQuantity(DashboardUser_DashboardUserFilterDTO);
        }
        private async Task When_UserInputFilter_User_StoreChecking_LastMonth()
        {
            DashboardUser_DashboardUserFilterDTO = new DashboardUser_DashboardUserFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                Time = new IdFilter { Equal = 3 }
            };
            Result = await DashboardUserController.StoreChecking(DashboardUser_DashboardUserFilterDTO);
        }
        private async Task When_UserInputFilter_User_Revenue_LastMonth()
        {
            DashboardUser_DashboardUserFilterDTO = new DashboardUser_DashboardUserFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                Time = new IdFilter { Equal = 3 }
            };
            DecimalResult = await DashboardUserController.Revenue(DashboardUser_DashboardUserFilterDTO);
        }
        private async Task When_UserInputFilter_User_StatisticIndirectSalesOrder_LastMonth()
        {
            DashboardUser_DashboardUserFilterDTO = new DashboardUser_DashboardUserFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                Time = new IdFilter { Equal = 3 }
            };
            Result = await DashboardUserController.StatisticIndirectSalesOrder(DashboardUser_DashboardUserFilterDTO);
        }
        private async Task When_UserInputFilter_User_DirectSaleQuality_LastMonth()
        {
            DashboardUser_DashboardUserFilterDTO = new DashboardUser_DashboardUserFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                Time = new IdFilter { Equal = 3 }
            };
            Result = await DashboardUserController.SalesQuantity(DashboardUser_DashboardUserFilterDTO);
        }
        private async Task When_UserInputFilter_User_DirectSaleQuality_ThisWeek()
        {
            DashboardUser_DashboardUserFilterDTO = new DashboardUser_DashboardUserFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                Time = new IdFilter { Equal = 1 }
            };
            Result = await DashboardUserController.SalesQuantity(DashboardUser_DashboardUserFilterDTO);
        }
        private async Task When_UserInputFilter_User_DirectRevenue_LastMonth()
        {
            DashboardUser_DashboardUserFilterDTO = new DashboardUser_DashboardUserFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                Time = new IdFilter { Equal = 3 }
            };
            DecimalResult = await DashboardUserController.Revenue(DashboardUser_DashboardUserFilterDTO);
        }
        private async Task When_UserInputFilter_User_StatisticDirectSalesOrder_LastMonth()
        {
            DashboardUser_DashboardUserFilterDTO = new DashboardUser_DashboardUserFilterDTO
            {
                Skip = 0,
                Take = int.MaxValue,
                Time = new IdFilter { Equal = 3 }
            };
            Result = await DashboardUserController.StatisticDirectSalesOrder(DashboardUser_DashboardUserFilterDTO);
        }
    }
}
