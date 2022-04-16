using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MLuckyNumberGrouping
{
    public interface ILuckyNumberGroupingValidator : IServiceScoped
    {
        Task<bool> Create(LuckyNumberGrouping LuckyNumberGrouping);
        Task<bool> Update(LuckyNumberGrouping LuckyNumberGrouping);
        Task<bool> Delete(LuckyNumberGrouping LuckyNumberGrouping);
        Task<bool> BulkDelete(List<LuckyNumberGrouping> LuckyNumberGroupings);
        Task<bool> Import(List<LuckyNumberGrouping> LuckyNumberGroupings);
    }

    public class LuckyNumberGroupingValidator : ILuckyNumberGroupingValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public LuckyNumberGroupingValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(LuckyNumberGrouping LuckyNumberGrouping)
        {
            LuckyNumberGroupingFilter LuckyNumberGroupingFilter = new LuckyNumberGroupingFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = LuckyNumberGrouping.Id },
                Selects = LuckyNumberGroupingSelect.Id
            };

            int count = await UOW.LuckyNumberGroupingRepository.Count(LuckyNumberGroupingFilter);
            if (count == 0)
                LuckyNumberGrouping.AddError(nameof(LuckyNumberGroupingValidator), nameof(LuckyNumberGrouping.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(LuckyNumberGrouping LuckyNumberGrouping)
        {
            return LuckyNumberGrouping.IsValidated;
        }

        public async Task<bool> Update(LuckyNumberGrouping LuckyNumberGrouping)
        {
            if (await ValidateId(LuckyNumberGrouping))
            {
            }
            return LuckyNumberGrouping.IsValidated;
        }

        public async Task<bool> Delete(LuckyNumberGrouping LuckyNumberGrouping)
        {
            if (await ValidateId(LuckyNumberGrouping))
            {
            }
            return LuckyNumberGrouping.IsValidated;
        }

        public async Task<bool> BulkDelete(List<LuckyNumberGrouping> LuckyNumberGroupings)
        {
            foreach (LuckyNumberGrouping LuckyNumberGrouping in LuckyNumberGroupings)
            {
                await Delete(LuckyNumberGrouping);
            }
            return LuckyNumberGroupings.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<LuckyNumberGrouping> LuckyNumberGroupings)
        {
            return true;
        }
    }
}
