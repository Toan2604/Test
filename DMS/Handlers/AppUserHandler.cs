using DMS.Entities;
using DMS.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using DMS.Handlers.Configuration;
using System.Linq;

namespace DMS.Handlers
{
    public class AppUserHandler : Handler
    {
        private string SyncKey => Name + ".Sync";
        private string UpdateGPSKey => Name + ".Gps";
        public override string Name => nameof(AppUser);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {
            IUOW UOW = ServiceProvider.GetService<IUOW>();
            if (routingKey == SyncKey)
                await Sync(UOW, content);
            if (routingKey == UpdateGPSKey)
                await UpdateGPS(UOW, content);
        }

        private async Task Sync(IUOW UOW, string json)
        {
            try
            {
                List<AppUser> AppUsers = JsonConvert.DeserializeObject<List<AppUser>>(json);
                await UOW.AppUserRepository.BulkMerge(AppUsers);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(AppUserHandler));
            }
        }
        private async Task UpdateGPS(IUOW UOW, string json)
        {
            try
            {
                List<AppUser> AppUsers = JsonConvert.DeserializeObject<List<AppUser>>(json);
                var AppUser = AppUsers.FirstOrDefault();
                await UOW.AppUserRepository.SimpleUpdate(AppUser);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(AppUserHandler));
            }
        }
    }
}
