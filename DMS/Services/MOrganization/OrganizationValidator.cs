using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MOrganization
{
    public interface IOrganizationValidator : IServiceScoped
    {
        Task<bool> Create(Organization Organization);
        Task<bool> Update(Organization Organization);
        Task<bool> Delete(Organization Organization);
        Task<bool> BulkDelete(List<Organization> Organizations);
        Task<bool> Import(List<Organization> Organizations);
    }

    public class OrganizationValidator : IOrganizationValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public OrganizationValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Organization Organization)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Organization.Id },
                Selects = OrganizationSelect.Id
            };

            int count = await UOW.OrganizationRepository.Count(OrganizationFilter);
            if (count == 0)
                Organization.AddError(nameof(OrganizationValidator), nameof(Organization.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(Organization Organization)
        {
            return Organization.IsValidated;
        }

        public async Task<bool> Update(Organization Organization)
        {
            if (await ValidateId(Organization))
            {
            }
            return Organization.IsValidated;
        }

        public async Task<bool> Delete(Organization Organization)
        {
            if (await ValidateId(Organization))
            {
            }
            return Organization.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Organization> Organizations)
        {
            return true;
        }

        public async Task<bool> Import(List<Organization> Organizations)
        {
            return true;
        }
    }
}
