using DMS.Entities;
using DMS.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using DMS.Handlers.Configuration;

namespace DMS.Handlers
{
    public class ProvinceHandler : Handler
    {
        private string SyncKey => Name + ".Sync";
        public override string Name => nameof(Province);

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
                List<Province> Provinces = JsonConvert.DeserializeObject<List<Province>>(json);
                await UOW.ProvinceRepository.BulkMerge(Provinces);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(ProvinceHandler));
            }
        }
    }
}
