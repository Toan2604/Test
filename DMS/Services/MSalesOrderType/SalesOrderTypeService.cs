using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MSalesOrderType
{
    public interface ISalesOrderTypeService : IServiceScoped
    {
        Task<int> Count(SalesOrderTypeFilter SalesOrderTypeFilter);
        Task<List<SalesOrderType>> List(SalesOrderTypeFilter SalesOrderTypeFilter);
        Task<SalesOrderType> Get(long Id);
        Task<SalesOrderType> Create(SalesOrderType SalesOrderType);
        Task<SalesOrderType> Update(SalesOrderType SalesOrderType);
        Task<SalesOrderType> Delete(SalesOrderType SalesOrderType);
        Task<List<SalesOrderType>> BulkDelete(List<SalesOrderType> SalesOrderTypes);
        Task<List<SalesOrderType>> Import(List<SalesOrderType> SalesOrderTypes);
        Task<SalesOrderTypeFilter> ToFilter(SalesOrderTypeFilter SalesOrderTypeFilter);
    }

    public class SalesOrderTypeService : BaseService, ISalesOrderTypeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private ISalesOrderTypeValidator SalesOrderTypeValidator;

        public SalesOrderTypeService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            ISalesOrderTypeValidator SalesOrderTypeValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.SalesOrderTypeValidator = SalesOrderTypeValidator;
        }
        public async Task<int> Count(SalesOrderTypeFilter SalesOrderTypeFilter)
        {
            try
            {
                int result = await UOW.SalesOrderTypeRepository.Count(SalesOrderTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SalesOrderTypeService));
            }
            return 0;
        }

        public async Task<List<SalesOrderType>> List(SalesOrderTypeFilter SalesOrderTypeFilter)
        {
            try
            {
                List<SalesOrderType> SalesOrderTypes = await UOW.SalesOrderTypeRepository.List(SalesOrderTypeFilter);
                return SalesOrderTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SalesOrderTypeService));
            }
            return null;
        }
        public async Task<SalesOrderType> Get(long Id)
        {
            SalesOrderType SalesOrderType = await UOW.SalesOrderTypeRepository.Get(Id);
            if (SalesOrderType == null)
                return null;
            return SalesOrderType;
        }

        public async Task<SalesOrderType> Create(SalesOrderType SalesOrderType)
        {
            if (!await SalesOrderTypeValidator.Create(SalesOrderType))
                return SalesOrderType;

            try
            {

                await UOW.SalesOrderTypeRepository.Create(SalesOrderType);

                SalesOrderType = await UOW.SalesOrderTypeRepository.Get(SalesOrderType.Id);
                Logging.CreateAuditLog(SalesOrderType, new { }, nameof(SalesOrderTypeService));
                return SalesOrderType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SalesOrderTypeService));
            }
            return null;
        }

        public async Task<SalesOrderType> Update(SalesOrderType SalesOrderType)
        {
            if (!await SalesOrderTypeValidator.Update(SalesOrderType))
                return SalesOrderType;
            try
            {
                var oldData = await UOW.SalesOrderTypeRepository.Get(SalesOrderType.Id);


                await UOW.SalesOrderTypeRepository.Update(SalesOrderType);


                SalesOrderType = await UOW.SalesOrderTypeRepository.Get(SalesOrderType.Id);
                Logging.CreateAuditLog(SalesOrderType, oldData, nameof(SalesOrderTypeService));
                return SalesOrderType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SalesOrderTypeService));
            }
            return null;
        }

        public async Task<SalesOrderType> Delete(SalesOrderType SalesOrderType)
        {
            if (!await SalesOrderTypeValidator.Delete(SalesOrderType))
                return SalesOrderType;

            try
            {

                await UOW.SalesOrderTypeRepository.Delete(SalesOrderType);

                Logging.CreateAuditLog(new { }, SalesOrderType, nameof(SalesOrderTypeService));
                return SalesOrderType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SalesOrderTypeService));
            }
            return null;
        }

        public async Task<List<SalesOrderType>> BulkDelete(List<SalesOrderType> SalesOrderTypes)
        {
            if (!await SalesOrderTypeValidator.BulkDelete(SalesOrderTypes))
                return SalesOrderTypes;

            try
            {

                await UOW.SalesOrderTypeRepository.BulkDelete(SalesOrderTypes);

                Logging.CreateAuditLog(new { }, SalesOrderTypes, nameof(SalesOrderTypeService));
                return SalesOrderTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SalesOrderTypeService));
            }
            return null;
        }

        public async Task<List<SalesOrderType>> Import(List<SalesOrderType> SalesOrderTypes)
        {
            if (!await SalesOrderTypeValidator.Import(SalesOrderTypes))
                return SalesOrderTypes;
            try
            {

                await UOW.SalesOrderTypeRepository.BulkMerge(SalesOrderTypes);


                Logging.CreateAuditLog(SalesOrderTypes, new { }, nameof(SalesOrderTypeService));
                return SalesOrderTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SalesOrderTypeService));
            }
            return null;
        }

        public async Task<SalesOrderTypeFilter> ToFilter(SalesOrderTypeFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<SalesOrderTypeFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                SalesOrderTypeFilter subFilter = new SalesOrderTypeFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.Code))






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
