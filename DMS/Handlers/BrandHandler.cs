using DMS.Entities;
using DMS.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using DMS.Handlers.Configuration;
using DMS.Services.MBrand;

namespace DMS.Handlers
{
    public class BrandHandler : Handler
    {
        private string SyncKey => Name + ".Sync";

        public override string Name => nameof(Brand);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {
            IBrandService BrandService = ServiceProvider.GetService<IBrandService>();
            if (routingKey == SyncKey)
                await Sync(BrandService, content);
        }

        private async Task Sync(IBrandService BrandService, string json)
        {
            try
            {
                List<Brand> Brands = JsonConvert.DeserializeObject<List<Brand>>(json);
                await BrandService.BulkMerge(Brands);
            }
            catch (Exception e)
            {
                Log(e, nameof(BrandHandler));
            }
        }
    }
}
