using DMS.Common;
using DMS.Models;
using DMS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Entities;
using DMS.Enums;
using DMS.Services.MOrganization;
using DMS.Services.MAppUser;
using DMS.Services.MStore;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreStatus;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Dynamic;
using DMS.Helpers;
using DMS.Services.MStoreStatusHistoryTypeHistoryType;
using TrueSight.Common;
using DMS.Services.MProvince;
using DMS.Services.MDistrict;

namespace DMS.Rpc.reports.report_store.report_store_state_change
{
    public class ReportStoreStateChangeController : RpcController
    {
        private IAppUserService AppUserService;
        private IOrganizationService OrganizationService;
        private IStoreService StoreService;
        private IStoreStatusHistoryTypeService StoreStatusHistoryTypeService;
        private DataContext DataContext;
        private ICurrentContext CurrentContext;
        private IProvinceService ProvinceService;
        private IDistrictService DistrictService;
        public ReportStoreStateChangeController(
            IAppUserService AppUserService,
            IOrganizationService OrganizationService,
            IStoreService StoreService,
            IStoreStatusHistoryTypeService StoreStatusHistoryTypeService,
            IProvinceService ProvinceService,
            IDistrictService DistrictService,
            DataContext DataContext,
            ICurrentContext CurrentContext)
        {
            this.AppUserService = AppUserService;
            this.OrganizationService = OrganizationService;
            this.StoreService = StoreService;
            this.StoreStatusHistoryTypeService = StoreStatusHistoryTypeService;
            this.DataContext = DataContext;
            this.CurrentContext = CurrentContext;
            this.ProvinceService = ProvinceService;
            this.DistrictService = DistrictService;
        }

        #region Filter List
        [Route(ReportStoreStateChangeRoute.FilterListOrganization), HttpPost]
        public async Task<List<ReportStoreStateChange_OrganizationDTO>> FilterListOrganization()
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
            List<ReportStoreStateChange_OrganizationDTO> StoreCheckerReport_OrganizationDTOs = Organizations
                .Select(x => new ReportStoreStateChange_OrganizationDTO(x)).ToList();
            return StoreCheckerReport_OrganizationDTOs;
        }

