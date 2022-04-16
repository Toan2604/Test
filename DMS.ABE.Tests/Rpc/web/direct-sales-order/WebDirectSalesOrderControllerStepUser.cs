using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using DMS.ABE.Entities;
using DMS.ABE.Helpers;
using DMS.ABE.Models;
using DMS.ABE.Enums;
using Newtonsoft.Json;
using DMS.ABE.Rpc.web.direct_sales_order;
using Microsoft.AspNetCore.Mvc;

namespace DMS.ABE.Tests.Rpc.web.direct_sales_order
{
    public partial class WebDirectSalesOrderControllerFeature
    {
        private async Task When_UserInput(string path)
        {
            string Payload = System.IO.File.ReadAllText(path);
            WebDirectSalesOrder_DirectSalesOrderDTO = JsonConvert.DeserializeObject<WebDirectSalesOrder_DirectSalesOrderDTO>(Payload);
        }
        private async Task When_UserCreate()
        {
            var Result = await WebDirectSalesOrderController.Create(WebDirectSalesOrder_DirectSalesOrderDTO);
            if (Result.Value != null) WebDirectSalesOrder_DirectSalesOrderDTO = Result.Value;
            if (Result.Result != null) WebDirectSalesOrder_DirectSalesOrderDTO = (WebDirectSalesOrder_DirectSalesOrderDTO)((BadRequestObjectResult)Result.Result).Value;
        }
        private async Task When_UserUpdate()
        {
            var Result = await WebDirectSalesOrderController.Update(WebDirectSalesOrder_DirectSalesOrderDTO);
            if (Result.Value != null) WebDirectSalesOrder_DirectSalesOrderDTO = Result.Value;
            if (Result.Result != null) WebDirectSalesOrder_DirectSalesOrderDTO = (WebDirectSalesOrder_DirectSalesOrderDTO)((BadRequestObjectResult)Result.Result).Value;
        }
        private async Task When_UserApproval()
        {
            var Result = await WebDirectSalesOrderController.Approve(WebDirectSalesOrder_DirectSalesOrderDTO);
            if (Result.Value != null) WebDirectSalesOrder_DirectSalesOrderDTO = Result.Value;
            if (Result.Result != null) WebDirectSalesOrder_DirectSalesOrderDTO = (WebDirectSalesOrder_DirectSalesOrderDTO)((BadRequestObjectResult)Result.Result).Value;
        }
        private async Task When_UserReject()
        {
            var Result = await WebDirectSalesOrderController.Reject(WebDirectSalesOrder_DirectSalesOrderDTO);
            if (Result.Value != null) WebDirectSalesOrder_DirectSalesOrderDTO = Result.Value;
            if (Result.Result != null) WebDirectSalesOrder_DirectSalesOrderDTO = (WebDirectSalesOrder_DirectSalesOrderDTO)((BadRequestObjectResult)Result.Result).Value;
        }
        private async Task When_UserFilterByDate()
        {
            WebDirectSalesOrder_DirectSalesOrderFilterDTO = new WebDirectSalesOrder_DirectSalesOrderFilterDTO
            {
                Take = int.MaxValue,
                Skip = 0,
                OrderDate = new TrueSight.Common.DateFilter { LessEqual = DateTime.Parse("2021-09-10T16:59:59.999Z"), GreaterEqual = DateTime.Parse("2021-09-08T17:00:00.000Z") }
            };
        }
        private async Task When_UserFilterByTotal()
        {
            WebDirectSalesOrder_DirectSalesOrderFilterDTO = new WebDirectSalesOrder_DirectSalesOrderFilterDTO
            {
                Take = int.MaxValue,
                Skip = 0,
                Total = new TrueSight.Common.DecimalFilter { LessEqual = 5_000_000, GreaterEqual = 1_000_000 }
            };
        }
        private async Task When_UserSearch()
        {
            var Result = await WebDirectSalesOrderController.List(WebDirectSalesOrder_DirectSalesOrderFilterDTO);
            if (Result.Value != null) WebDirectSalesOrder_DirectSalesOrderDTOs = Result.Value;
        }
    }
}
