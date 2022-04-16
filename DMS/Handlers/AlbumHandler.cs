using DMS.Entities;
using DMS.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DMS.Handlers.Configuration;

namespace DMS.Handlers
{
    public class AlbumHandler : Handler
    {
        private string UsedKey => Name + ".Used";
        public override string Name => nameof(Album);

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
                List<Album> Album = JsonConvert.DeserializeObject<List<Album>>(json);
                List<long> Ids = Album.Select(a => a.Id).ToList();
                await UOW.AlbumRepository.Used(Ids);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(AlbumHandler));
            }
        }
    }
}
