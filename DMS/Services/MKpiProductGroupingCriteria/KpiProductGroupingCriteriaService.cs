using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiProductGroupingCriteria
{
    public interface IKpiProductGroupingCriteriaService : IServiceScoped
    {
        Task<int> Count(KpiProductGroupingCriteriaFilter KpiProductGroupingCriteriaFilter);
        Task<List<KpiProductGroupingCriteria>> List(KpiProductGroupingCriteriaFilter KpiProductGroupingCriteriaFilter);
        Task<KpiProductGroupingCriteria> Get(long Id);
        Task<KpiProductGroupingCriteria> Create(KpiProductGroupingCriteria KpiProductGroupingCriteria);
        Task<KpiProductGroupingCriteria> Update(KpiProductGroupingCriteria KpiProductGroupingCriteria);
        Task<KpiProductGroupingCriteria> Delete(KpiProductGroupingCriteria KpiProductGroupingCriteria);
        Task<List<KpiProductGroupingCriteria>> BulkDelete(List<KpiProductGroupingCriteria> KpiProductGroupingCriterias);
        Task<List<KpiProductGroupingCriteria>> Import(List<KpiProductGroupingCriteria> KpiProductGroupingCriterias);
        Task<KpiProductGroupingCriteriaFilter> ToFilter(KpiProductGroupingCriteriaFilter KpiProductGroupingCriteriaFilter);
    }

    public class KpiProductGroupingCriteriaService : BaseService, IKpiProductGroupingCriteriaService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IKpiProductGroupingCriteriaValidator KpiProductGroupingCriteriaValidator;

        public KpiProductGroupingCriteriaService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IKpiProductGroupingCriteriaValidator KpiProductGroupingCriteriaValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.KpiProductGroupingCriteriaValidator = KpiProductGroupingCriteriaValidator;
        }
        public async Task<int> Count(KpiProductGroupingCriteriaFilter KpiProductGroupingCriteriaFilter)
        {
            try
            {
                int result = await UOW.KpiProductGroupingCriteriaRepository.Count(KpiProductGroupingCriteriaFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingCriteriaService));
            }
            return 0;
        }

        public async Task<List<KpiProductGroupingCriteria>> List(KpiProductGroupingCriteriaFilter KpiProductGroupingCriteriaFilter)
        {
            try
            {
                List<KpiProductGroupingCriteria> KpiProductGroupingCriterias = await UOW.KpiProductGroupingCriteriaRepository.List(KpiProductGroupingCriteriaFilter);
                return KpiProductGroupingCriterias;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingCriteriaService));
            }
            return null;
        }

        public async Task<KpiProductGroupingCriteria> Get(long Id)
        {
            KpiProductGroupingCriteria KpiProductGroupingCriteria = await UOW.KpiProductGroupingCriteriaRepository.Get(Id);
            if (KpiProductGroupingCriteria == null)
                return null;
            return KpiProductGroupingCriteria;
        }
        public async Task<KpiProductGroupingCriteria> Create(KpiProductGroupingCriteria KpiProductGroupingCriteria)
        {
            if (!await KpiProductGroupingCriteriaValidator.Create(KpiProductGroupingCriteria))
                return KpiProductGroupingCriteria;

            try
            {
                await UOW.KpiProductGroupingCriteriaRepository.Create(KpiProductGroupingCriteria);
                KpiProductGroupingCriteria = await UOW.KpiProductGroupingCriteriaRepository.Get(KpiProductGroupingCriteria.Id);
                Logging.CreateAuditLog(KpiProductGroupingCriteria, new { }, nameof(KpiProductGroupingCriteriaService));
                return KpiProductGroupingCriteria;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingCriteriaService));
            }
            return null;
        }

        public async Task<KpiProductGroupingCriteria> Update(KpiProductGroupingCriteria KpiProductGroupingCriteria)
        {
            if (!await KpiProductGroupingCriteriaValidator.Update(KpiProductGroupingCriteria))
                return KpiProductGroupingCriteria;
            try
            {
                var oldData = await UOW.KpiProductGroupingCriteriaRepository.Get(KpiProductGroupingCriteria.Id);

                await UOW.KpiProductGroupingCriteriaRepository.Update(KpiProductGroupingCriteria);

                KpiProductGroupingCriteria = await UOW.KpiProductGroupingCriteriaRepository.Get(KpiProductGroupingCriteria.Id);
                Logging.CreateAuditLog(KpiProductGroupingCriteria, oldData, nameof(KpiProductGroupingCriteriaService));
                return KpiProductGroupingCriteria;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingCriteriaService));
            }
            return null;
        }

        public async Task<KpiProductGroupingCriteria> Delete(KpiProductGroupingCriteria KpiProductGroupingCriteria)
        {
            if (!await KpiProductGroupingCriteriaValidator.Delete(KpiProductGroupingCriteria))
                return KpiProductGroupingCriteria;

            try
            {
                await UOW.KpiProductGroupingCriteriaRepository.Delete(KpiProductGroupingCriteria);
                Logging.CreateAuditLog(new { }, KpiProductGroupingCriteria, nameof(KpiProductGroupingCriteriaService));
                return KpiProductGroupingCriteria;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingCriteriaService));
            }
            return null;
        }

        public async Task<List<KpiProductGroupingCriteria>> BulkDelete(List<KpiProductGroupingCriteria> KpiProductGroupingCriterias)
        {
            if (!await KpiProductGroupingCriteriaValidator.BulkDelete(KpiProductGroupingCriterias))
                return KpiProductGroupingCriterias;

            try
            {
                await UOW.KpiProductGroupingCriteriaRepository.BulkDelete(KpiProductGroupingCriterias);
                Logging.CreateAuditLog(new { }, KpiProductGroupingCriterias, nameof(KpiProductGroupingCriteriaService));
                return KpiProductGroupingCriterias;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingCriteriaService));
            }
            return null;

        }

        public async Task<List<KpiProductGroupingCriteria>> Import(List<KpiProductGroupingCriteria> KpiProductGroupingCriterias)
        {
            if (!await KpiProductGroupingCriteriaValidator.Import(KpiProductGroupingCriterias))
                return KpiProductGroupingCriterias;
            try
            {
                await UOW.KpiProductGroupingCriteriaRepository.BulkMerge(KpiProductGroupingCriterias);

                Logging.CreateAuditLog(KpiProductGroupingCriterias, new { }, nameof(KpiProductGroupingCriteriaService));
                return KpiProductGroupingCriterias;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingCriteriaService));
            }
            return null;
        }

        public async Task<KpiProductGroupingCriteriaFilter> ToFilter(KpiProductGroupingCriteriaFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<KpiProductGroupingCriteriaFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                KpiProductGroupingCriteriaFilter subFilter = new KpiProductGroupingCriteriaFilter();
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
