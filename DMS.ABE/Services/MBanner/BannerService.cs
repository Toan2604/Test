using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using DMS.ABE.Services.MImage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.ABE.Services.MBanner
{
    public interface IBannerService : IServiceScoped
    {
        Task<int> Count(BannerFilter BannerFilter);
        Task<List<Banner>> List(BannerFilter BannerFilter);
        Task<Banner> Get(long Id);
    }

    public class BannerService : BaseService, IBannerService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IImageService ImageService;
        public BannerService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IImageService ImageService
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ImageService = ImageService;
        }
        public async Task<int> Count(BannerFilter BannerFilter)
        {
            try
            {
                Store Store = await GetStore();
                if (Store != null)
                {
                    BannerFilter.OrganizationId = new IdFilter { Equal = Store.OrganizationId };
                } // filter banner theo org cua hang
                int result = await UOW.BannerRepository.Count(BannerFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(BannerService));
            }
            return 0;
        }

        public async Task<List<Banner>> List(BannerFilter BannerFilter)
        {
            try
            {
                Store Store = await GetStore();
                if (Store != null)
                {
                    BannerFilter.OrganizationId = new IdFilter { Equal = Store.OrganizationId };
                } // filter banner theo org cua hang
                List<Banner> Banners = await UOW.BannerRepository.List(BannerFilter);
                return Banners;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(BannerService));
            }
            return null;
        }
        public async Task<Banner> Get(long Id)
        {
            Banner Banner = await UOW.BannerRepository.Get(Id);
            if (Banner == null)
                return null;
            return Banner;
        }

        private async Task<Store> GetStore()
        {
            var StoreUserId = CurrentContext.StoreUserId;
            StoreUser StoreUser = await UOW.StoreUserRepository.Get(StoreUserId);
            if (StoreUser == null)
            {
                return null;
            } // check storeUser co ton tai khong
            Store Store = await UOW.StoreRepository.Get(StoreUser.StoreId);
            if (Store == null)
            {
                return null;
            } // check store tuong ung vs storeUser co ton tai khong
            return Store;
        }
    }
}
