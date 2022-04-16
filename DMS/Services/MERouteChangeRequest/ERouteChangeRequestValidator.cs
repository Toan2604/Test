using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MERouteChangeRequest
{
    public interface IERouteChangeRequestValidator : IServiceScoped
    {
        Task<bool> Create(ERouteChangeRequest ERouteChangeRequest);
        Task<bool> Update(ERouteChangeRequest ERouteChangeRequest);
        Task<bool> Delete(ERouteChangeRequest ERouteChangeRequest);
        Task<bool> BulkDelete(List<ERouteChangeRequest> ERouteChangeRequests);
        Task<bool> Import(List<ERouteChangeRequest> ERouteChangeRequests);
    }

    public class ERouteChangeRequestValidator : IERouteChangeRequestValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            StoreEmpty,
            ERouteChangeRequestInUsed,
            ERouteNotExisted,
            RequestStateIsNotApproved,
            ERouteChangeRequestContentsEmpty
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public ERouteChangeRequestValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(ERouteChangeRequest ERouteChangeRequest)
        {
            ERouteChangeRequestFilter ERouteChangeRequestFilter = new ERouteChangeRequestFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = ERouteChangeRequest.Id },
                Selects = ERouteChangeRequestSelect.Id
            };

            int count = await UOW.ERouteChangeRequestRepository.Count(ERouteChangeRequestFilter);
            if (count == 0)
                ERouteChangeRequest.AddError(nameof(ERouteChangeRequestValidator), nameof(ERouteChangeRequest.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }
        private async Task<bool> ValidateERoute(ERouteChangeRequest ERouteChangeRequest)
        {
            ERoute ERoute = await UOW.ERouteRepository.Get(ERouteChangeRequest.ERouteId);
            if (ERoute == null)
                ERouteChangeRequest.AddError(nameof(ERouteChangeRequestValidator), nameof(ERouteChangeRequest.ERoute), ErrorCode.ERouteNotExisted);
            else
            {
                if (ERoute.RequestStateId != RequestStateEnum.APPROVED.Id)
                    ERouteChangeRequest.AddError(nameof(ERouteChangeRequestValidator), nameof(ERoute.RequestState), ErrorCode.RequestStateIsNotApproved);
            }
            return ERouteChangeRequest.IsValidated;
        }

        private async Task<bool> ValidateStore(ERouteChangeRequest ERouteChangeRequest)
        {
            if (ERouteChangeRequest.ERouteChangeRequestContents != null && ERouteChangeRequest.ERouteChangeRequestContents.Any())
            {
                var IdsStore = ERouteChangeRequest.ERouteChangeRequestContents.Select(x => x.StoreId).ToList();

                StoreFilter StoreFilter = new StoreFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = StoreSelect.Id,
                    Id = new IdFilter { In = IdsStore }
                };

                var IdsInDB = (await UOW.StoreRepository.List(StoreFilter)).Select(x => x.Id).ToList();
                var listIdsNotExisted = IdsStore.Except(IdsInDB);
                foreach (var ERouteChangeRequestContent in ERouteChangeRequest.ERouteChangeRequestContents)
                {
                    if (listIdsNotExisted.Contains(ERouteChangeRequestContent.StoreId))
                        ERouteChangeRequestContent.AddError(nameof(ERouteChangeRequestValidator), nameof(ERouteChangeRequestContent.Store), ErrorCode.StoreEmpty);
                }
            }
            else
            {
                ERouteChangeRequest.AddError(nameof(ERouteChangeRequestValidator), nameof(ERouteChangeRequest.ERouteChangeRequestContents), ErrorCode.ERouteChangeRequestContentsEmpty);
            }
            return ERouteChangeRequest.IsValidated;
        }

        public async Task<bool> Create(ERouteChangeRequest ERouteChangeRequest)
        {
            await ValidateERoute(ERouteChangeRequest);
            await ValidateStore(ERouteChangeRequest);
            return ERouteChangeRequest.IsValidated;
        }

        public async Task<bool> Update(ERouteChangeRequest ERouteChangeRequest)
        {
            if (await ValidateId(ERouteChangeRequest))
            {
                await ValidateStore(ERouteChangeRequest);
            }
            return ERouteChangeRequest.IsValidated;
        }

        public async Task<bool> Delete(ERouteChangeRequest ERouteChangeRequest)
        {
            if (await ValidateId(ERouteChangeRequest))
            {
                if (ERouteChangeRequest.RequestStateId != RequestStateEnum.NEW.Id)
                    ERouteChangeRequest.AddError(nameof(ERouteChangeRequestValidator), nameof(ERouteChangeRequest.RequestState), ErrorCode.ERouteChangeRequestInUsed);
            }
            return ERouteChangeRequest.IsValidated;
        }

        public async Task<bool> BulkDelete(List<ERouteChangeRequest> ERouteChangeRequests)
        {
            foreach (ERouteChangeRequest ERouteChangeRequest in ERouteChangeRequests)
            {
                await Delete(ERouteChangeRequest);
            }
            return ERouteChangeRequests.All(st => st.IsValidated);
        }

        public async Task<bool> Import(List<ERouteChangeRequest> ERouteChangeRequests)
        {
            return true;
        }
    }
}
