using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MProblemType
{
    public interface IProblemTypeService : IServiceScoped
    {
        Task<int> Count(ProblemTypeFilter ProblemTypeFilter);
        Task<List<ProblemType>> List(ProblemTypeFilter ProblemTypeFilter);
        Task<ProblemType> Get(long Id);
        Task<ProblemType> Create(ProblemType ProblemType);
        Task<ProblemType> Update(ProblemType ProblemType);
        Task<ProblemType> Delete(ProblemType ProblemType);
        Task<List<ProblemType>> BulkDelete(List<ProblemType> ProblemTypes);
        Task<List<ProblemType>> Import(List<ProblemType> ProblemTypes);
        Task<ProblemTypeFilter> ToFilter(ProblemTypeFilter ProblemTypeFilter);
    }

    public class ProblemTypeService : BaseService, IProblemTypeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IProblemTypeValidator ProblemTypeValidator;

        public ProblemTypeService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IProblemTypeValidator ProblemTypeValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ProblemTypeValidator = ProblemTypeValidator;
        }
        public async Task<int> Count(ProblemTypeFilter ProblemTypeFilter)
        {
            try
            {
                int result = await UOW.ProblemTypeRepository.Count(ProblemTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProblemTypeService));
            }
            return 0;
        }

        public async Task<List<ProblemType>> List(ProblemTypeFilter ProblemTypeFilter)
        {
            try
            {
                List<ProblemType> ProblemTypes = await UOW.ProblemTypeRepository.List(ProblemTypeFilter);
                return ProblemTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProblemTypeService));
            }
            return null;
        }
        public async Task<ProblemType> Get(long Id)
        {
            ProblemType ProblemType = await UOW.ProblemTypeRepository.Get(Id);
            if (ProblemType == null)
                return null;
            return ProblemType;
        }

        public async Task<ProblemType> Create(ProblemType ProblemType)
        {
            if (!await ProblemTypeValidator.Create(ProblemType))
                return ProblemType;

            try
            {

                await UOW.ProblemTypeRepository.Create(ProblemType);

                ProblemType = await UOW.ProblemTypeRepository.Get(ProblemType.Id);
                Logging.CreateAuditLog(ProblemType, new { }, nameof(ProblemTypeService));
                return ProblemType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProblemTypeService));
            }
            return null;
        }

        public async Task<ProblemType> Update(ProblemType ProblemType)
        {
            if (!await ProblemTypeValidator.Update(ProblemType))
                return ProblemType;
            try
            {
                var oldData = await UOW.ProblemTypeRepository.Get(ProblemType.Id);


                await UOW.ProblemTypeRepository.Update(ProblemType);


                ProblemType = await UOW.ProblemTypeRepository.Get(ProblemType.Id);
                Logging.CreateAuditLog(ProblemType, oldData, nameof(ProblemTypeService));
                return ProblemType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProblemTypeService));
            }
            return null;
        }

        public async Task<ProblemType> Delete(ProblemType ProblemType)
        {
            if (!await ProblemTypeValidator.Delete(ProblemType))
                return ProblemType;

            try
            {

                await UOW.ProblemTypeRepository.Delete(ProblemType);

                Logging.CreateAuditLog(new { }, ProblemType, nameof(ProblemTypeService));
                return ProblemType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProblemTypeService));
            }
            return null;
        }

        public async Task<List<ProblemType>> BulkDelete(List<ProblemType> ProblemTypes)
        {
            if (!await ProblemTypeValidator.BulkDelete(ProblemTypes))
                return ProblemTypes;

            try
            {

                await UOW.ProblemTypeRepository.BulkDelete(ProblemTypes);

                Logging.CreateAuditLog(new { }, ProblemTypes, nameof(ProblemTypeService));
                return ProblemTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProblemTypeService));
            }
            return null;
        }

        public async Task<List<ProblemType>> Import(List<ProblemType> ProblemTypes)
        {
            if (!await ProblemTypeValidator.Import(ProblemTypes))
                return ProblemTypes;
            try
            {

                await UOW.ProblemTypeRepository.BulkMerge(ProblemTypes);


                Logging.CreateAuditLog(ProblemTypes, new { }, nameof(ProblemTypeService));
                return ProblemTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProblemTypeService));
            }
            return null;
        }

        public async Task<ProblemTypeFilter> ToFilter(ProblemTypeFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<ProblemTypeFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                ProblemTypeFilter subFilter = new ProblemTypeFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.Code))






                        subFilter.Code = FilterBuilder.Merge(subFilter.Code, FilterPermissionDefinition.StringFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.Name))






                        subFilter.Name = FilterBuilder.Merge(subFilter.Name, FilterPermissionDefinition.StringFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.StatusId))
                        subFilter.StatusId = FilterBuilder.Merge(subFilter.StatusId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(CurrentContext.UserId) && FilterPermissionDefinition.IdFilter != null)
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
