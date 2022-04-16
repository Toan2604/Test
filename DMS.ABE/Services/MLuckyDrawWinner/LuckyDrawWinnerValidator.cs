using DMS.ABE.Common;
using DMS.ABE.Entities;
using DMS.ABE.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Services.MLuckyDrawWinner
{
    public interface ILuckyDrawWinnerValidator : IServiceScoped
    {
        Task Get(LuckyDrawWinner LuckyDrawWinner);
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
