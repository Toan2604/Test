using DMS.Entities;
using DMS.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DMS.Handlers.Configuration;

namespace DMS.Handlers
{
    public class ConversationHandler : Handler
    {
        private string SyncKey => Name + ".Sync";
        public override string Name => nameof(Conversation);

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
                List<Conversation> Conversations = JsonConvert.DeserializeObject<List<Conversation>>(json);
                await UOW.ConversationRepository.BulkMerge(Conversations);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(ConversationHandler));
            }
        }
    }
}
