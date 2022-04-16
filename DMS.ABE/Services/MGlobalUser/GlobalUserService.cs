using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using DMS.ABE.Repositories;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Common;
using DMS.ABE.Handlers.Configuration;
using DMS.ABE.Helpers;

namespace DMS.ABE.Services.MGlobalUser
{
    public interface IGlobalUserService : IServiceScoped
    {
        Task<int> Count(GlobalUserFilter GlobalUserFilter);
        Task<List<GlobalUser>> List(GlobalUserFilter GlobalUserFilter);
        Task<GlobalUser> Get(long Id);
        Task<GlobalUser> Get(Guid RowId);
        Task<List<GlobalUser>> BulkMerge(List<GlobalUser> GlobalUsers);
    }

    public class GlobalUserService : IGlobalUserService
    {
        private IUOW UOW;
        private IRabbitManager RabbitManager;
        private ILogging Logging;
        private ICurrentContext CurrentContext;

        public GlobalUserService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IRabbitManager RabbitManager,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(GlobalUserFilter GlobalUserFilter)
        {
            try
            {
                int result = await UOW.GlobalUserRepository.Count(GlobalUserFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(GlobalUserService));
            }
            return 0;
        }

        public async Task<List<GlobalUser>> List(GlobalUserFilter GlobalUserFilter)
        {
            try
            {
                List<GlobalUser> GlobalUsers = await UOW.GlobalUserRepository.List(GlobalUserFilter);
                return GlobalUsers;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(GlobalUserService));
            }
            return null;
        }

        public async Task<GlobalUser> Get(long Id)
        {
            GlobalUser GlobalUser = await UOW.GlobalUserRepository.Get(Id);
            if (GlobalUser == null)
                return null;
            return GlobalUser;
        }

        public async Task<GlobalUser> Get(Guid RowId)
        {
            GlobalUser GlobalUser = await UOW.GlobalUserRepository.Get(RowId);
            if (GlobalUser == null)
                return null;
            return GlobalUser;
        }

        public async Task<List<GlobalUser>> BulkMerge(List<GlobalUser> GlobalUsers)
        {
            try
            {
                await UOW.GlobalUserRepository.BulkMerge(GlobalUsers);
                RabbitManager.PublishList(GlobalUsers, RoutingKeyEnum.GlobalUserSync.Code);
                return GlobalUsers;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(GlobalUserService));
            }
            return null;
        }
    }
}
