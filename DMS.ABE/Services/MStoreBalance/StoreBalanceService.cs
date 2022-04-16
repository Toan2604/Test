using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using DMS.ABE.Services.MImage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.ABE.Services.MStoreBalance
{
    public interface IStoreBalanceService : IServiceScoped
    {
        Task<StoreBalance> Get(long Id);
        Task<List<StoreBalance>> List(StoreBalanceFilter StoreBalanceFilter);
    }

    public class StoreBalanceService : BaseService, IStoreBalanceService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IImageService ImageService;
        public StoreBalanceService(
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

        public async Task<StoreBalance> Get(long Id)
        {
            StoreBalance StoreBalance = await UOW.StoreBalanceRepository.Get(Id);
            if (StoreBalance == null)
                return null;
            return StoreBalance;
        }

        public async Task<List<StoreBalance>> List(StoreBalanceFilter StoreBalanceFilter)
        {
            List<StoreBalance> StoreBalances = await UOW.StoreBalanceRepository.List(StoreBalanceFilter);
            if (StoreBalances == null)
                return null;
            return StoreBalances;
        }
    }
}
