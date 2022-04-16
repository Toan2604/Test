using DMS.Common;
using DMS.Models;
using DMS.Services.MStoreChecking;
using Microsoft.AspNetCore.Mvc;
using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DMS.Services.MIndirectSalesOrder;
using Microsoft.EntityFrameworkCore;
using DMS.Services.MAppUser;
using DMS.Services.MOrganization;
using RestSharp;
using DMS.Helpers;
using DMS.Enums;
using DMS.Services.MBrand;
using DMS.Services.MStore;
using Thinktecture.EntityFrameworkCore.TempTables;
using Thinktecture;
using DMS.Services.MDistrict;
using DMS.Services.MProvince;
using System.IO;
using System.Dynamic;
using TrueSight.Common;
using DMS.DWModels;
using DMS.Services.MEstimatedRevenue;
using OfficeOpenXml;
using DMS.Services.MStoreScouting;

namespace DMS.Rpc.dashboards.store_information
{
    public partial class DashboardStoreInformationController : RpcController
    {
        private DataContext DataContext;
        private DWContext DWContext;
        private IEstimatedRevenueService EstimatedRevenueService;
        private IAppUserService AppUserService;
        private IBrandService BrandService;
        private ICurrentContext CurrentContext;
        private IDistrictService DistrictService;
        private IOrganizationService OrganizationService;
        private IProvinceService ProvinceService;
        private IStoreService StoreService;
        private IStoreScoutingService StoreScoutingService;
        public DashboardStoreInformationController(
            DataContext DataContext,
            DWContext DWContext,
            IEstimatedRevenueService EstimatedRevenueService,
            IAppUserService AppUserService,
            IBrandService BrandService,
            ICurrentContext CurrentContext,
            IDistrictService DistrictService,
            IOrganizationService OrganizationService,
            IProvinceService ProvinceService,
            IStoreService StoreService,
            IStoreScoutingService StoreScoutingService)
        {
            this.DataContext = DataContext;
            this.DWContext = DWContext;
            this.EstimatedRevenueService = EstimatedRevenueService;
            this.AppUserService = AppUserService;
            this.BrandService = BrandService;
            this.CurrentContext = CurrentContext;
            this.DistrictService = DistrictService;
            this.OrganizationService = OrganizationService;
            this.ProvinceService = ProvinceService;
            this.StoreService = StoreService;
            this.StoreScoutingService = StoreScoutingService;
        }

        #region Filter List
        [Route(DashboardStoreInformationRoute.FilterListBrand), HttpPost]
        public async Task<List<DashboardStoreInformation_BrandDTO>> FilterListBrand([FromBody] DashboardStoreInformation_BrandFilterDTO DashboardStoreInformation_BrandFilterDTO)
        {
            BrandFilter BrandFilter = new BrandFilter();
            BrandFilter.Skip = 0;
            BrandFilter.Take = 20;
            BrandFilter.OrderBy = BrandOrder.Id;
            BrandFilter.OrderType = OrderType.ASC;
            BrandFilter.Selects = BrandSelect.ALL;
            BrandFilter.Id = DashboardStoreInformation_BrandFilterDTO.Id;
            BrandFilter.Code = DashboardStoreInformation_BrandFilterDTO.Code;
            BrandFilter.Name = DashboardStoreInformation_BrandFilterDTO.Name;
            BrandFilter.Description = DashboardStoreInformation_BrandFilterDTO.Description;
            BrandFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            BrandFilter.UpdateTime = DashboardStoreInformation_BrandFilterDTO.UpdateTime;

            List<Brand> Brands = await BrandService.List(BrandFilter);
            List<DashboardStoreInformation_BrandDTO> DashboardStoreInformation_BrandDTOs = Brands
                .Select(x => new DashboardStoreInformation_BrandDTO(x)).ToList();
            return DashboardStoreInformation_BrandDTOs;
        }

        [Route(DashboardStoreInformationRoute.FilterListOrganization), HttpPost]
        public async Task<List<DashboardStoreInformation_OrganizationDTO>> FilterListOrganization([FromBody] DashboardStoreInformation_OrganizationFilterDTO DashboardStoreInformation_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Code = DashboardStoreInformation_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = DashboardStoreInformation_OrganizationFilterDTO.Name;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<DashboardStoreInformation_OrganizationDTO> DashboardStoreInformation_OrganizationDTOs = Organizations
                .Select(x => new DashboardStoreInformation_OrganizationDTO(x)).ToList();
            return DashboardStoreInformation_OrganizationDTOs;
        }

        [Route(DashboardStoreInformationRoute.FilterListDistrict), HttpPost]
        public async Task<List<DashboardStoreInformation_DistrictDTO>> FilterListDistrict([FromBody] DashboardStoreInformation_DistrictFilterDTO DashboardStoreInformation_DistrictFilterDTO)
        {
            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = DashboardStoreInformation_DistrictFilterDTO.Id;
            DistrictFilter.Name = DashboardStoreInformation_DistrictFilterDTO.Name;
            DistrictFilter.Priority = DashboardStoreInformation_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = DashboardStoreInformation_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = DashboardStoreInformation_DistrictFilterDTO.StatusId;

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<DashboardStoreInformation_DistrictDTO> DashboardStoreInformation_DistrictDTOs = Districts
                .Select(x => new DashboardStoreInformation_DistrictDTO(x)).ToList();
            return DashboardStoreInformation_DistrictDTOs;
        }

        [Route(DashboardStoreInformationRoute.FilterListProvince), HttpPost]
        public async Task<List<DashboardStoreInformation_ProvinceDTO>> FilterListProvince([FromBody] DashboardStoreInformation_ProvinceFilterDTO DashboardStoreInformation_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = DashboardStoreInformation_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = DashboardStoreInformation_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = DashboardStoreInformation_ProvinceFilterDTO.StatusId;

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<DashboardStoreInformation_ProvinceDTO> DashboardStoreInformation_ProvinceDTOs = Provinces
                .Select(x => new DashboardStoreInformation_ProvinceDTO(x)).ToList();
            return DashboardStoreInformation_ProvinceDTOs;
        }

        [Route(DashboardStoreInformationRoute.FilterListAppUser), HttpPost]
        public async Task<List<DashboardStoreInformation_AppUserDTO>> FilterListAppUser([FromBody] DashboardStoreInformation_AppUserFilterDTO DashboardStoreInformation_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = DashboardStoreInformation_AppUserFilterDTO.Id;
            AppUserFilter.Username = DashboardStoreInformation_AppUserFilterDTO.Username;
            AppUserFilter.Password = DashboardStoreInformation_AppUserFilterDTO.Password;
            AppUserFilter.DisplayName = DashboardStoreInformation_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = DashboardStoreInformation_AppUserFilterDTO.Address;
            AppUserFilter.Email = DashboardStoreInformation_AppUserFilterDTO.Email;
            AppUserFilter.Phone = DashboardStoreInformation_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = DashboardStoreInformation_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = DashboardStoreInformation_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = DashboardStoreInformation_AppUserFilterDTO.OrganizationId;
            AppUserFilter.SexId = DashboardStoreInformation_AppUserFilterDTO.SexId;
            AppUserFilter.StatusId = DashboardStoreInformation_AppUserFilterDTO.StatusId;
            AppUserFilter.Birthday = DashboardStoreInformation_AppUserFilterDTO.Birthday;
            AppUserFilter.ProvinceId = DashboardStoreInformation_AppUserFilterDTO.ProvinceId;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<DashboardStoreInformation_AppUserDTO> DashboardStoreInformation_AppUserDTOs = AppUsers
                .Select(x => new DashboardStoreInformation_AppUserDTO(x)).ToList();
            return DashboardStoreInformation_AppUserDTOs;
        }


        #endregion

        [Route(DashboardStoreInformationRoute.EstimatedRevenueStatistic), HttpPost]
        public async Task<List<DashboardStoreInformation_EstimatedRevenueStatisticsDTO>> EstimatedRevenueStatistic([FromBody] DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long? EstimatedRevenueId = DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO.EstimatedRevenueId?.Equal;
            long? ProvinceId = DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO.ProvinceId?.Equal;
            long? DistrictId = DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO.DistrictId?.Equal;
            long? AppUserId = DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO.AppUserId?.Equal;
            var AppUserIdFilter = new IdFilter();
            if (AppUserId.HasValue) AppUserIdFilter.Equal = AppUserId;
            var EstimatedRevenueIdFilter = new IdFilter();
            var ProvinceIdFilter = new IdFilter();
            var DistrictIdFilter = new IdFilter();
            var OrderDateFilter = new DateFilter { LessEqual = DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO.OrderDate?.LessEqual };
            if (EstimatedRevenueId.HasValue) EstimatedRevenueIdFilter.Equal = EstimatedRevenueId;
            if (ProvinceId.HasValue) ProvinceIdFilter.Equal = ProvinceId;
            if (DistrictId.HasValue) DistrictIdFilter.Equal = DistrictId;
            if (DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO.OrganizationId == null)
                DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO.OrganizationId = new IdFilter();
            var OrganizationFilter = DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO.OrganizationId;
            List<long> OrganizationIds, ListStoreIds, AppuserIds;
            (OrganizationIds, ListStoreIds, AppuserIds) = await DynamicFilter(OrganizationFilter, ProvinceIdFilter, DistrictIdFilter, AppUserIdFilter);
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var StoreIdFilter = new IdFilter { In = ListStoreIds };

            var StoreHistoryQuery = DataContext.StoreHistory.AsNoTracking();
            StoreHistoryQuery = StoreHistoryQuery.Where(x => x.CreatedAt, OrderDateFilter);
            StoreHistoryQuery = StoreHistoryQuery.Where(x => x.StoreId, StoreIdFilter);
            var StoreHistory = await StoreHistoryQuery.ToListWithNoLockAsync();
            var ActiveStores = StoreHistory.GroupBy(x => x.StoreId).Select(x => new
            { StoreId = x.Key, x.OrderByDescending(x => x.CreatedAt).First().StatusId, x.OrderByDescending(x => x.CreatedAt).First().EstimatedRevenueId }).ToList();
            List<long> ActiveSurveyedStoreIds = ActiveStores.Where(x => x.StatusId == StatusEnum.ACTIVE.Id).Select(x => x.StoreId).Distinct().ToList();

            List<Store> ActiveSurveyedStores = ActiveStores.Where(x => x.StatusId == StatusEnum.ACTIVE.Id).Select(x => new Store
            {
                Id = x.StoreId,
                EstimatedRevenueId = x.EstimatedRevenueId,
            }).ToList();

            long StoreHasEstimatedRevenueCount = ActiveSurveyedStores.Where(x => x.EstimatedRevenueId != null).Count();

            List<EstimatedRevenue> EstimatedRevenues = await EstimatedRevenueService.List(new EstimatedRevenueFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = EstimatedRevenueSelect.ALL,
            });

            List<DashboardStoreInformation_EstimatedRevenueStatisticsDTO> DashboardStoreInformation_EstimatedRevenueStatisticsDTOs = EstimatedRevenues
                .Select(x => new DashboardStoreInformation_EstimatedRevenueStatisticsDTO
                {
                    EstimatedRevenueId = x.Id,
                    EstimatedRevenueName = x.Name
                }).ToList();

            foreach (var DashboardStoreInformation_EstimatedRevenueStatisticsDTO in DashboardStoreInformation_EstimatedRevenueStatisticsDTOs)
            {
                DashboardStoreInformation_EstimatedRevenueStatisticsDTO.Value = ActiveSurveyedStores.
                    Where(x => x.EstimatedRevenueId == DashboardStoreInformation_EstimatedRevenueStatisticsDTO.EstimatedRevenueId).Count();
                DashboardStoreInformation_EstimatedRevenueStatisticsDTO.Total = StoreHasEstimatedRevenueCount;
            }
            DashboardStoreInformation_EstimatedRevenueStatisticsDTOs = DashboardStoreInformation_EstimatedRevenueStatisticsDTOs.OrderByDescending(x => x.Value).ToList();
            return DashboardStoreInformation_EstimatedRevenueStatisticsDTOs;
        }

