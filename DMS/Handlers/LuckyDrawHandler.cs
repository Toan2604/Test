using DMS.Entities;
using DMS.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using DMS.Handlers.Configuration;

namespace DMS.Handlers
{
    public class LuckyDrawHandler : Handler
    {
        private string UsedKey => Name + ".Used";
        public override string Name => nameof(LuckyDraw);

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
                List<LuckyDraw> LuckyDraw = JsonConvert.DeserializeObject<List<LuckyDraw>>(json);
                List<long> Ids = LuckyDraw.Select(a => a.Id).ToList();
                await UOW.LuckyDrawRepository.Used(Ids);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(LuckyDrawHandler));
            }
        }
    }
}
