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
    public class StoreStatusHistoryHandler : Handler
    {
        private string SyncKey => $"{Name}.Sync";
        public override string Name => nameof(StoreStatusHistory);

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
                List<StoreStatusHistory> StoreStatusHistorys = JsonConvert.DeserializeObject<List<StoreStatusHistory>>(json);
                await UOW.StoreStatusHistoryRepository.BulkMerge(StoreStatusHistorys);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(StoreStatusHistoryHandler));
            }
        }
    }
}
