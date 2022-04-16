using DMS.Entities;
using DMS.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using DMS.Handlers.Configuration;
using DMS.Services.MStoreBalance;

namespace DMS.Handlers
{
    public class StoreBalanceHandler : Handler
    {
        private string CreateKey => $"{Name}.Create";
        public override string Name => nameof(StoreBalance);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {
            IStoreBalanceService StoreBalanceService = ServiceProvider.GetService<IStoreBalanceService>();
            if (routingKey == CreateKey)
                await Create(StoreBalanceService, content);
        }

        private async Task Create(IStoreBalanceService StoreBalanceService, string json)
        {
            try
            {
                List<StoreBalance> StoreBalances = JsonConvert.DeserializeObject<List<StoreBalance>>(json);
                await StoreBalanceService.BulkMerge(StoreBalances);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(StoreBalanceHandler));
            }
        }
    }
}
