using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MStoreScoutingType
{
    public interface IStoreScoutingTypeService : IServiceScoped
    {
        Task<int> Count(StoreScoutingTypeFilter StoreScoutingTypeFilter);
        Task<List<StoreScoutingType>> List(StoreScoutingTypeFilter StoreScoutingTypeFilter);
        Task<StoreScoutingType> Get(long Id);
        Task<StoreScoutingType> Create(StoreScoutingType StoreScoutingType);
        Task<StoreScoutingType> Update(StoreScoutingType StoreScoutingType);
        Task<StoreScoutingType> Delete(StoreScoutingType StoreScoutingType);
        Task<List<StoreScoutingType>> BulkDelete(List<StoreScoutingType> StoreScoutingTypes);
        Task<List<StoreScoutingType>> Import(List<StoreScoutingType> StoreScoutingTypes);
        Task<StoreScoutingTypeFilter> ToFilter(StoreScoutingTypeFilter StoreScoutingTypeFilter);
    }

    public class StoreScoutingTypeService : BaseService, IStoreScoutingTypeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IStoreScoutingTypeValidator StoreScoutingTypeValidator;

        public StoreScoutingTypeService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IStoreScoutingTypeValidator StoreScoutingTypeValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.StoreScoutingTypeValidator = StoreScoutingTypeValidator;
        }
        public async Task<int> Count(StoreScoutingTypeFilter StoreScoutingTypeFilter)
        {
            try
            {
                int result = await UOW.StoreScoutingTypeRepository.Count(StoreScoutingTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreScoutingTypeService));
            }
            return 0;
        }

        public async Task<List<StoreScoutingType>> List(StoreScoutingTypeFilter StoreScoutingTypeFilter)
        {
            try
            {
                List<StoreScoutingType> StoreScoutingTypes = await UOW.StoreScoutingTypeRepository.List(StoreScoutingTypeFilter);
                return StoreScoutingTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreScoutingTypeService));
            }
            return null;
        }
        public async Task<StoreScoutingType> Get(long Id)
        {
            StoreScoutingType StoreScoutingType = await UOW.StoreScoutingTypeRepository.Get(Id);
            if (StoreScoutingType == null)
                return null;
            return StoreScoutingType;
        }

        public async Task<StoreScoutingType> Create(StoreScoutingType StoreScoutingType)
        {
            if (!await StoreScoutingTypeValidator.Create(StoreScoutingType))
                return StoreScoutingType;

            try
            {

                await UOW.StoreScoutingTypeRepository.Create(StoreScoutingType);

                StoreScoutingType = await UOW.StoreScoutingTypeRepository.Get(StoreScoutingType.Id);
                Logging.CreateAuditLog(StoreScoutingType, new { }, nameof(StoreScoutingTypeService));
                return StoreScoutingType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreScoutingTypeService));
            }
            return null;
        }

        public async Task<StoreScoutingType> Update(StoreScoutingType StoreScoutingType)
        {
            if (!await StoreScoutingTypeValidator.Update(StoreScoutingType))
                return StoreScoutingType;
            try
            {
                var oldData = await UOW.StoreScoutingTypeRepository.Get(StoreScoutingType.Id);


                await UOW.StoreScoutingTypeRepository.Update(StoreScoutingType);


                StoreScoutingType = await UOW.StoreScoutingTypeRepository.Get(StoreScoutingType.Id);
                Logging.CreateAuditLog(StoreScoutingType, oldData, nameof(StoreScoutingTypeService));
                return StoreScoutingType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreScoutingTypeService));
            }
            return null;
        }

        public async Task<StoreScoutingType> Delete(StoreScoutingType StoreScoutingType)
        {
            if (!await StoreScoutingTypeValidator.Delete(StoreScoutingType))
                return StoreScoutingType;

            try
            {

                await UOW.StoreScoutingTypeRepository.Delete(StoreScoutingType);

                Logging.CreateAuditLog(new { }, StoreScoutingType, nameof(StoreScoutingTypeService));
                return StoreScoutingType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreScoutingTypeService));
            }
            return null;
        }

        public async Task<List<StoreScoutingType>> BulkDelete(List<StoreScoutingType> StoreScoutingTypes)
        {
            if (!await StoreScoutingTypeValidator.BulkDelete(StoreScoutingTypes))
                return StoreScoutingTypes;

            try
            {

                await UOW.StoreScoutingTypeRepository.BulkDelete(StoreScoutingTypes);

                Logging.CreateAuditLog(new { }, StoreScoutingTypes, nameof(StoreScoutingTypeService));
                return StoreScoutingTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreScoutingTypeService));
            }
            return null;
        }

        public async Task<List<StoreScoutingType>> Import(List<StoreScoutingType> StoreScoutingTypes)
        {
            if (!await StoreScoutingTypeValidator.Import(StoreScoutingTypes))
                return StoreScoutingTypes;
            try
            {

                await UOW.StoreScoutingTypeRepository.BulkMerge(StoreScoutingTypes);


                Logging.CreateAuditLog(StoreScoutingTypes, new { }, nameof(StoreScoutingTypeService));
                return StoreScoutingTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreScoutingTypeService));
            }
            return null;
        }

        public async Task<StoreScoutingTypeFilter> ToFilter(StoreScoutingTypeFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<StoreScoutingTypeFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                StoreScoutingTypeFilter subFilter = new StoreScoutingTypeFilter();
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