        [Route(ReportStoreStateChangeRoute.FilterListStore), HttpPost]
        public async Task<List<ReportStoreStateChange_StoreDTO>> FilterListStore([FromBody] ReportStoreStateChange_StoreFilterDTO ReportStoreStateChange_StoreFilterDTO)
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
            StoreFilter.Id = ReportStoreStateChange_StoreFilterDTO.Id;
            StoreFilter.Code = ReportStoreStateChange_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ReportStoreStateChange_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ReportStoreStateChange_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = ReportStoreStateChange_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = ReportStoreStateChange_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = ReportStoreStateChange_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = ReportStoreStateChange_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            if (CurrentUser.AppUserStoreMappings != null && CurrentUser.AppUserStoreMappings.Count > 0)
            {
                StoreFilter.Id.In = CurrentUser.AppUserStoreMappings.Select(x => x.StoreId).ToList();
            }
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<ReportStoreStateChange_StoreDTO> ReportStoreStateChange_StoreDTOs = Stores
                .Select(x => new ReportStoreStateChange_StoreDTO(x)).ToList();
            return ReportStoreStateChange_StoreDTOs;
        }

        [Route(ReportStoreStateChangeRoute.FilterListStoreStatusHistoryType), HttpPost]
        public async Task<List<ReportStoreStateChange_StoreStatusHistoryTypeDTO>> FilterListStoreStatusHistoryType([FromBody] ReportStoreStateChange_StoreStatusHistoryTypeFilterDTO ReportStoreStateChange_StoreStatusHistoryTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreStatusHistoryTypeFilter StoreStatusHistoryTypeFilter = new StoreStatusHistoryTypeFilter();
            StoreStatusHistoryTypeFilter.Skip = 0;
            StoreStatusHistoryTypeFilter.Take = 20;
            StoreStatusHistoryTypeFilter.OrderBy = StoreStatusHistoryTypeOrder.Id;
            StoreStatusHistoryTypeFilter.OrderType = OrderType.ASC;
            StoreStatusHistoryTypeFilter.Selects = StoreStatusHistoryTypeSelect.ALL;
            StoreStatusHistoryTypeFilter.Id = ReportStoreStateChange_StoreStatusHistoryTypeFilterDTO.Id;
            StoreStatusHistoryTypeFilter.Code = ReportStoreStateChange_StoreStatusHistoryTypeFilterDTO.Code;
            StoreStatusHistoryTypeFilter.Name = ReportStoreStateChange_StoreStatusHistoryTypeFilterDTO.Name;

            List<StoreStatusHistoryType> StoreStatusHistoryTypes = await StoreStatusHistoryTypeService.List(StoreStatusHistoryTypeFilter);
            List<ReportStoreStateChange_StoreStatusHistoryTypeDTO> ReportStoreStateChange_StoreStatusHistoryTypeDTOs = StoreStatusHistoryTypes
                .Select(x => new ReportStoreStateChange_StoreStatusHistoryTypeDTO(x)).ToList();
            return ReportStoreStateChange_StoreStatusHistoryTypeDTOs;
        }

        [Route(ReportStoreStateChangeRoute.FilterListProvince), HttpPost]
        public async Task<List<ReportStoreStateChange_ProvinceDTO>> FilterListProvince([FromBody] ReportStoreStateChange_ProvinceFilterDTO ReportStoreStateChange_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = ReportStoreStateChange_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = ReportStoreStateChange_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = ReportStoreStateChange_ProvinceFilterDTO.StatusId;

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<ReportStoreStateChange_ProvinceDTO> ReportStoreStateChange_ProvinceDTOs = Provinces
                .Select(x => new ReportStoreStateChange_ProvinceDTO(x)).ToList();
            return ReportStoreStateChange_ProvinceDTOs;
        }

        [Route(ReportStoreStateChangeRoute.FilterListDistrict), HttpPost]
        public async Task<List<ReportStoreStateChange_DistrictDTO>> FilterListDistrict([FromBody] ReportStoreStateChange_DistrictFilterDTO ReportStoreStateChange_DistrictFilterDTO)
        {
            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = ReportStoreStateChange_DistrictFilterDTO.Id;
            DistrictFilter.Name = ReportStoreStateChange_DistrictFilterDTO.Name;
            DistrictFilter.Priority = ReportStoreStateChange_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = ReportStoreStateChange_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = ReportStoreStateChange_DistrictFilterDTO.StatusId;

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<ReportStoreStateChange_DistrictDTO> ReportStoreStateChange_DistrictDTOs = Districts
                .Select(x => new ReportStoreStateChange_DistrictDTO(x)).ToList();
            return ReportStoreStateChange_DistrictDTOs;
        }


        #endregion


        [Route(ReportStoreStateChangeRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] ReportStoreStateChange_ReportStoreStateChangeFilterDTO ReportStoreStateChange_ReportStoreStateChangeFilterDTO)
        {
            IQueryable<StoreStatusHistoryDAO> StoreStatusHistoryDAOs = await Filter(ReportStoreStateChange_ReportStoreStateChangeFilterDTO);
            return await StoreStatusHistoryDAOs.CountWithNoLockAsync();
        }
        [Route(ReportStoreStateChangeRoute.List), HttpPost]
        public async Task<List<ReportStoreStateChange_ReportStoreStateChangeDTO>> List([FromBody] ReportStoreStateChange_ReportStoreStateChangeFilterDTO ReportStoreStateChange_ReportStoreStateChangeFilterDTO)
        {
            IQueryable<StoreStatusHistoryDAO> StoreStatusHistoryDAOs = await Filter(ReportStoreStateChange_ReportStoreStateChangeFilterDTO);
            List<StoreStatusHistoryDAO> result = await StoreStatusHistoryDAOs
                .Include(x => x.Store.Organization)
                .Include(x => x.PreviousStoreStatus)
                .Include(x => x.StoreStatus)
                .ToListWithNoLockAsync();
            List<ReportStoreStateChange_ReportStoreStateChangeDetailDTO> ReportStoreStateChange_ReportStoreStateChangeDetailDTOs = new List<ReportStoreStateChange_ReportStoreStateChangeDetailDTO>();
            for (int i = 0; i < StoreStatusHistoryDAOs.Count(); i++)
            {
                StoreStatusHistoryDAO StoreStatusHistoryDAO = result[i];
                ReportStoreStateChange_ReportStoreStateChangeDetailDTO ReportStoreStateChange_ReportStoreStateChangeDetailDTO = new ReportStoreStateChange_ReportStoreStateChangeDetailDTO
                {
                    Stt = i + 1,
                    CreatedAt = StoreStatusHistoryDAO.CreatedAt,
                    OrganizationName = StoreStatusHistoryDAO.Store.Organization.Name,
                    PreviousCreatedAt = StoreStatusHistoryDAO.PreviousCreatedAt,
                    PreviousStoreStatus = StoreStatusHistoryDAO.PreviousStoreStatus.Name,
                    StoreAddress = StoreStatusHistoryDAO.Store?.Address,
                    StoreCode = StoreStatusHistoryDAO.Store?.Code,
                    StoreName = StoreStatusHistoryDAO.Store?.Name,
                    StorePhoneNumber = StoreStatusHistoryDAO.Store?.OwnerPhone ?? "",
                    StoreStatus = StoreStatusHistoryDAO.StoreStatus.Name,
                };
                ReportStoreStateChange_ReportStoreStateChangeDetailDTOs.Add(ReportStoreStateChange_ReportStoreStateChangeDetailDTO);
            }

            List<ReportStoreStateChange_ReportStoreStateChangeDTO> ReportStoreStateChange_ReportStoreStateChangeDTOs = ReportStoreStateChange_ReportStoreStateChangeDetailDTOs
                .Select(x => x.OrganizationName).Distinct().Select(x => new ReportStoreStateChange_ReportStoreStateChangeDTO
                {
                    OrganizationName = x,
                }).ToList();

            foreach (ReportStoreStateChange_ReportStoreStateChangeDTO ReportStoreStateChange_ReportStoreStateChangeDTO in ReportStoreStateChange_ReportStoreStateChangeDTOs)
            {
                ReportStoreStateChange_ReportStoreStateChangeDTO.Details = ReportStoreStateChange_ReportStoreStateChangeDetailDTOs
                    .Where(x => x.OrganizationName == ReportStoreStateChange_ReportStoreStateChangeDTO.OrganizationName).ToList();
                ReportStoreStateChange_ReportStoreStateChangeDTO.Total = ReportStoreStateChange_ReportStoreStateChangeDTO.Details.Count();
            }
            return ReportStoreStateChange_ReportStoreStateChangeDTOs;
        }

        [Route(ReportStoreStateChangeRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] ReportStoreStateChange_ReportStoreStateChangeFilterDTO ReportStoreStateChange_ReportStoreStateChangeFilterDTO)
        {
            DateTime Start = ReportStoreStateChange_ReportStoreStateChangeFilterDTO.CreatedAt?.GreaterEqual == null ?
               LocalStartDay(CurrentContext) :
               ReportStoreStateChange_ReportStoreStateChangeFilterDTO.CreatedAt.GreaterEqual.Value;

            DateTime End = ReportStoreStateChange_ReportStoreStateChangeFilterDTO.CreatedAt?.LessEqual == null ?
                LocalEndDay(CurrentContext) :
                ReportStoreStateChange_ReportStoreStateChangeFilterDTO.CreatedAt.LessEqual.Value;

            ReportStoreStateChange_ReportStoreStateChangeFilterDTO.Skip = 0;
            ReportStoreStateChange_ReportStoreStateChangeFilterDTO.Take = int.MaxValue;
            IQueryable<StoreStatusHistoryDAO> StoreStatusHistoryDAOs = await Filter(ReportStoreStateChange_ReportStoreStateChangeFilterDTO);
            List<StoreStatusHistoryDAO> result = await StoreStatusHistoryDAOs
                .Include(x => x.Store.Organization)
                .Include(x => x.PreviousStoreStatus)
                .Include(x => x.StoreStatus)
                .ToListWithNoLockAsync();

            List<ReportStoreStateChange_ReportStoreStateChangeDetailDTO> ReportStoreStateChange_ReportStoreStateChangeDetailDTOs = new List<ReportStoreStateChange_ReportStoreStateChangeDetailDTO>();
            for (int i = 0; i < StoreStatusHistoryDAOs.Count(); i++)
            {
                StoreStatusHistoryDAO StoreStatusHistoryDAO = result[i];
                ReportStoreStateChange_ReportStoreStateChangeDetailDTO ReportStoreStateChange_ReportStoreStateChangeDetailDTO = new ReportStoreStateChange_ReportStoreStateChangeDetailDTO
                {
                    Stt = i + 1,
                    CreatedAt = StoreStatusHistoryDAO.CreatedAt,
                    OrganizationName = StoreStatusHistoryDAO.Store.Organization.Name,
                    PreviousCreatedAt = StoreStatusHistoryDAO.PreviousCreatedAt,
                    PreviousStoreStatus = StoreStatusHistoryDAO.PreviousStoreStatus.Name,
                    StoreAddress = StoreStatusHistoryDAO.Store?.Address,
                    StoreCode = StoreStatusHistoryDAO.Store?.Code,
                    StoreName = StoreStatusHistoryDAO.Store?.Name,
                    StorePhoneNumber = StoreStatusHistoryDAO.Store?.OwnerPhone ?? "",
                    StoreStatus = StoreStatusHistoryDAO.StoreStatus.Name,
                };
                ReportStoreStateChange_ReportStoreStateChangeDetailDTOs.Add(ReportStoreStateChange_ReportStoreStateChangeDetailDTO);
            }

            List<ReportStoreStateChange_ReportStoreStateChangeDTO> ReportStoreStateChange_ReportStoreStateChangeDTOs = ReportStoreStateChange_ReportStoreStateChangeDetailDTOs
                .Select(x => x.OrganizationName).Distinct().Select(x => new ReportStoreStateChange_ReportStoreStateChangeDTO
                {
                    OrganizationName = x,
                }).ToList();

            foreach (ReportStoreStateChange_ReportStoreStateChangeDTO ReportStoreStateChange_ReportStoreStateChangeDTO in ReportStoreStateChange_ReportStoreStateChangeDTOs)
            {
                ReportStoreStateChange_ReportStoreStateChangeDTO.Details = ReportStoreStateChange_ReportStoreStateChangeDetailDTOs
                    .Where(x => x.OrganizationName == ReportStoreStateChange_ReportStoreStateChangeDTO.OrganizationName).ToList();
                ReportStoreStateChange_ReportStoreStateChangeDTO.Total = ReportStoreStateChange_ReportStoreStateChangeDTO.Details.Count();
            }
            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            long Total = ReportStoreStateChange_ReportStoreStateChangeDTOs.Select(x => x.Total).Sum();

            string path = "Templates/Report_Store_State_Change.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.ReportStoreStateChange = ReportStoreStateChange_ReportStoreStateChangeDTOs;
            Data.Total = Total;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "ReportStoreChecked.xlsx");
        }

        private async Task<IQueryable<StoreStatusHistoryDAO>> Filter(ReportStoreStateChange_ReportStoreStateChangeFilterDTO ReportStoreStateChange_ReportStoreStateChangeFilterDTO)
        {
            long? OrganizationId = ReportStoreStateChange_ReportStoreStateChangeFilterDTO.OrganizationId?.Equal;
            long? StoreId = ReportStoreStateChange_ReportStoreStateChangeFilterDTO.StoreId?.Equal;
            long? StoreStatusId = ReportStoreStateChange_ReportStoreStateChangeFilterDTO.StoreStatusId?.Equal;
            long? PreviousStoreStatusId = ReportStoreStateChange_ReportStoreStateChangeFilterDTO.PreviousStoreStatusId?.Equal;
            long? ProvinceId = ReportStoreStateChange_ReportStoreStateChangeFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportStoreStateChange_ReportStoreStateChangeFilterDTO.DistrictId?.Equal;

            IQueryable<StoreStatusHistoryDAO> StoreStatusHistoryDAOs = DataContext.StoreStatusHistory.AsNoTracking();
            if (ReportStoreStateChange_ReportStoreStateChangeFilterDTO.StoreAddress != null)
                StoreStatusHistoryDAOs = StoreStatusHistoryDAOs.Where(x => x.Store.Address, ReportStoreStateChange_ReportStoreStateChangeFilterDTO.StoreAddress);

            if (ReportStoreStateChange_ReportStoreStateChangeFilterDTO.CreatedAt.HasValue)
                StoreStatusHistoryDAOs = StoreStatusHistoryDAOs.Where(x => x.CreatedAt, ReportStoreStateChange_ReportStoreStateChangeFilterDTO.CreatedAt);

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportStoreStateChange_ReportStoreStateChangeFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportStoreStateChange_ReportStoreStateChangeFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();
            List<long> AppUserIds = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            StoreStatusHistoryDAOs = StoreStatusHistoryDAOs.Where(x => OrganizationIds.Contains(x.Store.OrganizationId));
            StoreStatusHistoryDAOs = StoreStatusHistoryDAOs.Where(x => AppUserIds.Contains(x.AppUserId));
            if (StoreId.HasValue)
                StoreStatusHistoryDAOs = StoreStatusHistoryDAOs.Where(x => x.StoreId == StoreId.Value);
            if (StoreStatusId.HasValue)
                StoreStatusHistoryDAOs = StoreStatusHistoryDAOs.Where(x => x.StoreStatusId == StoreStatusId.Value);
            if (PreviousStoreStatusId.HasValue)
                StoreStatusHistoryDAOs = StoreStatusHistoryDAOs.Where(x => x.PreviousStoreStatusId == PreviousStoreStatusId.Value);
            if (ProvinceId.HasValue)
                StoreStatusHistoryDAOs = StoreStatusHistoryDAOs.Where(x => x.Store.ProvinceId == ProvinceId.Value);
            if (DistrictId.HasValue)
                StoreStatusHistoryDAOs = StoreStatusHistoryDAOs.Where(x => x.Store.DistrictId == DistrictId.Value);
            return StoreStatusHistoryDAOs;
        }
    }
}
