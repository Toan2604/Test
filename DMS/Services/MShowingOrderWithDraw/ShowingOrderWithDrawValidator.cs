using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MShowingOrderWithDraw
{
    public interface IShowingOrderWithDrawValidator : IServiceScoped
    {
        Task<bool> Create(ShowingOrderWithDraw ShowingOrderWithDraw);
        Task<bool> Update(ShowingOrderWithDraw ShowingOrderWithDraw);
        Task<bool> Delete(ShowingOrderWithDraw ShowingOrderWithDraw);
        Task<bool> BulkDelete(List<ShowingOrderWithDraw> ShowingOrderWithDraws);
        Task<bool> Import(List<ShowingOrderWithDraw> ShowingOrderWithDraws);
    }

    public class ShowingOrderWithDrawValidator : IShowingOrderWithDrawValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            OrganizationNotExisted,
            OrganizationEmpty,
            StoresEmpty,
            StatusNotExisted,
            DateEmpty,
            ContentEmpty,
            QuantityEmpty,
            ShowingItemNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public ShowingOrderWithDrawValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(ShowingOrderWithDraw ShowingOrderWithDraw)
        {
            ShowingOrderWithDrawFilter ShowingOrderWithDrawFilter = new ShowingOrderWithDrawFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = ShowingOrderWithDraw.Id },
                Selects = ShowingOrderWithDrawSelect.Id
            };

            int count = await UOW.ShowingOrderWithDrawRepository.Count(ShowingOrderWithDrawFilter);
            if (count == 0)
                ShowingOrderWithDraw.AddError(nameof(ShowingOrderWithDrawValidator), nameof(ShowingOrderWithDraw.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> ValidateDate(ShowingOrderWithDraw ShowingOrderWithDraw)
        {
            if (ShowingOrderWithDraw.Date == DateTime.MinValue)
            {
                ShowingOrderWithDraw.AddError(nameof(ShowingOrderWithDrawValidator), nameof(ShowingOrderWithDraw.Date), ErrorCode.DateEmpty);
            }
            return ShowingOrderWithDraw.IsValidated;
        }

        private async Task<bool> ValidateOrganization(ShowingOrderWithDraw ShowingOrderWithDraw)
        {
            if (ShowingOrderWithDraw.OrganizationId == 0)
            {
                ShowingOrderWithDraw.AddError(nameof(ShowingOrderWithDrawValidator), nameof(ShowingOrderWithDraw.Organization), ErrorCode.OrganizationEmpty);
            }
            else
            {
                OrganizationFilter OrganizationFilter = new OrganizationFilter
                {
                    Id = new IdFilter { Equal = ShowingOrderWithDraw.OrganizationId },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                };

                var count = await UOW.OrganizationRepository.Count(OrganizationFilter);
                if (count == 0)
                    ShowingOrderWithDraw.AddError(nameof(ShowingOrderWithDrawValidator), nameof(ShowingOrderWithDraw.Organization), ErrorCode.OrganizationNotExisted);
            }

            return ShowingOrderWithDraw.IsValidated;
        }

        private async Task<bool> ValidateStores(ShowingOrderWithDraw ShowingOrderWithDraw)
        {
            if (ShowingOrderWithDraw.Stores == null || !ShowingOrderWithDraw.Stores.Any())
                ShowingOrderWithDraw.AddError(nameof(ShowingOrderWithDrawValidator), nameof(ShowingOrderWithDraw.Stores), ErrorCode.StoresEmpty);
            else
            {
                var StoreIds = ShowingOrderWithDraw.Stores.Select(x => x.Id).ToList();
                StoreFilter StoreFilter = new StoreFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Id = new IdFilter { In = StoreIds },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                    Selects = StoreSelect.Id
                };

                var StoreIdsInDB = (await UOW.StoreRepository.List(StoreFilter)).Select(x => x.Id).ToList();
                foreach (var Store in ShowingOrderWithDraw.Stores)
                {
                    if (!StoreIdsInDB.Contains(Store.Id))
                    {
                        Store.AddError(nameof(ShowingOrderWithDrawValidator), nameof(Store.Id), ErrorCode.IdNotExisted);
                    }
                }
            }
            return ShowingOrderWithDraw.IsValidated;
        }

        private async Task<bool> ValidateStatus(ShowingOrderWithDraw ShowingOrderWithDraw)
        {
            if (StatusEnum.ACTIVE.Id != ShowingOrderWithDraw.StatusId && StatusEnum.INACTIVE.Id != ShowingOrderWithDraw.StatusId)
                ShowingOrderWithDraw.AddError(nameof(ShowingOrderWithDrawValidator), nameof(ShowingOrderWithDraw.Status), ErrorCode.StatusNotExisted);
            return ShowingOrderWithDraw.IsValidated;
        }

        private async Task<bool> ValidateContent(ShowingOrderWithDraw ShowingOrderWithDraw)
        {
            if (ShowingOrderWithDraw.ShowingOrderContentWithDraws == null || ShowingOrderWithDraw.ShowingOrderContentWithDraws.Count == 0)
            {
                ShowingOrderWithDraw.AddError(nameof(ShowingOrderWithDrawValidator), nameof(ShowingOrderWithDraw.ShowingOrderContentWithDraws), ErrorCode.ContentEmpty);
            }
            else
            {
                var ShowingItemIds = ShowingOrderWithDraw.ShowingOrderContentWithDraws.Select(x => x.ShowingItemId).ToList();
                var ShowingItems = await UOW.ShowingItemRepository.List(new ShowingItemFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = ShowingItemSelect.ALL,
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                    Id = new IdFilter { In = ShowingItemIds }
                });

                foreach (var ShowingOrderWithDrawContent in ShowingOrderWithDraw.ShowingOrderContentWithDraws)
                {
                    if (ShowingOrderWithDrawContent.Quantity == 0)
                    {
                        ShowingOrderWithDrawContent.AddError(nameof(ShowingOrderWithDrawValidator), nameof(ShowingOrderWithDrawContent.Quantity), ErrorCode.QuantityEmpty);
                    }
                    else
                    {
                        ShowingItem ShowingItem = ShowingItems.Where(x => x.Id == ShowingOrderWithDrawContent.ShowingItemId).FirstOrDefault();
                        if (ShowingItem == null)
                        {
                            ShowingOrderWithDrawContent.AddError(nameof(ShowingOrderWithDrawValidator), nameof(ShowingOrderWithDrawContent.ShowingItem), ErrorCode.ShowingItemNotExisted);
                        }
                    }
                }
            }
            return ShowingOrderWithDraw.IsValidated;
        }

        public async Task<bool> Create(ShowingOrderWithDraw ShowingOrderWithDraw)
        {
            await ValidateDate(ShowingOrderWithDraw);
            await ValidateOrganization(ShowingOrderWithDraw);
            await ValidateStores(ShowingOrderWithDraw);
            await ValidateStatus(ShowingOrderWithDraw);
            await ValidateContent(ShowingOrderWithDraw);
            return ShowingOrderWithDraw.IsValidated;
        }

        public async Task<bool> Update(ShowingOrderWithDraw ShowingOrderWithDraw)
        {
            if (await ValidateId(ShowingOrderWithDraw))
            {
                await ValidateDate(ShowingOrderWithDraw);
                await ValidateOrganization(ShowingOrderWithDraw);
                await ValidateStatus(ShowingOrderWithDraw);
                await ValidateContent(ShowingOrderWithDraw);
            }
            return ShowingOrderWithDraw.IsValidated;
        }

        public async Task<bool> Delete(ShowingOrderWithDraw ShowingOrderWithDraw)
        {
            if (await ValidateId(ShowingOrderWithDraw))
            {
            }
            return ShowingOrderWithDraw.IsValidated;
        }

        public async Task<bool> BulkDelete(List<ShowingOrderWithDraw> ShowingOrderWithDraws)
        {
            foreach (ShowingOrderWithDraw ShowingOrderWithDraw in ShowingOrderWithDraws)
            {
                await Delete(ShowingOrderWithDraw);
            }
            return ShowingOrderWithDraws.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<ShowingOrderWithDraw> ShowingOrderWithDraws)
        {
            return true;
        }
    }
}
