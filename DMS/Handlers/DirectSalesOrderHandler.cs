using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using DMS.Handlers.Configuration;
using DMS.Services.MDirectSalesOrder;

namespace DMS.Handlers
{
    public class DirectSalesOrderHandler : Handler
    {
        private string CreateKey => $"{StaticParams.ModuleName}.{Name}.Create";

        private string UpdateKey => $"{StaticParams.ModuleName}.{Name}.Update";
        private string BulkMergeKey => $"{StaticParams.ModuleName}.{Name}.BulkMerge";

        public override string Name => nameof(DirectSalesOrder);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{StaticParams.ModuleName}.{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {
            IUOW UOW = ServiceProvider.GetService<IUOW>();
            IDirectSalesOrderService DirectSalesOrderService = ServiceProvider.GetService<IDirectSalesOrderService>();
            if (routingKey == CreateKey)
                await Create(UOW, content);
            if (routingKey == UpdateKey)
                await Update(UOW, content);
            if (routingKey == BulkMergeKey)
                await BulkMerge(DirectSalesOrderService, content);
        }

        private async Task Create(IUOW UOW, string json)
        {
            try
            {
                IRabbitManager RabbitManager = ServiceProvider.GetService<IRabbitManager>();
                DirectSalesOrder DirectSalesOrder = JsonConvert.DeserializeObject<DirectSalesOrder>(json);
                DirectSalesOrder.RequestStateId = RequestStateEnum.APPROVED.Id; // don hang tao tu ams.abe mac dinh khong co wf -> wf requestState = approved
                DirectSalesOrder.StoreApprovalStateId = StoreApprovalStateEnum.PENDING.Id; // trang thai doi cua hang duyet
                DirectSalesOrder.GeneralApprovalStateId = GeneralApprovalStateEnum.STORE_PENDING.Id;
                DirectSalesOrder.EditedPriceStatusId = EditedPriceStatusEnum.INACTIVE.Id; // don hang tao tu ams.abe mac dinh khong cho sua gia
                DirectSalesOrder.DirectSalesOrderSourceTypeId = DirectSalesOrderSourceTypeEnum.FROM_STORE.Id; // sourceType

                await UOW.DirectSalesOrderRepository.Create(DirectSalesOrder);

                DirectSalesOrder = (await UOW.DirectSalesOrderRepository.List(new List<long> { DirectSalesOrder.Id })).FirstOrDefault();

                await NotifyUsed(DirectSalesOrder);
                RabbitManager.PublishSingle(DirectSalesOrder, RoutingKeyEnum.DirectSalesOrderSync.Code); // đồng bộ lên AMS 
                AuditLog(DirectSalesOrder, new { }, nameof(DirectSalesOrderHandler)); // ghi log
            }
            catch (Exception ex)
            {
                Log(ex, nameof(DirectSalesOrderHandler));
            }
        }

        private async Task Update(IUOW UOW, string json)
        {
            try
            {
                IRabbitManager RabbitManager = ServiceProvider.GetService<IRabbitManager>();
                DirectSalesOrder DirectSalesOrder = JsonConvert.DeserializeObject<DirectSalesOrder>(json);
                var oldData = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
                DirectSalesOrder.OrganizationId = DirectSalesOrder.OrganizationId == 0 ? oldData.OrganizationId : DirectSalesOrder.OrganizationId;
                DirectSalesOrder.BuyerStoreId = DirectSalesOrder.BuyerStoreId == 0 ? oldData.BuyerStoreId : DirectSalesOrder.BuyerStoreId;
                DirectSalesOrder.SaleEmployeeId = DirectSalesOrder.SaleEmployeeId == 0 ? oldData.SaleEmployeeId : DirectSalesOrder.SaleEmployeeId;
                DirectSalesOrder.RequestStateId = DirectSalesOrder.RequestStateId == 0 ? oldData.RequestStateId : DirectSalesOrder.RequestStateId;
                DirectSalesOrder.StoreApprovalStateId = DirectSalesOrder.StoreApprovalStateId == 0 ? oldData.StoreApprovalStateId : DirectSalesOrder.StoreApprovalStateId;
                DirectSalesOrder.GeneralApprovalStateId = DirectSalesOrder.GeneralApprovalStateId == 0 ? oldData.GeneralApprovalStateId : DirectSalesOrder.GeneralApprovalStateId;
                DirectSalesOrder.StoreUserCreatorId = DirectSalesOrder.StoreUserCreatorId == null ? oldData.StoreUserCreatorId : DirectSalesOrder.StoreUserCreatorId; // luu acc cua store tao don hang
                DirectSalesOrder.OrderDate = DirectSalesOrder.OrderDate == null ? oldData.OrderDate : DirectSalesOrder.OrderDate;

                await UOW.DirectSalesOrderRepository.Update(DirectSalesOrder);
                DirectSalesOrder = (await UOW.DirectSalesOrderRepository.List(new List<long> { DirectSalesOrder.Id })).FirstOrDefault();

                await NotifyUsed(DirectSalesOrder);
                RabbitManager.PublishSingle(DirectSalesOrder, RoutingKeyEnum.DirectSalesOrderSync.Code); // đồng bộ lên AMS 
                AuditLog(DirectSalesOrder, oldData, nameof(DirectSalesOrderHandler)); // ghi log
            }
            catch (Exception ex)
            {
                Log(ex, nameof(DirectSalesOrderHandler));
            }
        }

        private async Task NotifyUsed(DirectSalesOrder DirectSalesOrder)
        {
            IRabbitManager RabbitManager = ServiceProvider.GetService<IRabbitManager>();
            List<Item> itemMessages = DirectSalesOrder.DirectSalesOrderContents.Select(x => new Item { Id = x.ItemId }).Distinct().ToList();
            RabbitManager.PublishList(itemMessages, RoutingKeyEnum.ItemUsed.Code);
            List<UnitOfMeasure> UnitOfMeasureMessages = new List<UnitOfMeasure>();
            UnitOfMeasureMessages.AddRange(DirectSalesOrder.DirectSalesOrderContents.Select(x => new UnitOfMeasure { Id = x.PrimaryUnitOfMeasureId }));
            UnitOfMeasureMessages.AddRange(DirectSalesOrder.DirectSalesOrderContents.Select(x => new UnitOfMeasure { Id = x.UnitOfMeasureId }));
            UnitOfMeasureMessages = UnitOfMeasureMessages.Distinct().ToList();
            RabbitManager.PublishList(UnitOfMeasureMessages, RoutingKeyEnum.UnitOfMeasureUsed.Code);
            RabbitManager.PublishSingle(new Store { Id = DirectSalesOrder.BuyerStoreId }, RoutingKeyEnum.StoreUsed.Code);
            RabbitManager.PublishSingle(new StoreUser { Id = DirectSalesOrder.StoreUserCreatorId.Value }, RoutingKeyEnum.StoreUserUsed.Code);
            RabbitManager.PublishSingle(new AppUser { Id = DirectSalesOrder.SaleEmployeeId }, RoutingKeyEnum.AppUserUsed.Code);
        }

        private async Task BulkMerge(IDirectSalesOrderService DirectSalesOrderService, string json)
        {
            try
            {
                List<DirectSalesOrder> DirectSalesOrders = JsonConvert.DeserializeObject<List<DirectSalesOrder>>(json); 
                await DirectSalesOrderService.BulkMerge(DirectSalesOrders);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(DirectSalesOrderHandler));
            }
        }
    }
}
