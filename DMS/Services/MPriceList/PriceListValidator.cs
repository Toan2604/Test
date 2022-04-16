using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPriceList
{
    public interface IPriceListValidator : IServiceScoped
    {
        Task<bool> Create(PriceList PriceList);
        Task<bool> Update(PriceList PriceList);
        Task<bool> Delete(PriceList PriceList);
        Task<bool> BulkDelete(List<PriceList> PriceLists);
        Task<bool> Import(List<PriceList> PriceLists);
    }

    public class PriceListValidator : IPriceListValidator
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
            StatusNotExisted,
            PriceListTypeEmpty,
            PriceListTypeNotExisted,
            EndDateInvalid,
            ItemNotExisted,
            StoreNotExisted,
            StoreTypeNotExisted,
            StoreGroupingNotExisted,
            SalesOrderTypeEmpty,
            SalesOrderTypeNotExisted
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PriceListValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PriceList PriceList)
        {
            PriceListFilter PriceListFilter = new PriceListFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PriceList.Id },
                Selects = PriceListSelect.Id
            };

            int count = await UOW.PriceListRepository.Count(PriceListFilter);
            if (count == 0)
                PriceList.AddError(nameof(PriceListValidator), nameof(PriceList.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateCode(PriceList PriceList)
        {
            if (string.IsNullOrWhiteSpace(PriceList.Code))
            {
                PriceList.AddError(nameof(PriceListValidator), nameof(PriceList.Code), ErrorCode.CodeEmpty);
            }
            else
            {
                var Code = PriceList.Code;
                if (PriceList.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(PriceList.Code))
                {
                    PriceList.AddError(nameof(PriceListValidator), nameof(PriceList.Code), ErrorCode.CodeHasSpecialCharacter);
                }

                PriceListFilter PriceListFilter = new PriceListFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { NotEqual = PriceList.Id },
                    Code = new StringFilter { Equal = PriceList.Code },
                    Selects = PriceListSelect.Code
                };

                int count = await UOW.PriceListRepository.Count(PriceListFilter);
                if (count != 0)
                    PriceList.AddError(nameof(PriceListValidator), nameof(PriceList.Code), ErrorCode.CodeExisted);
            }

            return PriceList.IsValidated;
        }

        private async Task<bool> ValidateName(PriceList PriceList)
        {
            if (string.IsNullOrWhiteSpace(PriceList.Name))
            {
                PriceList.AddError(nameof(PriceListValidator), nameof(PriceList.Name), ErrorCode.NameEmpty);
            }
            else if (PriceList.Name.Length > 255)
            {
                PriceList.AddError(nameof(PriceListValidator), nameof(PriceList.Name), ErrorCode.NameOverLength);
            }
            return PriceList.IsValidated;
        }

        private async Task<bool> ValidateOrganization(PriceList PriceList)
        {
            if (PriceList.OrganizationId == 0)
            {
                PriceList.AddError(nameof(PriceListValidator), nameof(PriceList.Organization), ErrorCode.OrganizationEmpty);
            }
            else
            {
                OrganizationFilter OrganizationFilter = new OrganizationFilter
                {
                    Id = new IdFilter { Equal = PriceList.OrganizationId }
                };

                var count = await UOW.OrganizationRepository.Count(OrganizationFilter);
                if (count == 0)
                    PriceList.AddError(nameof(PriceListValidator), nameof(PriceList.Organization), ErrorCode.OrganizationNotExisted);
            }
            return PriceList.IsValidated;
        }

        private async Task<bool> ValidateDate(PriceList PriceList)
        {
            if (PriceList.StartDate.HasValue)
            {
                if (PriceList.EndDate.HasValue)
                {
                    if (PriceList.EndDate.Value < StaticParams.DateTimeNow || PriceList.EndDate.Value <= PriceList.StartDate)
                    {
                        PriceList.AddError(nameof(PriceListValidator), nameof(PriceList.EndDate), ErrorCode.EndDateInvalid);
                    }
                }
            }
            return PriceList.IsValidated;
        }

        private async Task<bool> ValidatePriceListType(PriceList PriceList)
        {
            if (PriceList.PriceListTypeId == 0)
            {
                PriceList.AddError(nameof(PriceListValidator), nameof(PriceList.PriceListType), ErrorCode.PriceListTypeEmpty);
            }
            else
            {
                var PriceListTypeIds = PriceListTypeEnum.PriceListTypeEnumList.Select(x => x.Id).ToList();
                if (!PriceListTypeIds.Contains(PriceList.PriceListTypeId))
                {
                    PriceList.AddError(nameof(PriceListValidator), nameof(PriceList.PriceListType), ErrorCode.PriceListTypeNotExisted);
                }
            }
            return PriceList.IsValidated;
        }

        private async Task<bool> ValidateSalesOrderType(PriceList PriceList)
        {
            if (PriceList.SalesOrderTypeId == 0)
            {
                PriceList.AddError(nameof(PriceListValidator), nameof(PriceList.SalesOrderType), ErrorCode.SalesOrderTypeEmpty);
            }
            else
            {
                var SalesOrderTypeIds = SalesOrderTypeEnum.SalesOrderTypeEnumList.Select(x => x.Id).ToList();
                if (!SalesOrderTypeIds.Contains(PriceList.SalesOrderTypeId))
                {
                    PriceList.AddError(nameof(PriceListValidator), nameof(PriceList.SalesOrderType), ErrorCode.SalesOrderTypeNotExisted);
                }
            }
            return PriceList.IsValidated;
        }

        private async Task<bool> ValidateStatus(PriceList PriceList)
        {
            if (StatusEnum.ACTIVE.Id != PriceList.StatusId && StatusEnum.INACTIVE.Id != PriceList.StatusId)
                PriceList.AddError(nameof(PriceListValidator), nameof(PriceList.Status), ErrorCode.StatusNotExisted);
            return PriceList.IsValidated;
        }

        private async Task<bool> ValidateMapping(PriceList PriceList)
        {
            if (PriceList.PriceListItemMappings != null && PriceList.PriceListItemMappings.Any())
            {
                var ItemIds = PriceList.PriceListItemMappings.Select(x => x.ItemId).ToList();
                var ListItemInDB = await UOW.ItemRepository.List(new ItemFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = ItemSelect.Id,
                    Id = new IdFilter { In = ItemIds },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                });
                var Ids = ListItemInDB.Select(x => x.Id).ToList();
                var ExceptIds = ItemIds.Except(Ids).ToList();
                foreach (var PriceListItemMapping in PriceList.PriceListItemMappings)
                {
                    if (ExceptIds.Contains(PriceListItemMapping.ItemId))
                        PriceListItemMapping.AddError(nameof(PriceListValidator), nameof(PriceListItemMapping.Item), ErrorCode.ItemNotExisted);
                }
            }

            if (PriceList.PriceListStoreMappings != null && PriceList.PriceListStoreMappings.Any())
            {
                var StoreIds = PriceList.PriceListStoreMappings.Select(x => x.StoreId).ToList();
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
                foreach (var PriceListStoreMapping in PriceList.PriceListStoreMappings)
                {
                    if (ExceptIds.Contains(PriceListStoreMapping.StoreId))
                        PriceListStoreMapping.AddError(nameof(PriceListValidator), nameof(PriceListStoreMapping.Store), ErrorCode.StoreNotExisted);
                }
            }

            if (PriceList.PriceListStoreTypeMappings != null && PriceList.PriceListStoreTypeMappings.Any())
            {
                var StoreTypeIds = PriceList.PriceListStoreTypeMappings.Select(x => x.StoreTypeId).ToList();
                var ListStoreTypeInDB = await UOW.StoreTypeRepository.List(new StoreTypeFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = StoreTypeSelect.Id,
                    Id = new IdFilter { In = StoreTypeIds },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                });
                var Ids = ListStoreTypeInDB.Select(x => x.Id).ToList();
                var ExceptIds = StoreTypeIds.Except(Ids).ToList();
                foreach (var PriceListStoreTypeMapping in PriceList.PriceListStoreTypeMappings)
                {
                    if (ExceptIds.Contains(PriceListStoreTypeMapping.StoreTypeId))
                        PriceListStoreTypeMapping.AddError(nameof(PriceListValidator), nameof(PriceListStoreTypeMapping.StoreType), ErrorCode.StoreTypeNotExisted);
                }
            }

            if (PriceList.PriceListStoreGroupingMappings != null && PriceList.PriceListStoreGroupingMappings.Any())
            {
                var StoreGroupingIds = PriceList.PriceListStoreGroupingMappings.Select(x => x.StoreGroupingId).ToList();
                var ListStoreGroupingInDB = await UOW.StoreGroupingRepository.List(new StoreGroupingFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = StoreGroupingSelect.Id,
                    Id = new IdFilter { In = StoreGroupingIds },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },

                });
                var Ids = ListStoreGroupingInDB.Select(x => x.Id).ToList();
                var ExceptIds = StoreGroupingIds.Except(Ids).ToList();
                foreach (var PriceListStoreGroupingMapping in PriceList.PriceListStoreGroupingMappings)
                {
                    if (ExceptIds.Contains(PriceListStoreGroupingMapping.StoreGroupingId))
                        PriceListStoreGroupingMapping.AddError(nameof(PriceListValidator), nameof(PriceListStoreGroupingMapping.StoreGrouping), ErrorCode.StoreGroupingNotExisted);
                }
            }

            return PriceList.IsValidated;
        }

        public async Task<bool> Create(PriceList PriceList)
        {
            await ValidateCode(PriceList);
            await ValidateName(PriceList);
            await ValidateDate(PriceList);
            await ValidateOrganization(PriceList);
            await ValidateStatus(PriceList);
            await ValidatePriceListType(PriceList);
            await ValidateSalesOrderType(PriceList);
            await ValidateMapping(PriceList);
            return PriceList.IsValidated;
        }

        public async Task<bool> Update(PriceList PriceList)
        {
            if (await ValidateId(PriceList))
            {
                var oldData = await UOW.PriceListRepository.Get(PriceList.Id);
                if (oldData.RequestStateId != RequestStateEnum.APPROVED.Id)
                {
                    await ValidateCode(PriceList);
                    await ValidateName(PriceList);
                    await ValidateDate(PriceList);
                    await ValidateOrganization(PriceList);
                    await ValidateStatus(PriceList);
                    await ValidatePriceListType(PriceList);
                    await ValidateSalesOrderType(PriceList);
                    await ValidateMapping(PriceList);
                }
            }
            return PriceList.IsValidated;
        }

        public async Task<bool> Delete(PriceList PriceList)
        {
            if (await ValidateId(PriceList))
            {
            }
            return PriceList.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PriceList> PriceLists)
        {
            return true;
        }

        public async Task<bool> Import(List<PriceList> PriceLists)
        {
            return true;
        }
    }
}
