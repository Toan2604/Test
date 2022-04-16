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

namespace DMS.Handlers
{
    public class ShowingCategoryHandler : Handler
    {
        private string UsedKey => $"{Name}.Used";
        public override string Name => nameof(ShowingCategoryHandler);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {
            IUOW UOW = ServiceProvider.GetService<IUOW>();
            if (routingKey == UsedKey)
                await Used(UOW, content);
        }

        private async Task Used(IUOW UOW, string json)
        {
            try
            {
                List<ShowingCategory> ShowingCategory = JsonConvert.DeserializeObject<List<ShowingCategory>>(json);
                List<long> Ids = ShowingCategory.Select(a => a.Id).ToList();
                await UOW.ShowingCategoryRepository.Used(Ids);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(ShowingCategoryHandler));
            }
        }
    }
}
