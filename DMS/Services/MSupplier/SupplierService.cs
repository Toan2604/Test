using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MSupplier
{
    public interface ISupplierService : IServiceScoped
    {
        Task<int> Count(SupplierFilter SupplierFilter);
        Task<List<Supplier>> List(SupplierFilter SupplierFilter);
        Task<Supplier> Get(long Id);
        SupplierFilter ToFilter(SupplierFilter SupplierFilter);
    }

    public class SupplierService : BaseService, ISupplierService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private ISupplierValidator SupplierValidator;

        public SupplierService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            ISupplierValidator SupplierValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.SupplierValidator = SupplierValidator;
        }
        public async Task<int> Count(SupplierFilter SupplierFilter)
        {
            try
            {
                int result = await UOW.SupplierRepository.Count(SupplierFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SupplierService));
            }
            return 0;
        }

        public async Task<List<Supplier>> List(SupplierFilter SupplierFilter)
        {
            try
            {
                List<Supplier> Suppliers = await UOW.SupplierRepository.List(SupplierFilter);
                return Suppliers;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SupplierService));
            }
            return null;
        }
        public async Task<Supplier> Get(long Id)
        {
            Supplier Supplier = await UOW.SupplierRepository.Get(Id);
            if (Supplier == null)
                return null;
            return Supplier;
        }
        public SupplierFilter ToFilter(SupplierFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<SupplierFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                SupplierFilter subFilter = new SupplierFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                }
            }
            return filter;
        }
    }
}
