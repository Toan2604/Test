using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers.Configuration;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MProductGrouping
{
    public interface IProductGroupingService : IServiceScoped
    {
        Task<int> Count(ProductGroupingFilter ProductGroupingFilter);
        Task<List<ProductGrouping>> List(ProductGroupingFilter ProductGroupingFilter);
        Task<ProductGrouping> Get(long Id);
        ProductGroupingFilter ToFilter(ProductGroupingFilter ProductGroupingFilter);
        Task<bool> BulkMerge(List<ProductGrouping> ProductGroupings);
    }

    public class ProductGroupingService : BaseService, IProductGroupingService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IProductGroupingValidator ProductGroupingValidator;
        private IRabbitManager RabbitManager;

        public ProductGroupingService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IProductGroupingValidator ProductGroupingValidator,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ProductGroupingValidator = ProductGroupingValidator;
            this.RabbitManager = RabbitManager;
        }
        public async Task<int> Count(ProductGroupingFilter ProductGroupingFilter)
        {
            try
            {
                int result = await UOW.ProductGroupingRepository.Count(ProductGroupingFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProductGroupingService));
            }
            return 0;
        }

        public async Task<List<ProductGrouping>> List(ProductGroupingFilter ProductGroupingFilter)
        {
            try
            {
                List<ProductGrouping> ProductGroupings = await UOW.ProductGroupingRepository.List(ProductGroupingFilter);
                return ProductGroupings;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProductGroupingService));
            }
            return null;
        }
        public async Task<ProductGrouping> Get(long Id)
        {
            ProductGrouping ProductGrouping = await UOW.ProductGroupingRepository.Get(Id);
            if (ProductGrouping == null)
                return null;
            return ProductGrouping;
        }

        public async Task<bool> BulkMerge(List<ProductGrouping> ProductGroupings)
        {
            try
            {
                await UOW.ProductGroupingRepository.BulkMerge(ProductGroupings);

                List<ProductGroupingHistory> ProductGroupingHistories = ProductGroupings.Select(x => new ProductGroupingHistory
                {
                    ProductGroupingId = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    AppUserId = x.AppUserId,
                    StatusId = x.StatusId,
                    CreatedAt = StaticParams.DateTimeNow,
                }).ToList();
                await UOW.ProductGroupingHistoryRepository.BulkMerge(ProductGroupingHistories);
                await SyncProductGroupingHistory(ProductGroupingHistories);

                return true;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProductGroupingService));
            }
            return false;
        }

        private async Task<bool> SyncProductGroupingHistory(List<ProductGroupingHistory> ProductGroupingHistories)
        {
            RabbitManager.PublishList(ProductGroupingHistories, RoutingKeyEnum.ProductGroupingHistorySync.Code);
            return true;
        }

        public ProductGroupingFilter ToFilter(ProductGroupingFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<ProductGroupingFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                ProductGroupingFilter subFilter = new ProductGroupingFilter();
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
