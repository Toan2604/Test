using DMS.Entities;
using DMS.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using DMS.Handlers.Configuration;
using DMS.Services.MProductGrouping;

namespace DMS.Handlers
{
    public class ProductGroupingHandler : Handler
    {
        private string SyncKey => $"{Name}.Sync";
        public override string Name => nameof(ProductGrouping);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {
            IProductGroupingService ProductGroupingService = ServiceProvider.GetService<IProductGroupingService>();
            if (routingKey == SyncKey)
                await Sync(ProductGroupingService, content);
        }

        private async Task Sync(IProductGroupingService ProductGroupingService, string json)
        {
            try
            {
                List<ProductGrouping> ProductGroupings = JsonConvert.DeserializeObject<List<ProductGrouping>>(json);
                await ProductGroupingService.BulkMerge(ProductGroupings);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(ProductGroupingHandler));
            }
        }
    }
}
