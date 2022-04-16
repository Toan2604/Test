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
using DMS.Services.MPriceList;

namespace DMS.Handlers
{
    public class PriceListHandler : Handler
    {
        private string CreateKey => Name + ".Create";
        public override string Name => nameof(PriceList);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {
            IPriceListService PriceListService = ServiceProvider.GetService<IPriceListService>();
            ICurrentContext CurrentContext = ServiceProvider.GetService<ICurrentContext>();
            CurrentContext.UserId = 1;
            if (routingKey == CreateKey)
                await Create(PriceListService, content);
        }
        private async Task Create(IPriceListService PriceListService, string json)
        {
            try
            {
                List<PriceList> PriceLists = JsonConvert.DeserializeObject<List<PriceList>>(json);
                await PriceListService.BulkMerge(PriceLists);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(PriceListHandler));
            }
        }
    }
}
