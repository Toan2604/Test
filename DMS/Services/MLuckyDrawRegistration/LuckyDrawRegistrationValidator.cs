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

namespace DMS.Services.MLuckyDrawRegistration
{
    public interface ILuckyDrawRegistrationValidator : IServiceScoped
    {
        Task Get(LuckyDrawRegistration LuckyDrawRegistration);
        Task<bool> Create(LuckyDrawRegistration LuckyDrawRegistration);
        Task<bool> Update(LuckyDrawRegistration LuckyDrawRegistration);
        Task<bool> Delete(LuckyDrawRegistration LuckyDrawRegistration);
        Task<bool> BulkDelete(List<LuckyDrawRegistration> LuckyDrawRegistrations);
        Task<bool> Import(List<LuckyDrawRegistration> LuckyDrawRegistrations);
    }

    public class LuckyDrawRegistrationValidator : ILuckyDrawRegistrationValidator
    {
        private IUOW UOW;
        private ICurrentContext CurrentContext;
        private LuckyDrawRegistrationMessage LuckyDrawRegistrationMessage;

        public LuckyDrawRegistrationValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.LuckyDrawRegistrationMessage = new LuckyDrawRegistrationMessage();
        }

        public async Task Get(LuckyDrawRegistration LuckyDrawRegistration)
        {
        }

        public async Task<bool> Create(LuckyDrawRegistration LuckyDrawRegistration)
        {
            await ValidateRevenue(LuckyDrawRegistration);
            await ValidateTime(LuckyDrawRegistration);
            await ValidateAppUser(LuckyDrawRegistration);
            await ValidateLuckyDraw(LuckyDrawRegistration);
            await ValidateStore(LuckyDrawRegistration);
            return LuckyDrawRegistration.IsValidated;
        }

        public async Task<bool> Update(LuckyDrawRegistration LuckyDrawRegistration)
        {
            if (await ValidateId(LuckyDrawRegistration))
            {
                await ValidateRevenue(LuckyDrawRegistration);
                await ValidateTime(LuckyDrawRegistration);
                await ValidateAppUser(LuckyDrawRegistration);
                await ValidateLuckyDraw(LuckyDrawRegistration);
                await ValidateStore(LuckyDrawRegistration);
            }
            return LuckyDrawRegistration.IsValidated;
        }

        public async Task<bool> Delete(LuckyDrawRegistration LuckyDrawRegistration)
        {
            if (await ValidateId(LuckyDrawRegistration))
            {
            }
            return LuckyDrawRegistration.IsValidated;
        }
        
        public async Task<bool> BulkDelete(List<LuckyDrawRegistration> LuckyDrawRegistrations)
        {
            foreach (LuckyDrawRegistration LuckyDrawRegistration in LuckyDrawRegistrations)
            {
                await Delete(LuckyDrawRegistration);
            }
            return LuckyDrawRegistrations.All(x => x.IsValidated);
        }
        
        public async Task<bool> Import(List<LuckyDrawRegistration> LuckyDrawRegistrations)
        {
            return true;
        }
        
