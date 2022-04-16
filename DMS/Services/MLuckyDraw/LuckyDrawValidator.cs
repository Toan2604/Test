using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS;
using DMS.Common;
using DMS.Enums;
using DMS.Entities;
using DMS.Repositories;
using DMS.Helpers;

namespace DMS.Services.MLuckyDraw
{
    public interface ILuckyDrawValidator : IServiceScoped
    {
        Task Get(LuckyDraw LuckyDraw);
        Task<bool> Create(LuckyDraw LuckyDraw);
        Task<bool> Update(LuckyDraw LuckyDraw);
        Task<bool> DrawByEmployee(LuckyDraw LuckyDraw);
        Task<bool> Delete(LuckyDraw LuckyDraw);
        Task<bool> BulkDelete(List<LuckyDraw> LuckyDraws);
        Task<bool> Import(List<LuckyDraw> LuckyDraws);
    }

    public class LuckyDrawValidator : ILuckyDrawValidator
    {
        private IUOW UOW;
        private ICurrentContext CurrentContext;
        private LuckyDrawMessage LuckyDrawMessage;

        public LuckyDrawValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.LuckyDrawMessage = new LuckyDrawMessage();
        }
        public async Task Get(LuckyDraw LuckyDraw)
        {
        }

        public async Task<bool> Create(LuckyDraw LuckyDraw)
        {
            await ValidateCode(LuckyDraw);
            await ValidateName(LuckyDraw);
            await ValidateRevenuePerTurn(LuckyDraw);
            await ValidateStartAt(LuckyDraw);
            await ValidateEndAt(LuckyDraw);
            await ValidateUsed(LuckyDraw);
            await ValidateAvatarImage(LuckyDraw);
            await ValidateImage(LuckyDraw);
            await ValidateDescription(LuckyDraw);
            await ValidateLuckyDrawType(LuckyDraw);
            await ValidateOrganization(LuckyDraw);
            await ValidateStatus(LuckyDraw);
            await ValidateLuckyDrawStructures(LuckyDraw);
            await ValidateLuckyDrawMappings(LuckyDraw);
            return LuckyDraw.IsValidated;
        }
        public async Task<bool> Update(LuckyDraw LuckyDraw)
        {
            if (await ValidateId(LuckyDraw))
            {
                await ValidateCode(LuckyDraw);
                await ValidateName(LuckyDraw);
                await ValidateRevenuePerTurn(LuckyDraw);
                await ValidateStartAt(LuckyDraw);
                await ValidateEndAt(LuckyDraw);
                await ValidateUsed(LuckyDraw);
                await ValidateAvatarImage(LuckyDraw);
                await ValidateImage(LuckyDraw);
                await ValidateDescription(LuckyDraw);
                await ValidateLuckyDrawType(LuckyDraw);
                await ValidateOrganization(LuckyDraw);
                await ValidateStatus(LuckyDraw);
                await ValidateLuckyDrawStructures(LuckyDraw);
                await ValidateLuckyDrawMappings(LuckyDraw);
            }
            return LuckyDraw.IsValidated;
        }
        public async Task<bool> DrawByEmployee(LuckyDraw LuckyDraw)
        {
            if (await ValidateId(LuckyDraw))
            {
                await ValidateRegistratedStore(LuckyDraw);
                await ValidateDraw(LuckyDraw);
            }
            return LuckyDraw.IsValidated;
        }
        public async Task<bool> Delete(LuckyDraw LuckyDraw)
        {
            if (await ValidateId(LuckyDraw))
            {
            }
            return LuckyDraw.IsValidated;
        }        
        public async Task<bool> BulkDelete(List<LuckyDraw> LuckyDraws)
        {
            foreach (LuckyDraw LuckyDraw in LuckyDraws)
            {
                await Delete(LuckyDraw);
            }
            return LuckyDraws.All(x => x.IsValidated);
        }        
        public async Task<bool> Import(List<LuckyDraw> LuckyDraws)
        {
            return true;
        }        
        public async Task<bool> ValidateId(LuckyDraw LuckyDraw)
        {
            LuckyDrawFilter LuckyDrawFilter = new LuckyDrawFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = LuckyDraw.Id },
                Selects = LuckyDrawSelect.Id
            };

            int count = await UOW.LuckyDrawRepository.CountAll(LuckyDrawFilter);
            if (count == 0)
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Id), LuckyDrawMessage.Error.IdNotExisted, LuckyDrawMessage);
            return count == 1;
        }
        public async Task<bool> ValidateCode(LuckyDraw LuckyDraw)
        {
            if(string.IsNullOrEmpty(LuckyDraw.Code))
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Code), LuckyDrawMessage.Error.CodeEmpty, LuckyDrawMessage);
            }
            else
            {
                var Code = LuckyDraw.Code;
                if (LuckyDraw.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(LuckyDraw.Code))
                {
                    LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Code), LuckyDrawMessage.Error.CodeHasSpecialCharacter);
                }

                LuckyDrawFilter LuckyDrawFilter = new LuckyDrawFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { NotEqual = LuckyDraw.Id },
                    Code = new StringFilter { Equal = LuckyDraw.Code },
                    Selects = LuckyDrawSelect.Code
                };

                int count = await UOW.LuckyDrawRepository.Count(LuckyDrawFilter);
                if (count != 0)
                    LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Code), LuckyDrawMessage.Error.CodeExisted);
            }
            return true;
        }
        public async Task<bool> ValidateName(LuckyDraw LuckyDraw)
        {
            if(string.IsNullOrEmpty(LuckyDraw.Name))
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Name), LuckyDrawMessage.Error.NameEmpty, LuckyDrawMessage);
            }
            return true;
        }
        public async Task<bool> ValidateRevenuePerTurn(LuckyDraw LuckyDraw)
        {
            if (LuckyDraw.RevenuePerTurn <= 0)
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.RevenuePerTurn), LuckyDrawMessage.Error.RevenuePerTurnInvalid, LuckyDrawMessage);
                return false;
            }
            return true;
        }
        public async Task<bool> ValidateStartAt(LuckyDraw LuckyDraw)
        {       
            if(LuckyDraw.StartAt <= new DateTime(2000, 1, 1))
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.StartAt), LuckyDrawMessage.Error.StartAtEmpty, LuckyDrawMessage);
            }
            if (LuckyDraw.Id == 0 && LuckyDraw.StartAt > new DateTime(2000, 1, 1) && LuckyDraw.StartAt < StaticParams.DateTimeNow.Date)
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.StartAt), LuckyDrawMessage.Error.StartAtInvalid, LuckyDrawMessage);
            }
            return true;
        }
        public async Task<bool> ValidateEndAt(LuckyDraw LuckyDraw)
        {       
            if(LuckyDraw.EndAt <= new DateTime(2000, 1, 1))
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.EndAt), LuckyDrawMessage.Error.EndAtEmpty, LuckyDrawMessage);
            }
            else if (LuckyDraw.EndAt < StaticParams.DateTimeNow.Date)
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.EndAt), LuckyDrawMessage.Error.EndAtInvalid, LuckyDrawMessage);
            }
            if (LuckyDraw.EndAt < LuckyDraw.StartAt)
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.EndAt), LuckyDrawMessage.Error.EndAtInvalid, LuckyDrawMessage);
            }
            return true;
        }
        public async Task<bool> ValidateUsed(LuckyDraw LuckyDraw)
        {   
            return true;
        }
        public async Task<bool> ValidateAvatarImage(LuckyDraw LuckyDraw)
        {
            if (LuckyDraw.AvatarImageId == 0)
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.AvatarImage), LuckyDrawMessage.Error.AvatarImageEmpty, LuckyDrawMessage);
                return false;
            }
            int count = await UOW.ImageRepository.CountAll(new ImageFilter
            {
                Id = new IdFilter { Equal = LuckyDraw.AvatarImageId },
            });
            if (count == 0)
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.AvatarImage), LuckyDrawMessage.Error.AvatarImageNotExisted, LuckyDrawMessage);
                return false;
            }
            return true;
        }
        public async Task<bool> ValidateImage(LuckyDraw LuckyDraw)
        {
            if (LuckyDraw.ImageId == 0)
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Image), LuckyDrawMessage.Error.ImageEmpty, LuckyDrawMessage);
                return false;
            }
            int count = await UOW.ImageRepository.CountAll(new ImageFilter
            {
                Id = new IdFilter{ Equal =  LuckyDraw.ImageId },
            });
            if(count == 0)
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Image), LuckyDrawMessage.Error.ImageNotExisted, LuckyDrawMessage);
                return false;
            }
            return true;
        }
        public async Task<bool> ValidateDescription(LuckyDraw LuckyDraw)
        {
            if (string.IsNullOrEmpty(LuckyDraw.Description))
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Description), LuckyDrawMessage.Error.DescriptionEmpty, LuckyDrawMessage);
            }
            return true;
        }
        public async Task<bool> ValidateLuckyDrawType(LuckyDraw LuckyDraw)
        {       
            if(LuckyDraw.LuckyDrawTypeId == 0)
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.LuckyDrawType), LuckyDrawMessage.Error.LuckyDrawTypeEmpty, LuckyDrawMessage);
                return false;
            }
            int count = await UOW.LuckyDrawTypeRepository.CountAll(new LuckyDrawTypeFilter
            {
                Id = new IdFilter{ Equal =  LuckyDraw.LuckyDrawTypeId },
            });
            if(count == 0)
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.LuckyDrawType), LuckyDrawMessage.Error.LuckyDrawTypeNotExisted, LuckyDrawMessage);
                return false;
            }
            return true;
        }
        public async Task<bool> ValidateOrganization(LuckyDraw LuckyDraw)
        {       
            if(LuckyDraw.OrganizationId == 0)
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Organization), LuckyDrawMessage.Error.OrganizationEmpty, LuckyDrawMessage);
                return false;
            }
            int count = await UOW.OrganizationRepository.CountAll(new OrganizationFilter
            {
                Id = new IdFilter{ Equal =  LuckyDraw.OrganizationId },
            });
            if(count == 0)
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Organization), LuckyDrawMessage.Error.OrganizationNotExisted, LuckyDrawMessage);
                return false;
            }
            return true;
        }
        public async Task<bool> ValidateStatus(LuckyDraw LuckyDraw)
        {
            if (StatusEnum.ACTIVE.Id != LuckyDraw.StatusId && StatusEnum.INACTIVE.Id != LuckyDraw.StatusId)
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Status), LuckyDrawMessage.Error.StatusNotExisted, LuckyDrawMessage);
            return true;

        }
        public async Task<bool> ValidateLuckyDrawMappings(LuckyDraw LuckyDraw)
        {
            if (LuckyDraw.Id == 0 | LuckyDraw.Used == false)
            {
                if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.STORE.Id && (LuckyDraw.LuckyDrawStoreMappings == null || !LuckyDraw.LuckyDrawStoreMappings.Any()))
                {
                    LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.LuckyDrawStoreMappings), LuckyDrawMessage.Error.LuckyDrawStoreMapping_StoreEmpty, LuckyDrawMessage);
                }
                if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.STOREGROUPING.Id && (LuckyDraw.LuckyDrawStoreGroupingMappings == null || !LuckyDraw.LuckyDrawStoreGroupingMappings.Any()))
                {
                    LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.LuckyDrawStoreGroupingMappings), LuckyDrawMessage.Error.LuckyDrawStoreGroupingMapping_StoreGroupingEmpty, LuckyDrawMessage);
                }
                if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.STORETYPE.Id && (LuckyDraw.LuckyDrawStoreTypeMappings == null || !LuckyDraw.LuckyDrawStoreTypeMappings.Any()))
                {
                    LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.LuckyDrawStoreTypeMappings), LuckyDrawMessage.Error.LuckyDrawStoreTypeMapping_StoreTypeEmpty, LuckyDrawMessage);
                }
                if (LuckyDraw.LuckyDrawStoreMappings != null && LuckyDraw.LuckyDrawStoreMappings.Any())
                {
                    var StoreIds = LuckyDraw.LuckyDrawStoreMappings.Select(x => x.StoreId).ToList();
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
                    foreach (var LuckyDrawStoreMapping in LuckyDraw.LuckyDrawStoreMappings)
                    {
                        if (ExceptIds.Contains(LuckyDrawStoreMapping.StoreId))
                            LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.LuckyDrawStoreMappings), LuckyDrawMessage.Error.LuckyDrawStoreMapping_StoreNotExisted, LuckyDrawMessage);
                    }
                }
                if (LuckyDraw.LuckyDrawStoreTypeMappings != null && LuckyDraw.LuckyDrawStoreTypeMappings.Any())
                {
                    var StoreTypeIds = LuckyDraw.LuckyDrawStoreTypeMappings.Select(x => x.StoreTypeId).ToList();
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
                    foreach (var LuckyDrawStoreTypeMapping in LuckyDraw.LuckyDrawStoreTypeMappings)
                    {
                        if (ExceptIds.Contains(LuckyDrawStoreTypeMapping.StoreTypeId))
                            LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.LuckyDrawStoreTypeMappings), LuckyDrawMessage.Error.LuckyDrawStoreTypeMapping_StoreTypeNotExisted, LuckyDrawMessage);
                    }
                }

                if (LuckyDraw.LuckyDrawStoreGroupingMappings != null && LuckyDraw.LuckyDrawStoreGroupingMappings.Any())
                {
                    var StoreGroupingIds = LuckyDraw.LuckyDrawStoreGroupingMappings.Select(x => x.StoreGroupingId).ToList();
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
                    foreach (var LuckyDrawStoreGroupingMapping in LuckyDraw.LuckyDrawStoreGroupingMappings)
                    {
                        if (ExceptIds.Contains(LuckyDrawStoreGroupingMapping.StoreGroupingId))
                            LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.LuckyDrawStoreGroupingMappings), LuckyDrawMessage.Error.LuckyDrawStoreGroupingMapping_StoreGroupingNotExisted, LuckyDrawMessage);
                    }
                }

            }
            else
            {
                var OldData = await UOW.LuckyDrawRepository.Get(LuckyDraw.Id);
                if (LuckyDraw.LuckyDrawTypeId != OldData.LuckyDrawTypeId)
                {
                    LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.LuckyDrawType), LuckyDrawMessage.Error.LuckyDrawUsed, LuckyDrawMessage);
                }
                else
                {
                    if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.STORE.Id)
                    {
                        var OldDataIds = OldData.LuckyDrawStoreMappings?.Select(x => x.StoreId).ToList();
                        var StoreIds = LuckyDraw.LuckyDrawStoreMappings?.Select(x => x.StoreId).ToList();
                        var except = OldDataIds.Except(StoreIds).Count();
                        var except2 = StoreIds.Except(OldDataIds).Count();
                        if (except > 0 | except2 > 0)
                            LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.LuckyDrawType), LuckyDrawMessage.Error.LuckyDrawUsed, LuckyDrawMessage);
                    }
                    if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.STORETYPE.Id)
                    {
                        var OldDataIds = OldData.LuckyDrawStoreTypeMappings?.Select(x => x.StoreTypeId).ToList();
                        var StoreTypeIds = LuckyDraw.LuckyDrawStoreTypeMappings?.Select(x => x.StoreTypeId).ToList();
                        var except = OldDataIds.Except(StoreTypeIds).Count();
                        var except2 = StoreTypeIds.Except(OldDataIds).Count();
                        if (except > 0 | except2 > 0)
                            LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.LuckyDrawType), LuckyDrawMessage.Error.LuckyDrawUsed, LuckyDrawMessage); 
                    }
                    if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.STOREGROUPING.Id)
                    {
                        var OldDataIds = OldData.LuckyDrawStoreGroupingMappings?.Select(x => x.StoreGroupingId).ToList();
                        var StoreGroupingIds = LuckyDraw.LuckyDrawStoreGroupingMappings?.Select(x => x.StoreGroupingId).ToList();
                        var except = OldDataIds.Except(StoreGroupingIds).Count();
                        var except2 = StoreGroupingIds.Except(OldDataIds).Count();
                        if (except > 0 | except2 > 0)
                            LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.LuckyDrawType), LuckyDrawMessage.Error.LuckyDrawUsed, LuckyDrawMessage);
                    }
                }
            }

            return true;
        }
        public async Task<bool> ValidateLuckyDrawStructures(LuckyDraw LuckyDraw)
        {
            if (LuckyDraw.LuckyDrawStructures == null || LuckyDraw.LuckyDrawStructures.Count == 0)
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Id), LuckyDrawMessage.Error.LuckyDrawStructuresEmpty);
            if (LuckyDraw.LuckyDrawStructures?.Any() ?? false)
            {
                foreach (LuckyDrawStructure LuckyDrawStructure in LuckyDraw.LuckyDrawStructures)
                {
                    if (string.IsNullOrEmpty(LuckyDrawStructure.Name))
                    {
                        LuckyDrawStructure.AddError(nameof(LuckyDrawValidator), nameof(LuckyDrawStructure.Name), LuckyDrawMessage.Error.LuckyDrawStructure_NameEmpty, LuckyDrawMessage);
                    }
                    if (string.IsNullOrEmpty(LuckyDrawStructure.Value))
                    {
                        LuckyDrawStructure.AddError(nameof(LuckyDrawValidator), nameof(LuckyDrawStructure.Value), LuckyDrawMessage.Error.LuckyDrawStructure_ValueEmpty, LuckyDrawMessage);
                    }
                    if (LuckyDrawStructure.Quantity <= 0)
                    {
                        LuckyDrawStructure.AddError(nameof(LuckyDrawValidator), nameof(LuckyDrawStructure.Quantity), LuckyDrawMessage.Error.LuckyDrawStructure_QuantityInvalid, LuckyDrawMessage);
                    }
                }
                if (LuckyDraw.Used == true && LuckyDraw.LuckyDrawStructures.Any(x => x.Id == 0))
                {
                    LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.LuckyDrawStructures), LuckyDrawMessage.Error.LuckyDrawUsed, LuckyDrawMessage);
                }
            }
            return true;
        }
        public async Task<bool> ValidateDraw(LuckyDraw LuckyDraw)
        {
            List<LuckyDrawStructure> LuckyDrawStructures = await UOW.LuckyDrawStructureRepository.List(new LuckyDrawStructureFilter
            {
                LuckyDrawId = new IdFilter { Equal = LuckyDraw.Id },
                Selects = LuckyDrawStructureSelect.Quantity,
                Skip = 0,
                Take = int.MaxValue
            });
            var TotalPrizes = LuckyDrawStructures.Sum(x => x.Quantity);
            List<LuckyDrawWinner> LuckyDrawWinners = await UOW.LuckyDrawWinnerRepository.List(new LuckyDrawWinnerFilter
            {
                LuckyDrawId = new IdFilter { Equal = LuckyDraw.Id },
                Selects = LuckyDrawWinnerSelect.Id,
                Skip = 0,
                Take = int.MaxValue
            });
            var TotalGivenPrizes = LuckyDrawWinners.Count();
            if (TotalPrizes <= TotalGivenPrizes)
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Id), LuckyDrawMessage.Error.PrizeOver, LuckyDrawMessage);
                return false;
            }
            return true;
        }
        public async Task<bool> ValidateRegistratedStore(LuckyDraw LuckyDraw)
        {
            var LuckyDrawRegistrations = await UOW.LuckyDrawRegistrationRepository.List(new LuckyDrawRegistrationFilter
            {
                Selects = LuckyDrawRegistrationSelect.Id | LuckyDrawRegistrationSelect.RemainingTurnCounter,
                Skip = 0,
                Take = int.MaxValue,
                LuckyDrawId = new IdFilter { Equal = LuckyDraw.Id },
                AppUserId = new IdFilter { Equal = CurrentContext.UserId },
                StoreId = new IdFilter { Equal = LuckyDraw.StoreId },
                IsDrawnByStore = false
            });
            if (LuckyDrawRegistrations.Count == 0)
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Id), LuckyDrawMessage.Error.StoreNotInScoped, LuckyDrawMessage);
                return false;
            }
            if (LuckyDrawRegistrations.Count > 0)
            {
                var RemainingTurnCounter = LuckyDrawRegistrations.Sum(x => x.RemainingTurnCounter);
                if (RemainingTurnCounter <= 0)
                {
                    LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Id), LuckyDrawMessage.Error.StoreOverTurn, LuckyDrawMessage);
                    return false;
                }

            }
            return true;
        }
        
    }
}
