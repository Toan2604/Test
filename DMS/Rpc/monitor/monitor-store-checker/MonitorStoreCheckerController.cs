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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_store_checker
{
    public class MonitorStoreCheckerController : MonitorController
    {
        private DataContext DataContext;
        private DWContext DWContext;
        private IAppUserService AppUserService;
        private IOrganizationService OrganizationService;
        private ICurrentContext CurrentContext;
        private IProvinceService ProvinceService;
        private IDistrictService DistrictService;
        public MonitorStoreCheckerController(
            DataContext DataContext,
            DWContext DWContext,
            IOrganizationService OrganizationService,
            IAppUserService AppUserService,
            IProvinceService ProvinceService,
            IDistrictService DistrictService,
            ICurrentContext CurrentContext)
        {
            this.DataContext = DataContext;
            this.DWContext = DWContext;
            this.AppUserService = AppUserService;
            this.OrganizationService = OrganizationService;
            this.CurrentContext = CurrentContext;
            this.ProvinceService = ProvinceService;
            this.DistrictService = DistrictService;
        }

        [Route(MonitorStoreCheckerRoute.FilterListAppUser), HttpPost]
        public async Task<List<MonitorStoreChecker_AppUserDTO>> FilterListAppUser([FromBody] StoreCheckerMonitor_AppUserFilterDTO StoreCheckerMonitor_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.Id = StoreCheckerMonitor_AppUserFilterDTO.Id;
            AppUserFilter.Username = StoreCheckerMonitor_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = StoreCheckerMonitor_AppUserFilterDTO.DisplayName;
            AppUserFilter.OrganizationId = StoreCheckerMonitor_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<MonitorStoreChecker_AppUserDTO> StoreCheckerMonitor_AppUserDTOs = AppUsers
                .Select(x => new MonitorStoreChecker_AppUserDTO(x)).ToList();
            return StoreCheckerMonitor_AppUserDTOs;
        }

        [Route(MonitorStoreCheckerRoute.FilterListOrganization), HttpPost]
        public async Task<List<MonitorStoreChecker_OrganizationDTO>> FilterListOrganization()
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
            List<MonitorStoreChecker_OrganizationDTO> StoreCheckerMonitor_OrganizationDTOs = Organizations
                .Select(x => new MonitorStoreChecker_OrganizationDTO(x)).ToList();
            return StoreCheckerMonitor_OrganizationDTOs;
        }

        [Route(MonitorStoreCheckerRoute.FilterListChecking), HttpPost]
        public List<EnumList> FilterListChecking()
        {
            List<EnumList> EnumList = new List<EnumList>();
            EnumList.Add(new EnumList { Id = 0, Name = "Chưa viếng thăm" });
            EnumList.Add(new EnumList { Id = 1, Name = "Đã viếng thăm" });
            return EnumList;
        }

        [Route(MonitorStoreCheckerRoute.FilterListImage), HttpPost]
        public List<EnumList> FilterListImage()
        {
            List<EnumList> EnumList = new List<EnumList>();
            EnumList.Add(new EnumList { Id = 0, Name = "Không hình ảnh" });
            EnumList.Add(new EnumList { Id = 1, Name = "Có hình ảnh" });
            return EnumList;
        }

        [Route(MonitorStoreCheckerRoute.FilterListSalesOrder), HttpPost]
        public List<EnumList> FilterListSalesOrder()
        {
            List<EnumList> EnumList = new List<EnumList>();
            EnumList.Add(new EnumList { Id = 0, Name = "Không có đơn hàng" });
            EnumList.Add(new EnumList { Id = 1, Name = "Có đơn hàng" });
            return EnumList;
        }

        [Route(MonitorStoreCheckerRoute.FilterListProvince), HttpPost]
        public async Task<List<MonitorStoreChecker_ProvinceDTO>> FilterListProvince([FromBody] MonitorStoreChecker_ProvinceFilterDTO MonitorStoreChecker_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = MonitorStoreChecker_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = MonitorStoreChecker_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = MonitorStoreChecker_ProvinceFilterDTO.StatusId;

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<MonitorStoreChecker_ProvinceDTO> MonitorStoreChecker_ProvinceDTOs = Provinces
                .Select(x => new MonitorStoreChecker_ProvinceDTO(x)).ToList();
            return MonitorStoreChecker_ProvinceDTOs;
        }

        [Route(MonitorStoreCheckerRoute.FilterListDistrict), HttpPost]
        public async Task<List<MonitorStoreChecker_DistrictDTO>> FilterListDistrict([FromBody] MonitorStoreChecker_DistrictFilterDTO MonitorStoreChecker_DistrictFilterDTO)
        {
            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = MonitorStoreChecker_DistrictFilterDTO.Id;
            DistrictFilter.Name = MonitorStoreChecker_DistrictFilterDTO.Name;
            DistrictFilter.Priority = MonitorStoreChecker_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = MonitorStoreChecker_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = MonitorStoreChecker_DistrictFilterDTO.StatusId;

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<MonitorStoreChecker_DistrictDTO> MonitorStoreChecker_DistrictDTOs = Districts
                .Select(x => new MonitorStoreChecker_DistrictDTO(x)).ToList();
            return MonitorStoreChecker_DistrictDTOs;
        }


        [Route(MonitorStoreCheckerRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] MonitorStoreChecker_MonitorStoreCheckerFilterDTO MonitorStoreChecker_MonitorStoreCheckerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = MonitorStoreChecker_MonitorStoreCheckerFilterDTO.CheckIn?.GreaterEqual == null ?
              LocalStartDay(CurrentContext) :
              MonitorStoreChecker_MonitorStoreCheckerFilterDTO.CheckIn.GreaterEqual.Value;

            DateTime End = MonitorStoreChecker_MonitorStoreCheckerFilterDTO.CheckIn?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    MonitorStoreChecker_MonitorStoreCheckerFilterDTO.CheckIn.LessEqual.Value;

            long? SaleEmployeeId = MonitorStoreChecker_MonitorStoreCheckerFilterDTO.AppUserId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (MonitorStoreChecker_MonitorStoreCheckerFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == MonitorStoreChecker_MonitorStoreCheckerFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
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
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            var AppUsers = await AppUserService.List(AppUserFilter);
            var AppUserIds = AppUsers.Select(x => x.Id).ToList();

            int count = await DataContext.AppUser.Where(au =>
                AppUserIds.Contains(au.Id) &&
                au.DeletedAt == null &
                (SaleEmployeeId == null || au.Id == SaleEmployeeId.Value))
                .CountWithNoLockAsync();
            return count;
        }

        [Route(MonitorStoreCheckerRoute.List), HttpPost]
        public async Task<ActionResult<List<MonitorStoreChecker_MonitorStoreCheckerDTO>>> List([FromBody] MonitorStoreChecker_MonitorStoreCheckerFilterDTO MonitorStoreChecker_MonitorStoreCheckerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = MonitorStoreChecker_MonitorStoreCheckerFilterDTO.CheckIn?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    MonitorStoreChecker_MonitorStoreCheckerFilterDTO.CheckIn.GreaterEqual.Value;

            DateTime End = MonitorStoreChecker_MonitorStoreCheckerFilterDTO.CheckIn?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    MonitorStoreChecker_MonitorStoreCheckerFilterDTO.CheckIn.LessEqual.Value;

            if (End.Subtract(Start).Days > 7)
                return BadRequest(new { message = "Chỉ được phép xem tối đa trong vòng 7 ngày" });
            List<MonitorStoreChecker_MonitorStoreCheckerDTO> MonitorStoreChecker_MonitorStoreCheckerDTOs = await ListDataDW(MonitorStoreChecker_MonitorStoreCheckerFilterDTO, Start, End);
            return MonitorStoreChecker_MonitorStoreCheckerDTOs;
        }

        [Route(MonitorStoreCheckerRoute.Get), HttpPost]
        public async Task<List<MonitorStoreChecker_MonitorStoreCheckerDetailDTO>> Get([FromBody] MonitorStoreChecker_MonitorStoreCheckerDetailFilterDTO MonitorStoreChecker_MonitorStoreCheckerDetailFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            long SaleEmployeeId = MonitorStoreChecker_MonitorStoreCheckerDetailFilterDTO.SaleEmployeeId;
            long? ProvinceId = MonitorStoreChecker_MonitorStoreCheckerDetailFilterDTO.ProvinceId?.Equal;
            long? DistrictId = MonitorStoreChecker_MonitorStoreCheckerDetailFilterDTO.DistrictId?.Equal;

            DateTime Start = MonitorStoreChecker_MonitorStoreCheckerDetailFilterDTO.Date.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone);
            DateTime End = Start.AddDays(1).AddSeconds(-1);
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            var storeQuery = DWContext.Dim_Store.AsNoTracking();
            storeQuery = storeQuery.Where(x => x.DeletedAt == null);
            storeQuery = storeQuery.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            storeQuery = storeQuery.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            var StoreIdFilter = new IdFilter { In = await storeQuery.Select(x => x.StoreId).ToListWithNoLockAsync() };


            var StoreCheckingQuery = DWContext.Fact_StoreChecking.AsNoTracking();
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.CheckOutAt, DateFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.SaleEmployeeId, new IdFilter { Equal = SaleEmployeeId });
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.StoreId, StoreIdFilter);
            List<long> StoreCheckingStoreIds = await StoreCheckingQuery
                .Select(x => x.StoreId)
                .ToListWithNoLockAsync();

            var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, DateFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, new IdFilter { Equal = SaleEmployeeId });
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, new IdFilter
            { In = new List<long> { RequestStateEnum.APPROVED.Id, RequestStateEnum.PENDING.Id } });
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            List<IndirectSalesOrderDAO> IndirectSalesOrderDAOs = await IndirectSalesOrderQuery
                .Select(x => new IndirectSalesOrderDAO
                {
                    Id = x.IndirectSalesOrderId,
                    Code = x.Code,
                    BuyerStoreId = x.BuyerStoreId,
                    Total = x.Total,
                })
                .ToListWithNoLockAsync();

            var ProblemQuery = DWContext.Fact_Problem.AsNoTracking();
            ProblemQuery = ProblemQuery.Where(x => x.CreatorId, new IdFilter { Equal = SaleEmployeeId });
            ProblemQuery = ProblemQuery.Where(x => x.NoteAt, new DateFilter { GreaterEqual = Start, LessEqual = End });
            ProblemQuery = ProblemQuery.Where(x => x.StoreId, StoreIdFilter);
            var ProblemDAOs = await ProblemQuery.ToListWithNoLockAsync();


            var StoreImageQuery = DWContext.Fact_Image.AsNoTracking();
            StoreImageQuery = StoreImageQuery.Where(x => x.SaleEmployeeId, new IdFilter { Equal = SaleEmployeeId });
            StoreImageQuery = StoreImageQuery.Where(x => x.ShootingAt, new DateFilter { GreaterEqual = Start, LessEqual = End });
            StoreImageQuery = StoreImageQuery.Where(x => x.StoreId, StoreIdFilter);
            List<Fact_ImageDAO> StoreImageDAOs = await StoreImageQuery
                .ToListWithNoLockAsync();

            List<long> StoreIds = new List<long>();
            StoreIds.AddRange(StoreCheckingStoreIds);
            List<long> SalesOrder_StoreIds = IndirectSalesOrderDAOs.Select(o => o.BuyerStoreId).ToList();
            StoreIds.AddRange(SalesOrder_StoreIds);
            var Problem_StoreIds = ProblemDAOs.Select(x => x.StoreId).Distinct().ToList();
            StoreIds.AddRange(Problem_StoreIds);
            var StoreImage_StoreId = StoreImageDAOs.Select(x => x.StoreId).Distinct().ToList();
            StoreIds.AddRange(StoreImage_StoreId);

            StoreIds = StoreIds.Distinct().ToList();
            storeQuery = storeQuery.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            List<StoreDAO> StoreDAOs = await storeQuery.Select(s => new StoreDAO
            {
                Id = s.StoreId,
                Code = s.Code,
                Name = s.Name,
                Address = s.Address,
            }).ToListWithNoLockAsync();

            List<MonitorStoreChecker_MonitorStoreCheckerDetailDTO> MonitorStoreChecker_MonitorStoreCheckerDetailDTOs = new List<MonitorStoreChecker_MonitorStoreCheckerDetailDTO>();
            foreach (long StoreId in StoreIds)
            {
                var Problems = ProblemDAOs.Where(p => p.StoreId == StoreId).ToList();
                List<IndirectSalesOrderDAO> SubIndirectSalesOrderDAOs = IndirectSalesOrderDAOs.Where(i => i.BuyerStoreId == StoreId).ToList();
                List<Fact_ImageDAO> SubStoreImageDAOs = StoreImageDAOs.Where(x => x.StoreId == StoreId).ToList();

                int Max = 1;
                Max = SubIndirectSalesOrderDAOs.Count > Max ? IndirectSalesOrderDAOs.Count : Max;
                Max = Problems.Count > Max ? Problems.Count : Max;
                StoreDAO storeDAO = StoreDAOs.Where(s => s.Id == StoreId).FirstOrDefault();
                MonitorStoreChecker_MonitorStoreCheckerDetailDTO MonitorStoreChecker_MonitorStoreCheckerDetailDTO = new MonitorStoreChecker_MonitorStoreCheckerDetailDTO
                {
                    StoreId = storeDAO.Id,
                    StoreCode = storeDAO.Code,
                    StoreName = storeDAO.Name,
                    ImageCounter = SubStoreImageDAOs.Count(),
                    Infoes = new List<MonitorStoreChecker_MonitorStoreCheckerDetailInfoDTO>(),
                };
                MonitorStoreChecker_MonitorStoreCheckerDetailDTOs.Add(MonitorStoreChecker_MonitorStoreCheckerDetailDTO);
                for (int i = 0; i < Max; i++)
                {
                    MonitorStoreChecker_MonitorStoreCheckerDetailInfoDTO Info = new MonitorStoreChecker_MonitorStoreCheckerDetailInfoDTO();
                    MonitorStoreChecker_MonitorStoreCheckerDetailDTO.Infoes.Add(Info);

                    if (SubStoreImageDAOs.Count > i)
                    {
                        Info.ImagePath = SubStoreImageDAOs[i].Url;
                    }

                    if (SubIndirectSalesOrderDAOs.Count > i)
                    {
                        Info.IndirectSalesOrderCode = SubIndirectSalesOrderDAOs[i].Code;
                        Info.Sales = SubIndirectSalesOrderDAOs[i].Total;
                    }
                    if (Problems.Count > i)
                    {
                        Info.ProblemCode = Problems[i].Code;
                        Info.ProblemId = Problems[i].ProblemId;
                    }
                }
            }
            return MonitorStoreChecker_MonitorStoreCheckerDetailDTOs;
        }

        [Route(MonitorStoreCheckerRoute.ListImage), HttpPost]
        public async Task<List<MonitorStoreChecker_StoreCheckingImageMappingDTO>> ListImage([FromBody] MonitorStoreChecker_StoreCheckingDTO MonitorStoreChecker_StoreCheckingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            DateTime Start = MonitorStoreChecker_StoreCheckingDTO.Date.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone);
            DateTime End = Start.AddDays(1);

            var query = from si in DataContext.StoreImage
                        where si.StoreId == MonitorStoreChecker_StoreCheckingDTO.StoreId &&
                        si.SaleEmployeeId.HasValue && si.SaleEmployeeId.Value == MonitorStoreChecker_StoreCheckingDTO.SaleEmployeeId &&
                        Start <= si.ShootingAt && si.ShootingAt < End
                        select new MonitorStoreChecker_StoreCheckingImageMappingDTO
                        {
                            AlbumId = si.AlbumId,
                            ImageId = si.ImageId,
                            SaleEmployeeId = si.SaleEmployeeId.Value,
                            ShootingAt = si.ShootingAt,
                            StoreId = si.StoreId,
                            Distance = si.Distance,
                            Album = new MonitorStoreChecker_AlbumDTO
                            {
                                Id = si.AlbumId,
                                Name = si.AlbumName
                            },
                            Image = new MonitorStoreChecker_ImageDTO
                            {
                                Url = si.Url
                            },
                            SaleEmployee = new MonitorStoreChecker_AppUserDTO
                            {
                                Id = si.SaleEmployeeId.Value,
                                DisplayName = si.DisplayName
                            },
                            Store = new MonitorStoreChecker_StoreDTO
                            {
                                Id = si.StoreId,
                                Address = si.StoreAddress,
                                Name = si.StoreName
                            }
                        };

            return await query.ToListWithNoLockAsync();
        }

        [Route(MonitorStoreCheckerRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] MonitorStoreChecker_MonitorStoreCheckerFilterDTO MonitorStoreChecker_MonitorStoreCheckerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = MonitorStoreChecker_MonitorStoreCheckerFilterDTO.CheckIn?.GreaterEqual == null ?
               LocalStartDay(CurrentContext) :
               MonitorStoreChecker_MonitorStoreCheckerFilterDTO.CheckIn.GreaterEqual.Value;

            DateTime End = MonitorStoreChecker_MonitorStoreCheckerFilterDTO.CheckIn?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    MonitorStoreChecker_MonitorStoreCheckerFilterDTO.CheckIn.LessEqual.Value;

            MonitorStoreChecker_MonitorStoreCheckerFilterDTO.Skip = 0;
            MonitorStoreChecker_MonitorStoreCheckerFilterDTO.Take = int.MaxValue;
            List<MonitorStoreChecker_MonitorStoreCheckerDTO> MonitorStoreChecker_MonitorStoreCheckerDTOs = await ListDataDW(MonitorStoreChecker_MonitorStoreCheckerFilterDTO, Start, End);
            long stt = 1;
            foreach (MonitorStoreChecker_MonitorStoreCheckerDTO MonitorStoreChecker_MonitorStoreCheckerDTO in MonitorStoreChecker_MonitorStoreCheckerDTOs)
            {
                foreach (var SaleEmployee in MonitorStoreChecker_MonitorStoreCheckerDTO.SaleEmployees)
                {
                    foreach (var storeChecking in SaleEmployee.StoreCheckings)
                    {
                        storeChecking.STT = stt;
                        stt++;
                        storeChecking.Date = storeChecking.Date.AddHours(CurrentContext.TimeZone);
                    }
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

            string path = "Templates/Monitor_Store_Checker_Report.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.MonitorStoreCheckers = MonitorStoreChecker_MonitorStoreCheckerDTOs;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "MonitorStoreChecker.xlsx");
        }

        private async Task<List<MonitorStoreChecker_MonitorStoreCheckerDTO>> ListDataDW(
            MonitorStoreChecker_MonitorStoreCheckerFilterDTO MonitorStoreChecker_MonitorStoreCheckerFilterDTO,
            DateTime Start, DateTime End)
        {
            long? SaleEmployeeId = MonitorStoreChecker_MonitorStoreCheckerFilterDTO.AppUserId?.Equal;
            long? ProvinceId = MonitorStoreChecker_MonitorStoreCheckerFilterDTO.ProvinceId?.Equal;
            long? DistrictId = MonitorStoreChecker_MonitorStoreCheckerFilterDTO.DistrictId?.Equal;
            DateFilter DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (MonitorStoreChecker_MonitorStoreCheckerFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == MonitorStoreChecker_MonitorStoreCheckerFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
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
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            var AppUsers = await AppUserService.List(AppUserFilter);
            var AppUserIds = AppUsers.Select(x => x.Id).ToList();
            if (SaleEmployeeId != null)
            {
                AppUserIds = AppUserIds.Intersect(new List<long> { SaleEmployeeId.Value }).ToList();
            }

            var storeQuery = DWContext.Dim_Store.AsNoTracking();
            storeQuery = storeQuery.Where(x => x.DeletedAt == null);
            storeQuery = storeQuery.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            storeQuery = storeQuery.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            var StoreIdFilter = new IdFilter { In = await storeQuery.Select(x => x.StoreId).ToListWithNoLockAsync() };

            var storeCheckingQuery = DWContext.Fact_StoreChecking.AsNoTracking();
            storeCheckingQuery = storeCheckingQuery.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            storeCheckingQuery = storeCheckingQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            storeCheckingQuery = storeCheckingQuery.Where(x => x.StoreId, StoreIdFilter);
            storeCheckingQuery = storeCheckingQuery.Where(x => x.CheckOutAt, DateFilter);

            var storeImageQuery = DWContext.Fact_Image.AsNoTracking();
            storeImageQuery = storeImageQuery.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            storeImageQuery = storeImageQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            storeImageQuery = storeImageQuery.Where(x => x.StoreId, StoreIdFilter);
            storeImageQuery = storeImageQuery.Where(x => x.ShootingAt, DateFilter);

            var salesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            salesOrderQuery = salesOrderQuery.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            salesOrderQuery = salesOrderQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            salesOrderQuery = salesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            salesOrderQuery = salesOrderQuery.Where(x => x.OrderDate, DateFilter);
            salesOrderQuery = salesOrderQuery.Where(x => x.RequestStateId, new IdFilter { In = new List<long> { RequestStateEnum.APPROVED.Id, RequestStateEnum.PENDING.Id } });

            var storeUncheckingQuery = DWContext.Fact_StoreUnchecking.AsNoTracking();
            storeUncheckingQuery = storeUncheckingQuery.Where(x => x.AppUserId, new IdFilter { In = AppUserIds });
            storeUncheckingQuery = storeUncheckingQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            storeUncheckingQuery = storeUncheckingQuery.Where(x => x.StoreId, StoreIdFilter);
            storeUncheckingQuery = storeUncheckingQuery.Where(x => x.Date, DateFilter);

            var appsuerstore_query = DataContext.AppUserStoreMapping.AsNoTracking();
            var AppUserStoreMappings = await appsuerstore_query.ToListWithNoLockAsync();

            var Ids1 = await storeCheckingQuery.Select(x => new
            {
                OrganizationId = x.OrganizationId,
                SalesEmployeeId = x.SaleEmployeeId,
            }).ToListWithNoLockAsync();
            var Ids2 = await storeImageQuery.Select(x => new
            {
                OrganizationId = x.OrganizationId.Value,
                SalesEmployeeId = x.SaleEmployeeId.Value,
            }).ToListWithNoLockAsync();
            var Ids3 = await salesOrderQuery.Select(x => new
            {
                OrganizationId = x.OrganizationId,
                SalesEmployeeId = x.SaleEmployeeId,
            }).ToListWithNoLockAsync();
            var Ids4 = await storeUncheckingQuery.Select(x => new
            {
                OrganizationId = x.OrganizationId,
                SalesEmployeeId = x.AppUserId,
            }).ToListWithNoLockAsync();
            var Ids = Ids1;
            Ids.AddRange(Ids2);
            Ids.AddRange(Ids3);
            Ids.AddRange(Ids4);
            Ids = Ids.Distinct()
                .OrderBy(x => x.OrganizationId)
                .Skip(MonitorStoreChecker_MonitorStoreCheckerFilterDTO.Skip)
                .Take(MonitorStoreChecker_MonitorStoreCheckerFilterDTO.Take)
                .ToList();

            AppUserIds = Ids.Select(x => x.SalesEmployeeId).Distinct().ToList();
            List<AppUserDAO> AppUserDAOs = await DataContext.AppUser.Where(au =>
               AppUserIds.Contains(au.Id) &&
               au.DeletedAt == null &&
               OrganizationIds.Contains(au.OrganizationId) &&
               (SaleEmployeeId == null || au.Id == SaleEmployeeId.Value))
                .Select(x => new AppUserDAO
                {
                    Id = x.Id,
                    Username = x.Username,
                    DisplayName = x.DisplayName,
                    OrganizationId = x.OrganizationId,
                }).ToListWithNoLockAsync();

            OrganizationIds = Ids.Select(x => x.OrganizationId).Distinct().ToList();
            var Organizations = await DataContext.Organization
                .Where(x => OrganizationIds.Contains(x.Id))
                .OrderBy(x => x.Id)
                .Select(x => new OrganizationDAO
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListWithNoLockAsync();

            List<ERouteContentDAO> ERouteContentDAOs = await DataContext.ERouteContent
              .Where(ec => ec.ERoute.RealStartDate <= End &&
                    (ec.ERoute.EndDate == null || ec.ERoute.EndDate.Value >= Start) &&
                    AppUserIds.Contains(ec.ERoute.SaleEmployeeId))
              .Where(x => x.ERoute.StatusId == StatusEnum.ACTIVE.Id &&
              x.ERoute.RequestStateId == RequestStateEnum.APPROVED.Id)
              .Where(x => x.Store.ProvinceId, new IdFilter { Equal = ProvinceId })
              .Where(x => x.Store.DistrictId, new IdFilter { Equal = DistrictId })
              .Include(ec => ec.ERouteContentDays)
              .Include(ec => ec.ERoute)
              .ToListWithNoLockAsync();

            var StoreCheckingDAOs = await storeCheckingQuery.ToListWithNoLockAsync();
            List<Fact_ImageDAO> StoreImageDAOs = await storeImageQuery.ToListWithNoLockAsync();
            var IndirectSalesOrderDAOs = await salesOrderQuery.ToListWithNoLockAsync();
            var StoreUncheckingDAOs = await storeUncheckingQuery.ToListWithNoLockAsync();
            var Stores = await storeQuery.Select(x => new StoreDAO
            {
                Id = x.StoreId,
                StoreStatusId = x.StoreStatusId,
                StatusId = x.StatusId,
                OrganizationId = x.OrganizationId,
            }).ToListWithNoLockAsync();

            List<MonitorStoreChecker_MonitorStoreCheckerDTO> MonitorStoreChecker_MonitorStoreCheckerDTOs = new List<MonitorStoreChecker_MonitorStoreCheckerDTO>();
            foreach (var Organization in Organizations)
            {
                MonitorStoreChecker_MonitorStoreCheckerDTO MonitorStoreChecker_MonitorStoreCheckerDTO = new MonitorStoreChecker_MonitorStoreCheckerDTO()
                {
                    OrganizationId = Organization.Id,
                    OrganizationName = Organization.Name,
                    SaleEmployees = new List<MonitorStoreChecker_SaleEmployeeDTO>()
                };
                MonitorStoreChecker_MonitorStoreCheckerDTO.SaleEmployees = Ids
                    .Where(x => x.OrganizationId == Organization.Id)
                    .Select(x => new MonitorStoreChecker_SaleEmployeeDTO
                    {
                        SaleEmployeeId = x.SalesEmployeeId
                    }).ToList();
                MonitorStoreChecker_MonitorStoreCheckerDTOs.Add(MonitorStoreChecker_MonitorStoreCheckerDTO);
            }

            foreach (var MonitorStoreChecker_MonitorStoreCheckerDTO in MonitorStoreChecker_MonitorStoreCheckerDTOs)
            {
                foreach(var SaleEmployee in MonitorStoreChecker_MonitorStoreCheckerDTO.SaleEmployees)
                {
                    var appUser = AppUserDAOs.Where(x => x.Id == SaleEmployee.SaleEmployeeId).FirstOrDefault();
                    if (appUser != null)
                    {
                        SaleEmployee.SaleEmployeeId = appUser.Id;
                        SaleEmployee.Username = appUser.Username;
                        SaleEmployee.DisplayName = appUser.DisplayName;
                    }
                }

                foreach (var SaleEmployee in MonitorStoreChecker_MonitorStoreCheckerDTO.SaleEmployees)
                {
                    SaleEmployee.StoreCheckings = new List<MonitorStoreChecker_StoreCheckingDTO>();
                    for (DateTime i = Start.AddHours(CurrentContext.TimeZone); i < End.AddHours(CurrentContext.TimeZone); i = i.AddDays(1))
                    {
                        MonitorStoreChecker_StoreCheckingDTO StoreCheckerMonitor_StoreCheckingDTO = new MonitorStoreChecker_StoreCheckingDTO();
                        StoreCheckerMonitor_StoreCheckingDTO.SaleEmployeeId = SaleEmployee.SaleEmployeeId;
                        StoreCheckerMonitor_StoreCheckingDTO.Date = i;
                        StoreCheckerMonitor_StoreCheckingDTO.Image = new HashSet<long>();
                        StoreCheckerMonitor_StoreCheckingDTO.External = new HashSet<long>();
                        StoreCheckerMonitor_StoreCheckingDTO.Internal = new HashSet<long>();
                        SaleEmployee.StoreCheckings.Add(StoreCheckerMonitor_StoreCheckingDTO);
                    }
                }

                foreach (var SaleEmployee in MonitorStoreChecker_MonitorStoreCheckerDTO.SaleEmployees)
                {
                    for (DateTime i = Start.AddHours(CurrentContext.TimeZone); i <= End.AddHours(CurrentContext.TimeZone); i = i.AddDays(1))
                    {
                        MonitorStoreChecker_StoreCheckingDTO MonitorStoreChecker_StoreCheckingDTO = SaleEmployee.StoreCheckings
                            .Where(s => s.Date == i).FirstOrDefault();
                        MonitorStoreChecker_StoreCheckingDTO.SalesOrderCounter = IndirectSalesOrderDAOs
                            .Where(o => i <= o.OrderDate.AddHours(CurrentContext.TimeZone) && o.OrderDate.AddHours(CurrentContext.TimeZone) < i.AddDays(1) &&
                            o.SaleEmployeeId == SaleEmployee.SaleEmployeeId)
                            .Count();
                        MonitorStoreChecker_StoreCheckingDTO.RevenueCounter = IndirectSalesOrderDAOs
                            .Where(o => i <= o.OrderDate.AddHours(CurrentContext.TimeZone) && o.OrderDate.AddHours(CurrentContext.TimeZone) < i.AddDays(1) &&
                            o.SaleEmployeeId == SaleEmployee.SaleEmployeeId)
                            .Select(o => o.Total).DefaultIfEmpty(0).Sum();
                        MonitorStoreChecker_StoreCheckingDTO.Unchecking = StoreUncheckingDAOs
                        .Where(x => i <= x.Date.AddHours(CurrentContext.TimeZone) && x.Date.AddHours(CurrentContext.TimeZone) < i.AddDays(1) &&
                        x.AppUserId == SaleEmployee.SaleEmployeeId)
                        .Count();

                        MonitorStoreChecker_StoreCheckingDTO.PlanCounter = CountPlan(i.AddHours(0 - CurrentContext.TimeZone), SaleEmployee.SaleEmployeeId, MonitorStoreChecker_MonitorStoreCheckerDTO.OrganizationId, ERouteContentDAOs, AppUserStoreMappings, Stores);

                        var ListChecked = StoreCheckingDAOs
                               .Where(s =>
                                   s.SaleEmployeeId == SaleEmployee.SaleEmployeeId &&
                                   i <= s.CheckOutAt.Value.AddHours(CurrentContext.TimeZone) && s.CheckOutAt.Value.AddHours(CurrentContext.TimeZone) < i.AddDays(1)
                               ).ToList();
                        foreach (var Checked in ListChecked)
                        {
                            if (Checked.Planned)
                                MonitorStoreChecker_StoreCheckingDTO.Internal.Add(Checked.StoreId);
                            else
                                MonitorStoreChecker_StoreCheckingDTO.External.Add(Checked.StoreId);
                        }

                        var ImageIds = StoreImageDAOs
                            .Where(x => x.SaleEmployeeId.HasValue && x.SaleEmployeeId == SaleEmployee.SaleEmployeeId)
                            .Where(x => x.ShootingAt.AddHours(CurrentContext.TimeZone).Date == i.Date)
                            .Select(x => x.ImageId)
                            .ToList();
                        ImageIds.ForEach(x => MonitorStoreChecker_StoreCheckingDTO.Image.Add(x));
                    }
                }

                foreach (var SaleEmployee in MonitorStoreChecker_MonitorStoreCheckerDTO.SaleEmployees)
                {
                    foreach (var StoreChecking in SaleEmployee.StoreCheckings)
                    {
                        StoreChecking.Date = StoreChecking.Date.AddHours(CurrentContext.TimeZone);
                    }
                    if (MonitorStoreChecker_MonitorStoreCheckerFilterDTO.Checking?.Equal != null)
                    {
                        if (MonitorStoreChecker_MonitorStoreCheckerFilterDTO.Checking.Equal.Value == 0)
                            SaleEmployee.StoreCheckings = SaleEmployee.StoreCheckings.Where(sc => sc.InternalCounter + sc.ExternalCounter == 0).ToList();
                        if (MonitorStoreChecker_MonitorStoreCheckerFilterDTO.Checking.Equal.Value == 1)
                            SaleEmployee.StoreCheckings = SaleEmployee.StoreCheckings.Where(sc => sc.InternalCounter + sc.ExternalCounter > 0).ToList();
                    }
                    if (MonitorStoreChecker_MonitorStoreCheckerFilterDTO.Image?.Equal != null)
                    {
                        if (MonitorStoreChecker_MonitorStoreCheckerFilterDTO.Image.Equal.Value == 0)
                            SaleEmployee.StoreCheckings = SaleEmployee.StoreCheckings.Where(sc => sc.Image.Count == 0).ToList();
                        if (MonitorStoreChecker_MonitorStoreCheckerFilterDTO.Image.Equal.Value == 1)
                            SaleEmployee.StoreCheckings = SaleEmployee.StoreCheckings.Where(sc => sc.Image.Count > 0).ToList();
                    }
                    if (MonitorStoreChecker_MonitorStoreCheckerFilterDTO.SalesOrder?.Equal != null)
                    {
                        if (MonitorStoreChecker_MonitorStoreCheckerFilterDTO.SalesOrder.Equal.Value == 0)
                            SaleEmployee.StoreCheckings = SaleEmployee.StoreCheckings.Where(sc => sc.SalesOrderCounter == 0).ToList();
                        if (MonitorStoreChecker_MonitorStoreCheckerFilterDTO.SalesOrder.Equal.Value == 1)
                            SaleEmployee.StoreCheckings = SaleEmployee.StoreCheckings.Where(sc => sc.SalesOrderCounter > 0).ToList();
                    }
                }

                foreach (var SaleEmployee in MonitorStoreChecker_MonitorStoreCheckerDTO.SaleEmployees)
                {
                    SaleEmployee.StoreCheckings = SaleEmployee.StoreCheckings.Where(x => x.HasValue).ToList();
                }
            }

            return MonitorStoreChecker_MonitorStoreCheckerDTOs.Where(x => x.SaleEmployees.Any()).ToList();
        }

    }
}
