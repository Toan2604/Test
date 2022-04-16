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
    public class StoreTypeHandler : Handler
    {
        private string UsedKey => $"{Name}.Used";
        private string CreateKey => $"{Name}.Create";
        public override string Name => nameof(StoreType);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {
            IUOW UOW = ServiceProvider.GetService<IUOW>();
            IStoreTypeService StoreTypeService = ServiceProvider.GetService<IStoreTypeService>();
            if (routingKey == UsedKey)
                await Used(UOW, content);
            if (routingKey == CreateKey)
                await Create(StoreTypeService, content);
        }

        private async Task Used(IUOW UOW, string json)
        {
            try
            {
                List<StoreType> StoreType = JsonConvert.DeserializeObject<List<StoreType>>(json);
                List<long> Ids = StoreType.Select(a => a.Id).ToList();
                await UOW.StoreTypeRepository.Used(Ids);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(StoreTypeHandler));
            }
        }
        private async Task Create(IStoreTypeService StoreTypeService, string json)
        {
            try
            {
                List<StoreType> StoreTypes = JsonConvert.DeserializeObject<List<StoreType>>(json);
                await StoreTypeService.BulkMerge(StoreTypes);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(StoreTypeHandler));
            }
        }
    }
}
