using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MDirectSalesOrder
{
    public interface IDirectSalesOrderValidator : IServiceScoped
    {
        Task<bool> ApplyPromotionCode(DirectSalesOrder DirectSalesOrder);
        Task<bool> Create(DirectSalesOrder DirectSalesOrder);
        Task<bool> Update(DirectSalesOrder DirectSalesOrder);
        Task<bool> Delete(DirectSalesOrder DirectSalesOrder);
        Task<bool> BulkDelete(List<DirectSalesOrder> DirectSalesOrders);
        Task<bool> Import(List<DirectSalesOrder> DirectSalesOrders);
    }

    public class DirectSalesOrderValidator : IDirectSalesOrderValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            BuyerStoreNotExisted,
            SellerStoreNotExisted,
            BuyerStoreEmpty,
            SellerStoreEmpty,
            SaleEmployeeNotExisted,
            SaleEmployeeEmpty,
            OrderDateEmpty,
            EditedPriceStatusNotExisted,
            PriceOutOfRange,
            UnitOfMeasureEmpty,
            UnitOfMeasureNotExisted,
            QuantityEmpty,
            ItemNotExisted,
            QuantityInvalid,
            SellerStoreEqualBuyerStore,
            ContentEmpty,
            DeliveryDateInvalid,
            BuyerStoreNotInERouteScope,
            SaleEmployeeNotInOrg,
            PromotionCodeNotExisted,
            PromotionCodeHasUsed,
            OrganizationInvalid,
            StoreInvalid,
            ItemInvalid,
            DraftStoreNotHavePermission
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
                    AppUserId = new IdFilter { Equal = DirectSalesOrder.SaleEmployeeId },
                    StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id },
                    Selects = StoreSelect.Id,
                    
                };

                int count = await UOW.StoreRepository.Count(StoreFilter);
                if (count == 0)
                {
                    DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.BuyerStore), ErrorCode.BuyerStoreNotInERouteScope);
                }
                else
                {
                    SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
                    if (DirectSalesOrder.BuyerStore == null) //dữ liệu ở DMS FAST ko có reference BuyerStore
                    {
                        DirectSalesOrder.BuyerStore = await UOW.StoreRepository.Get(DirectSalesOrder.BuyerStoreId);
                    }
                    if (SystemConfiguration.ALLOW_DRAFT_STORE_TO_CREATE_ORDER == false && DirectSalesOrder.BuyerStore.StoreStatusId == StoreStatusEnum.DRAFT.Id)
                        DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.BuyerStore), ErrorCode.DraftStoreNotHavePermission);
                }

            }

            return DirectSalesOrder.IsValidated;
        }

        private async Task<bool> ValidateEmployee(DirectSalesOrder DirectSalesOrder)
        {
            if (DirectSalesOrder.SaleEmployeeId == 0)
                DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.SaleEmployee), ErrorCode.SaleEmployeeEmpty);
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

                var AppUser = (await UOW.AppUserRepository.List(AppUserFilter)).FirstOrDefault();
                if (AppUser == null)
                    DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.SaleEmployee), ErrorCode.SaleEmployeeNotExisted);
                else
                {
                    var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                    if (!AppUser.Organization.Path.StartsWith(CurrentUser.Organization.Path))
                    {
                        DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.SaleEmployee), ErrorCode.SaleEmployeeNotInOrg);
                    }
                }
            }

            return DirectSalesOrder.IsValidated;
        }

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
                if (DirectSalesOrder.DeliveryDate.Value < DirectSalesOrder.OrderDate)
                {
                    DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.DeliveryDate), ErrorCode.DeliveryDateInvalid);
                }
            }
            return DirectSalesOrder.IsValidated;
        }

        private async Task<bool> ValidateContent(DirectSalesOrder DirectSalesOrder)
        {
            if (DirectSalesOrder.DirectSalesOrderContents != null && DirectSalesOrder.DirectSalesOrderContents.Any())
            {
                var ItemIds = DirectSalesOrder.DirectSalesOrderContents.Select(x => x.ItemId).ToList();
                var ProductIds = DirectSalesOrder.DirectSalesOrderContents.Select(x => x.Item.ProductId).ToList();
                List<Item> Items = await UOW.ItemRepository.List(new ItemFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Id = new IdFilter { In = ItemIds },
                    Selects = ItemSelect.Id | ItemSelect.SalePrice | ItemSelect.ProductId
                });

                var Products = await UOW.ProductRepository.List(new ProductFilter
                {
                    Id = new IdFilter { In = ProductIds },
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = ProductSelect.UnitOfMeasure | ProductSelect.UnitOfMeasureGrouping | ProductSelect.Id | ProductSelect.TaxType
                });

                var UOMGs = await UOW.UnitOfMeasureGroupingRepository.List(new UnitOfMeasureGroupingFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = UnitOfMeasureGroupingSelect.Id | UnitOfMeasureGroupingSelect.UnitOfMeasure | UnitOfMeasureGroupingSelect.UnitOfMeasureGroupingContents
                });

                foreach (var DirectSalesOrderContent in DirectSalesOrder.DirectSalesOrderContents)
                {
                    if (DirectSalesOrderContent.UnitOfMeasureId == 0)
                        DirectSalesOrderContent.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrderContent.UnitOfMeasure), ErrorCode.UnitOfMeasureEmpty);
                    else
                    {
                        var Item = Items.Where(x => x.Id == DirectSalesOrderContent.ItemId).FirstOrDefault();
                        if (Item == null)
                        {
                            DirectSalesOrderContent.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrderContent.Item), ErrorCode.ItemNotExisted);
                        }
                        else
                        {
                            var Product = Products.Where(x => Item.ProductId == x.Id).FirstOrDefault();
                            List<UnitOfMeasure> UnitOfMeasures = new List<UnitOfMeasure>();
                            if (Product.UnitOfMeasureGroupingId.HasValue)
                            {
                                var UOMG = UOMGs.Where(x => x.Id == Product.UnitOfMeasureGroupingId).FirstOrDefault();
                                UnitOfMeasures = UOMG.UnitOfMeasureGroupingContents.Select(x => new UnitOfMeasure
                                {
                                    Id = x.UnitOfMeasure.Id,
                                    Code = x.UnitOfMeasure.Code,
                                    Name = x.UnitOfMeasure.Name,
                                    Description = x.UnitOfMeasure.Description,
                                    StatusId = x.UnitOfMeasure.StatusId,
                                    Factor = x.Factor
                                }).ToList();
                            }

                            UnitOfMeasures.Add(new UnitOfMeasure
                            {
                                Id = Product.UnitOfMeasure.Id,
                                Code = Product.UnitOfMeasure.Code,
                                Name = Product.UnitOfMeasure.Name,
                                Description = Product.UnitOfMeasure.Description,
                                StatusId = Product.UnitOfMeasure.StatusId,
                                Factor = 1
                            });

                            var UOM = UnitOfMeasures.Where(x => DirectSalesOrderContent.UnitOfMeasureId == x.Id).FirstOrDefault();
                            if (UOM == null)
                                DirectSalesOrderContent.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrderContent.UnitOfMeasure), ErrorCode.UnitOfMeasureNotExisted);
                            //else
                            //{
                            //    if(DirectSalesOrder.EditedPriceStatusId == EditedPriceStatusEnum.ACTIVE.Id)
                            //    {
                            //        if(DirectSalesOrderContent.SalePrice < (Item.SalePrice * UOM.Factor.Value) * 0.9m
                            //            || DirectSalesOrderContent.SalePrice > (Item.SalePrice * UOM.Factor.Value) * 1.1m)
                            //            DirectSalesOrderContent.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrderContent.SalePrice), ErrorCode.PriceOutOfRange);
                            //    }
                            //}
                        }

                        if (DirectSalesOrderContent.Quantity <= 0)
                            DirectSalesOrderContent.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrderContent.Quantity), ErrorCode.QuantityEmpty);

                    }

                }
            }
            else
            {
                if (DirectSalesOrder.DirectSalesOrderPromotions == null || !DirectSalesOrder.DirectSalesOrderPromotions.Any())
                {
                    DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.Id), ErrorCode.ContentEmpty);
                }
            }

            return DirectSalesOrder.IsValidated;
        }

        private async Task<bool> ValidatePromotion(DirectSalesOrder DirectSalesOrder)
        {
            if (DirectSalesOrder.DirectSalesOrderPromotions != null)
            {
                await ValidateItem(DirectSalesOrder);
                //validate đơn vị tính sản phẩm khuyến mãi
                var Ids = DirectSalesOrder.DirectSalesOrderPromotions.Select(x => x.UnitOfMeasureId).ToList();
                var UnitOfMeasureFilter = new UnitOfMeasureFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Id = new IdFilter { In = Ids },
                    StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id },
                    Selects = UnitOfMeasureSelect.Id
                };

                var listIdsInDB = (await UOW.UnitOfMeasureRepository.List(UnitOfMeasureFilter)).Select(x => x.Id);
                var listIdsNotExisted = Ids.Except(listIdsInDB);

                foreach (var DirectSalesOrderPromotion in DirectSalesOrder.DirectSalesOrderPromotions)
                {
                    if (listIdsNotExisted.Contains(DirectSalesOrderPromotion.UnitOfMeasureId))
                        DirectSalesOrderPromotion.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrderPromotion.UnitOfMeasure), ErrorCode.UnitOfMeasureEmpty);
                    //validate số lượng
                    if (DirectSalesOrderPromotion.Quantity <= 0)
                        DirectSalesOrderPromotion.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrderPromotion.Quantity), ErrorCode.QuantityEmpty);
                }

            }
            return DirectSalesOrder.IsValidated;
        }


        private async Task<bool> ValidateItem(DirectSalesOrder DirectSalesOrder)
        {

            if (DirectSalesOrder.DirectSalesOrderPromotions != null)
            {
                var Ids = DirectSalesOrder.DirectSalesOrderPromotions.Select(x => x.ItemId).ToList();
                var ItemFilter = new ItemFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Id = new IdFilter { In = Ids },
                    StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id },
                    Selects = ItemSelect.Id
                };

                var listIdsInDB = (await UOW.ItemRepository.List(ItemFilter)).Select(x => x.Id);
                var listIdsNotExisted = Ids.Except(listIdsInDB);
                foreach (var DirectSalesOrderPromotion in DirectSalesOrder.DirectSalesOrderPromotions)
                {
                    if (listIdsNotExisted.Contains(DirectSalesOrderPromotion.ItemId))
                        DirectSalesOrderPromotion.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrderPromotion.Item), ErrorCode.ItemNotExisted);
                }
            }
            return DirectSalesOrder.IsValidated;
        }

        private async Task<bool> ValidateEditedPrice(DirectSalesOrder DirectSalesOrder)
        {
            if (EditedPriceStatusEnum.ACTIVE.Id != DirectSalesOrder.EditedPriceStatusId && EditedPriceStatusEnum.INACTIVE.Id != DirectSalesOrder.EditedPriceStatusId)
                DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.EditedPriceStatus), ErrorCode.EditedPriceStatusNotExisted);
            return DirectSalesOrder.IsValidated;
        }

        public async Task<bool> ApplyPromotionCode(DirectSalesOrder DirectSalesOrder)
        {
            if (!string.IsNullOrWhiteSpace(DirectSalesOrder.PromotionCode))
            {
                var appUser = await UOW.AppUserRepository.Get(DirectSalesOrder.SaleEmployeeId);
                OrganizationFilter OrganizationFilter = new OrganizationFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = OrganizationSelect.ALL,
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                };

                var Organizations = await UOW.OrganizationRepository.List(OrganizationFilter);
                var OrganizationIds = Organizations
                    .Where(x => appUser.Organization.Path.StartsWith(x.Path))
                    .Select(x => x.Id)
                    .ToList();
                PromotionCodeFilter PromotionCodeFilter = new PromotionCodeFilter()
                {
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                    StartDate = new DateFilter { LessEqual = DirectSalesOrder.OrderDate },
                    EndDate = new DateFilter { GreaterEqual = DirectSalesOrder.OrderDate },
                    OrganizationId = new IdFilter { In = OrganizationIds },
                    Code = new StringFilter { Equal = DirectSalesOrder.PromotionCode },
                    Skip = 0,
                    Take = 1,
                    Selects = PromotionCodeSelect.Id | PromotionCodeSelect.Quantity | PromotionCodeSelect.Organization
                };
                var PromotionCodes = await UOW.PromotionCodeRepository.List(PromotionCodeFilter);
                PromotionCodes = PromotionCodes.Where(x => OrganizationIds.Contains(x.OrganizationId)).ToList();
                if (PromotionCodes.Count() == 0)
                {
                    DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.PromotionCode), ErrorCode.PromotionCodeNotExisted);
                }
                else
                {
                    var PromotionCode = PromotionCodes.FirstOrDefault();
                    PromotionCodeHistoryFilter PromotionCodeHistoryFilter = new PromotionCodeHistoryFilter
                    {
                        PromotionCodeId = new IdFilter { Equal = PromotionCode.Id }
                    };

                    var countHistory = await UOW.PromotionCodeHistoryRepository.Count(PromotionCodeHistoryFilter);
                    if (PromotionCode.Quantity.HasValue && countHistory >= PromotionCode.Quantity)
                    {
                        DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.PromotionCode), ErrorCode.PromotionCodeHasUsed);
                    }
                    else
                    {
                        PromotionCode = await UOW.PromotionCodeRepository.Get(PromotionCode.Id);
                        var BuyerStore = await UOW.StoreRepository.Get(DirectSalesOrder.BuyerStoreId);
                        if (BuyerStore != null)
                        {
                            if (PromotionCode.PromotionTypeId == PromotionTypeEnum.ORGANIZATION.Id)
                            {
                                var organizationIds = PromotionCode.PromotionCodeOrganizationMappings?.Select(x => x.OrganizationId).ToList();
                                if (!organizationIds.Contains(BuyerStore.OrganizationId))
                                {
                                    DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.PromotionCode), ErrorCode.OrganizationInvalid);
                                    return DirectSalesOrder.IsValidated;
                                }
                            }
                            else if (PromotionCode.PromotionTypeId == PromotionTypeEnum.STORE.Id)
                            {
                                var StoreIds = PromotionCode.PromotionCodeStoreMappings?.Select(x => x.StoreId).ToList();
                                if (!StoreIds.Contains(BuyerStore.Id))
                                {
                                    DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.PromotionCode), ErrorCode.StoreInvalid);
                                    return DirectSalesOrder.IsValidated;
                                }
                            }

                            if (PromotionCode.PromotionProductAppliedTypeId == PromotionProductAppliedTypeEnum.PRODUCT.Id)
                            {
                                var ProductIds = PromotionCode.PromotionCodeProductMappings.Select(x => x.ProductId).ToList();
                                var ProductIdsInOrder = DirectSalesOrder.DirectSalesOrderContents?.Select(x => x.Item.ProductId).Distinct().ToList();

                                var Intersect = ProductIdsInOrder.Intersect(ProductIds).Count();
                                if (Intersect == 0)
                                {
                                    DirectSalesOrder.AddError(nameof(DirectSalesOrderValidator), nameof(DirectSalesOrder.PromotionCode), ErrorCode.ItemInvalid);
                                }
                            }
                        }
                    }
                }
            }

            return DirectSalesOrder.IsValidated;
        }

        public async Task<bool> Create(DirectSalesOrder DirectSalesOrder)
        {
            await ValidateStore(DirectSalesOrder);
            await ValidateEmployee(DirectSalesOrder);
            await ValidateOrderDate(DirectSalesOrder);
            await ValidateDeliveryDate(DirectSalesOrder);
            await ValidateContent(DirectSalesOrder);
            await ValidatePromotion(DirectSalesOrder);
            await ValidateEditedPrice(DirectSalesOrder);
            return DirectSalesOrder.IsValidated;
        }

        public async Task<bool> Update(DirectSalesOrder DirectSalesOrder)
        {
            if (await ValidateId(DirectSalesOrder))
            {
                await ValidateStore(DirectSalesOrder);
                await ValidateEmployee(DirectSalesOrder);
                await ValidateOrderDate(DirectSalesOrder);
                await ValidateDeliveryDate(DirectSalesOrder);
                await ValidateContent(DirectSalesOrder);
                await ValidatePromotion(DirectSalesOrder);
                await ValidateEditedPrice(DirectSalesOrder);
            }
            return DirectSalesOrder.IsValidated;
        }

        public async Task<bool> Delete(DirectSalesOrder DirectSalesOrder)
        {
            if (await ValidateId(DirectSalesOrder))
            {
            }
            return DirectSalesOrder.IsValidated;
        }

        public async Task<bool> BulkDelete(List<DirectSalesOrder> DirectSalesOrders)
        {
            foreach (DirectSalesOrder DirectSalesOrder in DirectSalesOrders)
            {
                await Delete(DirectSalesOrder);
            }
            return DirectSalesOrders.All(st => st.IsValidated);
        }

        public async Task<bool> Import(List<DirectSalesOrder> DirectSalesOrders)
        {
            return true;
        }
    }
}
