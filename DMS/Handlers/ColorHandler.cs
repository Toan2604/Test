using DMS.Entities;
using DMS.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using DMS.Handlers.Configuration;

namespace DMS.Handlers
{
    public class ColorHandler : Handler
    {
        private string SyncKey => Name + ".Sync";

        public override string Name => nameof(Color);

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
                List<Color> Colors = JsonConvert.DeserializeObject<List<Color>>(json);
                await UOW.ColorRepository.BulkMerge(Colors);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(ColorHandler));
            }
        }
    }
}
