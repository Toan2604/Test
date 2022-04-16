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

namespace DMS.Services.MLuckyDrawStructure
{
    public interface ILuckyDrawStructureValidator : IServiceScoped
    {
        Task Get(LuckyDrawStructure LuckyDrawStructure);
        Task<bool> Create(LuckyDrawStructure LuckyDrawStructure);
        Task<bool> Update(LuckyDrawStructure LuckyDrawStructure);
        Task<bool> Delete(LuckyDrawStructure LuckyDrawStructure);
        Task<bool> BulkDelete(List<LuckyDrawStructure> LuckyDrawStructures);
        Task<bool> Import(List<LuckyDrawStructure> LuckyDrawStructures);
    }

    public class LuckyDrawStructureValidator : ILuckyDrawStructureValidator
    {
        private IUOW UOW;
        private ICurrentContext CurrentContext;
        private LuckyDrawStructureMessage LuckyDrawStructureMessage;

        public LuckyDrawStructureValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.LuckyDrawStructureMessage = new LuckyDrawStructureMessage();
        }

        public async Task Get(LuckyDrawStructure LuckyDrawStructure)
        {
        }

        public async Task<bool> Create(LuckyDrawStructure LuckyDrawStructure)
        {
            await ValidateName(LuckyDrawStructure);
            await ValidateValue(LuckyDrawStructure);
            await ValidateQuantity(LuckyDrawStructure);
            await ValidateLuckyDraw(LuckyDrawStructure);
            return LuckyDrawStructure.IsValidated;
        }

        public async Task<bool> Update(LuckyDrawStructure LuckyDrawStructure)
        {
            if (await ValidateId(LuckyDrawStructure))
            {
                await ValidateName(LuckyDrawStructure);
                await ValidateValue(LuckyDrawStructure);
                await ValidateQuantity(LuckyDrawStructure);
                await ValidateLuckyDraw(LuckyDrawStructure);
            }
            return LuckyDrawStructure.IsValidated;
        }

        public async Task<bool> Delete(LuckyDrawStructure LuckyDrawStructure)
        {
            if (await ValidateId(LuckyDrawStructure))
            {
            }
            return LuckyDrawStructure.IsValidated;
        }
        
        public async Task<bool> BulkDelete(List<LuckyDrawStructure> LuckyDrawStructures)
        {
            foreach (LuckyDrawStructure LuckyDrawStructure in LuckyDrawStructures)
            {
                await Delete(LuckyDrawStructure);
            }
            return LuckyDrawStructures.All(x => x.IsValidated);
        }
        
        public async Task<bool> Import(List<LuckyDrawStructure> LuckyDrawStructures)
        {
            return true;
        }
        
        public async Task<bool> ValidateId(LuckyDrawStructure LuckyDrawStructure)
        {
            LuckyDrawStructureFilter LuckyDrawStructureFilter = new LuckyDrawStructureFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = LuckyDrawStructure.Id },
                Selects = LuckyDrawStructureSelect.Id
            };

            int count = await UOW.LuckyDrawStructureRepository.CountAll(LuckyDrawStructureFilter);
            if (count == 0)
                LuckyDrawStructure.AddError(nameof(LuckyDrawStructureValidator), nameof(LuckyDrawStructure.Id), LuckyDrawStructureMessage.Error.IdNotExisted, LuckyDrawStructureMessage);
            return count == 1;
        }

        public async Task<bool> ValidateName(LuckyDrawStructure LuckyDrawStructure)
        {
            if(string.IsNullOrEmpty(LuckyDrawStructure.Name))
            {
                LuckyDrawStructure.AddError(nameof(LuckyDrawStructureValidator), nameof(LuckyDrawStructure.Name), LuckyDrawStructureMessage.Error.NameEmpty, LuckyDrawStructureMessage);
            }
            else if(LuckyDrawStructure.Name.Count() > 500)
            {
                LuckyDrawStructure.AddError(nameof(LuckyDrawStructureValidator), nameof(LuckyDrawStructure.Name), LuckyDrawStructureMessage.Error.NameOverLength, LuckyDrawStructureMessage);
            }
            return true;
        }
        public async Task<bool> ValidateValue(LuckyDrawStructure LuckyDrawStructure)
        {
            if(string.IsNullOrEmpty(LuckyDrawStructure.Value))
            {
                LuckyDrawStructure.AddError(nameof(LuckyDrawStructureValidator), nameof(LuckyDrawStructure.Value), LuckyDrawStructureMessage.Error.ValueEmpty, LuckyDrawStructureMessage);
            }
            else if(LuckyDrawStructure.Value.Count() > 500)
            {
                LuckyDrawStructure.AddError(nameof(LuckyDrawStructureValidator), nameof(LuckyDrawStructure.Value), LuckyDrawStructureMessage.Error.ValueOverLength, LuckyDrawStructureMessage);
            }
            return true;
        }
        public async Task<bool> ValidateQuantity(LuckyDrawStructure LuckyDrawStructure)
        {   
            return true;
        }
        public async Task<bool> ValidateLuckyDraw(LuckyDrawStructure LuckyDrawStructure)
        {       
            if(LuckyDrawStructure.LuckyDrawId == 0)
            {
                LuckyDrawStructure.AddError(nameof(LuckyDrawStructureValidator), nameof(LuckyDrawStructure.LuckyDraw), LuckyDrawStructureMessage.Error.LuckyDrawEmpty, LuckyDrawStructureMessage);
                return false;
            }
            int count = await UOW.LuckyDrawRepository.CountAll(new LuckyDrawFilter
            {
                Id = new IdFilter{ Equal =  LuckyDrawStructure.LuckyDrawId },
            });
            if(count == 0)
            {
                LuckyDrawStructure.AddError(nameof(LuckyDrawStructureValidator), nameof(LuckyDrawStructure.LuckyDraw), LuckyDrawStructureMessage.Error.LuckyDrawNotExisted, LuckyDrawStructureMessage);
                return false;
            }
            return true;
        }
    }
}
