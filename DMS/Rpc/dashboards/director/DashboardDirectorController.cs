using DMS.Common;
using DMS.DWModels;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MDistrict;
using DMS.Services.MOrganization;
using DMS.Services.MProvince;
using DMS.Services.MStore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.director
{
    public partial class DashboardDirectorController : RpcController
    {
        private ITimeService TimeService;
        private DataContext DataContext;
        private DWContext DWContext;
        private IAppUserService AppUserService;
        private ICurrentContext CurrentContext;
        private IOrganizationService OrganizationService;
        private IProvinceService ProvinceService;
        private IDistrictService DistrictService;
        private IStoreService StoreService;

        public DashboardDirectorController(
            ITimeService TimeService,
            DataContext DataContext,
            DWContext DWContext,
            IAppUserService AppUserService,
            ICurrentContext CurrentContext,
            IOrganizationService OrganizationService,
            IProvinceService ProvinceService,
            IDistrictService DistrictService,
            IStoreService StoreService)
        {
            this.TimeService = TimeService;
            this.DataContext = DataContext;
            this.DWContext = DWContext;
            this.AppUserService = AppUserService;
            this.CurrentContext = CurrentContext;
            this.OrganizationService = OrganizationService;
            this.ProvinceService = ProvinceService;
            this.DistrictService = DistrictService;
            this.StoreService = StoreService;
        }

        #region Filter List
        [Route(DashboardDirectorRoute.FilterListTime), HttpPost]
        public List<DashboardDirector_EnumList> FilterListTime()
        {
            List<DashboardDirector_EnumList> Dashboard_EnumLists = new List<DashboardDirector_EnumList>();
            Dashboard_EnumLists.Add(new DashboardDirector_EnumList(DashboardPeriodTimeEnum.TODAY));
            Dashboard_EnumLists.Add(new DashboardDirector_EnumList(DashboardPeriodTimeEnum.THIS_WEEK));
            Dashboard_EnumLists.Add(new DashboardDirector_EnumList(DashboardPeriodTimeEnum.LAST_WEEK));
            Dashboard_EnumLists.Add(new DashboardDirector_EnumList(DashboardPeriodTimeEnum.THIS_MONTH));
            Dashboard_EnumLists.Add(new DashboardDirector_EnumList(DashboardPeriodTimeEnum.LAST_MONTH));
            Dashboard_EnumLists.Add(new DashboardDirector_EnumList(DashboardPeriodTimeEnum.THIS_QUARTER));
            Dashboard_EnumLists.Add(new DashboardDirector_EnumList(DashboardPeriodTimeEnum.LAST_QUATER));
            Dashboard_EnumLists.Add(new DashboardDirector_EnumList(DashboardPeriodTimeEnum.YEAR));
            return Dashboard_EnumLists;
        }

        [Route(DashboardDirectorRoute.FilterListTimeDetail), HttpPost]
        public DashboardDirector_TimeDetailDTO FilterListTimeDetail(DashboardDirector_TimeDetailFilterDTO DashboardDirector_TimeDetailFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = new DateTime(Now.Year, Now.Month, 1).AddHours(0 - CurrentContext.TimeZone);
            DateTime End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddHours(0 - CurrentContext.TimeZone);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_TimeDetailFilterDTO.PeriodTimeId, CurrentContext);
            DashboardDirector_TimeDetailDTO DashboardDirector_TimeDetailDTO = new DashboardDirector_TimeDetailDTO { StartDate = Start, EndDate = End };
            return DashboardDirector_TimeDetailDTO;
        }

        [Route(DashboardDirectorRoute.FilterListAppUser), HttpPost]
        public async Task<List<DashboardDirector_AppUserDTO>> FilterListAppUser([FromBody] DashboardDirector_AppUserFilterDTO DashboardDirector_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = DashboardDirector_AppUserFilterDTO.Id;
            AppUserFilter.Username = DashboardDirector_AppUserFilterDTO.Username;
            AppUserFilter.Password = DashboardDirector_AppUserFilterDTO.Password;
            AppUserFilter.DisplayName = DashboardDirector_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = DashboardDirector_AppUserFilterDTO.Address;
            AppUserFilter.Email = DashboardDirector_AppUserFilterDTO.Email;
            AppUserFilter.Phone = DashboardDirector_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = DashboardDirector_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = DashboardDirector_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = DashboardDirector_AppUserFilterDTO.OrganizationId;
            AppUserFilter.SexId = DashboardDirector_AppUserFilterDTO.SexId;
            AppUserFilter.StatusId = DashboardDirector_AppUserFilterDTO.StatusId;
            AppUserFilter.Birthday = DashboardDirector_AppUserFilterDTO.Birthday;
            AppUserFilter.ProvinceId = DashboardDirector_AppUserFilterDTO.ProvinceId;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<DashboardDirector_AppUserDTO> DashboardDirector_AppUserDTOs = AppUsers
                .Select(x => new DashboardDirector_AppUserDTO(x)).ToList();
            return DashboardDirector_AppUserDTOs;
        }

        [Route(DashboardDirectorRoute.FilterListOrganization), HttpPost]
        public async Task<List<DashboardDirector_OrganizationDTO>> FilterListOrganization([FromBody] DashboardDirector_OrganizationFilterDTO DashboardDirector_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 20;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = DashboardDirector_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = DashboardDirector_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = DashboardDirector_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = DashboardDirector_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = DashboardDirector_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = DashboardDirector_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = DashboardDirector_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = DashboardDirector_OrganizationFilterDTO.Phone;
            OrganizationFilter.Address = DashboardDirector_OrganizationFilterDTO.Address;
            OrganizationFilter.Email = DashboardDirector_OrganizationFilterDTO.Email;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<DashboardDirector_OrganizationDTO> DashboardDirector_OrganizationDTOs = Organizations
                .Select(x => new DashboardDirector_OrganizationDTO(x)).ToList();
            return DashboardDirector_OrganizationDTOs;
        }

        [Route(DashboardDirectorRoute.FilterListProvince), HttpPost]
        public async Task<List<DashboardDirector_ProvinceDTO>> FilterListProvince([FromBody] DashboardDirector_ProvinceFilterDTO DashboardDirector_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = DashboardDirector_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = DashboardDirector_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = DashboardDirector_ProvinceFilterDTO.StatusId;

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<DashboardDirector_ProvinceDTO> DashboardDirector_ProvinceDTOs = Provinces
                .Select(x => new DashboardDirector_ProvinceDTO(x)).ToList();
            return DashboardDirector_ProvinceDTOs;
        }

        [Route(DashboardDirectorRoute.FilterListDistrict), HttpPost]
        public async Task<List<DashboardDirector_DistrictDTO>> FilterListDistrict([FromBody] DashboardDirector_DistrictFilterDTO DashboardDirector_DistrictFilterDTO)
        {
            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = DashboardDirector_DistrictFilterDTO.Id;
            DistrictFilter.Name = DashboardDirector_DistrictFilterDTO.Name;
            DistrictFilter.StatusId = DashboardDirector_DistrictFilterDTO.StatusId;
            DistrictFilter.ProvinceId = DashboardDirector_DistrictFilterDTO.ProvinceId;

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<DashboardDirector_DistrictDTO> DashboardDirector_DistrictDTOs = Districts
                .Select(x => new DashboardDirector_DistrictDTO(x)).ToList();
            return DashboardDirector_DistrictDTOs;
        }
        #endregion

        [Route(DashboardDirectorRoute.CountStore), HttpPost]
        public async Task<long> CountStore([FromBody] DashboardDirector_StoreFilterDTO DashboardDirector_StoreFilterDTO)
        {
            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_StoreFilterDTO);

            var StoreQuery = DataContext.Store.AsNoTracking();
            if (DashboardDirector_StoreFilterDTO.AppUserId.Equal.HasValue)
            {
                long? SaleEmployeeId = DashboardDirector_StoreFilterDTO.AppUserId?.Equal;
                AppUserIds = AppUserIds.Where(x => x == SaleEmployeeId.Value).ToList();
                var SaleEmployeeIdFilter = new IdFilter { In = AppUserIds };
                List<long> StoresInScopedIds = await DataContext.AppUserStoreMapping.Where(m => m.AppUserId, SaleEmployeeIdFilter)
                .Select(s => s.StoreId).ToListWithNoLockAsync();
                AppUserDAO AppUserDAO = await DataContext.AppUser.Where(x => x.Id, SaleEmployeeIdFilter)
                .Include(x => x.Organization)
                .FirstOrDefaultWithNoLockAsync();
                var ActiveOrganizationQuery = DataContext.Organization.AsNoTracking();
                ActiveOrganizationQuery = ActiveOrganizationQuery.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });
                ActiveOrganizationQuery = ActiveOrganizationQuery.Where(x => x.Id, new IdFilter { In = OrganizationIds });
                var ActiveOrganizationIds = await ActiveOrganizationQuery.Select(x => x.Id).ToListWithNoLockAsync();
                if (StoresInScopedIds.Count > 0)
                {
                    var DraftStoreQuery = StoreQuery.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusEnum.DRAFT.Id });
                    DraftStoreQuery = DraftStoreQuery.Where(x => x.DeletedAt == null);
                    DraftStoreQuery = DraftStoreQuery.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });
                    DraftStoreQuery = DraftStoreQuery.Where(x => x.OrganizationId, new IdFilter { In = ActiveOrganizationIds });
                    DraftStoreQuery = DraftStoreQuery.Where(x => x.Organization.Path, new StringFilter { StartWith = AppUserDAO.Organization.Path });
                    List<long> DraftStoreIds = await DraftStoreQuery.Select(x => x.Id).ToListWithNoLockAsync();
                    StoresInScopedIds.AddRange(DraftStoreIds);
                    StoreIds = StoreIds.Intersect(StoresInScopedIds).Distinct().ToList();
                }
                else
                {
                    var AllStoreQuery = StoreQuery.Where(x => x.DeletedAt == null);
                    AllStoreQuery = AllStoreQuery.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });
                    AllStoreQuery = AllStoreQuery.Where(x => x.OrganizationId, new IdFilter { In = ActiveOrganizationIds });
                    AllStoreQuery = AllStoreQuery.Where(x => x.Organization.Path, new StringFilter { StartWith = AppUserDAO.Organization.Path });
                    var AllStoreIds = await AllStoreQuery.Select(x => x.Id).ToListWithNoLockAsync();
                    StoreIds = StoreIds.Intersect(AllStoreIds).Distinct().ToList();
                }
            } // Nếu filter theo nhân viên thì lấy theo phạm vi đi tuyến, nếu ko có lấy draft
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationFilter = new IdFilter { In = OrganizationIds };
            var StatusIdFilter = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            StoreQuery = StoreQuery.Where(x => x.Id, StoreIdFilter);
            StoreQuery = StoreQuery.Where(x => x.OrganizationId, OrganizationFilter);
            StoreQuery = StoreQuery.Where(x => x.StatusId, StatusIdFilter);

            var StoreCounter = await StoreQuery.CountWithNoLockAsync();
            return StoreCounter;
        }

        [Route(DashboardDirectorRoute.CountIndirectSalesOrder), HttpPost]
        public async Task<long> CountIndirectSalesOrder([FromBody] DashboardDirector_IndirectSalesOrderFluctuationFilterDTO DashboardDirector_IndirectSalesOrderFluctuationFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = new DateTime(Now.Year, Now.Month, 1).AddHours(0 - CurrentContext.TimeZone);
            DateTime End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddHours(0 - CurrentContext.TimeZone);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_IndirectSalesOrderFluctuationFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_IndirectSalesOrderFluctuationFilterDTO);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var OrderDateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            if (DashboardDirector_IndirectSalesOrderFluctuationFilterDTO.OrderDate.HasValue)
                OrderDateFilter = DashboardDirector_IndirectSalesOrderFluctuationFilterDTO.OrderDate;
            var RequestStateIdFilter = new IdFilter { Equal = RequestStateEnum.APPROVED.Id };

            var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, OrderDateFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, RequestStateIdFilter);

            return await IndirectSalesOrderQuery.CountWithNoLockAsync();
        }

        [Route(DashboardDirectorRoute.RevenueTotal), HttpPost]
        public async Task<decimal> RevenueTotal([FromBody] DashboardDirector_SaledItemFluctuationFilterDTO DashboardDirector_SaledItemFluctuationFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = new DateTime(Now.Year, Now.Month, 1).AddHours(0 - CurrentContext.TimeZone);
            DateTime End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddHours(0 - CurrentContext.TimeZone);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_SaledItemFluctuationFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_SaledItemFluctuationFilterDTO);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var OrderDateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            if (DashboardDirector_SaledItemFluctuationFilterDTO.OrderDate.HasValue)
                OrderDateFilter = DashboardDirector_SaledItemFluctuationFilterDTO.OrderDate;
            var RequestStateIdFilter = new IdFilter { Equal = RequestStateEnum.APPROVED.Id };

            var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, OrderDateFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, RequestStateIdFilter);

            var RevenueTotal = IndirectSalesOrderQuery.Select(x => x.Total).Sum();
            return RevenueTotal;
        }

        [Route(DashboardDirectorRoute.StoreHasCheckedCounter), HttpPost]
        public async Task<long> StoreHasCheckedCounter([FromBody] DashboardDirector_SaledItemFluctuationFilterDTO DashboardDirector_SaledItemFluctuationFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = new DateTime(Now.Year, Now.Month, 1).AddHours(0 - CurrentContext.TimeZone);
            DateTime End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddHours(0 - CurrentContext.TimeZone);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_SaledItemFluctuationFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_SaledItemFluctuationFilterDTO);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var OrderDateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            if (DashboardDirector_SaledItemFluctuationFilterDTO.OrderDate.HasValue)
                OrderDateFilter = DashboardDirector_SaledItemFluctuationFilterDTO.OrderDate;

            var StoreCheckingQuery = DWContext.Fact_StoreChecking.AsNoTracking();
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.StoreId, StoreIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.CheckOutAt, OrderDateFilter);

            var StoreHasCheckedCounter = await StoreCheckingQuery.Select(x => x.StoreId).Distinct().CountWithNoLockAsync();
            return StoreHasCheckedCounter;
        }

        [Route(DashboardDirectorRoute.CountStoreChecking), HttpPost]
        public async Task<long> CountStoreChecking([FromBody] DashboardDirector_StoreFilterDTO DashboardDirector_StoreFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = new DateTime(Now.Year, Now.Month, 1).AddHours(0 - CurrentContext.TimeZone);
            DateTime End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddHours(0 - CurrentContext.TimeZone);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_StoreFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_StoreFilterDTO);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var OrderDateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            if (DashboardDirector_StoreFilterDTO.OrderDate.HasValue)
                OrderDateFilter = DashboardDirector_StoreFilterDTO.OrderDate;

            var StoreCheckingQuery = DWContext.Fact_StoreChecking.AsNoTracking();
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.StoreId, StoreIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.CheckOutAt, OrderDateFilter);

            var StoreCheckingCounter = await StoreCheckingQuery.CountWithNoLockAsync();
            return StoreCheckingCounter;
        }
        [Route(DashboardDirectorRoute.StatisticToday), HttpPost]
        public async Task<DashboardDirector_StatisticDailyDTO> StatisticToday([FromBody] DashboardDirector_StoreFilterDTO DashboardDirector_StoreFilterDTO)
        {
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_StoreFilterDTO);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var StartFilter = new DateFilter { GreaterEqual = Start };
            var EndFilter = new DateFilter { LessEqual = End };
            var RequestStateIdFilter = new IdFilter { Equal = RequestStateEnum.APPROVED.Id };

            var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, StartFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, EndFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, RequestStateIdFilter);

            var StoreCheckingQuery = DWContext.Fact_StoreChecking.AsNoTracking();
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.StoreId, StoreIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.CheckOutAt, StartFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.CheckOutAt, EndFilter);

            var RevenueTotal = await IndirectSalesOrderQuery.Select(x => x.Total).SumAsync();
            var IndirectSalesOrderCounter = await IndirectSalesOrderQuery.CountWithNoLockAsync();
            var StoreHasCheckedCounter = await StoreCheckingQuery.Select(x => x.StoreId).Distinct().CountWithNoLockAsync();
            var StoreCheckingCounter = await StoreCheckingQuery.CountWithNoLockAsync();

            DashboardDirector_StatisticDailyDTO DashboardDirector_StatisticDailyDTO = new DashboardDirector_StatisticDailyDTO()
            {
                Revenue = RevenueTotal,
                IndirectSalesOrderCounter = IndirectSalesOrderCounter,
                StoreHasCheckedCounter = StoreHasCheckedCounter,
                StoreCheckingCounter = StoreCheckingCounter
            };
            return DashboardDirector_StatisticDailyDTO;
        }

        [Route(DashboardDirectorRoute.StatisticYesterday), HttpPost]
        public async Task<DashboardDirector_StatisticDailyDTO> StatisticYesterday([FromBody] DashboardDirector_IndirectSalesOrderFluctuationFilterDTO DashboardDirector_IndirectSalesOrderFluctuationFilterDTO)
        {
            DateTime Start = LocalStartDay(CurrentContext).AddDays(-1);
            DateTime End = LocalEndDay(CurrentContext).AddDays(-1);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_IndirectSalesOrderFluctuationFilterDTO);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var StartFilter = new DateFilter { GreaterEqual = Start };
            var EndFilter = new DateFilter { LessEqual = End };
            var RequestStateIdFilter = new IdFilter { Equal = RequestStateEnum.APPROVED.Id };

            var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, StartFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, EndFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, RequestStateIdFilter);

            var StoreCheckingQuery = DWContext.Fact_StoreChecking.AsNoTracking();
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.StoreId, StoreIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.CheckOutAt, StartFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.CheckOutAt, EndFilter);

            var RevenueTotal = await IndirectSalesOrderQuery.Select(x => x.Total).SumAsync();
            var IndirectSalesOrderCounter = await IndirectSalesOrderQuery.CountWithNoLockAsync();
            var StoreHasCheckedCounter = await StoreCheckingQuery.Select(x => x.StoreId).Distinct().CountWithNoLockAsync();
            var StoreCheckingCounter = await StoreCheckingQuery.CountWithNoLockAsync();

            DashboardDirector_StatisticDailyDTO DashboardDirector_StatisticDailyDTO = new DashboardDirector_StatisticDailyDTO()
            {
                Revenue = RevenueTotal,
                IndirectSalesOrderCounter = IndirectSalesOrderCounter,
                StoreHasCheckedCounter = StoreHasCheckedCounter,
                StoreCheckingCounter = StoreCheckingCounter
            };
            return DashboardDirector_StatisticDailyDTO;
        }

        [Route(DashboardDirectorRoute.StoreCoverage), HttpPost]
        public async Task<List<DashboardDirector_StoreDTO>> StoreCoverage([FromBody] DashboardDirector_StoreFilterDTO DashboardDirector_StoreFilterDTO)
        {
            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_StoreFilterDTO);

            var StoreQuery = DataContext.Store.AsNoTracking();
            if (DashboardDirector_StoreFilterDTO.AppUserId.Equal.HasValue)
            {
                long? SaleEmployeeId = DashboardDirector_StoreFilterDTO.AppUserId?.Equal;
                AppUserIds = AppUserIds.Where(x => x == SaleEmployeeId.Value).ToList();
                var SaleEmployeeIdFilter = new IdFilter { In = AppUserIds };
                List<long> StoresInScopedIds = await DataContext.AppUserStoreMapping.Where(m => m.AppUserId, SaleEmployeeIdFilter)
                .Select(s => s.StoreId).ToListWithNoLockAsync();
                AppUserDAO AppUserDAO = await DataContext.AppUser.Where(x => x.Id, SaleEmployeeIdFilter)
                .Include(x => x.Organization)
                .FirstOrDefaultWithNoLockAsync();
                var ActiveOrganizationQuery = DataContext.Organization.AsNoTracking();
                ActiveOrganizationQuery = ActiveOrganizationQuery.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });
                ActiveOrganizationQuery = ActiveOrganizationQuery.Where(x => x.Id, new IdFilter { In = OrganizationIds });
                var ActiveOrganizationIds = await ActiveOrganizationQuery.Select(x => x.Id).ToListWithNoLockAsync();
                if (StoresInScopedIds.Count > 0)
                {
                    var DraftStoreQuery = StoreQuery.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusEnum.DRAFT.Id });
                    DraftStoreQuery = DraftStoreQuery.Where(x => x.DeletedAt == null);
                    DraftStoreQuery = DraftStoreQuery.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });
                    DraftStoreQuery = DraftStoreQuery.Where(x => x.OrganizationId, new IdFilter { In = ActiveOrganizationIds });
                    DraftStoreQuery = DraftStoreQuery.Where(x => x.Organization.Path, new StringFilter { StartWith = AppUserDAO.Organization.Path });
                    List<long> DraftStoreIds = await DraftStoreQuery.Select(x => x.Id).ToListWithNoLockAsync();
                    StoresInScopedIds.AddRange(DraftStoreIds);
                    StoreIds = StoreIds.Intersect(StoresInScopedIds).Distinct().ToList();
                }
                else
                {
                    var AllStoreQuery = StoreQuery.Where(x => x.DeletedAt == null);
                    AllStoreQuery = AllStoreQuery.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });
                    AllStoreQuery = AllStoreQuery.Where(x => x.OrganizationId, new IdFilter { In = ActiveOrganizationIds });
                    AllStoreQuery = AllStoreQuery.Where(x => x.Organization.Path, new StringFilter { StartWith = AppUserDAO.Organization.Path });
                    var AllStoreIds = await AllStoreQuery.Select(x => x.Id).ToListWithNoLockAsync();
                    StoreIds = StoreIds.Intersect(AllStoreIds).Distinct().ToList();
                }
            } // Nếu filter theo nhân viên thì lấy theo phạm vi đi tuyến, nếu ko có lấy draft
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };

            StoreQuery = StoreQuery.Where(x => x.Id, StoreIdFilter);
            StoreQuery = StoreQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            List<DashboardDirector_StoreDTO> DashboardMonitor_StoreDTOs = await StoreQuery.Select(s => new DashboardDirector_StoreDTO
            {
                Id = s.Id,
                Name = s.Name,
                Address = s.Address,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                Telephone = s.OwnerPhone,
                StoreStatusId = s.StoreStatusId,
                IsScouting = false
            }).Distinct().ToListWithNoLockAsync();

            //Add Store Scouting
            var StoreScoutingQuery = DWContext.Fact_StoreScouting.AsNoTracking();
            var StoreScoutingIds = await StoreScoutingQuery.Select(ss => ss.StoreScoutingId).ToListWithNoLockAsync();
            var StoreScoutingdIdFilter = new IdFilter { In = StoreScoutingIds };
            var StoreScoutingStatusIdFilter = new IdFilter { Equal = StoreScoutingStatusEnum.NOTOPEN.Id };
            var ProvinceIdFilter = new IdFilter();
            var AppUserIdFilter = new IdFilter();
            if (DashboardDirector_StoreFilterDTO.ProvinceId.HasValue) ProvinceIdFilter.Equal = DashboardDirector_StoreFilterDTO.ProvinceId.Equal;
            if (DashboardDirector_StoreFilterDTO.AppUserId.HasValue) AppUserIdFilter.Equal = DashboardDirector_StoreFilterDTO.AppUserId.Equal;
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.StoreScoutingId, StoreScoutingdIdFilter);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.ProvinceId, ProvinceIdFilter);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.CreatorId, AppUserIdFilter);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.StoreScoutingStatusId, StoreScoutingStatusIdFilter);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.DeletedAt == null);
            List<DashboardDirector_StoreDTO> DashboardMonitor_StoreScoutingDTOs = await StoreScoutingQuery.Select(ss => new DashboardDirector_StoreDTO
            {
                Id = ss.StoreScoutingId,
                Name = ss.Name,
                Address = ss.Address,
                Latitude = ss.Latitude,
                Longitude = ss.Longitude,
                Telephone = ss.OwnerPhone,
                IsScouting = true
            }).Distinct().ToListWithNoLockAsync();

            DashboardMonitor_StoreDTOs.AddRange(DashboardMonitor_StoreScoutingDTOs);
            return DashboardMonitor_StoreDTOs;
        }

        [Route(DashboardDirectorRoute.SaleEmployeeLocation), HttpPost]
        public async Task<List<DashboardDirector_AppUserDTO>> SaleEmployeeLocation([FromBody] DashboardDirector_StoreFilterDTO DashboardDirector_StoreFilterDTO)
        {
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_StoreFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_StoreFilterDTO);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var StartFilter = new DateFilter { GreaterEqual = Start };
            var EndFilter = new DateFilter { LessEqual = End };
            var AppUserGpsQuery = DataContext.AppUserGps.AsNoTracking();
            AppUserGpsQuery = AppUserGpsQuery.Where(x => x.GPSUpdatedAt, StartFilter);
            AppUserGpsQuery = AppUserGpsQuery.Where(x => x.GPSUpdatedAt, EndFilter);
            var AppUserGpsIds = await AppUserGpsQuery.Select(x => x.AppUserId).Distinct().ToListWithNoLockAsync();
            var AppUserGpsIdFilter = new IdFilter { In = AppUserGpsIds };

            var AppUserQuery = DWContext.Dim_AppUser.AsNoTracking();
            AppUserQuery = AppUserQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            AppUserQuery = AppUserQuery.Where(x => x.AppUserId, AppUserGpsIdFilter);
            List<DashboardDirector_AppUserDTO> DashboardDirector_AppUserDTOs = await AppUserQuery.Select(au => new DashboardDirector_AppUserDTO
            {
                Id = au.AppUserId,
                DisplayName = au.DisplayName,
                Username = au.Username,
                Phone = au.Phone,
            }).Distinct().ToListWithNoLockAsync();

            List<AppUserGpsDAO> AppUserGpsDAOs = await DataContext.AppUserGps.AsNoTracking()
                .Where(x => x.AppUserId, AppUserIdFilter)
                .ToListWithNoLockAsync();
            foreach (DashboardDirector_AppUserDTO DashboardDirector_AppUserDTO in DashboardDirector_AppUserDTOs)
            {
                AppUserGpsDAO AppUserGpsDAO = AppUserGpsDAOs.Where(x => x.AppUserId == DashboardDirector_AppUserDTO.Id)
                    .OrderByDescending(x => x.GPSUpdatedAt).FirstOrDefault();
                if (AppUserGpsDAO != null)
                {
                    DashboardDirector_AppUserDTO.Longitude = AppUserGpsDAO.Longitude;
                    DashboardDirector_AppUserDTO.Latitude = AppUserGpsDAO.Latitude;
                }
            }
            return DashboardDirector_AppUserDTOs;
        }

        [Route(DashboardDirectorRoute.ListIndirectSalesOrder), HttpPost]
        public async Task<List<DashboardDirector_IndirectSalesOrderDTO>> ListIndirectSalesOrder([FromBody] DashboardDirector_IndirectSalesOrderFluctuationFilterDTO DashboardDirector_IndirectSalesOrderFluctuationFilterDTO)
        {
            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_IndirectSalesOrderFluctuationFilterDTO);

            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var RequestStateIdFilter = new IdFilter { NotEqual = RequestStateEnum.NEW.Id };
            var appUser = await AppUserService.Get(CurrentContext.UserId);

            var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, RequestStateIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.OrderByDescending(x => x.OrderDate);
            List<DashboardDirector_IndirectSalesOrderDTO> DashboardUser_IndirectSalesOrderDTOs = await IndirectSalesOrderQuery.Select(i => new DashboardDirector_IndirectSalesOrderDTO
            {
                Id = i.IndirectSalesOrderId,
                Code = i.Code,
                OrderDate = i.OrderDate,
                RequestStateId = i.RequestStateId,
                SaleEmployeeId = i.SaleEmployeeId,
                Total = i.Total
            }).Skip(0).Take(10).ToListWithNoLockAsync();

            List<AppUserDAO> AppUserDAOs = await DataContext.AppUser.Where(x => x.Id, AppUserIdFilter).ToListWithNoLockAsync();
            RequestStateDAO RequestStateDAO = await DataContext.RequestState.Where(x => x.Id, RequestStateIdFilter).FirstOrDefaultWithNoLockAsync();

            foreach (DashboardDirector_IndirectSalesOrderDTO DashboardUser_IndirectSalesOrderDTO in DashboardUser_IndirectSalesOrderDTOs)
            {
                AppUserDAO AppUserDAO = AppUserDAOs.Where(x => x.Id == DashboardUser_IndirectSalesOrderDTO.SaleEmployeeId).FirstOrDefault();
                if (AppUserDAO != null)
                {
                    DashboardUser_IndirectSalesOrderDTO.SaleEmployee = new DashboardDirector_AppUserDTO
                    {
                        Id = AppUserDAO.Id,
                        DisplayName = AppUserDAO.DisplayName,
                        Username = AppUserDAO.Username
                    };
                }
                if (RequestStateDAO != null)
                {
                    DashboardUser_IndirectSalesOrderDTO.RequestState = new DashboardDirector_RequestStateDTO
                    {
                        Id = RequestStateDAO.Id,
                        Code = RequestStateDAO.Code,
                        Name = RequestStateDAO.Name
                    };
                }

            }
            return DashboardUser_IndirectSalesOrderDTOs;
        }

        [Route(DashboardDirectorRoute.Top5RevenueByProduct), HttpPost]
        public async Task<List<DashboardDirector_Top5RevenueByProductDTO>> Top5RevenueByProduct([FromBody] DashboardDirector_Top5RevenueByProductFilterDTO DashboardDirector_Top5RevenueByProductFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_Top5RevenueByProductFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_Top5RevenueByProductFilterDTO);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var OrderDateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            if (DashboardDirector_Top5RevenueByProductFilterDTO.OrderDate.HasValue)
                OrderDateFilter = DashboardDirector_Top5RevenueByProductFilterDTO.OrderDate;
            var RequestStateIdFilter = new IdFilter { Equal = RequestStateEnum.APPROVED.Id };

            var IndirectSalesOrderTransactionQuery = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.SalesEmployeeId, AppUserIdFilter);
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.RequestStateId, RequestStateIdFilter);
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, OrderDateFilter);
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => !x.DeletedAt.HasValue);
            var RevenueByItemIds = await IndirectSalesOrderTransactionQuery
                .GroupBy(x => x.ItemId)
                .Select(x => new
                {
                    ItemId = x.Key,
                    Revenue = (decimal)x.Sum(y => y.Amount) - x.Sum(y => y.GeneralDiscountAmount)
                }).Distinct().ToListWithNoLockAsync();

            var RevenueByProductIds = RevenueByItemIds.Select(x => new
            {
                ProductId = DataContext.Item.Where(i => i.Id == x.ItemId).FirstOrDefault(),
                Revenue = x.Revenue
            }); // convert ItemId to ProductId
            var RevenueByProduct = RevenueByProductIds.GroupBy(x => x.ProductId).Select(x => new DashboardDirector_Top5RevenueByProductDTO
            {
                ProductId = x.Key.ProductId,
                Revenue = (decimal)x.Sum(s => s.Revenue)
            });

            List<DashboardDirector_Top5RevenueByProductDTO> DashboardDirector_Top5RevenueByProductDTOs = RevenueByProduct.OrderByDescending(x => x.Revenue)
                .Skip(0).Take(5).ToList();
            foreach (var DashboardDirector_Top5RevenueByProductDTO in DashboardDirector_Top5RevenueByProductDTOs)
            {
                DashboardDirector_Top5RevenueByProductDTO.ProductName = DataContext.Product.Where(p => p.Id == DashboardDirector_Top5RevenueByProductDTO.ProductId)
                    .Select(p => p.Name).FirstOrDefault();
            } // Filter ProductName by ProductId
            return DashboardDirector_Top5RevenueByProductDTOs;
        }

        [Route(DashboardDirectorRoute.Top5RevenueByEmployee), HttpPost]
        public async Task<List<DashboardDirector_Top5RevenueByEmployeeDTO>> Top5RevenueByEmployee([FromBody] DashboardDirector_Top5RevenueByEmployeeFilterDTO DashboardDirector_Top5RevenueByEmployeeFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_Top5RevenueByEmployeeFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_Top5RevenueByEmployeeFilterDTO);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var OrderDateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            if (DashboardDirector_Top5RevenueByEmployeeFilterDTO.OrderDate.HasValue)
                OrderDateFilter = DashboardDirector_Top5RevenueByEmployeeFilterDTO.OrderDate;
            var RequestStateIdFilter = new IdFilter { Equal = RequestStateEnum.APPROVED.Id };

            var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, RequestStateIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, OrderDateFilter);
            List<DashboardDirector_Top5RevenueByEmployeeDTO> DashboardDirector_Top5RevenueByEmployeeDTOs = await IndirectSalesOrderQuery
                .GroupBy(x => x.SaleEmployeeId)
                .Select(x => new DashboardDirector_Top5RevenueByEmployeeDTO
                {
                    EmployeeId = x.Key,
                    Revenue = (decimal)x.Sum(y => y.Total)
                }).Distinct().OrderByDescending(x => x.Revenue).Skip(0).Take(5).ToListWithNoLockAsync();

            foreach (var DashboardDirector_Top5RevenueByEmployeeDTO in DashboardDirector_Top5RevenueByEmployeeDTOs)
            {
                DashboardDirector_Top5RevenueByEmployeeDTO.EmployeeName = DataContext.AppUser.Where(a => a.Id == DashboardDirector_Top5RevenueByEmployeeDTO.EmployeeId)
                    .Select(a => a.DisplayName).FirstOrDefault();
            }
            return DashboardDirector_Top5RevenueByEmployeeDTOs;
        }

        [Route(DashboardDirectorRoute.RevenueFluctuation), HttpPost]
        public async Task<DashboardDirector_RevenueFluctuationDTO> RevenueFluctuation([FromBody] DashboardDirector_RevenueFluctuationFilterDTO DashboardDirector_RevenueFluctuationFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_RevenueFluctuationFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_RevenueFluctuationFilterDTO);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var StartFilter = new DateFilter { GreaterEqual = Start };
            var EndFilter = new DateFilter { LessEqual = End };
            var RequestStateIdFilter = new IdFilter { Equal = RequestStateEnum.APPROVED.Id };

            var IndirectSalesOrderTransactionQuery = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.SalesEmployeeId, AppUserIdFilter);
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.RequestStateId, RequestStateIdFilter);
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, StartFilter);
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, EndFilter);
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => !x.DeletedAt.HasValue);
            var IndirectSalesOrderTransactionDAOs = await IndirectSalesOrderTransactionQuery
                .Select(x => new IndirectSalesOrderTransactionDAO
                {
                    OrderDate = x.OrderDate,
                    Revenue = x.Amount - x.GeneralDiscountAmount
                }).ToListWithNoLockAsync();

            DashboardDirector_RevenueFluctuationDTO DashboardDirector_RevenueFluctuationDTO = new DashboardDirector_RevenueFluctuationDTO();

            if (DashboardDirector_RevenueFluctuationFilterDTO.Time.Equal.HasValue == false
                || DashboardDirector_RevenueFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.THIS_MONTH.Id)
            {
                DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths = new List<DashboardDirector_RevenueFluctuationByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = 1; i < number_of_day_in_this_month + 1; i++)
                {
                    DashboardDirector_RevenueFluctuationByMonthDTO RevenueFluctuationByMonth = new DashboardDirector_RevenueFluctuationByMonthDTO
                    {
                        Day = i,
                        Revenue = 0
                    };
                    DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths.Add(RevenueFluctuationByMonth);
                }

                foreach (var RevenueFluctuationByMonth in DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths)
                {
                    DateTime LocalStart = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, (int)RevenueFluctuationByMonth.Day).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddDays(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueFluctuationByMonth.Revenue = IndirectSalesOrderTransactionDAOs.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Where(x => x.Revenue.HasValue)
                        .Select(x => x.Revenue.Value)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return DashboardDirector_RevenueFluctuationDTO;
            }
            else if (DashboardDirector_RevenueFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.LAST_MONTH.Id)
            {
                DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths = new List<DashboardDirector_RevenueFluctuationByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = 1; i < number_of_day_in_this_month + 1; i++)
                {
                    DashboardDirector_RevenueFluctuationByMonthDTO RevenueFluctuationByMonth = new DashboardDirector_RevenueFluctuationByMonthDTO
                    {
                        Day = i,
                        Revenue = 0
                    };
                    DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths.Add(RevenueFluctuationByMonth);
                }

                foreach (var RevenueFluctuationByMonth in DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths)
                {
                    DateTime LocalStart = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).AddMonths(-1).Month, (int)RevenueFluctuationByMonth.Day).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddDays(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueFluctuationByMonth.Revenue = IndirectSalesOrderTransactionDAOs.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Where(x => x.Revenue.HasValue)
                        .Select(x => x.Revenue.Value)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return DashboardDirector_RevenueFluctuationDTO;
            }
            else if (DashboardDirector_RevenueFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.THIS_QUARTER.Id)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.AddHours(CurrentContext.TimeZone).Month / 3m));

                DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters = new List<DashboardDirector_RevenueFluctuationByQuarterDTO>();
                int start = 3 * (this_quarter - 1) + 1;
                int end = start + 3;
                for (int i = start; i < end; i++)
                {
                    DashboardDirector_RevenueFluctuationByQuarterDTO RevenueFluctuationByQuarter = new DashboardDirector_RevenueFluctuationByQuarterDTO
                    {
                        Month = i,
                        Revenue = 0
                    };
                    DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters.Add(RevenueFluctuationByQuarter);
                }

                foreach (var RevenueFluctuationByQuarter in DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters)
                {
                    DateTime LocalStart = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, (int)RevenueFluctuationByQuarter.Month, 1).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueFluctuationByQuarter.Revenue = IndirectSalesOrderTransactionDAOs.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Where(x => x.Revenue.HasValue)
                        .Select(x => x.Revenue.Value)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return DashboardDirector_RevenueFluctuationDTO;
            }
            else if (DashboardDirector_RevenueFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.LAST_QUATER.Id)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.AddHours(CurrentContext.TimeZone).Month / 3m));
                var last_quarter = (this_quarter + 3) % 4;

                DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters = new List<DashboardDirector_RevenueFluctuationByQuarterDTO>();
                int start = 3 * (last_quarter - 1) + 1;
                int end = start + 3;
                for (int i = start; i < end; i++)
                {
                    DashboardDirector_RevenueFluctuationByQuarterDTO RevenueFluctuationByQuarter = new DashboardDirector_RevenueFluctuationByQuarterDTO
                    {
                        Month = i,
                        Revenue = 0
                    };
                    DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters.Add(RevenueFluctuationByQuarter);
                }

                foreach (var RevenueFluctuationByQuarter in DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters)
                {
                    DateTime LocalStart = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, (int)RevenueFluctuationByQuarter.Month, 1).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueFluctuationByQuarter.Revenue = IndirectSalesOrderTransactionDAOs.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Where(x => x.Revenue.HasValue)
                        .Select(x => x.Revenue.Value)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return DashboardDirector_RevenueFluctuationDTO;
            }
            else if (DashboardDirector_RevenueFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.YEAR.Id)
            {

                DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByYears = new List<DashboardDirector_RevenueFluctuationByYearDTO>();
                for (int i = 1; i <= 12; i++)
                {
                    DashboardDirector_RevenueFluctuationByYearDTO RevenueFluctuationByYear = new DashboardDirector_RevenueFluctuationByYearDTO
                    {
                        Month = i,
                        Revenue = 0
                    };
                    DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByYears.Add(RevenueFluctuationByYear);
                }

                foreach (var RevenueFluctuationByYear in DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByYears)
                {
                    DateTime LocalStart = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, (int)RevenueFluctuationByYear.Month, 1).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueFluctuationByYear.Revenue = IndirectSalesOrderTransactionDAOs.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Where(x => x.Revenue.HasValue)
                        .Select(x => x.Revenue.Value)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return DashboardDirector_RevenueFluctuationDTO;
            }
            return new DashboardDirector_RevenueFluctuationDTO();
        }

        [Route(DashboardDirectorRoute.IndirectSalesOrderFluctuation), HttpPost]
        public async Task<DashboardDirector_IndirectSalesOrderFluctuationDTO> IndirectSalesOrderFluctuation([FromBody] DashboardDirector_IndirectSalesOrderFluctuationFilterDTO DashboardDirector_IndirectSalesOrderFluctuationFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_IndirectSalesOrderFluctuationFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_IndirectSalesOrderFluctuationFilterDTO);

            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var StartFilter = new DateFilter { GreaterEqual = Start };
            var EndFilter = new DateFilter { LessEqual = End };
            var RequestStateIdFilter = new IdFilter { Equal = RequestStateEnum.APPROVED.Id };

            if (DashboardDirector_IndirectSalesOrderFluctuationFilterDTO.Time.Equal.HasValue == false
                || DashboardDirector_IndirectSalesOrderFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.THIS_MONTH.Id)
            {
                var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, StartFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, EndFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, RequestStateIdFilter);
                var DashboardDirector_IndirectSalesOrderFluctuationByMonthDTOs = await IndirectSalesOrderQuery
                    .GroupBy(x => x.OrderDate.Day)
                    .Select(x => new DashboardDirector_IndirectSalesOrderFluctuationByMonthDTO
                    {
                        Day = x.Key,
                        IndirectSalesOrderCounter = x.Count()
                    })
                    .ToListWithNoLockAsync();

                DashboardDirector_IndirectSalesOrderFluctuationDTO DashboardDirector_IndirectSalesOrderFluctuationDTO = new DashboardDirector_IndirectSalesOrderFluctuationDTO();
                DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByMonths = new List<DashboardDirector_IndirectSalesOrderFluctuationByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = 1; i < number_of_day_in_this_month + 1; i++)
                {
                    DashboardDirector_IndirectSalesOrderFluctuationByMonthDTO IndirectSalesOrderFluctuationByMonth = new DashboardDirector_IndirectSalesOrderFluctuationByMonthDTO
                    {
                        Day = i,
                        IndirectSalesOrderCounter = 0
                    };
                    DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByMonths.Add(IndirectSalesOrderFluctuationByMonth);
                }

                foreach (var IndirectSalesOrderFluctuationByMonth in DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByMonths)
                {
                    var data = DashboardDirector_IndirectSalesOrderFluctuationByMonthDTOs.Where(x => x.Day == IndirectSalesOrderFluctuationByMonth.Day).FirstOrDefault();
                    if (data != null)
                        IndirectSalesOrderFluctuationByMonth.IndirectSalesOrderCounter = data.IndirectSalesOrderCounter;
                }

                return DashboardDirector_IndirectSalesOrderFluctuationDTO;
            }
            else if (DashboardDirector_IndirectSalesOrderFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.LAST_MONTH.Id)
            {

                var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, StartFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, EndFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, RequestStateIdFilter);
                var DashboardDirector_IndirectSalesOrderFluctuationByMonthDTOs = await IndirectSalesOrderQuery
                    .GroupBy(x => x.OrderDate.Day)
                    .Select(x => new DashboardDirector_IndirectSalesOrderFluctuationByMonthDTO
                    {
                        Day = x.Key,
                        IndirectSalesOrderCounter = x.Count()
                    })
                    .ToListWithNoLockAsync();

                DashboardDirector_IndirectSalesOrderFluctuationDTO DashboardDirector_IndirectSalesOrderFluctuationDTO = new DashboardDirector_IndirectSalesOrderFluctuationDTO();
                DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByMonths = new List<DashboardDirector_IndirectSalesOrderFluctuationByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = 1; i < number_of_day_in_this_month + 1; i++)
                {
                    DashboardDirector_IndirectSalesOrderFluctuationByMonthDTO IndirectSalesOrderFluctuationByMonth = new DashboardDirector_IndirectSalesOrderFluctuationByMonthDTO
                    {
                        Day = i,
                        IndirectSalesOrderCounter = 0
                    };
                    DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByMonths.Add(IndirectSalesOrderFluctuationByMonth);
                }

                foreach (var IndirectSalesOrderFluctuationByMonth in DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByMonths)
                {
                    var data = DashboardDirector_IndirectSalesOrderFluctuationByMonthDTOs.Where(x => x.Day == IndirectSalesOrderFluctuationByMonth.Day).FirstOrDefault();
                    if (data != null)
                        IndirectSalesOrderFluctuationByMonth.IndirectSalesOrderCounter = data.IndirectSalesOrderCounter;
                }

                return DashboardDirector_IndirectSalesOrderFluctuationDTO;
            }
            else if (DashboardDirector_IndirectSalesOrderFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.THIS_QUARTER.Id)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.AddHours(CurrentContext.TimeZone).Month / 3m));

                var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, StartFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, EndFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, RequestStateIdFilter);
                var DashboardDirector_IndirectSalesOrderFluctuationByQuarterDTOs = await IndirectSalesOrderQuery
                    .GroupBy(x => x.OrderDate.Month)
                    .Select(x => new DashboardDirector_IndirectSalesOrderFluctuationByQuarterDTO
                    {
                        Month = x.Key,
                        IndirectSalesOrderCounter = x.Count()
                    })
                    .ToListWithNoLockAsync();

                DashboardDirector_IndirectSalesOrderFluctuationDTO DashboardDirector_IndirectSalesOrderFluctuationDTO = new DashboardDirector_IndirectSalesOrderFluctuationDTO();
                DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByQuaters = new List<DashboardDirector_IndirectSalesOrderFluctuationByQuarterDTO>();
                int start = 3 * (this_quarter - 1) + 1;
                int end = start + 3;
                for (int i = start; i < end; i++)
                {
                    DashboardDirector_IndirectSalesOrderFluctuationByQuarterDTO IndirectSalesOrderFluctuationByQuarter = new DashboardDirector_IndirectSalesOrderFluctuationByQuarterDTO
                    {
                        Month = i,
                        IndirectSalesOrderCounter = 0
                    };
                    DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByQuaters.Add(IndirectSalesOrderFluctuationByQuarter);
                }

                foreach (var IndirectSalesOrderFluctuationByQuater in DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByQuaters)
                {
                    var data = DashboardDirector_IndirectSalesOrderFluctuationByQuarterDTOs.Where(x => x.Month == IndirectSalesOrderFluctuationByQuater.Month).FirstOrDefault();
                    if (data != null)
                        IndirectSalesOrderFluctuationByQuater.IndirectSalesOrderCounter = data.IndirectSalesOrderCounter;
                }

                return DashboardDirector_IndirectSalesOrderFluctuationDTO;
            }
            else if (DashboardDirector_IndirectSalesOrderFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.LAST_QUATER.Id)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.AddHours(CurrentContext.TimeZone).Month / 3m));
                var last_quarter = (this_quarter + 3) % 4;

                var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, StartFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, EndFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, RequestStateIdFilter);
                var DashboardDirector_IndirectSalesOrderFluctuationByQuarterDTOs = await IndirectSalesOrderQuery
                    .GroupBy(x => x.OrderDate.Month)
                    .Select(x => new DashboardDirector_IndirectSalesOrderFluctuationByQuarterDTO
                    {
                        Month = x.Key,
                        IndirectSalesOrderCounter = x.Count()
                    })
                    .ToListWithNoLockAsync();

                DashboardDirector_IndirectSalesOrderFluctuationDTO DashboardDirector_IndirectSalesOrderFluctuationDTO = new DashboardDirector_IndirectSalesOrderFluctuationDTO();
                DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByQuaters = new List<DashboardDirector_IndirectSalesOrderFluctuationByQuarterDTO>();
                int start = 3 * (last_quarter - 1) + 1;
                int end = start + 3;
                for (int i = start; i < end; i++)
                {
                    DashboardDirector_IndirectSalesOrderFluctuationByQuarterDTO IndirectSalesOrderFluctuationByQuarter = new DashboardDirector_IndirectSalesOrderFluctuationByQuarterDTO
                    {
                        Month = i,
                        IndirectSalesOrderCounter = 0
                    };
                    DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByQuaters.Add(IndirectSalesOrderFluctuationByQuarter);
                }

                foreach (var IndirectSalesOrderFluctuationByQuater in DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByQuaters)
                {
                    var data = DashboardDirector_IndirectSalesOrderFluctuationByQuarterDTOs.Where(x => x.Month == IndirectSalesOrderFluctuationByQuater.Month).FirstOrDefault();
                    if (data != null)
                        IndirectSalesOrderFluctuationByQuater.IndirectSalesOrderCounter = data.IndirectSalesOrderCounter;
                }

                return DashboardDirector_IndirectSalesOrderFluctuationDTO;
            }
            else if (DashboardDirector_IndirectSalesOrderFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.YEAR.Id)
            {

                var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, StartFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, EndFilter);
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, RequestStateIdFilter);
                var DashboardDirector_IndirectSalesOrderFluctuationByYearDTOs = await IndirectSalesOrderQuery
                    .GroupBy(x => x.OrderDate.Month)
                    .Select(x => new DashboardDirector_IndirectSalesOrderFluctuationByYearDTO
                    {
                        Month = x.Key,
                        IndirectSalesOrderCounter = x.Count()
                    })
                    .ToListWithNoLockAsync();

                DashboardDirector_IndirectSalesOrderFluctuationDTO DashboardDirector_IndirectSalesOrderFluctuationDTO = new DashboardDirector_IndirectSalesOrderFluctuationDTO();
                DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByYears = new List<DashboardDirector_IndirectSalesOrderFluctuationByYearDTO>();
                for (int i = 1; i <= 12; i++)
                {
                    DashboardDirector_IndirectSalesOrderFluctuationByYearDTO IndirectSalesOrderFluctuationByYear = new DashboardDirector_IndirectSalesOrderFluctuationByYearDTO
                    {
                        Month = i,
                        IndirectSalesOrderCounter = 0
                    };
                    DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByYears.Add(IndirectSalesOrderFluctuationByYear);
                }

                foreach (var IndirectSalesOrderFluctuationByYear in DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByYears)
                {
                    var data = DashboardDirector_IndirectSalesOrderFluctuationByYearDTOs.Where(x => x.Month == IndirectSalesOrderFluctuationByYear.Month).FirstOrDefault();
                    if (data != null)
                        IndirectSalesOrderFluctuationByYear.IndirectSalesOrderCounter = data.IndirectSalesOrderCounter;
                }

                return DashboardDirector_IndirectSalesOrderFluctuationDTO;
            }
            return new DashboardDirector_IndirectSalesOrderFluctuationDTO();
        }

        [Route(DashboardDirectorRoute.SaledItemFluctuation), HttpPost]
        public async Task<DashboardDirector_SaledItemFluctuationDTO> SaledItemFluctuation([FromBody] DashboardDirector_SaledItemFluctuationFilterDTO DashboardDirector_SaledItemFluctuationFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_SaledItemFluctuationFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_SaledItemFluctuationFilterDTO);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var StartFilter = new DateFilter { GreaterEqual = Start };
            var EndFilter = new DateFilter { LessEqual = End };
            var RequestStateIdFilter = new IdFilter { Equal = RequestStateEnum.APPROVED.Id };

            if (DashboardDirector_SaledItemFluctuationFilterDTO.Time.Equal.HasValue == false
                || DashboardDirector_SaledItemFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.THIS_MONTH.Id)
            {
                var IndirectSalesOrderTransactionQuery = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.SalesEmployeeId, AppUserIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, StartFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, EndFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.RequestStateId, RequestStateIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => !x.DeletedAt.HasValue);
                var DashboardDirector_SaledItemFluctuationByMonthDTOs = await IndirectSalesOrderTransactionQuery
                    .GroupBy(x => x.OrderDate.Day)
                    .Select(x => new DashboardDirector_SaledItemFluctuationByMonthDTO
                    {
                        Day = x.Key,
                        SaledItemCounter = (decimal)x.Sum(x => x.RequestedQuantity)
                    })
                    .ToListWithNoLockAsync();
                DashboardDirector_SaledItemFluctuationDTO DashboardDirector_SaledItemFluctuationDTO = new DashboardDirector_SaledItemFluctuationDTO();
                DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByMonths = new List<DashboardDirector_SaledItemFluctuationByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = 1; i < number_of_day_in_this_month + 1; i++)
                {
                    DashboardDirector_SaledItemFluctuationByMonthDTO SaledItemFluctuationByMonth = new DashboardDirector_SaledItemFluctuationByMonthDTO
                    {
                        Day = i,
                        SaledItemCounter = 0
                    };
                    DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByMonths.Add(SaledItemFluctuationByMonth);
                }

                foreach (var SaledItemFluctuationByMonth in DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByMonths)
                {
                    var data = DashboardDirector_SaledItemFluctuationByMonthDTOs.Where(x => x.Day == SaledItemFluctuationByMonth.Day).FirstOrDefault();
                    if (data != null)
                        SaledItemFluctuationByMonth.SaledItemCounter = data.SaledItemCounter;
                }

                return DashboardDirector_SaledItemFluctuationDTO;
            }
            else if (DashboardDirector_SaledItemFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.LAST_MONTH.Id)
            {
                var IndirectSalesOrderTransactionQuery = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.SalesEmployeeId, AppUserIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, StartFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, EndFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.RequestStateId, RequestStateIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => !x.DeletedAt.HasValue);
                var DashboardDirector_SaledItemFluctuationByMonthDTOs = await IndirectSalesOrderTransactionQuery
                    .GroupBy(x => x.OrderDate.Day)
                    .Select(x => new DashboardDirector_SaledItemFluctuationByMonthDTO
                    {
                        Day = x.Key,
                        SaledItemCounter = (decimal)x.Sum(x => x.RequestedQuantity)
                    })
                    .ToListWithNoLockAsync();
                DashboardDirector_SaledItemFluctuationDTO DashboardDirector_SaledItemFluctuationDTO = new DashboardDirector_SaledItemFluctuationDTO();
                DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByMonths = new List<DashboardDirector_SaledItemFluctuationByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = 1; i < number_of_day_in_this_month + 1; i++)
                {
                    DashboardDirector_SaledItemFluctuationByMonthDTO SaledItemFluctuationByMonth = new DashboardDirector_SaledItemFluctuationByMonthDTO
                    {
                        Day = i,
                        SaledItemCounter = 0
                    };
                    DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByMonths.Add(SaledItemFluctuationByMonth);
                }

                foreach (var SaledItemFluctuationByMonth in DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByMonths)
                {
                    var data = DashboardDirector_SaledItemFluctuationByMonthDTOs.Where(x => x.Day == SaledItemFluctuationByMonth.Day).FirstOrDefault();
                    if (data != null)
                        SaledItemFluctuationByMonth.SaledItemCounter = data.SaledItemCounter;
                }

                return DashboardDirector_SaledItemFluctuationDTO;
            }
            else if (DashboardDirector_SaledItemFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.THIS_QUARTER.Id)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.Month / 3m));

                var IndirectSalesOrderTransactionQuery = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.SalesEmployeeId, AppUserIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, StartFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, EndFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.RequestStateId, RequestStateIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => !x.DeletedAt.HasValue);
                var DashboardDirector_SaledItemFluctuationByQuarterDTOs = await IndirectSalesOrderTransactionQuery
                    .GroupBy(x => x.OrderDate.Month)
                    .Select(x => new DashboardDirector_SaledItemFluctuationByQuarterDTO
                    {
                        Month = x.Key,
                        SaledItemCounter = (decimal)x.Sum(x => x.RequestedQuantity)
                    })
                    .ToListWithNoLockAsync();
                DashboardDirector_SaledItemFluctuationDTO DashboardDirector_SaledItemFluctuationDTO = new DashboardDirector_SaledItemFluctuationDTO();
                DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByQuaters = new List<DashboardDirector_SaledItemFluctuationByQuarterDTO>();
                int start = 3 * (this_quarter - 1) + 1;
                int end = start + 3;
                for (int i = start; i < end; i++)
                {
                    DashboardDirector_SaledItemFluctuationByQuarterDTO SaledItemFluctuationByQuarter = new DashboardDirector_SaledItemFluctuationByQuarterDTO
                    {
                        Month = i,
                        SaledItemCounter = 0
                    };
                    DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByQuaters.Add(SaledItemFluctuationByQuarter);
                }

                foreach (var SaledItemFluctuationByQuater in DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByQuaters)
                {
                    var data = DashboardDirector_SaledItemFluctuationByQuarterDTOs.Where(x => x.Month == SaledItemFluctuationByQuater.Month).FirstOrDefault();
                    if (data != null)
                        SaledItemFluctuationByQuater.SaledItemCounter = data.SaledItemCounter;
                }

                return DashboardDirector_SaledItemFluctuationDTO;
            }
            else if (DashboardDirector_SaledItemFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.LAST_QUATER.Id)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.Month / 3m));
                var last_quarter = (this_quarter + 3) % 4;

                var IndirectSalesOrderTransactionQuery = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.SalesEmployeeId, AppUserIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, StartFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, EndFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.RequestStateId, RequestStateIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => !x.DeletedAt.HasValue);
                var DashboardDirector_SaledItemFluctuationByQuarterDTOs = await IndirectSalesOrderTransactionQuery
                    .GroupBy(x => x.OrderDate.Month)
                    .Select(x => new DashboardDirector_SaledItemFluctuationByQuarterDTO
                    {
                        Month = x.Key,
                        SaledItemCounter = (decimal)x.Sum(x => x.RequestedQuantity)
                    })
                    .ToListWithNoLockAsync();

                DashboardDirector_SaledItemFluctuationDTO DashboardDirector_SaledItemFluctuationDTO = new DashboardDirector_SaledItemFluctuationDTO();
                DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByQuaters = new List<DashboardDirector_SaledItemFluctuationByQuarterDTO>();
                int start = 3 * (last_quarter - 1) + 1;
                int end = start + 3;
                for (int i = start; i < end; i++)
                {
                    DashboardDirector_SaledItemFluctuationByQuarterDTO SaledItemFluctuationByQuarter = new DashboardDirector_SaledItemFluctuationByQuarterDTO
                    {
                        Month = i,
                        SaledItemCounter = 0
                    };
                    DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByQuaters.Add(SaledItemFluctuationByQuarter);
                }

                foreach (var SaledItemFluctuationByQuater in DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByQuaters)
                {
                    var data = DashboardDirector_SaledItemFluctuationByQuarterDTOs.Where(x => x.Month == SaledItemFluctuationByQuater.Month).FirstOrDefault();
                    if (data != null)
                        SaledItemFluctuationByQuater.SaledItemCounter = data.SaledItemCounter;
                }

                return DashboardDirector_SaledItemFluctuationDTO;
            }
            else if (DashboardDirector_SaledItemFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.YEAR.Id)
            {
                var IndirectSalesOrderTransactionQuery = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.SalesEmployeeId, AppUserIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, StartFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, EndFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.RequestStateId, RequestStateIdFilter);
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => !x.DeletedAt.HasValue);
                var DashboardDirector_SaledItemFluctuationByYearDTO = await IndirectSalesOrderTransactionQuery
                    .GroupBy(x => x.OrderDate.Month)
                    .Select(x => new DashboardDirector_SaledItemFluctuationByYearDTO
                    {
                        Month = x.Key,
                        SaledItemCounter = (decimal)x.Sum(x => x.RequestedQuantity)
                    })
                    .ToListWithNoLockAsync();

                DashboardDirector_SaledItemFluctuationDTO DashboardDirector_SaledItemFluctuationDTO = new DashboardDirector_SaledItemFluctuationDTO();
                DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByYears = new List<DashboardDirector_SaledItemFluctuationByYearDTO>();
                for (int i = 1; i <= 12; i++)
                {
                    DashboardDirector_SaledItemFluctuationByYearDTO SaledItemFluctuationByYear = new DashboardDirector_SaledItemFluctuationByYearDTO
                    {
                        Month = i,
                        SaledItemCounter = 0
                    };
                    DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByYears.Add(SaledItemFluctuationByYear);
                }

                foreach (var SaledItemFluctuationByYear in DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByYears)
                {
                    var data = DashboardDirector_SaledItemFluctuationByYearDTO.Where(x => x.Month == SaledItemFluctuationByYear.Month).FirstOrDefault();
                    if (data != null)
                        SaledItemFluctuationByYear.SaledItemCounter = data.SaledItemCounter;
                }

                return DashboardDirector_SaledItemFluctuationDTO;
            }
            return new DashboardDirector_SaledItemFluctuationDTO();
        }

        private async Task<Tuple<List<long>, List<long>, List<long>>> DynamicFilter(DashboardDirector_FilterDTO DashboardDirector_FilterDTO)
        {
            long? ProvinceId = DashboardDirector_FilterDTO.ProvinceId?.Equal;
            long? DistrictId = DashboardDirector_FilterDTO.DistrictId?.Equal;
            long? SaleEmployeeId = DashboardDirector_FilterDTO.AppUserId?.Equal;
            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && OrganizationIds.Contains(o.Id)).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (DashboardDirector_FilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == DashboardDirector_FilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Where(o => o.DeletedAt == null).Select(o => o.Id).ToList();

            List<long> StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);

            var store_query = DWContext.Dim_Store.AsNoTracking();
            store_query = store_query.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            store_query = store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            store_query = store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });

            StoreIds = await store_query.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();

            AppUserFilter AppUserFilter = new AppUserFilter
            {
                OrganizationId = new IdFilter { In = OrganizationIds },
                Id = new IdFilter { },
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.DisplayName | AppUserSelect.Organization
            };
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            var AppUsers = await AppUserService.List(AppUserFilter);
            var AppUserIds = AppUsers.Where(a => a.DeletedAt == null).Select(x => x.Id).ToList();
            if (SaleEmployeeId.HasValue)
            {
                AppUserIds = AppUserIds.Where(x => x == SaleEmployeeId.Value).ToList();
                // Get list stores in Routing scoped
                //List<long> StoresInScopedId = await DataContext.AppUserStoreMapping.Where(m => m.AppUserId == SaleEmployeeId)
                //.Select(s => s.StoreId).ToListWithNoLockAsync();
                //// if the employee has Routing scoped StoreIds = StoresInScopedId + DraftStoreIds
                //if (StoresInScopedId.Count > 0)
                //{
                //    AppUserDAO AppUserDAO = await DataContext.AppUser.Where(x => x.Id == SaleEmployeeId)
                //    .Include(x => x.Organization)
                //    .FirstOrDefaultWithNoLockAsync();
                //    List<long> DraftStoreIds = await DataContext.Store.Where(x =>
                //        x.StoreStatusId == StoreStatusEnum.DRAFT.Id &&
                //        x.Organization.Path.StartsWith(AppUserDAO.Organization.Path) &&
                //        x.DeletedAt == null)
                //    .Select(x => x.Id).ToListWithNoLockAsync();
                //    StoresInScopedId.AddRange(DraftStoreIds);
                //    StoreIds = StoresInScopedId.Distinct().ToList();
                //}
            }
            return Tuple.Create(StoreIds, OrganizationIds, AppUserIds);
        }
    }
}
