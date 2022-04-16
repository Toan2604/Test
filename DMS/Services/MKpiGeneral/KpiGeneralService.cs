using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Services.MOrganization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Handlers.Configuration;

namespace DMS.Services.MKpiGeneral
{
    public interface IKpiGeneralService : IServiceScoped
    {
        Task<int> Count(KpiGeneralFilter KpiGeneralFilter);
        Task<List<KpiGeneral>> List(KpiGeneralFilter KpiGeneralFilter);
        Task<KpiGeneral> Get(long Id);
        Task<KpiGeneral> Create(KpiGeneral KpiGeneral);
        Task<KpiGeneral> Update(KpiGeneral KpiGeneral);
        Task<KpiGeneral> Delete(KpiGeneral KpiGeneral);
        Task<List<KpiGeneral>> BulkDelete(List<KpiGeneral> KpiGenerals);
        Task<List<KpiGeneral>> Import(List<KpiGeneral> KpiGenerals);
        Task<List<AppUser>> ListAppUser(AppUserFilter appUserFilter, IdFilter KpiYearId);
        Task<int> CountAppUser(AppUserFilter appUserFilter, IdFilter KpiYearId);
        Task<KpiGeneralFilter> ToFilter(KpiGeneralFilter KpiGeneralFilter);
    }

    public class KpiGeneralService : BaseService, IKpiGeneralService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IKpiGeneralValidator KpiGeneralValidator;
        private IOrganizationService OrganizationService;
        private IRabbitManager RabbitManager;

