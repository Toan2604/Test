using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MShowingOrder
{
    public interface IShowingOrderValidator : IServiceScoped
    {
        Task<bool> Create(ShowingOrder ShowingOrder);
        Task<bool> Update(ShowingOrder ShowingOrder);
        Task<bool> Delete(ShowingOrder ShowingOrder);
        Task<bool> BulkDelete(List<ShowingOrder> ShowingOrders);
        Task<bool> Import(List<ShowingOrder> ShowingOrders);
    }

    public class ShowingOrderValidator : IShowingOrderValidator
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
            SaleStockEmpty,
            ShowingWarehouseEmpty,
            ShowingWarehouseIdNotExisted
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public ShowingOrderValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(ShowingOrder ShowingOrder)
        {
            ShowingOrderFilter ShowingOrderFilter = new ShowingOrderFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = ShowingOrder.Id },
                Selects = ShowingOrderSelect.Id
            };

            int count = await UOW.ShowingOrderRepository.Count(ShowingOrderFilter);
            if (count == 0)
                ShowingOrder.AddError(nameof(ShowingOrderValidator), nameof(ShowingOrder.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> ValidateDate(ShowingOrder ShowingOrder)
        {
            if (ShowingOrder.Date == DateTime.MinValue)
            {
                ShowingOrder.AddError(nameof(ShowingOrderValidator), nameof(ShowingOrder.Date), ErrorCode.DateEmpty);
            }
            return ShowingOrder.IsValidated;
        }

        private async Task<bool> ValidateOrganization(ShowingOrder ShowingOrder)
        {
            if (ShowingOrder.OrganizationId == 0)
            {
                ShowingOrder.AddError(nameof(ShowingOrderValidator), nameof(ShowingOrder.Organization), ErrorCode.OrganizationEmpty);
            }
            else
            {
                OrganizationFilter OrganizationFilter = new OrganizationFilter
                {
                    Id = new IdFilter { Equal = ShowingOrder.OrganizationId },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                };

                var count = await UOW.OrganizationRepository.Count(OrganizationFilter);
                if (count == 0)
                    ShowingOrder.AddError(nameof(ShowingOrderValidator), nameof(ShowingOrder.Organization), ErrorCode.OrganizationNotExisted);
            }

            return ShowingOrder.IsValidated;
        }

        private async Task<bool> ValidateStores(ShowingOrder ShowingOrder)
        {
            if (ShowingOrder.Stores == null || !ShowingOrder.Stores.Any())
                ShowingOrder.AddError(nameof(ShowingOrderValidator), nameof(ShowingOrder.Stores), ErrorCode.StoresEmpty);
            else
            {
                var StoreIds = ShowingOrder.Stores.Select(x => x.Id).ToList();
                StoreFilter StoreFilter = new StoreFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Id = new IdFilter { In = StoreIds },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                    Selects = StoreSelect.Id
                };

                var StoreIdsInDB = (await UOW.StoreRepository.List(StoreFilter)).Select(x => x.Id).ToList();
                foreach (var Store in ShowingOrder.Stores)
                {
                    if (!StoreIdsInDB.Contains(Store.Id))
                    {
                        Store.AddError(nameof(ShowingOrderValidator), nameof(Store.Id), ErrorCode.IdNotExisted);
                    }
                }
            }
            return ShowingOrder.IsValidated;
        }

        private async Task<bool> ValidateStatus(ShowingOrder ShowingOrder)
        {
            if (StatusEnum.ACTIVE.Id != ShowingOrder.StatusId && StatusEnum.INACTIVE.Id != ShowingOrder.StatusId)
                ShowingOrder.AddError(nameof(ShowingOrderValidator), nameof(ShowingOrder.Status), ErrorCode.StatusNotExisted);
            return ShowingOrder.IsValidated;
        }

        private async Task<bool> ValidateContent(ShowingOrder ShowingOrder)
        {
            if (ShowingOrder.ShowingOrderContents == null || ShowingOrder.ShowingOrderContents.Count == 0)
            {
                ShowingOrder.AddError(nameof(ShowingOrderValidator), nameof(ShowingOrder.ShowingOrderContents), ErrorCode.ContentEmpty);
            }
            else
            {
                var ShowingItemIds = ShowingOrder.ShowingOrderContents.Select(x => x.ShowingItemId).ToList();
                var ShowingItems = await UOW.ShowingItemRepository.List(new ShowingItemFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = ShowingItemSelect.ALL,
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                    Id = new IdFilter { In = ShowingItemIds }
                });

                foreach (var ShowingOrderContent in ShowingOrder.ShowingOrderContents)
                {
                    if (ShowingOrderContent.Quantity == 0)
                    {
                        ShowingOrderContent.AddError(nameof(ShowingOrderValidator), nameof(ShowingOrderContent.Quantity), ErrorCode.QuantityEmpty);
                    }
                    else
                    {
                        ShowingItem ShowingItem = ShowingItems.Where(x => x.Id == ShowingOrderContent.ShowingItemId).FirstOrDefault();
                        if (ShowingItem == null)
                        {
                            ShowingOrderContent.AddError(nameof(ShowingOrderValidator), nameof(ShowingOrderContent.ShowingItem), ErrorCode.ShowingItemNotExisted);
                        }
                    }
                }
            }
            return ShowingOrder.IsValidated;
        }

        public async Task<bool> Create(ShowingOrder ShowingOrder)
        {
            await ValidateDate(ShowingOrder);
            await ValidateOrganization(ShowingOrder);
            await ValidateStores(ShowingOrder);
            await ValidateStatus(ShowingOrder);
            await ValidateContent(ShowingOrder);
            return ShowingOrder.IsValidated;
        }

        public async Task<bool> Update(ShowingOrder ShowingOrder)
        {
            if (await ValidateId(ShowingOrder))
            {
                await ValidateDate(ShowingOrder);
                await ValidateOrganization(ShowingOrder);
                await ValidateStatus(ShowingOrder);
                await ValidateContent(ShowingOrder);
            }
            return ShowingOrder.IsValidated;
        }

        public async Task<bool> Delete(ShowingOrder ShowingOrder)
        {
            if (await ValidateId(ShowingOrder))
            {
            }
            return ShowingOrder.IsValidated;
        }

        public async Task<bool> BulkDelete(List<ShowingOrder> ShowingOrders)
        {
            foreach (ShowingOrder ShowingOrder in ShowingOrders)
            {
                await Delete(ShowingOrder);
            }
            return ShowingOrders.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<ShowingOrder> ShowingOrders)
        {
            return true;
        }
    }
}
