using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Helpers;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Handlers.Configuration;

namespace DMS.Services
{
    public interface IMaintenanceService : IServiceScoped
    {
        Task CleanHangfire();
        Task Job_Checking();
        Task CompleteStoreCheckout();
        Task CreateStoreUnchecking();
        Task AutoInactive();
        Task UpdateProductOrderSaleCounter();
        Task AutoClearCheckin();
    }
    public class MaintenanceService : IMaintenanceService
    {
        private DataContext DataContext;
        private IRabbitManager RabbitManager;
        public MaintenanceService(DataContext DataContext,
            IRabbitManager RabbitManager)
        {
            this.DataContext = DataContext;
            this.RabbitManager = RabbitManager;
        }

        public async Task CleanHangfire()
        {
            var commandText = @"
                TRUNCATE TABLE [HangFire].[AggregatedCounter]
                TRUNCATE TABLE[HangFire].[Counter]
                TRUNCATE TABLE[HangFire].[JobParameter]
                TRUNCATE TABLE[HangFire].[JobQueue]
                TRUNCATE TABLE[HangFire].[List]
                TRUNCATE TABLE[HangFire].[State]
                DELETE FROM[HangFire].[Job]
                DBCC CHECKIDENT('[HangFire].[Job]', reseed, 0)
                UPDATE[HangFire].[Hash] SET Value = 1 WHERE Field = 'LastJobId'";
            var result = await DataContext.Database.ExecuteSqlRawAsync(commandText);
        }

        public async Task Job_Checking()
        {
            await CompleteStoreCheckout();
            await CreateStoreUnchecking();
        }

        public async Task CompleteStoreCheckout()
        {
            DateTime Now = StaticParams.DateTimeNow;
            List<StoreCheckingDAO> StoreCheckingDAOs = await DataContext.StoreChecking
                .Where(sc => sc.CheckOutAt.HasValue == false && sc.CheckInAt.HasValue).ToListWithNoLockAsync();
            foreach (StoreCheckingDAO StoreCheckingDAO in StoreCheckingDAOs)
            {
                StoreCheckingDAO.CheckOutAt = Now;
                StoreChecking StoreChecking = new StoreChecking
                {
                    Id = StoreCheckingDAO.Id,
                    StoreId = StoreCheckingDAO.StoreId,
                    SaleEmployeeId = StoreCheckingDAO.SaleEmployeeId,
                    OrganizationId = StoreCheckingDAO.OrganizationId,
                    Longitude = StoreCheckingDAO.Longitude,
                    Latitude = StoreCheckingDAO.Latitude,
                    CheckInAt = StoreCheckingDAO.CheckInAt,
                    CheckOutAt = StoreCheckingDAO.CheckOutAt,
                    ImageCounter = StoreCheckingDAO.ImageCounter,
                    Planned = StoreCheckingDAO.Planned,
                    IsOpenedStore = StoreCheckingDAO.IsOpenedStore,
                    DeviceName = StoreCheckingDAO.DeviceName,
                    CheckOutLongitude = StoreCheckingDAO.CheckOutLongitude,
                    CheckOutLatitude = StoreCheckingDAO.CheckOutLatitude,
                    CheckInDistance = StoreCheckingDAO.CheckInDistance,
                    CheckOutDistance = StoreCheckingDAO.CheckOutDistance,
                };
                RabbitManager.PublishSingle(StoreChecking, RoutingKeyEnum.StoreCheckingSync.Code);
            }
            await DataContext.SaveChangesAsync();
        }

