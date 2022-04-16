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

namespace DMS.Services.MLuckyDrawWinner
{
    public interface ILuckyDrawWinnerValidator : IServiceScoped
    {
        Task Get(LuckyDrawWinner LuckyDrawWinner);
        Task<bool> Create(LuckyDrawWinner LuckyDrawWinner);
        Task<bool> Update(LuckyDrawWinner LuckyDrawWinner);
        Task<bool> Delete(LuckyDrawWinner LuckyDrawWinner);
        Task<bool> BulkDelete(List<LuckyDrawWinner> LuckyDrawWinners);
        Task<bool> Import(List<LuckyDrawWinner> LuckyDrawWinners);
    }

    public class LuckyDrawWinnerValidator : ILuckyDrawWinnerValidator
    {
        private IUOW UOW;
        private ICurrentContext CurrentContext;
        private LuckyDrawWinnerMessage LuckyDrawWinnerMessage;

        public LuckyDrawWinnerValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.LuckyDrawWinnerMessage = new LuckyDrawWinnerMessage();
        }

        public async Task Get(LuckyDrawWinner LuckyDrawWinner)
        {
        }

        public async Task<bool> Create(LuckyDrawWinner LuckyDrawWinner)
        {
            await ValidateLuckyDraw(LuckyDrawWinner);
            await ValidateLuckyDrawStructure(LuckyDrawWinner);
            return LuckyDrawWinner.IsValidated;
        }

        public async Task<bool> Update(LuckyDrawWinner LuckyDrawWinner)
        {
            if (await ValidateId(LuckyDrawWinner))
            {
                await ValidateLuckyDraw(LuckyDrawWinner);
                await ValidateLuckyDrawStructure(LuckyDrawWinner);
            }
            return LuckyDrawWinner.IsValidated;
        }

        public async Task<bool> Delete(LuckyDrawWinner LuckyDrawWinner)
        {
            if (await ValidateId(LuckyDrawWinner))
            {
            }
            return LuckyDrawWinner.IsValidated;
        }
        
        public async Task<bool> BulkDelete(List<LuckyDrawWinner> LuckyDrawWinners)
        {
            foreach (LuckyDrawWinner LuckyDrawWinner in LuckyDrawWinners)
            {
                await Delete(LuckyDrawWinner);
            }
            return LuckyDrawWinners.All(x => x.IsValidated);
        }
        
        public async Task<bool> Import(List<LuckyDrawWinner> LuckyDrawWinners)
        {
            return true;
        }
        
        public async Task<bool> ValidateId(LuckyDrawWinner LuckyDrawWinner)
        {
            LuckyDrawWinnerFilter LuckyDrawWinnerFilter = new LuckyDrawWinnerFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = LuckyDrawWinner.Id },
                Selects = LuckyDrawWinnerSelect.Id
            };

            int count = await UOW.LuckyDrawWinnerRepository.CountAll(LuckyDrawWinnerFilter);
            if (count == 0)
                LuckyDrawWinner.AddError(nameof(LuckyDrawWinnerValidator), nameof(LuckyDrawWinner.Id), LuckyDrawWinnerMessage.Error.IdNotExisted, LuckyDrawWinnerMessage);
            return count == 1;
        }

        public async Task<bool> ValidateLuckyDraw(LuckyDrawWinner LuckyDrawWinner)
        {       
            if(LuckyDrawWinner.LuckyDrawId == 0)
            {
                LuckyDrawWinner.AddError(nameof(LuckyDrawWinnerValidator), nameof(LuckyDrawWinner.LuckyDraw), LuckyDrawWinnerMessage.Error.LuckyDrawEmpty, LuckyDrawWinnerMessage);
                return false;
            }
            int count = await UOW.LuckyDrawRepository.CountAll(new LuckyDrawFilter
            {
                Id = new IdFilter{ Equal =  LuckyDrawWinner.LuckyDrawId },
            });
            if(count == 0)
            {
                LuckyDrawWinner.AddError(nameof(LuckyDrawWinnerValidator), nameof(LuckyDrawWinner.LuckyDraw), LuckyDrawWinnerMessage.Error.LuckyDrawNotExisted, LuckyDrawWinnerMessage);
                return false;
            }
            return true;
        }
        public async Task<bool> ValidateLuckyDrawStructure(LuckyDrawWinner LuckyDrawWinner)
        {       
            if(LuckyDrawWinner.LuckyDrawStructureId == 0)
            {
                LuckyDrawWinner.AddError(nameof(LuckyDrawWinnerValidator), nameof(LuckyDrawWinner.LuckyDrawStructure), LuckyDrawWinnerMessage.Error.LuckyDrawStructureEmpty, LuckyDrawWinnerMessage);
                return false;
            }
            int count = await UOW.LuckyDrawStructureRepository.CountAll(new LuckyDrawStructureFilter
            {
                Id = new IdFilter{ Equal =  LuckyDrawWinner.LuckyDrawStructureId },
            });
            if(count == 0)
            {
                LuckyDrawWinner.AddError(nameof(LuckyDrawWinnerValidator), nameof(LuckyDrawWinner.LuckyDrawStructure), LuckyDrawWinnerMessage.Error.LuckyDrawStructureNotExisted, LuckyDrawWinnerMessage);
                return false;
            }
            return true;
        }
    }
}
