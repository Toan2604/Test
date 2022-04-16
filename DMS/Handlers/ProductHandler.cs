using DMS.Entities;
using DMS.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using DMS.Handlers.Configuration;
using DMS.Services.MProduct;
using System.Linq;

namespace DMS.Handlers
{
    public class ProductHandler : Handler
    {
        private string SyncKey => Name + ".Sync";

        public override string Name => nameof(Product);

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
                List<Product> Products = JsonConvert.DeserializeObject<List<Product>>(json);
                await UOW.ProductRepository.BulkMerge(Products);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(ProductHandler));
            }
        }
    }

    public class ProductOrderCountHandler : Handler
    {
        private string SyncKey => Name + ".Cal";

        public override string Name => nameof(Product);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {
            IProductService ProductService = ServiceProvider.GetService<IProductService>();
            if (routingKey == SyncKey)
                await Sync(ProductService, content);
        }

        private async Task Sync(IProductService ProductService, string json)
        {
            try
            {
                List<Product> Products = JsonConvert.DeserializeObject<List<Product>>(json);
                List<long> ProductIds = Products.Select(x => x.Id).ToList();
                await ProductService.SalesOrderCalculator(ProductIds);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(ProductHandler));
            }
        }
    }
}
