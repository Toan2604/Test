using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MOrganization;
using DMS.Services.MStore;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreStatus;
using DMS.Services.MStoreType;
using DMS.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using TrueSight.Common;
using DMS.Services.MProvince;
using DMS.Services.MDistrict;
using DMS.DWModels;

namespace DMS.Rpc.reports.report_store.report_store_general
{
    public class ReportStoreGeneralController : RpcController
    {
        private DataContext DataContext;
        private DWContext DWContext;
        private IAppUserService AppUserService;
        private IOrganizationService OrganizationService;
        private IStoreService StoreService;
        private IStoreGroupingService StoreGroupingService;
        private IStoreTypeService StoreTypeService;
        private IStoreStatusService StoreStatusService;
        private ICurrentContext CurrentContext;
        private IProvinceService ProvinceService;
        private IDistrictService DistrictService;
        public ReportStoreGeneralController(
            DataContext DataContext,
            DWContext DWContext,
            IOrganizationService OrganizationService,
            IAppUserService AppUserService,
            IStoreService StoreService,
            IStoreGroupingService StoreGroupingService,
            IStoreTypeService StoreTypeService,
            IStoreStatusService StoreStatusService,
            IProvinceService ProvinceService,
            IDistrictService DistrictService,
            ICurrentContext CurrentContext)
        {
            this.DataContext = DataContext;
            this.DWContext = DWContext;
            this.AppUserService = AppUserService;
            this.OrganizationService = OrganizationService;
            this.StoreService = StoreService;
            this.StoreGroupingService = StoreGroupingService;
            this.StoreTypeService = StoreTypeService;
            this.StoreStatusService = StoreStatusService;
            this.CurrentContext = CurrentContext;
            this.ProvinceService = ProvinceService;
            this.DistrictService = DistrictService;
        }

