using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionDirectSalesOrder
{
    public interface IPromotionDirectSalesOrderValidator : IServiceScoped
    {
        Task<bool> Create(PromotionDirectSalesOrder PromotionDirectSalesOrder);
        Task<bool> Update(PromotionDirectSalesOrder PromotionDirectSalesOrder);
        Task<bool> Delete(PromotionDirectSalesOrder PromotionDirectSalesOrder);
        Task<bool> BulkDelete(List<PromotionDirectSalesOrder> PromotionDirectSalesOrders);
        Task<bool> Import(List<PromotionDirectSalesOrder> PromotionDirectSalesOrders);
    }

    public class PromotionDirectSalesOrderValidator : IPromotionDirectSalesOrderValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PromotionDirectSalesOrderValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PromotionDirectSalesOrder PromotionDirectSalesOrder)
        {
            PromotionDirectSalesOrderFilter PromotionDirectSalesOrderFilter = new PromotionDirectSalesOrderFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PromotionDirectSalesOrder.Id },
                Selects = PromotionDirectSalesOrderSelect.Id
            };

            int count = await UOW.PromotionDirectSalesOrderRepository.Count(PromotionDirectSalesOrderFilter);
            if (count == 0)
                PromotionDirectSalesOrder.AddError(nameof(PromotionDirectSalesOrderValidator), nameof(PromotionDirectSalesOrder.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(PromotionDirectSalesOrder PromotionDirectSalesOrder)
        {
            return PromotionDirectSalesOrder.IsValidated;
        }

        public async Task<bool> Update(PromotionDirectSalesOrder PromotionDirectSalesOrder)
        {
            if (await ValidateId(PromotionDirectSalesOrder))
            {
            }
            return PromotionDirectSalesOrder.IsValidated;
        }

        public async Task<bool> Delete(PromotionDirectSalesOrder PromotionDirectSalesOrder)
        {
            if (await ValidateId(PromotionDirectSalesOrder))
            {
            }
            return PromotionDirectSalesOrder.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PromotionDirectSalesOrder> PromotionDirectSalesOrders)
        {
            foreach (PromotionDirectSalesOrder PromotionDirectSalesOrder in PromotionDirectSalesOrders)
            {
                await Delete(PromotionDirectSalesOrder);
            }
            return PromotionDirectSalesOrders.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<PromotionDirectSalesOrder> PromotionDirectSalesOrders)
        {
            return true;
        }
    }
}
