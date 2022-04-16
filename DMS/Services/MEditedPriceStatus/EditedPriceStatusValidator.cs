using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MEditedPriceStatus
{
    public interface IEditedPriceStatusValidator : IServiceScoped
    {
        Task<bool> Create(EditedPriceStatus EditedPriceStatus);
        Task<bool> Update(EditedPriceStatus EditedPriceStatus);
        Task<bool> Delete(EditedPriceStatus EditedPriceStatus);
        Task<bool> BulkDelete(List<EditedPriceStatus> EditedPriceStatuses);
        Task<bool> Import(List<EditedPriceStatus> EditedPriceStatuses);
    }

    public class EditedPriceStatusValidator : IEditedPriceStatusValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public EditedPriceStatusValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(EditedPriceStatus EditedPriceStatus)
        {
            EditedPriceStatusFilter EditedPriceStatusFilter = new EditedPriceStatusFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = EditedPriceStatus.Id },
                Selects = EditedPriceStatusSelect.Id
            };

            int count = await UOW.EditedPriceStatusRepository.Count(EditedPriceStatusFilter);
            if (count == 0)
                EditedPriceStatus.AddError(nameof(EditedPriceStatusValidator), nameof(EditedPriceStatus.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(EditedPriceStatus EditedPriceStatus)
        {
            return EditedPriceStatus.IsValidated;
        }

        public async Task<bool> Update(EditedPriceStatus EditedPriceStatus)
        {
            if (await ValidateId(EditedPriceStatus))
            {
            }
            return EditedPriceStatus.IsValidated;
        }

        public async Task<bool> Delete(EditedPriceStatus EditedPriceStatus)
        {
            if (await ValidateId(EditedPriceStatus))
            {
            }
            return EditedPriceStatus.IsValidated;
        }

        public async Task<bool> BulkDelete(List<EditedPriceStatus> EditedPriceStatuses)
        {
            return true;
        }

        public async Task<bool> Import(List<EditedPriceStatus> EditedPriceStatuses)
        {
            return true;
        }
    }
}
