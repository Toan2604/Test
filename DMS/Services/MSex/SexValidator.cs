using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MSex
{
    public interface ISexValidator : IServiceScoped
    {
        Task<bool> Create(Sex Sex);
        Task<bool> Update(Sex Sex);
        Task<bool> Delete(Sex Sex);
        Task<bool> BulkDelete(List<Sex> Sexes);
        Task<bool> Import(List<Sex> Sexes);
    }

    public class SexValidator : ISexValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public SexValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Sex Sex)
        {
            SexFilter SexFilter = new SexFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Sex.Id },
                Selects = SexSelect.Id
            };

            int count = await UOW.SexRepository.Count(SexFilter);
            if (count == 0)
                Sex.AddError(nameof(SexValidator), nameof(Sex.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(Sex Sex)
        {
            return Sex.IsValidated;
        }

        public async Task<bool> Update(Sex Sex)
        {
            if (await ValidateId(Sex))
            {
            }
            return Sex.IsValidated;
        }

        public async Task<bool> Delete(Sex Sex)
        {
            if (await ValidateId(Sex))
            {
            }
            return Sex.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Sex> Sexes)
        {
            return true;
        }

        public async Task<bool> Import(List<Sex> Sexes)
        {
            return true;
        }
    }
}