        public async Task<bool> ValidateId(LuckyDrawRegistration LuckyDrawRegistration)
        {
            LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter = new LuckyDrawRegistrationFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = LuckyDrawRegistration.Id },
                Selects = LuckyDrawRegistrationSelect.Id
            };

            int count = await UOW.LuckyDrawRegistrationRepository.CountAll(LuckyDrawRegistrationFilter);
            if (count == 0)
                LuckyDrawRegistration.AddError(nameof(LuckyDrawRegistrationValidator), nameof(LuckyDrawRegistration.Id), LuckyDrawRegistrationMessage.Error.IdNotExisted, LuckyDrawRegistrationMessage);
            return count == 1;
        }

        public async Task<bool> ValidateRevenue(LuckyDrawRegistration LuckyDrawRegistration)
        {   
            if (LuckyDrawRegistration.Revenue <= 0)
            {
                LuckyDrawRegistration.AddError(nameof(LuckyDrawRegistrationValidator), nameof(LuckyDrawRegistration.Revenue), LuckyDrawRegistrationMessage.Error.RevenueInvalid, LuckyDrawRegistrationMessage);
            }
            else
            {
                LuckyDrawFilter LuckyDrawFilter = new LuckyDrawFilter
                {
                    Skip = 0,
                    Take = 1,
                    Id = new IdFilter { Equal = LuckyDrawRegistration.LuckyDrawId },
                    Selects = LuckyDrawSelect.RevenuePerTurn
                };
                var RevenuePerTurn = (await UOW.LuckyDrawRepository.List(LuckyDrawFilter)).Select(x => x.RevenuePerTurn).FirstOrDefault();
                if (RevenuePerTurn == 0)
                {
                    LuckyDrawRegistration.AddError(nameof(LuckyDrawRegistrationValidator), nameof(LuckyDrawRegistration.LuckyDrawId), LuckyDrawRegistrationMessage.Error.LuckyDrawNotExisted, LuckyDrawRegistrationMessage);
                }
                if (LuckyDrawRegistration.Revenue < RevenuePerTurn)
                {
                    LuckyDrawRegistration.AddError(nameof(LuckyDrawRegistrationValidator), nameof(LuckyDrawRegistration.Revenue), LuckyDrawRegistrationMessage.Error.RevenueInsufficient, LuckyDrawRegistrationMessage);
                }
                LuckyDrawWinnerFilter LuckyDrawWinnerFilter = new LuckyDrawWinnerFilter
                {
                    Selects = LuckyDrawWinnerSelect.Id,
                    Skip = 0,
                    Take = int.MaxValue,
                    LuckyDrawId = new IdFilter { Equal = LuckyDrawRegistration.LuckyDrawId },                    
                };
                //tổng số giải đã trao (hoặc đã đăng ký lượt quay)
                var TotalGivenPrizes = await UOW.LuckyDrawWinnerRepository.Count(LuckyDrawWinnerFilter);
                LuckyDrawStructureFilter LuckyDrawStructureFilter = new LuckyDrawStructureFilter
                {
                    Selects = LuckyDrawStructureSelect.Id | LuckyDrawStructureSelect.Quantity,
                    Skip = 0,
                    Take = int.MaxValue,
                    LuckyDrawId = new IdFilter { Equal = LuckyDrawRegistration.LuckyDrawId }
                };
                var RemainingPrizes = (await UOW.LuckyDrawStructureRepository.List(LuckyDrawStructureFilter)).Sum(x => x.Quantity) - TotalGivenPrizes;
                if ((long)Math.Floor(LuckyDrawRegistration.Revenue / RevenuePerTurn) > RemainingPrizes)
                {
                    LuckyDrawRegistration.AddError(nameof(LuckyDrawRegistrationValidator), nameof(LuckyDrawRegistration.TurnCounter), LuckyDrawRegistrationMessage.Error.PrizesInsufficient, LuckyDrawRegistrationMessage);
                };
            }


            return true;
        }
        public async Task<bool> ValidateTime(LuckyDrawRegistration LuckyDrawRegistration)
        {       
            if(LuckyDrawRegistration.Time <= new DateTime(2000, 1, 1))
            {
                LuckyDrawRegistration.AddError(nameof(LuckyDrawRegistrationValidator), nameof(LuckyDrawRegistration.Time), LuckyDrawRegistrationMessage.Error.TimeEmpty, LuckyDrawRegistrationMessage);
            }
            return true;
        }
        public async Task<bool> ValidateAppUser(LuckyDrawRegistration LuckyDrawRegistration)
        {       
            if(LuckyDrawRegistration.AppUserId == 0)
            {
                LuckyDrawRegistration.AddError(nameof(LuckyDrawRegistrationValidator), nameof(LuckyDrawRegistration.AppUser), LuckyDrawRegistrationMessage.Error.AppUserEmpty, LuckyDrawRegistrationMessage);
                return false;
            }
            int count = await UOW.AppUserRepository.CountAll(new AppUserFilter
            {
                Id = new IdFilter{ Equal =  LuckyDrawRegistration.AppUserId },
            });
            if(count == 0)
            {
                LuckyDrawRegistration.AddError(nameof(LuckyDrawRegistrationValidator), nameof(LuckyDrawRegistration.AppUser), LuckyDrawRegistrationMessage.Error.AppUserNotExisted, LuckyDrawRegistrationMessage);
                return false;
            }
            return true;
        }
        public async Task<bool> ValidateLuckyDraw(LuckyDrawRegistration LuckyDrawRegistration)
        {       
            if(LuckyDrawRegistration.LuckyDrawId == 0)
            {
                LuckyDrawRegistration.AddError(nameof(LuckyDrawRegistrationValidator), nameof(LuckyDrawRegistration.LuckyDraw), LuckyDrawRegistrationMessage.Error.LuckyDrawEmpty, LuckyDrawRegistrationMessage);
                return false;
            }
            int count = await UOW.LuckyDrawRepository.CountAll(new LuckyDrawFilter
            {
                Id = new IdFilter{ Equal =  LuckyDrawRegistration.LuckyDrawId },
            });
            if(count == 0)
            {
                LuckyDrawRegistration.AddError(nameof(LuckyDrawRegistrationValidator), nameof(LuckyDrawRegistration.LuckyDraw), LuckyDrawRegistrationMessage.Error.LuckyDrawNotExisted, LuckyDrawRegistrationMessage);
                return false;
            }
            return true;
        }
        public async Task<bool> ValidateStore(LuckyDrawRegistration LuckyDrawRegistration)
        {       
            if(LuckyDrawRegistration.StoreId == 0)
            {
                LuckyDrawRegistration.AddError(nameof(LuckyDrawRegistrationValidator), nameof(LuckyDrawRegistration.Store), LuckyDrawRegistrationMessage.Error.StoreEmpty, LuckyDrawRegistrationMessage);
                return false;
            }
            int count = await UOW.StoreRepository.CountAll(new StoreFilter
            {
                Id = new IdFilter{ Equal =  LuckyDrawRegistration.StoreId },
            });
            if(count == 0)
            {
                LuckyDrawRegistration.AddError(nameof(LuckyDrawRegistrationValidator), nameof(LuckyDrawRegistration.Store), LuckyDrawRegistrationMessage.Error.StoreNotExisted, LuckyDrawRegistrationMessage);
                return false;
            }
            LuckyDraw LuckyDraw = await UOW.LuckyDrawRepository.Get(LuckyDrawRegistration.LuckyDrawId);
            if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.ALLSTORE.Id)
            {                
            }
            else if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.STOREGROUPING.Id)
            {
                var StoreGroupingIds = LuckyDraw.LuckyDrawStoreGroupingMappings.Select(x => x.StoreGroupingId).ToList();
                var Store = await UOW.StoreRepository.Get(LuckyDrawRegistration.StoreId);
                var Ids = Store.StoreStoreGroupingMappings.Select(x => x.StoreGroupingId).ToList();
                var counter = Ids.Count(x => StoreGroupingIds.Contains(x));
                if (counter == 0)
                    LuckyDrawRegistration.AddError(nameof(LuckyDrawRegistrationValidator), nameof(LuckyDrawRegistration.Store), LuckyDrawRegistrationMessage.Error.StoreNotInScoped, LuckyDrawRegistrationMessage);
            }
            else if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.STORETYPE.Id)
            {
                var StoreTypeIds = LuckyDraw.LuckyDrawStoreTypeMappings.Select(x => x.StoreTypeId).ToList();
                var Store = await UOW.StoreRepository.Get(LuckyDrawRegistration.StoreId);
                if (!StoreTypeIds.Contains(Store.StoreTypeId))
                {
                    LuckyDrawRegistration.AddError(nameof(LuckyDrawRegistrationValidator), nameof(LuckyDrawRegistration.Store), LuckyDrawRegistrationMessage.Error.StoreNotInScoped, LuckyDrawRegistrationMessage);
                }

            }
            else if (LuckyDraw.LuckyDrawTypeId == LuckyDrawTypeEnum.STORE.Id)
            {
                var StoreIds = LuckyDraw.LuckyDrawStoreMappings.Select(x => x.StoreId).ToList();
                if (!StoreIds.Contains(LuckyDrawRegistration.StoreId))
                {
                    LuckyDrawRegistration.AddError(nameof(LuckyDrawRegistrationValidator), nameof(LuckyDrawRegistration.Store), LuckyDrawRegistrationMessage.Error.StoreNotInScoped, LuckyDrawRegistrationMessage);
                }
            }
            if (LuckyDrawRegistration.IsDrawnByStore == true)
            {
                int counter = await UOW.StoreUserRepository.CountAll(new StoreUserFilter
                {
                    StoreId = new IdFilter { Equal = LuckyDrawRegistration.StoreId }
                });
                if (counter == 0)
                {
                    LuckyDrawRegistration.AddError(nameof(LuckyDrawRegistrationValidator), nameof(LuckyDrawRegistration.Store), LuckyDrawRegistrationMessage.Error.StoreUserNotExisted, LuckyDrawRegistrationMessage);
                    return false;
                }
            }
            return true;
        }
    }
}
