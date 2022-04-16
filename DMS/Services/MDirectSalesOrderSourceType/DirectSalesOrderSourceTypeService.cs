using TrueSight.Common;
using DMS.Handlers.Configuration;
using DMS.Common;
using DMS.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using DMS.Repositories;
using DMS.Entities;
using DMS.Enums;

namespace DMS.Services.MDirectSalesOrderSourceType
{
    public interface IDirectSalesOrderSourceTypeService :  IServiceScoped
    {
        Task<int> Count(DirectSalesOrderSourceTypeFilter DirectSalesOrderSourceTypeFilter);
        Task<List<DirectSalesOrderSourceType>> List(DirectSalesOrderSourceTypeFilter DirectSalesOrderSourceTypeFilter);
    }

    public class DirectSalesOrderSourceTypeService : BaseService, IDirectSalesOrderSourceTypeService
    {
        private IUOW UOW;
        private IRabbitManager RabbitManager;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        

        public DirectSalesOrderSourceTypeService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IRabbitManager RabbitManager,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.Logging = Logging;
           
        }

        public async Task<int> Count(DirectSalesOrderSourceTypeFilter DirectSalesOrderSourceTypeFilter)
        {
            try
            {
                int result = await UOW.DirectSalesOrderSourceTypeRepository.Count(DirectSalesOrderSourceTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderSourceTypeService));
            }
            return 0;
        }

        public async Task<List<DirectSalesOrderSourceType>> List(DirectSalesOrderSourceTypeFilter DirectSalesOrderSourceTypeFilter)
        {
            try
            {
                List<DirectSalesOrderSourceType> DirectSalesOrderSourceTypes = await UOW.DirectSalesOrderSourceTypeRepository.List(DirectSalesOrderSourceTypeFilter);
                return DirectSalesOrderSourceTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderSourceTypeService));
            }
            return null;
        }                
        public async Task<DirectSalesOrderSourceTypeFilter> ToFilter(DirectSalesOrderSourceTypeFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<DirectSalesOrderSourceTypeFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                DirectSalesOrderSourceTypeFilter subFilter = new DirectSalesOrderSourceTypeFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == "DirectSalesOrderSourceTypeId")
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Code))
                        subFilter.Code = FilterBuilder.Merge(subFilter.Code, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Name))
                        subFilter.Name = FilterBuilder.Merge(subFilter.Name, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(CurrentContext.UserId) && FilterPermissionDefinition.IdFilter != null)
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.IS.Id)
                        {
                        }
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.ISNT.Id)
                        {
                        }
                    }
                }
            }
            return filter;
        }


    }
}
