using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MIndirectSalesOrder;
using DMS.Services.MOrganization;
using DMS.Services.MStoreChecking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.monitor
{
    public class DashboardMonitorController : RpcController
    {
        private ITimeService TimeService;
        private DataContext DataContext;
        private IAppUserService AppUserService;
        private ICurrentContext CurrentContext;
        private IOrganizationService OrganizationService;
        private IIndirectSalesOrderService IndirectSalesOrderService;
        private IStoreCheckingService StoreCheckingService;
        public DashboardMonitorController(
            ITimeService TimeService,
            DataContext DataContext,
            IAppUserService AppUserService,
            ICurrentContext CurrentContext,
            IOrganizationService OrganizationService,
            IIndirectSalesOrderService IndirectSalesOrderService,
            IStoreCheckingService StoreCheckingService)
        {
            this.TimeService = TimeService;
            this.DataContext = DataContext;
            this.AppUserService = AppUserService;
            this.CurrentContext = CurrentContext;
            this.OrganizationService = OrganizationService;
            this.IndirectSalesOrderService = IndirectSalesOrderService;
            this.StoreCheckingService = StoreCheckingService;
        }

        [Route(DashboardMonitorRoute.FilterListTime), HttpPost]
        public List<DashboardMonitor_EnumList> FilterListTime()
        {
            List<DashboardMonitor_EnumList> Dashborad_EnumLists = new List<DashboardMonitor_EnumList>();

            Dashborad_EnumLists.Add(new DashboardMonitor_EnumList(DashboardPeriodTimeEnum.TODAY));
            Dashborad_EnumLists.Add(new DashboardMonitor_EnumList(DashboardPeriodTimeEnum.THIS_WEEK));
            Dashborad_EnumLists.Add(new DashboardMonitor_EnumList(DashboardPeriodTimeEnum.THIS_MONTH));
            Dashborad_EnumLists.Add(new DashboardMonitor_EnumList(DashboardPeriodTimeEnum.LAST_MONTH));
            return Dashborad_EnumLists;
        }

        [Route(DashboardMonitorRoute.FilterListOrganization), HttpPost]
        public async Task<List<DashboardMonitor_OrganizationDTO>> FilterListOrganization([FromBody] DashboardMonitor_OrganizationFilterDTO DashboardMonitor_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = DashboardMonitor_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = DashboardMonitor_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = DashboardMonitor_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = DashboardMonitor_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = DashboardMonitor_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = DashboardMonitor_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = DashboardMonitor_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = DashboardMonitor_OrganizationFilterDTO.Phone;
            OrganizationFilter.Address = DashboardMonitor_OrganizationFilterDTO.Address;
            OrganizationFilter.Email = DashboardMonitor_OrganizationFilterDTO.Email;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && OrganizationFilter.Id.In.Contains(o.Id)).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (DashboardMonitor_OrganizationFilterDTO.Id?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == DashboardMonitor_OrganizationFilterDTO.Id.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationFilter.Id.In = OrganizationDAOs.Select(o => o.Id).ToList();

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<DashboardMonitor_OrganizationDTO> DashboardMonitor_OrganizationDTOs = Organizations
                .Select(x => new DashboardMonitor_OrganizationDTO(x)).ToList();
            return DashboardMonitor_OrganizationDTOs;
        }

        [Route(DashboardMonitorRoute.StoreChecking), HttpPost]
        public async Task<DashboardMonitor_StoreCheckingDTO> StoreChecking()
        {
            DateTime Now = StaticParams.DateTimeNow.Date;
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);

            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);
            var AppUserIds = await DataContext.AppUser
                .Where(x => x.DeletedAt == null)
                .Where(x => x.Organization.Path.StartsWith(CurrentUser.Organization.Path))
                .Select(x => x.Id)
                .ToListWithNoLockAsync();

            StoreCheckingFilter StoreCheckingFilter = new StoreCheckingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreCheckingSelect.Id | StoreCheckingSelect.CheckOutAt,
                SaleEmployeeId = new IdFilter { In = AppUserIds },
                CheckOutAt = new DateFilter { GreaterEqual = Start, LessEqual = End }
            };

            List<StoreChecking> StoreCheckings = await StoreCheckingService.List(StoreCheckingFilter);
            List<DashboardMonitor_StoreCheckingHourDTO> NumberCheckingHours = StoreCheckings.GroupBy(x => x.CheckOutAt.Value.AddHours(CurrentContext.TimeZone).Hour)
                .Select(x => new DashboardMonitor_StoreCheckingHourDTO { Hour = (x.Key).ToString(), Counter = x.Count() }).ToList();

            DashboardMonitor_StoreCheckingDTO DashboardMonitor_StoreCheckingDTO = new DashboardMonitor_StoreCheckingDTO();
            DashboardMonitor_StoreCheckingDTO.StoreCheckingHours = new List<DashboardMonitor_StoreCheckingHourDTO>();
            for (int i = 0; i < 24; i++)
            {
                DashboardMonitor_StoreCheckingDTO.StoreCheckingHours.Add(new DashboardMonitor_StoreCheckingHourDTO
                {
                    Counter = 0,
                    Hour = i.ToString()
                });
            }
            foreach (var NumberCheckingHour in NumberCheckingHours)
            {
                var x = DashboardMonitor_StoreCheckingDTO.StoreCheckingHours.Where(x => x.Hour == NumberCheckingHour.Hour).FirstOrDefault();
                x.Counter = NumberCheckingHour.Counter;
            }

            return DashboardMonitor_StoreCheckingDTO;
        }

        [Route(DashboardMonitorRoute.SaleEmployeeOnline), HttpPost]
        public async Task<DashboardMonitor_SaleEmployeeOnlineDTO> SaleEmployeeOnline()
        {
            var AppUser = await AppUserService.Get(CurrentContext.UserId);
            var OnlineTime = StaticParams.DateTimeNow.AddMinutes(-30);

            var query = from au in DataContext.AppUser
                        join o in DataContext.Organization on au.OrganizationId equals o.Id
                        where au.DeletedAt.HasValue == false && au.StatusId == Enums.StatusEnum.ACTIVE.Id &&
                        o.Path.StartsWith(AppUser.Organization.Path) &&
                        au.UpdatedAt > OnlineTime &&
                        o.DeletedAt == null && au.DeletedAt == null
                        group au by au.AppUserGps.Select(x => x.GPSUpdatedAt).FirstOrDefault()
                        .AddHours(CurrentContext.TimeZone).Hour into x
                        select new DashboardMonitor_SaleEmployeeOnlineHourDTO
                        {
                            Hour = x.Key.ToString(),
                            Counter = x.Count()
                        };

            List<DashboardMonitor_SaleEmployeeOnlineHourDTO> DashboardMonitor_SaleEmployeeOnlineHourDTOs = await query.ToListWithNoLockAsync();
            DashboardMonitor_SaleEmployeeOnlineDTO DashboardMonitor_SaleEmployeeOnlineDTO = new DashboardMonitor_SaleEmployeeOnlineDTO();

            DashboardMonitor_SaleEmployeeOnlineDTO.SaleEmployeeOnlineHours = new List<DashboardMonitor_SaleEmployeeOnlineHourDTO>();
            for (int i = 0; i < 24; i++)
            {
                DashboardMonitor_SaleEmployeeOnlineDTO.SaleEmployeeOnlineHours.Add(new DashboardMonitor_SaleEmployeeOnlineHourDTO
                {
                    Counter = 0,
                    Hour = i.ToString()
                });
            }
            foreach (var SaleEmployeeOnlineHour in DashboardMonitor_SaleEmployeeOnlineHourDTOs)
            {
                var x = DashboardMonitor_SaleEmployeeOnlineDTO.SaleEmployeeOnlineHours.Where(x => x.Hour == SaleEmployeeOnlineHour.Hour).FirstOrDefault();
                x.Counter = SaleEmployeeOnlineHour.Counter;
            }
            return DashboardMonitor_SaleEmployeeOnlineDTO;
        }

        [Route(DashboardMonitorRoute.StatisticIndirectSalesOrder), HttpPost]
        public async Task<DashboardMonitor_StatisticIndirectSalesOrderDTO> StatisticIndirectSalesOrder()
        {
            DateTime Now = StaticParams.DateTimeNow.Date;
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);

            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);
            var AppUserIds = await DataContext.AppUser
                .Where(x => x.DeletedAt == null)
                .Where(x => x.Organization.Path.StartsWith(CurrentUser.Organization.Path))
                .Select(x => x.Id)
                .ToListWithNoLockAsync();

            IndirectSalesOrderFilter IndirectSalesOrderFilter = new IndirectSalesOrderFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = IndirectSalesOrderSelect.Id | IndirectSalesOrderSelect.OrderDate,
                OrderDate = new DateFilter { GreaterEqual = Start, LessEqual = End },
                AppUserId = new IdFilter { In = AppUserIds },
                RequestStateId = new IdFilter { Equal = RequestStateEnum.APPROVED.Id }
            };

            List<IndirectSalesOrder> IndirectSalesOrders = await IndirectSalesOrderService.List(IndirectSalesOrderFilter);
            List<DashboardMonitor_StatisticIndirectSalesOrderHourDTO> DashboardMonitor_StatisticIndirectSalesOrderHourDTOs = IndirectSalesOrders.GroupBy(x => x.OrderDate.AddHours(CurrentContext.TimeZone).Hour)
                .Select(x => new DashboardMonitor_StatisticIndirectSalesOrderHourDTO { Hour = x.Key.ToString(), Counter = x.Count() }).ToList();

            DashboardMonitor_StatisticIndirectSalesOrderDTO DashboardMonitor_StatisticIndirectSalesOrderDTO = new DashboardMonitor_StatisticIndirectSalesOrderDTO();
            DashboardMonitor_StatisticIndirectSalesOrderDTO.StatisticIndirectSalesOrderHours = new List<DashboardMonitor_StatisticIndirectSalesOrderHourDTO>();

            for (int i = 0; i < 24; i++)
            {
                DashboardMonitor_StatisticIndirectSalesOrderDTO.StatisticIndirectSalesOrderHours.Add(new DashboardMonitor_StatisticIndirectSalesOrderHourDTO
                {
                    Counter = 0,
                    Hour = i.ToString()
                });
            }
            foreach (var IndirectSalesOrderHour in DashboardMonitor_StatisticIndirectSalesOrderHourDTOs)
            {
                var x = DashboardMonitor_StatisticIndirectSalesOrderDTO.StatisticIndirectSalesOrderHours.Where(x => x.Hour == IndirectSalesOrderHour.Hour).FirstOrDefault();
                x.Counter = IndirectSalesOrderHour.Counter;
            }
            return DashboardMonitor_StatisticIndirectSalesOrderDTO;
        }

        [Route(DashboardMonitorRoute.ImageStoreCheking), HttpPost]
        public async Task<DashboardMonitor_StoreCheckingImageMappingDTO> ImageStoreCheking()
        {
            DateTime Now = StaticParams.DateTimeNow.Date;
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);

            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);
            var AppUserIds = await DataContext.AppUser
                .Where(x => x.DeletedAt == null)
                .Where(x => x.Organization.Path.StartsWith(CurrentUser.Organization.Path))
                .Select(x => x.Id)
                .ToListWithNoLockAsync();

            var query = from si in DataContext.StoreImage
                        where si.SaleEmployeeId.HasValue && AppUserIds.Contains(si.SaleEmployeeId.Value) &&
                        Now <= si.ShootingAt && si.ShootingAt <= End
                        group si by si.ShootingAt.AddHours(CurrentContext.TimeZone).Hour into i
                        select new DashboardMonitor_StoreCheckingImageMappingHourDTO
                        {
                            Hour = i.Key.ToString(),
                            Counter = i.Count()
                        };
            List<DashboardMonitor_StoreCheckingImageMappingHourDTO> DashboardMonitor_StoreCheckingImageMappingHourDTOs = await query.ToListWithNoLockAsync();
            DashboardMonitor_StoreCheckingImageMappingDTO DashboardMonitor_StoreCheckingImageMappingDTO = new DashboardMonitor_StoreCheckingImageMappingDTO();

            DashboardMonitor_StoreCheckingImageMappingDTO.StoreCheckingImageMappingHours = new List<DashboardMonitor_StoreCheckingImageMappingHourDTO>();
            for (int i = 0; i < 24; i++)
            {
                DashboardMonitor_StoreCheckingImageMappingDTO.StoreCheckingImageMappingHours.Add(new DashboardMonitor_StoreCheckingImageMappingHourDTO
                {
                    Counter = 0,
                    Hour = i.ToString()
                });
            }
            foreach (var StoreCheckingImageMappingHour in DashboardMonitor_StoreCheckingImageMappingHourDTOs)
            {
                var x = DashboardMonitor_StoreCheckingImageMappingDTO.StoreCheckingImageMappingHours.Where(x => x.Hour == StoreCheckingImageMappingHour.Hour).FirstOrDefault();
                x.Counter = StoreCheckingImageMappingHour.Counter;
            }
            return DashboardMonitor_StoreCheckingImageMappingDTO;
        }

        [Route(DashboardMonitorRoute.StoreCoverage), HttpPost]
        public async Task<List<DashboardMonitor_StoreDTO>> StoreCoverage([FromBody] DashboardMonitor_StoreFilterDTO DashboardMonitor_StoreFilterDTO)
        {
            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (DashboardMonitor_StoreFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == DashboardMonitor_StoreFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();

            var query = from s in DataContext.Store
                        where OrganizationIds.Contains(s.OrganizationId) &&
                        s.DeletedAt == null
                        select new DashboardMonitor_StoreDTO
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Latitude = s.Latitude,
                            Longitude = s.Longitude,
                            Telephone = s.Telephone,
                            IsScouting = false
                        };
            List<DashboardMonitor_StoreDTO> DashboardMonitor_StoreDTOs = await query.Distinct().ToListWithNoLockAsync();

            var query_Scouting = from ss in DataContext.StoreScouting
                                 join au in DataContext.AppUser on ss.CreatorId equals au.Id
                                 where (OrganizationIds.Contains(au.OrganizationId)) &&
                                 ss.DeletedAt == null
                                 select new DashboardMonitor_StoreDTO
                                 {
                                     Id = ss.Id,
                                     Name = ss.Name,
                                     Latitude = ss.Latitude,
                                     Longitude = ss.Longitude,
                                     Telephone = ss.OwnerPhone,
                                     IsScouting = true
                                 };
            List<DashboardMonitor_StoreDTO> DashboardMonitor_StoreScotingDTOs = await query_Scouting.Distinct().ToListWithNoLockAsync();
            DashboardMonitor_StoreDTOs.AddRange(DashboardMonitor_StoreScotingDTOs);
            return DashboardMonitor_StoreDTOs;
        }

        [Route(DashboardMonitorRoute.SaleEmployeeLocation), HttpPost]
        public async Task<List<DashboardMonitor_AppUserDTO>> SaleEmployeeLocation([FromBody] DashboardMonitor_AppUserFilterDTO DashboardMonitor_AppUserFilterDTO)
        {
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (DashboardMonitor_AppUserFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == DashboardMonitor_AppUserFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();

            var query = from au in DataContext.AppUser
                        where au.StatusId == Enums.StatusEnum.ACTIVE.Id &&
                        au.DeletedAt == null &&
                        au.AppUserGps.Where(x => Start <= x.GPSUpdatedAt && x.GPSUpdatedAt <= End).Count() > 0 &&
                        OrganizationIds.Contains(au.OrganizationId)
                        select new DashboardMonitor_AppUserDTO
                        {
                            Id = au.Id,
                            DisplayName = au.DisplayName,
                            Username = au.Username,
                            Latitude = au.AppUserGps.Select(x => x.Latitude).LastOrDefault(),
                            Longitude = au.AppUserGps.Select(x => x.Longitude).LastOrDefault(),
                        };

            List<DashboardMonitor_AppUserDTO> DashboardMonitor_AppUserDTOs = await query.Distinct().ToListWithNoLockAsync();
            return DashboardMonitor_AppUserDTOs;
        }

        [Route(DashboardMonitorRoute.ListIndirectSalesOrder), HttpPost]
        public async Task<List<DashboardMonitor_IndirectSalesOrderDTO>> ListIndirectSalesOrder()
        {
            var appUser = await AppUserService.Get(CurrentContext.UserId);
            var query = from i in DataContext.IndirectSalesOrder
                        join r in DataContext.RequestState on i.RequestStateId equals r.Id
                        join au in DataContext.AppUser on i.SaleEmployeeId equals au.Id
                        join o in DataContext.Organization on au.OrganizationId equals o.Id
                        where o.Path.StartsWith(appUser.Organization.Path) &&
                        i.RequestStateId != RequestStateEnum.NEW.Id &&
                        au.DeletedAt == null && o.DeletedAt == null
                        orderby i.OrderDate descending
                        select new DashboardMonitor_IndirectSalesOrderDTO
                        {
                            Id = i.Id,
                            Code = i.Code,
                            OrderDate = i.OrderDate,
                            RequestStateId = r.Id,
                            SaleEmployeeId = i.SaleEmployeeId,
                            Total = i.Total,
                            SaleEmployee = i.SaleEmployee == null ? null : new DashboardMonitor_AppUserDTO
                            {
                                Id = i.SaleEmployee.Id,
                                DisplayName = i.SaleEmployee.DisplayName,
                                Username = i.SaleEmployee.Username,
                            },
                            RequestState = new DashboardMonitor_RequestStateDTO
                            {
                                Id = r.Id,
                                Code = r.Code,
                                Name = r.Name,
                            }
                        };

            return await query.Skip(0).Take(5).ToListWithNoLockAsync();
        }

        [Route(DashboardMonitorRoute.TopSaleEmployeeStoreChecking), HttpPost]
        public async Task<List<DashboardMonitor_TopSaleEmployeeStoreCheckingDTO>> TopSaleEmployeeStoreChecking([FromBody] DashboardMonitor_TopSaleEmployeeStoreCheckingFilterDTO DashboardMonitor_TopSaleEmployeeStoreCheckingFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow.Date;
            DateTime Start = new DateTime(Now.Year, Now.Month, Now.Day);
            DateTime End = new DateTime(Now.Year, Now.Month, Now.Day);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardMonitor_TopSaleEmployeeStoreCheckingFilterDTO.Time, CurrentContext);

            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);
            var AppUserIds = await DataContext.AppUser
                .Where(x => x.DeletedAt == null)
                .Where(x => x.Organization.Path.StartsWith(CurrentUser.Organization.Path))
                .Select(x => x.Id)
                .ToListWithNoLockAsync();

            StoreCheckingFilter StoreCheckingFilter = new StoreCheckingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreCheckingSelect.Id | StoreCheckingSelect.CheckOutAt | StoreCheckingSelect.SaleEmployee,
                CheckOutAt = new DateFilter { GreaterEqual = Start, LessEqual = End },
                SaleEmployeeId = new IdFilter { In = AppUserIds }
            };

            var StoreCheckings = await StoreCheckingService.List(StoreCheckingFilter);
            var SaleEmployeeIds = StoreCheckings.Select(x => x.SaleEmployeeId).Distinct().ToList();

            AppUserFilter AppUserFilter = new AppUserFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.DisplayName,
                Id = new IdFilter { In = SaleEmployeeIds }
            };
            var AppUsers = await AppUserService.List(AppUserFilter);

            List<DashboardMonitor_TopSaleEmployeeStoreCheckingDTO> DashboardMonitor_TopSaleEmployeeStoreCheckingDTOs = StoreCheckings.GroupBy(x => x.SaleEmployeeId)
                .Select(x => new DashboardMonitor_TopSaleEmployeeStoreCheckingDTO
                {
                    SaleEmployeeId = x.Key,
                    Counter = x.Count()
                }).ToList();
            foreach (var DashboardMonitor_TopSaleEmployeeStoreCheckingDTO in DashboardMonitor_TopSaleEmployeeStoreCheckingDTOs)
            {
                DashboardMonitor_TopSaleEmployeeStoreCheckingDTO.DisplayName = AppUsers.Where(x => x.Id == DashboardMonitor_TopSaleEmployeeStoreCheckingDTO.SaleEmployeeId)
                    .Select(x => x.DisplayName).FirstOrDefault();
            }

            return DashboardMonitor_TopSaleEmployeeStoreCheckingDTOs.OrderByDescending(x => x.Counter).Skip(0).Take(5).ToList();
        }
    }
}
