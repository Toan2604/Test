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

namespace DMS.Services.MBrand
{
    public interface IBrandService : IServiceScoped
    {
        Task<int> Count(BrandFilter BrandFilter);
        Task<List<Brand>> List(BrandFilter BrandFilter);
        Task<Brand> Get(long Id);
        Task<bool> BulkMerge(List<Brand> Brands);
    }

    public class BrandService : BaseService, IBrandService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IBrandValidator BrandValidator;
        private IRabbitManager RabbitManager;

        public BrandService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IBrandValidator BrandValidator,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.BrandValidator = BrandValidator;
            this.RabbitManager = RabbitManager;
        }
        public async Task<int> Count(BrandFilter BrandFilter)
        {
            try
            {
                int result = await UOW.BrandRepository.Count(BrandFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(BrandService));
            }
            return 0;
        }

        public async Task<List<Brand>> List(BrandFilter BrandFilter)
        {
            try
            {
                List<Brand> Brands = await UOW.BrandRepository.List(BrandFilter);
                return Brands;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(BrandService));
            }
            return null;
        }
        public async Task<Brand> Get(long Id)
        {
            Brand Brand = await UOW.BrandRepository.Get(Id);
            if (Brand == null)
                return null;
            return Brand;
        }

        public async Task<bool> BulkMerge(List<Brand> Brands)
        {
            await UOW.BrandRepository.BulkMerge(Brands);

            List<BrandHistory> BrandHistories = Brands.Select(x => new BrandHistory
            {
                BrandId = x.Id,
                AppUserId = x.AppUserId,
                StatusId = x.StatusId,
                Used = x.Used,
                CreatedAt = StaticParams.DateTimeNow,
            }).ToList();
            await UOW.BrandHistoryRepository.BulkMerge(BrandHistories);
            await SyncBrandHistory(BrandHistories);

            return true;
        }

        private async Task<bool> SyncBrandHistory(List<BrandHistory> BrandHistories)
        {
            RabbitManager.PublishList(BrandHistories, RoutingKeyEnum.BrandHistorySync.Code);
            return true;
        }
    }
}
