using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MERouteType
{
    public interface IERouteTypeService : IServiceScoped
    {
        Task<int> Count(ERouteTypeFilter ERouteTypeFilter);
        Task<List<ERouteType>> List(ERouteTypeFilter ERouteTypeFilter);
        Task<ERouteType> Get(long Id);
        Task<ERouteType> Create(ERouteType ERouteType);
        Task<ERouteType> Update(ERouteType ERouteType);
        Task<ERouteType> Delete(ERouteType ERouteType);
        Task<List<ERouteType>> BulkDelete(List<ERouteType> ERouteTypes);
        Task<List<ERouteType>> Import(List<ERouteType> ERouteTypes);
        ERouteTypeFilter ToFilter(ERouteTypeFilter ERouteTypeFilter);
    }

    public class ERouteTypeService : BaseService, IERouteTypeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IERouteTypeValidator ERouteTypeValidator;

        public ERouteTypeService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IERouteTypeValidator ERouteTypeValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ERouteTypeValidator = ERouteTypeValidator;
        }
        public async Task<int> Count(ERouteTypeFilter ERouteTypeFilter)
        {
            try
            {
                int result = await UOW.ERouteTypeRepository.Count(ERouteTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteTypeService));
            }
            return 0;
        }

        public async Task<List<ERouteType>> List(ERouteTypeFilter ERouteTypeFilter)
        {
            try
            {
                List<ERouteType> ERouteTypes = await UOW.ERouteTypeRepository.List(ERouteTypeFilter);
                return ERouteTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteTypeService));
            }
            return null;
        }
        public async Task<ERouteType> Get(long Id)
        {
            ERouteType ERouteType = await UOW.ERouteTypeRepository.Get(Id);
            if (ERouteType == null)
                return null;
            return ERouteType;
        }

        public async Task<ERouteType> Create(ERouteType ERouteType)
        {
            if (!await ERouteTypeValidator.Create(ERouteType))
                return ERouteType;

            try
            {

                await UOW.ERouteTypeRepository.Create(ERouteType);


                Logging.CreateAuditLog(ERouteType, new { }, nameof(ERouteTypeService));
                return await UOW.ERouteTypeRepository.Get(ERouteType.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteTypeService));
            }
            return null;
        }

        public async Task<ERouteType> Update(ERouteType ERouteType)
        {
            if (!await ERouteTypeValidator.Update(ERouteType))
                return ERouteType;
            try
            {
                var oldData = await UOW.ERouteTypeRepository.Get(ERouteType.Id);


                await UOW.ERouteTypeRepository.Update(ERouteType);


                var newData = await UOW.ERouteTypeRepository.Get(ERouteType.Id);
                Logging.CreateAuditLog(newData, oldData, nameof(ERouteTypeService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteTypeService));
            }
            return null;
        }

        public async Task<ERouteType> Delete(ERouteType ERouteType)
        {
            if (!await ERouteTypeValidator.Delete(ERouteType))
                return ERouteType;

            try
            {

                await UOW.ERouteTypeRepository.Delete(ERouteType);

                Logging.CreateAuditLog(new { }, ERouteType, nameof(ERouteTypeService));
                return ERouteType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteTypeService));
            }
            return null;
        }

        public async Task<List<ERouteType>> BulkDelete(List<ERouteType> ERouteTypes)
        {
            if (!await ERouteTypeValidator.BulkDelete(ERouteTypes))
                return ERouteTypes;

            try
            {

                await UOW.ERouteTypeRepository.BulkDelete(ERouteTypes);

                Logging.CreateAuditLog(new { }, ERouteTypes, nameof(ERouteTypeService));
                return ERouteTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteTypeService));
            }
            return null;
        }

        public async Task<List<ERouteType>> Import(List<ERouteType> ERouteTypes)
        {
            if (!await ERouteTypeValidator.Import(ERouteTypes))
                return ERouteTypes;
            try
            {

                await UOW.ERouteTypeRepository.BulkMerge(ERouteTypes);


                Logging.CreateAuditLog(ERouteTypes, new { }, nameof(ERouteTypeService));
                return ERouteTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteTypeService));
            }
            return null;
        }

        public ERouteTypeFilter ToFilter(ERouteTypeFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<ERouteTypeFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                ERouteTypeFilter subFilter = new ERouteTypeFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                }
            }
            return filter;
        }
    }
}
