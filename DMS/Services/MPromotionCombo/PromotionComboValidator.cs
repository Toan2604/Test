using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionCombo
{
    public interface IPromotionComboValidator : IServiceScoped
    {
        Task<bool> Create(PromotionCombo PromotionCombo);
        Task<bool> Update(PromotionCombo PromotionCombo);
        Task<bool> Delete(PromotionCombo PromotionCombo);
        Task<bool> BulkDelete(List<PromotionCombo> PromotionCombos);
        Task<bool> Import(List<PromotionCombo> PromotionCombos);
    }

    public class PromotionComboValidator : IPromotionComboValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PromotionComboValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PromotionCombo PromotionCombo)
        {
            PromotionComboFilter PromotionComboFilter = new PromotionComboFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PromotionCombo.Id },
                Selects = PromotionComboSelect.Id
            };

            int count = await UOW.PromotionComboRepository.Count(PromotionComboFilter);
            if (count == 0)
                PromotionCombo.AddError(nameof(PromotionComboValidator), nameof(PromotionCombo.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(PromotionCombo PromotionCombo)
        {
            return PromotionCombo.IsValidated;
        }

        public async Task<bool> Update(PromotionCombo PromotionCombo)
        {
            if (await ValidateId(PromotionCombo))
            {
            }
            return PromotionCombo.IsValidated;
        }

        public async Task<bool> Delete(PromotionCombo PromotionCombo)
        {
            if (await ValidateId(PromotionCombo))
            {
            }
            return PromotionCombo.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PromotionCombo> PromotionCombos)
        {
            foreach (PromotionCombo PromotionCombo in PromotionCombos)
            {
                await Delete(PromotionCombo);
            }
            return PromotionCombos.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<PromotionCombo> PromotionCombos)
        {
            return true;
        }
    }
}
