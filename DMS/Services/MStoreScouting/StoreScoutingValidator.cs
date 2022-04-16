using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MStoreScouting
{
    public interface IStoreScoutingValidator : IServiceScoped
    {
        Task<bool> Create(StoreScouting StoreScouting);
        Task<bool> Update(StoreScouting StoreScouting);
        Task<bool> Delete(StoreScouting StoreScouting);
        Task<bool> Reject(StoreScouting StoreScouting);
        Task<bool> BulkDelete(List<StoreScouting> StoreScoutings);
        Task<bool> Import(List<StoreScouting> StoreScoutings);
    }

    public class StoreScoutingValidator : IStoreScoutingValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            CodeEmpty,
            CodeOverLength,
            NameEmpty,
            NameOverLength,
            OwnerPhoneOverLength,
            OwnerPhoneInUsed,
            OwnerPhoneEmpty,
            AddressEmpty,
            AddressOverLength,
            ProvinceNotExisted,
            DistrictNotExisted,
            WardNotExisted,
            StoreScoutingHasOpened,
            StoreScoutingHasRejected,
            LongitudeInvalid,
            LatitudeInvalid,
            LongitudeEmpty,
            LatitudeEmpty,
            StoreScoutingTypeNotExisted,
            OrganizationNotExisted,
            CreatorNotExisted,
            StoreScoutingStatusNotExisted
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public StoreScoutingValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(StoreScouting StoreScouting)
        {
            StoreScoutingFilter StoreScoutingFilter = new StoreScoutingFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = StoreScouting.Id },
                Selects = StoreScoutingSelect.Id
            };

            int count = await UOW.StoreScoutingRepository.Count(StoreScoutingFilter);
            if (count == 0)
                StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateOwnerPhone(StoreScouting StoreScouting)
        {
            if (!string.IsNullOrWhiteSpace(StoreScouting.OwnerPhone))
            {
                if (StoreScouting.OwnerPhone.Length > 255)
                {
                    StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.OwnerPhone), ErrorCode.OwnerPhoneOverLength);
                }
                else
                {
                    StoreScoutingFilter StoreScoutingFilter = new StoreScoutingFilter
                    {
                        OwnerPhone = new StringFilter { Equal = StoreScouting.OwnerPhone },
                        Id = new IdFilter { NotEqual = StoreScouting.Id }
                    };

                    int count = await UOW.StoreScoutingRepository.Count(StoreScoutingFilter);
                    if (count != 0)
                        StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.OwnerPhone), ErrorCode.OwnerPhoneInUsed);
                }
            }
            return StoreScouting.IsValidated;
        }

        private async Task<bool> ValidateAddress(StoreScouting StoreScouting)
        {
            if (string.IsNullOrWhiteSpace(StoreScouting.Address))
            {
                StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.Address), ErrorCode.AddressEmpty);
            }
            else if (StoreScouting.Address.Length > 3000)
            {
                StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.Address), ErrorCode.AddressOverLength);
            }
            return StoreScouting.IsValidated;
        }

        private async Task<bool> ValidateName(StoreScouting StoreScouting)
        {
            if (string.IsNullOrWhiteSpace(StoreScouting.Name))
            {
                StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.Name), ErrorCode.NameEmpty);
            }
            else if (StoreScouting.Name.Length > 500)
            {
                StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.Name), ErrorCode.NameOverLength);
            }
            return StoreScouting.IsValidated;
        }

        private async Task<bool> ValidateProvinceId(StoreScouting StoreScouting)
        {
            if (StoreScouting.ProvinceId != 0)
            {
                ProvinceFilter ProvinceFilter = new ProvinceFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = StoreScouting.ProvinceId },
                    StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id },
                    Selects = ProvinceSelect.Id
                };

                int count = await UOW.ProvinceRepository.Count(ProvinceFilter);
                if (count == 0)
                    StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.ProvinceId), ErrorCode.ProvinceNotExisted);
            }
            return StoreScouting.IsValidated;
        }
        private async Task<bool> ValidateDistrictId(StoreScouting StoreScouting)
        {
            if (StoreScouting.DistrictId != 0)
            {
                DistrictFilter DistrictFilter = new DistrictFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = StoreScouting.DistrictId },
                    StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id },
                    Selects = DistrictSelect.Id
                };

                int count = await UOW.DistrictRepository.Count(DistrictFilter);
                if (count == 0)
                    StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.DistrictId), ErrorCode.DistrictNotExisted);
            }
            return StoreScouting.IsValidated;
        }
        private async Task<bool> ValidateWardId(StoreScouting StoreScouting)
        {
            if (StoreScouting.WardId != 0)
            {
                WardFilter WardFilter = new WardFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = StoreScouting.WardId },
                    StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id },
                    Selects = WardSelect.Id
                };

                int count = await UOW.WardRepository.Count(WardFilter);
                if (count == 0)
                    StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.WardId), ErrorCode.WardNotExisted);
            }
            return StoreScouting.IsValidated;
        }

        private async Task<bool> ValidateLocation(StoreScouting StoreScouting)
        {
            if (StoreScouting.Longitude == 0)
            {
                StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.Longitude), ErrorCode.LongitudeEmpty);
            }
            else if (!(-180 <= StoreScouting.Longitude && StoreScouting.Longitude <= 180))
            {
                StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.Longitude), ErrorCode.LongitudeInvalid);
            }

            if (StoreScouting.Latitude == 0)
            {
                StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.Latitude), ErrorCode.LatitudeEmpty);
            }
            else if (!(-90 <= StoreScouting.Latitude && StoreScouting.Latitude <= 90))
            {
                StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.Latitude), ErrorCode.LatitudeInvalid);
            }
            return StoreScouting.IsValidated;
        }

        private async Task<bool> ValidateStatus(StoreScouting StoreScouting)
        {
            if (StoreScouting.StoreScoutingStatusId == Enums.StoreScoutingStatusEnum.OPENED.Id)
                StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.StoreScoutingStatus), ErrorCode.StoreScoutingHasOpened);
            else if (StoreScouting.StoreScoutingStatusId == Enums.StoreScoutingStatusEnum.REJECTED.Id)
                StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.StoreScoutingStatus), ErrorCode.StoreScoutingHasRejected);
            return StoreScouting.IsValidated;
        }

        public async Task<bool> Create(StoreScouting StoreScouting)
        {
            await ValidateOwnerPhone(StoreScouting);
            await ValidateName(StoreScouting);
            await ValidateAddress(StoreScouting);
            await ValidateProvinceId(StoreScouting);
            await ValidateDistrictId(StoreScouting);
            await ValidateWardId(StoreScouting);
            await ValidateLocation(StoreScouting);
            return StoreScouting.IsValidated;
        }

        public async Task<bool> Update(StoreScouting StoreScouting)
        {
            if (await ValidateId(StoreScouting))
            {
                await ValidateOwnerPhone(StoreScouting);
                await ValidateName(StoreScouting);
                await ValidateAddress(StoreScouting);
                await ValidateProvinceId(StoreScouting);
                await ValidateDistrictId(StoreScouting);
                await ValidateWardId(StoreScouting);
                await ValidateStatus(StoreScouting);
                await ValidateLocation(StoreScouting);
            }
            return StoreScouting.IsValidated;
        }

        public async Task<bool> Delete(StoreScouting StoreScouting)
        {
            if (await ValidateId(StoreScouting))
            {
                StoreScouting = await UOW.StoreScoutingRepository.Get(StoreScouting.Id);
                if (StoreScouting.StoreScoutingStatusId == StoreScoutingStatusEnum.OPENED.Id)
                    StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.StoreScoutingStatus), ErrorCode.StoreScoutingHasOpened);
            }
            return StoreScouting.IsValidated;
        }

        public async Task<bool> Reject(StoreScouting StoreScouting)
        {
            if (await ValidateId(StoreScouting))
            {
                await ValidateStatus(StoreScouting);
            }
            return StoreScouting.IsValidated;
        }

        public async Task<bool> BulkDelete(List<StoreScouting> StoreScoutings)
        {
            return true;
        }

        public async Task<bool> Import(List<StoreScouting> StoreScoutings)
        {
            var listAppUserCodeInDB = (await UOW.AppUserRepository.List(new AppUserFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Username
            })).Select(e => e.Username);
            var listOrganizationCodeInDB = (await UOW.OrganizationRepository.List(new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.Code
            })).Select(e => e.Code);
            var listStoreScoutingTypeCodeInDB = (await UOW.StoreScoutingTypeRepository.List(new StoreScoutingTypeFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreScoutingTypeSelect.Code
            })).Select(e => e.Code);
            var listProvinceCodeInDB = (await UOW.ProvinceRepository.List(new ProvinceFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProvinceSelect.Code
            })).Select(e => e.Code);
            var listDistrictCodeInDB = (await UOW.DistrictRepository.List(new DistrictFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = DistrictSelect.Code
            })).Select(e => e.Code);
            var listWardCodeInDB = (await UOW.WardRepository.List(new WardFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = WardSelect.Code
            })).Select(e => e.Code);
            var StoreScountingStatusIds = StoreScoutingStatusEnum.StoreScoutingStatusEnumList.Select(x => x.Id).ToList();
            foreach (var StoreScouting in StoreScoutings)
            {
                if (!listStoreScoutingTypeCodeInDB.Contains(StoreScouting.StoreScoutingType.Code))
                {
                    StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.StoreScoutingType), ErrorCode.StoreScoutingTypeNotExisted);
                }
                if (!listOrganizationCodeInDB.Contains(StoreScouting.Organization.Code))
                {
                    StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.Organization), ErrorCode.OrganizationNotExisted);
                }
                if (StoreScouting.Province != null)
                {
                    if (!listProvinceCodeInDB.Contains(StoreScouting.Province.Code))
                    {
                        StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.Province), ErrorCode.ProvinceNotExisted);
                    }
                }
                if (StoreScouting.District != null)
                {
                    if (!listDistrictCodeInDB.Contains(StoreScouting.District.Code))
                    {
                        StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.District), ErrorCode.DistrictNotExisted);
                    }
                }
                if (StoreScouting.Ward != null)
                {
                    if (!listWardCodeInDB.Contains(StoreScouting.Ward.Code))
                    {
                        StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.Ward), ErrorCode.WardNotExisted);
                    }
                }
                if (!listAppUserCodeInDB.Contains(StoreScouting.Creator.Username))
                {
                    StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.Creator), ErrorCode.CreatorNotExisted);
                }
                if (!StoreScountingStatusIds.Contains(StoreScouting.StoreScoutingStatusId))
                {
                    StoreScouting.AddError(nameof(StoreScoutingValidator), nameof(StoreScouting.StoreScoutingStatus), ErrorCode.StoreScoutingStatusNotExisted);
                }

                await ValidateName(StoreScouting);
                await ValidateAddress(StoreScouting);
                await ValidateLocation(StoreScouting);
                await ValidateOwnerPhone(StoreScouting);
            }
            return StoreScoutings.Any(s => !s.IsValidated) ? false : true;
        }
    }
}
