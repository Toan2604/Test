using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.ABE.Repositories
{
    public interface ISystemConfigurationRepository
    {
        Task<SystemConfiguration> Get();
        Task<bool> Update(SystemConfiguration SystemConfiguration);
    }
    public class SystemConfigurationRepository : ISystemConfigurationRepository
    {
        private DataContext DataContext;
        public SystemConfigurationRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<bool> Update(SystemConfiguration SystemConfiguration)
        {
            List<SystemConfigurationDAO> SystemConfigurationDAOs = await DataContext.SystemConfiguration.ToListAsync();
            foreach (SystemConfigurationDAO SystemConfigurationDAO in SystemConfigurationDAOs)
            {
                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.ALLOW_EDIT_KPI_IN_PERIOD))
                    SystemConfigurationDAO.Value = SystemConfiguration.ALLOW_EDIT_KPI_IN_PERIOD.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.DASH_BOARD_REFRESH_TIME))
                    SystemConfigurationDAO.Value = SystemConfiguration.DASH_BOARD_REFRESH_TIME.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.PRIORITY_USE_PRICE_LIST))
                    SystemConfigurationDAO.Value = SystemConfiguration.PRIORITY_USE_PRICE_LIST.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.PRIORITY_USE_PROMOTION))
                    SystemConfigurationDAO.Value = SystemConfiguration.PRIORITY_USE_PROMOTION.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.STORE_CHECKING_DISTANCE))
                    SystemConfigurationDAO.Value = SystemConfiguration.STORE_CHECKING_DISTANCE.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.STORE_CHECKING_MINIMUM_TIME))
                    SystemConfigurationDAO.Value = SystemConfiguration.STORE_CHECKING_MINIMUM_TIME.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.STORE_CHECKING_OFFLINE_CONSTRAINT_DISTANCE))
                    SystemConfigurationDAO.Value = SystemConfiguration.STORE_CHECKING_OFFLINE_CONSTRAINT_DISTANCE.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.USE_DIRECT_SALES_ORDER))
                    SystemConfigurationDAO.Value = SystemConfiguration.USE_DIRECT_SALES_ORDER.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.USE_INDIRECT_SALES_ORDER))
                    SystemConfigurationDAO.Value = SystemConfiguration.USE_INDIRECT_SALES_ORDER.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.YOUTUBE_ID))
                    SystemConfigurationDAO.Value = SystemConfiguration.YOUTUBE_ID;

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.ALLOW_DRAFT_STORE_TO_CREATE_ORDER))
                    SystemConfigurationDAO.Value = SystemConfiguration.ALLOW_DRAFT_STORE_TO_CREATE_ORDER.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.AMPLITUDE_PRICE_IN_DIRECT))
                    SystemConfigurationDAO.Value = SystemConfiguration.AMPLITUDE_PRICE_IN_DIRECT.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.AMPLITUDE_PRICE_IN_INDIRECT))
                    SystemConfigurationDAO.Value = SystemConfiguration.AMPLITUDE_PRICE_IN_INDIRECT.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.ALLOW_EDIT_PRICE_IN_DIRECT_SALES_ORDER))
                    SystemConfigurationDAO.Value = SystemConfiguration.ALLOW_EDIT_PRICE_IN_DIRECT_SALES_ORDER.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.ALLOW_EDIT_PRICE_IN_INDIRECT_SALES_ORDER))
                    SystemConfigurationDAO.Value = SystemConfiguration.ALLOW_EDIT_PRICE_IN_INDIRECT_SALES_ORDER.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.ALLOW_BUYER_STORE_APPROVE_DIRECT_SALES_ORDER))
                    SystemConfigurationDAO.Value = SystemConfiguration.ALLOW_BUYER_STORE_APPROVE_DIRECT_SALES_ORDER.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.SYNC_DIRECT_SALES_ORDER_WITH_ERP))
                    SystemConfigurationDAO.Value = SystemConfiguration.SYNC_DIRECT_SALES_ORDER_WITH_ERP.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.ERP_CALCULATE_SALES_ORDER_PRICE))
                    SystemConfigurationDAO.Value = SystemConfiguration.ERP_CALCULATE_SALES_ORDER_PRICE.ToString();

                if (SystemConfigurationDAO.Code == nameof(SystemConfigurationEnum.URL_API_FOR_ERP_CALCULATE_SALES_ORDER))
                    SystemConfigurationDAO.Value = SystemConfiguration.URL_API_FOR_ERP_CALCULATE_SALES_ORDER;
            }
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<SystemConfiguration> Get()
        {
            List<SystemConfigurationDAO> SystemConfigurationDAOs = await DataContext.SystemConfiguration.ToListAsync();
            SystemConfiguration SystemConfiguration = new SystemConfiguration();
            foreach (SystemConfigurationDAO SystemConfigurationDAO in SystemConfigurationDAOs)
            {
                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.ALLOW_EDIT_KPI_IN_PERIOD.Id)
                {
                    bool result;
                    if (SystemConfigurationDAO.Value == null || !bool.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.ALLOW_EDIT_KPI_IN_PERIOD = false;
                    else
                        SystemConfiguration.ALLOW_EDIT_KPI_IN_PERIOD = result;
                };

                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.AMPLITUDE_PRICE_IN_DIRECT.Id)
                {
                    decimal result;
                    if (SystemConfigurationDAO.Value == null || !decimal.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.AMPLITUDE_PRICE_IN_DIRECT = 10;
                    else
                        SystemConfiguration.AMPLITUDE_PRICE_IN_DIRECT = result;
                };

                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.AMPLITUDE_PRICE_IN_INDIRECT.Id)
                {
                    decimal result;
                    if (SystemConfigurationDAO.Value == null || !decimal.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.AMPLITUDE_PRICE_IN_INDIRECT = 10;
                    else
                        SystemConfiguration.AMPLITUDE_PRICE_IN_INDIRECT = result;
                };

                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.DASH_BOARD_REFRESH_TIME.Id)
                {
                    long result;
                    if (SystemConfigurationDAO.Value == null || !long.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.DASH_BOARD_REFRESH_TIME = 300;
                    else
                        SystemConfiguration.DASH_BOARD_REFRESH_TIME = result;
                };

                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.PRIORITY_USE_PRICE_LIST.Id)
                {
                    long result;
                    if (SystemConfigurationDAO.Value == null || !long.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.PRIORITY_USE_PRICE_LIST = 0;
                    else
                        SystemConfiguration.PRIORITY_USE_PRICE_LIST = result;
                };

                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.PRIORITY_USE_PROMOTION.Id)
                {
                    long result;
                    if (SystemConfigurationDAO.Value == null || !long.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.PRIORITY_USE_PROMOTION = 0;
                    else
                        SystemConfiguration.PRIORITY_USE_PROMOTION = result;
                };

                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.STORE_CHECKING_DISTANCE.Id)
                {
                    long result;
                    if (SystemConfigurationDAO.Value == null || !long.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.STORE_CHECKING_DISTANCE = 100;
                    else
                        SystemConfiguration.STORE_CHECKING_DISTANCE = result;
                };

                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.STORE_CHECKING_MINIMUM_TIME.Id)
                {
                    long result;
                    if (SystemConfigurationDAO.Value == null || !long.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.STORE_CHECKING_MINIMUM_TIME = 600;
                    else
                        SystemConfiguration.STORE_CHECKING_MINIMUM_TIME = result;
                };

                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.STORE_CHECKING_OFFLINE_CONSTRAINT_DISTANCE.Id)
                {
                    bool result;
                    if (SystemConfigurationDAO.Value == null || !bool.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.STORE_CHECKING_OFFLINE_CONSTRAINT_DISTANCE = false;
                    else
                        SystemConfiguration.STORE_CHECKING_OFFLINE_CONSTRAINT_DISTANCE = result;
                };

                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.USE_DIRECT_SALES_ORDER.Id)
                {
                    bool result;
                    if (SystemConfigurationDAO.Value == null || !bool.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.USE_DIRECT_SALES_ORDER = false;
                    else
                        SystemConfiguration.USE_DIRECT_SALES_ORDER = result;
                };
                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.USE_INDIRECT_SALES_ORDER.Id)
                {
                    bool result;
                    if (SystemConfigurationDAO.Value == null || !bool.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.USE_INDIRECT_SALES_ORDER = false;
                    else
                        SystemConfiguration.USE_INDIRECT_SALES_ORDER = result;
                };
                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.YOUTUBE_ID.Id)
                {
                    SystemConfiguration.YOUTUBE_ID = SystemConfigurationDAO.Value;
                };

                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.ALLOW_DRAFT_STORE_TO_CREATE_ORDER.Id)
                {
                    bool result;
                    if (SystemConfigurationDAO.Value == null || !bool.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.ALLOW_DRAFT_STORE_TO_CREATE_ORDER = false;
                    else
                        SystemConfiguration.ALLOW_DRAFT_STORE_TO_CREATE_ORDER = result;
                };
                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.ALLOW_EDIT_PRICE_IN_DIRECT_SALES_ORDER.Id)
                {
                    bool result;
                    if (SystemConfigurationDAO.Value == null || !bool.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.ALLOW_EDIT_PRICE_IN_DIRECT_SALES_ORDER = false;
                    else
                        SystemConfiguration.ALLOW_EDIT_PRICE_IN_DIRECT_SALES_ORDER = result;
                };
                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.ALLOW_EDIT_PRICE_IN_INDIRECT_SALES_ORDER.Id)
                {
                    bool result;
                    if (SystemConfigurationDAO.Value == null || !bool.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.ALLOW_EDIT_PRICE_IN_INDIRECT_SALES_ORDER = false;
                    else
                        SystemConfiguration.ALLOW_EDIT_PRICE_IN_INDIRECT_SALES_ORDER = result;
                };
                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.ALLOW_BUYER_STORE_APPROVE_DIRECT_SALES_ORDER.Id)
                {
                    bool result;
                    if (SystemConfigurationDAO.Value == null || !bool.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.ALLOW_BUYER_STORE_APPROVE_DIRECT_SALES_ORDER = false;
                    else
                        SystemConfiguration.ALLOW_BUYER_STORE_APPROVE_DIRECT_SALES_ORDER = result;
                };
                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.SYNC_DIRECT_SALES_ORDER_WITH_ERP.Id)
                {
                    bool result;
                    if (SystemConfigurationDAO.Value == null || !bool.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.SYNC_DIRECT_SALES_ORDER_WITH_ERP = false;
                    else
                        SystemConfiguration.SYNC_DIRECT_SALES_ORDER_WITH_ERP = result;
                };
                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.ERP_CALCULATE_SALES_ORDER_PRICE.Id)
                {
                    bool result;
                    if (SystemConfigurationDAO.Value == null || !bool.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.ERP_CALCULATE_SALES_ORDER_PRICE = false;
                    else
                        SystemConfiguration.ERP_CALCULATE_SALES_ORDER_PRICE = result;
                };
                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.URL_API_FOR_ERP_CALCULATE_SALES_ORDER.Id)
                {
                    SystemConfiguration.URL_API_FOR_ERP_CALCULATE_SALES_ORDER = SystemConfigurationDAO.Value;
                };
                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.ALLOW_UPDATE_APPROVED_PRICE_LIST.Id)
                {
                    bool result;
                    if (SystemConfigurationDAO.Value == null || !bool.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.ALLOW_UPDATE_APPROVED_PRICE_LIST = false;
                    else
                        SystemConfiguration.ALLOW_UPDATE_APPROVED_PRICE_LIST = result;
                };
                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.START_WORKFLOW_BY_USER_TYPE.Id)
                {
                    long result;
                    if (SystemConfigurationDAO.Value == null || !long.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.START_WORKFLOW_BY_USER_TYPE = 1;
                    else
                        SystemConfiguration.START_WORKFLOW_BY_USER_TYPE = result;
                };
                if (SystemConfigurationDAO.Id == SystemConfigurationEnum.USE_ELASTICSEARCH.Id)
                {
                    bool result;
                    if (SystemConfigurationDAO.Value == null || !bool.TryParse(SystemConfigurationDAO.Value, out result))
                        SystemConfiguration.USE_ELASTICSEARCH = false;
                    else
                        SystemConfiguration.USE_ELASTICSEARCH = result;
                };
            }

            return SystemConfiguration;
        }
    }
}
