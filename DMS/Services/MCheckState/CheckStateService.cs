using TrueSight.Common;
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

namespace DMS.Services.MCheckState
{
    public interface ICheckStateService :  IServiceScoped
    {
        Task<int> Count(CheckStateFilter CheckStateFilter);
        Task<List<CheckState>> List(CheckStateFilter CheckStateFilter);
        Task<CheckState> Get(long Id);
        Task<CheckState> Create(CheckState CheckState);
        Task<CheckState> Update(CheckState CheckState);
        Task<CheckState> Delete(CheckState CheckState);
        Task<List<CheckState>> BulkDelete(List<CheckState> CheckStates);
        Task<List<CheckState>> Import(List<CheckState> CheckStates);
        Task<CheckStateFilter> ToFilter(CheckStateFilter CheckStateFilter);
    }

    public class CheckStateService : BaseService, ICheckStateService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        
        private ICheckStateValidator CheckStateValidator;

        public CheckStateService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            ICheckStateValidator CheckStateValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.Logging = Logging;
           
            this.CheckStateValidator = CheckStateValidator;
        }
        public async Task<int> Count(CheckStateFilter CheckStateFilter)
        {
            try
            {
                int result = await UOW.CheckStateRepository.Count(CheckStateFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(CheckStateService));
            }
            return 0;
        }

        public async Task<List<CheckState>> List(CheckStateFilter CheckStateFilter)
        {
            try
            {
                List<CheckState> CheckStates = await UOW.CheckStateRepository.List(CheckStateFilter);
                return CheckStates;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(CheckStateService));
            }
            return null;
        }

        public async Task<CheckState> Get(long Id)
        {
            CheckState CheckState = await UOW.CheckStateRepository.Get(Id);
            await CheckStateValidator.Get(CheckState);
            if (CheckState == null)
                return null;
            return CheckState;
        }
        
        public async Task<CheckState> Create(CheckState CheckState)
        {
            if (!await CheckStateValidator.Create(CheckState))
                return CheckState;

            try
            {
                await UOW.CheckStateRepository.Create(CheckState);
                CheckState = await UOW.CheckStateRepository.Get(CheckState.Id);
                Logging.CreateAuditLog(CheckState, new { }, nameof(CheckStateService));
                return CheckState;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(CheckStateService));
            }
            return null;
        }

        public async Task<CheckState> Update(CheckState CheckState)
        {
            if (!await CheckStateValidator.Update(CheckState))
                return CheckState;
            try
            {
                var oldData = await UOW.CheckStateRepository.Get(CheckState.Id);

                await UOW.CheckStateRepository.Update(CheckState);

                CheckState = await UOW.CheckStateRepository.Get(CheckState.Id);
                Logging.CreateAuditLog(CheckState, oldData, nameof(CheckStateService));
                return CheckState;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(CheckStateService));
            }
            return null;
        }

        public async Task<CheckState> Delete(CheckState CheckState)
        {
            if (!await CheckStateValidator.Delete(CheckState))
                return CheckState;

            try
            {
                await UOW.CheckStateRepository.Delete(CheckState);
                Logging.CreateAuditLog(new { }, CheckState, nameof(CheckStateService));
                return CheckState;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(CheckStateService));
            }
            return null;
        }

        public async Task<List<CheckState>> BulkDelete(List<CheckState> CheckStates)
        {
            if (!await CheckStateValidator.BulkDelete(CheckStates))
                return CheckStates;

            try
            {
                await UOW.CheckStateRepository.BulkDelete(CheckStates);
                Logging.CreateAuditLog(new { }, CheckStates, nameof(CheckStateService));
                return CheckStates;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(CheckStateService));
            }
            return null;

        }
        
        public async Task<List<CheckState>> Import(List<CheckState> CheckStates)
        {
            if (!await CheckStateValidator.Import(CheckStates))
                return CheckStates;
            try
            {
                await UOW.CheckStateRepository.BulkMerge(CheckStates);

                Logging.CreateAuditLog(CheckStates, new { }, nameof(CheckStateService));
                return CheckStates;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(CheckStateService));
            }
            return null;
        }     
        
        public async Task<CheckStateFilter> ToFilter(CheckStateFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<CheckStateFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                CheckStateFilter subFilter = new CheckStateFilter();
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

        private void Sync(List<CheckState> CheckStates)
        {
            
        }

    }
}
