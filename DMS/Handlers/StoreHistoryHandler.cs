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
using DMS.Services.MStoreType;

namespace DMS.Handlers
{
    public class StoreHistoryHandler : Handler
    {
        private string SyncKey => $"{Name}.His";
        public override string Name => nameof(StoreHistory);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {
            IUOW UOW = ServiceProvider.GetService<IUOW>();
            if (routingKey == SyncKey)
                await Sync(UOW, content);
        }

        private async Task Sync(IUOW UOW, string json)
        {
            try
            {
                List<StoreHistory> StoreHistories = JsonConvert.DeserializeObject<List<StoreHistory>>(json);
                await UOW.StoreHistoryRepository.BulkMerge(StoreHistories);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(StoreHistoryHandler));
            }
        }
    }
}
