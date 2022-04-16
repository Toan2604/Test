using DMS.ABE.Common;
using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.ABE.Helpers;

namespace DMS.ABE.Services.MDirectSalesOrder
{
    public interface IDirectSalesOrderValidator : IServiceScoped
    {
        Task<bool> Create(DirectSalesOrder DirectSalesOrder);
        Task<bool> Update(DirectSalesOrder DirectSalesOrder);
        Task<bool> Delete(DirectSalesOrder DirectSalesOrder);
        Task<bool> Approve(DirectSalesOrder DirectSalesOrder);
        Task<bool> Reject(DirectSalesOrder DirectSalesOrder);
    }

    public class DirectSalesOrderValidator : IDirectSalesOrderValidator
    {
        public enum ErrorCode
        {
            BuyerStoreEmpty,
            BuyerStoreNotExisted,
            BuyerStoreNotInERouteScope,
            CreatorNotExisted,
            ContentEmpty,
            DeliveryAddressRequired,
            DeliveryDateInvalid,
            DraftStateRequired,
            EditedPriceStatusNotExisted,
            GeneralApprovalStateInvalid,
            ItemNotExisted,
            IdNotExisted,
            ItemInvalid,
            OrderDateEmpty,
            OrganizationInvalid,
            PriceOutOfRange,
            PromotionCodeNotExisted,
            PromotionCodeHasUsed,
            QuantityEmpty,
            QuantityInvalid,
            RequestStateInvalid,
            SaleEmployeeEmpty,
            SaleEmployeeNotExisted,
            SaleEmployeeNotInOrg,
            SellerStoreNotExisted,
            SellerStoreEmpty,
            SellerStoreEqualBuyerStore,
            StoreUserCreatorEmpty,
            StoreInvalid,
            StoreApprovalStateInvalid,
            UnitOfMeasureEmpty,
            UnitOfMeasureNotExisted,
            TotalInvalid
        }
        private IUOW UOW;
        private ICurrentContext CurrentContext;
        public DirectSalesOrderValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }
        public async Task<bool> ValidateId(DirectSalesOrder DirectSalesOrder)
        {
            DirectSalesOrderFilter DirectSalesOrderFilter = new DirectSalesOrderFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = DirectSalesOrder.Id },
                Selects = DirectSalesOrderSelect.Id
            };

            int count = await UOW.DirectSalesOrderRepository.Count(DirectSalesOrderFilter);
            if (count == 0)
                DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }
        private async Task<bool> ValidateStore(DirectSalesOrder DirectSalesOrder)
        {
            if (DirectSalesOrder.BuyerStoreId == 0)
                DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.BuyerStore), ErrorCode.BuyerStoreEmpty);
            else
            {
                StoreFilter StoreFilter = new StoreFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { In = new List<long> { DirectSalesOrder.BuyerStoreId } },
                    StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id },
                    Selects = StoreSelect.Id
                };

                int count = await UOW.StoreRepository.Count(StoreFilter);
                if (count == 0)
                    DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.BuyerStore), ErrorCode.BuyerStoreNotExisted);
            }

            return DirectSalesOrder.IsValidated;
        } // validate cua hang mua co ton tai khong
        private async Task<bool> ValidateEmployee(DirectSalesOrder DirectSalesOrder)
        {
            if (DirectSalesOrder.SaleEmployeeId == 0)
            {
                DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.SaleEmployee), ErrorCode.SaleEmployeeEmpty);
            }
            else
            {
                AppUserFilter AppUserFilter = new AppUserFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = DirectSalesOrder.SaleEmployeeId },
                    StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id },
                    Selects = AppUserSelect.Id | AppUserSelect.Organization
                };

                var count = await UOW.AppUserRepository.Count(AppUserFilter);
                if (count == 0)
                    DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.SaleEmployee), ErrorCode.SaleEmployeeNotExisted);
            }

            return DirectSalesOrder.IsValidated;
        } // validate nhân viên phụ trách
        private async Task<bool> ValidateOrderDate(DirectSalesOrder DirectSalesOrder)
        {
            if (DirectSalesOrder.OrderDate == default(DateTime))
                DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.OrderDate), ErrorCode.OrderDateEmpty);
            return DirectSalesOrder.IsValidated;
        }
        private async Task<bool> ValidateDeliveryDate(DirectSalesOrder DirectSalesOrder)
        {
            if (DirectSalesOrder.DeliveryDate.HasValue)
            {
                if (DirectSalesOrder.DeliveryDate.Value.Date < StaticParams.DateTimeNow.Date)
                {
                    DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.DeliveryDate), ErrorCode.DeliveryDateInvalid);
                }
            }
            return DirectSalesOrder.IsValidated;
        }
        private async Task<bool> ValidateDeliveryAddress(DirectSalesOrder DirectSalesOrder)
        {
            if (string.IsNullOrWhiteSpace(DirectSalesOrder.DeliveryAddress))
            {
                DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.DeliveryAddress), ErrorCode.DeliveryAddressRequired);
            }
            return DirectSalesOrder.IsValidated;
        }
        private async Task<bool> ValidateEditedPrice(DirectSalesOrder DirectSalesOrder)
        {
            if (EditedPriceStatusEnum.ACTIVE.Id != DirectSalesOrder.EditedPriceStatusId && EditedPriceStatusEnum.INACTIVE.Id != DirectSalesOrder.EditedPriceStatusId)
                DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.EditedPriceStatus), ErrorCode.EditedPriceStatusNotExisted);
            return DirectSalesOrder.IsValidated;
        }
        private async Task<bool> ValidateContent(DirectSalesOrder DirectSalesOrder)
        {
            if (DirectSalesOrder.DirectSalesOrderContents == null || DirectSalesOrder.DirectSalesOrderContents.Count == 0)
            {
                DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.Id), ErrorCode.ContentEmpty);
                return DirectSalesOrder.IsValidated;
            }

            var ItemIds = DirectSalesOrder.DirectSalesOrderContents.Select(x => x.ItemId).ToList();

            List<Item> Items = await UOW.ItemRepository.List(new ItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Id = new IdFilter { In = ItemIds },
                Selects = ItemSelect.Id | ItemSelect.SalePrice | ItemSelect.ProductId
            });

            var ProductIds = Items.Select(x => x.ProductId).ToList();

            List<Product> Products = await UOW.ProductRepository.List(new ProductFilter
            {
                Id = new IdFilter { In = ProductIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProductSelect.UnitOfMeasure | ProductSelect.UnitOfMeasureGrouping | ProductSelect.Id
            });

            var UOMGs = await UOW.UnitOfMeasureGroupingRepository.List(new UnitOfMeasureGroupingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = UnitOfMeasureGroupingSelect.Id | UnitOfMeasureGroupingSelect.UnitOfMeasure | UnitOfMeasureGroupingSelect.UnitOfMeasureGroupingContents
            });

            foreach (var DirectSalesOrderContent in DirectSalesOrder.DirectSalesOrderContents)
            {
                var Item = Items.Where(x => x.Id == DirectSalesOrderContent.ItemId).FirstOrDefault();
                if (Item == null)
                {
                    DirectSalesOrderContent.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrderContent.Item), ErrorCode.ItemNotExisted);
                    continue;
                } // validate Item

                var Product = Products.Where(x => Item.ProductId == x.Id).FirstOrDefault();

                UnitOfMeasure UOM = new UnitOfMeasure
                {
                    Id = Product.UnitOfMeasure.Id,
                    Code = Product.UnitOfMeasure.Code,
                    Name = Product.UnitOfMeasure.Name,
                    Description = Product.UnitOfMeasure.Description,
                    StatusId = Product.UnitOfMeasure.StatusId,
                    Factor = 1
                };

                if (UOM.Id == 0)
                    DirectSalesOrderContent.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrderContent.UnitOfMeasure), ErrorCode.UnitOfMeasureNotExisted);

                if (DirectSalesOrderContent.Quantity <= 0)
                    DirectSalesOrderContent.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrderContent.Quantity), ErrorCode.QuantityEmpty);
            }

            return DirectSalesOrder.IsValidated;
        }
        private async Task<bool> ValidateRequestState(DirectSalesOrder DirectSalesOrder)
        {
            if (DirectSalesOrder.RequestStateId != RequestStateEnum.APPROVED.Id && DirectSalesOrder.RequestStateId != RequestStateEnum.NEW.Id)
            {
                DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.RequestStateId), ErrorCode.RequestStateInvalid);
            }
            return DirectSalesOrder.IsValidated;
        }
        private async Task<bool> ValidateGeneralApprovalState(DirectSalesOrder DirectSalesOrder)
        {
            SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
            bool AppUserStartWFCondition = SystemConfiguration.START_WORKFLOW_BY_USER_TYPE == 1 && (
                DirectSalesOrder.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_DRAFT.Id ||
                DirectSalesOrder.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_REJECTED.Id ||
                DirectSalesOrder.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_PENDING.Id);
            bool StoreStartWFCondition = SystemConfiguration.START_WORKFLOW_BY_USER_TYPE == 2 &&
                (DirectSalesOrder.GeneralApprovalStateId != GeneralApprovalStateEnum.NEW.Id ||
                DirectSalesOrder.GeneralApprovalStateId != GeneralApprovalStateEnum.APPROVED.Id);

            if (!AppUserStartWFCondition && !StoreStartWFCondition)
            {
                DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.GeneralApprovalState), ErrorCode.GeneralApprovalStateInvalid);
            }
            return DirectSalesOrder.IsValidated;
        }
        private async Task<bool> ValidateDraftState(DirectSalesOrder DirectSalesOrder)
        {
            if (DirectSalesOrder.GeneralApprovalStateId != GeneralApprovalStateEnum.STORE_DRAFT.Id)
            {
                DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.GeneralApprovalState), ErrorCode.DraftStateRequired);
            }
            return DirectSalesOrder.IsValidated;
        }
        public async Task<bool> ValidateTotal(DirectSalesOrder DirectSalesOrder)
        {
            if (DirectSalesOrder.Total <= 0)
                DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.Total), ErrorCode.TotalInvalid);
            return DirectSalesOrder.IsValidated;
        }
        public async Task<bool> Create(DirectSalesOrder DirectSalesOrder)
        {
            await ValidateStore(DirectSalesOrder);
            await ValidateEmployee(DirectSalesOrder);
            await ValidateDeliveryDate(DirectSalesOrder);
            await ValidateDeliveryAddress(DirectSalesOrder);
            await ValidateContent(DirectSalesOrder);
            await ValidateEditedPrice(DirectSalesOrder);
            await ValidateTotal(DirectSalesOrder);
            return DirectSalesOrder.IsValidated;
        }
        public async Task<bool> Update(DirectSalesOrder DirectSalesOrder)
        {
            if (await ValidateId(DirectSalesOrder))
            {
                await ValidateGeneralApprovalState(DirectSalesOrder);
                await ValidateStore(DirectSalesOrder);
                await ValidateEmployee(DirectSalesOrder);
                await ValidateOrderDate(DirectSalesOrder);
                await ValidateDeliveryDate(DirectSalesOrder);
                await ValidateTotal(DirectSalesOrder);
                await ValidateContent(DirectSalesOrder);
                await ValidateEditedPrice(DirectSalesOrder);
            }
            return DirectSalesOrder.IsValidated;
        }
        public async Task<bool> Approve(DirectSalesOrder DirectSalesOrder)
        {
            if (await ValidateId(DirectSalesOrder))
            {
            }
            return DirectSalesOrder.IsValidated;
        }
        public async Task<bool> Reject(DirectSalesOrder DirectSalesOrder)
        {
            if (await ValidateId(DirectSalesOrder))
            {
            }
            return DirectSalesOrder.IsValidated;
        }
        public async Task<bool> Delete(DirectSalesOrder DirectSalesOrder)
        {
            if (await ValidateId(DirectSalesOrder))
            {
                await ValidateDraftState(DirectSalesOrder);
            }
            return DirectSalesOrder.IsValidated;
        }
    }
}
