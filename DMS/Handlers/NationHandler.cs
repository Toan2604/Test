using DMS.Entities;
using DMS.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DMS.Handlers.Configuration;

namespace DMS.Handlers
{
    public class NationHandler : Handler
    {
        private string SyncKey => $"{Name}.Sync";
        public override string Name => nameof(Nation);

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
        public async Task Sync(IUOW UOW, string json)
        {
            try
            {
                List<Nation> Nations = JsonConvert.DeserializeObject<List<Nation>>(json);
                await UOW.NationRepository.BulkMerge(Nations);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(NationHandler));
            }
        }
    }
}
