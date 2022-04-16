
using DMS.Common;
using DMS.Entities;
using DMS.Services.MSystemConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.system_configuration
{
    public class SystemConfigurationController : RpcController
    {
        private ISystemConfigurationService SystemConfigurationService;
        private ICurrentContext CurrentContext;
        public SystemConfigurationController(
            ISystemConfigurationService SystemConfigurationService,
            ICurrentContext CurrentContext
        )
        {
            this.SystemConfigurationService = SystemConfigurationService;
            this.CurrentContext = CurrentContext;
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


        [Route(SystemConfigurationRoute.Update), HttpPost]
        public async Task<SystemConfiguration_SystemConfigurationDTO> Update([FromBody] SystemConfiguration_SystemConfigurationDTO SystemConfiguration_SystemConfigurationDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            SystemConfiguration SystemConfiguration = new SystemConfiguration
            {
                STORE_CHECKING_DISTANCE = SystemConfiguration_SystemConfigurationDTO.STORE_CHECKING_DISTANCE,
                STORE_CHECKING_OFFLINE_CONSTRAINT_DISTANCE = SystemConfiguration_SystemConfigurationDTO.STORE_CHECKING_OFFLINE_CONSTRAINT_DISTANCE,
                USE_DIRECT_SALES_ORDER = SystemConfiguration_SystemConfigurationDTO.USE_DIRECT_SALES_ORDER,
                USE_INDIRECT_SALES_ORDER = SystemConfiguration_SystemConfigurationDTO.USE_INDIRECT_SALES_ORDER,
                ALLOW_EDIT_KPI_IN_PERIOD = SystemConfiguration_SystemConfigurationDTO.ALLOW_EDIT_KPI_IN_PERIOD,
                PRIORITY_USE_PRICE_LIST = SystemConfiguration_SystemConfigurationDTO.PRIORITY_USE_PRICE_LIST,
                PRIORITY_USE_PROMOTION = SystemConfiguration_SystemConfigurationDTO.PRIORITY_USE_PROMOTION,
                STORE_CHECKING_MINIMUM_TIME = SystemConfiguration_SystemConfigurationDTO.STORE_CHECKING_MINIMUM_TIME,
                DASH_BOARD_REFRESH_TIME = SystemConfiguration_SystemConfigurationDTO.DASH_BOARD_REFRESH_TIME,
                AMPLITUDE_PRICE_IN_DIRECT = SystemConfiguration_SystemConfigurationDTO.AMPLITUDE_PRICE_IN_DIRECT,
                AMPLITUDE_PRICE_IN_INDIRECT = SystemConfiguration_SystemConfigurationDTO.AMPLITUDE_PRICE_IN_INDIRECT,
                YOUTUBE_ID = SystemConfiguration_SystemConfigurationDTO.YOUTUBE_ID,
                ALLOW_DRAFT_STORE_TO_CREATE_ORDER = SystemConfiguration_SystemConfigurationDTO.ALLOW_DRAFT_STORE_TO_CREATE_ORDER,
                ALLOW_EDIT_PRICE_IN_DIRECT_SALES_ORDER = SystemConfiguration_SystemConfigurationDTO.ALLOW_EDIT_PRICE_IN_DIRECT_SALES_ORDER,
                ALLOW_EDIT_PRICE_IN_INDIRECT_SALES_ORDER = SystemConfiguration_SystemConfigurationDTO.ALLOW_EDIT_PRICE_IN_INDIRECT_SALES_ORDER,
                ALLOW_BUYER_STORE_APPROVE_DIRECT_SALES_ORDER = SystemConfiguration_SystemConfigurationDTO.ALLOW_BUYER_STORE_APPROVE_DIRECT_SALES_ORDER,
                SYNC_DIRECT_SALES_ORDER_WITH_ERP = SystemConfiguration_SystemConfigurationDTO.SYNC_DIRECT_SALES_ORDER_WITH_ERP,
                ERP_CALCULATE_SALES_ORDER_PRICE = SystemConfiguration_SystemConfigurationDTO.ERP_CALCULATE_SALES_ORDER_PRICE,
                URL_API_FOR_ERP_CALCULATE_SALES_ORDER = SystemConfiguration_SystemConfigurationDTO.URL_API_FOR_ERP_CALCULATE_SALES_ORDER,
                ALLOW_UPDATE_APPROVED_PRICE_LIST = SystemConfiguration_SystemConfigurationDTO.ALLOW_UPDATE_APPROVED_PRICE_LIST,
                START_WORKFLOW_BY_USER_TYPE = SystemConfiguration_SystemConfigurationDTO.START_WORKFLOW_BY_USER_TYPE,
                USE_ELASTICSEARCH = SystemConfiguration_SystemConfigurationDTO.USE_ELASTICSEARCH,
            };
            if (SystemConfiguration.ALLOW_EDIT_PRICE_IN_DIRECT_SALES_ORDER == false)
                SystemConfiguration.AMPLITUDE_PRICE_IN_DIRECT = 0;
            if (SystemConfiguration.ALLOW_EDIT_PRICE_IN_INDIRECT_SALES_ORDER == false)
                SystemConfiguration.AMPLITUDE_PRICE_IN_INDIRECT = 0;

            await SystemConfigurationService.Update(SystemConfiguration);
            SystemConfiguration = await SystemConfigurationService.Get();
            return new SystemConfiguration_SystemConfigurationDTO(SystemConfiguration);
        }
    }
}

