using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using TrueSight.Common;
using DMS.Handlers.Configuration;

namespace DMS.Helpers
{
    public interface ILogging : IServiceScoped
    {
        void CreateAuditLog(object newData, object oldData, string className, [CallerMemberName] string methodName = "");
        void CreateSystemLog(Exception ex, string className, [CallerMemberName] string methodName = "");
    }
    public class Logging : ILogging
    {
        private ICurrentContext CurrentContext;
        private IRabbitManager RabbitManager;
        public Logging(
            ICurrentContext CurrentContext,
            IRabbitManager RabbitManager
            )
        {
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
        }
        public void CreateAuditLog(object newData, object oldData, string className, [CallerMemberName] string methodName = "")
        {
            //AuditLog AuditLog = new AuditLog
            //{
            //    AppUserId = CurrentContext.UserId,
            //    AppUser = CurrentContext.UserName,
            //    ClassName = className,
            //    MethodName = methodName,
            //    ModuleName = StaticParams.ModuleName,
            //    OldData = JsonConvert.SerializeObject(oldData),
            //    NewData = JsonConvert.SerializeObject(newData),
            //    Time = StaticParams.DateTimeNow,
            //    RowId = Guid.NewGuid(),
            //};
            //RabbitManager.PublishSingle(AuditLog, RoutingKeyEnum.AuditLogSend.Code);
        }
        public void CreateSystemLog(Exception ex, string className, [CallerMemberName] string methodName = "")
        {
            if (ex.InnerException != null)
                ex = ex.InnerException;

            SystemLog SystemLog = new SystemLog
            {
                AppUserId = CurrentContext.UserId,
                AppUser = CurrentContext.UserName,
                ClassName = className,
                MethodName = methodName,
                ModuleName = StaticParams.ModuleName,
                Exception = ex.ToString(),
                Time = StaticParams.DateTimeNow,
            };
            RabbitManager.PublishSingle(SystemLog, RoutingKeyEnum.SystemLogSend.Code);
            throw new MessageException(ex);
        }
    }
}
