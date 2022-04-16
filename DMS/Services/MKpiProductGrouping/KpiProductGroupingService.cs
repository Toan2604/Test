using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiProductGrouping
{
    public interface IKpiProductGroupingService : IServiceScoped
    {
        Task<int> Count(KpiProductGroupingFilter KpiProductGroupingFilter);
        Task<List<KpiProductGrouping>> List(KpiProductGroupingFilter KpiProductGroupingFilter);
        Task<KpiProductGrouping> Get(long Id);
        Task<KpiProductGrouping> Create(KpiProductGrouping KpiProductGrouping);
        Task<KpiProductGrouping> Update(KpiProductGrouping KpiProductGrouping);
        Task<KpiProductGrouping> Delete(KpiProductGrouping KpiProductGrouping);
        Task<List<KpiProductGrouping>> BulkDelete(List<KpiProductGrouping> KpiProductGroupings);
        Task<List<KpiProductGrouping>> Import(List<KpiProductGrouping> KpiProductGroupings);
        Task<KpiProductGroupingFilter> ToFilter(KpiProductGroupingFilter KpiProductGroupingFilter);
        Task<int> CountAppUser(AppUserFilter AppUserFilter, IdFilter KpiYearId, IdFilter KpiPeriodId, IdFilter KpiProductGroupingTypeId);
        Task<List<AppUser>> ListAppUser(AppUserFilter AppUserFilter, IdFilter KpiYearId, IdFilter KpiPeriodId, IdFilter KpiProductGroupingTypeId);
    }

    public class KpiProductGroupingService : BaseService, IKpiProductGroupingService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IKpiProductGroupingValidator KpiProductGroupingValidator;

        public KpiProductGroupingService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IKpiProductGroupingValidator KpiProductGroupingValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.KpiProductGroupingValidator = KpiProductGroupingValidator;
        }
        public async Task<int> Count(KpiProductGroupingFilter KpiProductGroupingFilter)
        {
            try
            {
                int result = await UOW.KpiProductGroupingRepository.Count(KpiProductGroupingFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingService));
            }
            return 0;
        }

        public async Task<List<KpiProductGrouping>> List(KpiProductGroupingFilter KpiProductGroupingFilter)
        {
            try
            {
                List<KpiProductGrouping> KpiProductGroupings = await UOW.KpiProductGroupingRepository.List(KpiProductGroupingFilter);
                return KpiProductGroupings;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingService));
            }
            return null;
        }

        public async Task<KpiProductGrouping> Get(long Id)
        {
            KpiProductGrouping KpiProductGrouping = await UOW.KpiProductGroupingRepository.Get(Id);
            if (KpiProductGrouping == null)
                return null;
            return KpiProductGrouping;
        }
        public async Task<KpiProductGrouping> Create(KpiProductGrouping KpiProductGrouping)
        {
            if (!await KpiProductGroupingValidator.Create(KpiProductGrouping))
                return KpiProductGrouping;

            try
            {
                KpiProductGrouping.CreatedAt = StaticParams.DateTimeNow;

                List<KpiProductGrouping> KpiProductGroupings = new List<KpiProductGrouping>();
                if (KpiProductGrouping.Employees != null && KpiProductGrouping.Employees.Any())
                {
                    foreach (var Employee in KpiProductGrouping.Employees)
                    {
                        var newObj = Utils.Clone(KpiProductGrouping);
                        newObj.EmployeeId = Employee.Id;
                        newObj.CreatorId = CurrentContext.UserId;
                        newObj.RowId = Guid.NewGuid();
                        KpiProductGroupings.Add(newObj);
                    } // với mỗi nhân viên tạo ra 1 kpiProductGrouping
                }

                await UOW.KpiProductGroupingRepository.BulkMerge(KpiProductGroupings);
                Logging.CreateAuditLog(KpiProductGrouping, new { }, nameof(KpiProductGroupingService));
                return KpiProductGrouping;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingService));
            }
            return null;
        }

        public async Task<KpiProductGrouping> Update(KpiProductGrouping KpiProductGrouping)
        {
            if (!await KpiProductGroupingValidator.Update(KpiProductGrouping))
                return KpiProductGrouping;
            try
            {
                var oldData = await UOW.KpiProductGroupingRepository.Get(KpiProductGrouping.Id);

                await UOW.KpiProductGroupingRepository.Update(KpiProductGrouping);

                KpiProductGrouping = await UOW.KpiProductGroupingRepository.Get(KpiProductGrouping.Id);
                Logging.CreateAuditLog(KpiProductGrouping, oldData, nameof(KpiProductGroupingService));
                return KpiProductGrouping;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingService));
            }
            return null;
        }

        public async Task<KpiProductGrouping> Delete(KpiProductGrouping KpiProductGrouping)
        {
            if (!await KpiProductGroupingValidator.Delete(KpiProductGrouping))
                return KpiProductGrouping;

            try
            {
                await UOW.KpiProductGroupingRepository.Delete(KpiProductGrouping);
                Logging.CreateAuditLog(new { }, KpiProductGrouping, nameof(KpiProductGroupingService));
                return KpiProductGrouping;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingService));
            }
            return null;
        }

        public async Task<List<KpiProductGrouping>> BulkDelete(List<KpiProductGrouping> KpiProductGroupings)
        {
            if (!await KpiProductGroupingValidator.BulkDelete(KpiProductGroupings))
                return KpiProductGroupings;

            try
            {
                await UOW.KpiProductGroupingRepository.BulkDelete(KpiProductGroupings);
                Logging.CreateAuditLog(new { }, KpiProductGroupings, nameof(KpiProductGroupingService));
                return KpiProductGroupings;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingService));
            }
            return null;

        }

        public async Task<List<KpiProductGrouping>> Import(List<KpiProductGrouping> KpiProductGroupings)
        {
            if (!await KpiProductGroupingValidator.Import(KpiProductGroupings))
                return KpiProductGroupings;
            try
            {
                await UOW.KpiProductGroupingRepository.BulkMerge(KpiProductGroupings);

                Logging.CreateAuditLog(KpiProductGroupings, new { }, nameof(KpiProductGroupingService));
                return KpiProductGroupings;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingService));
            }
            return null;
        }

        public async Task<KpiProductGroupingFilter> ToFilter(KpiProductGroupingFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<KpiProductGroupingFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                KpiProductGroupingFilter subFilter = new KpiProductGroupingFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.OrganizationId))
                        subFilter.OrganizationId = FilterBuilder.Merge(subFilter.OrganizationId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.KpiYearId))
                        subFilter.KpiYearId = FilterBuilder.Merge(subFilter.KpiYearId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.KpiPeriodId))
                        subFilter.KpiPeriodId = FilterBuilder.Merge(subFilter.KpiPeriodId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.KpiProductGroupingTypeId))
                        subFilter.KpiProductGroupingTypeId = FilterBuilder.Merge(subFilter.KpiProductGroupingTypeId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StatusId))
                        subFilter.StatusId = FilterBuilder.Merge(subFilter.StatusId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.EmployeeId))
                        subFilter.EmployeeId = FilterBuilder.Merge(subFilter.EmployeeId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.CreatorId))
                        subFilter.CreatorId = FilterBuilder.Merge(subFilter.CreatorId, FilterPermissionDefinition.IdFilter);
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

        public async Task<int> CountAppUser(AppUserFilter AppUserFilter, IdFilter KpiYearId, IdFilter KpiPeriodId, IdFilter KpiProductGroupingTypeId)
        {
            try
            {
                KpiProductGroupingFilter KpiProductGroupingFilter = new KpiProductGroupingFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    KpiYearId = KpiYearId,
                    KpiPeriodId = KpiPeriodId,
                    KpiProductGroupingTypeId = KpiProductGroupingTypeId,
                    Selects = KpiProductGroupingSelect.Id | KpiProductGroupingSelect.Employee
                };

                var KpiProductGroupings = await UOW.KpiProductGroupingRepository.List(KpiProductGroupingFilter);
                var AppUserIds = KpiProductGroupings.Select(x => x.EmployeeId).ToList();

                if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
                AppUserFilter.Id.NotIn = AppUserIds;

                var count = await UOW.AppUserRepository.Count(AppUserFilter);
                return count;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingService));
            }
            return 0;
        }

        public async Task<List<AppUser>> ListAppUser(AppUserFilter AppUserFilter, IdFilter KpiYearId, IdFilter KpiPeriodId, IdFilter KpiProductGroupingTypeId)
        {
            try
            {
                KpiProductGroupingFilter KpiProductGroupingFilter = new KpiProductGroupingFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    KpiYearId = KpiYearId,
                    KpiPeriodId = KpiPeriodId,
                    KpiProductGroupingTypeId = KpiProductGroupingTypeId,
                    Selects = KpiProductGroupingSelect.Id | KpiProductGroupingSelect.Employee
                };

                var KpiProductGroupings = await UOW.KpiProductGroupingRepository.List(KpiProductGroupingFilter);
                var AppUserIds = KpiProductGroupings.Select(x => x.EmployeeId).ToList();
                if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
                AppUserFilter.Id.NotIn = AppUserIds;
                AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName | AppUserSelect.Phone | AppUserSelect.Email;

                var AppUsers = await UOW.AppUserRepository.List(AppUserFilter);
                return AppUsers;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingService));
            }
            return null;
        }
    }
}
