using DMS.Enums;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;
using System.IO;
using Microsoft.AspNetCore.Http;
using DMS.Rpc.direct_sales_order;
using Microsoft.AspNetCore.Mvc;
using System;
using DMS.Rpc.store_balance;
using DMS.Rpc.store;
using DMS.Rpc.warehouse;

namespace DMS.Tests.Rpc.direct_sales_order
{
    public partial class DirectSalesOrderControllerFeature
    {
        private async Task When_UserInput(string path)
        {
            DirectSalesOrder_DirectSalesOrderDTO = ReadFileFromJson<DirectSalesOrder_DirectSalesOrderDTO>(path);
        }
        private async Task When_UserCreate()
        {
            var Result = await DirectSalesOrderController.Create(DirectSalesOrder_DirectSalesOrderDTO);
            if (Result.Value != null) DirectSalesOrder_DirectSalesOrderDTO = Result.Value;
            if (Result.Result != null) DirectSalesOrder_DirectSalesOrderDTO = (DirectSalesOrder_DirectSalesOrderDTO)((BadRequestObjectResult)Result.Result).Value;
        }
        private async Task When_UserUpdate()
        {
            var Result = await DirectSalesOrderController.Update(DirectSalesOrder_DirectSalesOrderDTO);
            if (Result.Value != null) DirectSalesOrder_DirectSalesOrderDTO = Result.Value;
            if (Result.Result != null) DirectSalesOrder_DirectSalesOrderDTO = (DirectSalesOrder_DirectSalesOrderDTO)((BadRequestObjectResult)Result.Result).Value;
        }
        private async Task When_UserApproval()
        {
            var Result = await DirectSalesOrderController.Approve(DirectSalesOrder_DirectSalesOrderDTO);
            if (Result.Value != null) DirectSalesOrder_DirectSalesOrderDTO = Result.Value;
            if (Result.Result != null) DirectSalesOrder_DirectSalesOrderDTO = (DirectSalesOrder_DirectSalesOrderDTO)((BadRequestObjectResult)Result.Result).Value;
        }
        private async Task When_UserReject()
        {
            var Result = await DirectSalesOrderController.Reject(DirectSalesOrder_DirectSalesOrderDTO);
            if (Result.Value != null) DirectSalesOrder_DirectSalesOrderDTO = Result.Value;
            if (Result.Result != null) DirectSalesOrder_DirectSalesOrderDTO = (DirectSalesOrder_DirectSalesOrderDTO)((BadRequestObjectResult)Result.Result).Value;
        }
        private async Task When_UserFilterByDate()
        {
            DirectSalesOrder_DirectSalesOrderFilterDTO = new DirectSalesOrder_DirectSalesOrderFilterDTO
            {
                Take = int.MaxValue,
                Skip = 0,
                OrderDate = new DateFilter { LessEqual = DateTime.Parse("2021-09-10T16:59:59.999Z"), GreaterEqual = DateTime.Parse("2021-09-08T17:00:00.000Z") }
            };
        }
        private async Task When_UserFilterById(long Id)
        {
            DirectSalesOrder_DirectSalesOrderFilterDTO = new DirectSalesOrder_DirectSalesOrderFilterDTO
            {
                Take = int.MaxValue,
                Skip = 0,
                Id = new IdFilter { Equal = Id }
            };
        }
        private async Task When_UserFilterByTotal()
        {
            DirectSalesOrder_DirectSalesOrderFilterDTO = new DirectSalesOrder_DirectSalesOrderFilterDTO
            {
                Take = int.MaxValue,
                Skip = 0,
                Total = new DecimalFilter { LessEqual = 5_000_000, GreaterEqual = 1_000_000 }
            };
        }
        private async Task When_UserSearch()
        {
            var Result = await DirectSalesOrderController.List(DirectSalesOrder_DirectSalesOrderFilterDTO);
            if (Result.Value != null) DirectSalesOrder_DirectSalesOrderDTOs = Result.Value;
        }
        private async Task When_UserInputStoreBalance(string path)
        {
            StoreBalance_StoreBalanceDTO = ReadFileFromJson<StoreBalance_StoreBalanceDTO>(path);
        }
        private async Task When_UserUpdateStoreBalance()
        {
            await StoreBalanceController.Update(StoreBalance_StoreBalanceDTO);
        }
        private async Task When_UserInputStore(string path)
        {
            Store_StoreDTO = ReadFileFromJson<Store_StoreDTO>(path);
        }
        private async Task When_UserUpdateStore()
        {
            await StoreController.Update(Store_StoreDTO);
        }
        private async Task When_UserInputWarehouse(string path)
        {
            Warehouse_WarehouseDTO = ReadFileFromJson<Warehouse_WarehouseDTO>(path);
        }
        private async Task When_UserUpdateWarehouse()
        {
            await WarehouseController.Update(Warehouse_WarehouseDTO);
        }
    }
}