        [Route(DashboardStoreInformationRoute.StoreCounter), HttpPost]
        public async Task<DashboardStoreInformation_StoreCounterDTO> StoreCounter([FromBody] DashboardStoreInformation_StoreCounterFilterDTO DashboardStoreInformation_StoreCounterFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            long? ProvinceId = DashboardStoreInformation_StoreCounterFilterDTO.ProvinceId?.Equal;
            long? DistrictId = DashboardStoreInformation_StoreCounterFilterDTO.DistrictId?.Equal;
            long? AppUserId = DashboardStoreInformation_StoreCounterFilterDTO.AppUserId?.Equal;
            var AppUserIdFilter = new IdFilter();
            if (AppUserId.HasValue) AppUserIdFilter.Equal = AppUserId;

            var ProvinceIdFilter = new IdFilter();
            var DistrictIdFilter = new IdFilter();
            var OrderDateFilter = new DateFilter { LessEqual = DashboardStoreInformation_StoreCounterFilterDTO.OrderDate?.LessEqual };
            if (ProvinceId.HasValue) ProvinceIdFilter.Equal = ProvinceId;
            if (DistrictId.HasValue) DistrictIdFilter.Equal = DistrictId;
            if (DashboardStoreInformation_StoreCounterFilterDTO.OrganizationId == null)
                DashboardStoreInformation_StoreCounterFilterDTO.OrganizationId = new IdFilter();
            var OrganizationFilter = DashboardStoreInformation_StoreCounterFilterDTO.OrganizationId;
            List<long> OrganizationIds, ListStoreIds, AppuserIds;
            (OrganizationIds, ListStoreIds, AppuserIds) = await DynamicFilter(OrganizationFilter, ProvinceIdFilter, DistrictIdFilter, AppUserIdFilter);
            var StoreIdFilter = new IdFilter { In = ListStoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserFilter = new IdFilter { In = AppuserIds };

            var BrandQuery = DataContext.Brand.AsNoTracking();
            BrandQuery = BrandQuery.Where(x => x.DeletedAt == null);
            List<long> BrandIds = await BrandQuery.Select(x => x.Id).ToListWithNoLockAsync();
            var BrandIdFilter = new IdFilter { In = BrandIds };

            List<long> ActiveStoreIds, ActiveBrandIds;
            (ActiveStoreIds, ActiveBrandIds) = await GetActiveIdFromHistory(StoreIdFilter, BrandIdFilter, OrderDateFilter);
            var TotalStoreIdFilter = new IdFilter { In = ActiveStoreIds };
            BrandIdFilter = new IdFilter { In = ActiveBrandIds };
            var StoreCounter = ActiveStoreIds.Count();

            var SurveyedStoreQuery = DataContext.BrandInStore.AsNoTracking();
            if (OrderDateFilter.LessEqual != null)
                SurveyedStoreQuery = SurveyedStoreQuery.Where(x => x.CreatedAt <= OrderDateFilter.LessEqual && (x.DeletedAt == null || x.DeletedAt >= OrderDateFilter.LessEqual));
            else
                SurveyedStoreQuery = SurveyedStoreQuery.Where(x => x.DeletedAt == null);
            SurveyedStoreQuery = SurveyedStoreQuery.Where(x => x.StoreId, TotalStoreIdFilter);
            SurveyedStoreQuery = SurveyedStoreQuery.Where(x => x.BrandId, BrandIdFilter);
            List<long> SurveyedStoreIds = await SurveyedStoreQuery.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();

            var SurveyedStoreCounter = SurveyedStoreIds.Count();

            List<long> StoreScoutingIds = await FilterStoreScouting(StoreScoutingService, OrganizationService, CurrentContext);
            var StoreScoutingStatusFilter = new IdFilter { Equal = StoreScoutingStatusEnum.NOTOPEN.Id };
            var StoreScoutingIdFilter = new IdFilter { In = StoreScoutingIds };
            var StoreScoutingQuery = DWContext.Fact_StoreScouting.AsNoTracking();

            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.DeletedAt == null);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.CreatorId, AppUserFilter);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.StoreScoutingStatusId, StoreScoutingStatusFilter);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.StoreScoutingId, StoreScoutingIdFilter);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.ProvinceId, ProvinceIdFilter);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.DistrictId, DistrictIdFilter);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            var StoreScoutingCounter = await StoreScoutingQuery.CountWithNoLockAsync();
            DashboardStoreInformation_StoreCounterDTO DashboardStoreInformation_StoreCounterDTO = new DashboardStoreInformation_StoreCounterDTO();
            DashboardStoreInformation_StoreCounterDTO.SurveyedStoreCounter = SurveyedStoreCounter;
            DashboardStoreInformation_StoreCounterDTO.StoreCounter = StoreCounter + StoreScoutingCounter;
            return DashboardStoreInformation_StoreCounterDTO;
        }

        [Route(DashboardStoreInformationRoute.BrandStatistic), HttpPost]
        public async Task<List<DashboardStoreInformation_BrandStatisticsDTO>> BrandStatistic([FromBody] DashboardStoreInformation_BrandStatisticsFilterDTO DashboardStoreInformation_BrandStatisticsFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long? BrandId = DashboardStoreInformation_BrandStatisticsFilterDTO.BrandId?.Equal;
            long? ProvinceId = DashboardStoreInformation_BrandStatisticsFilterDTO.ProvinceId?.Equal;
            long? DistrictId = DashboardStoreInformation_BrandStatisticsFilterDTO.DistrictId?.Equal;
            long? AppUserId = DashboardStoreInformation_BrandStatisticsFilterDTO.AppUserId?.Equal;
            var AppUserIdFilter = new IdFilter();
            if (AppUserId.HasValue) AppUserIdFilter.Equal = AppUserId;
            var BrandIdFilter = new IdFilter();
            var ProvinceIdFilter = new IdFilter();
            var DistrictIdFilter = new IdFilter();
            var OrderDateFilter = new DateFilter { LessEqual = DashboardStoreInformation_BrandStatisticsFilterDTO.OrderDate?.LessEqual };
            if (BrandId.HasValue) BrandIdFilter.Equal = BrandId;
            if (ProvinceId.HasValue) ProvinceIdFilter.Equal = ProvinceId;
            if (DistrictId.HasValue) DistrictIdFilter.Equal = DistrictId;
            if (DashboardStoreInformation_BrandStatisticsFilterDTO.OrganizationId == null)
                DashboardStoreInformation_BrandStatisticsFilterDTO.OrganizationId = new IdFilter();
            var OrganizationFilter = DashboardStoreInformation_BrandStatisticsFilterDTO.OrganizationId;
            List<long> OrganizationIds, ListStoreIds, AppuserIds;
            (OrganizationIds, ListStoreIds, AppuserIds) = await DynamicFilter(OrganizationFilter, ProvinceIdFilter, DistrictIdFilter, AppUserIdFilter);
            var StoreIdFilter = new IdFilter { In = ListStoreIds };

            var BrandQuery = DataContext.Brand.AsNoTracking();
            BrandQuery = BrandQuery.Where(x => x.DeletedAt == null);
            List<long> TotalBrandIds = await BrandQuery.Select(x => x.Id).Distinct().ToListWithNoLockAsync();
            var TotalBrandIdFilter = new IdFilter { In = TotalBrandIds };

            #region Check active by Order date
            List<long> ActiveStoreIds, ActiveBrandIds;
            (ActiveStoreIds, ActiveBrandIds) = await GetActiveIdFromHistory(StoreIdFilter, TotalBrandIdFilter, OrderDateFilter);
            StoreIdFilter = new IdFilter { In = ActiveStoreIds };
            TotalBrandIdFilter = new IdFilter { In = ActiveBrandIds };

            BrandQuery = BrandQuery.Where(x => x.Id, BrandIdFilter);
            var Brands = await BrandQuery.ToListWithNoLockAsync();
            #endregion

            #region get Surveyed Store
            var BrandInStoreQuery = DataContext.BrandInStore.AsNoTracking();
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.StoreId, StoreIdFilter);
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.BrandId, TotalBrandIdFilter);
            if (OrderDateFilter.LessEqual == null)
                BrandInStoreQuery = BrandInStoreQuery.Where(x => x.DeletedAt == null);
            else
                BrandInStoreQuery = BrandInStoreQuery.Where(x => (x.CreatedAt <= OrderDateFilter.LessEqual) && (x.DeletedAt == null || x.DeletedAt >= OrderDateFilter.LessEqual));

            var SurveyedStoreIds = await BrandInStoreQuery.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();
            #endregion

            #region Convert to Result
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.BrandId, BrandIdFilter);
            var BrandInStore = await BrandInStoreQuery.ToListWithNoLockAsync();
            List<DashboardStoreInformation_BrandStatisticsDTO> DashboardStoreInformation_BrandStatisticsDTOs = BrandInStore
                .GroupBy(x => x.BrandId)
                .Select(x => new DashboardStoreInformation_BrandStatisticsDTO
                {
                    BrandId = x.Key,
                    Value = x.Select(x => x.StoreId).Distinct().Count()
                })
                .ToList();
            foreach (var DashboardStoreInformation_BrandStatisticsDTO in DashboardStoreInformation_BrandStatisticsDTOs)
            {
                DashboardStoreInformation_BrandStatisticsDTO.BrandName = Brands.Where(x => x.Id == DashboardStoreInformation_BrandStatisticsDTO.BrandId).Select(x => x.Name).FirstOrDefault();
                DashboardStoreInformation_BrandStatisticsDTO.Total = SurveyedStoreIds.Count();
            }
            #endregion

            DashboardStoreInformation_BrandStatisticsDTOs = DashboardStoreInformation_BrandStatisticsDTOs.OrderByDescending(x => x.Value).ToList();
            return DashboardStoreInformation_BrandStatisticsDTOs;
        }

        [Route(DashboardStoreInformationRoute.ExportBrandStatistic), HttpPost]
        public async Task<ActionResult> ExportBrandStatistic([FromBody] DashboardStoreInformation_BrandStatisticsFilterDTO DashboardStoreInformation_BrandStatisticsFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long? BrandId = DashboardStoreInformation_BrandStatisticsFilterDTO.BrandId?.Equal;
            long? ProvinceId = DashboardStoreInformation_BrandStatisticsFilterDTO.ProvinceId?.Equal;
            long? DistrictId = DashboardStoreInformation_BrandStatisticsFilterDTO.DistrictId?.Equal;
            long? AppUserId = DashboardStoreInformation_BrandStatisticsFilterDTO.AppUserId?.Equal;
            var AppUserIdFilter = new IdFilter();
            if (AppUserId.HasValue) AppUserIdFilter.Equal = AppUserId;
            var BrandIdFilter = new IdFilter();
            var ProvinceIdFilter = new IdFilter();
            var DistrictIdFilter = new IdFilter();
            var OrderDateFilter = new DateFilter { LessEqual = DashboardStoreInformation_BrandStatisticsFilterDTO.OrderDate?.LessEqual };
            if (BrandId.HasValue) BrandIdFilter.Equal = BrandId;
            if (ProvinceId.HasValue) ProvinceIdFilter.Equal = ProvinceId;
            if (DistrictId.HasValue) DistrictIdFilter.Equal = DistrictId;
            if (BrandId.HasValue == false)
                return BadRequest("Bạn chưa chọn hãng");
            if (DashboardStoreInformation_BrandStatisticsFilterDTO.OrganizationId == null)
                DashboardStoreInformation_BrandStatisticsFilterDTO.OrganizationId = new IdFilter();
            var OrganizationFilter = DashboardStoreInformation_BrandStatisticsFilterDTO.OrganizationId;
            List<long> OrganizationIds, StoreIds, AppuserIds;
            (OrganizationIds, StoreIds, AppuserIds) = await DynamicFilter(OrganizationFilter, ProvinceIdFilter, DistrictIdFilter, AppUserIdFilter);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };

            var Brand = DataContext.Brand.Where(x => x.Id, BrandIdFilter).FirstOrDefault();

            #region Check active by Order date
            List<long> ActiveStoreIds, ActiveBrandIds;
            (ActiveStoreIds, ActiveBrandIds) = await GetActiveIdFromHistory(StoreIdFilter, BrandIdFilter, OrderDateFilter);
            StoreIdFilter = new IdFilter { In = ActiveStoreIds };
            #endregion

            var BrandInStoreQuery = DataContext.BrandInStore.AsNoTracking();
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.StoreId, StoreIdFilter);
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.BrandId, BrandIdFilter);
            if (OrderDateFilter.LessEqual == null)
                BrandInStoreQuery = BrandInStoreQuery.Where(x => x.DeletedAt == null);
            else
                BrandInStoreQuery = BrandInStoreQuery.Where(x => (x.CreatedAt <= OrderDateFilter.LessEqual) && (x.DeletedAt == null || x.DeletedAt >= OrderDateFilter.LessEqual));

            var SurveyedStoreIds = await BrandInStoreQuery.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();
            StoreIdFilter.In = SurveyedStoreIds;
            var StoreQuery = DataContext.Store.AsNoTracking();
            StoreQuery = StoreQuery.Where(x => x.Id, StoreIdFilter);
            var StoreDAOs = await StoreQuery.ToListWithNoLockAsync();

            var Stores = StoreDAOs.Select(x => new DashboardStoreInformation_StoreDTO
            {
                Id = x.Id,
                Code = x.Code,
                CodeDraft = x.CodeDraft,
                Name = x.Name,
                Telephone = x.Telephone,
                Address = x.Address,
                OrganizationId = x.OrganizationId,
            }).ToList();

            OrganizationIds = Stores.Select(x => x.OrganizationId).Distinct().ToList();
            OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var OrganizationQuery = DataContext.Organization.AsNoTracking();
            OrganizationQuery = OrganizationQuery.Where(x => x.Id, OrganizationIdFilter);
            OrganizationQuery = OrganizationQuery.Where(x => x.DeletedAt == null);
            OrganizationQuery = OrganizationQuery.OrderBy(x => x.Id);
            var Organizations = await OrganizationQuery.ToListWithNoLockAsync();

            List<DashboardStoreInformation_StoreExportDTO> DashboardStoreInformation_StoreExportDTOs = new List<DashboardStoreInformation_StoreExportDTO>();
            foreach (var Organization in Organizations)
            {
                DashboardStoreInformation_StoreExportDTO DashboardStoreInformation_StoreExportDTO = new DashboardStoreInformation_StoreExportDTO();
                DashboardStoreInformation_StoreExportDTO.OrganizationId = Organization.Id;
                DashboardStoreInformation_StoreExportDTO.OrganizationName = Organization.Name;
                DashboardStoreInformation_StoreExportDTO.Stores = Stores.Where(x => x.OrganizationId == Organization.Id).OrderBy(x => x.Code).ToList();
                DashboardStoreInformation_StoreExportDTOs.Add(DashboardStoreInformation_StoreExportDTO);
            }
            long stt = 1;
            foreach (var DashboardStoreInformation_StoreExportDTO in DashboardStoreInformation_StoreExportDTOs)
            {
                foreach (var Store in DashboardStoreInformation_StoreExportDTO.Stores)
                {
                    Store.STT = stt++;
                }
            }
            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            var Total = DashboardStoreInformation_StoreExportDTOs.SelectMany(x => x.Stores).Count();

            string path = "Templates/Dashboard_StoreInformation_Brand.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Brand = Brand;
            Data.Data = DashboardStoreInformation_StoreExportDTOs;
            Data.Total = Total;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "StoreInformation_Brand.xlsx");
        }

        [Route(DashboardStoreInformationRoute.BrandUnStatistic), HttpPost]
        public async Task<List<DashboardStoreInformation_BrandStatisticsDTO>> BrandUnStatistic([FromBody] DashboardStoreInformation_BrandStatisticsFilterDTO DashboardStoreInformation_BrandStatisticsFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long? BrandId = DashboardStoreInformation_BrandStatisticsFilterDTO.BrandId?.Equal;
            long? ProvinceId = DashboardStoreInformation_BrandStatisticsFilterDTO.ProvinceId?.Equal;
            long? DistrictId = DashboardStoreInformation_BrandStatisticsFilterDTO.DistrictId?.Equal;
            long? AppUserId = DashboardStoreInformation_BrandStatisticsFilterDTO.AppUserId?.Equal;
            var AppUserIdFilter = new IdFilter();
            if (AppUserId.HasValue) AppUserIdFilter.Equal = AppUserId;
            var BrandIdFilter = new IdFilter();
            var ProvinceIdFilter = new IdFilter();
            var DistrictIdFilter = new IdFilter();
            var OrderDateFilter = new DateFilter { LessEqual = DashboardStoreInformation_BrandStatisticsFilterDTO.OrderDate?.LessEqual };
            if (BrandId.HasValue) BrandIdFilter.Equal = BrandId;
            if (ProvinceId.HasValue) ProvinceIdFilter.Equal = ProvinceId;
            if (DistrictId.HasValue) DistrictIdFilter.Equal = DistrictId;
            if (DashboardStoreInformation_BrandStatisticsFilterDTO.OrganizationId == null)
                DashboardStoreInformation_BrandStatisticsFilterDTO.OrganizationId = new IdFilter();
            var OrganizationFilter = DashboardStoreInformation_BrandStatisticsFilterDTO.OrganizationId;
            List<long> OrganizationIds, StoreIds, AppuserIds;
            (OrganizationIds, StoreIds, AppuserIds) = await DynamicFilter(OrganizationFilter, ProvinceIdFilter, DistrictIdFilter, AppUserIdFilter);
            var StoreIdFilter = new IdFilter { In = StoreIds };

            var BrandQuery = DataContext.Brand.AsNoTracking();
            BrandQuery = BrandQuery.Where(x => x.DeletedAt == null);
            List<long> TotalBrandIds = await BrandQuery.Select(x => x.Id).Distinct().ToListWithNoLockAsync();
            var TotalBrandIdFilter = new IdFilter { In = TotalBrandIds };

            #region Check active by Order date
            List<long> ActiveStoreIds, ActiveBrandIds;
            (ActiveStoreIds, ActiveBrandIds) = await GetActiveIdFromHistory(StoreIdFilter, TotalBrandIdFilter, OrderDateFilter);
            StoreIdFilter = new IdFilter { In = ActiveStoreIds };
            TotalBrandIdFilter = new IdFilter { In = ActiveBrandIds };

            BrandQuery = BrandQuery.Where(x => x.Id, BrandIdFilter);
            var Brands = await BrandQuery.ToListWithNoLockAsync();
            #endregion

            #region get Surveyed Store
            var BrandInStoreQuery = DataContext.BrandInStore.AsNoTracking();
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.StoreId, StoreIdFilter);
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.BrandId, TotalBrandIdFilter);
            if (OrderDateFilter.LessEqual == null)
                BrandInStoreQuery = BrandInStoreQuery.Where(x => x.DeletedAt == null);
            else
                BrandInStoreQuery = BrandInStoreQuery.Where(x => (x.CreatedAt <= OrderDateFilter.LessEqual) && (x.DeletedAt == null || x.DeletedAt >= OrderDateFilter.LessEqual));

            var SurveyedStoreIds = await BrandInStoreQuery.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();
            long SurveyedStoreCounter = SurveyedStoreIds.Count();
            #endregion

            #region Convert to Result
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.BrandId, BrandIdFilter);
            var BrandInStore = await BrandInStoreQuery.ToListWithNoLockAsync();
            List<DashboardStoreInformation_BrandStatisticsDTO> DashboardStoreInformation_BrandStatisticsDTOs = BrandInStore
                .GroupBy(x => x.BrandId)
                .Select(x => new DashboardStoreInformation_BrandStatisticsDTO
                {
                    BrandId = x.Key,
                    Value = x.Select(x => x.StoreId).Distinct().Count()
                })
                .ToList();

            foreach (var DashboardStoreInformation_BrandStatisticsDTO in DashboardStoreInformation_BrandStatisticsDTOs)
            {
                DashboardStoreInformation_BrandStatisticsDTO.BrandName = Brands.Where(x => x.Id == DashboardStoreInformation_BrandStatisticsDTO.BrandId).Select(x => x.Name).FirstOrDefault();
                DashboardStoreInformation_BrandStatisticsDTO.Value = SurveyedStoreCounter - DashboardStoreInformation_BrandStatisticsDTO.Value;
                DashboardStoreInformation_BrandStatisticsDTO.Total = SurveyedStoreCounter;
            }
            #endregion
            DashboardStoreInformation_BrandStatisticsDTOs = DashboardStoreInformation_BrandStatisticsDTOs.OrderByDescending(x => x.Value).ToList();
            return DashboardStoreInformation_BrandStatisticsDTOs;
        }

        [Route(DashboardStoreInformationRoute.ExportBrandUnStatistic), HttpPost]
        public async Task<ActionResult> ExportUnBrandUnStatistic([FromBody] DashboardStoreInformation_BrandStatisticsFilterDTO DashboardStoreInformation_BrandStatisticsFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long? BrandId = DashboardStoreInformation_BrandStatisticsFilterDTO.BrandId?.Equal;
            long? ProvinceId = DashboardStoreInformation_BrandStatisticsFilterDTO.ProvinceId?.Equal;
            long? DistrictId = DashboardStoreInformation_BrandStatisticsFilterDTO.DistrictId?.Equal;
            long? AppUserId = DashboardStoreInformation_BrandStatisticsFilterDTO.AppUserId?.Equal;
            var AppUserIdFilter = new IdFilter();
            if (AppUserId.HasValue) AppUserIdFilter.Equal = AppUserId;
            var BrandIdFilter = new IdFilter();
            var ProvinceIdFilter = new IdFilter();
            var DistrictIdFilter = new IdFilter();
            var OrderDateFilter = new DateFilter { LessEqual = DashboardStoreInformation_BrandStatisticsFilterDTO.OrderDate?.LessEqual };
            if (BrandId.HasValue) BrandIdFilter.Equal = BrandId;
            if (ProvinceId.HasValue) ProvinceIdFilter.Equal = ProvinceId;
            if (DistrictId.HasValue) DistrictIdFilter.Equal = DistrictId;
            if (BrandId.HasValue == false)
                return BadRequest("Bạn chưa chọn hãng");
            if (DashboardStoreInformation_BrandStatisticsFilterDTO.OrganizationId == null)
                DashboardStoreInformation_BrandStatisticsFilterDTO.OrganizationId = new IdFilter();
            var OrganizationFilter = DashboardStoreInformation_BrandStatisticsFilterDTO.OrganizationId;
            List<long> OrganizationIds, StoreIds, AppuserIds;
            (OrganizationIds, StoreIds, AppuserIds) = await DynamicFilter(OrganizationFilter, ProvinceIdFilter, DistrictIdFilter, AppUserIdFilter);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };

            var Brand = DataContext.Brand.Where(x => x.Id, BrandIdFilter).FirstOrDefault();

            var BrandQuery = DataContext.Brand.AsNoTracking();
            BrandQuery = BrandQuery.Where(x => x.DeletedAt == null);
            List<long> TotalBrandIds = await BrandQuery.Select(x => x.Id).Distinct().ToListWithNoLockAsync();
            var TotalBrandIdFilter = new IdFilter { In = TotalBrandIds };

            #region Check active by Order date
            List<long> ActiveStoreIds, ActiveBrandIds;
            (ActiveStoreIds, ActiveBrandIds) = await GetActiveIdFromHistory(StoreIdFilter, TotalBrandIdFilter, OrderDateFilter);
            StoreIdFilter = new IdFilter { In = ActiveStoreIds };
            TotalBrandIdFilter = new IdFilter { In = ActiveBrandIds };
            #endregion

            var BrandInStoreQuery = DataContext.BrandInStore.AsNoTracking();
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.StoreId, StoreIdFilter);
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.BrandId, TotalBrandIdFilter);

            if (OrderDateFilter.LessEqual == null)
                BrandInStoreQuery = BrandInStoreQuery.Where(x => x.DeletedAt == null);
            else
                BrandInStoreQuery = BrandInStoreQuery.Where(x => (x.CreatedAt <= OrderDateFilter.LessEqual) && (x.DeletedAt == null || x.DeletedAt >= OrderDateFilter.LessEqual));

            StoreIds = await BrandInStoreQuery.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();
            StoreIdFilter = new IdFilter { In = StoreIds };
            var StoreQuery = DataContext.Store.AsNoTracking();
            StoreQuery = StoreQuery.Where(x => x.Id, StoreIdFilter);

            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.BrandId, BrandIdFilter);
            var UnBrandStoreIds = await BrandInStoreQuery.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();
            var UnBrandStoreIdFIlter = new IdFilter { NotIn = UnBrandStoreIds };
            StoreQuery = StoreQuery.Where(x => x.Id, UnBrandStoreIdFIlter);

            var Stores = await StoreQuery.Select(x => new DashboardStoreInformation_StoreDTO
            {
                Id = x.Id,
                Code = x.Code,
                CodeDraft = x.CodeDraft,
                Name = x.Name,
                Telephone = x.Telephone,
                Address = x.Address,
                OrganizationId = x.OrganizationId,
            }).ToListWithNoLockAsync();
            var OrganizationQuery = DataContext.Organization.AsNoTracking();
            OrganizationIds = Stores.Select(x => x.OrganizationId).Distinct().ToList();
            OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            OrganizationQuery = DataContext.Organization.AsNoTracking();
            OrganizationQuery = OrganizationQuery.Where(x => x.Id, OrganizationIdFilter);
            OrganizationQuery = OrganizationQuery.Where(x => x.DeletedAt == null);
            OrganizationQuery = OrganizationQuery.OrderBy(x => x.Id);
            var Organizations = await OrganizationQuery.ToListWithNoLockAsync();

            List<DashboardStoreInformation_StoreExportDTO> DashboardStoreInformation_StoreExportDTOs = new List<DashboardStoreInformation_StoreExportDTO>();
            foreach (var Organization in Organizations)
            {
                DashboardStoreInformation_StoreExportDTO DashboardStoreInformation_StoreExportDTO = new DashboardStoreInformation_StoreExportDTO();
                DashboardStoreInformation_StoreExportDTO.OrganizationId = Organization.Id;
                DashboardStoreInformation_StoreExportDTO.OrganizationName = Organization.Name;
                DashboardStoreInformation_StoreExportDTO.Stores = Stores.Where(x => x.OrganizationId == Organization.Id).OrderBy(x => x.Code).ToList();
                DashboardStoreInformation_StoreExportDTOs.Add(DashboardStoreInformation_StoreExportDTO);
            }
            long stt = 1;
            foreach (var DashboardStoreInformation_StoreExportDTO in DashboardStoreInformation_StoreExportDTOs)
            {
                foreach (var Store in DashboardStoreInformation_StoreExportDTO.Stores)
                {
                    Store.STT = stt++;
                }
            }

            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            var Total = DashboardStoreInformation_StoreExportDTOs.SelectMany(x => x.Stores).Count();

            string path = "Templates/Dashboard_StoreInformation_UnBrand.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Brand = Brand;
            Data.Data = DashboardStoreInformation_StoreExportDTOs;
            Data.Total = Total;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "StoreInformation_UnBrand.xlsx");
        }

        [Route(DashboardStoreInformationRoute.StoreCoverage), HttpPost]
        public async Task<List<DashboardStoreInformation_StoreDTO>> StoreCoverage([FromBody] DashboardStoreInformation_StoreFilterDTO DashboardStoreInformation_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long? BrandId = DashboardStoreInformation_StoreFilterDTO.BrandId?.Equal;
            long? ProvinceId = DashboardStoreInformation_StoreFilterDTO.ProvinceId?.Equal;
            long? DistrictId = DashboardStoreInformation_StoreFilterDTO.DistrictId?.Equal;
            long? AppUserId = DashboardStoreInformation_StoreFilterDTO.AppUserId?.Equal;
            var AppUserIdFilter = new IdFilter();
            if (AppUserId.HasValue) AppUserIdFilter.Equal = AppUserId;
            var BrandIdFilter = new IdFilter();
            var ProvinceIdFilter = new IdFilter();
            var DistrictIdFilter = new IdFilter();
            var OrderDateFilter = new DateFilter { LessEqual = DashboardStoreInformation_StoreFilterDTO.OrderDate?.LessEqual };
            //if (BrandId.HasValue) BrandIdFilter.Equal = BrandId;
            if (ProvinceId.HasValue) ProvinceIdFilter.Equal = ProvinceId;
            if (DistrictId.HasValue) DistrictIdFilter.Equal = DistrictId;
            if (DashboardStoreInformation_StoreFilterDTO.OrganizationId == null)
                DashboardStoreInformation_StoreFilterDTO.OrganizationId = new IdFilter();
            var OrganizationFilter = DashboardStoreInformation_StoreFilterDTO.OrganizationId;
            List<long> OrganizationIds, StoreIds, AppuserIds;
            (OrganizationIds, StoreIds, AppuserIds) = await DynamicFilter(OrganizationFilter, ProvinceIdFilter, DistrictIdFilter, AppUserIdFilter);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var BrandInStoreQuery = DataContext.BrandInStore.AsNoTracking();

            var StoreHistoryQuery = DataContext.StoreHistory.AsNoTracking();
            StoreHistoryQuery = StoreHistoryQuery.Where(x => x.CreatedAt, OrderDateFilter);
            StoreHistoryQuery = StoreHistoryQuery.Where(x => x.StoreId, StoreIdFilter);
            var StoreHistory = await StoreHistoryQuery.Select(x => new
            {
                x.StoreId,
                x.CreatedAt,
                x.StatusId,
            }).ToListWithNoLockAsync();
            var ActiveStores = StoreHistory.GroupBy(x => x.StoreId).Select(x => new { StoreId = x.Key, x.OrderByDescending(x => x.CreatedAt).First().StatusId }).ToList();
            List<long> ActiveSurveyedStoreIds = ActiveStores.Where(x => x.StatusId == StatusEnum.ACTIVE.Id).Select(x => x.StoreId).Distinct().ToList();
            StoreIdFilter = new IdFilter { In = ActiveSurveyedStoreIds };
            var StoreQuery = DataContext.Store.AsNoTracking();
            StoreQuery = StoreQuery.Where(x => x.Id, StoreIdFilter);

            List<DashboardStoreInformation_StoreDTO> DashboardMonitor_StoreDTOs = await StoreQuery.Select(x => new DashboardStoreInformation_StoreDTO
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Address = x.Address,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Telephone = x.OwnerPhone,
                StoreStatusId = x.StoreStatusId,
                IsScouting = false
            }).Distinct().ToListWithNoLockAsync();
            StoreIds = DashboardMonitor_StoreDTOs.Select(x => x.Id).ToList();
            StoreIdFilter = new IdFilter { In = StoreIds };
            var TopFilter = new IdFilter { Equal = 1 };

            var BrandQuery = DataContext.Brand.AsNoTracking();
            BrandQuery = BrandQuery.Where(x => x.DeletedAt == null);
            BrandQuery = BrandQuery.Where(x => x.Id, BrandIdFilter);
            var Brands = await BrandQuery.Select(x => new BrandDAO
            {
                Id = x.Id,
                Name = x.Name
            }).ToListWithNoLockAsync();
            var BrandIds = Brands.Select(x => x.Id).ToList();

            var BrandHistoryQuery = DataContext.BrandHistory.AsNoTracking();
            BrandHistoryQuery = BrandHistoryQuery.Where(x => x.CreatedAt, OrderDateFilter);
            var BrandHistory = await BrandHistoryQuery.Select(x => new
            {
                x.BrandId,
                x.CreatedAt,
                x.StatusId,
            }).ToListWithNoLockAsync();
            var ActiveBrands = BrandHistory.GroupBy(x => x.BrandId).Select(x => new { BrandId = x.Key, x.OrderByDescending(x => x.CreatedAt).First().StatusId }).ToList();
            List<long> ActiveSurveyedBrandIds = ActiveBrands.Where(x => x.StatusId == StatusEnum.ACTIVE.Id).Select(x => x.BrandId).Distinct().ToList();
            BrandIdFilter = new IdFilter { In = ActiveSurveyedBrandIds };

            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.BrandId, BrandIdFilter);
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.StoreId, StoreIdFilter);
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.Top, TopFilter);
            var BrandInStores = await BrandInStoreQuery.Select(x => new BrandInStoreDAO
            {
                Id = x.Id,
                StoreId = x.StoreId,
                BrandId = x.BrandId
            })
                .ToListWithNoLockAsync();
            foreach (var BrandInStore in BrandInStores)
            {
                BrandInStore.Brand = new BrandDAO
                {
                    Name = Brands.Where(x => x.Id == BrandInStore.BrandId).FirstOrDefault().Name
                };
            }
            foreach (var DashboardMonitor_StoreDTO in DashboardMonitor_StoreDTOs)
            {
                DashboardMonitor_StoreDTO.Top1BrandName = BrandInStores.Where(x => x.StoreId == DashboardMonitor_StoreDTO.Id).Select(x => x.Brand.Name).FirstOrDefault();
            }
            var AppUserIds = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            var AppUserQuery = DataContext.AppUser.AsNoTracking();
            AppUserQuery = AppUserQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            var appUserIds = await AppUserQuery.Select(x => x.Id).ToListWithNoLockAsync();
            AppUserIds = AppUserIds.Intersect(appUserIds).ToList();
            var AppUserFilter = new IdFilter { In = AppUserIds };

            List<long> StoreScoutingIds = await FilterStoreScouting(StoreScoutingService, OrganizationService, CurrentContext);
            var StoreScoutingStatusFilter = new IdFilter { Equal = StoreScoutingStatusEnum.NOTOPEN.Id };
            var StoreScoutingIdFilter = new IdFilter { In = StoreScoutingIds };
            var StoreScoutingQuery = DWContext.Fact_StoreScouting.AsNoTracking();

            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.DeletedAt == null);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.CreatorId, AppUserFilter);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.StoreScoutingStatusId, StoreScoutingStatusFilter);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.ProvinceId, ProvinceIdFilter);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.DistrictId, DistrictIdFilter);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            StoreScoutingQuery = StoreScoutingQuery.Where(x => x.CreatorId, AppUserFilter);
            List<DashboardStoreInformation_StoreDTO> DashboardMonitor_StoreScotingDTOs = await StoreScoutingQuery
                .Select(x => new DashboardStoreInformation_StoreDTO
                {
                    Id = x.StoreScoutingId,
                    Name = x.Name,
                    Address = x.Address,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Telephone = x.OwnerPhone,
                    IsScouting = true
                })
                .Distinct()
                .ToListWithNoLockAsync();
            DashboardMonitor_StoreDTOs.AddRange(DashboardMonitor_StoreScotingDTOs);
            return DashboardMonitor_StoreDTOs;
        }

        [Route(DashboardStoreInformationRoute.TopBrand), HttpPost]
        public async Task<List<DashboardStoreInformation_TopBrandDTO>> TopBrand([FromBody] DashboardStoreInformation_TopBrandFilterDTO DashboardStoreInformation_TopBrandFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long? BrandId = DashboardStoreInformation_TopBrandFilterDTO.BrandId?.Equal;
            long? ProvinceId = DashboardStoreInformation_TopBrandFilterDTO.ProvinceId?.Equal;
            long? DistrictId = DashboardStoreInformation_TopBrandFilterDTO.DistrictId?.Equal;
            long? AppUserId = DashboardStoreInformation_TopBrandFilterDTO.AppUserId?.Equal;
            var AppUserIdFilter = new IdFilter();
            if (AppUserId.HasValue) AppUserIdFilter.Equal = AppUserId;
            var Top = DashboardStoreInformation_TopBrandFilterDTO.Top?.Equal ?? 1; // mặc định ở tab Hạng 1
            var BrandIdFilter = new IdFilter();
            var ProvinceIdFilter = new IdFilter();
            var DistrictIdFilter = new IdFilter();
            var OrderDateFilter = new DateFilter { LessEqual = DashboardStoreInformation_TopBrandFilterDTO.OrderDate?.LessEqual };
            var TopFilter = new IdFilter { Equal = Top };
            if (BrandId.HasValue) BrandIdFilter.Equal = BrandId;
            if (ProvinceId.HasValue) ProvinceIdFilter.Equal = ProvinceId;
            if (DistrictId.HasValue) DistrictIdFilter.Equal = DistrictId;
            if (DashboardStoreInformation_TopBrandFilterDTO.OrganizationId == null)
                DashboardStoreInformation_TopBrandFilterDTO.OrganizationId = new IdFilter();
            var OrganizationFilter = DashboardStoreInformation_TopBrandFilterDTO.OrganizationId;
            List<long> OrganizationIds, StoreIds, AppuserIds;
            (OrganizationIds, StoreIds, AppuserIds) = await DynamicFilter(OrganizationFilter, ProvinceIdFilter, DistrictIdFilter, AppUserIdFilter);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };

            var BrandQuery = DataContext.Brand.AsNoTracking();
            BrandQuery = BrandQuery.Where(x => x.DeletedAt == null);
            List<long> TotalBrandIds = await BrandQuery.Select(x => x.Id).Distinct().ToListWithNoLockAsync();
            var TotalBrandIdFilter = new IdFilter { In = TotalBrandIds };

            List<long> ActiveStoreIds, ActiveBrandIds;
            (ActiveStoreIds, ActiveBrandIds) = await GetActiveIdFromHistory(StoreIdFilter, TotalBrandIdFilter, OrderDateFilter);
            StoreIdFilter = new IdFilter { In = ActiveStoreIds };
            TotalBrandIdFilter = new IdFilter { In = ActiveBrandIds };

            BrandQuery = BrandQuery.Where(x => x.Id, BrandIdFilter);
            var Brands = await BrandQuery.ToListWithNoLockAsync();

            var BrandInStoreQuery = DataContext.BrandInStore.AsNoTracking();
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.StoreId, StoreIdFilter);
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.BrandId, TotalBrandIdFilter);
            if (OrderDateFilter.LessEqual == null)
                BrandInStoreQuery = BrandInStoreQuery.Where(x => x.DeletedAt == null);
            else
                BrandInStoreQuery = BrandInStoreQuery.Where(x => (x.CreatedAt <= OrderDateFilter.LessEqual) && (x.DeletedAt == null || x.DeletedAt >= OrderDateFilter.LessEqual));
            List<long> BrandInStoreIds = await BrandInStoreQuery.Select(x => x.Id).ToListWithNoLockAsync();

            var BrandInStoreHistoryQuery = DataContext.BrandInStoreHistory.AsNoTracking();
            BrandInStoreHistoryQuery = BrandInStoreHistoryQuery.Where(x => x.CreatedAt, OrderDateFilter);
            BrandInStoreHistoryQuery = BrandInStoreHistoryQuery.Where(x => x.BrandInStoreId, new IdFilter { In = BrandInStoreIds });
            var BrandInStoreHistory = await BrandInStoreHistoryQuery.Select(x => new
            {
                x.BrandInStoreId,
                x.CreatedAt,
                x.Top,
            }).ToListWithNoLockAsync();
            List<BrandInStoreHistory> ActiveBrandInStore = BrandInStoreHistory.GroupBy(x => x.BrandInStoreId).Select(x => new BrandInStoreHistory
            { BrandInStoreId = x.Key, Top = x.OrderByDescending(x => x.CreatedAt).First().Top }).ToList();
            BrandInStoreIds = ActiveBrandInStore.Where(x => x.Top == Top).Select(x => x.BrandInStoreId).Distinct().ToList();

            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.Id, new IdFilter { In = BrandInStoreIds });
            var BrandInStores = await BrandInStoreQuery.Select(x => new BrandInStore
            {
                Id = x.Id,
                BrandId = x.BrandId,
                StoreId = x.StoreId
            }).ToListWithNoLockAsync();

            var SurveyedStoreCounter = BrandInStores.Select(x => x.StoreId).Distinct().Count();

            if (BrandId.HasValue) BrandInStores = BrandInStores.Where(x => x.BrandId == BrandId).ToList();

            List<DashboardStoreInformation_TopBrandDTO> DashboardStoreInformation_TopBrandDTOs = BrandInStores
                .GroupBy(x => x.BrandId)
                .Select(x => new DashboardStoreInformation_TopBrandDTO
                {
                    BrandId = x.Key,
                    Value = x.Select(x => x.StoreId).Distinct().Count()
                }).ToList();
            foreach (var DashboardStoreInformation_TopBrandDTO in DashboardStoreInformation_TopBrandDTOs)
            {
                DashboardStoreInformation_TopBrandDTO.Total = SurveyedStoreCounter;
                DashboardStoreInformation_TopBrandDTO.BrandName = Brands.Where(x => x.Id == DashboardStoreInformation_TopBrandDTO.BrandId)
                    .Select(x => x.Name).FirstOrDefault();
            }
            DashboardStoreInformation_TopBrandDTOs
                 = DashboardStoreInformation_TopBrandDTOs.OrderByDescending(x => x.Value).ToList();
            return DashboardStoreInformation_TopBrandDTOs;
        }

        [Route(DashboardStoreInformationRoute.ExportTopBrand), HttpPost]
        public async Task<ActionResult> ExportTopBrand([FromBody] DashboardStoreInformation_TopBrandFilterDTO DashboardStoreInformation_TopBrandFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long? BrandId = DashboardStoreInformation_TopBrandFilterDTO.BrandId?.Equal;
            long? ProvinceId = DashboardStoreInformation_TopBrandFilterDTO.ProvinceId?.Equal;
            long? DistrictId = DashboardStoreInformation_TopBrandFilterDTO.DistrictId?.Equal;
            var Top = DashboardStoreInformation_TopBrandFilterDTO.Top?.Equal ?? 1; // mặc định ở tab Hạng 1
            long? AppUserId = DashboardStoreInformation_TopBrandFilterDTO.AppUserId?.Equal;
            var AppUserIdFilter = new IdFilter();
            if (AppUserId.HasValue) AppUserIdFilter.Equal = AppUserId;
            var BrandIdFilter = new IdFilter();
            var ProvinceIdFilter = new IdFilter();
            var DistrictIdFilter = new IdFilter();
            var OrderDateFilter = new DateFilter { LessEqual = DashboardStoreInformation_TopBrandFilterDTO.OrderDate?.LessEqual };
            var TopFilter = new IdFilter { Equal = Top };
            if (BrandId.HasValue) BrandIdFilter.Equal = BrandId;
            if (ProvinceId.HasValue) ProvinceIdFilter.Equal = ProvinceId;
            if (DistrictId.HasValue) DistrictIdFilter.Equal = DistrictId;
            if (!BrandId.HasValue)
                return BadRequest("Chưa chọn hãng");
            if (DashboardStoreInformation_TopBrandFilterDTO.OrganizationId == null)
                DashboardStoreInformation_TopBrandFilterDTO.OrganizationId = new IdFilter();
            var OrganizationFilter = DashboardStoreInformation_TopBrandFilterDTO.OrganizationId;
            List<long> OrganizationIds, StoreIds, AppuserIds;
            (OrganizationIds, StoreIds, AppuserIds) = await DynamicFilter(OrganizationFilter, ProvinceIdFilter, DistrictIdFilter, AppUserIdFilter);
            IdFilter StoreIdFilter = new IdFilter { In = StoreIds };
            IdFilter OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            IdFilter StatusIdFilter = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            var BrandQuery = DataContext.Brand.AsNoTracking();
            BrandQuery = BrandQuery.Where(x => x.DeletedAt == null);
            BrandQuery = BrandQuery.Where(x => x.Id, BrandIdFilter);
            List<long> TotalBrandIds = await BrandQuery.Select(x => x.Id).Distinct().ToListWithNoLockAsync();
            var TotalBrandIdFilter = new IdFilter { In = TotalBrandIds };

            List<long> ActiveStoreIds, ActiveBrandIds;
            (ActiveStoreIds, ActiveBrandIds) = await GetActiveIdFromHistory(StoreIdFilter, TotalBrandIdFilter, OrderDateFilter);
            StoreIdFilter = new IdFilter { In = ActiveStoreIds };
            TotalBrandIdFilter = new IdFilter { In = ActiveBrandIds };

            var Brand = await BrandQuery.Where(x => x.Id, BrandIdFilter).FirstOrDefaultWithNoLockAsync();

            var BrandInStoreQuery = DataContext.BrandInStore.AsNoTracking();
            if (OrderDateFilter.LessEqual == null)
                BrandInStoreQuery = BrandInStoreQuery.Where(x => x.DeletedAt == null);
            else
                BrandInStoreQuery = BrandInStoreQuery.Where(x => (x.CreatedAt <= OrderDateFilter.LessEqual) && (x.DeletedAt == null || x.DeletedAt >= OrderDateFilter.LessEqual));
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.StoreId, StoreIdFilter);
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.BrandId, BrandIdFilter);
            List<long> BrandInStoreIds = await BrandInStoreQuery.Select(x => x.Id).ToListWithNoLockAsync();

            var BrandInStoreHistoryQuery = DataContext.BrandInStoreHistory.AsNoTracking();
            BrandInStoreHistoryQuery = BrandInStoreHistoryQuery.Where(x => x.CreatedAt, OrderDateFilter);
            BrandInStoreHistoryQuery = BrandInStoreHistoryQuery.Where(x => x.BrandInStoreId, new IdFilter { In = BrandInStoreIds });
            var BrandInStoreHistory = await BrandInStoreHistoryQuery.Select(x => new
            {
                x.BrandInStoreId,
                x.CreatedAt,
                x.Top,
            }).ToListWithNoLockAsync();
            List<BrandInStoreHistory> ActiveBrandInStore = BrandInStoreHistory.GroupBy(x => x.BrandInStoreId).Select(x => new BrandInStoreHistory
            { BrandInStoreId = x.Key, Top = x.OrderByDescending(x => x.CreatedAt).First().Top }).ToList();
            BrandInStoreIds = ActiveBrandInStore.Where(x => x.Top == Top).Select(x => x.BrandInStoreId).Distinct().ToList();

            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.Id, new IdFilter { In = BrandInStoreIds });
            var BrandInStores = await BrandInStoreQuery.Select(x => new BrandInStore
            {
                Id = x.Id,
                BrandId = x.BrandId,
                StoreId = x.StoreId
            }).ToListWithNoLockAsync();

            List<long> SurveyedStoreIds = BrandInStores.Select(x => x.StoreId).Distinct().ToList();
            var SurveyedStoreCounter = SurveyedStoreIds.Count();
            var StoreQuery = DataContext.Store.AsNoTracking();
            StoreIdFilter.In = SurveyedStoreIds;
            StoreQuery = StoreQuery.Where(x => x.Id, StoreIdFilter);
            var Stores = await StoreQuery.ToListWithNoLockAsync();

            List<DashboardStoreInformation_StoreDTO> DashboardStoreInformation_StoreDTOs = Stores.Select(x => new DashboardStoreInformation_StoreDTO
            {
                Id = x.Id,
            }).Distinct().ToList();

            foreach (var DashboardStoreInformation_StoreDTO in DashboardStoreInformation_StoreDTOs)
            {
                var store = Stores.Where(x => x.Id == DashboardStoreInformation_StoreDTO.Id).FirstOrDefault();
                DashboardStoreInformation_StoreDTO.Code = store.Code;
                DashboardStoreInformation_StoreDTO.CodeDraft = store.CodeDraft;
                DashboardStoreInformation_StoreDTO.Name = store.Name;
                DashboardStoreInformation_StoreDTO.Telephone = store.Telephone;
                DashboardStoreInformation_StoreDTO.Address = store.Address;
                DashboardStoreInformation_StoreDTO.OrganizationId = store.OrganizationId;
            }
            var OrganizationQuery = DataContext.Organization.AsNoTracking();
            OrganizationIds = Stores.Select(x => x.OrganizationId).Distinct().ToList();
            OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            OrganizationQuery = DataContext.Organization.AsNoTracking();
            OrganizationQuery = OrganizationQuery.Where(x => x.Id, OrganizationIdFilter);
            OrganizationQuery = OrganizationQuery.Where(x => x.StatusId, StatusIdFilter);
            OrganizationQuery = OrganizationQuery.Where(x => x.DeletedAt == null);
            OrganizationQuery = OrganizationQuery.OrderBy(x => x.Id);
            var Organizations = await OrganizationQuery.ToListWithNoLockAsync();

            List<DashboardStoreInformation_StoreExportDTO> DashboardStoreInformation_StoreExportDTOs = new List<DashboardStoreInformation_StoreExportDTO>();
            foreach (var Organization in Organizations)
            {
                DashboardStoreInformation_StoreExportDTO DashboardStoreInformation_StoreExportDTO = new DashboardStoreInformation_StoreExportDTO();
                DashboardStoreInformation_StoreExportDTO.OrganizationId = Organization.Id;
                DashboardStoreInformation_StoreExportDTO.OrganizationName = Organization.Name;
                DashboardStoreInformation_StoreExportDTO.Stores = DashboardStoreInformation_StoreDTOs.Where(x => x.OrganizationId == Organization.Id).OrderBy(x => x.Code).ToList();
                DashboardStoreInformation_StoreExportDTOs.Add(DashboardStoreInformation_StoreExportDTO);
            }
            long stt = 1;
            foreach (var DashboardStoreInformation_StoreExportDTO in DashboardStoreInformation_StoreExportDTOs)
            {
                foreach (var Store in DashboardStoreInformation_StoreExportDTO.Stores)
                {
                    Store.STT = stt++;
                }
            }
            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            var Total = DashboardStoreInformation_StoreDTOs.Count();

            string path = "Templates/Dashboard_StoreInformation_TopBrand.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Brand = Brand;
            Data.Top = Top;
            Data.Data = DashboardStoreInformation_StoreExportDTOs;
            Data.Total = Total;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "StoreInformation_TopBrand.xlsx");
        }

        [Route(DashboardStoreInformationRoute.ProductGroupingNumberStatistic), HttpPost]
        public async Task<List<DashboardStoreInformation_ProductGroupingNumberStatisticsDTO>> ProductGroupingNumberStatistic([FromBody] DashboardStoreInformation_ProductGroupingNumberStatisticsFilterDTO DashboardStoreInformation_ProductGroupingStatisticsFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long? BrandId = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.BrandId?.Equal;
            long? ProvinceId = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.ProvinceId?.Equal;
            long? DistrictId = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.DistrictId?.Equal;
            long? AppUserId = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.AppUserId?.Equal;
            var AppUserIdFilter = new IdFilter();
            if (AppUserId.HasValue) AppUserIdFilter.Equal = AppUserId;
            var BrandIdFilter = new IdFilter();
            var ProvinceIdFilter = new IdFilter();
            var DistrictIdFilter = new IdFilter();
            var OrderDateFilter = new DateFilter { LessEqual = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.OrderDate?.LessEqual };
            if (BrandId.HasValue) BrandIdFilter.Equal = BrandId;
            if (ProvinceId.HasValue) ProvinceIdFilter.Equal = ProvinceId;
            if (DistrictId.HasValue) DistrictIdFilter.Equal = DistrictId;
            if (DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.OrganizationId == null)
                DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.OrganizationId = new IdFilter();
            var OrganizationFilter = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.OrganizationId;
            List<long> OrganizationIds, StoreIds, AppuserIds;
            (OrganizationIds, StoreIds, AppuserIds) = await DynamicFilter(OrganizationFilter, ProvinceIdFilter, DistrictIdFilter, AppUserIdFilter);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };

            var BrandQuery = DataContext.Brand.AsNoTracking();
            BrandQuery = BrandQuery.Where(x => x.DeletedAt == null);
            BrandQuery = BrandQuery.Where(x => x.Id, BrandIdFilter);
            List<long> TotalBrandIds = await BrandQuery.Select(x => x.Id).Distinct().ToListWithNoLockAsync();
            BrandIdFilter = new IdFilter { In = TotalBrandIds };

            List<long> ActiveStoreIds, ActiveBrandIds;
            (ActiveStoreIds, ActiveBrandIds) = await GetActiveIdFromHistory(StoreIdFilter, BrandIdFilter, OrderDateFilter);
            StoreIdFilter = new IdFilter { In = ActiveStoreIds };
            BrandIdFilter = new IdFilter { In = ActiveBrandIds };

            BrandQuery = BrandQuery.Where(x => x.Id, BrandIdFilter);
            var Brands = await BrandQuery.ToListWithNoLockAsync();

            var BrandInStoreQuery = DataContext.BrandInStore.AsNoTracking();
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.StoreId, StoreIdFilter);
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.BrandId, BrandIdFilter);
            if (OrderDateFilter.LessEqual == null)
                BrandInStoreQuery = BrandInStoreQuery.Where(x => x.DeletedAt == null);
            else
                BrandInStoreQuery = BrandInStoreQuery.Where(x => (x.CreatedAt <= OrderDateFilter.LessEqual) && (x.DeletedAt == null || x.DeletedAt >= OrderDateFilter.LessEqual));

            var SurveyedStoreIds = await BrandInStoreQuery.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();
            var SurveyedBrandIds = await BrandInStoreQuery.Select(x => x.BrandId).Distinct().ToListWithNoLockAsync();

            var ProductGroupingStatistics = await BrandInStoreQuery
                .GroupBy(x => x.BrandId)
                .Select(x => new DashboardStoreInformation_ProductGroupingNumberStatisticsDTO
                {
                    BrandId = x.Key,
                    Total = x.Count()
                }).ToListWithNoLockAsync();
            foreach (var ProductGroupingStatistic in ProductGroupingStatistics)
            {
                ProductGroupingStatistic.BrandName = Brands.Where(x => x.Id == ProductGroupingStatistic.BrandId).FirstOrDefault().Name;
            }

            var BrandInStores = await BrandInStoreQuery.Select(x => new
            {
                BrandInStoreId = x.Id,
                StoreId = x.StoreId,
                BrandId = x.BrandId
            }).Distinct().ToListWithNoLockAsync();
            var BrandInStoreIds = BrandInStores.Select(x => x.BrandInStoreId).ToList();
            var BrandInStoreIdFilter = new IdFilter { In = BrandInStoreIds };

            var ProductGroupingQuery = DataContext.ProductGrouping.AsNoTracking();
            if (OrderDateFilter.LessEqual == null)
                ProductGroupingQuery = ProductGroupingQuery.Where(x => x.DeletedAt == null);
            else
                ProductGroupingQuery = ProductGroupingQuery.Where(x => (x.CreatedAt <= OrderDateFilter.LessEqual) && (x.DeletedAt == null || x.DeletedAt >= OrderDateFilter.LessEqual));
            var ProductGroupingIds = await ProductGroupingQuery.Select(x => x.Id).Distinct().ToListWithNoLockAsync();
            var ProductGroupingIdFilter = new IdFilter { In = ProductGroupingIds };

            var ProductGroupingHistoryQuery = DataContext.ProductGroupingHistory.AsTracking();
            ProductGroupingHistoryQuery = ProductGroupingHistoryQuery.Where(x => x.CreatedAt, OrderDateFilter);
            ProductGroupingHistoryQuery = ProductGroupingHistoryQuery.Where(x => x.ProductGroupingId, ProductGroupingIdFilter);
            var ProductGroupingHistory = await ProductGroupingHistoryQuery.Select(x => new
            {
                x.ProductGroupingId,
                x.CreatedAt,
                x.StatusId,
            }).ToListWithNoLockAsync();
            var ActiveProductGroupings = ProductGroupingHistory.GroupBy(x => x.ProductGroupingId).Select(x => new
            { ProductGroupingId = x.Key, x.OrderByDescending(x => x.CreatedAt).First().StatusId }).ToList();
            List<long> ActiveProductGroupingIds = ActiveProductGroupings.Where(x => x.StatusId == StatusEnum.ACTIVE.Id).Select(x => x.ProductGroupingId).Distinct().ToList();
            ProductGroupingIdFilter.In = ActiveProductGroupingIds;

            var BrandInStoreProductGroupingMappingQuery = DataContext.BrandInStoreProductGroupingMapping.AsNoTracking();
            BrandInStoreProductGroupingMappingQuery = BrandInStoreProductGroupingMappingQuery.Where(x => x.BrandInStoreId, BrandInStoreIdFilter);
            BrandInStoreProductGroupingMappingQuery = BrandInStoreProductGroupingMappingQuery.Where(x => x.ProductGroupingId, ProductGroupingIdFilter);
            var BrandInStoreProductGroupingMapping = await BrandInStoreProductGroupingMappingQuery
               .Select(x => new
               {
                   x.BrandInStoreId,
                   x.ProductGroupingId,
                   x.BrandInStore.StoreId,
                   x.BrandInStore.BrandId
               }).ToListWithNoLockAsync();

            var DashboardStoreInformation_ProductGroupingStatisticsDTOs = ProductGroupingStatistics.Where(x => BrandId.HasValue == false || x.BrandId == BrandId).ToList();

            foreach (var DashboardStoreInformation_ProductGroupingStatisticsDTO in DashboardStoreInformation_ProductGroupingStatisticsDTOs)
            {
                var subBrandInStoreProductGroupingMapping = BrandInStoreProductGroupingMapping
                    .Where(x => x.BrandId == DashboardStoreInformation_ProductGroupingStatisticsDTO.BrandId)
                    .ToList();

                var aggregate = subBrandInStoreProductGroupingMapping
                    .GroupBy(x => x.StoreId)
                    .Select(x => new { StoreId = x.Key, ProductGroupingCounter = x.Count() })
                    .ToList();
                DashboardStoreInformation_ProductGroupingStatisticsDTO.Value3 = aggregate.Where(x => x.ProductGroupingCounter >= 3).Count();
                DashboardStoreInformation_ProductGroupingStatisticsDTO.Value4 = aggregate.Where(x => x.ProductGroupingCounter >= 4).Count();
                DashboardStoreInformation_ProductGroupingStatisticsDTO.Value5 = aggregate.Where(x => x.ProductGroupingCounter >= 5).Count();
                DashboardStoreInformation_ProductGroupingStatisticsDTO.Value6 = aggregate.Where(x => x.ProductGroupingCounter >= 6).Count();
                DashboardStoreInformation_ProductGroupingStatisticsDTO.Value7 = aggregate.Where(x => x.ProductGroupingCounter >= 7).Count();
                DashboardStoreInformation_ProductGroupingStatisticsDTO.Value8 = aggregate.Where(x => x.ProductGroupingCounter >= 8).Count();
            }
            DashboardStoreInformation_ProductGroupingStatisticsDTOs
                 = DashboardStoreInformation_ProductGroupingStatisticsDTOs.OrderByDescending(x => x.Total).ToList();
            return DashboardStoreInformation_ProductGroupingStatisticsDTOs;
        }

        [Route(DashboardStoreInformationRoute.ExportProductGroupingNumberStatistic), HttpPost]
        public async Task<ActionResult> ExportProductGroupingNumberStatistic([FromBody] DashboardStoreInformation_ProductGroupingNumberStatisticsFilterDTO DashboardStoreInformation_ProductGroupingStatisticsFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long? BrandId = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.BrandId?.Equal;
            long? ProvinceId = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.ProvinceId?.Equal;
            long? DistrictId = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.DistrictId?.Equal;
            long? AppUserId = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.AppUserId?.Equal;
            var AppUserIdFilter = new IdFilter();
            if (AppUserId.HasValue) AppUserIdFilter.Equal = AppUserId;
            var BrandIdFilter = new IdFilter();
            var ProvinceIdFilter = new IdFilter();
            var DistrictIdFilter = new IdFilter();
            var OrderDateFilter = new DateFilter { LessEqual = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.OrderDate?.LessEqual };
            if (BrandId.HasValue) BrandIdFilter.Equal = BrandId;
            if (ProvinceId.HasValue) ProvinceIdFilter.Equal = ProvinceId;
            if (DistrictId.HasValue) DistrictIdFilter.Equal = DistrictId;
            if (!BrandId.HasValue)
                return BadRequest("Chưa nhập thông tin hãng");
            if (DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.OrganizationId == null)
                DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.OrganizationId = new IdFilter();
            var OrganizationFilter = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.OrganizationId;
            List<long> OrganizationIds, StoreIds, AppuserIds;
            (OrganizationIds, StoreIds, AppuserIds) = await DynamicFilter(OrganizationFilter, ProvinceIdFilter, DistrictIdFilter, AppUserIdFilter);
            IdFilter StoreIdFilter = new IdFilter { In = StoreIds };
            IdFilter OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            IdFilter StatusIdFilter = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            var BrandQuery = DataContext.Brand.AsNoTracking();
            BrandQuery = BrandQuery.Where(x => x.DeletedAt == null);
            BrandQuery = BrandQuery.Where(x => x.Id, BrandIdFilter);
            List<long> TotalBrandIds = await BrandQuery.Select(x => x.Id).Distinct().ToListWithNoLockAsync();
            var TotalBrandIdFilter = new IdFilter { In = TotalBrandIds };

            List<long> ActiveStoreIds, ActiveBrandIds;
            (ActiveStoreIds, ActiveBrandIds) = await GetActiveIdFromHistory(StoreIdFilter, TotalBrandIdFilter, OrderDateFilter);
            StoreIdFilter = new IdFilter { In = ActiveStoreIds };
            TotalBrandIdFilter = new IdFilter { In = ActiveBrandIds };

            var Brand = await BrandQuery.Where(x => x.Id, BrandIdFilter).FirstOrDefaultWithNoLockAsync();

            var BrandInStoreQuery = DataContext.BrandInStore.AsNoTracking();
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.StoreId, StoreIdFilter);
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.BrandId, BrandIdFilter);
            if (OrderDateFilter.LessEqual == null)
                BrandInStoreQuery = BrandInStoreQuery.Where(x => x.DeletedAt == null);
            else
                BrandInStoreQuery = BrandInStoreQuery.Where(x => (x.CreatedAt <= OrderDateFilter.LessEqual) && (x.DeletedAt == null || x.DeletedAt >= OrderDateFilter.LessEqual));

            var SurveyedStoreIds = await BrandInStoreQuery.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();
            var SurveyedBrandIds = await BrandInStoreQuery.Select(x => x.BrandId).Distinct().ToListWithNoLockAsync();
            var StoreQuery = DataContext.Store.AsNoTracking();
            StoreQuery = StoreQuery.Where(x => x.Id, new IdFilter { In = SurveyedStoreIds });
            var Stores = await StoreQuery.Select(x => new
            {
                x.Id,
                x.Code,
                x.CodeDraft,
                x.Name,
                x.Telephone,
                x.Address,
                x.OrganizationId,
            }).ToListWithNoLockAsync();

            var BrandInStores = await BrandInStoreQuery.Select(x => new
            {
                BrandInStoreId = x.Id,
                StoreId = x.StoreId,
                BrandId = x.BrandId
            }).Distinct().ToListWithNoLockAsync();
            var BrandInStoreIds = BrandInStores.Select(x => x.BrandInStoreId).ToList();
            var BrandInStoreIdFilter = new IdFilter { In = BrandInStoreIds };

            var ProductGroupingQuery = DataContext.ProductGrouping.AsNoTracking();
            if (OrderDateFilter.LessEqual == null)
                ProductGroupingQuery = ProductGroupingQuery.Where(x => x.DeletedAt == null);
            else
                ProductGroupingQuery = ProductGroupingQuery.Where(x => (x.CreatedAt <= OrderDateFilter.LessEqual) && (x.DeletedAt == null || x.DeletedAt >= OrderDateFilter.LessEqual));
            var ProductGroupingIds = await ProductGroupingQuery.Select(x => x.Id).Distinct().ToListWithNoLockAsync();
            var ProductGroupingIdFilter = new IdFilter { In = ProductGroupingIds };

            var ProductGroupingHistoryQuery = DataContext.ProductGroupingHistory.AsTracking();
            ProductGroupingHistoryQuery = ProductGroupingHistoryQuery.Where(x => x.CreatedAt, OrderDateFilter);
            ProductGroupingHistoryQuery = ProductGroupingHistoryQuery.Where(x => x.ProductGroupingId, ProductGroupingIdFilter);
            var ProductGroupingHistory = await ProductGroupingHistoryQuery.ToListWithNoLockAsync();
            var ActiveProductGroupings = ProductGroupingHistory.GroupBy(x => x.ProductGroupingId).Select(x => new
            { ProductGroupingId = x.Key, x.OrderByDescending(x => x.CreatedAt).First().StatusId }).ToList();
            List<long> ActiveProductGroupingIds = ActiveProductGroupings.Where(x => x.StatusId == StatusEnum.ACTIVE.Id).Select(x => x.ProductGroupingId).Distinct().ToList();
            ProductGroupingIdFilter.In = ActiveProductGroupingIds;

            var BrandInStoreProductGroupingMappingQuery = DataContext.BrandInStoreProductGroupingMapping.AsNoTracking();
            BrandInStoreProductGroupingMappingQuery = BrandInStoreProductGroupingMappingQuery.Where(x => x.BrandInStoreId, BrandInStoreIdFilter);
            BrandInStoreProductGroupingMappingQuery = BrandInStoreProductGroupingMappingQuery.Where(x => x.ProductGroupingId, ProductGroupingIdFilter);
            var BrandInStoreProductGroupingMapping = await BrandInStoreProductGroupingMappingQuery
               .Select(x => new
               {
                   x.BrandInStoreId,
                   x.ProductGroupingId,
                   x.BrandInStore.StoreId,
                   x.BrandInStore.BrandId
               }).OrderBy(x => x.StoreId).ToListWithNoLockAsync();
            var OrganizationQuery = DataContext.Organization.AsNoTracking();
            OrganizationIds = Stores.Select(x => x.OrganizationId).Distinct().ToList();
            OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            OrganizationQuery = DataContext.Organization.AsNoTracking();
            OrganizationQuery = OrganizationQuery.Where(x => x.Id, OrganizationIdFilter);
            OrganizationQuery = OrganizationQuery.Where(x => x.StatusId, StatusIdFilter);
            OrganizationQuery = OrganizationQuery.Where(x => x.DeletedAt == null);
            OrganizationQuery = OrganizationQuery.OrderBy(x => x.Id);
            var Organizations = await OrganizationQuery.ToListWithNoLockAsync();

            List<DashboardStoreInformation_StoreDTO> DashboardStoreInformation_StoreDTOs = Stores.Select(store => new DashboardStoreInformation_StoreDTO
            {
                Id = store.Id,
                Code = store.Code,
                CodeDraft = store.CodeDraft,
                Name = store.Name,
                Telephone = store.Telephone,
                Address = store.Address,
                OrganizationId = store.OrganizationId,
            }).OrderBy(x => x.Id).ToList();

            int StoreIndex1 = 0;
            int StoreIndex2 = 0;

            while (StoreIndex1 < DashboardStoreInformation_StoreDTOs.Count && StoreIndex2 < BrandInStoreProductGroupingMapping.Count)
            {
                if (DashboardStoreInformation_StoreDTOs[StoreIndex1].Id < BrandInStoreProductGroupingMapping[StoreIndex2].StoreId)
                    StoreIndex1++;
                else if (DashboardStoreInformation_StoreDTOs[StoreIndex1].Id > BrandInStoreProductGroupingMapping[StoreIndex2].StoreId)
                    StoreIndex2++;
                else if (DashboardStoreInformation_StoreDTOs[StoreIndex1].Id == BrandInStoreProductGroupingMapping[StoreIndex2].StoreId)
                {
                    DashboardStoreInformation_StoreDTOs[StoreIndex1].ProductGroupingCounter++;
                    StoreIndex2++;
                }
            }

            List<DashboardStoreInformation_ProductGroupingNumberStatisticsExportDTO> DashboardStoreInformation_ProductGroupingStatisticsExportDTOs = new List<DashboardStoreInformation_ProductGroupingNumberStatisticsExportDTO>();
            foreach (var Organization in Organizations)
            {
                DashboardStoreInformation_ProductGroupingNumberStatisticsExportDTO DashboardStoreInformation_ProductGroupingStatisticsExportDTO = new DashboardStoreInformation_ProductGroupingNumberStatisticsExportDTO();
                DashboardStoreInformation_ProductGroupingStatisticsExportDTO.OrganizationId = Organization.Id;
                DashboardStoreInformation_ProductGroupingStatisticsExportDTO.OrganizationName = Organization.Name;
                DashboardStoreInformation_ProductGroupingStatisticsExportDTO.Stores = DashboardStoreInformation_StoreDTOs.Where(x => x.OrganizationId == Organization.Id).OrderBy(x => x.Code).ToList();
                DashboardStoreInformation_ProductGroupingStatisticsExportDTOs.Add(DashboardStoreInformation_ProductGroupingStatisticsExportDTO);
            }

            long stt = 1;
            foreach (var DashboardStoreInformation_ProductGroupingStatisticsExportDTO in DashboardStoreInformation_ProductGroupingStatisticsExportDTOs)
            {
                foreach (var Store in DashboardStoreInformation_ProductGroupingStatisticsExportDTO.Stores)
                {
                    Store.STT = stt++;
                }
            }
            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            var Total = new
            {
                Rate8 = DashboardStoreInformation_StoreDTOs.Where(x => x.Rate8 == "x").Count(),
                Rate7 = DashboardStoreInformation_StoreDTOs.Where(x => x.Rate7 == "x").Count(),
                Rate6 = DashboardStoreInformation_StoreDTOs.Where(x => x.Rate6 == "x").Count(),
                Rate5 = DashboardStoreInformation_StoreDTOs.Where(x => x.Rate5 == "x").Count(),
                Rate4 = DashboardStoreInformation_StoreDTOs.Where(x => x.Rate4 == "x").Count(),
                Rate3 = DashboardStoreInformation_StoreDTOs.Where(x => x.Rate3 == "x").Count(),
            };

            string path = "Templates/Dashboard_StoreInformation_ProductGrouping.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Brand = Brand;
            Data.Data = DashboardStoreInformation_ProductGroupingStatisticsExportDTOs;
            Data.Total = Total;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "StoreInformation_ProductGroupingNumber.xlsx");
        }

        [Route(DashboardStoreInformationRoute.ProductGroupingStatistic), HttpPost]
        public async Task<List<DashboardStoreInformation_ProductGroupingStatisticsDTO>> ProductGroupingStatistic([FromBody] DashboardStoreInformation_ProductGroupingStatisticsFilterDTO DashboardStoreInformation_ProductGroupingStatisticsFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long? BrandId = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.BrandId?.Equal;
            long? ProvinceId = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.ProvinceId?.Equal;
            long? DistrictId = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.DistrictId?.Equal;
            long? AppUserId = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.AppUserId?.Equal;
            var AppUserIdFilter = new IdFilter();
            if (AppUserId.HasValue) AppUserIdFilter.Equal = AppUserId;
            var BrandIdFilter = new IdFilter();
            var ProvinceIdFilter = new IdFilter();
            var DistrictIdFilter = new IdFilter();
            var OrderDateFilter = new DateFilter { LessEqual = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.OrderDate?.LessEqual };
            if (BrandId.HasValue) BrandIdFilter.Equal = BrandId;
            if (ProvinceId.HasValue) ProvinceIdFilter.Equal = ProvinceId;
            if (DistrictId.HasValue) DistrictIdFilter.Equal = DistrictId;
            if (DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.OrganizationId == null)
                DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.OrganizationId = new IdFilter();
            var OrganizationFilter = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.OrganizationId;
            List<long> OrganizationIds, StoreIds, AppuserIds;
            (OrganizationIds, StoreIds, AppuserIds) = await DynamicFilter(OrganizationFilter, ProvinceIdFilter, DistrictIdFilter, AppUserIdFilter);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };

            var BrandQuery = DataContext.Brand.AsNoTracking();
            BrandQuery = BrandQuery.Where(x => x.DeletedAt == null);
            BrandQuery = BrandQuery.Where(x => x.Id, BrandIdFilter);
            List<long> TotalBrandIds = await BrandQuery.Select(x => x.Id).Distinct().ToListWithNoLockAsync();
            BrandIdFilter = new IdFilter { In = TotalBrandIds };

            List<long> ActiveStoreIds, ActiveBrandIds;
            (ActiveStoreIds, ActiveBrandIds) = await GetActiveIdFromHistory(StoreIdFilter, BrandIdFilter, OrderDateFilter);
            StoreIdFilter = new IdFilter { In = ActiveStoreIds };
            BrandIdFilter = new IdFilter { In = ActiveBrandIds };

            BrandQuery = BrandQuery.Where(x => x.Id, BrandIdFilter);

            var BrandInStoreQuery = DataContext.BrandInStore.AsNoTracking();
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.StoreId, StoreIdFilter);
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.BrandId, BrandIdFilter);
            if (OrderDateFilter.LessEqual == null)
                BrandInStoreQuery = BrandInStoreQuery.Where(x => x.DeletedAt == null);
            else
                BrandInStoreQuery = BrandInStoreQuery.Where(x => (x.CreatedAt <= OrderDateFilter.LessEqual) && (x.DeletedAt == null || x.DeletedAt >= OrderDateFilter.LessEqual));

            var SurveyedStoreIds = await BrandInStoreQuery.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();
            var SurveyedBrandIds = await BrandInStoreQuery.Select(x => x.BrandId).Distinct().ToListWithNoLockAsync();

            var BrandInStores = await BrandInStoreQuery.Select(x => new
            {
                x.Id,
                x.StoreId,
                x.BrandId
            }).Distinct().ToListWithNoLockAsync();
            var BrandInStoreIds = BrandInStores.Select(x => x.Id).Distinct().ToList();
            var BrandInStoreIdFilter = new IdFilter { In = BrandInStoreIds };

            #region Check active Product grouping
            var ProductGroupingQuery = DataContext.ProductGrouping.AsNoTracking();
            if (OrderDateFilter.LessEqual == null)
                ProductGroupingQuery = ProductGroupingQuery.Where(x => x.DeletedAt == null);
            else
                ProductGroupingQuery = ProductGroupingQuery.Where(x => (x.CreatedAt <= OrderDateFilter.LessEqual) && (x.DeletedAt == null || x.DeletedAt >= OrderDateFilter.LessEqual));
            var ProductGroupingIds = await ProductGroupingQuery.Select(x => x.Id).Distinct().ToListWithNoLockAsync();
            var ProductGroupingIdFilter = new IdFilter { In = ProductGroupingIds };

            var ProductGroupingHistoryQuery = DataContext.ProductGroupingHistory.AsTracking();
            ProductGroupingHistoryQuery = ProductGroupingHistoryQuery.Where(x => x.CreatedAt, OrderDateFilter);
            ProductGroupingHistoryQuery = ProductGroupingHistoryQuery.Where(x => x.ProductGroupingId, ProductGroupingIdFilter);
            var ProductGroupingHistory = await ProductGroupingHistoryQuery.Select(x => new
            {
                x.ProductGroupingId,
                x.CreatedAt,
                x.StatusId,
            }).ToListWithNoLockAsync();
            var ActiveProductGroupings = ProductGroupingHistory.GroupBy(x => x.ProductGroupingId).Select(x => new
            { ProductGroupingId = x.Key, x.OrderByDescending(x => x.CreatedAt).First().StatusId }).ToList();

            List<long> ActiveProductGroupingIds = ActiveProductGroupings.Where(x => x.StatusId == StatusEnum.ACTIVE.Id).Select(x => x.ProductGroupingId).Distinct().ToList();
            ProductGroupingIdFilter.In = ActiveProductGroupingIds;
            #endregion

            var BrandInStoreProductGroupingMappingQuery = DataContext.BrandInStoreProductGroupingMapping.AsNoTracking();
            BrandInStoreProductGroupingMappingQuery = BrandInStoreProductGroupingMappingQuery.Where(x => x.BrandInStoreId, BrandInStoreIdFilter);
            BrandInStoreProductGroupingMappingQuery = BrandInStoreProductGroupingMappingQuery.Where(x => x.ProductGroupingId, ProductGroupingIdFilter);

            var BrandInStoreProductGroupingMapping = await BrandInStoreProductGroupingMappingQuery
                .Select(x => new
                {
                    x.BrandInStoreId,
                    x.ProductGroupingId,
                    x.BrandInStore.StoreId,
                    x.BrandInStore.BrandId
                }).Distinct().ToListWithNoLockAsync();
            ProductGroupingIds = BrandInStoreProductGroupingMapping.Select(x => x.ProductGroupingId).Distinct().ToList();
            ActiveBrandIds = BrandInStoreProductGroupingMapping.Select(x => x.BrandId).Distinct().ToList();
            BrandIdFilter.In = ActiveBrandIds;
            ProductGroupingIdFilter.In = ProductGroupingIds;

            ProductGroupingQuery = ProductGroupingQuery.Where(x => x.Id, ProductGroupingIdFilter);
            var ProductGroupings = await ProductGroupingQuery.Select(x => new
            {
                x.Id,
                x.Name,
            }).ToListWithNoLockAsync();

            BrandQuery = BrandQuery.Where(x => x.Id, BrandIdFilter);
            var Brands = await BrandQuery.ToListWithNoLockAsync();

            List<DashboardStoreInformation_ProductGroupingStatisticsDTO> DashboardStoreInformation_ProductGroupingStatisticsDTOs =
                new List<DashboardStoreInformation_ProductGroupingStatisticsDTO>();
            if (Brands == null || Brands.Count == 0)
                return DashboardStoreInformation_ProductGroupingStatisticsDTOs;
            foreach (var Brand in Brands)
            {
                DashboardStoreInformation_ProductGroupingStatisticsDTO DashboardStoreInformation_ProductGroupingStatisticsDTO = new DashboardStoreInformation_ProductGroupingStatisticsDTO();

                DashboardStoreInformation_ProductGroupingStatisticsDTO.Total = BrandInStores.Where(x => x.BrandId == Brand.Id)
                    .Select(x => x.StoreId).Distinct().Count();
                DashboardStoreInformation_ProductGroupingStatisticsDTO.BrandId = Brand.Id;
                DashboardStoreInformation_ProductGroupingStatisticsDTO.BrandName = Brand.Name;
                DashboardStoreInformation_ProductGroupingStatisticsDTO.ProductGroupings = new List<DashboardStoreInformation_ProductGroupingDTO>();

                var subBrandInStoreProductGroupingMapping = BrandInStoreProductGroupingMapping.Where(x => x.BrandId == Brand.Id).ToList();

                foreach (var ProductGrouping in ProductGroupings)
                {
                    long StoreCounter = subBrandInStoreProductGroupingMapping.Where(x => x.ProductGroupingId == ProductGrouping.Id)
                        .Select(x => x.StoreId).Distinct().Count();

                    DashboardStoreInformation_ProductGroupingDTO DashboardStoreInformation_ProductGroupingDTO = new DashboardStoreInformation_ProductGroupingDTO
                    {
                        ProductGroupingId = ProductGrouping.Id,
                        ProductGroupingName = ProductGrouping.Name,
                        Total = DashboardStoreInformation_ProductGroupingStatisticsDTO.Total,
                        Value = StoreCounter
                    };

                    DashboardStoreInformation_ProductGroupingStatisticsDTO.ProductGroupings.Add(DashboardStoreInformation_ProductGroupingDTO);
                }

                DashboardStoreInformation_ProductGroupingStatisticsDTOs.Add(DashboardStoreInformation_ProductGroupingStatisticsDTO);
            }

            DashboardStoreInformation_ProductGroupingStatisticsDTOs = DashboardStoreInformation_ProductGroupingStatisticsDTOs.OrderByDescending(x => x.Total).ToList();
            DashboardStoreInformation_ProductGroupingStatisticsDTOs[0].ProductGroupings = DashboardStoreInformation_ProductGroupingStatisticsDTOs[0].ProductGroupings.OrderByDescending(x => x.Value).ToList();
            return DashboardStoreInformation_ProductGroupingStatisticsDTOs;
        }

        [Route(DashboardStoreInformationRoute.ExportProductGroupingStatistic), HttpPost]
        public async Task<ActionResult> ExportProductGroupingStatistic([FromBody] DashboardStoreInformation_ProductGroupingStatisticsFilterDTO DashboardStoreInformation_ProductGroupingStatisticsFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long? BrandId = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.BrandId?.Equal;
            long? ProvinceId = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.ProvinceId?.Equal;
            long? DistrictId = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.DistrictId?.Equal;
            long? AppUserId = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.AppUserId?.Equal;
            var AppUserIdFilter = new IdFilter();
            if (AppUserId.HasValue) AppUserIdFilter.Equal = AppUserId;
            var BrandIdFilter = new IdFilter();
            var ProvinceIdFilter = new IdFilter();
            var DistrictIdFilter = new IdFilter();
            var OrderDateFilter = new DateFilter { LessEqual = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.OrderDate?.LessEqual };
            if (BrandId.HasValue) BrandIdFilter.Equal = BrandId;
            if (ProvinceId.HasValue) ProvinceIdFilter.Equal = ProvinceId;
            if (DistrictId.HasValue) DistrictIdFilter.Equal = DistrictId;
            if (!BrandId.HasValue)
                return BadRequest("Chưa chọn hãng");
            if (DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.OrganizationId == null)
                DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.OrganizationId = new IdFilter();
            var OrganizationFilter = DashboardStoreInformation_ProductGroupingStatisticsFilterDTO.OrganizationId;
            List<long> OrganizationIds, StoreIds, AppuserIds;
            (OrganizationIds, StoreIds, AppuserIds) = await DynamicFilter(OrganizationFilter, ProvinceIdFilter, DistrictIdFilter, AppUserIdFilter);
            IdFilter StoreIdFilter = new IdFilter { In = StoreIds };
            IdFilter OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            IdFilter StatusIdFilter = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            var BrandQuery = DataContext.Brand.AsNoTracking();
            BrandQuery = BrandQuery.Where(x => x.DeletedAt == null);
            BrandQuery = BrandQuery.Where(x => x.Id, BrandIdFilter);
            List<long> TotalBrandIds = await BrandQuery.Select(x => x.Id).Distinct().ToListWithNoLockAsync();
            var TotalBrandIdFilter = new IdFilter { In = TotalBrandIds };

            List<long> ActiveStoreIds, ActiveBrandIds;
            (ActiveStoreIds, ActiveBrandIds) = await GetActiveIdFromHistory(StoreIdFilter, TotalBrandIdFilter, OrderDateFilter);
            StoreIdFilter = new IdFilter { In = ActiveStoreIds };
            TotalBrandIdFilter = new IdFilter { In = ActiveBrandIds };

            var Brand = await BrandQuery.Where(x => x.Id, BrandIdFilter).FirstOrDefaultWithNoLockAsync();

            var BrandInStoreQuery = DataContext.BrandInStore.AsNoTracking();
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.StoreId, StoreIdFilter);
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.BrandId, BrandIdFilter);
            if (OrderDateFilter.LessEqual == null)
                BrandInStoreQuery = BrandInStoreQuery.Where(x => x.DeletedAt == null);
            else
                BrandInStoreQuery = BrandInStoreQuery.Where(x => (x.CreatedAt <= OrderDateFilter.LessEqual) && (x.DeletedAt == null || x.DeletedAt >= OrderDateFilter.LessEqual));

            var SurveyedStoreIds = await BrandInStoreQuery.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();
            var SurveyedBrandIds = await BrandInStoreQuery.Select(x => x.BrandId).Distinct().ToListWithNoLockAsync();

            var StoreQuery = DataContext.Store.AsNoTracking();
            StoreQuery = StoreQuery.Where(x => x.Id, new IdFilter { In = SurveyedStoreIds });
            var Stores = await StoreQuery.Select(x => new
            {
                x.Id,
                x.Code,
                x.CodeDraft,
                x.Name,
                x.Telephone,
                x.Address,
                x.OrganizationId
            }).ToListWithNoLockAsync();

            var BrandInStores = await BrandInStoreQuery.Select(x => new
            {
                x.Id,
                x.StoreId,
                x.BrandId
            }).Distinct().ToListWithNoLockAsync();
            var BrandInStoreIds = BrandInStores.Select(x => x.Id).ToList();
            var BrandInStoreIdFilter = new IdFilter { In = BrandInStoreIds };

            var ProductGroupingQuery = DataContext.ProductGrouping.AsNoTracking();
            if (OrderDateFilter.LessEqual == null)
                ProductGroupingQuery = ProductGroupingQuery.Where(x => x.DeletedAt == null);
            else
                ProductGroupingQuery = ProductGroupingQuery.Where(x => (x.CreatedAt <= OrderDateFilter.LessEqual) && (x.DeletedAt == null || x.DeletedAt >= OrderDateFilter.LessEqual));
            var ProductGroupingIds = await ProductGroupingQuery.Select(x => x.Id).Distinct().ToListWithNoLockAsync();
            var ProductGroupingIdFilter = new IdFilter { In = ProductGroupingIds };

            var ProductGroupingHistoryQuery = DataContext.ProductGroupingHistory.AsTracking();
            ProductGroupingHistoryQuery = ProductGroupingHistoryQuery.Where(x => x.CreatedAt, OrderDateFilter);
            ProductGroupingHistoryQuery = ProductGroupingHistoryQuery.Where(x => x.ProductGroupingId, ProductGroupingIdFilter);
            var ProductGroupingHistory = await ProductGroupingHistoryQuery.Select(x => new
            {
                x.ProductGroupingId,
                x.CreatedAt,
                x.StatusId,
            }).ToListWithNoLockAsync();
            var ActiveProductGroupings = ProductGroupingHistory.GroupBy(x => x.ProductGroupingId).Select(x => new
            { ProductGroupingId = x.Key, x.OrderByDescending(x => x.CreatedAt).First().StatusId }).ToList();
            List<long> ActiveProductGroupingIds = ActiveProductGroupings.Where(x => x.StatusId == StatusEnum.ACTIVE.Id).Select(x => x.ProductGroupingId).Distinct().ToList();
            ProductGroupingIdFilter.In = ActiveProductGroupingIds;

            var BrandInStoreProductGroupingMappingQuery = DataContext.BrandInStoreProductGroupingMapping.AsNoTracking();
            BrandInStoreProductGroupingMappingQuery = BrandInStoreProductGroupingMappingQuery.Where(x => x.BrandInStoreId, BrandInStoreIdFilter);
            BrandInStoreProductGroupingMappingQuery = BrandInStoreProductGroupingMappingQuery.Where(x => x.ProductGroupingId, ProductGroupingIdFilter);
            var BrandInStoreProductGroupingMapping = await BrandInStoreProductGroupingMappingQuery
               .Select(x => new
               {
                   x.BrandInStoreId,
                   x.ProductGroupingId,
                   x.BrandInStore.StoreId,
                   x.BrandInStore.BrandId
               }).OrderBy(x => x.StoreId).ToListWithNoLockAsync();

            var SurveyedProductGroupingIds = BrandInStoreProductGroupingMapping.Select(x => x.ProductGroupingId).Distinct().ToList();
            ProductGroupingIdFilter.In = SurveyedProductGroupingIds;
            ProductGroupingQuery = ProductGroupingQuery.Where(x => x.Id, ProductGroupingIdFilter);
            var ProductGroupings = await ProductGroupingQuery.Select(x => new { x.Id, x.Name }).ToListWithNoLockAsync();

            List<DashboardStoreInformation_StoreDTO> DashboardStoreInformation_StoreDTOs = Stores.Select(store => new DashboardStoreInformation_StoreDTO
            {
                Id = store.Id,
                Code = store.Code,
                CodeDraft = store.CodeDraft,
                Name = store.Name,
                Telephone = store.Telephone,
                Address = store.Address,
                OrganizationId = store.OrganizationId,
                ProductGroupings = ProductGroupings.ToDictionary(x => x.Id, x => ""),
            }).OrderBy(x => x.Id).ToList();

            int StoreIndex1 = 0;
            int StoreIndex2 = 0;
            while (StoreIndex1 < DashboardStoreInformation_StoreDTOs.Count && StoreIndex2 < BrandInStoreProductGroupingMapping.Count)
            {
                if (DashboardStoreInformation_StoreDTOs[StoreIndex1].Id > BrandInStoreProductGroupingMapping[StoreIndex2].StoreId)
                    StoreIndex2++;
                else if (DashboardStoreInformation_StoreDTOs[StoreIndex1].Id < BrandInStoreProductGroupingMapping[StoreIndex2].StoreId)
                    StoreIndex1++;
                else if (DashboardStoreInformation_StoreDTOs[StoreIndex1].Id == BrandInStoreProductGroupingMapping[StoreIndex2].StoreId)
                {
                    long ProductGroupingKey = BrandInStoreProductGroupingMapping[StoreIndex2].ProductGroupingId;
                    DashboardStoreInformation_StoreDTOs[StoreIndex1].ProductGroupings[ProductGroupingKey] = "x";
                    StoreIndex2++;
                }
            }
            var OrganizationQuery = DataContext.Organization.AsNoTracking();
            OrganizationIds = Stores.Select(x => x.OrganizationId).Distinct().ToList();
            OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            OrganizationQuery = DataContext.Organization.AsNoTracking();
            OrganizationQuery = OrganizationQuery.Where(x => x.Id, OrganizationIdFilter);
            OrganizationQuery = OrganizationQuery.Where(x => x.StatusId, StatusIdFilter);
            OrganizationQuery = OrganizationQuery.Where(x => x.DeletedAt == null);
            OrganizationQuery = OrganizationQuery.OrderBy(x => x.Id);
            var Organizations = await OrganizationQuery.ToListWithNoLockAsync();


            List<DashboardStoreInformation_StoreExportDTO> DashboardStoreInformation_StoreExportDTOs = new List<DashboardStoreInformation_StoreExportDTO>();
            foreach (var Organization in Organizations)
            {
                DashboardStoreInformation_StoreExportDTO DashboardStoreInformation_StoreExportDTO = new DashboardStoreInformation_StoreExportDTO();
                DashboardStoreInformation_StoreExportDTO.OrganizationId = Organization.Id;
                DashboardStoreInformation_StoreExportDTO.OrganizationName = Organization.Name;
                DashboardStoreInformation_StoreExportDTO.Stores = DashboardStoreInformation_StoreDTOs.Where(x => x.OrganizationId == Organization.Id).OrderBy(x => x.Code).ToList();
                DashboardStoreInformation_StoreExportDTOs.Add(DashboardStoreInformation_StoreExportDTO);
            }
            long stt = 1;
            foreach (var DashboardStoreInformation_StoreExportDTO in DashboardStoreInformation_StoreExportDTOs)
            {
                foreach (var Store in DashboardStoreInformation_StoreExportDTO.Stores)
                {
                    Store.STT = stt++;
                }
            }
            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            MemoryStream output = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(output))
            {
                #region Tiêu đề báo cáo
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add("Stores");
                ws.Cells.Style.Font.Name = "Times New Roman";
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                ws.Cells["A1"].Value = OrgRoot.Name.ToUpper();
                ws.Cells["A1:A3"].Style.Font.Bold = true;

                ws.Cells["A3"].Value = "BÁO CÁO HIỆN DIỆN THEO NHÓM SẢN PHẨM";
                ws.Cells["A3:F3"].Merge = true;
                ws.Cells["A3:F3"].Style.Font.Size = 14;
                ws.Cells["A3:F3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                ws.Cells["C4"].Value = "Hãng";
                ws.Cells["C4"].Style.Font.Bold = true;
                ws.Cells["C4"].Style.Font.Italic = true;
                ws.Cells["D4"].Value = Brand.Name;
                #endregion

                #region Header
                List<string> headers = new List<string>
                {
                    "STT",
                    "Mã đại lý tự sinh",
                    "Mã đại lý tự nhập",
                    "Tên đại lý",
                    "Số điện thoại",
                    "Địa chỉ"
                };
                for (int i = 0; i < ProductGroupings.Count; i++)
                {
                    headers.Add(ProductGroupings[i].Name);
                }

                int endColumnNumber = headers.Count;
                string endColumnString = Char.ConvertFromUtf32(endColumnNumber + 64);
                if (endColumnNumber > 26) endColumnString = Char.ConvertFromUtf32(endColumnNumber / 26 + 64) + Char.ConvertFromUtf32(endColumnNumber % 26 + 64);

                List<string[]> Header = new List<string[]> { headers.ToArray() };
                string headerRange = $"A6:{endColumnString}6";
                ws.Cells[headerRange].LoadFromArrays(Header);
                ws.Cells[headerRange].Style.Font.Bold = true;
                ws.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                ws.Cells[headerRange].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[headerRange].AutoFitColumns();
                #endregion

                #region Dữ liệu Stores
                int startRow = 7;
                int endRow = startRow;
                int startColumn = 1; // Gán lại cột bắt đầu từ A
                int endColumn = startColumn;
                if (DashboardStoreInformation_StoreExportDTOs != null)
                {
                    foreach (var StoreExportDTO in DashboardStoreInformation_StoreExportDTOs)
                    {
                        startColumn = 1; // Gán lại cột bắt đầu từ A
                        endColumn = startColumn;

                        ws.Cells[$"A{startRow}"].Value = $"{StoreExportDTO.OrganizationName}";
                        ws.Cells[$"A{startRow}"].Style.Font.Bold = true;
                        ws.Cells[$"A{startRow}:{endColumnString}{startRow}"].Merge = true;
                        ws.Cells[$"A{startRow}:{endColumnString}{startRow}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                        endRow = startRow; // Gán lại endrow = start row dể bắt đầu vùng dữ liệu mới
                        #region Cột STT
                        List<Object[]> STTData = new List<object[]>();
                        foreach (var store in StoreExportDTO.Stores)
                        {
                            STTData.Add(new object[]
                            {
                                store.STT,
                            });
                            endRow++;
                        }

                        ws.Cells[$"A{startRow + 1}:A{endRow}"].LoadFromArrays(STTData);
                        endColumn += 1; // chiếm 1 cột
                        startColumn = endColumn;
                        #endregion

                        #region Các cột thông tin đại lý
                        List<Object[]> StoreInfoData = new List<object[]>();
                        endRow = startRow;
                        foreach (var store in StoreExportDTO.Stores)
                        {
                            StoreInfoData.Add(new object[]
                            {
                                store.Code,
                                store.CodeDraft,
                                store.Name,
                                store.Telephone,
                                store.Address,
                            });
                            endRow++;
                        }
                        string startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                        if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                        string currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                        if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);

                        ws.Cells[$"{startColumnString}{startRow + 1}:{currentColumnString}{endRow}"].LoadFromArrays(StoreInfoData); // fill dữ liệu

                        endColumn += 5;
                        startColumn = endColumn; // gán lại cột bắt đầu cho dữ liệu tiếp sau
                        #endregion

                        #region Các cột product grouping
                        for (int i = 0; i < ProductGroupings.Count; i++)
                        {
                            List<Object[]> ProductGroupingData = new List<object[]>();
                            endRow = startRow;
                            foreach (var store in StoreExportDTO.Stores)
                            {
                                var value = store.ProductGroupings[ProductGroupings[i].Id];
                                ProductGroupingData.Add(new object[]
                                {
                                    value,
                                });
                                endRow++;
                            }
                            startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                            if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                            currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                            if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);

                            ws.Cells[$"{startColumnString}{startRow + 1}:{currentColumnString}{endRow}"].LoadFromArrays(ProductGroupingData); // fill dữ liệu

                            endColumn += 1; // Chiếm 1 cột cho mỗi productgrouping
                            startColumn = endColumn; // gán lại cột bắt đầu cho dữ liệu tiếp sau
                        }
                        #endregion

                        startRow = endRow + 1; // gán dòng bắt đầu cho org tiếp theo
                    }
                }

                #region Total
                endRow = startRow + 1;
                ws.Cells[$"A{startRow}"].Value = "Total";
                startColumn = 7;
                endColumn = startColumn;
                for (int i = 0; i < ProductGroupings.Count; i++)
                {
                    List<Object[]> TotalData = new List<object[]>();
                    endRow = startRow;
                    long totalStore = BrandInStoreProductGroupingMapping.Where(x => x.ProductGroupingId == ProductGroupings[i].Id)
                        .Select(x => x.StoreId).Distinct().Count();
                    TotalData.Add(new object[]
                    {
                        totalStore
                    });
                    var startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                    if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                    var currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                    if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);

                    ws.Cells[$"{startColumnString}{startRow}:{currentColumnString}{endRow}"].LoadFromArrays(TotalData); // fill dữ liệu

                    endColumn += 1; // Chiếm 1 cột cho mỗi productgrouping
                    startColumn = endColumn; // gán lại cột bắt đầu cho dữ liệu tiếp sau
                }
                ws.Cells[$"A{endRow}:F{endRow}"].Merge = true;
                ws.Cells[$"A{endRow}:F{endRow}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                ws.Cells[$"A{endRow}:{endColumnString}{endRow}"].Style.Font.Bold = true;
                ws.Cells[$"A{endRow}:{endColumnString}{endRow}"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FF0000"));
                #endregion

                ws.Column(4).Width = 25; // column ten cua hang
                ws.Column(6).Width = 40; // column dia chi
                ws.Cells[$"A1:A{endRow}"].Style.Numberformat.Format = "#"; // format number column STT

                // All borders
                ws.Cells[$"A6:{endColumnString}{endRow}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A6:{endColumnString}{endRow}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A6:{endColumnString}{endRow}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A6:{endColumnString}{endRow}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                #endregion

                excel.Save();
            }

            return File(output.ToArray(), "application/octet-stream", "StoreInformation_ProductGroupings.xlsx");
        }

        [Route(DashboardStoreInformationRoute.ExportEstimatedRevenueStatistic), HttpPost]
        public async Task<ActionResult> ExportEstimatedRevenueStatistic([FromBody] DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long? EstimatedRevenueId = DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO.EstimatedRevenueId?.Equal;
            long? ProvinceId = DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO.ProvinceId?.Equal;
            long? DistrictId = DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO.DistrictId?.Equal;
            long? AppUserId = DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO.AppUserId?.Equal;
            var AppUserIdFilter = new IdFilter();
            if (AppUserId.HasValue) AppUserIdFilter.Equal = AppUserId;
            var EstimatedRevenueIdFilter = new IdFilter();
            var ProvinceIdFilter = new IdFilter();
            var DistrictIdFilter = new IdFilter();
            var OrderDateFilter = new DateFilter { LessEqual = DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO.OrderDate?.LessEqual };
            if (EstimatedRevenueId.HasValue) EstimatedRevenueIdFilter.Equal = EstimatedRevenueId;
            if (ProvinceId.HasValue) ProvinceIdFilter.Equal = ProvinceId;
            if (DistrictId.HasValue) DistrictIdFilter.Equal = DistrictId;
            if (EstimatedRevenueId.HasValue == false)
                return BadRequest("Bạn chưa chọn ước doanh thu ngành đèn");
            if (DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO.OrganizationId == null)
                DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO.OrganizationId = new IdFilter();
            var OrganizationFilter = DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO.OrganizationId;
            List<long> OrganizationIds, StoreIds, AppuserIds;
            (OrganizationIds, StoreIds, AppuserIds) = await DynamicFilter(OrganizationFilter, ProvinceIdFilter, DistrictIdFilter, AppUserIdFilter);
            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            IdFilter StatusIdFilter = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            var StoreHistoryQuery = DataContext.StoreHistory.AsNoTracking();
            StoreHistoryQuery = StoreHistoryQuery.Where(x => x.CreatedAt, OrderDateFilter);
            StoreHistoryQuery = StoreHistoryQuery.Where(x => x.StoreId, StoreIdFilter);
            var StoreHistory = await StoreHistoryQuery.Select(x => new
            {
                x.StoreId,
                x.CreatedAt,
                x.StatusId,
                x.EstimatedRevenueId,
            }).ToListWithNoLockAsync();
            var ActiveStores = StoreHistory.GroupBy(x => x.StoreId).Select(x => new
            { StoreId = x.Key, x.OrderByDescending(x => x.CreatedAt).First().StatusId, x.OrderByDescending(x => x.CreatedAt).First().EstimatedRevenueId }).ToList();
            List<long> ActiveSurveyedStoreIds = ActiveStores.Where(x => x.StatusId == StatusEnum.ACTIVE.Id && x.EstimatedRevenueId == EstimatedRevenueId).Select(x => x.StoreId).Distinct().ToList();
            StoreIdFilter = new IdFilter { In = ActiveSurveyedStoreIds };
            var StoreQuery = DataContext.Store.AsNoTracking();
            StoreQuery = StoreQuery.Where(x => x.Id, StoreIdFilter);
            var DashboardStoreInformation_StoreDTOs = await StoreQuery.Select(store => new DashboardStoreInformation_StoreDTO
            {
                Id = store.Id,
                Code = store.Code,
                CodeDraft = store.CodeDraft,
                Name = store.Name,
                Telephone = store.Telephone,
                Address = store.Address,
                OrganizationId = store.OrganizationId,
            }).ToListWithNoLockAsync();
            var OrganizationQuery = DataContext.Organization.AsNoTracking();
            OrganizationIds = DashboardStoreInformation_StoreDTOs.Select(x => x.OrganizationId).Distinct().ToList();
            OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            OrganizationQuery = DataContext.Organization.AsNoTracking();
            OrganizationQuery = OrganizationQuery.Where(x => x.Id, OrganizationIdFilter);
            OrganizationQuery = OrganizationQuery.Where(x => x.StatusId, StatusIdFilter);
            OrganizationQuery = OrganizationQuery.Where(x => x.DeletedAt == null);
            OrganizationQuery = OrganizationQuery.OrderBy(x => x.Id);
            var Organizations = await OrganizationQuery.ToListWithNoLockAsync();

            List<DashboardStoreInformation_StoreExportDTO> DashboardStoreInformation_StoreExportDTOs = new List<DashboardStoreInformation_StoreExportDTO>();
            foreach (var Organization in Organizations)
            {
                DashboardStoreInformation_StoreExportDTO DashboardStoreInformation_StoreExportDTO = new DashboardStoreInformation_StoreExportDTO();
                DashboardStoreInformation_StoreExportDTO.OrganizationId = Organization.Id;
                DashboardStoreInformation_StoreExportDTO.OrganizationName = Organization.Name;
                DashboardStoreInformation_StoreExportDTO.Stores = DashboardStoreInformation_StoreDTOs.Where(x => x.OrganizationId == Organization.Id).OrderBy(x => x.Code).ToList();
                DashboardStoreInformation_StoreExportDTOs.Add(DashboardStoreInformation_StoreExportDTO);
            }
            long stt = 1;
            foreach (var DashboardStoreInformation_StoreExportDTO in DashboardStoreInformation_StoreExportDTOs)
            {
                foreach (var Store in DashboardStoreInformation_StoreExportDTO.Stores)
                {
                    Store.STT = stt++;
                }
            }
            var EstimatedRevenue = await DataContext.EstimatedRevenue.Where(x => x.Id == EstimatedRevenueIdFilter.Equal.Value).FirstOrDefaultWithNoLockAsync();
            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            var Total = DashboardStoreInformation_StoreExportDTOs.SelectMany(x => x.Stores).Count();

            string path = "Templates/Dashboard_StoreInformation_EstimatedRevenue.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.EstimatedRevenue = EstimatedRevenue;
            Data.Data = DashboardStoreInformation_StoreExportDTOs;
            Data.Total = Total;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "StoreInformation_EstimatedRevenue.xlsx");
        }

        private async Task<Tuple<List<long>, List<long>, List<long>>> DynamicFilter(IdFilter OrganizationIdFilter, IdFilter ProvinceIdFilter, IdFilter DistrictIdFilter, IdFilter AppUserIdFilter)
        {
            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            var organizationIdFilter = new IdFilter();
            if (OrganizationIds.Count > 0) organizationIdFilter = new IdFilter { In = OrganizationIds };
            var OrganizationQuery = DataContext.Organization.AsNoTracking();
            OrganizationQuery = OrganizationQuery.Where(x => x.DeletedAt == null);
            OrganizationQuery = OrganizationQuery.Where(x => x.Id, organizationIdFilter);
            OrganizationDAO OrganizationDAO = null;
            if (OrganizationIdFilter.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == OrganizationIdFilter.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationQuery = OrganizationQuery.Where(o => o.Path, new StringFilter { StartWith = OrganizationDAO.Path });
            }
            OrganizationIds = await OrganizationQuery.Select(o => o.Id).ToListWithNoLockAsync();
            OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var StoreQuery = DataContext.Store.AsNoTracking();
            List<long> AppUserIds = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            List<long> StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            List<long> StoreInScopeIds = new List<long>();
            AppUser AppUser = new AppUser();
            if (AppUserIdFilter.Equal != null)
            {
                AppUserIds = AppUserIds.Intersect(new List<long> { AppUserIdFilter.Equal.Value }).ToList();
                AppUser = await AppUserService.Get(AppUserIdFilter.Equal.Value);

                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == AppUser.OrganizationId).FirstOrDefaultWithNoLockAsync();
                OrganizationQuery = OrganizationQuery.Where(o => o.Path, new StringFilter { StartWith = OrganizationDAO.Path });
                OrganizationIds = await OrganizationQuery.Select(o => o.Id).ToListWithNoLockAsync();
                OrganizationIdFilter.In = OrganizationIdFilter.In.Intersect(OrganizationIds).ToList();

                StoreInScopeIds = AppUser.AppUserStoreMappings.Select(x => x.StoreId).ToList();
                if (StoreInScopeIds.Count > 0)
                {
                    var substore_query = DWContext.Dim_Store.AsNoTracking();
                    substore_query = substore_query.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusEnum.DRAFT.Id });
                    substore_query = substore_query.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
                    StoreInScopeIds.AddRange(await substore_query.Select(x => x.StoreId).ToListWithNoLockAsync());
                    StoreIds = StoreIds.Intersect(StoreInScopeIds).ToList();
                }
            }
            var StoreIdFilter = new IdFilter { In = StoreIds };
            StoreQuery = StoreQuery.Where(x => x.Id, StoreIdFilter);
            StoreQuery = StoreQuery.Where(x => x.DeletedAt == null);
            StoreQuery = StoreQuery.Where(x => x.StatusId, new IdFilter {  Equal = StatusEnum.ACTIVE.Id});
            StoreQuery = StoreQuery.Where(x => x.ProvinceId, ProvinceIdFilter);
            StoreQuery = StoreQuery.Where(x => x.DistrictId, DistrictIdFilter);
            StoreQuery = StoreQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            StoreIds = await StoreQuery.Select(x => x.Id).ToListWithNoLockAsync();
            return Tuple.Create(OrganizationIds, StoreIds, AppUserIds);
        }

        private async Task<Tuple<List<long>, List<long>>> GetActiveIdFromHistory(IdFilter StoreIdFilter, IdFilter BrandIdFilter, DateFilter OrderDateFilter)
        {
            var StoreHistoryQuery = DataContext.StoreHistory.AsNoTracking();
            StoreHistoryQuery = StoreHistoryQuery.Where(x => x.CreatedAt, OrderDateFilter);
            StoreHistoryQuery = StoreHistoryQuery.Where(x => x.StoreId, StoreIdFilter);
            var StoreHistory = await StoreHistoryQuery.Select(x => new
            {
                x.StoreId,
                x.CreatedAt,
                x.StatusId,
            }).ToListWithNoLockAsync();
            var ActiveStore = StoreHistory.GroupBy(x => x.StoreId).Select(x => new { StoreId = x.Key, x.OrderByDescending(x => x.CreatedAt).First().StatusId }).ToList();
            List<long> ActiveStoreIds = ActiveStore.Where(x => x.StatusId == StatusEnum.ACTIVE.Id).Select(x => x.StoreId).Distinct().ToList();

            var BrandHistoryQuery = DataContext.BrandHistory.AsNoTracking();
            BrandHistoryQuery = BrandHistoryQuery.Where(x => x.BrandId, BrandIdFilter);
            BrandHistoryQuery = BrandHistoryQuery.Where(x => x.CreatedAt, OrderDateFilter);
            var BrandHistory = await BrandHistoryQuery.Select(x => new
            {
                x.BrandId,
                x.CreatedAt,
                x.StatusId,
            }).ToListWithNoLockAsync();
            var ActiveBrand = BrandHistory.GroupBy(x => x.BrandId).Select(x => new { BrandId = x.Key, x.OrderByDescending(x => x.CreatedAt).First().StatusId }).ToList();
            List<long> ActiveBrandIds = ActiveBrand.Where(x => x.StatusId == StatusEnum.ACTIVE.Id).Select(x => x.BrandId).Distinct().ToList();

            return Tuple.Create(ActiveStoreIds, ActiveBrandIds);
        }
    }
}
