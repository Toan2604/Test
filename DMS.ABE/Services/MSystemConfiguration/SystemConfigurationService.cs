using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using System;
using System.Threading.Tasks;

namespace DMS.ABE.Services.MSystemConfiguration
{
    public interface ISystemConfigurationService : IServiceScoped
    {
        Task<SystemConfiguration> Get();
    }

    public class SystemConfigurationService : BaseService, ISystemConfigurationService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;

        public SystemConfigurationService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
        }
        public async Task<SystemConfiguration> Get()
        {
            try
            {
                var result = await UOW.SystemConfigurationRepository.Get();
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SystemConfigurationService));
            }
            return null;
        }
    }
}