        public KpiGeneralService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IKpiGeneralValidator KpiGeneralValidator,
            IOrganizationService OrganizationService,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.KpiGeneralValidator = KpiGeneralValidator;
            this.OrganizationService = OrganizationService;
            this.RabbitManager = RabbitManager;
        }
        public async Task<int> Count(KpiGeneralFilter KpiGeneralFilter)
        {
            try
            {
                int result = await UOW.KpiGeneralRepository.Count(KpiGeneralFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiGeneralService));
            }
            return 0;
        }

        public async Task<List<KpiGeneral>> List(KpiGeneralFilter KpiGeneralFilter)
        {
            try
            {
                List<KpiGeneral> KpiGenerals = await UOW.KpiGeneralRepository.List(KpiGeneralFilter);
                return KpiGenerals;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiGeneralService));
            }
            return null;
        }
        public async Task<KpiGeneral> Get(long Id)
        {
            KpiGeneral KpiGeneral = await UOW.KpiGeneralRepository.Get(Id);
            if (KpiGeneral == null)
                return null;
            return KpiGeneral;
        }

        public async Task<KpiGeneral> Create(KpiGeneral KpiGeneral)
        {
            if (!await KpiGeneralValidator.Create(KpiGeneral))
                return KpiGeneral;

            try
            {
                KpiGeneral.CreatedAt = StaticParams.DateTimeNow;
                List<KpiGeneral> KpiGenerals = new List<KpiGeneral>();
                if (KpiGeneral.EmployeeIds != null && KpiGeneral.EmployeeIds.Any())
                {
                    foreach (var EmployeeId in KpiGeneral.EmployeeIds)
                    {
                        var newObj = Utils.Clone(KpiGeneral);
                        newObj.EmployeeId = EmployeeId;
                        newObj.CreatorId = CurrentContext.UserId;
                        newObj.RowId = Guid.NewGuid();
                        KpiGenerals.Add(newObj);
                    }
                }
                await UOW.KpiGeneralRepository.BulkMerge(KpiGenerals);
                await Sync(KpiGenerals);
                Logging.CreateAuditLog(KpiGeneral, new { }, nameof(KpiGeneralService));
                return KpiGeneral;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiGeneralService));
            }
            return null;
        }

        public async Task<KpiGeneral> Update(KpiGeneral KpiGeneral)
        {
            if (!await KpiGeneralValidator.Update(KpiGeneral))
                return KpiGeneral;
            try
            {
                var oldData = await UOW.KpiGeneralRepository.Get(KpiGeneral.Id);


                await UOW.KpiGeneralRepository.Update(KpiGeneral);


                var newData = await UOW.KpiGeneralRepository.Get(KpiGeneral.Id);
                Logging.CreateAuditLog(newData, oldData, nameof(KpiGeneralService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiGeneralService));
            }
            return null;
        }

        public async Task<KpiGeneral> Delete(KpiGeneral KpiGeneral)
        {
            if (!await KpiGeneralValidator.Delete(KpiGeneral))
                return KpiGeneral;

            try
            {

                await UOW.KpiGeneralRepository.Delete(KpiGeneral);

                Logging.CreateAuditLog(new { }, KpiGeneral, nameof(KpiGeneralService));
                return KpiGeneral;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiGeneralService));
            }
            return null;
        }

        public async Task<List<KpiGeneral>> BulkDelete(List<KpiGeneral> KpiGenerals)
        {
            if (!await KpiGeneralValidator.BulkDelete(KpiGenerals))
                return KpiGenerals;

            try
            {

                await UOW.KpiGeneralRepository.BulkDelete(KpiGenerals);

                Logging.CreateAuditLog(new { }, KpiGenerals, nameof(KpiGeneralService));
                return KpiGenerals;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiGeneralService));
            }
            return null;
        }

        public async Task<List<KpiGeneral>> Import(List<KpiGeneral> KpiGenerals)
        {
            if (!await KpiGeneralValidator.Import(KpiGenerals))
                return KpiGenerals;
            try
            {

                await UOW.KpiGeneralRepository.BulkMerge(KpiGenerals);


                Logging.CreateAuditLog(KpiGenerals, new { }, nameof(KpiGeneralService));
                return KpiGenerals;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiGeneralService));
            }
            return null;
        }
        public async Task<List<AppUser>> ListAppUser(AppUserFilter AppUserFilter, IdFilter KpiYearId)
        {
            try
            {
                KpiGeneralFilter KpiGeneralFilter = new KpiGeneralFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    KpiYearId = KpiYearId,
                    Selects = KpiGeneralSelect.Id | KpiGeneralSelect.Employee
                };

                var KpiGenerals = await UOW.KpiGeneralRepository.List(KpiGeneralFilter);
                var AppUserIds = KpiGenerals.Select(x => x.EmployeeId).ToList();
                if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
                AppUserFilter.Id.NotIn = AppUserIds;
                AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName | AppUserSelect.Phone | AppUserSelect.Email;

                var AppUsers = await UOW.AppUserRepository.List(AppUserFilter);
                return AppUsers;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiGeneralService));
            }
            return null;

        }

        public async Task<int> CountAppUser(AppUserFilter AppUserFilter, IdFilter KpiYearId)
        {
            try
            {
                KpiGeneralFilter KpiGeneralFilter = new KpiGeneralFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    KpiYearId = KpiYearId,
                    Selects = KpiGeneralSelect.Id | KpiGeneralSelect.Employee
                };

                var KpiGenerals = await UOW.KpiGeneralRepository.List(KpiGeneralFilter);
                var AppUserIds = KpiGenerals.Select(x => x.EmployeeId).ToList();
                if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
                AppUserFilter.Id.NotIn = AppUserIds;

                var count = await UOW.AppUserRepository.Count(AppUserFilter);
                return count;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiGeneralService));
            }
            return 0;

        }
        public async Task<KpiGeneralFilter> ToFilter(KpiGeneralFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<KpiGeneralFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;

            List<Organization> Organizations = await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.ALL,
                OrderBy = OrganizationOrder.Id,
                OrderType = OrderType.ASC
            });

            foreach (var currentFilter in CurrentContext.Filters)
            {
                KpiGeneralFilter subFilter = new KpiGeneralFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.OrganizationId))
                    {
                        var organizationIds = FilterOrganization(Organizations, FilterPermissionDefinition.IdFilter);
                        IdFilter IdFilter = new IdFilter { In = organizationIds };
                        subFilter.OrganizationId = FilterBuilder.Merge(subFilter.OrganizationId, IdFilter);
                    }

                    if (FilterPermissionDefinition.Name == nameof(subFilter.AppUserId))
                        subFilter.AppUserId = FilterBuilder.Merge(subFilter.AppUserId, FilterPermissionDefinition.IdFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.KpiYearId))
                        subFilter.KpiYearId = FilterBuilder.Merge(subFilter.KpiYearId, FilterPermissionDefinition.IdFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.CreatedAt))
                        subFilter.CreatedAt = FilterBuilder.Merge(subFilter.CreatedAt, FilterPermissionDefinition.DateFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.UpdatedAt))
                        subFilter.UpdatedAt = FilterBuilder.Merge(subFilter.UpdatedAt, FilterPermissionDefinition.DateFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.StatusId))
                        subFilter.StatusId = FilterBuilder.Merge(subFilter.StatusId, FilterPermissionDefinition.IdFilter);

                    if (FilterPermissionDefinition.Name == nameof(CurrentContext.UserId) && FilterPermissionDefinition.IdFilter != null)
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.IS.Id)
                        {
                            if (subFilter.AppUserId == null) subFilter.AppUserId = new IdFilter { };
                            subFilter.AppUserId.Equal = CurrentContext.UserId;
                        }
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.ISNT.Id)
                        {
                            if (subFilter.AppUserId == null) subFilter.AppUserId = new IdFilter { };
                            subFilter.AppUserId.NotEqual = CurrentContext.UserId;
                        }
                    }
                }
            }
            return filter;
        }

        private async Task Sync(List<KpiGeneral> KpiGenerals)
        {
            List<long> KpiGeneralIds = KpiGenerals.Select(x => x.Id).ToList();
            var count = KpiGeneralIds.Count();
            for (int i = 0; i < count; i += 1000)
            {
                int skip = i;
                int take = 1000;
                var Batch = KpiGeneralIds.Skip(skip).Take(take).ToList();
                var kpiGenerals = await UOW.KpiGeneralRepository.List(Batch);
                RabbitManager.PublishList(kpiGenerals, RoutingKeyEnum.KpiGeneralSync.Code);
            }
        }
    }
}
