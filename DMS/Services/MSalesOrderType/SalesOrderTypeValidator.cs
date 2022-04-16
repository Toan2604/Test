using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MSalesOrderType
{
    public interface ISalesOrderTypeValidator : IServiceScoped
    {
        Task<bool> Create(SalesOrderType SalesOrderType);
        Task<bool> Update(SalesOrderType SalesOrderType);
        Task<bool> Delete(SalesOrderType SalesOrderType);
        Task<bool> BulkDelete(List<SalesOrderType> SalesOrderTypes);
        Task<bool> Import(List<SalesOrderType> SalesOrderTypes);
    }

    public class SalesOrderTypeValidator : ISalesOrderTypeValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public SalesOrderTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(SalesOrderType SalesOrderType)
        {
            SalesOrderTypeFilter SalesOrderTypeFilter = new SalesOrderTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = SalesOrderType.Id },
                Selects = SalesOrderTypeSelect.Id
            };

            int count = await UOW.SalesOrderTypeRepository.Count(SalesOrderTypeFilter);
            if (count == 0)
                SalesOrderType.AddError(nameof(SalesOrderTypeValidator), nameof(SalesOrderType.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(SalesOrderType SalesOrderType)
        {
            return SalesOrderType.IsValidated;
        }

        public async Task<bool> Update(SalesOrderType SalesOrderType)
        {
            if (await ValidateId(SalesOrderType))
            {
            }
            return SalesOrderType.IsValidated;
        }

        public async Task<bool> Delete(SalesOrderType SalesOrderType)
        {
            if (await ValidateId(SalesOrderType))
            {
            }
            return SalesOrderType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<SalesOrderType> SalesOrderTypes)
        {
            foreach (SalesOrderType SalesOrderType in SalesOrderTypes)
            {
                await Delete(SalesOrderType);
            }
            return SalesOrderTypes.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<SalesOrderType> SalesOrderTypes)
        {
            return true;
        }
    }
}