        #region Filter List
        [Route(ReportStoreGeneralRoute.FilterListOrganization), HttpPost]
        public async Task<List<ReportStoreGeneral_OrganizationDTO>> FilterListOrganization()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);
            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<ReportStoreGeneral_OrganizationDTO> StoreCheckerReport_OrganizationDTOs = Organizations
                .Select(x => new ReportStoreGeneral_OrganizationDTO(x)).ToList();
            return StoreCheckerReport_OrganizationDTOs;
        }

        [Route(ReportStoreGeneralRoute.FilterListStore), HttpPost]
        public async Task<List<ReportStoreGeneral_StoreDTO>> FilterListStore([FromBody] ReportStoreGeneral_StoreFilterDTO ReportStoreGeneral_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = ReportStoreGeneral_StoreFilterDTO.Id;
            StoreFilter.Code = ReportStoreGeneral_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ReportStoreGeneral_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ReportStoreGeneral_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = ReportStoreGeneral_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = ReportStoreGeneral_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = ReportStoreGeneral_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = ReportStoreGeneral_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            if (CurrentUser.AppUserStoreMappings != null && CurrentUser.AppUserStoreMappings.Count > 0)
            {
                StoreFilter.Id.In = CurrentUser.AppUserStoreMappings.Select(x => x.StoreId).ToList();
            }
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<ReportStoreGeneral_StoreDTO> ReportStoreGeneral_StoreDTOs = Stores
                .Select(x => new ReportStoreGeneral_StoreDTO(x)).ToList();
            return ReportStoreGeneral_StoreDTOs;
        }

        [Route(ReportStoreGeneralRoute.FilterListStoreGrouping), HttpPost]
        public async Task<List<ReportStoreGeneral_StoreGroupingDTO>> FilterListStoreGrouping([FromBody] ReportStoreGeneral_StoreGroupingFilterDTO ReportStoreGeneral_StoreGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = 99999;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Code = ReportStoreGeneral_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = ReportStoreGeneral_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.Level = ReportStoreGeneral_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.Path = ReportStoreGeneral_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreGroupingFilter.Id == null) StoreGroupingFilter.Id = new IdFilter();
            StoreGroupingFilter.Id.In = await FilterStoreGrouping(StoreGroupingService, CurrentContext);
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<ReportStoreGeneral_StoreGroupingDTO> ReportStoreGeneral_StoreGroupingDTOs = StoreGroupings
                .Select(x => new ReportStoreGeneral_StoreGroupingDTO(x)).ToList();
            return ReportStoreGeneral_StoreGroupingDTOs;
        }

        [Route(ReportStoreGeneralRoute.FilterListStoreType), HttpPost]
        public async Task<List<ReportStoreGeneral_StoreTypeDTO>> FilterListStoreType([FromBody] ReportStoreGeneral_StoreTypeFilterDTO ReportStoreGeneral_StoreTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = ReportStoreGeneral_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = ReportStoreGeneral_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = ReportStoreGeneral_StoreTypeFilterDTO.Name;
            StoreTypeFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreTypeFilter.Id == null) StoreTypeFilter.Id = new IdFilter();
            StoreTypeFilter.Id.In = await FilterStoreType(StoreTypeService, CurrentContext);
            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<ReportStoreGeneral_StoreTypeDTO> ReportStoreGeneral_StoreTypeDTOs = StoreTypes
                .Select(x => new ReportStoreGeneral_StoreTypeDTO(x)).ToList();
            return ReportStoreGeneral_StoreTypeDTOs;
        }

        [Route(ReportStoreGeneralRoute.FilterListStoreStatus), HttpPost]
        public async Task<List<ReportStoreGeneral_StoreStatusDTO>> FilterListStoreStatus([FromBody] ReportStoreGeneral_StoreStatusFilterDTO ReportStoreGeneral_StoreStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreStatusFilter StoreStatusFilter = new StoreStatusFilter();
            StoreStatusFilter.Skip = 0;
            StoreStatusFilter.Take = 20;
            StoreStatusFilter.OrderBy = StoreStatusOrder.Id;
            StoreStatusFilter.OrderType = OrderType.ASC;
            StoreStatusFilter.Selects = StoreStatusSelect.ALL;
            StoreStatusFilter.Id = ReportStoreGeneral_StoreStatusFilterDTO.Id;
            StoreStatusFilter.Code = ReportStoreGeneral_StoreStatusFilterDTO.Code;
            StoreStatusFilter.Name = ReportStoreGeneral_StoreStatusFilterDTO.Name;

            List<StoreStatus> StoreStatuses = await StoreStatusService.List(StoreStatusFilter);
            List<ReportStoreGeneral_StoreStatusDTO> ReportStoreGeneral_StoreStatusDTOs = StoreStatuses
                .Select(x => new ReportStoreGeneral_StoreStatusDTO(x)).ToList();
            return ReportStoreGeneral_StoreStatusDTOs;
        }
        [Route(ReportStoreGeneralRoute.FilterListAppUser), HttpPost]
        public async Task<List<ReportStoreGeneral_AppUserDTO>> FilterListAppUser([FromBody] ReportStoreGeneral_AppUserFilterDTO ReportStoreGeneral_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = int.MaxValue;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            AppUserFilter.Id = ReportStoreGeneral_AppUserFilterDTO.Id;
            AppUserFilter.Username = ReportStoreGeneral_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = ReportStoreGeneral_AppUserFilterDTO.DisplayName;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<ReportStoreGeneral_AppUserDTO> ReportStoreGeneral_AppUserDTOs = AppUsers
                .Select(x => new ReportStoreGeneral_AppUserDTO(x)).ToList();
            return ReportStoreGeneral_AppUserDTOs;
        }

        [Route(ReportStoreGeneralRoute.FilterListProvince), HttpPost]
        public async Task<List<ReportStoreGeneral_ProvinceDTO>> FilterListProvince([FromBody] ReportStoreGeneral_ProvinceFilterDTO ReportStoreGeneral_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = ReportStoreGeneral_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = ReportStoreGeneral_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = ReportStoreGeneral_ProvinceFilterDTO.StatusId;

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<ReportStoreGeneral_ProvinceDTO> ReportStoreGeneral_ProvinceDTOs = Provinces
                .Select(x => new ReportStoreGeneral_ProvinceDTO(x)).ToList();
            return ReportStoreGeneral_ProvinceDTOs;
        }

        [Route(ReportStoreGeneralRoute.FilterListDistrict), HttpPost]
        public async Task<List<ReportStoreGeneral_DistrictDTO>> FilterListDistrict([FromBody] ReportStoreGeneral_DistrictFilterDTO ReportStoreGeneral_DistrictFilterDTO)
        {
            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = ReportStoreGeneral_DistrictFilterDTO.Id;
            DistrictFilter.Name = ReportStoreGeneral_DistrictFilterDTO.Name;
            DistrictFilter.Priority = ReportStoreGeneral_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = ReportStoreGeneral_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = ReportStoreGeneral_DistrictFilterDTO.StatusId;

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<ReportStoreGeneral_DistrictDTO> ReportStoreGeneral_DistrictDTOs = Districts
                .Select(x => new ReportStoreGeneral_DistrictDTO(x)).ToList();
            return ReportStoreGeneral_DistrictDTOs;
        }


        #endregion

        [Route(ReportStoreGeneralRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] ReportStoreGeneral_ReportStoreGeneralFilterDTO ReportStoreGeneral_ReportStoreGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportStoreGeneral_ReportStoreGeneralFilterDTO.HasValue == false)
                return 0;

            DateTime Start = ReportStoreGeneral_ReportStoreGeneralFilterDTO.CheckIn?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportStoreGeneral_ReportStoreGeneralFilterDTO.CheckIn.GreaterEqual.Value;

            DateTime End = ReportStoreGeneral_ReportStoreGeneralFilterDTO.CheckIn?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportStoreGeneral_ReportStoreGeneralFilterDTO.CheckIn.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return 0;

            long? StoreId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.StoreId?.Equal;
            long? StoreTypeId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.StoreTypeId?.Equal;
            long? StoreGroupingId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.StoreGroupingId?.Equal;
            long? StoreStatusId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.StoreStatusId?.Equal;
            long? AppUserId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.AppUserId?.Equal;
            long? ProvinceId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.DistrictId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && OrganizationIds.Contains(o.Id)).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportStoreGeneral_ReportStoreGeneralFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportStoreGeneral_ReportStoreGeneralFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();
            List<long> StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            if (StoreId.HasValue)
            {
                var listId = new List<long> { StoreId.Value };
                StoreIds = StoreIds.Intersect(listId).ToList();
            }
            IdFilter AppUserIdFilter = new IdFilter();
            List<long> AppUserIds = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            if (AppUserId.HasValue)
            {
                var listIds = new List<long> { AppUserId.Value };
                AppUserIds = AppUserIds.Intersect(listIds).ToList();
                AppUserIdFilter.In = AppUserIds;
                List<long> storeIds = await StoreService.ListToIds(new StoreFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = StoreSelect.Id,
                    AppUserId = AppUserIdFilter
                });
                if (storeIds != null && storeIds.Count > 0)
                {
                    StoreIds = StoreIds.Intersect(storeIds).ToList();
                }
            }
            List<long> StoreTypeIds = await FilterStoreType(StoreTypeService, CurrentContext);
            if (StoreTypeId.HasValue)
            {
                var listId = new List<long> { StoreTypeId.Value };
                StoreTypeIds = StoreTypeIds.Intersect(listId).ToList();
            }
            List<long> StoreGroupingIds = await FilterStoreGrouping(StoreGroupingService, CurrentContext);

            var StoreStoreGroupingMappingQuery = DataContext.StoreStoreGroupingMapping.AsNoTracking();
            var MappedStoreIds = await StoreStoreGroupingMappingQuery.Select(x => x.StoreId).ToListWithNoLockAsync();

            var StoreQuery = DWContext.Dim_Store.AsNoTracking();
            StoreQuery = StoreQuery.Where(x => x.DeletedAt == null);
            StoreQuery = StoreQuery.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            StoreQuery = StoreQuery.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });
            StoreQuery = StoreQuery.Where(x => x.StoreTypeId, new IdFilter { In = StoreTypeIds });
            StoreQuery = StoreQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            StoreQuery = StoreQuery.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            StoreQuery = StoreQuery.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            StoreQuery = StoreQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            if (StoreStatusId.HasValue && StoreStatusId != StoreStatusEnum.ALL.Id)
                StoreQuery = StoreQuery.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusId });
            if (!StoreGroupingId.HasValue)
            {
                var sub1 = StoreQuery.Where(x => x.StoreId, new IdFilter { NotIn = MappedStoreIds });
                var storeIds = await StoreStoreGroupingMappingQuery.Where(x => x.StoreGroupingId, new IdFilter { In = StoreGroupingIds }).Select(x => x.StoreId).ToListWithNoLockAsync();
                var sub2 = StoreQuery.Where(x => x.StoreId, new IdFilter { In = storeIds });
                StoreQuery = sub1.Union(sub2);
            }
            else
            {
                var storeIds = await StoreStoreGroupingMappingQuery.Where(x => x.StoreGroupingId, new IdFilter { Equal = StoreGroupingId.Value }).Select(x => x.StoreId).ToListWithNoLockAsync();
                StoreQuery = StoreQuery.Where(x => x.StoreId, new IdFilter { In = storeIds });
            }

            int count = await StoreQuery.Distinct().CountWithNoLockAsync();
            return count;
        }

        [Route(ReportStoreGeneralRoute.List), HttpPost]
        public async Task<ActionResult<List<ReportStoreGeneral_ReportStoreGeneralDTO>>> List([FromBody] ReportStoreGeneral_ReportStoreGeneralFilterDTO ReportStoreGeneral_ReportStoreGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportStoreGeneral_ReportStoreGeneralFilterDTO.HasValue == false)
                return new List<ReportStoreGeneral_ReportStoreGeneralDTO>();

            DateTime Start = ReportStoreGeneral_ReportStoreGeneralFilterDTO.CheckIn?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportStoreGeneral_ReportStoreGeneralFilterDTO.CheckIn.GreaterEqual.Value;

            DateTime End = ReportStoreGeneral_ReportStoreGeneralFilterDTO.CheckIn?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportStoreGeneral_ReportStoreGeneralFilterDTO.CheckIn.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return BadRequest(new { message = "Chỉ được phép xem tối đa trong vòng 31 ngày" });
            List<ReportStoreGeneral_ReportStoreGeneralDTO> ReportStoreGeneral_ReportStoreGeneralDTOs = await ListDataDW(ReportStoreGeneral_ReportStoreGeneralFilterDTO, Start, End);
            return ReportStoreGeneral_ReportStoreGeneralDTOs;
        }

        [Route(ReportStoreGeneralRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] ReportStoreGeneral_ReportStoreGeneralFilterDTO ReportStoreGeneral_ReportStoreGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = ReportStoreGeneral_ReportStoreGeneralFilterDTO.CheckIn?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportStoreGeneral_ReportStoreGeneralFilterDTO.CheckIn.GreaterEqual.Value;

            DateTime End = ReportStoreGeneral_ReportStoreGeneralFilterDTO.CheckIn?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportStoreGeneral_ReportStoreGeneralFilterDTO.CheckIn.LessEqual.Value;

            ReportStoreGeneral_ReportStoreGeneralFilterDTO.Skip = 0;
            ReportStoreGeneral_ReportStoreGeneralFilterDTO.Take = int.MaxValue;
            List<ReportStoreGeneral_ReportStoreGeneralDTO> ReportStoreGeneral_ReportStoreGeneralDTOs = await ListDataDW(ReportStoreGeneral_ReportStoreGeneralFilterDTO, Start, End);
            ReportStoreGeneral_ReportStoreGeneralDTOs = ReportStoreGeneral_ReportStoreGeneralDTOs.OrderBy(x => x.OrganizationId).ToList();
            long stt = 1;
            foreach (ReportStoreGeneral_ReportStoreGeneralDTO ReportStoreGeneral_ReportStoreGeneralDTO in ReportStoreGeneral_ReportStoreGeneralDTOs)
            {
                foreach (var Store in ReportStoreGeneral_ReportStoreGeneralDTO.Stores)
                {
                    Store.STT = stt;
                    stt++;
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

            string path = "Templates/Report_Store_General.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.ReportStoreGenerals = ReportStoreGeneral_ReportStoreGeneralDTOs;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "ReportStoreGeneral.xlsx");
        }

        public async Task<List<ReportStoreGeneral_ReportStoreGeneralDTO>> ListData(
            ReportStoreGeneral_ReportStoreGeneralFilterDTO ReportStoreGeneral_ReportStoreGeneralFilterDTO,
            DateTime Start, DateTime End)
        {
            //var CurrentUser = await AppUserService.Get(CurrentContext.UserId);


            long? StoreId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.StoreId?.Equal;
            long? StoreTypeId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.StoreTypeId?.Equal;
            long? StoreStatusId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.StoreStatusId?.Equal;
            long? StoreGroupingId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.StoreGroupingId?.Equal;
            long? AppUserId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.AppUserId?.Equal;
            long? ProvinceId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.DistrictId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportStoreGeneral_ReportStoreGeneralFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportStoreGeneral_ReportStoreGeneralFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();

            AppUserFilter AppUserFilter = new AppUserFilter
            {
                OrganizationId = new IdFilter { In = OrganizationIds },
                Id = new IdFilter { },
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.DisplayName | AppUserSelect.Organization
            };
            List<long> AppUserIds = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            AppUser AppUser = new AppUser();
            if (AppUserId.HasValue)
            {
                var listIds = new List<long> { AppUserId.Value };
                AppUserIds = AppUserIds.Intersect(listIds).ToList();
                AppUser = await AppUserService.Get(AppUserId.Value);
            }
            AppUserFilter.Id.In = AppUserIds;
            var AppUsers = await AppUserService.List(AppUserFilter);
            AppUserIds = AppUsers.Select(x => x.Id).ToList();

            List<long> StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            if (StoreId.HasValue)
            {
                var listId = new List<long> { StoreId.Value };
                StoreIds = StoreIds.Intersect(listId).ToList();
            }
            if (AppUser.AppUserStoreMappings != null && AppUser.AppUserStoreMappings.Count > 0)
            {
                var StoreInScopeIds = AppUser.AppUserStoreMappings.Select(x => x.StoreId).ToList();
                StoreIds = StoreIds.Intersect(StoreInScopeIds).ToList();
            }
            List<long> StoreTypeIds = await FilterStoreType(StoreTypeService, CurrentContext);
            if (StoreTypeId.HasValue)
            {
                var listId = new List<long> { StoreTypeId.Value };
                StoreTypeIds = StoreTypeIds.Intersect(listId).ToList();
            }
            List<long> StoreGroupingIds = await FilterStoreGrouping(StoreGroupingService, CurrentContext);
            if (StoreGroupingId.HasValue)
            {
                var listId = new List<long> { StoreGroupingId.Value };
                StoreGroupingIds = StoreGroupingIds.Intersect(listId).ToList();
            }

            var store_query = DataContext.Store.AsNoTracking();
            store_query = store_query.Where(x => x.Id, new IdFilter { In = StoreIds });
            store_query = store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            store_query = store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            StoreIds = await store_query.Select(x => x.Id).ToListWithNoLockAsync();

            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
                    .BulkInsertValuesIntoTempTableAsync<long>(StoreIds);
            var query = from s in DataContext.Store
                        join tt in tempTableQuery.Query on s.Id equals tt.Column1
                        join sg in DataContext.StoreStoreGroupingMapping on s.Id equals sg.StoreId into subquery
                        from q in subquery.DefaultIfEmpty()
                        where StoreTypeIds.Contains(s.StoreTypeId) &&
                        (
                            (
                                StoreGroupingId.HasValue == false &&
                                (q == null || StoreGroupingIds.Contains(q.StoreGroupingId))
                            ) ||
                            (
                                StoreGroupingId.HasValue && StoreGroupingId.Value == q.StoreGroupingId
                            )
                        ) &&
                        (StoreStatusId.HasValue == false || StoreStatusId.Value == StoreStatusEnum.ALL.Id || s.StoreStatusId == StoreStatusId.Value) &&
                        OrganizationIds.Contains(s.OrganizationId) &&
                        s.DeletedAt == null
                        select new StoreDAO
                        {
                            Id = s.Id,
                            Code = s.Code,
                            CodeDraft = s.CodeDraft,
                            Name = s.Name,
                            StoreStatusId = s.StoreStatusId,
                            Address = s.Address,
                            Telephone = s.Telephone,
                            OrganizationId = s.OrganizationId,
                            OwnerPhone = s.OwnerPhone
                        };

            List<StoreDAO> StoreDAOs = await query
                .OrderBy(x => x.OrganizationId).ThenBy(x => x.Name)
                .Skip(ReportStoreGeneral_ReportStoreGeneralFilterDTO.Skip)
                .Take(ReportStoreGeneral_ReportStoreGeneralFilterDTO.Take)
                .ToListWithNoLockAsync();

            List<ReportStoreGeneral_StoreDTO> ReportStoreGeneral_StoreDTOs = new List<ReportStoreGeneral_StoreDTO>();
            foreach (var Store in StoreDAOs)
            {
                ReportStoreGeneral_StoreDTO ReportStoreGeneral_StoreDTO = new ReportStoreGeneral_StoreDTO();
                ReportStoreGeneral_StoreDTO.Id = Store.Id;
                ReportStoreGeneral_StoreDTO.Code = Store.Code;
                ReportStoreGeneral_StoreDTO.CodeDraft = Store.CodeDraft;
                ReportStoreGeneral_StoreDTO.Name = Store.Name;
                ReportStoreGeneral_StoreDTO.StoreStatusId = Store.StoreStatusId;
                ReportStoreGeneral_StoreDTO.Address = Store.Address;
                ReportStoreGeneral_StoreDTO.Phone = Store.OwnerPhone;
                ReportStoreGeneral_StoreDTO.OrganizationId = Store.OrganizationId;
                ReportStoreGeneral_StoreDTO.StoreStatus = StoreStatusEnum.StoreStatusEnumList.Where(x => x.Id == Store.StoreStatusId).Select(x => new ReportStoreGeneral_StoreStatusDTO
                {
                    Name = x.Name
                }).FirstOrDefault();
                ReportStoreGeneral_StoreDTOs.Add(ReportStoreGeneral_StoreDTO);
            }

            OrganizationIds = StoreDAOs.Select(x => x.OrganizationId).Distinct().ToList();
            OrganizationDAOs = await DataContext.Organization.Where(x => OrganizationIds.Contains(x.Id)).ToListWithNoLockAsync();

            StoreIds = ReportStoreGeneral_StoreDTOs.Select(x => x.Id).Distinct().ToList();
            tempTableQuery = await DataContext
                    .BulkInsertValuesIntoTempTableAsync<long>(StoreIds);
            List<ReportStoreGeneral_ReportStoreGeneralDTO> ReportStoreGeneral_ReportStoreGeneralDTOs = OrganizationDAOs.Select(on => new ReportStoreGeneral_ReportStoreGeneralDTO
            {
                OrganizationId = on.Id,
                OrganizationName = on.Name,
            }).ToList();
            foreach (ReportStoreGeneral_ReportStoreGeneralDTO ReportStoreGeneral_ReportStoreGeneralDTO in ReportStoreGeneral_ReportStoreGeneralDTOs)
            {
                ReportStoreGeneral_ReportStoreGeneralDTO.Stores = ReportStoreGeneral_StoreDTOs
                    .Where(x => x.OrganizationId == ReportStoreGeneral_ReportStoreGeneralDTO.OrganizationId)
                    .Select(x => new ReportStoreGeneral_StoreDetailDTO
                    {
                        Id = x.Id,
                        Code = x.Code,
                        CodeDraft = x.CodeDraft,
                        Name = x.Name,
                        StoreStatusName = x.StoreStatus.Name,
                        Address = x.Address,
                        Phone = x.Phone,
                    }).ToList();
            }

            ReportStoreGeneral_ReportStoreGeneralDTOs = ReportStoreGeneral_ReportStoreGeneralDTOs.Where(x => x.Stores.Any()).ToList();

            var storeCheckingQuery = from sc in DataContext.StoreChecking
                                     join au in DataContext.AppUser on sc.SaleEmployeeId equals au.Id
                                     join s in DataContext.Store on sc.StoreId equals s.Id
                                     join tt in tempTableQuery.Query on s.Id equals tt.Column1
                                     where sc.CheckOutAt.HasValue && Start <= sc.CheckOutAt.Value && sc.CheckOutAt.Value <= End &&
                                     AppUserIds.Contains(sc.SaleEmployeeId)
                                     select new StoreCheckingDAO
                                     {
                                         Id = sc.Id,
                                         CheckInAt = sc.CheckInAt,
                                         CheckOutAt = sc.CheckOutAt,
                                         Planned = sc.Planned,
                                         StoreId = sc.StoreId,
                                         SaleEmployeeId = sc.SaleEmployeeId,
                                         SaleEmployee = new AppUserDAO
                                         {
                                             Username = au.Username,
                                             DisplayName = au.DisplayName,
                                         }
                                     };
            List<StoreCheckingDAO> StoreCheckingDAOs = await storeCheckingQuery.ToListWithNoLockAsync();

            var indirectSalesOrderQuery = from i in DataContext.IndirectSalesOrder
                                          join au in DataContext.AppUser on i.SaleEmployeeId equals au.Id
                                          join s in DataContext.Store on i.BuyerStoreId equals s.Id
                                          join tt in tempTableQuery.Query on s.Id equals tt.Column1
                                          where Start <= i.OrderDate && i.OrderDate <= End &&
                                          i.RequestStateId == RequestStateEnum.APPROVED.Id &&
                                          AppUserIds.Contains(i.SaleEmployeeId) &&
                                          au.DeletedAt == null &&
                                          s.DeletedAt == null
                                          select new IndirectSalesOrderDAO
                                          {
                                              Id = i.Id,
                                              Code = i.Code,
                                              OrderDate = i.OrderDate,
                                              BuyerStoreId = i.BuyerStoreId,
                                              SaleEmployeeId = i.SaleEmployeeId,
                                              SellerStoreId = i.SellerStoreId,
                                              Total = i.Total
                                          };
            List<IndirectSalesOrderDAO> IndirectSalesOrderDAOs = await indirectSalesOrderQuery.ToListWithNoLockAsync();

            var IndirectSalesOrderIds = IndirectSalesOrderDAOs.Select(x => x.Id).ToList();
            tempTableQuery = await DataContext
                    .BulkInsertValuesIntoTempTableAsync<long>(IndirectSalesOrderIds);
            var indirectSalesOrderContentQuery = from ic in DataContext.IndirectSalesOrderContent
                                                 join i in DataContext.IndirectSalesOrder on ic.IndirectSalesOrderId equals i.Id
                                                 join tt in tempTableQuery.Query on i.Id equals tt.Column1
                                                 select new IndirectSalesOrderContentDAO
                                                 {
                                                     Id = ic.Id,
                                                     ItemId = ic.ItemId,
                                                     IndirectSalesOrderId = ic.IndirectSalesOrderId,
                                                     IndirectSalesOrder = new IndirectSalesOrderDAO
                                                     {
                                                         BuyerStoreId = i.BuyerStoreId
                                                     }
                                                 };
            List<IndirectSalesOrderContentDAO> IndirectSalesOrderContentDAOs = await indirectSalesOrderContentQuery.ToListWithNoLockAsync();

            var directSalesOrderQuery = from i in DataContext.DirectSalesOrder
                                        join au in DataContext.AppUser on i.SaleEmployeeId equals au.Id
                                        join s in DataContext.Store on i.BuyerStoreId equals s.Id
                                        join tt in tempTableQuery.Query on s.Id equals tt.Column1
                                        where Start <= i.OrderDate && i.OrderDate <= End &&
                                        (i.GeneralApprovalStateId == GeneralApprovalStateEnum.APPROVED.Id || i.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_APPROVED.Id) &&
                                        AppUserIds.Contains(i.SaleEmployeeId) &&
                                        au.DeletedAt == null &&
                                        s.DeletedAt == null
                                        select new DirectSalesOrderDAO
                                        {
                                            Id = i.Id,
                                            Code = i.Code,
                                            OrderDate = i.OrderDate,
                                            BuyerStoreId = i.BuyerStoreId,
                                            SaleEmployeeId = i.SaleEmployeeId,
                                            Total = i.Total
                                        };
            List<DirectSalesOrderDAO> DirectSalesOrderDAOs = await directSalesOrderQuery.ToListWithNoLockAsync();

            var DirectSalesOrderIds = DirectSalesOrderDAOs.Select(x => x.Id).ToList();
            tempTableQuery = await DataContext
                    .BulkInsertValuesIntoTempTableAsync<long>(DirectSalesOrderIds);
            var DirectSalesOrderContentQuery = from ic in DataContext.DirectSalesOrderContent
                                               join i in DataContext.DirectSalesOrder on ic.DirectSalesOrderId equals i.Id
                                               join tt in tempTableQuery.Query on i.Id equals tt.Column1
                                               select new DirectSalesOrderContentDAO
                                               {
                                                   Id = ic.Id,
                                                   ItemId = ic.ItemId,
                                                   DirectSalesOrderId = ic.DirectSalesOrderId,
                                                   DirectSalesOrder = new DirectSalesOrderDAO
                                                   {
                                                       BuyerStoreId = i.BuyerStoreId
                                                   }
                                               };
            List<DirectSalesOrderContentDAO> DirectSalesOrderContentDAOs = await DirectSalesOrderContentQuery.ToListWithNoLockAsync();

            // khởi tạo khung dữ liệu
            foreach (ReportStoreGeneral_ReportStoreGeneralDTO ReportStoreGeneral_ReportStoreGeneralDTO in ReportStoreGeneral_ReportStoreGeneralDTOs)
            {
                foreach (var StoreCheckingDAO in StoreCheckingDAOs)
                {
                    var Store = ReportStoreGeneral_ReportStoreGeneralDTO.Stores.Where(x => x.Id == StoreCheckingDAO.StoreId).FirstOrDefault();
                    if (Store == null)
                        continue;
                    if (StoreCheckingDAO.Planned)
                    {
                        if (Store.StoreCheckingPlannedIds == null)
                            Store.StoreCheckingPlannedIds = new HashSet<long>();
                        Store.StoreCheckingPlannedIds.Add(StoreCheckingDAO.Id);
                    }
                    else
                    {
                        if (Store.StoreCheckingUnPlannedIds == null)
                            Store.StoreCheckingUnPlannedIds = new HashSet<long>();
                        Store.StoreCheckingUnPlannedIds.Add(StoreCheckingDAO.Id);
                    }
                }

                foreach (var Store in ReportStoreGeneral_ReportStoreGeneralDTO.Stores)
                {
                    var StoreCheckings = StoreCheckingDAOs.Where(x => x.StoreId == Store.Id).ToList();

                    //lượt viếng thăm đầu tiên
                    Store.FirstChecking = StoreCheckings.OrderBy(x => x.CheckOutAt)
                        .Select(x => x.CheckOutAt.Value.Date)
                        .FirstOrDefault();
                    //lượt viếng thăm gần nhất
                    Store.LastChecking = StoreCheckings.OrderByDescending(x => x.CheckOutAt)
                        .Select(x => x.CheckOutAt.Value.Date)
                        .FirstOrDefault();
                    Store.EmployeeLastChecking = StoreCheckings.OrderByDescending(x => x.CheckOutAt)
                        .Select(x => x.SaleEmployee.DisplayName)
                        .FirstOrDefault();
                    //tổng thời gian viếng thăm
                    var TotalMinuteChecking = StoreCheckings.Sum(x => (x.CheckOutAt.Value.Subtract(x.CheckInAt.Value)).TotalSeconds);
                    TimeSpan timeSpan = TimeSpan.FromSeconds(TotalMinuteChecking);
                    Store.TotalCheckingTime = $"{timeSpan.Hours.ToString().PadLeft(2, '0')} : {timeSpan.Minutes.ToString().PadLeft(2, '0')} : {timeSpan.Seconds.ToString().PadLeft(2, '0')}";

                    //tổng doanh thu
                    var IndirectRevenue = IndirectSalesOrderDAOs.Where(x => x.BuyerStoreId == Store.Id).Sum(x => x.Total);
                    var DirectRevenue = DirectSalesOrderDAOs.Where(x => x.BuyerStoreId == Store.Id).Sum(x => x.Total);
                    Store.TotalRevenue = IndirectRevenue + DirectRevenue;
                    //ngày đặt hàng gần nhất
                    var IndireatLastOrder = IndirectSalesOrderDAOs.Where(x => x.BuyerStoreId == Store.Id)
                        .OrderByDescending(x => x.OrderDate)
                        .Select(x => x.OrderDate.AddHours(CurrentContext.TimeZone).Date)
                        .FirstOrDefault();
                    var DireatLastOrder = DirectSalesOrderDAOs.Where(x => x.BuyerStoreId == Store.Id)
                        .OrderByDescending(x => x.OrderDate)
                        .Select(x => x.OrderDate.AddHours(CurrentContext.TimeZone).Date)
                        .FirstOrDefault();
                    Store.LastOrder = IndireatLastOrder > DireatLastOrder ? IndireatLastOrder : DireatLastOrder;
                }

                foreach (var Store in ReportStoreGeneral_ReportStoreGeneralDTO.Stores)
                {
                    var indirectSalesOrderIds = IndirectSalesOrderDAOs.Where(x => x.BuyerStoreId == Store.Id).Select(x => x.Id).ToList();
                    foreach (var Id in indirectSalesOrderIds)
                    {
                        if (Store.SalesOrderIds == null)
                            Store.SalesOrderIds = new HashSet<long>();
                        Store.SalesOrderIds.Add(Id);

                        var itemIds = IndirectSalesOrderContentDAOs.Where(x => x.IndirectSalesOrderId == Id).Select(x => x.ItemId).Distinct().ToList();
                        if (Store.SKUItemIds == null)
                            Store.SKUItemIds = new List<long>();
                        Store.SKUItemIds.AddRange(itemIds);
                    }

                    var directSalesOrderIds = DirectSalesOrderDAOs.Where(x => x.BuyerStoreId == Store.Id).Select(x => x.Id).ToList();
                    foreach (var Id in directSalesOrderIds)
                    {
                        if (Store.SalesOrderIds == null)
                            Store.SalesOrderIds = new HashSet<long>();
                        Store.SalesOrderIds.Add(Id);

                        var itemIds = DirectSalesOrderContentDAOs.Where(x => x.DirectSalesOrderId == Id).Select(x => x.ItemId).Distinct().ToList();
                        if (Store.SKUItemIds == null)
                            Store.SKUItemIds = new List<long>();
                        Store.SKUItemIds.AddRange(itemIds);
                    }
                }
            }
            return ReportStoreGeneral_ReportStoreGeneralDTOs;
        }

        public async Task<List<ReportStoreGeneral_ReportStoreGeneralDTO>> ListDataDW(
           ReportStoreGeneral_ReportStoreGeneralFilterDTO ReportStoreGeneral_ReportStoreGeneralFilterDTO,
           DateTime Start, DateTime End)
        {
            //var CurrentUser = await AppUserService.Get(CurrentContext.UserId);


            long? StoreId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.StoreId?.Equal;
            long? StoreTypeId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.StoreTypeId?.Equal;
            long? StoreStatusId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.StoreStatusId?.Equal;
            long? StoreGroupingId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.StoreGroupingId?.Equal;
            long? AppUserId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.AppUserId?.Equal;
            long? ProvinceId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportStoreGeneral_ReportStoreGeneralFilterDTO.DistrictId?.Equal;
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            var OrganizationDAOs = await DWContext.Dim_Organization
                .Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.OrganizationId))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportStoreGeneral_ReportStoreGeneralFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportStoreGeneral_ReportStoreGeneralFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.OrganizationId).ToList();

            AppUserFilter AppUserFilter = new AppUserFilter
            {
                OrganizationId = new IdFilter { In = OrganizationIds },
                Id = new IdFilter { },
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.DisplayName | AppUserSelect.Organization
            };
            List<long> AppUserIds = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            AppUser AppUser = new AppUser();

            AppUserFilter.Id.In = AppUserIds;
            var AppUsers = await AppUserService.List(AppUserFilter);
            AppUserIds = AppUsers.Select(x => x.Id).ToList();

            List<long> StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            if (StoreId.HasValue)
            {
                var listId = new List<long> { StoreId.Value };
                StoreIds = StoreIds.Intersect(listId).ToList();
            }
            if (AppUserId.HasValue)
            {
                var listIds = new List<long> { AppUserId.Value };
                AppUserIds = AppUserIds.Intersect(listIds).ToList();
                AppUser = await AppUserService.Get(AppUserId.Value);
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == AppUser.OrganizationId).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
                OrganizationIds = OrganizationIds.Intersect(OrganizationDAOs.Select(o => o.OrganizationId).ToList()).ToList();
                if (AppUser.AppUserStoreMappings != null && AppUser.AppUserStoreMappings.Count > 0)
                { // nếu có pvdt thì lấy theo pvdt và dự thảo
                    var StoreInScopeIds = AppUser.AppUserStoreMappings.Select(x => x.StoreId).ToList();
                    var substore_query = DWContext.Dim_Store.AsNoTracking();
                    substore_query = substore_query.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusEnum.DRAFT.Id });
                    substore_query = substore_query.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
                    StoreInScopeIds.AddRange(await substore_query.Select(x => x.StoreId).ToListWithNoLockAsync());
                    StoreIds = StoreIds.Intersect(StoreInScopeIds).ToList();
                }
            }
            List<long> StoreTypeIds = await FilterStoreType(StoreTypeService, CurrentContext);
            if (StoreTypeId.HasValue)
            {
                var listId = new List<long> { StoreTypeId.Value };
                StoreTypeIds = StoreTypeIds.Intersect(listId).ToList();
            }
            List<long> StoreGroupingIds = await FilterStoreGrouping(StoreGroupingService, CurrentContext);

            var StoreStoreGroupingMappingQuery = DataContext.StoreStoreGroupingMapping.AsNoTracking();
            var MappedStoreIds = await StoreStoreGroupingMappingQuery.Select(x => x.StoreId).ToListWithNoLockAsync();

            var StoreQuery = DWContext.Dim_Store.AsNoTracking();
            StoreQuery = StoreQuery.Where(x => x.DeletedAt == null);
            StoreQuery = StoreQuery.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            StoreQuery = StoreQuery.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });
            StoreQuery = StoreQuery.Where(x => x.StoreTypeId, new IdFilter { In = StoreTypeIds });
            StoreQuery = StoreQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            StoreQuery = StoreQuery.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            StoreQuery = StoreQuery.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            StoreQuery = StoreQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            if (StoreStatusId.HasValue && StoreStatusId != StoreStatusEnum.ALL.Id)
                StoreQuery = StoreQuery.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusId });
            if (!StoreGroupingId.HasValue)
            {
                var sub1 = StoreQuery.Where(x => x.StoreId, new IdFilter { NotIn = MappedStoreIds });
                var storeIds = await StoreStoreGroupingMappingQuery.Where(x => x.StoreGroupingId, new IdFilter { In = StoreGroupingIds }).Select(x => x.StoreId).ToListWithNoLockAsync();
                var sub2 = StoreQuery.Where(x => x.StoreId, new IdFilter { In = storeIds });
                StoreQuery = sub1.Union(sub2);
            }
            else
            {
                var storeIds = await StoreStoreGroupingMappingQuery.Where(x => x.StoreGroupingId, new IdFilter { Equal = StoreGroupingId.Value }).Select(x => x.StoreId).ToListWithNoLockAsync();
                StoreQuery = StoreQuery.Where(x => x.StoreId, new IdFilter { In = storeIds });
            }

            List<StoreDAO> StoreDAOs = await StoreQuery.Select(s => new StoreDAO
            {
                Id = s.StoreId,
                Code = s.Code,
                CodeDraft = s.CodeDraft,
                Name = s.Name,
                StoreStatusId = s.StoreStatusId,
                Address = s.Address,
                Telephone = s.Telephone,
                OrganizationId = s.OrganizationId,
                OwnerPhone = s.OwnerPhone
            })
                .OrderBy(x => x.OrganizationId).ThenBy(x => x.Name)
                .Skip(ReportStoreGeneral_ReportStoreGeneralFilterDTO.Skip)
                .Take(ReportStoreGeneral_ReportStoreGeneralFilterDTO.Take)
                .ToListWithNoLockAsync();

            StoreIds = StoreDAOs.Select(x => x.Id).Distinct().ToList();

            List<ReportStoreGeneral_StoreDTO> ReportStoreGeneral_StoreDTOs = new List<ReportStoreGeneral_StoreDTO>();
            foreach (var Store in StoreDAOs)
            {
                ReportStoreGeneral_StoreDTO ReportStoreGeneral_StoreDTO = new ReportStoreGeneral_StoreDTO();
                ReportStoreGeneral_StoreDTO.Id = Store.Id;
                ReportStoreGeneral_StoreDTO.Code = Store.Code;
                ReportStoreGeneral_StoreDTO.CodeDraft = Store.CodeDraft;
                ReportStoreGeneral_StoreDTO.Name = Store.Name;
                ReportStoreGeneral_StoreDTO.StoreStatusId = Store.StoreStatusId;
                ReportStoreGeneral_StoreDTO.Address = Store.Address;
                ReportStoreGeneral_StoreDTO.Phone = Store.OwnerPhone;
                ReportStoreGeneral_StoreDTO.OrganizationId = Store.OrganizationId;
                ReportStoreGeneral_StoreDTO.StoreStatus = StoreStatusEnum.StoreStatusEnumList.Where(x => x.Id == Store.StoreStatusId).Select(x => new ReportStoreGeneral_StoreStatusDTO
                {
                    Name = x.Name
                }).FirstOrDefault();
                ReportStoreGeneral_StoreDTOs.Add(ReportStoreGeneral_StoreDTO);
            }

            OrganizationIds = StoreDAOs.Select(x => x.OrganizationId).Distinct().ToList();
            OrganizationDAOs = await DWContext.Dim_Organization.Where(x => OrganizationIds.Contains(x.OrganizationId)).ToListWithNoLockAsync();

            List<ReportStoreGeneral_ReportStoreGeneralDTO> ReportStoreGeneral_ReportStoreGeneralDTOs = OrganizationDAOs.Select(on => new ReportStoreGeneral_ReportStoreGeneralDTO
            {
                OrganizationId = on.OrganizationId,
                OrganizationName = on.Name,
            }).ToList();
            foreach (ReportStoreGeneral_ReportStoreGeneralDTO ReportStoreGeneral_ReportStoreGeneralDTO in ReportStoreGeneral_ReportStoreGeneralDTOs)
            {
                ReportStoreGeneral_ReportStoreGeneralDTO.Stores = ReportStoreGeneral_StoreDTOs
                    .Where(x => x.OrganizationId == ReportStoreGeneral_ReportStoreGeneralDTO.OrganizationId)
                    .Select(x => new ReportStoreGeneral_StoreDetailDTO
                    {
                        Id = x.Id,
                        Code = x.Code,
                        CodeDraft = x.CodeDraft,
                        Name = x.Name,
                        StoreStatusName = x.StoreStatus.Name,
                        Address = x.Address,
                        Phone = x.Phone,
                    }).ToList();
            }
            ReportStoreGeneral_ReportStoreGeneralDTOs = ReportStoreGeneral_ReportStoreGeneralDTOs.Where(x => x.Stores.Any()).ToList();

            var AllAppUsers = await DWContext.Dim_AppUser.Select(x => new AppUser
            {
                Id = x.AppUserId,
                DisplayName = x.DisplayName,
            }).ToArrayAsync();

            var storechecking_query = DWContext.Fact_StoreChecking.AsNoTracking();
            storechecking_query = storechecking_query.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            storechecking_query = storechecking_query.Where(x => x.CheckOutAt, DateFilter);
            storechecking_query = storechecking_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });

            List<StoreCheckingDAO> StoreCheckingDAOs = await storechecking_query.Select(sc => new StoreCheckingDAO
            {
                Id = sc.StoreCheckingId,
                CheckInAt = sc.CheckInAt,
                CheckOutAt = sc.CheckOutAt,
                Planned = sc.Planned,
                StoreId = sc.StoreId,
                SaleEmployeeId = sc.SaleEmployeeId,
            }).ToListWithNoLockAsync();

            var indirectsalesorder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.OrderDate, DateFilter);
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });

            List<IndirectSalesOrderDAO> IndirectSalesOrderDAOs = await indirectsalesorder_query.Select(i => new IndirectSalesOrderDAO
            {
                Id = i.IndirectSalesOrderId,
                Code = i.Code,
                OrderDate = i.OrderDate,
                BuyerStoreId = i.BuyerStoreId,
                SaleEmployeeId = i.SaleEmployeeId,
                SellerStoreId = i.SellerStoreId,
                Total = i.Total
            }).ToListWithNoLockAsync();

            var IndirectSalesOrderIds = IndirectSalesOrderDAOs.Select(x => x.Id).ToList();

            var indirectsalesordertransaction_query = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            indirectsalesordertransaction_query = indirectsalesordertransaction_query.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
            indirectsalesordertransaction_query = indirectsalesordertransaction_query.Where(x => x.DeletedAt == null);

            List<IndirectSalesOrderContentDAO> IndirectSalesOrderContentDAOs = await indirectsalesordertransaction_query.Select(ic => new IndirectSalesOrderContentDAO
            {
                Id = ic.IndirectSalesOrderTransactionId,
                ItemId = ic.ItemId,
                IndirectSalesOrderId = ic.IndirectSalesOrderId,

            }).ToListWithNoLockAsync();

            var directsalesorder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            directsalesorder_query = directsalesorder_query.Where(x => x.OrderDate, DateFilter);
            directsalesorder_query = directsalesorder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long>{
            GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id}
            });
            directsalesorder_query = directsalesorder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });

            List<DirectSalesOrderDAO> DirectSalesOrderDAOs = await directsalesorder_query.Select(i => new DirectSalesOrderDAO
            {
                Id = i.DirectSalesOrderId,
                Code = i.Code,
                OrderDate = i.OrderDate,
                BuyerStoreId = i.BuyerStoreId,
                SaleEmployeeId = i.SaleEmployeeId,
                Total = i.Total
            }).ToListWithNoLockAsync();

            var DirectSalesOrderIds = DirectSalesOrderDAOs.Select(x => x.Id).ToList();

            var directsalesordertransaction_query = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
            directsalesordertransaction_query = directsalesordertransaction_query.Where(x => x.DeletedAt == null);
            directsalesordertransaction_query = directsalesordertransaction_query.Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds });

            List<DirectSalesOrderContentDAO> DirectSalesOrderContentDAOs = await directsalesordertransaction_query.Select(ic => new DirectSalesOrderContentDAO
            {
                Id = ic.DirectSalesOrderTransactionId,
                ItemId = ic.ItemId,
                DirectSalesOrderId = ic.DirectSalesOrderId,

            }).ToListWithNoLockAsync();

            // khởi tạo khung dữ liệu
            foreach (ReportStoreGeneral_ReportStoreGeneralDTO ReportStoreGeneral_ReportStoreGeneralDTO in ReportStoreGeneral_ReportStoreGeneralDTOs)
            {
                foreach (var StoreCheckingDAO in StoreCheckingDAOs)
                {
                    var Store = ReportStoreGeneral_ReportStoreGeneralDTO.Stores.Where(x => x.Id == StoreCheckingDAO.StoreId).FirstOrDefault();
                    if (Store == null)
                        continue;
                    if (StoreCheckingDAO.Planned)
                    {
                        if (Store.StoreCheckingPlannedIds == null)
                            Store.StoreCheckingPlannedIds = new HashSet<long>();
                        Store.StoreCheckingPlannedIds.Add(StoreCheckingDAO.Id);
                    }
                    else
                    {
                        if (Store.StoreCheckingUnPlannedIds == null)
                            Store.StoreCheckingUnPlannedIds = new HashSet<long>();
                        Store.StoreCheckingUnPlannedIds.Add(StoreCheckingDAO.Id);
                    }
                }

                foreach (var Store in ReportStoreGeneral_ReportStoreGeneralDTO.Stores)
                {
                    var StoreCheckings = StoreCheckingDAOs.Where(x => x.StoreId == Store.Id).ToList();

                    //lượt viếng thăm đầu tiên
                    Store.FirstChecking = StoreCheckings.OrderBy(x => x.CheckOutAt)
                        .Select(x => x.CheckOutAt.Value.Date)
                        .FirstOrDefault();
                    //lượt viếng thăm gần nhất
                    Store.LastChecking = StoreCheckings.OrderByDescending(x => x.CheckOutAt)
                        .Select(x => x.CheckOutAt.Value.Date)
                        .FirstOrDefault();
                    long EmployeeLastCheckingId = StoreCheckings.OrderByDescending(x => x.CheckOutAt)
                        .Select(x => x.SaleEmployeeId)
                        .FirstOrDefault();
                    Store.EmployeeLastChecking = AllAppUsers.Where(x => x.Id == EmployeeLastCheckingId)
                        .Select(x => x.DisplayName)
                        .FirstOrDefault();
                    //tổng thời gian viếng thăm
                    var TotalMinuteChecking = StoreCheckings.Sum(x => (x.CheckOutAt.Value.Subtract(x.CheckInAt.Value)).TotalSeconds);
                    TimeSpan timeSpan = TimeSpan.FromSeconds(TotalMinuteChecking);
                    Store.TotalCheckingTime = $"{timeSpan.Hours.ToString().PadLeft(2, '0')} : {timeSpan.Minutes.ToString().PadLeft(2, '0')} : {timeSpan.Seconds.ToString().PadLeft(2, '0')}";

                    //tổng doanh thu
                    var IndirectRevenue = IndirectSalesOrderDAOs.Where(x => x.BuyerStoreId == Store.Id).Sum(x => x.Total);
                    var DirectRevenue = DirectSalesOrderDAOs.Where(x => x.BuyerStoreId == Store.Id).Sum(x => x.Total);
                    Store.TotalRevenue = IndirectRevenue + DirectRevenue;
                    //ngày đặt hàng gần nhất
                    var IndireatLastOrder = IndirectSalesOrderDAOs.Where(x => x.BuyerStoreId == Store.Id)
                        .OrderByDescending(x => x.OrderDate)
                        .Select(x => x.OrderDate.AddHours(CurrentContext.TimeZone).Date)
                        .FirstOrDefault();
                    var DireatLastOrder = DirectSalesOrderDAOs.Where(x => x.BuyerStoreId == Store.Id)
                        .OrderByDescending(x => x.OrderDate)
                        .Select(x => x.OrderDate.AddHours(CurrentContext.TimeZone).Date)
                        .FirstOrDefault();
                    Store.LastOrder = IndireatLastOrder > DireatLastOrder ? IndireatLastOrder : DireatLastOrder;
                }

                foreach (var Store in ReportStoreGeneral_ReportStoreGeneralDTO.Stores)
                {
                    var indirectSalesOrderIds = IndirectSalesOrderDAOs.Where(x => x.BuyerStoreId == Store.Id).Select(x => x.Id).ToList();
                    foreach (var Id in indirectSalesOrderIds)
                    {
                        if (Store.SalesOrderIds == null)
                            Store.SalesOrderIds = new HashSet<long>();
                        Store.SalesOrderIds.Add(Id);

                        var itemIds = IndirectSalesOrderContentDAOs.Where(x => x.IndirectSalesOrderId == Id).Select(x => x.ItemId).Distinct().ToList();
                        if (Store.SKUItemIds == null)
                            Store.SKUItemIds = new List<long>();
                        Store.SKUItemIds.AddRange(itemIds);
                    }

                    var directSalesOrderIds = DirectSalesOrderDAOs.Where(x => x.BuyerStoreId == Store.Id).Select(x => x.Id).ToList();
                    foreach (var Id in directSalesOrderIds)
                    {
                        if (Store.SalesOrderIds == null)
                            Store.SalesOrderIds = new HashSet<long>();
                        Store.SalesOrderIds.Add(Id);

                        var itemIds = DirectSalesOrderContentDAOs.Where(x => x.DirectSalesOrderId == Id).Select(x => x.ItemId).Distinct().ToList();
                        if (Store.SKUItemIds == null)
                            Store.SKUItemIds = new List<long>();
                        Store.SKUItemIds.AddRange(itemIds);
                    }
                }
            }
            return ReportStoreGeneral_ReportStoreGeneralDTOs;
        }

    }
}
