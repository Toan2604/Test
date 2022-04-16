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

namespace DMS.Services.MPromotion
{
    public interface IPromotionValidator : IServiceScoped
    {
        Task<bool> Create(Promotion Promotion);
        Task<bool> Update(Promotion Promotion);
        Task<bool> UpdateDirectSalesOrder(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<bool> UpdateProduct(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<bool> UpdateProductGrouping(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<bool> UpdateProductType(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<bool> UpdateCombo(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<bool> UpdateSamePrice(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<bool> Delete(Promotion Promotion);
        Task<bool> BulkDelete(List<Promotion> Promotions);
        Task<bool> Import(List<Promotion> Promotions);
    }

    public class PromotionValidator : IPromotionValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            CodeEmpty,
            CodeHasSpecialCharacter,
            CodeExisted,
            NameEmpty,
            NameOverLength,
            OrganizationEmpty,
            OrganizationNotExisted,
            PromotionTypeEmpty,
            PromotionTypeNotExisted,
            StatusNotExisted,
            StartDateEmpty,
            EndDateWrong,
            StoreEmpty,
            StoreNotExisted,
            StoreGroupingEmpty,
            StoreGroupingNotExisted,
            StoreTypeEmpty,
            StoreTypeNotExisted
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PromotionValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Promotion Promotion)
        {
            PromotionFilter PromotionFilter = new PromotionFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Promotion.Id },
                Selects = PromotionSelect.Id
            };

            int count = await UOW.PromotionRepository.Count(PromotionFilter);
            if (count == 0)
                Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateCode(Promotion Promotion)
        {
            if (string.IsNullOrWhiteSpace(Promotion.Code))
            {
                Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.Code), ErrorCode.CodeEmpty);
            }
            else
            {
                var Code = Promotion.Code;
                if (Promotion.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(Promotion.Code))
                {
                    Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.Code), ErrorCode.CodeHasSpecialCharacter);
                }
                else
                {
                    PromotionFilter PromotionFilter = new PromotionFilter
                    {
                        Skip = 0,
                        Take = 10,
                        Id = new IdFilter { NotEqual = Promotion.Id },
                        Code = new StringFilter { Equal = Promotion.Code },
                        Selects = PromotionSelect.Code
                    };

                    int count = await UOW.PromotionRepository.Count(PromotionFilter);
                    if (count != 0)
                        Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.Code), ErrorCode.CodeExisted);
                }
            }
            return Promotion.IsValidated;
        }

        private async Task<bool> ValidateName(Promotion Promotion)
        {
            if (string.IsNullOrWhiteSpace(Promotion.Name))
            {
                Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.Name), ErrorCode.NameEmpty);
            }
            else if (Promotion.Name.Length > 255)
            {
                Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.Name), ErrorCode.NameOverLength);
            }
            return Promotion.IsValidated;
        }

        private async Task<bool> ValidateOrganization(Promotion Promotion)
        {
            if (Promotion.OrganizationId == 0)
            {
                Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.Organization), ErrorCode.OrganizationEmpty);
            }
            else
            {
                OrganizationFilter OrganizationFilter = new OrganizationFilter
                {
                    Id = new IdFilter { Equal = Promotion.OrganizationId }
                };

                var count = await UOW.OrganizationRepository.Count(OrganizationFilter);
                if (count == 0)
                    Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.Organization), ErrorCode.OrganizationNotExisted);
            }
            return Promotion.IsValidated;
        }

        private async Task<bool> ValidatePromotionType(Promotion Promotion)
        {
            if (Promotion.PromotionTypeId == 0)
            {
                Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.PromotionType), ErrorCode.PromotionTypeEmpty);
            }
            else
            {
                var PromotionTypeIds = PromotionTypeEnum.PromotionTypeEnumList.Select(x => x.Id).ToList();
                if (!PromotionTypeIds.Contains(Promotion.PromotionTypeId))
                {
                    Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.PromotionType), ErrorCode.PromotionTypeNotExisted);
                }
                else
                {
                    if (Promotion.PromotionTypeId == PromotionTypeEnum.STORE.Id)
                    {
                        if (Promotion.PromotionStoreMappings == null || Promotion.PromotionStoreMappings.Any() == false)
                        {
                            Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.PromotionType), ErrorCode.StoreEmpty);
                        }
                        else
                        {
                            var StoreIds = Promotion.PromotionStoreMappings.Select(x => x.StoreId).ToList();
                            var ListStoreInDB = await UOW.StoreRepository.List(new StoreFilter
                            {
                                Skip = 0,
                                Take = int.MaxValue,
                                Selects = StoreSelect.Id,
                                Id = new IdFilter { In = StoreIds },
                                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                            });
                            var Ids = ListStoreInDB.Select(x => x.Id).ToList();
                            var ExceptIds = StoreIds.Except(Ids).ToList();
                            foreach (var PromotionStoreMapping in Promotion.PromotionStoreMappings)
                            {
                                if (ExceptIds.Contains(PromotionStoreMapping.StoreId))
                                    PromotionStoreMapping.AddError(nameof(PromotionValidator), nameof(PromotionStoreMapping.Store), ErrorCode.StoreNotExisted);
                            }
                        }
                    }

                    //if (Promotion.PromotionTypeId == PromotionTypeEnum.STORE_GROUPING.Id)
                    //{
                    //    if (Promotion.PromotionStoreGroupingMappings == null || Promotion.PromotionStoreGroupingMappings.Any() == false)
                    //    {
                    //        Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.PromotionType), ErrorCode.StoreGroupingEmpty);
                    //    }
                    //    else
                    //    {
                    //        var StoreGroupingIds = Promotion.PromotionStoreGroupingMappings.Select(x => x.StoreGroupingId).ToList();
                    //        var ListStoreGroupingInDB = await UOW.StoreGroupingRepository.List(new StoreGroupingFilter
                    //        {
                    //            Skip = 0,
                    //            Take = int.MaxValue,
                    //            Selects = StoreGroupingSelect.Id,
                    //            Id = new IdFilter { In = StoreGroupingIds },
                    //            StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                    //        });
                    //        var Ids = ListStoreGroupingInDB.Select(x => x.Id).ToList();
                    //        var ExceptIds = StoreGroupingIds.Except(Ids).ToList();
                    //        foreach (var PromotionStoreGroupingMapping in Promotion.PromotionStoreGroupingMappings)
                    //        {
                    //            if (ExceptIds.Contains(PromotionStoreGroupingMapping.StoreGroupingId))
                    //                PromotionStoreGroupingMapping.AddError(nameof(PromotionValidator), nameof(PromotionStoreGroupingMapping.StoreGrouping), ErrorCode.StoreGroupingNotExisted);
                    //        }
                    //    }
                    //}

                    //if (Promotion.PromotionTypeId == PromotionTypeEnum.STORE_TYPE.Id)
                    //{
                    //    if (Promotion.PromotionStoreTypeMappings == null || Promotion.PromotionStoreTypeMappings.Any() == false)
                    //    {
                    //        Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.PromotionType), ErrorCode.StoreTypeEmpty);
                    //    }
                    //    else
                    //    {
                    //        var StoreTypeIds = Promotion.PromotionStoreTypeMappings.Select(x => x.StoreTypeId).ToList();
                    //        var ListStoreTypeInDB = await UOW.StoreTypeRepository.List(new StoreTypeFilter
                    //        {
                    //            Skip = 0,
                    //            Take = int.MaxValue,
                    //            Selects = StoreTypeSelect.Id,
                    //            Id = new IdFilter { In = StoreTypeIds },
                    //            StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                    //        });
                    //        var Ids = ListStoreTypeInDB.Select(x => x.Id).ToList();
                    //        var ExceptIds = StoreTypeIds.Except(Ids).ToList();
                    //        foreach (var PromotionStoreTypeMapping in Promotion.PromotionStoreTypeMappings)
                    //        {
                    //            if (ExceptIds.Contains(PromotionStoreTypeMapping.StoreTypeId))
                    //                PromotionStoreTypeMapping.AddError(nameof(PromotionValidator), nameof(PromotionStoreTypeMapping.StoreType), ErrorCode.StoreTypeNotExisted);
                    //        }
                    //    }
                    //}
                }
            }
            return Promotion.IsValidated;
        }

        private async Task<bool> ValidateDate(Promotion Promotion)
        {
            if (Promotion.StartDate < new DateTime(2000, 1, 1))
                Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.StartDate), ErrorCode.StartDateEmpty);

            if (Promotion.EndDate.HasValue)
            {
                if (Promotion.EndDate.Value.Date < StaticParams.DateTimeNow.Date)
                {
                    Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.EndDate), ErrorCode.EndDateWrong);
                }
                else if (Promotion.EndDate.Value < Promotion.StartDate)
                {
                    Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.EndDate), ErrorCode.EndDateWrong);
                }
            }

            return Promotion.IsValidated;
        }

        private async Task<bool> ValidateStatus(Promotion Promotion)
        {
            if (StatusEnum.ACTIVE.Id != Promotion.StatusId && StatusEnum.INACTIVE.Id != Promotion.StatusId)
                Promotion.AddError(nameof(PromotionValidator), nameof(Promotion.Status), ErrorCode.StatusNotExisted);
            return Promotion.IsValidated;
        }

        public async Task<bool> Create(Promotion Promotion)
        {
            await ValidateCode(Promotion);
            await ValidateName(Promotion);
            await ValidateOrganization(Promotion);
            await ValidatePromotionType(Promotion);
            await ValidateDate(Promotion);
            await ValidateStatus(Promotion);
            return Promotion.IsValidated;
        }

        public async Task<bool> Update(Promotion Promotion)
        {
            if (await ValidateId(Promotion))
            {
                await ValidateCode(Promotion);
                await ValidateName(Promotion);
                await ValidateOrganization(Promotion);
                await ValidatePromotionType(Promotion);
                await ValidateDate(Promotion);
                await ValidateStatus(Promotion);
            }
            return Promotion.IsValidated;
        }

        public async Task<bool> UpdateDirectSalesOrder(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            if (PromotionPromotionPolicyMapping.PromotionPolicy != null && PromotionPromotionPolicyMapping.PromotionPolicy.PromotionDirectSalesOrders != null)
            {
                foreach (var PromotionDirectSalesOrder in PromotionPromotionPolicyMapping.PromotionPolicy.PromotionDirectSalesOrders)
                {
                    //if(PromotionDirectSalesOrder.PromotionDiscountTypeId == 0)
                    //{
                    //    PromotionDirectSalesOrder.AddError(nameof(PromotionValidator), nameof(PromotionDirectSalesOrder.PromotionDiscountType), ErrorCode.PromotionDiscountTypeEmpty);
                    //}
                    //else if(PromotionDirectSalesOrder.PromotionDiscountTypeId == PromotionDiscountTypeEnum.AMOUNT.Id)
                    //{
                    //    if(PromotionDirectSalesOrder.DiscountValue.HasValue == false)
                    //    {
                    //        PromotionDirectSalesOrder.AddError(nameof(PromotionValidator), nameof(PromotionDirectSalesOrder.DiscountValue), ErrorCode.DiscountValueEmpty);
                    //    }
                    //}
                    //else if (PromotionDirectSalesOrder.PromotionDiscountTypeId == PromotionDiscountTypeEnum.PERCENTAGE.Id)
                    //{
                    //    if (PromotionDirectSalesOrder.DiscountPercentage.HasValue == false)
                    //    {
                    //        PromotionDirectSalesOrder.AddError(nameof(PromotionValidator), nameof(PromotionDirectSalesOrder.DiscountPercentage), ErrorCode.DiscountPercentageEmpty);
                    //    }
                    //}
                    //else if (PromotionDirectSalesOrder.PromotionDiscountTypeId == PromotionDiscountTypeEnum.PRICE_FIXED.Id)
                    //{
                    //    if (PromotionDirectSalesOrder.DiscountPercentage.HasValue == false)
                    //    {
                    //        PromotionDirectSalesOrder.AddError(nameof(PromotionValidator), nameof(PromotionDirectSalesOrder.DiscountPercentage), ErrorCode.DiscountPercentageEmpty);
                    //    }
                    //}
                }
            }
            return PromotionPromotionPolicyMapping.IsValidated;
        }

        public async Task<bool> UpdateSamePrice(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            //if (await ValidateId(Promotion))
            //{

            //}
            return PromotionPromotionPolicyMapping.IsValidated;
        }

        public async Task<bool> UpdateProduct(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            //if (await ValidateId(Promotion))
            //{

            //}
            return PromotionPromotionPolicyMapping.IsValidated;
        }

        public async Task<bool> UpdateProductGrouping(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            //if (await ValidateId(Promotion))
            //{

            //}
            return PromotionPromotionPolicyMapping.IsValidated;
        }

        public async Task<bool> UpdateProductType(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            //if (await ValidateId(Promotion))
            //{

            //}
            return PromotionPromotionPolicyMapping.IsValidated;
        }

        public async Task<bool> UpdateCombo(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            //if (await ValidateId(Promotion))
            //{

            //}
            return PromotionPromotionPolicyMapping.IsValidated;
        }

        public async Task<bool> Delete(Promotion Promotion)
        {
            if (await ValidateId(Promotion))
            {
            }
            return Promotion.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Promotion> Promotions)
        {
            foreach (Promotion Promotion in Promotions)
            {
                await Delete(Promotion);
            }
            return Promotions.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<Promotion> Promotions)
        {
            return true;
        }
    }
}
