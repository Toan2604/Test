using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MLuckyNumber
{
    public interface ILuckyNumberValidator : IServiceScoped
    {
        Task<bool> Create(LuckyNumber LuckyNumber);
        Task<bool> Update(LuckyNumber LuckyNumber);
        Task<bool> Delete(LuckyNumber LuckyNumber);
        Task<bool> BulkDelete(List<LuckyNumber> LuckyNumbers);
        Task<bool> Import(List<LuckyNumber> LuckyNumbers);
    }

    public class LuckyNumberValidator : ILuckyNumberValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            LuckyNumberInUsed
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public LuckyNumberValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(LuckyNumber LuckyNumber)
        {
            LuckyNumberFilter LuckyNumberFilter = new LuckyNumberFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = LuckyNumber.Id },
                Selects = LuckyNumberSelect.Id
            };

            int count = await UOW.LuckyNumberRepository.Count(LuckyNumberFilter);
            if (count == 0)
                LuckyNumber.AddError(nameof(LuckyNumberValidator), nameof(LuckyNumber.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(LuckyNumber LuckyNumber)
        {
            return LuckyNumber.IsValidated;
        }

        public async Task<bool> Update(LuckyNumber LuckyNumber)
        {
            if (await ValidateId(LuckyNumber))
            {
            }
            return LuckyNumber.IsValidated;
        }

        public async Task<bool> Delete(LuckyNumber LuckyNumber)
        {
            if (await ValidateId(LuckyNumber))
            {
                if (LuckyNumber.Used)
                {
                    LuckyNumber.AddError(nameof(LuckyNumberValidator), nameof(LuckyNumber.Id), ErrorCode.LuckyNumberInUsed);
                }
            }
            return LuckyNumber.IsValidated;
        }

        public async Task<bool> BulkDelete(List<LuckyNumber> LuckyNumbers)
        {
            foreach (LuckyNumber LuckyNumber in LuckyNumbers)
            {
                await Delete(LuckyNumber);
            }
            return LuckyNumbers.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<LuckyNumber> LuckyNumbers)
        {
            return true;
        }
    }
}
