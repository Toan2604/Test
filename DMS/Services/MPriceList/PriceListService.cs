using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers.Configuration;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Services.MWorkflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPriceList
{
    public interface IPriceListService : IServiceScoped
    {
        Task<int> Count(PriceListFilter PriceListFilter);
        Task<List<PriceList>> List(PriceListFilter PriceListFilter);
        Task<int> CountNew(PriceListFilter PriceListFilter);
        Task<List<PriceList>> ListNew(PriceListFilter PriceListFilter);
        Task<int> CountPending(PriceListFilter PriceListFilter);
        Task<List<PriceList>> ListPending(PriceListFilter PriceListFilter);
        Task<int> CountCompleted(PriceListFilter PriceListFilter);
        Task<List<PriceList>> ListCompleted(PriceListFilter PriceListFilter);
        Task<PriceList> Get(long Id);
        Task<PriceList> GetDetail(long Id);
        Task<PriceList> Create(PriceList PriceList);
        Task<PriceList> Update(PriceList PriceList);
        Task<PriceList> Send(PriceList PriceList);
        Task<PriceList> Approve(PriceList PriceList);
        Task<PriceList> Reject(PriceList PriceList);
        Task<PriceList> Delete(PriceList PriceList);
        Task<List<PriceList>> BulkDelete(List<PriceList> PriceLists);
        Task<List<PriceList>> BulkMerge(List<PriceList> PriceLists);
        Task<List<PriceList>> Import(List<PriceList> PriceLists);
        Task<PriceListFilter> ToFilter(PriceListFilter filter);
    }

    public class PriceListService : BaseService, IPriceListService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IPriceListValidator PriceListValidator;
        private IRabbitManager RabbitManager;
        private IWorkflowService WorkflowService;

        public PriceListService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IPriceListValidator PriceListValidator,
            IRabbitManager RabbitManager,
            IWorkflowService WorkflowService
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.PriceListValidator = PriceListValidator;
            this.RabbitManager = RabbitManager;
            this.WorkflowService = WorkflowService;
        }
        public async Task<int> Count(PriceListFilter PriceListFilter)
        {
            try
            {
                int result = await UOW.PriceListRepository.Count(PriceListFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListService));
            }
            return 0;
        }

        public async Task<List<PriceList>> List(PriceListFilter PriceListFilter)
        {
            try
            {
                List<PriceList> PriceLists = await UOW.PriceListRepository.List(PriceListFilter);
                return PriceLists;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListService));
            }
            return null;
        }

        public async Task<int> CountNew(PriceListFilter PriceListFilter)
        {
            try
            {
                PriceListFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                int result = await UOW.PriceListRepository.CountNew(PriceListFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListService));
            }
            return 0;
        }
        public async Task<List<PriceList>> ListNew(PriceListFilter PriceListFilter)
        {
            try
            {
                PriceListFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                List<PriceList> PriceLists = await UOW.PriceListRepository.ListNew(PriceListFilter);
                return PriceLists;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListService));
            }
            return null;
        }

        public async Task<int> CountPending(PriceListFilter PriceListFilter)
        {
            try
            {
                PriceListFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                int result = await UOW.PriceListRepository.CountPending(PriceListFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListService));
            }
            return 0;
        }
        public async Task<List<PriceList>> ListPending(PriceListFilter PriceListFilter)
        {
            try
            {
                PriceListFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                List<PriceList> PriceLists = await UOW.PriceListRepository.ListPending(PriceListFilter);
                return PriceLists;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListService));
            }
            return null;
        }

        public async Task<int> CountCompleted(PriceListFilter PriceListFilter)
        {
            try
            {
                PriceListFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                int result = await UOW.PriceListRepository.CountCompleted(PriceListFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListService));
            }
            return 0;
        }
        public async Task<List<PriceList>> ListCompleted(PriceListFilter PriceListFilter)
        {
            try
            {
                PriceListFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                List<PriceList> PriceLists = await UOW.PriceListRepository.ListCompleted(PriceListFilter);
                return PriceLists;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListService));
            }
            return null;
        }

        public async Task<PriceList> Get(long Id)
        {
            PriceList PriceList = await UOW.PriceListRepository.Get(Id);
            if (PriceList == null)
                return null;
            PriceList.RequestState = await WorkflowService.GetRequestState(PriceList.RowId);
            if (PriceList.RequestState == null)
            {
                PriceList.RequestWorkflowStepMappings = new List<RequestWorkflowStepMapping>();
                RequestWorkflowStepMapping RequestWorkflowStepMapping = new RequestWorkflowStepMapping
                {
                    AppUserId = PriceList.CreatorId,
                    CreatedAt = PriceList.CreatedAt,
                    UpdatedAt = PriceList.UpdatedAt,
                    RequestId = PriceList.RowId,
                    AppUser = PriceList.Creator == null ? null : new AppUser
                    {
                        Id = PriceList.Creator.Id,
                        Username = PriceList.Creator.Username,
                        DisplayName = PriceList.Creator.DisplayName,
                    },
                };
                PriceList.RequestWorkflowStepMappings.Add(RequestWorkflowStepMapping);
                RequestWorkflowStepMapping.WorkflowStateId = PriceList.RequestStateId;
                PriceList.RequestState = WorkflowService.GetRequestState(PriceList.RequestStateId);
                RequestWorkflowStepMapping.WorkflowState = WorkflowService.GetWorkflowState(RequestWorkflowStepMapping.WorkflowStateId);
            }
            else
            {
                PriceList.RequestStateId = PriceList.RequestState.Id;
                PriceList.RequestWorkflowStepMappings = await WorkflowService.ListRequestWorkflowStepMapping(PriceList.RowId);
            }
            return PriceList;
        }

        public async Task<PriceList> GetDetail(long Id)
        {
            PriceList PriceList = await Get(Id);
            return PriceList;
        }

        public async Task<PriceList> Create(PriceList PriceList)
        {
            if (!await PriceListValidator.Create(PriceList))
                return PriceList;

            try
            {
                PriceList.RequestStateId = RequestStateEnum.NEW.Id;
                PriceList.CreatorId = CurrentContext.UserId;

                await UOW.PriceListRepository.Create(PriceList);
                PriceList = await UOW.PriceListRepository.Get(PriceList.Id);
                Sync(new List<PriceList> { PriceList });

                Logging.CreateAuditLog(PriceList, new { }, nameof(PriceListService));
                return await UOW.PriceListRepository.Get(PriceList.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListService));
            }
            return null;
        }

        public async Task<PriceList> Update(PriceList PriceList)
        {
            if (!await PriceListValidator.Update(PriceList))
                return PriceList;
            try
            {
                var oldData = await UOW.PriceListRepository.Get(PriceList.Id);
                var SystemConfiguration = await UOW.SystemConfigurationRepository.Get();

                if (oldData.RequestStateId == RequestStateEnum.APPROVED.Id && SystemConfiguration.ALLOW_UPDATE_APPROVED_PRICE_LIST == false)
                {
                    oldData.StatusId = PriceList.StatusId;
                    await UOW.PriceListRepository.Update(oldData);
                }
                else
                {
                    PriceList.RequestStateId = oldData.RequestStateId;
                    await BuildData(PriceList);
                    await UOW.PriceListRepository.Update(PriceList);
                }

                var newData = await UOW.PriceListRepository.Get(PriceList.Id);
                Sync(new List<PriceList> { newData });
                Logging.CreateAuditLog(newData, oldData, nameof(PriceListService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListService));
            }
            return null;
        }

        public async Task<PriceList> Delete(PriceList PriceList)
        {
            if (!await PriceListValidator.Delete(PriceList))
                return PriceList;

            try
            {

                await UOW.PriceListRepository.Delete(PriceList);
                var PriceLists = await UOW.PriceListRepository.List(new List<long> { PriceList.Id });
                Sync(PriceLists);
                Logging.CreateAuditLog(new { }, PriceList, nameof(PriceListService));
                return PriceList;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListService));
            }
            return null;
        }

        public async Task<List<PriceList>> BulkDelete(List<PriceList> PriceLists)
        {
            if (!await PriceListValidator.BulkDelete(PriceLists))
                return PriceLists;

            try
            {
                await UOW.PriceListRepository.BulkDelete(PriceLists);
                var Ids = PriceLists.Select(x => x.Id).ToList();
                PriceLists = await UOW.PriceListRepository.List(Ids);
                Sync(PriceLists);
                Logging.CreateAuditLog(new { }, PriceLists, nameof(PriceListService));
                return PriceLists;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListService));
            }
            return null;
        }

        public async Task<List<PriceList>> BulkMerge(List<PriceList> PriceLists)
        {
            if (!await PriceListValidator.Import(PriceLists))
                return PriceLists;

            try
            {

                await UOW.PriceListRepository.BulkMerge(PriceLists);
                var Ids = PriceLists.Select(x => x.Id).ToList();
                PriceLists = await UOW.PriceListRepository.List(Ids);
                Sync(PriceLists);
                Logging.CreateAuditLog(new { }, PriceLists, nameof(PriceListService));
                return PriceLists;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListService));
            }
            return null;
        }

        public async Task<List<PriceList>> Import(List<PriceList> PriceLists)
        {
            if (!await PriceListValidator.Import(PriceLists))
                return PriceLists;
            try
            {

                await UOW.PriceListRepository.BulkMerge(PriceLists);


                Logging.CreateAuditLog(PriceLists, new { }, nameof(PriceListService));
                return PriceLists;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListService));
            }
            return null;
        }

        public async Task<PriceListFilter> ToFilter(PriceListFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<PriceListFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                PriceListFilter subFilter = new PriceListFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.Code))
                        subFilter.Code = FilterBuilder.Merge(subFilter.Code, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Name))
                        subFilter.Name = FilterBuilder.Merge(subFilter.Name, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StartDate))
                        subFilter.StartDate = FilterBuilder.Merge(subFilter.StartDate, FilterPermissionDefinition.DateFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.EndDate))
                        subFilter.EndDate = FilterBuilder.Merge(subFilter.EndDate, FilterPermissionDefinition.DateFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StatusId))
                        subFilter.StatusId = FilterBuilder.Merge(subFilter.StatusId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.OrganizationId))
                        subFilter.OrganizationId = FilterBuilder.Merge(subFilter.OrganizationId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.PriceListTypeId))
                        subFilter.PriceListTypeId = FilterBuilder.Merge(subFilter.PriceListTypeId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.SalesOrderTypeId))
                        subFilter.SalesOrderTypeId = FilterBuilder.Merge(subFilter.SalesOrderTypeId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(CurrentContext.UserId) && FilterPermissionDefinition.IdFilter != null)
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

        private void Sync(List<PriceList> PriceLists)
        {
            RabbitManager.PublishList(PriceLists, RoutingKeyEnum.PriceListSync.Code);
        }

        public async Task<PriceList> Send(PriceList PriceList)
        {
            if (PriceList.Id == 0)
                PriceList = await Create(PriceList);
            else
                PriceList = await Update(PriceList);
            if (PriceList.IsValidated == false)
                return PriceList;
            PriceList = await UOW.PriceListRepository.Get(PriceList.Id);
            Dictionary<string, string> Parameters = await MapParameters(PriceList);
            GenericEnum RequestState = await WorkflowService.Send(PriceList.RowId, WorkflowTypeEnum.PRICE_LIST.Id, PriceList.OrganizationId, Parameters);
            PriceList.RequestStateId = RequestState.Id;
            await UOW.PriceListRepository.UpdateState(PriceList);
            return await Get(PriceList.Id);
        }

        public async Task<PriceList> Approve(PriceList PriceList)
        {
            PriceList = await Update(PriceList);
            if (PriceList.IsValidated == false)
                return PriceList;
            PriceList = await UOW.PriceListRepository.Get(PriceList.Id);
            Dictionary<string, string> Parameters = await MapParameters(PriceList);
            await WorkflowService.Approve(PriceList.RowId, WorkflowTypeEnum.PRICE_LIST.Id, Parameters);
            RequestState RequestState = await WorkflowService.GetRequestState(PriceList.RowId);
            PriceList.RequestStateId = RequestState.Id;
            await UOW.PriceListRepository.UpdateState(PriceList);
            return await Get(PriceList.Id);
        }

        public async Task<PriceList> Reject(PriceList PriceList)
        {
            PriceList = await UOW.PriceListRepository.Get(PriceList.Id);
            Dictionary<string, string> Parameters = await MapParameters(PriceList);
            GenericEnum Action = await WorkflowService.Reject(PriceList.RowId, WorkflowTypeEnum.PRICE_LIST.Id, Parameters);
            RequestState RequestState = await WorkflowService.GetRequestState(PriceList.RowId);
            PriceList.RequestStateId = RequestState.Id;
            await UOW.PriceListRepository.UpdateState(PriceList);
            return await Get(PriceList.Id);
        }

        private async Task<Dictionary<string, string>> MapParameters(PriceList PriceList)
        {
            var AppUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            Parameters.Add(nameof(PriceList.Id), PriceList.Id.ToString());
            Parameters.Add(nameof(PriceList.Code), PriceList.Code);
            Parameters.Add(nameof(PriceList.CreatorId), PriceList.CreatorId.ToString());
            Parameters.Add(nameof(PriceList.SalesOrderTypeId), PriceList.SalesOrderTypeId.ToString());
            Parameters.Add(nameof(PriceList.OrganizationId), PriceList.OrganizationId.ToString());
            Parameters.Add(nameof(AppUser.DisplayName), AppUser.DisplayName);
            Parameters.Add(nameof(PriceList.RequestStateId), PriceList.RequestStateId.ToString());

            RequestWorkflowDefinitionMapping RequestWorkflowDefinitionMapping = await UOW.RequestWorkflowDefinitionMappingRepository.Get(PriceList.RowId);
            if (RequestWorkflowDefinitionMapping == null)
                Parameters.Add(nameof(RequestState), RequestStateEnum.NEW.Id.ToString());
            else
                Parameters.Add(nameof(RequestState), RequestWorkflowDefinitionMapping.RequestStateId.ToString());
            Parameters.Add("Username", CurrentContext.UserName);
            return Parameters;
        }

        private async Task<PriceList> BuildData(PriceList PriceList)
        {
            PriceList oldData = await UOW.PriceListRepository.Get(PriceList.Id);
            if (oldData != null)
            {
                foreach (PriceListItemMapping PriceListItemMapping in PriceList.PriceListItemMappings)
                {
                    if (PriceListItemMapping.PriceListItemHistories == null)
                        PriceListItemMapping.PriceListItemHistories = new List<PriceListItemHistory>();
                    PriceListItemMapping PriceListItemMappingInDB = oldData.PriceListItemMappings.Where(i => i.ItemId == PriceListItemMapping.ItemId).FirstOrDefault();
                    if (PriceListItemMappingInDB != null)
                    {
                        if (PriceListItemMapping.Price != PriceListItemMappingInDB.Price)
                        {
                            PriceListItemHistory PriceListItemHistory = new PriceListItemHistory();
                            PriceListItemHistory.ItemId = PriceListItemMapping.ItemId;
                            PriceListItemHistory.PriceListId = PriceList.Id;
                            PriceListItemHistory.OldPrice = PriceListItemMappingInDB.Price;
                            PriceListItemHistory.NewPrice = PriceListItemMapping.Price;
                            PriceListItemHistory.ModifierId = CurrentContext.UserId;
                            PriceListItemMapping.PriceListItemHistories.Add(PriceListItemHistory);
                        }
                    }
                }
            }
            return PriceList;
        }
    }
}
