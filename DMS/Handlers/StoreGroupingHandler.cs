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
using DMS.Services.MStoreGrouping;

namespace DMS.Handlers
{
    public class StoreGroupingHandler : Handler
    {
        private string UsedKey => $"{Name}.Used";
        private string CreateKey => $"{Name}.Create";
        public override string Name => nameof(StoreGrouping);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {

            IUOW UOW = ServiceProvider.GetService<IUOW>();
            IStoreGroupingService StoreGroupingService = ServiceProvider.GetService<IStoreGroupingService>();
            if (routingKey == UsedKey)
                await Used(UOW, content);
            if (routingKey == CreateKey)
                await Create(StoreGroupingService, content);
        }

        private async Task Used(IUOW UOW, string json)
        {
            try
            {
                List<StoreGrouping> StoreGrouping = JsonConvert.DeserializeObject<List<StoreGrouping>>(json);
                List<long> Ids = StoreGrouping.Select(a => a.Id).ToList();
                await UOW.StoreGroupingRepository.Used(Ids);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(StoreGroupingHandler));
            }
        }
        private async Task Create(IStoreGroupingService StoreGroupingService, string json)
        {
            try
            {
                List<StoreGrouping> StoreGroupings = JsonConvert.DeserializeObject<List<StoreGrouping>>(json);
                await StoreGroupingService.BulkMerge(StoreGroupings);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(StoreGroupingHandler));
            }
        }
    }
}
