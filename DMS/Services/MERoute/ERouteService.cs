using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers.Configuration;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Services.MAppUser;
using DMS.Services.MNotification;
using DMS.Services.MOrganization;
using DMS.Services.MStore;
using DMS.Services.MWorkflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
namespace DMS.Services.MERoute
{
    public interface IERouteService : IServiceScoped
    {
        Task<int> Count(ERouteFilter ERouteFilter);
        Task<List<ERoute>> List(ERouteFilter ERouteFilter);
        Task<int> CountNew(ERouteFilter ERouteFilter);
        Task<List<ERoute>> ListNew(ERouteFilter ERouteFilter);
        Task<int> CountPending(ERouteFilter ERouteFilter);
        Task<List<ERoute>> ListPending(ERouteFilter ERouteFilter);
        Task<int> CountCompleted(ERouteFilter ERouteFilter);
        Task<List<ERoute>> ListCompleted(ERouteFilter ERouteFilter);
        Task<ERoute> Get(long Id);
        Task<ERoute> GetDetail(long Id);
        Task<ERoute> Create(ERoute ERoute);
        Task<ERoute> Update(ERoute ERoute);
        Task<ERoute> Send(ERoute ERoute);
        Task<ERoute> Approve(ERoute ERoute);
        Task<ERoute> Reject(ERoute ERoute);
        Task<ERoute> Delete(ERoute ERoute);
        Task<List<ERoute>> BulkDelete(List<ERoute> ERoutes);
        Task<List<ERoute>> Import(List<ERoute> ERoutes);
        Task<ERouteFilter> ToFilter(ERouteFilter ERouteFilter);
        Task<int> CountStore(StoreFilter StoreFilter, long? AppUserId);
        Task<List<Store>> ListStore(StoreFilter StoreFilter, long? AppUserId);
    }
    public class ERouteService : BaseService, IERouteService
    {
        private IUOW UOW;
        private ILogging Logging;
        private INotificationService NotificationService;
        private ICurrentContext CurrentContext;
        private IERouteValidator ERouteValidator;
        private IERouteTemplate ERouteTemplate;
        private IStoreService StoreService;
        private IOrganizationService OrganizationService;
        private IAppUserService AppUserService;
        private IRabbitManager RabbitManager;
        private IWorkflowService WorkflowService;
        public ERouteService(
            IUOW UOW,
            ILogging Logging,
            INotificationService NotificationService,
            IERouteTemplate ERouteTemplate,
            ICurrentContext CurrentContext,
            IStoreService StoreService,
            IOrganizationService OrganizationService,
            IAppUserService AppUserService,
            IWorkflowService WorkflowService,
            IRabbitManager RabbitManager,
            IERouteValidator ERouteValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.NotificationService = NotificationService;
            this.ERouteTemplate = ERouteTemplate;
            this.CurrentContext = CurrentContext;
            this.StoreService = StoreService;
            this.OrganizationService = OrganizationService;
            this.AppUserService = AppUserService;
            this.WorkflowService = WorkflowService;
            this.RabbitManager = RabbitManager;
            this.ERouteValidator = ERouteValidator;
        }
        public async Task<int> Count(ERouteFilter ERouteFilter)
        {
            try
            {
                int result = await UOW.ERouteRepository.Count(ERouteFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteService));
            }
            return 0;
        }
        public async Task<List<ERoute>> List(ERouteFilter ERouteFilter)
        {
            try
            {
                List<ERoute> ERoutes = await UOW.ERouteRepository.List(ERouteFilter);
                return ERoutes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteService));
            }
            return null;
        }
        public async Task<int> CountNew(ERouteFilter ERouteFilter)
        {
            try
            {
                ERouteFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                int result = await UOW.ERouteRepository.CountNew(ERouteFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteService));
            }
            return 0;
        }
        public async Task<List<ERoute>> ListNew(ERouteFilter ERouteFilter)
        {
            try
            {
                ERouteFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                List<ERoute> ERoutes = await UOW.ERouteRepository.ListNew(ERouteFilter);
                return ERoutes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteService));
            }
            return null;
        }
        public async Task<int> CountPending(ERouteFilter ERouteFilter)
        {
            try
            {
                ERouteFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                int result = await UOW.ERouteRepository.CountPending(ERouteFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteService));
            }
            return 0;
        }
        public async Task<List<ERoute>> ListPending(ERouteFilter ERouteFilter)
        {
            try
            {
                ERouteFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                List<ERoute> ERoutes = await UOW.ERouteRepository.ListPending(ERouteFilter);
                return ERoutes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteService));
            }
            return null;
        }
        public async Task<int> CountCompleted(ERouteFilter ERouteFilter)
        {
            try
            {
                ERouteFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                int result = await UOW.ERouteRepository.CountCompleted(ERouteFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteService));
            }
            return 0;
        }
        public async Task<List<ERoute>> ListCompleted(ERouteFilter ERouteFilter)
        {
            try
            {
                ERouteFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                List<ERoute> ERoutes = await UOW.ERouteRepository.ListCompleted(ERouteFilter);
                return ERoutes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteService));
            }
            return null;
        }
        public async Task<ERoute> Get(long Id)
        {
            ERoute ERoute = await UOW.ERouteRepository.Get(Id);
            if (ERoute == null)
                return null;
            ERoute.RequestState = await WorkflowService.GetRequestState(ERoute.RowId);
            if (ERoute.RequestState == null)
            {
                ERoute.RequestWorkflowStepMappings = new List<RequestWorkflowStepMapping>();
                RequestWorkflowStepMapping RequestWorkflowStepMapping = new RequestWorkflowStepMapping
                {
                    AppUserId = ERoute.SaleEmployeeId,
                    CreatedAt = ERoute.CreatedAt,
                    UpdatedAt = ERoute.UpdatedAt,
                    RequestId = ERoute.RowId,
                    AppUser = ERoute.SaleEmployee == null ? null : new AppUser
                    {
                        Id = ERoute.SaleEmployee.Id,
                        Username = ERoute.SaleEmployee.Username,
                        DisplayName = ERoute.SaleEmployee.DisplayName,
                    },
                };
                ERoute.RequestWorkflowStepMappings.Add(RequestWorkflowStepMapping);
                RequestWorkflowStepMapping.WorkflowStateId = ERoute.RequestStateId;
                ERoute.RequestState = WorkflowService.GetRequestState(ERoute.RequestStateId);
                RequestWorkflowStepMapping.WorkflowState = WorkflowService.GetWorkflowState(RequestWorkflowStepMapping.WorkflowStateId);
            }
            else
            {
                ERoute.RequestStateId = ERoute.RequestState.Id;
                ERoute.RequestWorkflowStepMappings = await WorkflowService.ListRequestWorkflowStepMapping(ERoute.RowId);
            }
            return ERoute;
        }
        public async Task<ERoute> GetDetail(long Id)
        {
            ERoute ERoute = await Get(Id);
            return ERoute;
        }
        public async Task<ERoute> Create(ERoute ERoute)
        {
            if (!await ERouteValidator.Create(ERoute))
                return ERoute;
            try
            {
                var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                var SaleEmployee = await UOW.AppUserRepository.Get(ERoute.SaleEmployeeId);
                ERoute = await CalculateTime(ERoute);
                ERoute.CreatorId = CurrentContext.UserId;
                ERoute.OrganizationId = SaleEmployee.OrganizationId;
                ERoute.RequestStateId = RequestStateEnum.NEW.Id;
                await UOW.ERouteRepository.Create(ERoute);
                var GlobalUserNotifications = new List<GlobalUserNotification>();
                GlobalUserNotification CreatorUserNotification = ERouteTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, ERoute, CurrentUser, NotificationType.TOCREATOR);
                GlobalUserNotification GlobalUserNotification = ERouteTemplate.CreateAppUserNotification(CurrentUser.RowId, SaleEmployee.RowId, ERoute, CurrentUser, NotificationType.CREATE);
                GlobalUserNotifications.Add(CreatorUserNotification);
                GlobalUserNotifications.Add(GlobalUserNotification);
                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                Logging.CreateAuditLog(ERoute, new { }, nameof(ERouteService));
                await UOW.ERouteRepository.Used(new List<long> { ERoute.Id }, false);
                return await UOW.ERouteRepository.Get(ERoute.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteService));
            }
            return null;
        }
        public async Task<ERoute> Update(ERoute ERoute)
        {
            if (!await ERouteValidator.Update(ERoute))
                return ERoute;
            try
            {
                var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                var oldData = await UOW.ERouteRepository.Get(ERoute.Id);
                if (oldData.RequestStateId == RequestStateEnum.APPROVED.Id)
                {
                    oldData.StatusId = ERoute.StatusId;
                    await UOW.ERouteRepository.Update(oldData);
                }
                else
                {
                    if (oldData.SaleEmployeeId != ERoute.SaleEmployeeId)
                    {
                        var SaleEmployee = await UOW.AppUserRepository.Get(ERoute.SaleEmployeeId);
                        ERoute.OrganizationId = SaleEmployee.OrganizationId;
                    }
                    ERoute = await CalculateTime(ERoute);
                    await UOW.ERouteRepository.Update(ERoute);
                }
                var Recipient = await UOW.AppUserRepository.Get(ERoute.SaleEmployeeId);
                var GlobalUserNotifications = new List<GlobalUserNotification>();
                GlobalUserNotification CreatorUserNotification = ERouteTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, ERoute, CurrentUser, NotificationType.TOUPDATER);
                GlobalUserNotification GlobalUserNotification = ERouteTemplate.CreateAppUserNotification(CurrentUser.RowId, Recipient.RowId, ERoute, CurrentUser, NotificationType.UPDATE);
                GlobalUserNotifications.Add(CreatorUserNotification);
                GlobalUserNotifications.Add(GlobalUserNotification);
                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                var newData = await UOW.ERouteRepository.Get(ERoute.Id);
                Logging.CreateAuditLog(newData, oldData, nameof(ERouteService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteService));
            }
            return null;
        }
        public async Task<ERoute> Delete(ERoute ERoute)
        {
            if (!await ERouteValidator.Delete(ERoute))
                return ERoute;
            try
            {
                await UOW.ERouteRepository.Delete(ERoute);
                var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                var SaleEmployee = await UOW.AppUserRepository.Get(ERoute.SaleEmployeeId);
                var GlobalUserNotifications = new List<GlobalUserNotification>();
                GlobalUserNotification CreatorUserNotification = ERouteTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, ERoute, CurrentUser, NotificationType.TODELETER);
                GlobalUserNotification GlobalUserNotification = ERouteTemplate.CreateAppUserNotification(CurrentUser.RowId, SaleEmployee.RowId, ERoute, CurrentUser, NotificationType.DELETE);
                GlobalUserNotifications.Add(CreatorUserNotification);
                GlobalUserNotifications.Add(GlobalUserNotification);
                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                Logging.CreateAuditLog(new { }, ERoute, nameof(ERouteService));
                return ERoute;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteService));
            }
            return null;
        }
        public async Task<List<ERoute>> BulkDelete(List<ERoute> ERoutes)
        {
            if (!await ERouteValidator.BulkDelete(ERoutes))
                return ERoutes;
            try
            {
                await UOW.ERouteRepository.BulkDelete(ERoutes);
                Logging.CreateAuditLog(new { }, ERoutes, nameof(ERouteService));
                return ERoutes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteService));
            }
            return null;
        }
        public async Task<List<ERoute>> Import(List<ERoute> ERoutes)
        {
            if (!await ERouteValidator.Import(ERoutes))
                return ERoutes;
            try
            {
                await UOW.ERouteRepository.BulkMerge(ERoutes);
                Logging.CreateAuditLog(ERoutes, new { }, nameof(ERouteService));
                return ERoutes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteService));
            }
            return null;
        }
        public async Task<ERouteFilter> ToFilter(ERouteFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<ERouteFilter>();
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
                ERouteFilter subFilter = new ERouteFilter();
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
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StoreId))
                        subFilter.StoreId = FilterBuilder.Merge(subFilter.StoreId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.ERouteTypeId))
                        subFilter.ERouteTypeId = FilterBuilder.Merge(subFilter.ERouteTypeId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.RequestStateId))
                        subFilter.RequestStateId = FilterBuilder.Merge(subFilter.RequestStateId, FilterPermissionDefinition.IdFilter);
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
        public async Task<int> CountStore(StoreFilter StoreFilter, long? AppUserId)
        {
            if (AppUserId.HasValue)
                StoreFilter.AppUserId = new IdFilter { Equal = AppUserId };
            else
                StoreFilter.AppUserId = new IdFilter { Equal = CurrentContext.UserId };
            int count = await UOW.StoreRepository.Count(StoreFilter);
            return count;
        }
        public async Task<List<Store>> ListStore(StoreFilter StoreFilter, long? AppUserId)
        {
            if (AppUserId.HasValue)
                StoreFilter.AppUserId = new IdFilter { Equal = AppUserId };
            else
                StoreFilter.AppUserId = new IdFilter { Equal = CurrentContext.UserId };
            List<Store> Stores = await UOW.StoreRepository.List(StoreFilter);
            List<long> StoreIds = Stores.Select(s => s.Id).ToList();
            ERouteContentFilter ERouteContentFilter = new ERouteContentFilter
            {
                StoreId = new IdFilter { In = StoreIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = ERouteContentSelect.Id | ERouteContentSelect.Store,
            };
            List<ERouteContent> ERouteContents = await UOW.ERouteContentRepository.List(ERouteContentFilter);
            foreach (Store Store in Stores)
            {
                Store.HasEroute = ERouteContents.Where(e => e.StoreId == Store.Id).Count() > 0;
            }
            return Stores;
        }
        public async Task<ERoute> Send(ERoute ERoute)
        {
            if (ERoute.Id == 0)
                ERoute = await Create(ERoute);
            else
                ERoute = await Update(ERoute);
            if (ERoute.IsValidated == false)
                return ERoute;
            ERoute = await UOW.ERouteRepository.Get(ERoute.Id);
            Dictionary<string, string> Parameters = await MapParameters(ERoute);
            GenericEnum RequestState = await WorkflowService.Send(ERoute.RowId, WorkflowTypeEnum.EROUTE.Id, ERoute.OrganizationId, Parameters);
            ERoute.RequestStateId = RequestState.Id;
            await UOW.ERouteRepository.UpdateState(ERoute);
            await UOW.ERouteRepository.Used(new List<long> { ERoute.Id });
            return await Get(ERoute.Id);
        }
        public async Task<ERoute> Approve(ERoute ERoute)
        {
            ERoute = await Update(ERoute);
            if (ERoute.IsValidated == false)
                return ERoute;
            ERoute = await UOW.ERouteRepository.Get(ERoute.Id);
            Dictionary<string, string> Parameters = await MapParameters(ERoute);
            await WorkflowService.Approve(ERoute.RowId, WorkflowTypeEnum.EROUTE.Id, Parameters);
            RequestState RequestState = await WorkflowService.GetRequestState(ERoute.RowId);
            ERoute.RequestStateId = RequestState.Id;
            await UOW.ERouteRepository.UpdateState(ERoute);
            await UOW.ERouteRepository.Used(new List<long> { ERoute.Id });
            return await Get(ERoute.Id);
        }
        public async Task<ERoute> Reject(ERoute ERoute)
        {
            ERoute = await UOW.ERouteRepository.Get(ERoute.Id);
            Dictionary<string, string> Parameters = await MapParameters(ERoute);
            GenericEnum Action = await WorkflowService.Reject(ERoute.RowId, WorkflowTypeEnum.EROUTE.Id, Parameters);
            RequestState RequestState = await WorkflowService.GetRequestState(ERoute.RowId);
            ERoute.RequestStateId = RequestState.Id;
            await UOW.ERouteRepository.UpdateState(ERoute);
            await UOW.ERouteRepository.Used(new List<long> { ERoute.Id }, false);
            return await Get(ERoute.Id);
        }
        private async Task<Dictionary<string, string>> MapParameters(ERoute ERoute)
        {
            var AppUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            Parameters.Add(nameof(ERoute.Id), ERoute.Id.ToString());
            Parameters.Add(nameof(ERoute.Code), ERoute.Code);
            Parameters.Add(nameof(ERoute.SaleEmployeeId), ERoute.SaleEmployeeId.ToString());
            Parameters.Add(nameof(ERoute.TotalStoreCounter), ERoute.TotalStoreCounter.ToString());
            Parameters.Add(nameof(AppUser.DisplayName), AppUser.DisplayName);
            Parameters.Add(nameof(ERoute.RequestStateId), ERoute.RequestStateId.ToString());
            Parameters.Add(nameof(ERoute.ERouteTypeId), ERoute.ERouteTypeId.ToString());
            Parameters.Add(nameof(ERoute.OrganizationId), ERoute.OrganizationId.ToString());
            RequestWorkflowDefinitionMapping RequestWorkflowDefinitionMapping = await UOW.RequestWorkflowDefinitionMappingRepository.Get(ERoute.RowId);
            if (RequestWorkflowDefinitionMapping == null)
                Parameters.Add(nameof(RequestState), RequestStateEnum.NEW.Id.ToString());
            else
                Parameters.Add(nameof(RequestState), RequestWorkflowDefinitionMapping.RequestStateId.ToString());
            Parameters.Add("Username", CurrentContext.UserName);
            return Parameters;
        }
        private async Task<ERoute> CalculateTime(ERoute ERoute)
        {
            ERoute.StartDate = ERoute.StartDate.AddHours(CurrentContext.TimeZone).Date;
            int diff = (7 + (ERoute.StartDate.DayOfWeek - DayOfWeek.Monday)) % 7;
            ERoute.RealStartDate = ERoute.StartDate.AddDays(-1 * diff);
            ERoute.StartDate = ERoute.StartDate.AddHours(0 - CurrentContext.TimeZone);
            ERoute.RealStartDate = ERoute.RealStartDate.AddHours(0 - CurrentContext.TimeZone);
            if (ERoute.EndDate.HasValue)
            {
                ERoute.EndDate = ERoute.EndDate.Value.AddHours(CurrentContext.TimeZone).Date.AddDays(1).AddSeconds(-1);
                ERoute.EndDate = ERoute.EndDate.Value.AddHours(0 - CurrentContext.TimeZone);
            }
            return ERoute;
        }
    }
}
