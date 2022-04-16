using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MEstimatedRevenue
{
    public interface IEstimatedRevenueValidator : IServiceScoped
    {
        Task<bool> Create(EstimatedRevenue EstimatedRevenue);
        Task<bool> Update(EstimatedRevenue EstimatedRevenue);
        Task<bool> Delete(EstimatedRevenue EstimatedRevenue);
        Task<bool> BulkDelete(List<EstimatedRevenue> EstimatedRevenues);
        Task<bool> Import(List<EstimatedRevenue> EstimatedRevenues);
    }

    public class EstimatedRevenueValidator : IEstimatedRevenueValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public EstimatedRevenueValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(EstimatedRevenue EstimatedRevenue)
        {
            EstimatedRevenueFilter EstimatedRevenueFilter = new EstimatedRevenueFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = EstimatedRevenue.Id },
                Selects = EstimatedRevenueSelect.Id
            };

            int count = await UOW.EstimatedRevenueRepository.Count(EstimatedRevenueFilter);
            if (count == 0)
                EstimatedRevenue.AddError(nameof(EstimatedRevenueValidator), nameof(EstimatedRevenue.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(EstimatedRevenue EstimatedRevenue)
        {
            return EstimatedRevenue.IsValidated;
        }

        public async Task<bool> Update(EstimatedRevenue EstimatedRevenue)
        {
            if (await ValidateId(EstimatedRevenue))
            {
            }
            return EstimatedRevenue.IsValidated;
        }

        public async Task<bool> Delete(EstimatedRevenue EstimatedRevenue)
        {
            if (await ValidateId(EstimatedRevenue))
            {
            }
            return EstimatedRevenue.IsValidated;
        }

        public async Task<bool> BulkDelete(List<EstimatedRevenue> EstimatedRevenues)
        {
            foreach (EstimatedRevenue EstimatedRevenue in EstimatedRevenues)
            {
                await Delete(EstimatedRevenue);
            }
            return EstimatedRevenues.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<EstimatedRevenue> EstimatedRevenues)
        {
            return true;
        }
    }
}
