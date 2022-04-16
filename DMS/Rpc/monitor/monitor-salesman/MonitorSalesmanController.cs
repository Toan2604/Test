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
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_salesman
{
    public class MonitorSalesmanController : MonitorController
    {
        private DataContext DataContext;
        private DWContext DWContext;
        private IAppUserService AppUserService;
        private IOrganizationService OrganizationService;
        private ICurrentContext CurrentContext;
        private IProvinceService ProvinceService;
        private IDistrictService DistrictService;
        public MonitorSalesmanController(DataContext DataContext,
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
        [Route(MonitorSalesmanRoute.FilterListAppUser), HttpPost]
        public async Task<List<MonitorSalesman_AppUserDTO>> FilterListAppUser([FromBody] SalesmanMonitor_AppUserFilterDTO SalesmanMonitor_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.Id = SalesmanMonitor_AppUserFilterDTO.Id;
            AppUserFilter.Username = SalesmanMonitor_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = SalesmanMonitor_AppUserFilterDTO.DisplayName;
            AppUserFilter.OrganizationId = SalesmanMonitor_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<MonitorSalesman_AppUserDTO> SalesmanMonitor_AppUserDTOs = AppUsers
                .Select(x => new MonitorSalesman_AppUserDTO(x)).ToList();
            return SalesmanMonitor_AppUserDTOs;
        }

        [Route(MonitorSalesmanRoute.FilterListOrganization), HttpPost]
        public async Task<List<MonitorSalesman_OrganizationDTO>> FilterListOrganization()
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
            List<MonitorSalesman_OrganizationDTO> SalesmanMonitor_OrganizationDTOs = Organizations
                .Select(x => new MonitorSalesman_OrganizationDTO(x)).ToList();
            return SalesmanMonitor_OrganizationDTOs;
        }

        [Route(MonitorSalesmanRoute.FilterListProvince), HttpPost]
        public async Task<List<MonitorSalesman_ProvinceDTO>> FilterListProvince([FromBody] MonitorSalesman_ProvinceFilterDTO MonitorSalesman_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = MonitorSalesman_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = MonitorSalesman_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = MonitorSalesman_ProvinceFilterDTO.StatusId;

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<MonitorSalesman_ProvinceDTO> MonitorSalesman_ProvinceDTOs = Provinces
                .Select(x => new MonitorSalesman_ProvinceDTO(x)).ToList();
            return MonitorSalesman_ProvinceDTOs;
        }

        [Route(MonitorSalesmanRoute.FilterListDistrict), HttpPost]
        public async Task<List<MonitorSalesman_DistrictDTO>> FilterListDistrict([FromBody] MonitorSalesman_DistrictFilterDTO MonitorSalesman_DistrictFilterDTO)
        {
            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = MonitorSalesman_DistrictFilterDTO.Id;
            DistrictFilter.Name = MonitorSalesman_DistrictFilterDTO.Name;
            DistrictFilter.Priority = MonitorSalesman_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = MonitorSalesman_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = MonitorSalesman_DistrictFilterDTO.StatusId;

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<MonitorSalesman_DistrictDTO> MonitorSalesman_DistrictDTOs = Districts
                .Select(x => new MonitorSalesman_DistrictDTO(x)).ToList();
            return MonitorSalesman_DistrictDTOs;
        }


        [Route(MonitorSalesmanRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] MonitorSalesman_MonitorSalesmanFilterDTO MonitorSalesman_MonitorSalesmanFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = MonitorSalesman_MonitorSalesmanFilterDTO.CheckIn?.GreaterEqual == null ?
                             LocalStartDay(CurrentContext) :
                             MonitorSalesman_MonitorSalesmanFilterDTO.CheckIn.GreaterEqual.Value;

            DateTime End = MonitorSalesman_MonitorSalesmanFilterDTO.CheckIn?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    MonitorSalesman_MonitorSalesmanFilterDTO.CheckIn.LessEqual.Value;

            long? OrganizationId = MonitorSalesman_MonitorSalesmanFilterDTO.OrganizationId?.Equal;
            long? SaleEmployeeId = MonitorSalesman_MonitorSalesmanFilterDTO.AppUserId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (MonitorSalesman_MonitorSalesmanFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == MonitorSalesman_MonitorSalesmanFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();
            List<long> AppUserIds = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            //var storeCheckingQuery = from sc in DataContext.StoreChecking
            //                         join au in DataContext.AppUser on sc.SaleEmployeeId equals au.Id
            //                         where AppUserIds.Contains(sc.SaleEmployeeId) &&
            //                         OrganizationIds.Contains(sc.OrganizationId) &&
            //                         (SaleEmployeeId == null || au.Id == SaleEmployeeId.Value) &&
            //                         au.DeletedAt == null &&
            //                         sc.CheckOutAt.HasValue && Start <= sc.CheckOutAt && sc.CheckOutAt <= End
            //                         select sc;

            //var storeImageQuery = from si in DataContext.StoreImage
            //                      join au in DataContext.AppUser on si.SaleEmployeeId equals au.Id
            //                      where si.SaleEmployeeId.HasValue && AppUserIds.Contains(si.SaleEmployeeId.Value) &&
            //                      OrganizationIds.Contains(si.OrganizationId) &&
            //                      (SaleEmployeeId == null || au.Id == SaleEmployeeId.Value) &&
            //                      au.DeletedAt == null &&
            //                      Start <= si.ShootingAt && si.ShootingAt <= End
            //                      select si;

            //var salesOrderQuery = from i in DataContext.IndirectSalesOrder
            //                      join au in DataContext.AppUser on i.SaleEmployeeId equals au.Id
            //                      where AppUserIds.Contains(i.SaleEmployeeId) &&
            //                      OrganizationIds.Contains(i.OrganizationId) &&
            //                      (SaleEmployeeId == null || au.Id == SaleEmployeeId.Value) &&
            //                      au.DeletedAt == null &&
            //                      Start <= i.OrderDate && i.OrderDate <= End
            //                      select new IndirectSalesOrderDAO
            //                      {
            //                          Id = i.Id,
            //                          OrganizationId = i.OrganizationId,
            //                          SaleEmployeeId = i.SaleEmployeeId,
            //                          BuyerStoreId = i.BuyerStoreId,
            //                          Total = i.Total
            //                      };

            //var Ids1 = storeCheckingQuery.ToListWithNoLockAsync();
            //var Ids2 = storeImageQuery.ToListWithNoLockAsync();
            //var Ids3 = salesOrderQuery.ToListWithNoLockAsync();
            int count = await DataContext.AppUser.Where(au =>
                au.DeletedAt == null &&
                AppUserIds.Contains(au.Id) &&
                OrganizationIds.Contains(au.OrganizationId) &&
                (SaleEmployeeId == null || au.Id == SaleEmployeeId.Value)
            ).CountWithNoLockAsync();
            return count;
        }


        [Route(MonitorSalesmanRoute.List), HttpPost]
        public async Task<ActionResult<List<MonitorSalesman_MonitorSalesmanDTO>>> List([FromBody] MonitorSalesman_MonitorSalesmanFilterDTO MonitorSalesman_MonitorSalesmanFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = MonitorSalesman_MonitorSalesmanFilterDTO.CheckIn?.GreaterEqual == null ?
                             LocalStartDay(CurrentContext) :
                             MonitorSalesman_MonitorSalesmanFilterDTO.CheckIn.GreaterEqual.Value;

            DateTime End = MonitorSalesman_MonitorSalesmanFilterDTO.CheckIn?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    MonitorSalesman_MonitorSalesmanFilterDTO.CheckIn.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return BadRequest(new { message = "Chỉ được phép xem tối đa trong vòng 31 ngày" });
            MonitorSalesman_MonitorSalesmanFilterDTO.Skip = 0;
            MonitorSalesman_MonitorSalesmanFilterDTO.Take = int.MaxValue;
            List<MonitorSalesman_MonitorSalesmanDTO> MonitorSalesman_MonitorSalesmanDTOs = await ListDataDW(MonitorSalesman_MonitorSalesmanFilterDTO, Start, End);
            return MonitorSalesman_MonitorSalesmanDTOs;
        }

        [Route(MonitorSalesmanRoute.Get), HttpPost]
        public async Task<List<MonitorSalesman_MonitorSalesmanDetailDTO>> Get([FromBody] MonitorSalesman_MonitorSalesmanDetailFilterDTO MonitorSalesman_MonitorSalesmanDetailFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            long SaleEmployeeId = MonitorSalesman_MonitorSalesmanDetailFilterDTO.SaleEmployeeId;
            DateTime Start = MonitorSalesman_MonitorSalesmanDetailFilterDTO.Date.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone);
            DateTime End = Start.AddDays(1).AddSeconds(-1);

            var StoreCheckingQuery = DWContext.Fact_StoreChecking.AsNoTracking();
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.CheckOutAt, new DateFilter { GreaterEqual = Start, LessEqual = End });
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.SaleEmployeeId, new IdFilter { Equal = SaleEmployeeId });
            List<long> StoreCheckingStoreIds = await StoreCheckingQuery.Select(x => x.StoreId).ToListWithNoLockAsync();

            var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, new IdFilter { Equal = SaleEmployeeId });
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, new IdFilter { In = new List<long> { RequestStateEnum.PENDING.Id, RequestStateEnum.APPROVED.Id } });
            List<Fact_IndirectSalesOrderDAO> IndirectSalesOrderDAOs = await IndirectSalesOrderQuery.Select(x => new Fact_IndirectSalesOrderDAO
            {
                IndirectSalesOrderId = x.IndirectSalesOrderId,
                Code = x.Code,
                BuyerStoreId = x.BuyerStoreId,
                Total = x.Total,
            }).ToListWithNoLockAsync();

            List<Fact_ProblemDAO> ProblemDAOs = await DWContext.Fact_Problem.AsNoTracking()
                .Where(x => x.CreatorId, new IdFilter { Equal = SaleEmployeeId })
                .Where(x => x.NoteAt, new DateFilter { GreaterEqual = Start, LessEqual = End })
                .ToListWithNoLockAsync();

            var StoreImageDAOs = await DWContext.Fact_Image.AsNoTracking()
                .Where(x => x.SaleEmployeeId, new IdFilter { Equal = SaleEmployeeId })
                .Where(x => x.ShootingAt, new DateFilter { GreaterEqual = Start, LessEqual = End })
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
            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
                     .BulkInsertValuesIntoTempTableAsync<long>(StoreIds);
            var query2 = from s in DataContext.Store
                         join tt in tempTableQuery.Query on s.Id equals tt.Column1
                         select new StoreDAO
                         {
                             Id = s.Id,
                             Code = s.Code,
                             Name = s.Name,
                             Address = s.Address,
                         };
            List<StoreDAO> StoreDAOs = await query2.ToListWithNoLockAsync();

            List<MonitorSalesman_MonitorSalesmanDetailDTO> MonitorStoreChecker_MonitorStoreCheckerDetailDTOs = new List<MonitorSalesman_MonitorSalesmanDetailDTO>();
            foreach (long StoreId in StoreIds)
            {
                List<Fact_ProblemDAO> Problems = ProblemDAOs.Where(p => p.StoreId == StoreId).ToList();
                List<Fact_IndirectSalesOrderDAO> SubIndirectSalesOrderDAOs = IndirectSalesOrderDAOs.Where(i => i.BuyerStoreId == StoreId).ToList();
                List<Fact_ImageDAO> SubStoreImageDAOs = StoreImageDAOs.Where(x => x.StoreId == StoreId).ToList();

                int Max = 1;
                Max = SubIndirectSalesOrderDAOs.Count > Max ? IndirectSalesOrderDAOs.Count : Max;
                Max = Problems.Count > Max ? Problems.Count : Max;
                StoreDAO storeDAO = StoreDAOs.Where(s => s.Id == StoreId).FirstOrDefault();
                MonitorSalesman_MonitorSalesmanDetailDTO MonitorStoreChecker_MonitorStoreCheckerDetailDTO = new MonitorSalesman_MonitorSalesmanDetailDTO
                {
                    StoreId = storeDAO.Id,
                    StoreCode = storeDAO.Code,
                    StoreName = storeDAO.Name,
                    ImageCounter = SubStoreImageDAOs.Count(),
                    Infoes = new List<MonitorSalesman_MonitorSalesmanDetailInfoDTO>(),
                };
                MonitorStoreChecker_MonitorStoreCheckerDetailDTOs.Add(MonitorStoreChecker_MonitorStoreCheckerDetailDTO);
                for (int i = 0; i < Max; i++)
                {
                    MonitorSalesman_MonitorSalesmanDetailInfoDTO Info = new MonitorSalesman_MonitorSalesmanDetailInfoDTO();
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

        [Route(MonitorSalesmanRoute.ListImage), HttpPost]
        public async Task<List<MonitorSalesman_StoreImageDTO>> ListImage([FromBody] MonitorSalesman_MonitorSalesmanDetailFilterDTO MonitorSalesman_MonitorSalesmanDetailFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            DateTime Start = MonitorSalesman_MonitorSalesmanDetailFilterDTO.Date.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone);
            DateTime End = Start.AddDays(1);

            var query = from si in DataContext.StoreImage
                        where si.StoreId == MonitorSalesman_MonitorSalesmanDetailFilterDTO.StoreId &&
                        si.SaleEmployeeId.HasValue && si.SaleEmployeeId.Value == MonitorSalesman_MonitorSalesmanDetailFilterDTO.SaleEmployeeId &&
                        Start <= si.ShootingAt && si.ShootingAt < End
                        select new MonitorSalesman_StoreImageDTO
                        {
                            AlbumId = si.AlbumId,
                            ImageId = si.ImageId,
                            SaleEmployeeId = si.SaleEmployeeId.Value,
                            ShootingAt = si.ShootingAt,
                            StoreId = si.StoreId,
                            Distance = si.Distance,
                            Album = new MonitorSalesman_AlbumDTO
                            {
                                Id = si.AlbumId,
                                Name = si.AlbumName
                            },
                            Image = new MonitorSalesman_ImageDTO
                            {
                                Url = si.Url
                            },
                            SaleEmployee = new MonitorSalesman_AppUserDTO
                            {
                                Id = si.SaleEmployeeId.Value,
                                DisplayName = si.DisplayName
                            },
                            Store = new MonitorSalesman_StoreDTO
                            {
                                Id = si.StoreId,
                                Address = si.StoreAddress,
                                Name = si.StoreName
                            }
                        };

            return await query.ToListWithNoLockAsync();
        }

        [Route(MonitorSalesmanRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] MonitorSalesman_MonitorSalesmanFilterDTO MonitorSalesman_MonitorSalesmanFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            DateTime Start = MonitorSalesman_MonitorSalesmanFilterDTO.CheckIn?.GreaterEqual == null ?
                             LocalStartDay(CurrentContext) :
                             MonitorSalesman_MonitorSalesmanFilterDTO.CheckIn.GreaterEqual.Value;

            DateTime End = MonitorSalesman_MonitorSalesmanFilterDTO.CheckIn?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    MonitorSalesman_MonitorSalesmanFilterDTO.CheckIn.LessEqual.Value;

            MonitorSalesman_MonitorSalesmanFilterDTO.Skip = 0;
            MonitorSalesman_MonitorSalesmanFilterDTO.Take = int.MaxValue;
            List<MonitorSalesman_MonitorSalesmanDTO> MonitorSalesman_MonitorSalesmanDTOs = await ListDataDW(MonitorSalesman_MonitorSalesmanFilterDTO, Start, End);
            int stt = 1;
            foreach (MonitorSalesman_MonitorSalesmanDTO MonitorSalesman_MonitorSalesmanDTO in MonitorSalesman_MonitorSalesmanDTOs)
            {
                foreach (var SaleEmployee in MonitorSalesman_MonitorSalesmanDTO.SaleEmployees)
                {
                    SaleEmployee.STT = stt;
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

            string path = "Templates/Monitor_Salesman_Report.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.MonitorSalesmans = MonitorSalesman_MonitorSalesmanDTOs;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "Monitor_Salesman_Report.xlsx");
        }

        [Route(MonitorSalesmanRoute.ExportUnchecking), HttpPost]
        public async Task<ActionResult> ExportUnchecking([FromBody] MonitorSalesman_MonitorSalesmanFilterDTO MonitorSalesman_MonitorSalesmanFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            DateTime Now = StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone);

            long? OrganizationId = MonitorSalesman_MonitorSalesmanFilterDTO.OrganizationId?.Equal;
            long? SaleEmployeeId = MonitorSalesman_MonitorSalesmanFilterDTO.AppUserId?.Equal;
            long? ProvinceId = MonitorSalesman_MonitorSalesmanFilterDTO.ProvinceId?.Equal;
            long? DistrictId = MonitorSalesman_MonitorSalesmanFilterDTO.DistrictId?.Equal;
            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (MonitorSalesman_MonitorSalesmanFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == MonitorSalesman_MonitorSalesmanFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();
            List<long> AppUserIds = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<ERouteContentDAO> ERouteContentDAOs = await DataContext.ERouteContent
                .Where(ec => (!ec.ERoute.EndDate.HasValue || Start <= ec.ERoute.EndDate) && ec.ERoute.StartDate <= End)
                .Where(x => x.ERoute.DeletedAt == null && x.ERoute.StatusId == StatusEnum.ACTIVE.Id)
                .Where(x => x.ERoute.RequestStateId == RequestStateEnum.APPROVED.Id)
                .Include(ec => ec.ERoute)
                .Include(ec => ec.ERouteContentDays)
                .ToListWithNoLockAsync();
            ERouteContentDAOs = ERouteContentDAOs.Where(x => OrganizationIds.Contains(x.ERoute.OrganizationId))
                .Where(x => AppUserIds.Contains(x.ERoute.SaleEmployeeId))
                .Where(x => SaleEmployeeId.HasValue == false || x.ERoute.SaleEmployeeId == SaleEmployeeId)
                .ToList();
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
            var DWStoreQuery = DWContext.Dim_Store.AsNoTracking();
            DWStoreQuery = DWStoreQuery.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            DWStoreQuery = DWStoreQuery.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            var storeIds = await DWStoreQuery.Select(x => x.StoreId).ToListWithNoLockAsync();
            var StoreCheckingQuery = DWContext.Fact_StoreChecking.AsNoTracking();
            StoreCheckingQuery = StoreCheckingQuery.Where(sc => sc.CheckOutAt, new DateFilter { GreaterEqual = Start, LessEqual = End });
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds});
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.StoreId, new IdFilter { In = storeIds });

            if (SaleEmployeeId.HasValue)
                StoreCheckingQuery = StoreCheckingQuery.Where(x =>  x.SaleEmployeeId, new IdFilter { Equal = SaleEmployeeId });
            List<Fact_StoreCheckingDAO> StoreCheckingDAOs = await StoreCheckingQuery.ToListWithNoLockAsync();

            foreach (StoreUncheckingDAO StoreUncheckingDAO in PlannedStoreUncheckingDAOs)
            {
                if (!StoreCheckingDAOs.Any(sc => sc.SaleEmployeeId == StoreUncheckingDAO.AppUserId && sc.StoreId == StoreUncheckingDAO.StoreId))
                {
                    StoreUncheckingDAOs.Add(StoreUncheckingDAO);
                }
            }

            AppUserIds = StoreUncheckingDAOs.Select(x => x.AppUserId).Distinct().ToList();
            var StoreIds = StoreUncheckingDAOs.Select(x => x.StoreId).Distinct().ToList();

            var AppUserDAOs = await DataContext.AppUser
                .Where(x => AppUserIds.Contains(x.Id))
                .Where(x => SaleEmployeeId.HasValue == false || x.Id == SaleEmployeeId)
                .Select(x => new AppUserDAO
                {
                    Id = x.Id,
                    Username = x.Username,
                    DisplayName = x.DisplayName,
                    OrganizationId = x.OrganizationId
                }).OrderBy(x => x.OrganizationId).ThenBy(x => x.DisplayName).ToListWithNoLockAsync();

            var StoreDAOs = await DataContext.Store.Where(x => StoreIds.Contains(x.Id)).Select(x => new StoreDAO
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                CodeDraft = x.CodeDraft,
                Address = x.Address,
                Telephone = x.Telephone,
                StoreType = x.StoreType == null ? null : new StoreTypeDAO
                {
                    Name = x.Name
                },
                StoreStatus = x.StoreStatus == null ? null : new StoreStatusDAO
                {
                    Name = x.StoreStatus.Name
                }
            }).ToListWithNoLockAsync();

            var MonitorSalesman_ExportEmployeeUncheckedDTOs = AppUserDAOs.Select(x => new MonitorSalesman_ExportEmployeeUncheckedDTO
            {
                AppUserId = x.Id,
                DisplayName = x.DisplayName,
            }).ToList();
            foreach(StoreUncheckingDAO StoreUncheckingDAO in StoreUncheckingDAOs)
            {
                StoreUncheckingDAO.Store = StoreDAOs.Where(x => x.Id == StoreUncheckingDAO.StoreId).FirstOrDefault();
                StoreUncheckingDAO.AppUser = AppUserDAOs.Where(x => x.Id == StoreUncheckingDAO.AppUserId).FirstOrDefault();
            }
            foreach(var MonitorSalesman_ExportEmployeeUncheckedDTO in MonitorSalesman_ExportEmployeeUncheckedDTOs)
            {
                MonitorSalesman_ExportEmployeeUncheckedDTO.Contents = StoreUncheckingDAOs
                .Where(x => x.AppUserId == MonitorSalesman_ExportEmployeeUncheckedDTO.AppUserId)
                .Select(x => new MonitorSalesman_ExportUncheckedDTO
                {
                    AppUserId = x.AppUserId,
                    Username = x.AppUser.Username,
                    DisplayName = x.AppUser.DisplayName,
                    StoreCode = x.Store.Code,
                    StoreCodeDraft = x.Store.CodeDraft,
                    StoreAddress = x.Store.Address,
                    StoreName = x.Store.Name,
                    StorePhone = x.Store.Telephone,
                    StoreTypeName = x.Store.StoreType?.Name,
                    StoreStatusName = x.Store.StoreStatus?.Name,
                }).ToList();
            }

            int stt = 1;
            foreach (MonitorSalesman_ExportEmployeeUncheckedDTO MonitorSalesman_ExportEmployeeUncheckedDTO in MonitorSalesman_ExportEmployeeUncheckedDTOs)
            {
                foreach (var Content in MonitorSalesman_ExportEmployeeUncheckedDTO.Contents)
                {
                    Content.STT = stt;
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

            string path = "Templates/Daily_Unchecking_Report.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Date = Now.ToString("HH:mm") + " ngày " + Now.ToString("dd-MM-yyyy");
            Data.Employees = MonitorSalesman_ExportEmployeeUncheckedDTOs;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "Daily_Unchecking_Report.xlsx");
        }

        [Route(MonitorSalesmanRoute.StoreCoverage), HttpPost]
        public async Task<ActionResult<List<MonitorSalesman_StoreCheckingDTO>>> StoreCoverage([FromBody] MonitorSalesman_MonitorSalesmanFilterDTO MonitorSalesman_MonitorSalesmanFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = MonitorSalesman_MonitorSalesmanFilterDTO.CheckIn?.GreaterEqual == null ?
                             LocalStartDay(CurrentContext) :
                             MonitorSalesman_MonitorSalesmanFilterDTO.CheckIn.GreaterEqual.Value;

            DateTime End = MonitorSalesman_MonitorSalesmanFilterDTO.CheckIn?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    MonitorSalesman_MonitorSalesmanFilterDTO.CheckIn.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return BadRequest(new { message = "Chỉ được phép xem tối đa trong vòng 31 ngày" });

            long? OrganizationId = MonitorSalesman_MonitorSalesmanFilterDTO.OrganizationId?.Equal;
            long? SaleEmployeeId = MonitorSalesman_MonitorSalesmanFilterDTO.AppUserId?.Equal;
            long? ProvinceId = MonitorSalesman_MonitorSalesmanFilterDTO.ProvinceId?.Equal;
            long? DistrictId = MonitorSalesman_MonitorSalesmanFilterDTO.DistrictId?.Equal;
            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (MonitorSalesman_MonitorSalesmanFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == MonitorSalesman_MonitorSalesmanFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();
            List<long> AppUserIds = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            var AppUserQuery = DWContext.Dim_AppUser.AsNoTracking();
            AppUserQuery = AppUserQuery.Where(x => x.AppUserId, new IdFilter { In = AppUserIds });
            AppUserQuery = AppUserQuery.Where(x => x.DeletedAt == null);
            if (SaleEmployeeId != null) AppUserQuery = AppUserQuery.Where(x => x.AppUserId, new IdFilter { Equal = SaleEmployeeId.Value });
            AppUserIds = await AppUserQuery.Select(x => x.AppUserId).ToListWithNoLockAsync();

            var DWStoreQuery = DWContext.Dim_Store.AsNoTracking();
            DWStoreQuery = DWStoreQuery.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            DWStoreQuery = DWStoreQuery.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            var storeIds = await DWStoreQuery.Select(x => x.StoreId).ToListWithNoLockAsync();

            var storeCheckingQuery = DWContext.Fact_StoreChecking.AsNoTracking();
            storeCheckingQuery = storeCheckingQuery.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            storeCheckingQuery = storeCheckingQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            storeCheckingQuery = storeCheckingQuery.Where(x => x.StoreId, new IdFilter { In = storeIds });
            storeCheckingQuery = storeCheckingQuery.Where(x => x.CheckOutAt, new DateFilter { GreaterEqual = Start, LessEqual = End });

            var storeImageQuery = DWContext.Fact_Image.AsNoTracking();
            storeImageQuery = storeImageQuery.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            storeImageQuery = storeImageQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            storeImageQuery = storeImageQuery.Where(x => x.StoreId, new IdFilter { In = storeIds });
            storeImageQuery = storeImageQuery.Where(x => x.ShootingAt, new DateFilter { GreaterEqual = Start, LessEqual = End });

            var salesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            salesOrderQuery = salesOrderQuery.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            salesOrderQuery = salesOrderQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            salesOrderQuery = salesOrderQuery.Where(x => x.BuyerStoreId, new IdFilter { In = storeIds });
            salesOrderQuery = salesOrderQuery.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            salesOrderQuery = salesOrderQuery.Where(x => x.RequestStateId, new IdFilter { In = new List<long> { RequestStateEnum.APPROVED.Id, RequestStateEnum.PENDING.Id } });
            salesOrderQuery = salesOrderQuery.Select(i => new Fact_IndirectSalesOrderDAO
            {
                IndirectSalesOrderId = i.IndirectSalesOrderId,
                OrganizationId = i.OrganizationId,
                SaleEmployeeId = i.SaleEmployeeId,
                BuyerStoreId = i.BuyerStoreId,
                Total = i.Total
            });

            List<Fact_StoreCheckingDAO> StoreCheckingDAOs = await storeCheckingQuery.ToListWithNoLockAsync();
            List<Fact_ImageDAO> StoreImageDAOs = await storeImageQuery.ToListWithNoLockAsync();
            List<Fact_IndirectSalesOrderDAO> IndirectSalesOrderDAOs = await salesOrderQuery.ToListWithNoLockAsync();

            List<Fact_ProblemDAO> ProblemDAOs = await DWContext.Fact_Problem
                .Where(p => p.CreatorId, new IdFilter { In = AppUserIds })
                .Where(x => x.NoteAt, new DateFilter { GreaterEqual = Start, LessEqual = End })
                .ToListWithNoLockAsync();

            List<long> StoreIds = new List<long>();
            StoreIds.AddRange(StoreCheckingDAOs.Select(x => x.StoreId).ToList());
            StoreIds.AddRange(StoreImageDAOs.Select(x => x.StoreId).ToList());
            StoreIds.AddRange(ProblemDAOs.Select(x => x.StoreId).ToList());
            StoreIds.AddRange(IndirectSalesOrderDAOs.Select(x => x.BuyerStoreId).ToList());
            StoreIds = StoreIds.Distinct().ToList();
            List<StoreDAO> StoreDAOs = await DWStoreQuery.Select(s => new StoreDAO
            {
                Id = s.StoreId,
                Code = s.Code,
                Name = s.Name,
                Address = s.Address,
            }).ToListWithNoLockAsync();

            var StoreCheckings = new List<MonitorSalesman_StoreCheckingDTO>();
            foreach (long StoreId in StoreIds)
            {
                List<Fact_StoreCheckingDAO> SubStoreCheckingDAOs = StoreCheckingDAOs.Where(s => s.SaleEmployeeId == SaleEmployeeId &&
                        StoreId == s.StoreId &&
                        Start <= s.CheckOutAt.Value && s.CheckOutAt.Value <= End).OrderByDescending(s => s.CheckInAt).ToList();
                Fact_StoreCheckingDAO Checked = SubStoreCheckingDAOs.FirstOrDefault();
                StoreDAO StoreDAO = StoreDAOs.Where(s => s.Id == StoreId).FirstOrDefault();
                MonitorSalesman_StoreCheckingDTO MonitorSalesman_StoreCheckingDTO = new MonitorSalesman_StoreCheckingDTO();
                StoreCheckings.Add(MonitorSalesman_StoreCheckingDTO);
                if (Checked != null)
                {
                    MonitorSalesman_StoreCheckingDTO.Id = Checked.StoreCheckingId;
                    MonitorSalesman_StoreCheckingDTO.Latitude = Checked.Latitude ?? 0;
                    MonitorSalesman_StoreCheckingDTO.Longitude = Checked.Longitude ?? 0;
                    MonitorSalesman_StoreCheckingDTO.CheckIn = Checked.CheckInAt;
                    MonitorSalesman_StoreCheckingDTO.CheckOut = Checked.CheckOutAt;
                }

                MonitorSalesman_StoreCheckingDTO.Image = StoreImageDAOs.Where(si => si.StoreId == StoreId).Select(si => si.Url).FirstOrDefault();
                MonitorSalesman_StoreCheckingDTO.StoreId = StoreDAO.Id;
                MonitorSalesman_StoreCheckingDTO.StoreCode = StoreDAO.Code;
                MonitorSalesman_StoreCheckingDTO.StoreName = StoreDAO.Name;
                MonitorSalesman_StoreCheckingDTO.Address = StoreDAO.Address;

                MonitorSalesman_StoreCheckingDTO.Problem = ProblemDAOs.Where(p => p.StoreId == StoreId)
                 .OrderByDescending(x => x.NoteAt)
                 .Select(p => new MonitorSalesman_ProblemDTO
                 {
                     Id = p.ProblemId,
                     Code = p.Code,
                 }).FirstOrDefault();
                MonitorSalesman_StoreCheckingDTO.IndirectSalesOrder = IndirectSalesOrderDAOs.Where(i => i.BuyerStoreId == StoreId)
                 .OrderByDescending(x => x.OrderDate)
                 .Select(i => new MonitorSalesman_IndirectSalesOrderDTO
                 {
                     Id = i.IndirectSalesOrderId,
                     Code = i.Code,
                 }).FirstOrDefault();

            };

            return StoreCheckings;
        }
        private async Task<List<MonitorSalesman_MonitorSalesmanDTO>> ListDataDW(
            MonitorSalesman_MonitorSalesmanFilterDTO MonitorSalesman_MonitorSalesmanFilterDTO,
            DateTime Start, DateTime End)
        {
            long? OrganizationId = MonitorSalesman_MonitorSalesmanFilterDTO.OrganizationId?.Equal;
            long? SaleEmployeeId = MonitorSalesman_MonitorSalesmanFilterDTO.AppUserId?.Equal;
            long? ProvinceId = MonitorSalesman_MonitorSalesmanFilterDTO.ProvinceId?.Equal;
            long? DistrictId = MonitorSalesman_MonitorSalesmanFilterDTO.DistrictId?.Equal;
            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (MonitorSalesman_MonitorSalesmanFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == MonitorSalesman_MonitorSalesmanFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();
            List<long> AppUserIds = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            var AppUserQuery = DWContext.Dim_AppUser.AsNoTracking();
            AppUserQuery = AppUserQuery.Where(x => x.AppUserId, new IdFilter { In = AppUserIds });
            AppUserQuery = AppUserQuery.Where(x => x.DeletedAt == null);
            if (SaleEmployeeId != null) AppUserQuery = AppUserQuery.Where(x => x.AppUserId, new IdFilter { Equal = SaleEmployeeId.Value });
            AppUserIds = await AppUserQuery.Select(x => x.AppUserId).ToListWithNoLockAsync();

            var DWStoreQuery = DWContext.Dim_Store.AsNoTracking();
            DWStoreQuery = DWStoreQuery.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            DWStoreQuery = DWStoreQuery.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            var storeIds = await DWStoreQuery.Select(x => x.StoreId).ToListWithNoLockAsync();

            var storeCheckingQuery = DWContext.Fact_StoreChecking.AsNoTracking();
            storeCheckingQuery = storeCheckingQuery.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            storeCheckingQuery = storeCheckingQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            storeCheckingQuery = storeCheckingQuery.Where(x => x.StoreId, new IdFilter { In = storeIds });
            storeCheckingQuery = storeCheckingQuery.Where(x => x.CheckOutAt, new DateFilter { GreaterEqual = Start, LessEqual = End });

            var storeImageQuery = DWContext.Fact_Image.AsNoTracking();
            storeImageQuery = storeImageQuery.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            storeImageQuery = storeImageQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            storeImageQuery = storeImageQuery.Where(x => x.StoreId, new IdFilter { In = storeIds });
            storeImageQuery = storeImageQuery.Where(x => x.ShootingAt, new DateFilter { GreaterEqual = Start, LessEqual = End });

            var salesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            salesOrderQuery = salesOrderQuery.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            salesOrderQuery = salesOrderQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            salesOrderQuery = salesOrderQuery.Where(x => x.BuyerStoreId, new IdFilter { In = storeIds });
            salesOrderQuery = salesOrderQuery.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            salesOrderQuery = salesOrderQuery.Where(x => x.RequestStateId, new IdFilter { In = new List<long> { RequestStateEnum.APPROVED.Id, RequestStateEnum.PENDING.Id } });
            salesOrderQuery = salesOrderQuery.Select(i => new Fact_IndirectSalesOrderDAO
            {
                IndirectSalesOrderId = i.IndirectSalesOrderId,
                OrganizationId = i.OrganizationId,
                SaleEmployeeId = i.SaleEmployeeId,
                BuyerStoreId = i.BuyerStoreId,
                Total = i.Total
            });

            var storeUncheckingQuery = DWContext.Fact_StoreUnchecking.AsNoTracking();
            storeUncheckingQuery = storeUncheckingQuery.Where(x => x.AppUserId, new IdFilter { In = AppUserIds });
            storeUncheckingQuery = storeUncheckingQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            storeUncheckingQuery = storeUncheckingQuery.Where(x => x.StoreId, new IdFilter { In = storeIds });
            storeUncheckingQuery = storeUncheckingQuery.Where(x => x.Date, new DateFilter { GreaterEqual = Start, LessEqual = End });

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
                .Skip(MonitorSalesman_MonitorSalesmanFilterDTO.Skip)
                .Take(MonitorSalesman_MonitorSalesmanFilterDTO.Take)
                .ToList();

            AppUserIds = Ids.Select(x => x.SalesEmployeeId).Distinct().ToList();

            List<AppUserDAO> AppUserDAOs = await DataContext.AppUser
                .Where(x => x.DeletedAt == null)
                .Where(x => x.Id, new IdFilter { In = AppUserIds })
                .OrderBy(su => su.OrganizationId).ThenBy(x => x.DisplayName)
                .Select(x => new AppUserDAO
                {
                    Id = x.Id,
                    DisplayName = x.DisplayName,
                    Username = x.Username,
                    OrganizationId = x.OrganizationId
                }).ToListWithNoLockAsync();

            var Organizations = await DataContext.Organization
                .Where(x => x.DeletedAt == null)
                .OrderBy(x => x.Id)
                .Select(x => new OrganizationDAO
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListWithNoLockAsync();

            List<Fact_StoreCheckingDAO> StoreCheckingDAOs = await storeCheckingQuery.ToListWithNoLockAsync();
            List<Fact_ImageDAO> StoreImageDAOs = await storeImageQuery.ToListWithNoLockAsync();
            List<Fact_IndirectSalesOrderDAO> IndirectSalesOrderDAOs = await salesOrderQuery.ToListWithNoLockAsync();
            List<Fact_StoreUncheckingDAO> StoreUncheckingDAOs = await storeUncheckingQuery.ToListWithNoLockAsync();

            List<Fact_ProblemDAO> ProblemDAOs = await DWContext.Fact_Problem
                .Where(p => p.CreatorId, new IdFilter { In = AppUserIds })
                .Where(x => x.NoteAt, new DateFilter { GreaterEqual = Start, LessEqual = End })
                .ToListWithNoLockAsync();

            var eroutecontent_query = DataContext.ERouteContent.AsNoTracking();
            eroutecontent_query = eroutecontent_query.Where(x => x.ERoute.DeletedAt == null);
            eroutecontent_query = eroutecontent_query.Where(x => x.ERoute.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });
            eroutecontent_query = eroutecontent_query.Where(x => x.ERoute.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            eroutecontent_query = eroutecontent_query.Where(ec => ec.ERoute.RealStartDate, new DateFilter { LessEqual = End });
            eroutecontent_query = eroutecontent_query.Where(ec => (ec.ERoute.EndDate == null || ec.ERoute.EndDate.Value >= Start));
            eroutecontent_query = eroutecontent_query.Where(x => x.ERoute.SaleEmployeeId, new IdFilter { In = AppUserIds });
            eroutecontent_query = eroutecontent_query.Where(x => x.Store.ProvinceId, new IdFilter { Equal = ProvinceId });
            eroutecontent_query = eroutecontent_query.Where(x => x.Store.DistrictId, new IdFilter { Equal = DistrictId });
            //eroutecontent_query = eroutecontent_query.Include(ec => ec.ERoute);
            //eroutecontent_query = eroutecontent_query.Include(ec => ec.ERouteContentDays);

            List<ERouteContentDAO> ERouteContentDAOs = await eroutecontent_query.Select (x => new ERouteContentDAO
            {
                Id = x.Id,
                ERouteId = x.ERouteId,
                StoreId = x.StoreId,
                ERoute = new ERouteDAO
                {
                    Id = x.ERouteId,
                    SaleEmployeeId = x.ERoute.SaleEmployeeId,
                    RealStartDate = x.ERoute.RealStartDate,
                    EndDate = x.ERoute.EndDate,
                    StartDate = x.ERoute.StartDate,
                },
                ERouteContentDays = x.ERouteContentDays.Select(e => new ERouteContentDayDAO
                {
                    Id = e.Id,
                    Planned = e.Planned,
                }).ToList()
            }).ToListWithNoLockAsync();

            List<long> StoreIds = new List<long>();
            StoreIds.AddRange(StoreCheckingDAOs.Select(x => x.StoreId).ToList());
            StoreIds.AddRange(StoreImageDAOs.Select(x => x.StoreId).ToList());
            StoreIds.AddRange(ProblemDAOs.Select(x => x.StoreId).ToList());
            StoreIds.AddRange(IndirectSalesOrderDAOs.Select(x => x.BuyerStoreId).ToList());
            StoreIds.AddRange(StoreUncheckingDAOs.Select(x => x.StoreId).ToList());
            StoreIds = StoreIds.Distinct().ToList();

            //var Store_query = DWContext.Dim_Store.AsNoTracking();
            //Store_query = Store_query.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            //Store_query = Store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            //Store_query = Store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });

            List<StoreDAO> StoreDAOs = await DWStoreQuery.Select(s => new StoreDAO
            {
                Id = s.StoreId,
                Code = s.Code,
                Name = s.Name,
                Address = s.Address,
                StoreStatusId = s.StoreStatusId,
                StatusId = s.StatusId,
                OrganizationId = s.OrganizationId,
            }).ToListWithNoLockAsync();

            List<MonitorSalesman_MonitorSalesmanDTO> MonitorSalesman_MonitorSalesmanDTOs = new List<MonitorSalesman_MonitorSalesmanDTO>();
            foreach (var Organization in Organizations)
            {
                MonitorSalesman_MonitorSalesmanDTO MonitorSalesman_MonitorSalesmanDTO = new MonitorSalesman_MonitorSalesmanDTO()
                {
                    OrganizationId = Organization.Id,
                    OrganizationName = Organization.Name,
                    SaleEmployees = new List<MonitorSalesman_SaleEmployeeDTO>()
                };
                MonitorSalesman_MonitorSalesmanDTO.SaleEmployees = Ids
                    .Where(x => x.OrganizationId == Organization.Id)
                    .Select(x => new MonitorSalesman_SaleEmployeeDTO
                    {
                        SaleEmployeeId = x.SalesEmployeeId
                    }).ToList();
                MonitorSalesman_MonitorSalesmanDTOs.Add(MonitorSalesman_MonitorSalesmanDTO);
            }

            foreach( var MonitorSalesman_MonitorSalesmanDTO in MonitorSalesman_MonitorSalesmanDTOs )
            {
                foreach (var MonitorSalesman_SaleEmployeeDTO in MonitorSalesman_MonitorSalesmanDTO.SaleEmployees)
                {
                    var Employee = AppUserDAOs.Where(x => x.Id == MonitorSalesman_SaleEmployeeDTO.SaleEmployeeId).FirstOrDefault();
                    if (Employee != null)
                    {
                        MonitorSalesman_SaleEmployeeDTO.Username = Employee.Username;
                        MonitorSalesman_SaleEmployeeDTO.DisplayName = Employee.DisplayName;
                    }

                    if (MonitorSalesman_SaleEmployeeDTO.StoreCheckings == null)
                        MonitorSalesman_SaleEmployeeDTO.StoreCheckings = new List<MonitorSalesman_StoreCheckingDTO>();
                    MonitorSalesman_SaleEmployeeDTO.PlanCounter = CountPlan(Start, MonitorSalesman_SaleEmployeeDTO.SaleEmployeeId, MonitorSalesman_MonitorSalesmanDTO.OrganizationId, ERouteContentDAOs, AppUserStoreMappings, StoreDAOs);
                    List<Fact_IndirectSalesOrderDAO> SubIndirectSalesOrderDAOs = IndirectSalesOrderDAOs.Where(i => i.SaleEmployeeId == MonitorSalesman_SaleEmployeeDTO.SaleEmployeeId).ToList();
                    MonitorSalesman_SaleEmployeeDTO.SalesOrderCounter = SubIndirectSalesOrderDAOs.Count();
                    MonitorSalesman_SaleEmployeeDTO.Revenue = SubIndirectSalesOrderDAOs.Select(o => o.Total).DefaultIfEmpty(0).Sum();
                    MonitorSalesman_SaleEmployeeDTO.Unchecking = StoreUncheckingDAOs.Where(x => x.AppUserId == MonitorSalesman_SaleEmployeeDTO.SaleEmployeeId).Count();

                    // Lấy tất cả các StoreChecking của AppUserId đang xét
                    List<Fact_StoreCheckingDAO> ListChecked = StoreCheckingDAOs
                           .Where(s => s.SaleEmployeeId == MonitorSalesman_SaleEmployeeDTO.SaleEmployeeId)
                           .ToList();

                    // Lất tất cả StoreIds với appuser đang xét
                    foreach (Fact_StoreCheckingDAO Checked in ListChecked)
                    {
                        if (Checked.Planned)
                        {
                            if (MonitorSalesman_SaleEmployeeDTO.Internal == null)
                                MonitorSalesman_SaleEmployeeDTO.Internal = new HashSet<long>();
                            MonitorSalesman_SaleEmployeeDTO.Internal.Add(Checked.StoreId);
                        }
                        else
                        {
                            if (MonitorSalesman_SaleEmployeeDTO.External == null)
                                MonitorSalesman_SaleEmployeeDTO.External = new HashSet<long>();
                            MonitorSalesman_SaleEmployeeDTO.External.Add(Checked.StoreId);
                        }
                    }

                    MonitorSalesman_SaleEmployeeDTO.ImageCounter = StoreImageDAOs
                        .Where(x => x.SaleEmployeeId == MonitorSalesman_SaleEmployeeDTO.SaleEmployeeId).Count();
                    #region store coverage
                    //foreach (long StoreId in StoreIds)
                    //{
                    //    List<Fact_StoreCheckingDAO> SubStoreCheckingDAOs = StoreCheckingDAOs.Where(s => s.SaleEmployeeId == MonitorSalesman_SaleEmployeeDTO.SaleEmployeeId &&
                    //            StoreId == s.StoreId &&
                    //            Start <= s.CheckOutAt.Value && s.CheckOutAt.Value <= End).OrderByDescending(s => s.CheckInAt).ToList();
                    //    Fact_StoreCheckingDAO Checked = SubStoreCheckingDAOs.FirstOrDefault();
                    //    StoreDAO StoreDAO = StoreDAOs.Where(s => s.Id == StoreId).FirstOrDefault();
                    //    MonitorSalesman_StoreCheckingDTO MonitorSalesman_StoreCheckingDTO = new MonitorSalesman_StoreCheckingDTO();
                    //    MonitorSalesman_SaleEmployeeDTO.StoreCheckings.Add(MonitorSalesman_StoreCheckingDTO);
                    //    if (Checked != null)
                    //    {
                    //        MonitorSalesman_StoreCheckingDTO.Id = Checked.StoreCheckingId;
                    //        MonitorSalesman_StoreCheckingDTO.Latitude = Checked.Latitude ?? 0;
                    //        MonitorSalesman_StoreCheckingDTO.Longitude = Checked.Longitude ?? 0;
                    //        MonitorSalesman_StoreCheckingDTO.CheckIn = Checked.CheckInAt;
                    //        MonitorSalesman_StoreCheckingDTO.CheckOut = Checked.CheckOutAt;
                    //    }

                    //    MonitorSalesman_StoreCheckingDTO.Image = StoreImageDAOs.Where(si => si.StoreId == StoreId).Select(si => si.Url).FirstOrDefault();
                    //    MonitorSalesman_StoreCheckingDTO.StoreId = StoreDAO.Id;
                    //    MonitorSalesman_StoreCheckingDTO.StoreCode = StoreDAO.Code;
                    //    MonitorSalesman_StoreCheckingDTO.StoreName = StoreDAO.Name;
                    //    MonitorSalesman_StoreCheckingDTO.Address = StoreDAO.Address;

                    //    MonitorSalesman_StoreCheckingDTO.Problem = ProblemDAOs.Where(p => p.StoreId == StoreId)
                    //     .OrderByDescending(x => x.NoteAt)
                    //     .Select(p => new MonitorSalesman_ProblemDTO
                    //     {
                    //         Id = p.ProblemId,
                    //         Code = p.Code,
                    //     }).FirstOrDefault();
                    //    MonitorSalesman_StoreCheckingDTO.IndirectSalesOrder = IndirectSalesOrderDAOs.Where(i => i.BuyerStoreId == StoreId)
                    //     .OrderByDescending(x => x.OrderDate)
                    //     .Select(i => new MonitorSalesman_IndirectSalesOrderDTO
                    //     {
                    //         Id = i.IndirectSalesOrderId,
                    //         Code = i.Code,
                    //     }).FirstOrDefault();

                    //};
                    #endregion
                }
            };

            AppUserDAOs = AppUserDAOs.Where(x => !Ids.Select(x => x.SalesEmployeeId).Contains(x.Id)).ToList();
            foreach (AppUserDAO AppUserDAO in AppUserDAOs)
            {
                MonitorSalesman_SaleEmployeeDTO MonitorSalesman_SaleEmployeeDTO = new MonitorSalesman_SaleEmployeeDTO();
                MonitorSalesman_SaleEmployeeDTO.SaleEmployeeId = AppUserDAO.Id;
                MonitorSalesman_SaleEmployeeDTO.Username = AppUserDAO.Username;
                MonitorSalesman_SaleEmployeeDTO.DisplayName = AppUserDAO.DisplayName;
                var MonitorSalesman_MonitorSalesmanDTO = MonitorSalesman_MonitorSalesmanDTOs.Where(x => x.OrganizationId == AppUserDAO.OrganizationId).FirstOrDefault();
                MonitorSalesman_MonitorSalesmanDTO.SaleEmployees.Add(MonitorSalesman_SaleEmployeeDTO);
            }

            return MonitorSalesman_MonitorSalesmanDTOs.Where(x => x.SaleEmployees.Any()).ToList();
        }
    }
}
