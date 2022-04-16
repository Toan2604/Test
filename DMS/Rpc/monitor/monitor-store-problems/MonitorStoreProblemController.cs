using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Rpc.monitor;
using DMS.Services.MAppUser;
using DMS.Services.MDistrict;
using DMS.Services.MOrganization;
using DMS.Services.MProblem;
using DMS.Services.MProblemType;
using DMS.Services.MProvince;
using DMS.Services.MStore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.monitor_store_problems
{
    public class MonitorStoreProblemController : MonitorController
    {
        private IProblemStatusService ProblemStatusService;
        private IProblemTypeService ProblemTypeService;
        private IProblemHistoryService ProblemHistoryService;
        private IStoreService StoreService;
        private IProblemService ProblemService;
        private ICurrentContext CurrentContext;
        private IAppUserService AppUserService;
        private IOrganizationService OrganizationService;
        private IProvinceService ProvinceService;
        private IDistrictService DistrictService;
        public MonitorStoreProblemController(
            IAppUserService AppUserService,
            IOrganizationService OrganizationService,
            IProblemStatusService ProblemStatusService,
            IProblemTypeService ProblemTypeService,
            IProblemHistoryService ProblemHistoryService,
            IStoreService StoreService,
            IProblemService ProblemService,
            IProvinceService ProvinceService,
            IDistrictService DistrictService,
            ICurrentContext CurrentContext
        )
        {
            this.AppUserService = AppUserService;
            this.OrganizationService = OrganizationService;
            this.ProblemStatusService = ProblemStatusService;
            this.ProblemTypeService = ProblemTypeService;
            this.ProblemHistoryService = ProblemHistoryService;
            this.StoreService = StoreService;
            this.ProblemService = ProblemService;
            this.CurrentContext = CurrentContext;
            this.ProvinceService = ProvinceService;
            this.DistrictService = DistrictService;
        }

        [Route(MonitorStoreProblemRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] MonitorStoreProblem_ProblemFilterDTO MonitorStoreProblem_ProblemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProblemFilter ProblemFilter = ConvertFilterDTOToFilterEntity(MonitorStoreProblem_ProblemFilterDTO);
            ProblemFilter = await ProblemService.ToFilter(ProblemFilter);
            int count = await ProblemService.Count(ProblemFilter);
            return count;
        }

        [Route(MonitorStoreProblemRoute.List), HttpPost]
        public async Task<List<MonitorStoreProblem_ProblemDTO>> List([FromBody] MonitorStoreProblem_ProblemFilterDTO MonitorStoreProblem_ProblemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProblemFilter ProblemFilter = ConvertFilterDTOToFilterEntity(MonitorStoreProblem_ProblemFilterDTO);
            ProblemFilter = await ProblemService.ToFilter(ProblemFilter);
            List<Problem> Problems = await ProblemService.List(ProblemFilter);
            List<MonitorStoreProblem_ProblemDTO> Problem_ProblemDTOs = Problems
                .Select(c => new MonitorStoreProblem_ProblemDTO(c)).ToList();
            return Problem_ProblemDTOs;
        }

        [Route(MonitorStoreProblemRoute.Get), HttpPost]
        public async Task<ActionResult<MonitorStoreProblem_ProblemDTO>> Get([FromBody] MonitorStoreProblem_ProblemDTO MonitorStoreProblem_ProblemDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(MonitorStoreProblem_ProblemDTO.Id))
                return Forbid();

            Problem Problem = await ProblemService.Get(MonitorStoreProblem_ProblemDTO.Id);
            return new MonitorStoreProblem_ProblemDTO(Problem);
        }

        [Route(MonitorStoreProblemRoute.Update), HttpPost]
        public async Task<ActionResult<MonitorStoreProblem_ProblemDTO>> Update([FromBody] MonitorStoreProblem_ProblemDTO MonitorStoreProblem_ProblemDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(MonitorStoreProblem_ProblemDTO.Id))
                return Forbid();

            Problem Problem = ConvertDTOToEntity(MonitorStoreProblem_ProblemDTO);
            Problem = await ProblemService.Update(Problem);
            MonitorStoreProblem_ProblemDTO = new MonitorStoreProblem_ProblemDTO(Problem);
            if (Problem.IsValidated)
                return MonitorStoreProblem_ProblemDTO;
            else
                return BadRequest(MonitorStoreProblem_ProblemDTO);
        }

        [Route(MonitorStoreProblemRoute.Delete), HttpPost]
        public async Task<ActionResult<MonitorStoreProblem_ProblemDTO>> Delete([FromBody] MonitorStoreProblem_ProblemDTO MonitorStoreProblem_ProblemDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(MonitorStoreProblem_ProblemDTO.Id))
                return Forbid();

            Problem Problem = ConvertDTOToEntity(MonitorStoreProblem_ProblemDTO);
            Problem = await ProblemService.Delete(Problem);
            MonitorStoreProblem_ProblemDTO = new MonitorStoreProblem_ProblemDTO(Problem);
            if (Problem.IsValidated)
                return MonitorStoreProblem_ProblemDTO;
            else
                return BadRequest(MonitorStoreProblem_ProblemDTO);
        }

        [Route(MonitorStoreProblemRoute.BulkDelete), HttpPost]
        public async Task<bool> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProblemFilter ProblemFilter = new ProblemFilter();
            ProblemFilter = await ProblemService.ToFilter(ProblemFilter);
            ProblemFilter.Id = new IdFilter { In = Ids };
            ProblemFilter.Selects = ProblemSelect.Id;
            ProblemFilter.Skip = 0;
            ProblemFilter.Take = int.MaxValue;

            List<Problem> Problems = await ProblemService.List(ProblemFilter);
            Problems = await ProblemService.BulkDelete(Problems);
            return true;
        }

        [Route(MonitorStoreProblemRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] MonitorStoreProblem_ProblemFilterDTO MonitorStoreProblem_ProblemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            MonitorStoreProblem_ProblemFilterDTO.Skip = 0;
            MonitorStoreProblem_ProblemFilterDTO.Take = int.MaxValue;
            List<MonitorStoreProblem_ProblemDTO> MonitorStoreProblem_ProblemDTOs = await List(MonitorStoreProblem_ProblemFilterDTO);
            MonitorStoreProblem_ProblemDTOs = MonitorStoreProblem_ProblemDTOs
                .OrderBy(x => x.Creator.Username).ThenByDescending(x => x.NoteAt)
                .ToList();

            var OrganizationNames = MonitorStoreProblem_ProblemDTOs
                .OrderBy(x => x.OrganizationId)
                .Select(x => x.Organization?.Name)
                .Distinct()
                .ToList();

            List<MonitorStoreProblem_ExportDTO> MonitorStoreProblem_ExportDTOs = new List<MonitorStoreProblem_ExportDTO>();
            foreach (var OrganizationName in OrganizationNames)
            {
                MonitorStoreProblem_ExportDTO MonitorStoreProblem_ExportDTO = new MonitorStoreProblem_ExportDTO
                {
                    OrganizationName = OrganizationName
                };
                MonitorStoreProblem_ExportDTOs.Add(MonitorStoreProblem_ExportDTO);
                MonitorStoreProblem_ExportDTO.MonitorStoreProblems = MonitorStoreProblem_ProblemDTOs.Where(x => x.Organization.Name == OrganizationName).ToList();
            }

            int stt = 1;
            foreach (var MonitorStoreProblem_ExportDTO in MonitorStoreProblem_ExportDTOs)
            {
                foreach (MonitorStoreProblem_ProblemDTO MonitorStoreProblem_ProblemDTO in MonitorStoreProblem_ExportDTO.MonitorStoreProblems)
                {
                    MonitorStoreProblem_ProblemDTO.STT = stt;
                    stt++;
                }
            }
            DateTime Start = MonitorStoreProblem_ProblemFilterDTO.NoteAt?.GreaterEqual == null ?
               LocalStartDay(CurrentContext) :
               MonitorStoreProblem_ProblemFilterDTO.NoteAt.GreaterEqual.Value;

            DateTime End = MonitorStoreProblem_ProblemFilterDTO.NoteAt?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    MonitorStoreProblem_ProblemFilterDTO.NoteAt.LessEqual.Value;
            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            string path = "Templates/Monitor_Store_Problem_Report.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.Exports = MonitorStoreProblem_ExportDTOs;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "Monitor_Store_Problem_Report.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            ProblemFilter ProblemFilter = new ProblemFilter();
            ProblemFilter = await ProblemService.ToFilter(ProblemFilter);
            if (Id == 0)
            {

            }
            else
            {
                ProblemFilter.Id = new IdFilter { Equal = Id };
                int count = await ProblemService.Count(ProblemFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private Problem ConvertDTOToEntity(MonitorStoreProblem_ProblemDTO MonitorStoreProblem_ProblemDTO)
        {
            MonitorStoreProblem_ProblemDTO.TrimString();
            Problem Problem = new Problem();
            Problem.Id = MonitorStoreProblem_ProblemDTO.Id;
            Problem.Code = MonitorStoreProblem_ProblemDTO.Code;
            Problem.StoreCheckingId = MonitorStoreProblem_ProblemDTO.StoreCheckingId;
            Problem.StoreId = MonitorStoreProblem_ProblemDTO.StoreId;
            Problem.CreatorId = MonitorStoreProblem_ProblemDTO.CreatorId;
            Problem.ProblemTypeId = MonitorStoreProblem_ProblemDTO.ProblemTypeId;
            Problem.NoteAt = MonitorStoreProblem_ProblemDTO.NoteAt;
            Problem.CompletedAt = MonitorStoreProblem_ProblemDTO.CompletedAt;
            Problem.Content = MonitorStoreProblem_ProblemDTO.Content;
            Problem.ProblemStatusId = MonitorStoreProblem_ProblemDTO.ProblemStatusId;
            Problem.Creator = MonitorStoreProblem_ProblemDTO.Creator == null ? null : new AppUser
            {
                Id = MonitorStoreProblem_ProblemDTO.Creator.Id,
                Username = MonitorStoreProblem_ProblemDTO.Creator.Username,
                DisplayName = MonitorStoreProblem_ProblemDTO.Creator.DisplayName,
                Address = MonitorStoreProblem_ProblemDTO.Creator.Address,
                Email = MonitorStoreProblem_ProblemDTO.Creator.Email,
                Phone = MonitorStoreProblem_ProblemDTO.Creator.Phone,
                PositionId = MonitorStoreProblem_ProblemDTO.Creator.PositionId,
                Department = MonitorStoreProblem_ProblemDTO.Creator.Department,
                OrganizationId = MonitorStoreProblem_ProblemDTO.Creator.OrganizationId,
                StatusId = MonitorStoreProblem_ProblemDTO.Creator.StatusId,
                Avatar = MonitorStoreProblem_ProblemDTO.Creator.Avatar,
                ProvinceId = MonitorStoreProblem_ProblemDTO.Creator.ProvinceId,
                SexId = MonitorStoreProblem_ProblemDTO.Creator.SexId,
                Birthday = MonitorStoreProblem_ProblemDTO.Creator.Birthday,
            };
            Problem.ProblemStatus = MonitorStoreProblem_ProblemDTO.ProblemStatus == null ? null : new ProblemStatus
            {
                Id = MonitorStoreProblem_ProblemDTO.ProblemStatus.Id,
                Code = MonitorStoreProblem_ProblemDTO.ProblemStatus.Code,
                Name = MonitorStoreProblem_ProblemDTO.ProblemStatus.Name,
            };
            Problem.ProblemType = MonitorStoreProblem_ProblemDTO.ProblemType == null ? null : new ProblemType
            {
                Id = MonitorStoreProblem_ProblemDTO.ProblemType.Id,
                Code = MonitorStoreProblem_ProblemDTO.ProblemType.Code,
                Name = MonitorStoreProblem_ProblemDTO.ProblemType.Name,
            };
            Problem.Store = MonitorStoreProblem_ProblemDTO.Store == null ? null : new Store
            {
                Id = MonitorStoreProblem_ProblemDTO.Store.Id,
                Code = MonitorStoreProblem_ProblemDTO.Store.Code,
                Name = MonitorStoreProblem_ProblemDTO.Store.Name,
                ParentStoreId = MonitorStoreProblem_ProblemDTO.Store.ParentStoreId,
                OrganizationId = MonitorStoreProblem_ProblemDTO.Store.OrganizationId,
                StoreTypeId = MonitorStoreProblem_ProblemDTO.Store.StoreTypeId,
                Telephone = MonitorStoreProblem_ProblemDTO.Store.Telephone,
                ProvinceId = MonitorStoreProblem_ProblemDTO.Store.ProvinceId,
                DistrictId = MonitorStoreProblem_ProblemDTO.Store.DistrictId,
                WardId = MonitorStoreProblem_ProblemDTO.Store.WardId,
                Address = MonitorStoreProblem_ProblemDTO.Store.Address,
                DeliveryAddress = MonitorStoreProblem_ProblemDTO.Store.DeliveryAddress,
                Latitude = MonitorStoreProblem_ProblemDTO.Store.Latitude,
                Longitude = MonitorStoreProblem_ProblemDTO.Store.Longitude,
                DeliveryLatitude = MonitorStoreProblem_ProblemDTO.Store.DeliveryLatitude,
                DeliveryLongitude = MonitorStoreProblem_ProblemDTO.Store.DeliveryLongitude,
                OwnerName = MonitorStoreProblem_ProblemDTO.Store.OwnerName,
                OwnerPhone = MonitorStoreProblem_ProblemDTO.Store.OwnerPhone,
                OwnerEmail = MonitorStoreProblem_ProblemDTO.Store.OwnerEmail,
                TaxCode = MonitorStoreProblem_ProblemDTO.Store.TaxCode,
                LegalEntity = MonitorStoreProblem_ProblemDTO.Store.LegalEntity,
                StatusId = MonitorStoreProblem_ProblemDTO.Store.StatusId,
            };
            Problem.StoreChecking = MonitorStoreProblem_ProblemDTO.StoreChecking == null ? null : new StoreChecking
            {
                Id = MonitorStoreProblem_ProblemDTO.StoreChecking.Id,
                StoreId = MonitorStoreProblem_ProblemDTO.StoreChecking.StoreId,
                SaleEmployeeId = MonitorStoreProblem_ProblemDTO.StoreChecking.SaleEmployeeId,
                Longitude = MonitorStoreProblem_ProblemDTO.StoreChecking.Longitude,
                Latitude = MonitorStoreProblem_ProblemDTO.StoreChecking.Latitude,
                CheckInAt = MonitorStoreProblem_ProblemDTO.StoreChecking.CheckInAt,
                CheckOutAt = MonitorStoreProblem_ProblemDTO.StoreChecking.CheckOutAt,
            };
            Problem.ProblemImageMappings = MonitorStoreProblem_ProblemDTO.ProblemImageMappings?
                .Select(x => new ProblemImageMapping
                {
                    ImageId = x.ImageId,
                    Image = x.Image == null ? null : new Image
                    {
                        Id = x.Image.Id,
                        Name = x.Image.Name,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                    },
                }).ToList();
            Problem.ProblemHistories = MonitorStoreProblem_ProblemDTO.ProblemHistorys?
                .Select(x => new ProblemHistory
                {
                    Id = x.Id,
                    ModifierId = x.ModifierId,
                    ProblemId = x.ProblemId,
                    ProblemStatusId = x.ProblemStatusId,
                    Time = x.Time
                }).ToList();
            Problem.BaseLanguage = CurrentContext.Language;
            return Problem;
        }

        private ProblemFilter ConvertFilterDTOToFilterEntity(MonitorStoreProblem_ProblemFilterDTO MonitorStoreProblem_ProblemFilterDTO)
        {
            ProblemFilter ProblemFilter = new ProblemFilter();
            ProblemFilter.Selects = ProblemSelect.ALL;
            ProblemFilter.Skip = MonitorStoreProblem_ProblemFilterDTO.Skip;
            ProblemFilter.Take = MonitorStoreProblem_ProblemFilterDTO.Take;
            ProblemFilter.OrderBy = MonitorStoreProblem_ProblemFilterDTO.OrderBy;
            ProblemFilter.OrderType = MonitorStoreProblem_ProblemFilterDTO.OrderType;

            ProblemFilter.Id = MonitorStoreProblem_ProblemFilterDTO.Id;
            ProblemFilter.Code = MonitorStoreProblem_ProblemFilterDTO.Code;
            ProblemFilter.StoreCheckingId = MonitorStoreProblem_ProblemFilterDTO.StoreCheckingId;
            ProblemFilter.OrganizationId = MonitorStoreProblem_ProblemFilterDTO.OrganizationId;
            ProblemFilter.StoreId = MonitorStoreProblem_ProblemFilterDTO.StoreId;
            ProblemFilter.AppUserId = MonitorStoreProblem_ProblemFilterDTO.AppUserId;
            ProblemFilter.ProblemTypeId = MonitorStoreProblem_ProblemFilterDTO.ProblemTypeId;
            ProblemFilter.NoteAt = MonitorStoreProblem_ProblemFilterDTO.NoteAt;
            ProblemFilter.CompletedAt = MonitorStoreProblem_ProblemFilterDTO.CompletedAt;
            ProblemFilter.Content = MonitorStoreProblem_ProblemFilterDTO.Content;
            ProblemFilter.ProblemStatusId = MonitorStoreProblem_ProblemFilterDTO.ProblemStatusId;
            ProblemFilter.ProvinceId = MonitorStoreProblem_ProblemFilterDTO.ProvinceId;
            ProblemFilter.DistrictId = MonitorStoreProblem_ProblemFilterDTO.DistrictId;
            return ProblemFilter;
        }

        [Route(MonitorStoreProblemRoute.FilterListAppUser), HttpPost]
        public async Task<List<MonitorStoreProblem_AppUserDTO>> FilterListAppUser([FromBody] MonitorStoreProblem_AppUserFilterDTO MonitorStoreProblem_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = MonitorStoreProblem_AppUserFilterDTO.Id;
            AppUserFilter.Username = MonitorStoreProblem_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = MonitorStoreProblem_AppUserFilterDTO.DisplayName;
            AppUserFilter.OrganizationId = MonitorStoreProblem_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<MonitorStoreProblem_AppUserDTO> Problem_AppUserDTOs = AppUsers
                .Select(x => new MonitorStoreProblem_AppUserDTO(x)).ToList();
            return Problem_AppUserDTOs;
        }

        [Route(MonitorStoreProblemRoute.FilterListOrganization), HttpPost]
        public async Task<List<MonitorStoreProblem_OrganizationDTO>> FilterListOrganization()
        {
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
            List<MonitorStoreProblem_OrganizationDTO> MonitorStoreProblem_OrganizationDTOs = Organizations
                .Select(x => new MonitorStoreProblem_OrganizationDTO(x)).ToList();
            return MonitorStoreProblem_OrganizationDTOs;
        }

        [Route(MonitorStoreProblemRoute.FilterListProblemStatus), HttpPost]
        public async Task<List<MonitorStoreProblem_ProblemStatusDTO>> FilterListProblemStatus([FromBody] MonitorStoreProblem_ProblemStatusFilterDTO MonitorStoreProblem_ProblemStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProblemStatusFilter ProblemStatusFilter = new ProblemStatusFilter();
            ProblemStatusFilter.Skip = 0;
            ProblemStatusFilter.Take = 20;
            ProblemStatusFilter.OrderBy = ProblemStatusOrder.Id;
            ProblemStatusFilter.OrderType = OrderType.ASC;
            ProblemStatusFilter.Selects = ProblemStatusSelect.ALL;
            ProblemStatusFilter.Id = MonitorStoreProblem_ProblemStatusFilterDTO.Id;
            ProblemStatusFilter.Code = MonitorStoreProblem_ProblemStatusFilterDTO.Code;
            ProblemStatusFilter.Name = MonitorStoreProblem_ProblemStatusFilterDTO.Name;

            List<ProblemStatus> ProblemStatuses = await ProblemStatusService.List(ProblemStatusFilter);
            List<MonitorStoreProblem_ProblemStatusDTO> Problem_ProblemStatusDTOs = ProblemStatuses
                .Select(x => new MonitorStoreProblem_ProblemStatusDTO(x)).ToList();
            return Problem_ProblemStatusDTOs;
        }
        [Route(MonitorStoreProblemRoute.FilterListProblemType), HttpPost]
        public async Task<List<MonitorStoreProblem_ProblemTypeDTO>> FilterListProblemType([FromBody] MonitorStoreProblem_ProblemTypeFilterDTO MonitorStoreProblem_ProblemTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProblemTypeFilter ProblemTypeFilter = new ProblemTypeFilter();
            ProblemTypeFilter.Skip = 0;
            ProblemTypeFilter.Take = int.MaxValue;
            ProblemTypeFilter.Take = 20;
            ProblemTypeFilter.OrderBy = ProblemTypeOrder.Id;
            ProblemTypeFilter.OrderType = OrderType.ASC;
            ProblemTypeFilter.Selects = ProblemTypeSelect.ALL;

            List<ProblemType> ProblemTypes = await ProblemTypeService.List(ProblemTypeFilter);
            List<MonitorStoreProblem_ProblemTypeDTO> Problem_ProblemTypeDTOs = ProblemTypes
                .Select(x => new MonitorStoreProblem_ProblemTypeDTO(x)).ToList();
            return Problem_ProblemTypeDTOs;
        }
        [Route(MonitorStoreProblemRoute.FilterListStore), HttpPost]
        public async Task<List<MonitorStoreProblem_StoreDTO>> FilterListStore([FromBody] MonitorStoreProblem_StoreFilterDTO MonitorStoreProblem_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = MonitorStoreProblem_StoreFilterDTO.Id;
            StoreFilter.Code = MonitorStoreProblem_StoreFilterDTO.Code;
            StoreFilter.Name = MonitorStoreProblem_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = MonitorStoreProblem_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = MonitorStoreProblem_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = MonitorStoreProblem_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = MonitorStoreProblem_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StatusId = null;

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<MonitorStoreProblem_StoreDTO> Problem_StoreDTOs = Stores
                .Select(x => new MonitorStoreProblem_StoreDTO(x)).ToList();
            return Problem_StoreDTOs;
        }

        [Route(MonitorStoreProblemRoute.FilterListProvince), HttpPost]
        public async Task<List<MonitorStoreProblem_ProvinceDTO>> FilterListProvince([FromBody] MonitorStoreProblem_ProvinceFilterDTO MonitorStoreProblem_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = MonitorStoreProblem_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = MonitorStoreProblem_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = MonitorStoreProblem_ProvinceFilterDTO.StatusId;

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<MonitorStoreProblem_ProvinceDTO> MonitorStoreProblem_ProvinceDTOs = Provinces
                .Select(x => new MonitorStoreProblem_ProvinceDTO(x)).ToList();
            return MonitorStoreProblem_ProvinceDTOs;
        }

        [Route(MonitorStoreProblemRoute.FilterListDistrict), HttpPost]
        public async Task<List<MonitorStoreProblem_DistrictDTO>> FilterListDistrict([FromBody] MonitorStoreProblem_DistrictFilterDTO MonitorStoreProblem_DistrictFilterDTO)
        {
            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = MonitorStoreProblem_DistrictFilterDTO.Id;
            DistrictFilter.Name = MonitorStoreProblem_DistrictFilterDTO.Name;
            DistrictFilter.Priority = MonitorStoreProblem_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = MonitorStoreProblem_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = MonitorStoreProblem_DistrictFilterDTO.StatusId;

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<MonitorStoreProblem_DistrictDTO> MonitorStoreProblem_DistrictDTOs = Districts
                .Select(x => new MonitorStoreProblem_DistrictDTO(x)).ToList();
            return MonitorStoreProblem_DistrictDTOs;
        }


        [Route(MonitorStoreProblemRoute.SingleListAppUser), HttpPost]
        public async Task<List<MonitorStoreProblem_AppUserDTO>> SingleListAppUser([FromBody] MonitorStoreProblem_AppUserFilterDTO MonitorStoreProblem_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = MonitorStoreProblem_AppUserFilterDTO.Id;
            AppUserFilter.Username = MonitorStoreProblem_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = MonitorStoreProblem_AppUserFilterDTO.DisplayName;
            AppUserFilter.OrganizationId = MonitorStoreProblem_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<MonitorStoreProblem_AppUserDTO> Problem_AppUserDTOs = AppUsers
                .Select(x => new MonitorStoreProblem_AppUserDTO(x)).ToList();
            return Problem_AppUserDTOs;
        }

        [Route(MonitorStoreProblemRoute.SingleListOrganization), HttpPost]
        public async Task<List<MonitorStoreProblem_OrganizationDTO>> SingleListOrganization()
        {
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
            List<MonitorStoreProblem_OrganizationDTO> MonitorStoreProblem_OrganizationDTOs = Organizations
                .Select(x => new MonitorStoreProblem_OrganizationDTO(x)).ToList();
            return MonitorStoreProblem_OrganizationDTOs;
        }

        [Route(MonitorStoreProblemRoute.SingleListProblemStatus), HttpPost]
        public async Task<List<MonitorStoreProblem_ProblemStatusDTO>> SingleListProblemStatus([FromBody] MonitorStoreProblem_ProblemStatusFilterDTO MonitorStoreProblem_ProblemStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProblemStatusFilter ProblemStatusFilter = new ProblemStatusFilter();
            ProblemStatusFilter.Skip = 0;
            ProblemStatusFilter.Take = 20;
            ProblemStatusFilter.OrderBy = ProblemStatusOrder.Id;
            ProblemStatusFilter.OrderType = OrderType.ASC;
            ProblemStatusFilter.Selects = ProblemStatusSelect.ALL;
            ProblemStatusFilter.Id = MonitorStoreProblem_ProblemStatusFilterDTO.Id;
            ProblemStatusFilter.Code = MonitorStoreProblem_ProblemStatusFilterDTO.Code;
            ProblemStatusFilter.Name = MonitorStoreProblem_ProblemStatusFilterDTO.Name;

            List<ProblemStatus> ProblemStatuses = await ProblemStatusService.List(ProblemStatusFilter);
            List<MonitorStoreProblem_ProblemStatusDTO> Problem_ProblemStatusDTOs = ProblemStatuses
                .Select(x => new MonitorStoreProblem_ProblemStatusDTO(x)).ToList();
            return Problem_ProblemStatusDTOs;
        }
        [Route(MonitorStoreProblemRoute.SingleListProblemType), HttpPost]
        public async Task<List<MonitorStoreProblem_ProblemTypeDTO>> SingleListProblemType([FromBody] MonitorStoreProblem_ProblemTypeFilterDTO MonitorStoreProblem_ProblemTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProblemTypeFilter ProblemTypeFilter = new ProblemTypeFilter();
            ProblemTypeFilter.Skip = 0;
            ProblemTypeFilter.Take = int.MaxValue;
            ProblemTypeFilter.Take = 20;
            ProblemTypeFilter.OrderBy = ProblemTypeOrder.Id;
            ProblemTypeFilter.OrderType = OrderType.ASC;
            ProblemTypeFilter.Selects = ProblemTypeSelect.ALL;

            List<ProblemType> ProblemTypes = await ProblemTypeService.List(ProblemTypeFilter);
            List<MonitorStoreProblem_ProblemTypeDTO> Problem_ProblemTypeDTOs = ProblemTypes
                .Select(x => new MonitorStoreProblem_ProblemTypeDTO(x)).ToList();
            return Problem_ProblemTypeDTOs;
        }
        [Route(MonitorStoreProblemRoute.SingleListStore), HttpPost]
        public async Task<List<MonitorStoreProblem_StoreDTO>> SingleListStore([FromBody] MonitorStoreProblem_StoreFilterDTO MonitorStoreProblem_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = MonitorStoreProblem_StoreFilterDTO.Id;
            StoreFilter.Code = MonitorStoreProblem_StoreFilterDTO.Code;
            StoreFilter.Name = MonitorStoreProblem_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = MonitorStoreProblem_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = MonitorStoreProblem_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = MonitorStoreProblem_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = MonitorStoreProblem_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<MonitorStoreProblem_StoreDTO> Problem_StoreDTOs = Stores
                .Select(x => new MonitorStoreProblem_StoreDTO(x)).ToList();
            return Problem_StoreDTOs;
        }

        [Route(MonitorStoreProblemRoute.CountProblemHistory), HttpPost]
        public async Task<int> CountProblemHistory([FromBody] MonitorStoreProblem_ProblemHistoryFilterDTO MonitorStoreProblem_ProblemHistoryFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProblemHistoryFilter ProblemHistoryFilter = new ProblemHistoryFilter();
            ProblemHistoryFilter.ProblemId = MonitorStoreProblem_ProblemHistoryFilterDTO.ProblemId;
            ProblemHistoryFilter.Time = MonitorStoreProblem_ProblemHistoryFilterDTO.Time;

            return await ProblemHistoryService.Count(ProblemHistoryFilter);
        }

        [Route(MonitorStoreProblemRoute.ListProblemHistory), HttpPost]
        public async Task<List<MonitorStoreProblem_ProblemHistoryDTO>> ListProblemHistory([FromBody] MonitorStoreProblem_ProblemHistoryFilterDTO MonitorStoreProblem_ProblemHistoryFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProblemHistoryFilter ProblemHistoryFilter = new ProblemHistoryFilter();
            ProblemHistoryFilter.Skip = MonitorStoreProblem_ProblemHistoryFilterDTO.Skip;
            ProblemHistoryFilter.Take = MonitorStoreProblem_ProblemHistoryFilterDTO.Take;
            ProblemHistoryFilter.OrderBy = ProblemHistoryOrder.Time;
            ProblemHistoryFilter.OrderType = OrderType.DESC;
            ProblemHistoryFilter.Selects = ProblemHistorySelect.ALL;
            ProblemHistoryFilter.ProblemId = MonitorStoreProblem_ProblemHistoryFilterDTO.ProblemId;
            ProblemHistoryFilter.Time = MonitorStoreProblem_ProblemHistoryFilterDTO.Time;

            List<ProblemHistory> ProblemHistorys = await ProblemHistoryService.List(ProblemHistoryFilter);
            List<MonitorStoreProblem_ProblemHistoryDTO> MonitorStoreProblem_ProblemHistoryDTOs = ProblemHistorys
                .Select(x => new MonitorStoreProblem_ProblemHistoryDTO(x)).ToList();
            return MonitorStoreProblem_ProblemHistoryDTOs;
        }
    }
}