        public async Task CreateStoreUnchecking()
        {
            DateTime End = StaticParams.DateTimeNow;
            DateTime Start = End.AddDays(-1).AddMinutes(1);
            List<ERouteContentDAO> ERouteContentDAOs = await DataContext.ERouteContent
                .Where(ec => (!ec.ERoute.EndDate.HasValue || Start <= ec.ERoute.EndDate) && ec.ERoute.StartDate <= End)
                .Include(ec => ec.ERoute)
                .Include(ec => ec.ERouteContentDays)
                .Where(x => x.ERoute.RequestStateId == RequestStateEnum.APPROVED.Id && x.ERoute.StatusId == StatusEnum.ACTIVE.Id)
                .ToListWithNoLockAsync();
            foreach (var ERouteContentDAO in ERouteContentDAOs)
            {
                ERouteContentDAO.RealStartDate = ERouteContentDAO.ERoute.RealStartDate;
            }
            ERouteContentDAOs = ERouteContentDAOs.Distinct().ToList();
            List<StoreUncheckingDAO> PlannedStoreUncheckingDAOs = new List<StoreUncheckingDAO>();
            List<StoreUncheckingDAO> StoreUncheckingDAOs = new List<StoreUncheckingDAO>();
            foreach (ERouteContentDAO ERouteContentDAO in ERouteContentDAOs)
            {
                StoreUncheckingDAO StoreUncheckingDAO = PlannedStoreUncheckingDAOs.Where(su =>
                    su.Date == Start &&
                    su.AppUserId == ERouteContentDAO.ERoute.SaleEmployeeId &&
                    su.StoreId == ERouteContentDAO.StoreId
                ).FirstOrDefault();
                if (StoreUncheckingDAO == null)
                {
                    if (Start >= ERouteContentDAO.ERoute.RealStartDate)
                    {
                        long gap = (Start - ERouteContentDAO.ERoute.RealStartDate).Days % 28;
                        if (ERouteContentDAO.ERouteContentDays.Any(ecd => ecd.OrderDay == gap && ecd.Planned))
                        {
                            StoreUncheckingDAO = new StoreUncheckingDAO
                            {
                                AppUserId = ERouteContentDAO.ERoute.SaleEmployeeId,
                                Date = End,
                                StoreId = ERouteContentDAO.StoreId,
                                OrganizationId = ERouteContentDAO.ERoute.OrganizationId
                            };
                            PlannedStoreUncheckingDAOs.Add(StoreUncheckingDAO);
                        }
                    }
                }
            }
            List<StoreCheckingDAO> StoreCheckingDAOs = await DataContext.StoreChecking.Where(sc => sc.CheckOutAt.HasValue &&
                Start <= sc.CheckOutAt.Value && sc.CheckOutAt.Value <= End).ToListWithNoLockAsync();
            foreach (StoreUncheckingDAO StoreUncheckingDAO in PlannedStoreUncheckingDAOs)
            {
                if (!StoreCheckingDAOs.Any(sc => sc.SaleEmployeeId == StoreUncheckingDAO.AppUserId && sc.StoreId == StoreUncheckingDAO.StoreId))
                {
                    StoreUncheckingDAOs.Add(StoreUncheckingDAO);
                }
            }

            StoreUncheckingDAOs = StoreUncheckingDAOs.Distinct().ToList();
            await DataContext.StoreUnchecking.BulkInsertAsync(StoreUncheckingDAOs);

            List<StoreUnchecking> StoreUnchecking = StoreUncheckingDAOs.Select(x => new StoreUnchecking
            {
                Id = x.Id,
                StoreId = x.StoreId,
                AppUserId = x.AppUserId,
                OrganizationId = x.OrganizationId,
                Date = x.Date,
            }).ToList();
            RabbitManager.PublishList(StoreUnchecking, RoutingKeyEnum.StoreUncheckingSync.Code);
        }

        public async Task UpdateProductOrderSaleCounter()
        {
            var Product_query = DataContext.Product.AsNoTracking();
            Product_query = Product_query.Where(x => x.DeletedAt == null);
            var Products = await Product_query.Select(x => new Product { Id = x.Id }).ToListWithNoLockAsync();
            RabbitManager.PublishList(Products, RoutingKeyEnum.ProductCal.Code);
        }

        public async Task AutoInactive()
        {
            var Now = StaticParams.DateTimeNow;
            await DataContext.ERoute.Where(x => x.EndDate.HasValue && x.EndDate.Value < Now).UpdateFromQueryAsync(x => new ERouteDAO { StatusId = StatusEnum.INACTIVE.Id });
            await DataContext.PriceList.Where(x => x.EndDate.HasValue && x.EndDate.Value < Now).UpdateFromQueryAsync(x => new PriceListDAO { StatusId = StatusEnum.INACTIVE.Id });
            await DataContext.WorkflowDefinition.Where(x => x.EndDate.HasValue && x.EndDate.Value < Now).UpdateFromQueryAsync(x => new WorkflowDefinitionDAO { StatusId = StatusEnum.INACTIVE.Id });
            await DataContext.Survey.Where(x => x.EndAt.HasValue && x.EndAt.Value < Now).UpdateFromQueryAsync(x => new SurveyDAO { StatusId = StatusEnum.INACTIVE.Id });
            await DataContext.PromotionCode.Where(x => x.EndDate.HasValue && x.EndDate.Value < Now).UpdateFromQueryAsync(x => new PromotionCodeDAO { StatusId = StatusEnum.INACTIVE.Id });
        }

        public async Task AutoClearCheckin()
        {
            DateTime date = StaticParams.DateTimeNow.AddDays(-30);
            await DataContext.AppUserGps
                .Where(x => date > x.GPSUpdatedAt)
                .DeleteFromQueryAsync();
            RabbitManager.PublishSingle(new AppUser(), RoutingKeyEnum.AppUserClear.Code);
        }
    }
}
