using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Services.MImage;
using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Handlers.Configuration;

namespace DMS.Services.MStoreChecking
{
    public interface IStoreCheckingService : IServiceScoped
    {
        Task<int> Count(StoreCheckingFilter StoreCheckingFilter);
        Task<List<StoreChecking>> List(StoreCheckingFilter StoreCheckingFilter);

        Task<StoreChecking> Get(long Id);
        Task<StoreChecking> CheckIn(StoreChecking StoreChecking);
        Task<StoreChecking> Update(StoreChecking StoreChecking);
        Task<StoreChecking> UpdateStoreCheckingImage(StoreChecking StoreChecking);
        Task<StoreChecking> CheckOut(StoreChecking StoreChecking);
        Task<Image> SaveImage(Image Image);
        Task<long> CountStore(StoreFilter StoreFilter, IdFilter ERouteId);
        Task<List<Store>> ListStore(StoreFilter StoreFilter, IdFilter ERouteId);
        Task<long> CountStorePlanned(StoreFilter StoreFilter, IdFilter ERouteId);
        Task<List<Store>> ListStorePlanned(StoreFilter StoreFilter, IdFilter ERouteId);
        Task<long> CountStoreUnPlanned(StoreFilter StoreFilter, IdFilter ERouteId, IdFilter Time);
        Task<List<Store>> ListStoreUnPlanned(StoreFilter StoreFilter, IdFilter ERouteId, IdFilter Time);
        Task<long> CountStoreInScope(StoreFilter StoreFilter, IdFilter ERouteId);
        Task<List<Store>> ListStoreInScope(StoreFilter StoreFilter, IdFilter ERouteId);
        StoreCheckingFilter ToFilter(StoreCheckingFilter StoreCheckingFilter);
    }

    public class StoreCheckingService : BaseService, IStoreCheckingService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IRabbitManager RabbitManager;
        private IImageService ImageService;

        private IStoreCheckingValidator StoreCheckingValidator;

