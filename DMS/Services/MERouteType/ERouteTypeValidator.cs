using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MERouteType
{
    public interface IERouteTypeValidator : IServiceScoped
    {
        Task<bool> Create(ERouteType ERouteType);
        Task<bool> Update(ERouteType ERouteType);
        Task<bool> Delete(ERouteType ERouteType);
        Task<bool> BulkDelete(List<ERouteType> ERouteTypes);
        Task<bool> Import(List<ERouteType> ERouteTypes);
    }

    public class ERouteTypeValidator : IERouteTypeValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public ERouteTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(ERouteType ERouteType)
        {
            ERouteTypeFilter ERouteTypeFilter = new ERouteTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = ERouteType.Id },
                Selects = ERouteTypeSelect.Id
            };

            int count = await UOW.ERouteTypeRepository.Count(ERouteTypeFilter);
            if (count == 0)
                ERouteType.AddError(nameof(ERouteTypeValidator), nameof(ERouteType.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(ERouteType ERouteType)
        {
            return ERouteType.IsValidated;
        }

        public async Task<bool> Update(ERouteType ERouteType)
        {
            if (await ValidateId(ERouteType))
            {
            }
            return ERouteType.IsValidated;
        }

        public async Task<bool> Delete(ERouteType ERouteType)
        {
            if (await ValidateId(ERouteType))
            {
            }
            return ERouteType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<ERouteType> ERouteTypes)
        {
            return true;
        }

        public async Task<bool> Import(List<ERouteType> ERouteTypes)
        {
            return true;
        }
    }
}
