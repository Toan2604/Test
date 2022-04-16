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

namespace DMS.Services.MPromotionCode
{
    public interface IPromotionCodeValidator : IServiceScoped
    {
        Task<bool> Create(PromotionCode PromotionCode);
        Task<bool> Update(PromotionCode PromotionCode);
        Task<bool> Delete(PromotionCode PromotionCode);
        Task<bool> BulkDelete(List<PromotionCode> PromotionCodes);
        Task<bool> Import(List<PromotionCode> PromotionCodes);
    }

    public class PromotionCodeValidator : IPromotionCodeValidator
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
            StartDateEmpty,
            EndDateWrong,
            PromotionTypeEmpty,
            PromotionTypeNotExisted,
            PromotionDiscountTypeEmpty,
            PromotionDiscountTypeNotExisted,
            ValueEmpty,
            PromotionProductAppliedTypeEmpty,
            PromotionProductAppliedTypeNotExisted,
            ProductNotExisted,
            StoreNotExisted,
            StatusNotExisted,
            PromotionCodeInUsed
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PromotionCodeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PromotionCode PromotionCode)
        {
            PromotionCodeFilter PromotionCodeFilter = new PromotionCodeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PromotionCode.Id },
                Selects = PromotionCodeSelect.Id
            };

            int count = await UOW.PromotionCodeRepository.Count(PromotionCodeFilter);
            if (count == 0)
                PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateCode(PromotionCode PromotionCode)
        {
            if (string.IsNullOrWhiteSpace(PromotionCode.Code))
            {
                PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.Code), ErrorCode.CodeEmpty);
            }
            else
            {
                var Code = PromotionCode.Code;
                if (PromotionCode.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(PromotionCode.Code))
                {
                    PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.Code), ErrorCode.CodeHasSpecialCharacter);
                }

                PromotionCodeFilter PromotionCodeFilter = new PromotionCodeFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { NotEqual = PromotionCode.Id },
                    Code = new StringFilter { Equal = PromotionCode.Code },
                    Selects = PromotionCodeSelect.Code
                };

                int count = await UOW.PromotionCodeRepository.Count(PromotionCodeFilter);
                if (count != 0)
                    PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.Code), ErrorCode.CodeExisted);
            }

            return PromotionCode.IsValidated;
        }

        private async Task<bool> ValidateName(PromotionCode PromotionCode)
        {
            if (string.IsNullOrWhiteSpace(PromotionCode.Name))
            {
                PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.Name), ErrorCode.NameEmpty);
            }
            else if (PromotionCode.Name.Length > 255)
            {
                PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.Name), ErrorCode.NameOverLength);
            }
            return PromotionCode.IsValidated;
        }

        private async Task<bool> ValidateOrganization(PromotionCode PromotionCode)
        {
            if (PromotionCode.OrganizationId == 0)
            {
                PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.Organization), ErrorCode.OrganizationEmpty);
            }
            else
            {
                OrganizationFilter OrganizationFilter = new OrganizationFilter
                {
                    Id = new IdFilter { Equal = PromotionCode.OrganizationId }
                };

                var count = await UOW.OrganizationRepository.Count(OrganizationFilter);
                if (count == 0)
                    PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.Organization), ErrorCode.OrganizationNotExisted);
            }
            return PromotionCode.IsValidated;
        }

        private async Task<bool> ValidateDate(PromotionCode PromotionCode)
        {
            if (PromotionCode.StartDate < new DateTime(2000, 1, 1))
                PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.StartDate), ErrorCode.StartDateEmpty);
            if (PromotionCode.EndDate.HasValue)
            {
                if (PromotionCode.EndDate.Value.AddHours(CurrentContext.TimeZone).Date < StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Date)
                {
                    PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.EndDate), ErrorCode.EndDateWrong);
                }
                else if (PromotionCode.EndDate.Value < PromotionCode.StartDate)
                {
                    PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.EndDate), ErrorCode.EndDateWrong);
                }
            }
            return PromotionCode.IsValidated;
        }

        private async Task<bool> ValidatePromotionType(PromotionCode PromotionCode)
        {
            if (PromotionCode.PromotionTypeId == 0)
            {
                PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.PromotionType), ErrorCode.PromotionTypeEmpty);
            }
            else
            {
                var PromotionCodeTypeIds = PromotionTypeEnum.PromotionTypeEnumList.Select(x => x.Id).ToList();
                if (!PromotionCodeTypeIds.Contains(PromotionCode.PromotionTypeId))
                {
                    PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.PromotionType), ErrorCode.PromotionTypeNotExisted);
                }
            }
            return PromotionCode.IsValidated;
        }

        private async Task<bool> ValidatePromotionDiscountType(PromotionCode PromotionCode)
        {
            if (PromotionCode.PromotionDiscountTypeId == 0)
            {
                PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.PromotionDiscountType), ErrorCode.PromotionDiscountTypeEmpty);
            }
            else
            {
                var PromotionCodeTypeIds = PromotionDiscountTypeEnum.PromotionDiscountTypeEnumList.Select(x => x.Id).ToList();
                if (!PromotionCodeTypeIds.Contains(PromotionCode.PromotionDiscountTypeId))
                {
                    PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.PromotionDiscountType), ErrorCode.PromotionDiscountTypeNotExisted);
                }
                else
                {
                    if (PromotionCode.Value <= 0)
                    {
                        PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.Value), ErrorCode.ValueEmpty);
                    }
                }
            }
            return PromotionCode.IsValidated;
        }

        private async Task<bool> ValidatePromotionProductAppliedType(PromotionCode PromotionCode)
        {
            if (PromotionCode.PromotionProductAppliedTypeId == 0)
            {
                PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.PromotionProductAppliedType), ErrorCode.PromotionProductAppliedTypeEmpty);
            }
            else
            {
                var PromotionCodeTypeIds = PromotionProductAppliedTypeEnum.PromotionProductAppliedTypeEnumList.Select(x => x.Id).ToList();
                if (!PromotionCodeTypeIds.Contains(PromotionCode.PromotionProductAppliedTypeId))
                {
                    PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.PromotionProductAppliedType), ErrorCode.PromotionProductAppliedTypeNotExisted);
                }
            }
            return PromotionCode.IsValidated;
        }

        private async Task<bool> ValidateMapping(PromotionCode PromotionCode)
        {
            if (PromotionCode.PromotionCodeProductMappings != null && PromotionCode.PromotionCodeProductMappings.Any())
            {
                var ProductIds = PromotionCode.PromotionCodeProductMappings.Select(x => x.ProductId).ToList();
                var ListProductInDB = await UOW.ProductRepository.List(new ProductFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = ProductSelect.Id,
                    Id = new IdFilter { In = ProductIds },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                });
                var Ids = ListProductInDB.Select(x => x.Id).ToList();
                var ExceptIds = ProductIds.Except(Ids).ToList();
                foreach (var PromotionCodeProductMapping in PromotionCode.PromotionCodeProductMappings)
                {
                    if (ExceptIds.Contains(PromotionCodeProductMapping.ProductId))
                        PromotionCodeProductMapping.AddError(nameof(PromotionCodeValidator), nameof(PromotionCodeProductMapping.Product), ErrorCode.ProductNotExisted);
                }
            }

            if (PromotionCode.PromotionCodeOrganizationMappings != null && PromotionCode.PromotionCodeOrganizationMappings.Any())
            {
                var OrganizationIds = PromotionCode.PromotionCodeOrganizationMappings.Select(x => x.OrganizationId).ToList();
                var ListOrganizationInDB = await UOW.OrganizationRepository.List(new OrganizationFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = OrganizationSelect.Id,
                    Id = new IdFilter { In = OrganizationIds },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                });
                var Ids = ListOrganizationInDB.Select(x => x.Id).ToList();
                var ExceptIds = OrganizationIds.Except(Ids).ToList();
                foreach (var PromotionCodeOrganizationMapping in PromotionCode.PromotionCodeOrganizationMappings)
                {
                    if (ExceptIds.Contains(PromotionCodeOrganizationMapping.OrganizationId))
                        PromotionCodeOrganizationMapping.AddError(nameof(PromotionCodeValidator), nameof(PromotionCodeOrganizationMapping.Organization), ErrorCode.OrganizationNotExisted);
                }
            }

            if (PromotionCode.PromotionCodeStoreMappings != null && PromotionCode.PromotionCodeStoreMappings.Any())
            {
                var StoreIds = PromotionCode.PromotionCodeStoreMappings.Select(x => x.StoreId).ToList();
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
                foreach (var PromotionCodeStoreMapping in PromotionCode.PromotionCodeStoreMappings)
                {
                    if (ExceptIds.Contains(PromotionCodeStoreMapping.StoreId))
                        PromotionCodeStoreMapping.AddError(nameof(PromotionCodeValidator), nameof(PromotionCodeStoreMapping.Store), ErrorCode.StoreNotExisted);
                }
            }
            return PromotionCode.IsValidated;
        }

        private async Task<bool> ValidateStatus(PromotionCode PromotionCode)
        {
            if (StatusEnum.ACTIVE.Id != PromotionCode.StatusId && StatusEnum.INACTIVE.Id != PromotionCode.StatusId)
                PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.Status), ErrorCode.StatusNotExisted);
            return PromotionCode.IsValidated;
        }

        public async Task<bool> Create(PromotionCode PromotionCode)
        {
            await ValidateCode(PromotionCode);
            await ValidateName(PromotionCode);
            await ValidateOrganization(PromotionCode);
            await ValidateDate(PromotionCode);
            await ValidatePromotionType(PromotionCode);
            await ValidatePromotionDiscountType(PromotionCode);
            await ValidatePromotionProductAppliedType(PromotionCode);
            await ValidateMapping(PromotionCode);
            await ValidateStatus(PromotionCode);
            return PromotionCode.IsValidated;
        }

        public async Task<bool> Update(PromotionCode PromotionCode)
        {
            if (await ValidateId(PromotionCode))
            {
                await ValidateCode(PromotionCode);
                await ValidateName(PromotionCode);
                await ValidateOrganization(PromotionCode);
                await ValidateDate(PromotionCode);
                await ValidatePromotionType(PromotionCode);
                await ValidatePromotionDiscountType(PromotionCode);
                await ValidatePromotionProductAppliedType(PromotionCode);
                await ValidateMapping(PromotionCode);
                await ValidateStatus(PromotionCode);
            }
            return PromotionCode.IsValidated;
        }

        public async Task<bool> Delete(PromotionCode PromotionCode)
        {
            if (await ValidateId(PromotionCode))
            {
                var oldData = await UOW.PromotionCodeRepository.Get(PromotionCode.Id);
                if (oldData.Used)
                {
                    PromotionCode.AddError(nameof(PromotionCodeValidator), nameof(PromotionCode.Id), ErrorCode.PromotionCodeInUsed);
                }
            }
            return PromotionCode.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PromotionCode> PromotionCodes)
        {
            foreach (PromotionCode PromotionCode in PromotionCodes)
            {
                await Delete(PromotionCode);
            }
            return PromotionCodes.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<PromotionCode> PromotionCodes)
        {
            return true;
        }
    }
}
