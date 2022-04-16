using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Services.MStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MERouteChangeRequest
{
    public interface IERouteChangeRequestService : IServiceScoped
    {
        Task<int> Count(ERouteChangeRequestFilter ERouteChangeRequestFilter);
        Task<List<ERouteChangeRequest>> List(ERouteChangeRequestFilter ERouteChangeRequestFilter);
        Task<ERouteChangeRequest> Get(long Id);
        Task<ERouteChangeRequest> Create(ERouteChangeRequest ERouteChangeRequest);
        Task<ERouteChangeRequest> Update(ERouteChangeRequest ERouteChangeRequest);
        Task<ERouteChangeRequest> Delete(ERouteChangeRequest ERouteChangeRequest);
        Task<List<ERouteChangeRequest>> BulkDelete(List<ERouteChangeRequest> ERouteChangeRequests);
        Task<List<ERouteChangeRequest>> Import(List<ERouteChangeRequest> ERouteChangeRequests);
        ERouteChangeRequestFilter ToFilter(ERouteChangeRequestFilter ERouteChangeRequestFilter);
        Task<List<Store>> ListStore(StoreFilter StoreFilter);
    }

    public class ERouteChangeRequestService : BaseService, IERouteChangeRequestService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IStoreService StoreService;
        private IERouteChangeRequestValidator ERouteChangeRequestValidator;

        public ERouteChangeRequestService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IStoreService StoreService,
            IERouteChangeRequestValidator ERouteChangeRequestValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.StoreService = StoreService;
            this.ERouteChangeRequestValidator = ERouteChangeRequestValidator;
        }
        public async Task<int> Count(ERouteChangeRequestFilter ERouteChangeRequestFilter)
        {
            try
            {
                int result = await UOW.ERouteChangeRequestRepository.Count(ERouteChangeRequestFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteChangeRequestService));
            }
            return 0;
        }

        public async Task<List<ERouteChangeRequest>> List(ERouteChangeRequestFilter ERouteChangeRequestFilter)
        {
            try
            {
                List<ERouteChangeRequest> ERouteChangeRequests = await UOW.ERouteChangeRequestRepository.List(ERouteChangeRequestFilter);
                return ERouteChangeRequests;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteChangeRequestService));
            }
            return null;
        }
        public async Task<ERouteChangeRequest> Get(long Id)
        {
            ERouteChangeRequest ERouteChangeRequest = await UOW.ERouteChangeRequestRepository.Get(Id);
            if (ERouteChangeRequest == null)
                return null;
            return ERouteChangeRequest;
        }

        public async Task<ERouteChangeRequest> Create(ERouteChangeRequest ERouteChangeRequest)
        {
            if (!await ERouteChangeRequestValidator.Create(ERouteChangeRequest))
                return ERouteChangeRequest;

            try
            {
                ERouteChangeRequest.CreatorId = CurrentContext.UserId;
                ERouteChangeRequest.RequestStateId = RequestStateEnum.NEW.Id;

                await UOW.ERouteChangeRequestRepository.Create(ERouteChangeRequest);


                Logging.CreateAuditLog(ERouteChangeRequest, new { }, nameof(ERouteChangeRequestService));
                return await UOW.ERouteChangeRequestRepository.Get(ERouteChangeRequest.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteChangeRequestService));
            }
            return null;
        }

        public async Task<ERouteChangeRequest> Update(ERouteChangeRequest ERouteChangeRequest)
        {
            if (!await ERouteChangeRequestValidator.Update(ERouteChangeRequest))
                return ERouteChangeRequest;
            try
            {
                var oldData = await UOW.ERouteChangeRequestRepository.Get(ERouteChangeRequest.Id);


                await UOW.ERouteChangeRequestRepository.Update(ERouteChangeRequest);


                var newData = await UOW.ERouteChangeRequestRepository.Get(ERouteChangeRequest.Id);
                Logging.CreateAuditLog(newData, oldData, nameof(ERouteChangeRequestService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteChangeRequestService));
            }
            return null;
        }

        public async Task<ERouteChangeRequest> Delete(ERouteChangeRequest ERouteChangeRequest)
        {
            if (!await ERouteChangeRequestValidator.Delete(ERouteChangeRequest))
                return ERouteChangeRequest;

            try
            {

                await UOW.ERouteChangeRequestRepository.Delete(ERouteChangeRequest);

                Logging.CreateAuditLog(new { }, ERouteChangeRequest, nameof(ERouteChangeRequestService));
                return ERouteChangeRequest;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteChangeRequestService));
            }
            return null;
        }

        public async Task<List<ERouteChangeRequest>> BulkDelete(List<ERouteChangeRequest> ERouteChangeRequests)
        {
            if (!await ERouteChangeRequestValidator.BulkDelete(ERouteChangeRequests))
                return ERouteChangeRequests;

            try
            {

                await UOW.ERouteChangeRequestRepository.BulkDelete(ERouteChangeRequests);

                Logging.CreateAuditLog(new { }, ERouteChangeRequests, nameof(ERouteChangeRequestService));
                return ERouteChangeRequests;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteChangeRequestService));
            }
            return null;
        }

        public async Task<List<ERouteChangeRequest>> Import(List<ERouteChangeRequest> ERouteChangeRequests)
        {
            if (!await ERouteChangeRequestValidator.Import(ERouteChangeRequests))
                return ERouteChangeRequests;
            try
            {

                await UOW.ERouteChangeRequestRepository.BulkMerge(ERouteChangeRequests);


                Logging.CreateAuditLog(ERouteChangeRequests, new { }, nameof(ERouteChangeRequestService));
                return ERouteChangeRequests;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ERouteChangeRequestService));
            }
            return null;
        }

        public ERouteChangeRequestFilter ToFilter(ERouteChangeRequestFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<ERouteChangeRequestFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                ERouteChangeRequestFilter subFilter = new ERouteChangeRequestFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterPermissionDefinition.IdFilter;

                    if (FilterPermissionDefinition.Name == nameof(subFilter.SaleEmployeeId))
                        subFilter.SaleEmployeeId = FilterPermissionDefinition.IdFilter;

                    if (FilterPermissionDefinition.Name == nameof(subFilter.StoreId))
                        subFilter.StoreId = FilterPermissionDefinition.IdFilter;

                    if (FilterPermissionDefinition.Name == nameof(subFilter.ERouteTypeId))
                        subFilter.ERouteTypeId = FilterPermissionDefinition.IdFilter;

                    if (FilterPermissionDefinition.Name == nameof(subFilter.RequestStateId))
                        subFilter.RequestStateId = FilterPermissionDefinition.IdFilter;
                }
            }
            return filter;
        }


        public async Task<List<Store>> ListStore(StoreFilter StoreFilter)
        {
            List<Store> Stores = await StoreService.List(StoreFilter);
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
    }
}
