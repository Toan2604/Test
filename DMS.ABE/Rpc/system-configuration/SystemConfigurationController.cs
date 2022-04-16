using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Services.MSystemConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.ABE.Rpc.system_configuration
{
    public class SystemConfigurationController : SimpleController
    {
        private ISystemConfigurationService SystemConfigurationService;
        public SystemConfigurationController(
            ISystemConfigurationService SystemConfigurationService,
            ICurrentContext CurrentContext
        )
        {
            this.SystemConfigurationService = SystemConfigurationService;
        }
        [AllowAnonymous]
        [Route(SystemConfigurationRoute.Get), HttpPost]
        public async Task<ActionResult<SystemConfiguration_SystemConfigurationDTO>> Get()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            SystemConfiguration SystemConfiguration = await SystemConfigurationService.Get();
            return new SystemConfiguration_SystemConfigurationDTO(SystemConfiguration);
        }
    }
}

