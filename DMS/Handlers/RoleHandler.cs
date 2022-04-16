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
    public class RoleHandler : Handler
    {
        private string UsedKey => Name + ".Used";
        private string SyncKey => Name + ".Sync";
        public override string Name => nameof(Role);

        public override void QueueBind(IModel channel, string queue, string exchange)
        {
            channel.QueueBind(queue, exchange, $"{Name}.*", null);
        }
        public override async Task Handle(string routingKey, string content)
        {
            IUOW UOW = ServiceProvider.GetService<IUOW>();
            if (routingKey == UsedKey)
                await Used(UOW, content);
            if (routingKey == SyncKey)
                await Sync(UOW, content);
        }
        private async Task Used(IUOW UOW, string json)
        {
            try
            {
                List<Role> Role = JsonConvert.DeserializeObject<List<Role>>(json);
                List<long> Ids = Role.Select(a => a.Id).ToList();
                await UOW.RoleRepository.Used(Ids);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(RoleHandler));
            }
        }
        private async Task Sync(IUOW UOW, string json)
        {
            try
            {
                List<Role> Roles = JsonConvert.DeserializeObject<List<Role>>(json);
                Roles = Roles.Where(x => x.Site?.Name == "DMS").ToList();
                List<Permission> Permissions = Roles.SelectMany(x => x.Permissions).ToList();
                await UOW.RoleRepository.BulkMerge(Roles);
                await UOW.PermissionRepository.BulkMerge(Permissions);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(RoleHandler));
            }
        }
    }
}
