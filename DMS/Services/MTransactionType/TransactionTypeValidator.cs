using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MTransactionType
{
    public interface ITransactionTypeValidator : IServiceScoped
    {
        Task<bool> Create(TransactionType TransactionType);
        Task<bool> Update(TransactionType TransactionType);
        Task<bool> Delete(TransactionType TransactionType);
        Task<bool> BulkDelete(List<TransactionType> TransactionTypes);
        Task<bool> Import(List<TransactionType> TransactionTypes);
    }

    public class TransactionTypeValidator : ITransactionTypeValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public TransactionTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(TransactionType TransactionType)
        {
            TransactionTypeFilter TransactionTypeFilter = new TransactionTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = TransactionType.Id },
                Selects = TransactionTypeSelect.Id
            };

            int count = await UOW.TransactionTypeRepository.Count(TransactionTypeFilter);
            if (count == 0)
                TransactionType.AddError(nameof(TransactionTypeValidator), nameof(TransactionType.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(TransactionType TransactionType)
        {
            return TransactionType.IsValidated;
        }

        public async Task<bool> Update(TransactionType TransactionType)
        {
            if (await ValidateId(TransactionType))
            {
            }
            return TransactionType.IsValidated;
        }

        public async Task<bool> Delete(TransactionType TransactionType)
        {
            if (await ValidateId(TransactionType))
            {
            }
            return TransactionType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<TransactionType> TransactionTypes)
        {
            foreach (TransactionType TransactionType in TransactionTypes)
            {
                await Delete(TransactionType);
            }
            return TransactionTypes.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<TransactionType> TransactionTypes)
        {
            return true;
        }
    }
}
