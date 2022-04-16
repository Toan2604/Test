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
    public class WorkflowDefinitionHandler : Handler
    {
        private string UsedKey => $"DMS.{Name}.Used";
        public override string Name => nameof(WorkflowDefinition);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"DMS.{Name}.*", null);
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
                List<WorkflowDefinition> WorkflowDefinition = JsonConvert.DeserializeObject<List<WorkflowDefinition>>(json);
                List<long> Ids = WorkflowDefinition.Select(a => a.Id).ToList();
                await UOW.WorkflowDefinitionRepository.Used(Ids);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(WorkflowDefinitionHandler));
            }
        }
    }
}