        public StoreCheckingService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IRabbitManager RabbitManager,
            IImageService ImageService,
            IStoreCheckingValidator StoreCheckingValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.ImageService = ImageService;
            this.StoreCheckingValidator = StoreCheckingValidator;
        }
        public async Task<int> Count(StoreCheckingFilter StoreCheckingFilter)
        {
            try
            {
                int result = await UOW.StoreCheckingRepository.Count(StoreCheckingFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreCheckingService));
            }
            return 0;
        }

        public async Task<List<StoreChecking>> List(StoreCheckingFilter StoreCheckingFilter)
        {
            try
            {
                List<StoreChecking> StoreCheckings = await UOW.StoreCheckingRepository.List(StoreCheckingFilter);
                return StoreCheckings;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreCheckingService));
            }
            return null;
        }
        public async Task<StoreChecking> Get(long Id)
        {
            StoreChecking StoreChecking = await UOW.StoreCheckingRepository.Get(Id);
            if (StoreChecking == null)
                return null;
            return StoreChecking;
        }

        public async Task<StoreChecking> CheckIn(StoreChecking StoreChecking)
        {
            if (!await StoreCheckingValidator.CheckIn(StoreChecking))
                return StoreChecking;

            try
            {
                var currentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                StoreChecking.CheckInAt = StaticParams.DateTimeNow;
                StoreChecking.SaleEmployeeId = CurrentContext.UserId;
                StoreChecking.OrganizationId = currentUser.OrganizationId;

                Dictionary<long, long> StorePlannedIds = await ListOnlineStoreIds(null);
                if (StorePlannedIds.Any(x => x.Key == StoreChecking.StoreId))
                {
                    StoreChecking.Planned = true;
                }
                else
                {
                    StoreChecking.Planned = false;
                }


                await UOW.StoreCheckingRepository.Create(StoreChecking);

                Sync(StoreChecking);
                Logging.CreateAuditLog(StoreChecking, new { }, nameof(StoreCheckingService));
                return await UOW.StoreCheckingRepository.Get(StoreChecking.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreCheckingService));
            }
            return null;
        }

        public async Task<StoreChecking> Update(StoreChecking StoreChecking)
        {
            if (!await StoreCheckingValidator.Update(StoreChecking))
                return StoreChecking;
            try
            {
                var oldData = await UOW.StoreCheckingRepository.Get(StoreChecking.Id);
                StoreChecking.CheckOutAt = oldData.CheckOutAt;
                StoreChecking.Planned = oldData.Planned;
                StoreChecking.ImageCounter = StoreChecking.StoreCheckingImageMappings?.Count() ?? 0;

                await UOW.StoreCheckingRepository.Update(StoreChecking);


                StoreChecking = await UOW.StoreCheckingRepository.Get(StoreChecking.Id);
                Sync(StoreChecking);
                Logging.CreateAuditLog(StoreChecking, oldData, nameof(StoreCheckingService));
                return StoreChecking;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreCheckingService));
            }
            return null;
        }

        public async Task<StoreChecking> UpdateStoreCheckingImage(StoreChecking StoreChecking)
        {
            try
            {
                var oldData = await UOW.StoreCheckingRepository.Get(StoreChecking.Id);
                oldData.StoreCheckingImageMappings = StoreChecking.StoreCheckingImageMappings;
                oldData.SaleEmployeeId = CurrentContext.UserId;
                oldData.ImageCounter = StoreChecking.StoreCheckingImageMappings?.Count() ?? 0; 
                await UOW.StoreCheckingRepository.Update(oldData);
                StoreChecking = await UOW.StoreCheckingRepository.Get(oldData.Id);
                Sync(StoreChecking);
                Logging.CreateAuditLog(StoreChecking, oldData, nameof(StoreCheckingService));
                return StoreChecking;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreCheckingService));
            }
            return null;
        }

        public async Task<StoreChecking> CheckOut(StoreChecking StoreChecking)
        {
            if (!await StoreCheckingValidator.CheckOut(StoreChecking))
                return StoreChecking;
            try
            {
                var oldData = await UOW.StoreCheckingRepository.Get(StoreChecking.Id);
                StoreChecking.CheckInAt = oldData.CheckInAt;
                if (oldData.CheckOutAt.HasValue == false)
                {
                    StoreChecking.CheckOutAt = StaticParams.DateTimeNow;
                }
                else
                {
                    StoreChecking.CheckOutAt = oldData.CheckOutAt;
                }
                StoreChecking.ImageCounter = StoreChecking.StoreCheckingImageMappings?.Count() ?? 0;
                StoreChecking.CheckInDistance = oldData.CheckInDistance;
                await UOW.StoreCheckingRepository.Update(StoreChecking);


                StoreChecking = await UOW.StoreCheckingRepository.Get(StoreChecking.Id);
                Sync(StoreChecking);

                Logging.CreateAuditLog(StoreChecking, oldData, nameof(StoreCheckingService));
                return StoreChecking;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreCheckingService));
            }
            return null;
        }

        /// <summary>
        /// Danh sách store chung
        /// </summary>
        /// <param name="StoreFilter"></param>
        /// <param name="ERouteId"></param>
        /// <returns></returns>
        public async Task<long> CountStore(StoreFilter StoreFilter, IdFilter ERouteId)
        {
            var AppUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
            StoreFilter.OrganizationId = new IdFilter { Equal = AppUser.OrganizationId };
            StoreFilter.TimeZone = CurrentContext.TimeZone;
            int count = await UOW.StoreRepository.Count(StoreFilter);
            return count;
        }

        /// <summary>
        /// Danh sách store chung
        /// </summary>
        /// <param name="StoreFilter"></param>
        /// <param name="ERouteId"></param>
        /// <returns></returns>
        public async Task<List<Store>> ListStore(StoreFilter StoreFilter, IdFilter ERouteId)
        {
            List<Store> Stores;
            // Lấy danh sách tất cả các cửa hàng ra
            // Tính khoảng cách
            // sắp xếp theo khoảng cách
            int skip = StoreFilter.Skip;
            int take = StoreFilter.Take;
            StoreFilter.Skip = 0;
            StoreFilter.Take = int.MaxValue;
            StoreFilter.Selects = StoreSelect.Id;

            //StoreFilter.AppUserId = new IdFilter { Equal = CurrentContext.UserId };
            var AppUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
            StoreFilter.OrganizationId = new IdFilter { Equal = AppUser.OrganizationId };
            StoreFilter.TimeZone = CurrentContext.TimeZone;
            if (CurrentContext.Latitude.HasValue && CurrentContext.Longitude.HasValue)
            {
                StoreFilter.Selects = StoreSelect.Id | StoreSelect.Longitude | StoreSelect.Latitude;
            }
            StoreFilter.NoCache = true;
            Stores = await UOW.StoreRepository.List(StoreFilter);
            if (CurrentContext.Latitude.HasValue && CurrentContext.Longitude.HasValue)
            {
                StoreFilter.Selects = StoreSelect.Id | StoreSelect.Longitude | StoreSelect.Latitude;
                Stores = await ListRecentStore(Stores, CurrentContext.Latitude.Value, CurrentContext.Longitude.Value);
            }
            Stores = await CheckStoreChecking(Stores);
            Stores = Stores.OrderBy(x => x.HasChecking).ThenBy(x => x.Distance).Skip(skip).Take(take).ToList();
            List<long> StoreIds = Stores.Select(x => x.Id).ToList();

            StoreFilter = new StoreFilter();
            StoreFilter.Id = new IdFilter { In = StoreIds };
            StoreFilter.Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.Name |
                StoreSelect.Address | StoreSelect.Telephone | StoreSelect.Latitude | StoreSelect.DeliveryAddress |
                StoreSelect.Longitude | StoreSelect.HasChecking | StoreSelect.OwnerPhone | StoreSelect.StoreType |
                StoreSelect.StoreStatus | StoreSelect.Province | StoreSelect.District | StoreSelect.Ward;

            Stores = await UOW.StoreRepository.List(StoreFilter);
            if (CurrentContext.Latitude.HasValue && CurrentContext.Longitude.HasValue)
            {
                Stores = await ListRecentStore(Stores, CurrentContext.Latitude.Value, CurrentContext.Longitude.Value);
            }
            Stores = await CheckStoreChecking(Stores);
            Stores = Stores.OrderBy(s => s.HasChecking).ThenBy(s => s.Distance).ToList();
            Stores = await CheckOrder(Stores);
            return Stores;
        }

        /// <summary>
        /// Danh sách store theo tuyến trong ngày
        /// </summary>
        /// <param name="StoreFilter"></param>
        /// <param name="ERouteId"></param>
        /// <returns></returns>
        public async Task<long> CountStorePlanned(StoreFilter StoreFilter, IdFilter ERouteId)
        {
            try
            {
                Dictionary<long, long> StoreIds = await ListOnlineStoreIds(ERouteId);
                StoreFilter.Id = new IdFilter { In = StoreIds.Select(x => x.Key).ToList() };
                StoreFilter.AppUserId = new IdFilter { Equal = CurrentContext.UserId };
                StoreFilter.TimeZone = CurrentContext.TimeZone;
                //if (CurrentContext.Latitude.HasValue && CurrentContext.Longitude.HasValue)
                //{
                //    StoreFilter.Latitude = new DecimalFilter { GreaterEqual = CurrentContext.Latitude - StaticParams.StoreScope, LessEqual = CurrentContext.Latitude + StaticParams.StoreScope };
                //    StoreFilter.Longitude = new DecimalFilter { GreaterEqual = CurrentContext.Longitude - StaticParams.StoreScope, LessEqual = CurrentContext.Longitude + StaticParams.StoreScope };
                //}
                int count = await UOW.StoreRepository.Count(StoreFilter);
                return count;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreCheckingService));
            }
            return 0;
        }

        /// <summary>
        /// Danh sách store theo tuyến trong ngày
        /// </summary>
        /// <param name="StoreFilter"></param>
        /// <param name="ERouteId"></param>
        /// <returns></returns>
        public async Task<List<Store>> ListStorePlanned(StoreFilter StoreFilter, IdFilter ERouteId)
        {
            try
            {
                List<Store> Stores;
                int skip = StoreFilter.Skip;
                int take = StoreFilter.Take;
                // Lấy danh sách tất cả các cửa hàng trong tuyến ra
                // Tính khoảng cách
                // sắp xếp theo thứ tự ưu tiên trước rồi đến khoảng cách
                Dictionary<long, long> StoreIds = await ListOnlineStoreIds(ERouteId);
                StoreFilter.Id = new IdFilter { In = StoreIds.Select(x => x.Key).ToList() };
                StoreFilter.AppUserId = new IdFilter { Equal = CurrentContext.UserId };
                StoreFilter.Skip = 0;
                StoreFilter.Take = int.MaxValue;
                StoreFilter.Selects = StoreSelect.Id;
                StoreFilter.TimeZone = CurrentContext.TimeZone;
                StoreFilter.NoCache = true;
                if (CurrentContext.Latitude.HasValue && CurrentContext.Longitude.HasValue)
                {
                    //StoreFilter.Latitude = new DecimalFilter { GreaterEqual = CurrentContext.Latitude - StaticParams.StoreScope, LessEqual = CurrentContext.Latitude + StaticParams.StoreScope };
                    //StoreFilter.Longitude = new DecimalFilter { GreaterEqual = CurrentContext.Longitude - StaticParams.StoreScope, LessEqual = CurrentContext.Longitude + StaticParams.StoreScope };
                    StoreFilter.Selects = StoreSelect.Id | StoreSelect.Longitude | StoreSelect.Latitude;
                }
                Stores = await UOW.StoreRepository.List(StoreFilter);
                if (CurrentContext.Latitude.HasValue && CurrentContext.Longitude.HasValue)
                {
                    Stores = await ListRecentStore(Stores, CurrentContext.Latitude.Value, CurrentContext.Longitude.Value);
                }
                Stores = await CheckStoreChecking(Stores);

                Stores = Stores.OrderBy(s => s.HasChecking).ThenBy(s => s.Distance).Skip(skip).Take(take).ToList();
                List<long> Ids = Stores.Select(x => x.Id).ToList();

                StoreFilter = new StoreFilter();
                StoreFilter.Id = new IdFilter { In = Ids };

                StoreFilter.Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.Name |
                StoreSelect.Address | StoreSelect.Telephone | StoreSelect.Latitude | StoreSelect.DeliveryAddress |
                StoreSelect.Longitude | StoreSelect.HasChecking | StoreSelect.OwnerPhone | StoreSelect.StoreType | StoreSelect.StoreStatus | StoreSelect.Province | StoreSelect.District | StoreSelect.Ward;

                Stores = await UOW.StoreRepository.List(StoreFilter);
                if (CurrentContext.Latitude.HasValue && CurrentContext.Longitude.HasValue)
                {
                    Stores = await ListRecentStore(Stores, CurrentContext.Latitude.Value, CurrentContext.Longitude.Value);
                }
                Stores = await CheckStoreChecking(Stores);
                Stores = Stores.OrderBy(s => s.HasChecking).ThenBy(s => s.Distance).ToList();
                Stores = await CheckOrder(Stores);
                return Stores;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreCheckingService));
            }
            return null;
        }

        /// <summary>
        /// Danh sách store theo tuyến nhưng không trong ngày
        /// </summary>
        /// <param name="StoreFilter"></param>
        /// <param name="ERouteId"></param>
        /// <returns></returns>
        public async Task<long> CountStoreUnPlanned(StoreFilter StoreFilter, IdFilter ERouteId, IdFilter Time)
        {
            try
            {
                var AppUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                Dictionary<long, long> StoreIds = new Dictionary<long, long>();
                if (Time != null && Time.Equal.HasValue)
                {
                    StoreIds = await ListOnlineStoreIds(ERouteId, Time);
                }
                else
                {
                    StoreIds = await ListOfflineStoreIds(ERouteId);
                }
                StoreFilter.Id.In = StoreIds.Select(x => x.Key).ToList();
                StoreFilter.AppUserId = new IdFilter { Equal = CurrentContext.UserId };
                StoreFilter.TimeZone = CurrentContext.TimeZone;
                //if (CurrentContext.Latitude.HasValue && CurrentContext.Longitude.HasValue)
                //{
                //    StoreFilter.Latitude = new DecimalFilter { GreaterEqual = CurrentContext.Latitude - StaticParams.StoreScope, LessEqual = CurrentContext.Latitude + StaticParams.StoreScope };
                //    StoreFilter.Longitude = new DecimalFilter { GreaterEqual = CurrentContext.Longitude - StaticParams.StoreScope, LessEqual = CurrentContext.Longitude + StaticParams.StoreScope };
                //}
                int count = await UOW.StoreRepository.Count(StoreFilter);
                return count;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreCheckingService));
            }
            return 0;
        }

        /// <summary>
        /// Danh sách store theo tuyến nhưng không trong ngày
        /// </summary>
        /// <param name="StoreFilter"></param>
        /// <param name="ERouteId"></param>
        /// <returns></returns>
        public async Task<List<Store>> ListStoreUnPlanned(StoreFilter StoreFilter, IdFilter ERouteId, IdFilter Time)
        {
            try
            {
                List<Store> Stores;

                // Lấy danh sách tất cả các cửa hàng ngoại tuyến ra
                // Tính khoảng cách
                // sắp xếp theo thứ tự ưu tiên trước rồi đến khoảng cách
                int skip = StoreFilter.Skip;
                int take = StoreFilter.Take;
                Dictionary<long, long> StoreIds = new Dictionary<long, long>();
                if (Time != null && Time.Equal.HasValue) //filter các cửa  hàng có tuyến trong ngày tiếp theo
                {
                    StoreIds = await ListOnlineStoreIds(ERouteId, Time);
                }
                else
                {
                    StoreIds = await ListOfflineStoreIds(ERouteId);
                }
                StoreFilter.Id.In = StoreIds.Select(x => x.Key).ToList();
                StoreFilter.AppUserId = new IdFilter { Equal = CurrentContext.UserId };
                StoreFilter.Skip = 0;
                StoreFilter.Take = int.MaxValue;
                StoreFilter.Selects = StoreSelect.Id;
                StoreFilter.TimeZone = CurrentContext.TimeZone;
                StoreFilter.NoCache = true;
                if (CurrentContext.Latitude.HasValue && CurrentContext.Longitude.HasValue)
                {
                    //StoreFilter.Latitude = new DecimalFilter { GreaterEqual = CurrentContext.Latitude - StaticParams.StoreScope, LessEqual = CurrentContext.Latitude + StaticParams.StoreScope };
                    //StoreFilter.Longitude = new DecimalFilter { GreaterEqual = CurrentContext.Longitude - StaticParams.StoreScope, LessEqual = CurrentContext.Longitude + StaticParams.StoreScope };
                    StoreFilter.Selects = StoreSelect.Id | StoreSelect.Longitude | StoreSelect.Latitude;
                }
                Stores = await UOW.StoreRepository.List(StoreFilter);
                if (CurrentContext.Latitude.HasValue && CurrentContext.Longitude.HasValue)
                {
                    Stores = await ListRecentStore(Stores, CurrentContext.Latitude.Value, CurrentContext.Longitude.Value);
                }
                Stores = await CheckStoreChecking(Stores);

                Stores = Stores.OrderBy(s => s.HasChecking).ThenBy(s => s.Distance).Skip(skip).Take(take).ToList();
                List<long> Ids = Stores.Select(x => x.Id).ToList();

                StoreFilter = new StoreFilter();
                StoreFilter.Id = new IdFilter { In = Ids };

                StoreFilter.Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.Name |
                StoreSelect.Address | StoreSelect.Telephone | StoreSelect.Latitude | StoreSelect.DeliveryAddress |
                StoreSelect.Longitude | StoreSelect.HasChecking | StoreSelect.OwnerPhone | StoreSelect.StoreType | StoreSelect.StoreStatus | StoreSelect.Province | StoreSelect.District | StoreSelect.Ward;

                Stores = await UOW.StoreRepository.List(StoreFilter);
                if (CurrentContext.Latitude.HasValue && CurrentContext.Longitude.HasValue)
                {
                    Stores = await ListRecentStore(Stores, CurrentContext.Latitude.Value, CurrentContext.Longitude.Value);
                }
                Stores = await CheckStoreChecking(Stores);
                Stores = Stores.OrderBy(s => s.HasChecking).ThenBy(s => s.Distance).ToList();
                Stores = await CheckOrder(Stores);
                return Stores;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreCheckingService));
            }
            return null;
        }

        /// <summary>
        /// Danh sách store theo phạm vi
        /// </summary>
        /// <param name="StoreFilter"></param>
        /// <param name="ERouteId"></param>
        /// <returns></returns>
        public async Task<long> CountStoreInScope(StoreFilter StoreFilter, IdFilter ERouteId)
        {
            try
            {
                StoreFilter.AppUserId = new IdFilter { Equal = CurrentContext.UserId };
                StoreFilter.TimeZone = CurrentContext.TimeZone;
                //if (CurrentContext.Latitude.HasValue && CurrentContext.Longitude.HasValue)
                //{
                //    StoreFilter.Latitude = new DecimalFilter { GreaterEqual = CurrentContext.Latitude - StaticParams.StoreScope, LessEqual = CurrentContext.Latitude + StaticParams.StoreScope };
                //    StoreFilter.Longitude = new DecimalFilter { GreaterEqual = CurrentContext.Longitude - StaticParams.StoreScope, LessEqual = CurrentContext.Longitude + StaticParams.StoreScope };
                //}
                var count = await UOW.StoreRepository.Count(StoreFilter);
                return count;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreCheckingService));
            }
            return 0;
        }

        /// <summary>
        /// Danh sách store theo phạm vi
        /// </summary>
        /// <param name="StoreFilter"></param>
        /// <param name="ERouteId"></param>
        /// <returns></returns>
        public async Task<List<Store>> ListStoreInScope(StoreFilter StoreFilter, IdFilter ERouteId)
        {
            try
            {
                AppUser AppUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                List<Store> Stores;
                int skip = StoreFilter.Skip;
                int take = StoreFilter.Take;
                StoreFilter.Skip = 0;
                StoreFilter.Take = int.MaxValue;
                StoreFilter.Selects = StoreSelect.Id;
                StoreFilter.AppUserId = new IdFilter { Equal = CurrentContext.UserId };
                StoreFilter.TimeZone = CurrentContext.TimeZone;
                StoreFilter.NoCache = true;
                if (CurrentContext.Latitude.HasValue && CurrentContext.Longitude.HasValue)
                {
                    //StoreFilter.Latitude = new DecimalFilter { GreaterEqual = CurrentContext.Latitude - StaticParams.StoreScope, LessEqual = CurrentContext.Latitude + StaticParams.StoreScope };
                    //StoreFilter.Longitude = new DecimalFilter { GreaterEqual = CurrentContext.Longitude - StaticParams.StoreScope, LessEqual = CurrentContext.Longitude + StaticParams.StoreScope };
                    StoreFilter.Selects = StoreSelect.Id | StoreSelect.Longitude | StoreSelect.Latitude;
                }
                Stores = await UOW.StoreRepository.List(StoreFilter);
                if (CurrentContext.Latitude.HasValue && CurrentContext.Longitude.HasValue)
                {
                    Stores = await ListRecentStore(Stores, CurrentContext.Latitude.Value, CurrentContext.Longitude.Value);
                }
                Stores = await CheckStoreChecking(Stores);
                Stores = Stores.OrderBy(s => s.HasChecking).ThenBy(s => s.Distance).Skip(skip).Take(take).ToList();
                List<long> StoreIds = Stores.Select(x => x.Id).ToList();

                StoreFilter = new StoreFilter();
                StoreFilter.Id = new IdFilter { In = StoreIds };
                StoreFilter.Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.Name |
                    StoreSelect.Address | StoreSelect.Telephone | StoreSelect.Latitude | StoreSelect.DeliveryAddress |
                    StoreSelect.Longitude | StoreSelect.HasChecking | StoreSelect.OwnerPhone | StoreSelect.StoreType | StoreSelect.CodeDraft |
                    StoreSelect.StoreStatus | StoreSelect.Province | StoreSelect.District | StoreSelect.Ward | StoreSelect.ParentStore;

                Stores = await UOW.StoreRepository.List(StoreFilter);
                if (CurrentContext.Latitude.HasValue && CurrentContext.Longitude.HasValue)
                {
                    Stores = await ListRecentStore(Stores, CurrentContext.Latitude.Value, CurrentContext.Longitude.Value);
                }
                Stores = await CheckStoreChecking(Stores);
                Stores = Stores.OrderBy(s => s.HasChecking).ThenBy(s => s.Distance).ToList();

                Stores = await CheckOrder(Stores);
                return Stores;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreCheckingService));
            }
            return null;
        }

        private async Task<List<Store>> CheckStoreChecking(List<Store> Stores)
        {
            List<long> StoreIds = Stores.Select(x => x.Id).ToList();
            DateTime StartToday = StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone);
            DateTime EndToday = StartToday.AddDays(1);
            StoreCheckingFilter StoreCheckingFilter = new StoreCheckingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreCheckingSelect.ALL,
                StoreId = new IdFilter { In = StoreIds },
                SaleEmployeeId = new IdFilter { Equal = CurrentContext.UserId },
                CheckOutAt = new DateFilter { GreaterEqual = StartToday, Less = EndToday }
            };
            List<StoreChecking> StoreCheckings = await UOW.StoreCheckingRepository.List(StoreCheckingFilter);
            foreach(Store Store in Stores)
            {
                var count = StoreCheckings.Where(x => x.StoreId == Store.Id).Count();
                Store.HasChecking = count != 0 ? true : false;
            }
            return Stores;
        }

        private async Task<List<Store>> CheckOrder(List<Store> Stores)
        {
            List<long> StoreIds = Stores.Select(x => x.Id).ToList();
            DateTime StartToday = StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone);
            DateTime EndToday = StartToday.AddDays(1);

            IndirectSalesOrderFilter IndirectSalesOrderFilter = new IndirectSalesOrderFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = IndirectSalesOrderSelect.Id | IndirectSalesOrderSelect.BuyerStore,
                BuyerStoreId = new IdFilter { In = StoreIds },
                AppUserId = new IdFilter { Equal = CurrentContext.UserId },
                OrderDate = new DateFilter { GreaterEqual = StartToday, Less = EndToday }
            };
            List<IndirectSalesOrder> IndirectSalesOrders = await UOW.IndirectSalesOrderRepository.List(IndirectSalesOrderFilter);

            DirectSalesOrderFilter DirectSalesOrderFilter = new DirectSalesOrderFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = DirectSalesOrderSelect.Id | DirectSalesOrderSelect.BuyerStore,
                BuyerStoreId = new IdFilter { In = StoreIds },
                AppUserId = new IdFilter { Equal = CurrentContext.UserId },
                OrderDate = new DateFilter { GreaterEqual = StartToday, Less = EndToday }
            };
            List<DirectSalesOrder> DirectSalesOrders = await UOW.DirectSalesOrderRepository.List(DirectSalesOrderFilter);
            foreach(Store Store in Stores)
            {
                var count = IndirectSalesOrders.Where(x => x.BuyerStoreId == Store.Id).Count()
                + DirectSalesOrders.Where(x => x.BuyerStoreId == Store.Id).Count();
                Store.HasOrder = count > 0;
            }
            return Stores;
        }

        public StoreCheckingFilter ToFilter(StoreCheckingFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<StoreCheckingFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                StoreCheckingFilter subFilter = new StoreCheckingFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                }
            }
            return filter;
        }

        public async Task<Image> SaveImage(Image Image)
        {
            FileInfo fileInfo = new FileInfo(Image.Name);
            string path = $"/store-checking/images/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}{fileInfo.Extension}";
            Image = await ImageService.Create(Image, path);
            return Image;
        }

        // Lấy danh sách tất cả các đại lý theo kế hoạch
        private async Task<Dictionary<long, long>> ListOnlineStoreIds(IdFilter ERouteId, IdFilter Time = null)
        {
            DateTime Now = StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone);
            if (Time != null && Time.Equal.HasValue)
            {
                Now = Now.AddDays(Time.Equal.Value);
            }
            List<ERoute> ERoutes = await UOW.ERouteRepository.List(new ERouteFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                StartDate = new DateFilter { LessEqual = Now },
                EndDate = new DateFilter { GreaterEqual = Now },
                Id = ERouteId,
                AppUserId = new IdFilter { Equal = CurrentContext.UserId },
                StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id },
                RequestStateId = new IdFilter { Equal = RequestStateEnum.APPROVED.Id },
                Selects = ERouteSelect.Id | ERouteSelect.RealStartDate
            });
            List<long> ERouteIds = ERoutes.Select(x => x.Id).ToList();
            List<ERouteContent> ERouteContents = await UOW.ERouteContentRepository.List(new ERouteContentFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                ERouteId = new IdFilter { In = ERouteIds },
                Selects = ERouteContentSelect.Id | ERouteContentSelect.Store | ERouteContentSelect.ERoute,
                HasCache = true
            });

            Dictionary<long, long> StoreIds = new Dictionary<long, long>();
            foreach (var ERouteContent in ERouteContents)
            {
                var ERoute = ERoutes.Where(x => x.Id == ERouteContent.ERouteId).FirstOrDefault();
                var index = (Now - ERoute.RealStartDate).Days % 28;
                bool Planned = ERouteContent.ERouteContentDays.Where(x => x.OrderDay == index).Select(x => x.Planned).FirstOrDefault();
                if (Planned == true)
                {
                    long StoreId = StoreIds.Where(x => x.Key == ERouteContent.StoreId)
                        .Select(x => x.Key)
                        .FirstOrDefault();
                    if (StoreId == 0)
                    {
                        long value = ERouteContent.OrderNumber ?? int.MaxValue;
                        StoreIds.Add(ERouteContent.StoreId, value);
                    }
                    else
                    {
                        long value = ERouteContent.OrderNumber ?? int.MaxValue;
                        long oldValue = StoreIds[StoreId];
                        if (oldValue > value)
                            StoreIds[StoreId] = value;
                    }
                }
            }

            return StoreIds;
        }

        private async Task<Dictionary<long, long>> ListOfflineStoreIds(IdFilter ERouteId)
        {
            DateTime Now = StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone);
            List<ERoute> ERoutes = await UOW.ERouteRepository.List(new ERouteFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                StartDate = new DateFilter { LessEqual = Now },
                EndDate = new DateFilter { GreaterEqual = Now },
                Id = ERouteId,
                AppUserId = new IdFilter { Equal = CurrentContext.UserId },
                StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id },
                RequestStateId = new IdFilter { Equal = RequestStateEnum.APPROVED.Id },
                Selects = ERouteSelect.Id | ERouteSelect.RealStartDate
            });
            List<long> ERouteIds = ERoutes.Select(x => x.Id).ToList();
            List<ERouteContent> ERouteContents = await UOW.ERouteContentRepository.List(new ERouteContentFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                ERouteId = new IdFilter { In = ERouteIds },
                Selects = ERouteContentSelect.Id | ERouteContentSelect.Store | ERouteContentSelect.ERoute,
                HasCache = true
            });

            Dictionary<long, long> StoreIds = new Dictionary<long, long>();
            foreach (var ERouteContent in ERouteContents)
            {
                var ERoute = ERoutes.Where(x => x.Id == ERouteContent.ERouteId).FirstOrDefault();
                var index = (Now - ERoute.RealStartDate).Days % 28;
                bool Planned = ERouteContent.ERouteContentDays.Where(x => x.OrderDay == index).Select(x => x.Planned).FirstOrDefault();
                if (Planned == false)
                {
                    long StoreId = StoreIds.Where(x => x.Key == ERouteContent.StoreId)
                        .Select(x => x.Key)
                        .FirstOrDefault();
                    if (StoreId == 0)
                    {
                        long value = ERouteContent.OrderNumber ?? int.MaxValue;
                        StoreIds.Add(ERouteContent.StoreId, value);
                    }
                    else
                    {
                        long value = ERouteContent.OrderNumber ?? int.MaxValue;
                        long oldValue = StoreIds[StoreId];
                        if (oldValue > value)
                            StoreIds[StoreId] = value;
                    }
                }
            }

            return StoreIds;
        }

        private async Task<List<Store>> ListRecentStore(List<Store> Stores, decimal CurrentLatitude, decimal CurrentLongitude)
        {
            GeoCoordinate sCoord = new GeoCoordinate((double)CurrentLatitude, (double)CurrentLongitude);
            foreach (Store Store in Stores)
            {
                GeoCoordinate eCoord = new GeoCoordinate((double)Store.Latitude, (double)Store.Longitude);
                Store.Distance = sCoord.GetDistanceTo(eCoord);
            }
            return Stores;
        }

        private void Sync(StoreChecking StoreChecking)
        {
            Store StoreMessage = new Store { Id = StoreChecking.StoreId };
            RabbitManager.PublishSingle(StoreMessage, RoutingKeyEnum.StoreUsed.Code);

            List<long> AlbumIds = new List<long>();
            if (StoreChecking.StoreCheckingImageMappings != null)
            {
                foreach (StoreCheckingImageMapping StoreCheckingImageMapping in StoreChecking.StoreCheckingImageMappings)
                {
                    AlbumIds.Add(StoreCheckingImageMapping.AlbumId);
                }
            }
            List<Album> messages = AlbumIds.Select(a => new Album { Id = a }).ToList();
            // phai check quan he lien ket 1:1, hay 1:n hay n:m de publishSingle, hay publishList
            if (messages.Any()) RabbitManager.PublishList(messages, RoutingKeyEnum.AlbumUsed.Code);

            RabbitManager.PublishSingle(StoreChecking, RoutingKeyEnum.StoreCheckingSync.Code);
        }
    }
}
