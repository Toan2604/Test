using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MSystemConfiguration
{
    public interface ISystemConfigurationService : IServiceScoped
    {
        Task<SystemConfiguration> Get();
        Task<bool> Update(SystemConfiguration SystemConfiguration);
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

        public async Task<bool> Update(SystemConfiguration SystemConfiguration)
        {
            try
            {
                return await UOW.SystemConfigurationRepository.Update(SystemConfiguration);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SystemConfigurationService));
            }
            return true;
        }
    }
}
