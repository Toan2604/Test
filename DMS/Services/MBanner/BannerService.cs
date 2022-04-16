using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers.Configuration;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Services.MImage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MBanner
{
    public interface IBannerService : IServiceScoped
    {
        Task<int> Count(BannerFilter BannerFilter);
        Task<List<Banner>> List(BannerFilter BannerFilter);
        Task<Banner> Get(long Id);
        Task<Banner> Create(Banner Banner);
        Task<Banner> Update(Banner Banner);
        Task<Banner> Delete(Banner Banner);
        Task<List<Banner>> BulkDelete(List<Banner> Banners);
        Task<Image> SaveImage(Image Image);
        Task<List<Banner>> Import(List<Banner> Banners);
        BannerFilter ToFilter(BannerFilter BannerFilter);
    }

    public class BannerService : BaseService, IBannerService
    {
        private IUOW UOW;
        private ILogging Logging;
        private IRabbitManager RabbitManager;
        private IBannerTemplate BannerTemplate;
        private ICurrentContext CurrentContext;
        private IBannerValidator BannerValidator;
        private IImageService ImageService;
        public BannerService(
            IUOW UOW,
            ILogging Logging,
            IBannerTemplate BannerTemplate,
            ICurrentContext CurrentContext,
            IRabbitManager RabbitManager,
            IBannerValidator BannerValidator,
            IImageService ImageService
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.BannerTemplate = BannerTemplate;
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.BannerValidator = BannerValidator;
            this.ImageService = ImageService;
        }
        public async Task<int> Count(BannerFilter BannerFilter)
        {
            try
            {
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

        public async Task<Banner> Create(Banner Banner)
        {
            if (!await BannerValidator.Create(Banner))
                return Banner;

            try
            {
                var Code = await UOW.BannerRepository.Count(new BannerFilter { });
                Banner.Code = (Code + 1).ToString();
                Banner.CreatorId = CurrentContext.UserId;
                await UOW.BannerRepository.Create(Banner);
                Banner = await UOW.BannerRepository.Get(Banner.Id);

                var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                var StoreUsers = await UOW.StoreUserRepository.List(new StoreUserFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = StoreUserSelect.RowId,
                    OrganizationId = new IdFilter { Equal = Banner.OrganizationId }
                });
                var AppUsers = await UOW.AppUserRepository.List(new AppUserFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = AppUserSelect.RowId,
                    OrganizationId = new IdFilter { Equal = Banner.OrganizationId }
                });
                var AppUserRowIds = AppUsers.Select(x => x.RowId).ToList();
                AppUserRowIds.Add(CurrentUser.RowId);
                AppUserRowIds = AppUserRowIds.Distinct().ToList();
                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                if (StoreUsers != null)
                {
                    foreach (var StoreUser in StoreUsers)
                    {
                        GlobalUserNotification GlobalUserNotification = BannerTemplate.CreateStoreUserNotification(CurrentUser.RowId, StoreUser.RowId, Banner, CurrentUser, NotificationType.CREATE);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                }
                if (AppUsers != null)
                {
                    foreach (var AppUserRowId in AppUserRowIds)
                    {
                        GlobalUserNotification GlobalUserNotification = new GlobalUserNotification();
                        if (AppUserRowId == CurrentUser.RowId)
                            GlobalUserNotification = BannerTemplate.CreateAppUserNotification(CurrentUser.RowId, AppUserRowId, Banner, CurrentUser, NotificationType.TOCREATOR);
                        else
                            GlobalUserNotification = BannerTemplate.CreateAppUserNotification(CurrentUser.RowId, AppUserRowId, Banner, CurrentUser, NotificationType.CREATE);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                }
                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);

                Logging.CreateAuditLog(Banner, new { }, nameof(BannerService));
                return await UOW.BannerRepository.Get(Banner.Id);
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(BannerService));
            }
            return null;
        }

        public async Task<Banner> Update(Banner Banner)
        {
            if (!await BannerValidator.Update(Banner))
                return Banner;
            try
            {
                var oldData = await UOW.BannerRepository.Get(Banner.Id);


                await UOW.BannerRepository.Update(Banner);


                var newData = await UOW.BannerRepository.Get(Banner.Id);
                Logging.CreateAuditLog(newData, oldData, nameof(BannerService));
                return newData;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(BannerService));
            }
            return null;
        }

        public async Task<Banner> Delete(Banner Banner)
        {
            if (!await BannerValidator.Delete(Banner))
                return Banner;

            try
            {

                await UOW.BannerRepository.Delete(Banner);

                Logging.CreateAuditLog(new { }, Banner, nameof(BannerService));
                return Banner;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(BannerService));
            }
            return null;
        }

        public async Task<List<Banner>> BulkDelete(List<Banner> Banners)
        {
            if (!await BannerValidator.BulkDelete(Banners))
                return Banners;

            try
            {

                await UOW.BannerRepository.BulkDelete(Banners);

                Logging.CreateAuditLog(new { }, Banners, nameof(BannerService));
                return Banners;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(BannerService));
            }
            return null;
        }

        public async Task<List<Banner>> Import(List<Banner> Banners)
        {
            if (!await BannerValidator.Import(Banners))
                return Banners;
            try
            {

                await UOW.BannerRepository.BulkMerge(Banners);


                Logging.CreateAuditLog(Banners, new { }, nameof(BannerService));
                return Banners;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(BannerService));
            }
            return null;
        }

        public async Task<Image> SaveImage(Image Image)
        {
            FileInfo fileInfo = new FileInfo(Image.Name);
            string path = $"/banner/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}{fileInfo.Extension}";
            //using (var image = SixLabors.ImageSharp.Image.Load(Image.Content))
            //{
            //    var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "foo.png");

            //    image.Clone(ctx => ctx.Crop(560, 300)).Save(path);
            //}
            Image = await ImageService.Create(Image, path);
            return Image;
        }

        public BannerFilter ToFilter(BannerFilter filter)
        {

            return filter;
        }
    }
}
