using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DMS.ABE.Handlers.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DMS.ABE.Handlers.Configuration
{
    public interface IHandler
    {
        string Name { get; }
        IServiceProvider ServiceProvider { get; set; }
        void QueueBind(IModel channel, string queue, string exchange);
        Task Handle(string routingKey, string content);
    }

    public abstract class Handler : IHandler
    {
        public abstract string Name { get; }
        public IServiceProvider ServiceProvider { get; set; }

        public abstract Task Handle(string routingKey, string content);

        public abstract void QueueBind(IModel channel, string queue, string exchange);

        protected void Log(Exception ex, string className, [CallerMemberName] string methodName = "")
        {
            IRabbitManager RabbitManager = ServiceProvider.GetService<IRabbitManager>();
            SystemLog SystemLog = new SystemLog
            {
                AppUserId = null,
                AppUser = "RABBITMQ",
                ClassName = className,
                MethodName = methodName,
                ModuleName = StaticParams.ModuleName,
                Exception = ex.ToString(),
                Time = StaticParams.DateTimeNow,
            };
            RabbitManager.PublishSingle(SystemLog, RoutingKeyEnum.SystemLogSend.Code);
        }

        protected void AuditLog(object newData, object oldData, string className, [CallerMemberName] string methodName = "")
        {
            IRabbitManager RabbitManager = ServiceProvider.GetService<IRabbitManager>();
            AuditLog AuditLog = new AuditLog
            {
                AppUserId = null,
                AppUser = "RABBITMQ",
                ClassName = className,
                MethodName = methodName,
                ModuleName = StaticParams.ModuleName,
                OldData = JsonConvert.SerializeObject(oldData),
                NewData = JsonConvert.SerializeObject(newData),
                Time = StaticParams.DateTimeNow,
                RowId = Guid.NewGuid(),
            };
            RabbitManager.PublishSingle(AuditLog, RoutingKeyEnum.AuditLogSend.Code);
        }
    }
}
