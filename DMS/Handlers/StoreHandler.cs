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
using DMS.Services.MStore;

namespace DMS.Handlers
{
    public class StoreHandler : Handler
    {
        private string UsedKey => $"{Name}.Used";
        private string CreateKey => $"{Name}.Create";
        public override string Name => nameof(Store);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {

            IUOW UOW = ServiceProvider.GetService<IUOW>();
            IStoreService StoreService = ServiceProvider.GetService<IStoreService>();
            if (routingKey == UsedKey)
                await Used(UOW, content);
            else if (routingKey == CreateKey)
                await Create(StoreService, content);
        }

        private async Task Used(IUOW UOW, string json)
        {
            try
            {
                List<Store> Store = JsonConvert.DeserializeObject<List<Store>>(json);
                List<long> Ids = Store.Select(a => a.Id).ToList();
                await UOW.StoreRepository.Used(Ids);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(StoreHandler));
            }
        }

        private async Task Create(IStoreService StoreService, string json)
        {
            try
            {
                List<Store> Stores = JsonConvert.DeserializeObject<List<Store>>(json);
                await StoreService.BulkMerge(Stores);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(StoreHandler));
            }
        }
    }
}
