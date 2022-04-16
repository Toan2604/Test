using DMS.Entities;
using DMS.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using DMS.Handlers.Configuration;
using DMS.Services.MWarehouse;
using DMS.Common;

namespace DMS.Handlers
{
    public class WarehouseHandler : Handler
    {
        private string CheckOrderKey => Name + ".CheckOrder";
        private string CreateKey => Name + ".Create";
        public override string Name => nameof(Warehouse);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {
            IWarehouseService WarehouseService = ServiceProvider.GetService<IWarehouseService>();
            ICurrentContext CurrentContext = ServiceProvider.GetService<ICurrentContext>();
            CurrentContext.UserId = 1;
            if (routingKey == CheckOrderKey)
                await CheckOrder(WarehouseService, content);
            if (routingKey == CreateKey)
                await Create(WarehouseService, content);
        }

        private async Task CheckOrder(IWarehouseService WarehouseService, string json)
        {
            try
            {
                List<Inventory> Inventories = JsonConvert.DeserializeObject<List<Inventory>>(json);
                await WarehouseService.CheckInventoryStateOrder(Inventories);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(WarehouseHandler));
            }
        }
        private async Task Create(IWarehouseService WarehouseService, string json)
        {
            try
            {
                List<Warehouse> Warehouses = JsonConvert.DeserializeObject<List<Warehouse>>(json);
                await WarehouseService.BulkMerge(Warehouses);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(WarehouseHandler));
            }
        }
    }
}
