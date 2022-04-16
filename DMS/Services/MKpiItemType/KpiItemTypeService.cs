using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiItemType
{
    public interface IKpiItemTypeService : IServiceScoped
    {
        Task<int> Count(KpiItemTypeFilter KpiItemTypeFilter);
        Task<List<KpiItemType>> List(KpiItemTypeFilter KpiItemTypeFilter);
        Task<KpiItemType> Get(long Id);
        Task<KpiItemType> Create(KpiItemType KpiItemType);
        Task<KpiItemType> Update(KpiItemType KpiItemType);
        Task<KpiItemType> Delete(KpiItemType KpiItemType);
        Task<List<KpiItemType>> BulkDelete(List<KpiItemType> KpiItemTypes);
        Task<List<KpiItemType>> Import(List<KpiItemType> KpiItemTypes);
        Task<KpiItemTypeFilter> ToFilter(KpiItemTypeFilter KpiItemTypeFilter);
    }

    public class KpiItemTypeService : BaseService, IKpiItemTypeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IKpiItemTypeValidator KpiItemTypeValidator;

        public KpiItemTypeService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IKpiItemTypeValidator KpiItemTypeValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.KpiItemTypeValidator = KpiItemTypeValidator;
        }
        public async Task<int> Count(KpiItemTypeFilter KpiItemTypeFilter)
        {
            try
            {
                int result = await UOW.KpiItemTypeRepository.Count(KpiItemTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiItemTypeService));
            }
            return 0;
        }

        public async Task<List<KpiItemType>> List(KpiItemTypeFilter KpiItemTypeFilter)
        {
            try
            {
                List<KpiItemType> KpiItemTypes = await UOW.KpiItemTypeRepository.List(KpiItemTypeFilter);
                return KpiItemTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiItemTypeService));
            }
            return null;
        }

        public async Task<KpiItemType> Get(long Id)
        {
            KpiItemType KpiItemType = await UOW.KpiItemTypeRepository.Get(Id);
            if (KpiItemType == null)
                return null;
            return KpiItemType;
        }
        public async Task<KpiItemType> Create(KpiItemType KpiItemType)
        {
            if (!await KpiItemTypeValidator.Create(KpiItemType))
                return KpiItemType;

            try
            {
                await UOW.KpiItemTypeRepository.Create(KpiItemType);
                KpiItemType = await UOW.KpiItemTypeRepository.Get(KpiItemType.Id);
                Logging.CreateAuditLog(KpiItemType, new { }, nameof(KpiItemTypeService));
                return KpiItemType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiItemTypeService));
            }
            return null;
        }

        public async Task<KpiItemType> Update(KpiItemType KpiItemType)
        {
            if (!await KpiItemTypeValidator.Update(KpiItemType))
                return KpiItemType;
            try
            {
                var oldData = await UOW.KpiItemTypeRepository.Get(KpiItemType.Id);

                await UOW.KpiItemTypeRepository.Update(KpiItemType);

                KpiItemType = await UOW.KpiItemTypeRepository.Get(KpiItemType.Id);
                Logging.CreateAuditLog(KpiItemType, oldData, nameof(KpiItemTypeService));
                return KpiItemType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiItemTypeService));
            }
            return null;
        }

        public async Task<KpiItemType> Delete(KpiItemType KpiItemType)
        {
            if (!await KpiItemTypeValidator.Delete(KpiItemType))
                return KpiItemType;

            try
            {
                await UOW.KpiItemTypeRepository.Delete(KpiItemType);
                Logging.CreateAuditLog(new { }, KpiItemType, nameof(KpiItemTypeService));
                return KpiItemType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiItemTypeService));
            }
            return null;
        }

        public async Task<List<KpiItemType>> BulkDelete(List<KpiItemType> KpiItemTypes)
        {
            if (!await KpiItemTypeValidator.BulkDelete(KpiItemTypes))
                return KpiItemTypes;

            try
            {
                await UOW.KpiItemTypeRepository.BulkDelete(KpiItemTypes);
                Logging.CreateAuditLog(new { }, KpiItemTypes, nameof(KpiItemTypeService));
                return KpiItemTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiItemTypeService));
            }
            return null;

        }

        public async Task<List<KpiItemType>> Import(List<KpiItemType> KpiItemTypes)
        {
            if (!await KpiItemTypeValidator.Import(KpiItemTypes))
                return KpiItemTypes;
            try
            {
                await UOW.KpiItemTypeRepository.BulkMerge(KpiItemTypes);

                Logging.CreateAuditLog(KpiItemTypes, new { }, nameof(KpiItemTypeService));
                return KpiItemTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiItemTypeService));
            }
            return null;
        }

        public async Task<KpiItemTypeFilter> ToFilter(KpiItemTypeFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<KpiItemTypeFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                KpiItemTypeFilter subFilter = new KpiItemTypeFilter();
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
