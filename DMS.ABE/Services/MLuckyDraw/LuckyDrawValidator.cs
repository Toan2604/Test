using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.ABE;
using DMS.ABE.Common;
using DMS.ABE.Enums;
using DMS.ABE.Entities;
using DMS.ABE.Repositories;
using DMS.ABE.Helpers;

namespace DMS.ABE.Services.MLuckyDraw
{
    public interface ILuckyDrawValidator : IServiceScoped
    {
        Task<bool> Draw(LuckyDraw LuckyDraw);
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
        public async Task<bool> Draw(LuckyDraw LuckyDraw)
        {
            if (await ValidateId(LuckyDraw))
            {
                await ValidateRegistratedStore(LuckyDraw);
                await ValidateDraw(LuckyDraw);
            }
            return LuckyDraw.IsValidated;
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
            var TotalGivenPrizes = await UOW.LuckyDrawWinnerRepository.Count(new LuckyDrawWinnerFilter
            {
                LuckyDrawId = new IdFilter { Equal = LuckyDraw.Id },
                Selects = LuckyDrawWinnerSelect.Id,
                Skip = 0,
                Take = int.MaxValue
            });
            if (TotalPrizes <= TotalGivenPrizes)
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Id), LuckyDrawMessage.Error.PrizeOver, LuckyDrawMessage);
                return false;
            }
            return true;
        }
        public async Task<bool> ValidateRegistratedStore(LuckyDraw LuckyDraw)
        {
            var StoreUserId = CurrentContext.StoreUserId;
            StoreUser StoreUser = await UOW.StoreUserRepository.Get(StoreUserId);
            var StoreId = StoreUser.StoreId;
            var LuckyDrawRegistrations = await UOW.LuckyDrawRegistrationRepository.List(new LuckyDrawRegistrationFilter
            {
                Selects = LuckyDrawRegistrationSelect.Id | LuckyDrawRegistrationSelect.IsDrawnByStore | LuckyDrawRegistrationSelect.RemainingTurnCounter,
                Skip = 0,
                Take = int.MaxValue,
                IsDrawnByStore = true,
                LuckyDrawId = new IdFilter { Equal = LuckyDraw.Id },
                StoreId = new IdFilter { Equal = StoreId }
            });
            if (LuckyDrawRegistrations.Count == 0)
            {
                LuckyDraw.AddError(nameof(LuckyDrawValidator), nameof(LuckyDraw.Id), LuckyDrawMessage.Error.StoreNotRegistered, LuckyDrawMessage);
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
