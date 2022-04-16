using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiProductGroupingType
{
    public interface IKpiProductGroupingTypeService : IServiceScoped
    {
        Task<int> Count(KpiProductGroupingTypeFilter KpiProductGroupingTypeFilter);
        Task<List<KpiProductGroupingType>> List(KpiProductGroupingTypeFilter KpiProductGroupingTypeFilter);
        Task<KpiProductGroupingType> Get(long Id);
        Task<KpiProductGroupingType> Create(KpiProductGroupingType KpiProductGroupingType);
        Task<KpiProductGroupingType> Update(KpiProductGroupingType KpiProductGroupingType);
        Task<KpiProductGroupingType> Delete(KpiProductGroupingType KpiProductGroupingType);
        Task<List<KpiProductGroupingType>> BulkDelete(List<KpiProductGroupingType> KpiProductGroupingTypes);
        Task<List<KpiProductGroupingType>> Import(List<KpiProductGroupingType> KpiProductGroupingTypes);
        Task<KpiProductGroupingTypeFilter> ToFilter(KpiProductGroupingTypeFilter KpiProductGroupingTypeFilter);
    }

    public class KpiProductGroupingTypeService : BaseService, IKpiProductGroupingTypeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IKpiProductGroupingTypeValidator KpiProductGroupingTypeValidator;

        public KpiProductGroupingTypeService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IKpiProductGroupingTypeValidator KpiProductGroupingTypeValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.KpiProductGroupingTypeValidator = KpiProductGroupingTypeValidator;
        }
        public async Task<int> Count(KpiProductGroupingTypeFilter KpiProductGroupingTypeFilter)
        {
            try
            {
                int result = await UOW.KpiProductGroupingTypeRepository.Count(KpiProductGroupingTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingTypeService));
            }
            return 0;
        }

        public async Task<List<KpiProductGroupingType>> List(KpiProductGroupingTypeFilter KpiProductGroupingTypeFilter)
        {
            try
            {
                List<KpiProductGroupingType> KpiProductGroupingTypes = await UOW.KpiProductGroupingTypeRepository.List(KpiProductGroupingTypeFilter);
                return KpiProductGroupingTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingTypeService));
            }
            return null;
        }

        public async Task<KpiProductGroupingType> Get(long Id)
        {
            KpiProductGroupingType KpiProductGroupingType = await UOW.KpiProductGroupingTypeRepository.Get(Id);
            if (KpiProductGroupingType == null)
                return null;
            return KpiProductGroupingType;
        }
        public async Task<KpiProductGroupingType> Create(KpiProductGroupingType KpiProductGroupingType)
        {
            if (!await KpiProductGroupingTypeValidator.Create(KpiProductGroupingType))
                return KpiProductGroupingType;

            try
            {
                await UOW.KpiProductGroupingTypeRepository.Create(KpiProductGroupingType);
                KpiProductGroupingType = await UOW.KpiProductGroupingTypeRepository.Get(KpiProductGroupingType.Id);
                Logging.CreateAuditLog(KpiProductGroupingType, new { }, nameof(KpiProductGroupingTypeService));
                return KpiProductGroupingType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingTypeService));
            }
            return null;
        }

        public async Task<KpiProductGroupingType> Update(KpiProductGroupingType KpiProductGroupingType)
        {
            if (!await KpiProductGroupingTypeValidator.Update(KpiProductGroupingType))
                return KpiProductGroupingType;
            try
            {
                var oldData = await UOW.KpiProductGroupingTypeRepository.Get(KpiProductGroupingType.Id);

                await UOW.KpiProductGroupingTypeRepository.Update(KpiProductGroupingType);

                KpiProductGroupingType = await UOW.KpiProductGroupingTypeRepository.Get(KpiProductGroupingType.Id);
                Logging.CreateAuditLog(KpiProductGroupingType, oldData, nameof(KpiProductGroupingTypeService));
                return KpiProductGroupingType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingTypeService));
            }
            return null;
        }

        public async Task<KpiProductGroupingType> Delete(KpiProductGroupingType KpiProductGroupingType)
        {
            if (!await KpiProductGroupingTypeValidator.Delete(KpiProductGroupingType))
                return KpiProductGroupingType;

            try
            {
                await UOW.KpiProductGroupingTypeRepository.Delete(KpiProductGroupingType);
                Logging.CreateAuditLog(new { }, KpiProductGroupingType, nameof(KpiProductGroupingTypeService));
                return KpiProductGroupingType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingTypeService));
            }
            return null;
        }

        public async Task<List<KpiProductGroupingType>> BulkDelete(List<KpiProductGroupingType> KpiProductGroupingTypes)
        {
            if (!await KpiProductGroupingTypeValidator.BulkDelete(KpiProductGroupingTypes))
                return KpiProductGroupingTypes;

            try
            {
                await UOW.KpiProductGroupingTypeRepository.BulkDelete(KpiProductGroupingTypes);
                Logging.CreateAuditLog(new { }, KpiProductGroupingTypes, nameof(KpiProductGroupingTypeService));
                return KpiProductGroupingTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingTypeService));
            }
            return null;

        }

        public async Task<List<KpiProductGroupingType>> Import(List<KpiProductGroupingType> KpiProductGroupingTypes)
        {
            if (!await KpiProductGroupingTypeValidator.Import(KpiProductGroupingTypes))
                return KpiProductGroupingTypes;
            try
            {
                await UOW.KpiProductGroupingTypeRepository.BulkMerge(KpiProductGroupingTypes);

                Logging.CreateAuditLog(KpiProductGroupingTypes, new { }, nameof(KpiProductGroupingTypeService));
                return KpiProductGroupingTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingTypeService));
            }
            return null;
        }

        public async Task<KpiProductGroupingTypeFilter> ToFilter(KpiProductGroupingTypeFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<KpiProductGroupingTypeFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                KpiProductGroupingTypeFilter subFilter = new KpiProductGroupingTypeFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
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
