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
    public class ProblemTypeHandler : Handler
    {
        private string UsedKey => Name + ".Used";
        public override string Name => nameof(ProblemType);

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
                List<ProblemType> ProblemType = JsonConvert.DeserializeObject<List<ProblemType>>(json);
                List<long> Ids = ProblemType.Select(a => a.Id).ToList();
                await UOW.ProblemTypeRepository.Used(Ids);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(ProblemTypeHandler));
            }
        }
    }
}
