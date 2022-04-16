using DMS.Common;
using DMS.DWModels;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers.Configuration;
using DMS.Helpers;
using DMS.Models;
using DMS.Repositories;
using DMS.Services.MAlbum;
using DMS.Services.MAppUser;
using DMS.Services.MBanner;
using DMS.Services.MBrand;
using DMS.Services.MCategory;
using DMS.Services.MColor;
using DMS.Services.MDirectSalesOrder;
using DMS.Services.MDistrict;
using DMS.Services.MERoute;
using DMS.Services.MErpApprovalState;
using DMS.Services.MEstimatedRevenue;
using DMS.Services.MExportTemplate;
using DMS.Services.MGeneralApprovalState;
using DMS.Services.MIndirectSalesOrder;
using DMS.Services.MLuckyDraw;
using DMS.Services.MLuckyDrawRegistration;
using DMS.Services.MLuckyDrawStructure;
using DMS.Services.MLuckyDrawWinner;
using DMS.Services.MLuckyNumber;
using DMS.Services.MNotification;
using DMS.Services.MProblem;
using DMS.Services.MProblemType;
using DMS.Services.MProduct;
using DMS.Services.MProductGrouping;
using DMS.Services.MProvince;
using DMS.Services.MRewardHistory;
using DMS.Services.MRole;
using DMS.Services.MShowingCategory;
using DMS.Services.MStore;
using DMS.Services.MStoreBalance;
using DMS.Services.MStoreChecking;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreScouting;
using DMS.Services.MStoreScoutingType;
using DMS.Services.MStoreStatus;
using DMS.Services.MStoreType;
using DMS.Services.MSupplier;
using DMS.Services.MSurvey;
using DMS.Services.MSystemConfiguration;
using DMS.Services.MTaxType;
using DMS.Services.MWard;
using DMS.Services.MWorkflow;
using GeoCoordinatePortable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public partial class MobileController : SimpleController
    {
        private IEstimatedRevenueService EstimatedRevenueService;
        private IShowingCategoryService ShowingCategoryService;
        private IGeneralApprovalStateService GeneralApprovalStateService;
        private IRequestStateService RequestStateService;
        private IAlbumService AlbumService;
        private IBrandService BrandService;
        private IBannerService BannerService;
        private IAppUserService AppUserService;
        private IColorService ColorService;
        private IERouteService ERouteService;
        private IExportTemplateService ExportTemplateService;
        private IErpApprovalStateService ErpApprovalStateService;
        private IIndirectSalesOrderService IndirectSalesOrderService;
        private IDirectSalesOrderService DirectSalesOrderService;
        private IItemService ItemService;
        private ILuckyNumberService LuckyNumberService;
        private ILuckyDrawService LuckyDrawService;
        private ILuckyDrawRegistrationService LuckyDrawRegistrationService;
        private ILuckyDrawStructureService LuckyDrawStructureService;
        private ILuckyDrawWinnerService LuckyDrawWinnerService;
        private IStoreService StoreService;
        private IStoreBalanceService StoreBalanceService;
        private IStoreScoutingService StoreScoutingService;
        private IStoreGroupingService StoreGroupingService;
        private IStoreCheckingService StoreCheckingService;
        private IStoreStatusService StoreStatusService;
        private IStoreTypeService StoreTypeService;
        private ITaxTypeService TaxTypeService;
        private IProductService ProductService;
        private IProblemService ProblemService;
        private IProblemTypeService ProblemTypeService;
        private IStoreScoutingTypeService StoreScoutingTypeService;
        private ISurveyService SurveyService;
        private IProvinceService ProvinceService;
        private IDistrictService DistrictService;
        private IWardService WardService;
        private ISupplierService SupplierService;
        private IProductGroupingService ProductGroupingService;
        private INotificationService NotificationService;
        private IRewardHistoryService RewardHistoryService;
        private IRabbitManager RabbitManager;
        private ISystemConfigurationService SystemConfigurationService;
        private ICategoryService CategoryService;
        private IPermissionService PermissionService;
        private ICurrentContext CurrentContext;
        private DataContext DataContext;
        private DWContext DWContext;
        private IUOW UOW;
        public MobileController(
            IEstimatedRevenueService EstimatedRevenueService,
            IShowingCategoryService ShowingCategoryService,
            IGeneralApprovalStateService GeneralApprovalStateService,
            IRequestStateService RequestStateService,
            IAlbumService AlbumService,
            IBannerService BannerService,
            IBrandService BrandService,
            IAppUserService AppUserService,
            IColorService ColorService,
            IERouteService ERouteService,
            IExportTemplateService ExportTemplateService,
            IErpApprovalStateService ErpApprovalStateService,
            IIndirectSalesOrderService IndirectSalesOrderService,
            IDirectSalesOrderService DirectSalesOrderService,
            IItemService ItemService,
            ILuckyNumberService LuckyNumberService,
            ILuckyDrawService LuckyDrawService,
            ILuckyDrawRegistrationService LuckyDrawRegistrationService,
            ILuckyDrawStructureService LuckyDrawStructureService,
            ILuckyDrawWinnerService LuckyDrawWinnerService,
            IStoreScoutingService StoreScoutingService,
            IStoreService StoreService,
            IStoreBalanceService StoreBalanceService,
            IStoreGroupingService StoreGroupingService,
            IStoreCheckingService StoreCheckingService,
            IStoreStatusService StoreStatusService,
            IStoreTypeService StoreTypeService,
            IProductService ProductService,
            ITaxTypeService TaxTypeService,
            IProblemService ProblemService,
            IProblemTypeService ProblemTypeService,
            IStoreScoutingTypeService StoreScoutingTypeService,
            ISurveyService SurveyService,
            IProvinceService ProvinceService,
            IDistrictService DistrictService,
            IWardService WardService,
            ISupplierService SupplierService,
            IProductGroupingService ProductGroupingService,
            INotificationService NotificationService,
            IRewardHistoryService RewardHistoryService,
            IRabbitManager RabbitManager,
            ISystemConfigurationService SystemConfigurationService,
            ICategoryService CategoryService,
            IPermissionService PermissionService,
            ICurrentContext CurrentContext,
            DWContext DWContext,
            DataContext DataContext,
            IUOW UOW
        )
        {
            this.EstimatedRevenueService = EstimatedRevenueService;
            this.ShowingCategoryService = ShowingCategoryService;
            this.GeneralApprovalStateService = GeneralApprovalStateService;
            this.RequestStateService = RequestStateService;
            this.AlbumService = AlbumService;
            this.BannerService = BannerService;
            this.BrandService = BrandService;
            this.AppUserService = AppUserService;
            this.ColorService = ColorService;
            this.ERouteService = ERouteService;
            this.ExportTemplateService = ExportTemplateService;
            this.ErpApprovalStateService = ErpApprovalStateService;
            this.IndirectSalesOrderService = IndirectSalesOrderService;
            this.DirectSalesOrderService = DirectSalesOrderService;
            this.ItemService = ItemService;
            this.LuckyNumberService = LuckyNumberService;
            this.LuckyDrawService = LuckyDrawService;
            this.LuckyDrawRegistrationService = LuckyDrawRegistrationService;
            this.LuckyDrawStructureService = LuckyDrawStructureService;
            this.LuckyDrawWinnerService = LuckyDrawWinnerService;
            this.StoreService = StoreService;
            this.StoreBalanceService = StoreBalanceService;
            this.StoreScoutingService = StoreScoutingService;
            this.StoreGroupingService = StoreGroupingService;
            this.StoreCheckingService = StoreCheckingService;
            this.StoreStatusService = StoreStatusService;
            this.StoreTypeService = StoreTypeService;
            this.TaxTypeService = TaxTypeService;
            this.ProductService = ProductService;
            this.ProblemService = ProblemService;
            this.ProblemTypeService = ProblemTypeService;
            this.StoreScoutingTypeService = StoreScoutingTypeService;
            this.SurveyService = SurveyService;
            this.ProvinceService = ProvinceService;
            this.DistrictService = DistrictService;
            this.WardService = WardService;
            this.SupplierService = SupplierService;
            this.ProductGroupingService = ProductGroupingService;
            this.NotificationService = NotificationService;
            this.RewardHistoryService = RewardHistoryService;
            this.SystemConfigurationService = SystemConfigurationService;
            this.CategoryService = CategoryService;
            this.PermissionService = PermissionService;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.DataContext = DataContext;
            this.DWContext = DWContext;
            this.UOW = UOW;
        }

        [Route(GeneralMobileRoute.GetConfiguration), HttpPost]
        public async Task<ActionResult<GeneralMobile_SystemConfigurationDTO>> ListConfiguration()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            SystemConfiguration SystemConfiguration = await SystemConfigurationService.Get();
            return new GeneralMobile_SystemConfigurationDTO(SystemConfiguration);
        }

        [Route(GeneralMobileRoute.CountStoreChecking), HttpPost]
        public async Task<ActionResult<int>> CountStoreChecking([FromBody] GeneralMobile_StoreCheckingFilterDTO GeneralMobile_StoreCheckingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreCheckingFilter StoreCheckingFilter = ConvertFilterDTOToFilterEntity(GeneralMobile_StoreCheckingFilterDTO);
            StoreCheckingFilter = StoreCheckingService.ToFilter(StoreCheckingFilter);
            int count = await StoreCheckingService.Count(StoreCheckingFilter);
            return count;
        }

        [Route(GeneralMobileRoute.ListStoreChecking), HttpPost]
        public async Task<ActionResult<List<GeneralMobile_StoreCheckingDTO>>> ListStoreChecking([FromBody] GeneralMobile_StoreCheckingFilterDTO GeneralMobile_StoreCheckingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreCheckingFilter StoreCheckingFilter = ConvertFilterDTOToFilterEntity(GeneralMobile_StoreCheckingFilterDTO);
            StoreCheckingFilter = StoreCheckingService.ToFilter(StoreCheckingFilter);
            List<StoreChecking> StoreCheckings = await StoreCheckingService.List(StoreCheckingFilter);
            List<GeneralMobile_StoreCheckingDTO> GeneralMobile_StoreCheckingDTOs = StoreCheckings
                .Select(c => new GeneralMobile_StoreCheckingDTO(c)).ToList();
            return GeneralMobile_StoreCheckingDTOs;
        }

        [Route(GeneralMobileRoute.GetStoreChecking), HttpPost]
        public async Task<ActionResult<GeneralMobile_StoreCheckingDTO>> GetStoreChecking([FromBody] GeneralMobile_StoreCheckingDTO GeneralMobile_StoreCheckingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreChecking StoreChecking = await StoreCheckingService.Get(GeneralMobile_StoreCheckingDTO.Id);
            return new GeneralMobile_StoreCheckingDTO(StoreChecking);
        }

        [Route(GeneralMobileRoute.CheckIn), HttpPost]
        public async Task<ActionResult<GeneralMobile_StoreCheckingDTO>> Checkin([FromBody] GeneralMobile_StoreCheckingDTO GeneralMobile_StoreCheckingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreChecking StoreChecking = ConvertDTOToEntity(GeneralMobile_StoreCheckingDTO);
            StoreChecking.DeviceName = HttpContext.Request.Headers["X-Device-Model"];
            StoreChecking = await StoreCheckingService.CheckIn(StoreChecking);
            StoreChecking.IsOpenedStore = true;
            GeneralMobile_StoreCheckingDTO = new GeneralMobile_StoreCheckingDTO(StoreChecking);
            if (StoreChecking.IsValidated)
                return GeneralMobile_StoreCheckingDTO;
            else
                return BadRequest(GeneralMobile_StoreCheckingDTO);
        }

        [Route(GeneralMobileRoute.UpdateStoreChecking), HttpPost]
        public async Task<ActionResult<GeneralMobile_StoreCheckingDTO>> Update([FromBody] GeneralMobile_StoreCheckingDTO GeneralMobile_StoreCheckingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreChecking StoreChecking = ConvertDTOToEntity(GeneralMobile_StoreCheckingDTO);
            StoreChecking = await StoreCheckingService.Update(StoreChecking);
            GeneralMobile_StoreCheckingDTO = new GeneralMobile_StoreCheckingDTO(StoreChecking);
            if (StoreChecking.IsValidated)
                return GeneralMobile_StoreCheckingDTO;
            else
                return BadRequest(GeneralMobile_StoreCheckingDTO);
        }

        [Route(GeneralMobileRoute.UpdateStoreCheckingImage), HttpPost]
        public async Task<ActionResult<GeneralMobile_StoreCheckingDTO>> UpdateStoreCheckingImage([FromBody] GeneralMobile_StoreCheckingDTO GeneralMobile_StoreCheckingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreChecking StoreChecking = ConvertDTOToEntity(GeneralMobile_StoreCheckingDTO);
            StoreChecking = await StoreCheckingService.UpdateStoreCheckingImage(StoreChecking);
            GeneralMobile_StoreCheckingDTO = new GeneralMobile_StoreCheckingDTO(StoreChecking);
            if (StoreChecking.IsValidated)
                return GeneralMobile_StoreCheckingDTO;
            else
                return BadRequest(GeneralMobile_StoreCheckingDTO);
        }

        [Route(GeneralMobileRoute.CheckOut), HttpPost]
        public async Task<ActionResult<GeneralMobile_StoreCheckingDTO>> CheckOut([FromBody] GeneralMobile_StoreCheckingDTO GeneralMobile_StoreCheckingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreChecking StoreChecking = ConvertDTOToEntity(GeneralMobile_StoreCheckingDTO);
            StoreChecking = await StoreCheckingService.CheckOut(StoreChecking);
            GeneralMobile_StoreCheckingDTO = new GeneralMobile_StoreCheckingDTO(StoreChecking);
            if (StoreChecking.IsValidated)
                return GeneralMobile_StoreCheckingDTO;
            else
                return BadRequest(GeneralMobile_StoreCheckingDTO);
        }

        [Route(GeneralMobileRoute.CreateProblem), HttpPost]
        public async Task<ActionResult<GeneralMobile_ProblemDTO>> CreateProblem([FromBody] GeneralMobile_ProblemDTO GeneralMobile_ProblemDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Problem Problem = new Problem
            {
                Id = GeneralMobile_ProblemDTO.Id,
                Content = GeneralMobile_ProblemDTO.Content,
                NoteAt = GeneralMobile_ProblemDTO.NoteAt,
                CompletedAt = GeneralMobile_ProblemDTO.CompletedAt,
                ProblemTypeId = GeneralMobile_ProblemDTO.ProblemTypeId,
                ProblemStatusId = GeneralMobile_ProblemDTO.ProblemStatusId,
                StoreCheckingId = GeneralMobile_ProblemDTO.StoreCheckingId,
                CreatorId = GeneralMobile_ProblemDTO.CreatorId,
                StoreId = GeneralMobile_ProblemDTO.StoreId,
                ProblemImageMappings = GeneralMobile_ProblemDTO.ProblemImageMappings?.Select(x => new ProblemImageMapping
                {
                    ImageId = x.ImageId,
                    ProblemId = x.ProblemId
                }).ToList()
            };
            Problem = await ProblemService.Create(Problem);
            if (Problem.IsValidated)
            {
                GeneralMobile_ProblemDTO = new GeneralMobile_ProblemDTO(Problem);
                return GeneralMobile_ProblemDTO;
            }
            else
                return BadRequest(GeneralMobile_ProblemDTO);
        }

        [Route(GeneralMobileRoute.GetSurveyForm), HttpPost]
        public async Task<ActionResult<GeneralMobile_SurveyDTO>> GetSurveyForm([FromBody] GeneralMobile_SurveyDTO GeneralMobile_SurveyDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Survey Survey = await SurveyService.GetForm(GeneralMobile_SurveyDTO.Id);
            GeneralMobile_SurveyDTO = new GeneralMobile_SurveyDTO(Survey);
            if (Survey.IsValidated)
                return GeneralMobile_SurveyDTO;
            else
                return BadRequest(GeneralMobile_SurveyDTO);
        }

        [Route(GeneralMobileRoute.SaveSurveyForm), HttpPost]
        public async Task<ActionResult<GeneralMobile_SurveyDTO>> SaveSurveyForm([FromBody] GeneralMobile_SurveyDTO GeneralMobile_SurveyDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Survey Survey = new Survey();
            Survey.Id = GeneralMobile_SurveyDTO.Id;
            Survey.Title = GeneralMobile_SurveyDTO.Title;
            Survey.Description = GeneralMobile_SurveyDTO.Description;
            Survey.RespondentAddress = GeneralMobile_SurveyDTO.RespondentAddress;
            Survey.RespondentEmail = GeneralMobile_SurveyDTO.RespondentEmail;
            Survey.RespondentName = GeneralMobile_SurveyDTO.RespondentName;
            Survey.RespondentPhone = GeneralMobile_SurveyDTO.RespondentPhone;
            Survey.StoreId = GeneralMobile_SurveyDTO.StoreId;
            Survey.StoreScoutingId = GeneralMobile_SurveyDTO.StoreScoutingId;
            Survey.SurveyRespondentTypeId = GeneralMobile_SurveyDTO.SurveyRespondentTypeId;
            Survey.StartAt = GeneralMobile_SurveyDTO.StartAt;
            Survey.EndAt = GeneralMobile_SurveyDTO.EndAt;
            Survey.StatusId = GeneralMobile_SurveyDTO.StatusId;

            Survey.SurveyQuestions = GeneralMobile_SurveyDTO.SurveyQuestions?
                .Select(x => new SurveyQuestion
                {
                    Id = x.Id,
                    Content = x.Content,
                    SurveyQuestionTypeId = x.SurveyQuestionTypeId,
                    IsMandatory = x.IsMandatory,
                    SurveyQuestionType = x.SurveyQuestionType == null ? null : new SurveyQuestionType
                    {
                        Id = x.SurveyQuestionType.Id,
                        Code = x.SurveyQuestionType.Code,
                        Name = x.SurveyQuestionType.Name,
                    },
                    SurveyOptions = x.SurveyOptions?.Select(x => new SurveyOption
                    {
                        Id = x.Id,
                        Content = x.Content,
                        SurveyOptionTypeId = x.SurveyOptionTypeId,
                        SurveyQuestionId = x.SurveyOptionTypeId
                    }).ToList(),
                    TableResult = x.TableResult,
                    ListResult = x.ListResult,
                    TextResult = x.TextResult,
                }).ToList();
            Survey.BaseLanguage = CurrentContext.Language;
            Survey = await SurveyService.SaveForm(Survey);
            GeneralMobile_SurveyDTO = new GeneralMobile_SurveyDTO(Survey);
            if (Survey.IsValidated)
                return GeneralMobile_SurveyDTO;
            else
                return BadRequest(GeneralMobile_SurveyDTO);
        }


        [Route(GeneralMobileRoute.GetStoreScouting), HttpPost]
        public async Task<ActionResult<GeneralMobile_StoreScoutingDTO>> GetStoreScouting([FromBody] GeneralMobile_StoreScoutingDTO GeneralMobile_StoreScoutingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreScouting StoreScouting = await StoreScoutingService.Get(GeneralMobile_StoreScoutingDTO.Id);
            return new GeneralMobile_StoreScoutingDTO(StoreScouting);
        }

        [Route(GeneralMobileRoute.CreateStoreScouting), HttpPost]
        public async Task<ActionResult<GeneralMobile_StoreScoutingDTO>> CreateStoreScouting([FromBody] GeneralMobile_StoreScoutingDTO GeneralMobile_StoreScoutingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreScouting StoreScouting = ConvertStoreScoutingToEntity(GeneralMobile_StoreScoutingDTO);
            StoreScouting = await StoreScoutingService.Create(StoreScouting);
            GeneralMobile_StoreScoutingDTO = new GeneralMobile_StoreScoutingDTO(StoreScouting);
            if (StoreScouting.IsValidated)
                return GeneralMobile_StoreScoutingDTO;
            else
                return BadRequest(GeneralMobile_StoreScoutingDTO);
        }

        [Route(GeneralMobileRoute.UpdateStoreScouting), HttpPost]
        public async Task<ActionResult<GeneralMobile_StoreScoutingDTO>> UpdateStoreScouting([FromBody] GeneralMobile_StoreScoutingDTO GeneralMobile_StoreScoutingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreScouting StoreScouting = ConvertStoreScoutingToEntity(GeneralMobile_StoreScoutingDTO);
            StoreScouting = await StoreScoutingService.Update(StoreScouting);
            GeneralMobile_StoreScoutingDTO = new GeneralMobile_StoreScoutingDTO(StoreScouting);
            if (StoreScouting.IsValidated)
                return GeneralMobile_StoreScoutingDTO;
            else
                return BadRequest(GeneralMobile_StoreScoutingDTO);
        }

        [Route(GeneralMobileRoute.DeleteStoreScouting), HttpPost]
        public async Task<ActionResult<GeneralMobile_StoreScoutingDTO>> DeleteStoreScouting([FromBody] GeneralMobile_StoreScoutingDTO GeneralMobile_StoreScoutingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreScouting StoreScouting = ConvertStoreScoutingToEntity(GeneralMobile_StoreScoutingDTO);
            StoreScouting = await StoreScoutingService.Delete(StoreScouting);
            GeneralMobile_StoreScoutingDTO = new GeneralMobile_StoreScoutingDTO(StoreScouting);
            if (StoreScouting.IsValidated)
                return GeneralMobile_StoreScoutingDTO;
            else
                return BadRequest(GeneralMobile_StoreScoutingDTO);
        }

        [Route(GeneralMobileRoute.SaveImage), HttpPost]
        public async Task<ActionResult<GeneralMobile_ImageDTO>> SaveImage(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            MemoryStream memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            DMS.Entities.Image Image = new DMS.Entities.Image
            {
                Name = file.FileName,
                Content = memoryStream.ToArray()
            };
            Image = await StoreCheckingService.SaveImage(Image);
            if (Image == null)
                return BadRequest();
            GeneralMobile_ImageDTO GeneralMobile_ImageDTO = new GeneralMobile_ImageDTO
            {
                Id = Image.Id,
                Name = Image.Name,
                Url = Image.Url,
                ThumbnailUrl = Image.ThumbnailUrl,
            };
            return Ok(GeneralMobile_ImageDTO);
        }

        [Route(GeneralMobileRoute.SaveImage64), HttpPost]
        public async Task<ActionResult<GeneralMobile_ImageDTO>> SaveImage64([FromBody] Image64DTO Image64DTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            byte[] array = Convert.FromBase64String(Image64DTO.Content);
            DMS.Entities.Image Image = new DMS.Entities.Image
            {
                Name = Image64DTO.FileName,
                Content = array,
            };

            Image = await StoreCheckingService.SaveImage(Image);
            if (Image == null)
                return BadRequest();
            GeneralMobile_ImageDTO GeneralMobile_ImageDTO = new GeneralMobile_ImageDTO
            {
                Id = Image.Id,
                Name = Image.Name,
                Url = Image.Url,
                ThumbnailUrl = Image.ThumbnailUrl,
            };
            return Ok(GeneralMobile_ImageDTO);
        }

        [Route(GeneralMobileRoute.UpdateAlbum), HttpPost]
        public async Task<ActionResult<GeneralMobile_AlbumDTO>> UpdateAlbum([FromBody] GeneralMobile_AlbumDTO GeneralMobile_AlbumDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Album Album = new Album
            {
                Id = GeneralMobile_AlbumDTO.Id,
                StatusId = GeneralMobile_AlbumDTO.StatusId,
                AlbumImageMappings = GeneralMobile_AlbumDTO.AlbumImageMappings?.Select(x => new AlbumImageMapping
                {
                    AlbumId = x.AlbumId,
                    ImageId = x.ImageId,
                    StoreId = x.StoreId,
                    OrganizationId = x.OrganizationId,
                    SaleEmployeeId = CurrentContext.UserId,
                    ShootingAt = x.ShootingAt,
                }).ToList()
            };

            var StoreIds = Album.AlbumImageMappings.Select(x => x.StoreId).Distinct().ToList();
            var Stores = await StoreService.List(new StoreFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreSelect.Id | StoreSelect.Longitude | StoreSelect.Latitude,
                Id = new IdFilter { In = StoreIds }
            });
            GeoCoordinate sCoord = new GeoCoordinate((double)CurrentContext.Latitude, (double)CurrentContext.Longitude);
            foreach (AlbumImageMapping AlbumImageMapping in Album.AlbumImageMappings)
            {
                Store Store = Stores.Where(x => x.Id == AlbumImageMapping.StoreId).FirstOrDefault();
                if (Store != null)
                {
                    GeoCoordinate eCoord = new GeoCoordinate((double)Store.Latitude, (double)Store.Longitude);
                    AlbumImageMapping.Distance = (long?)sCoord.GetDistanceTo(eCoord);
                }
            }

            Album = await AlbumService.UpdateMobile(Album);
            GeneralMobile_AlbumDTO = new GeneralMobile_AlbumDTO(Album);
            if (!Album.IsValidated)
                return BadRequest(GeneralMobile_AlbumDTO);
            return Ok(GeneralMobile_AlbumDTO);
        }

        [Route(GeneralMobileRoute.CreateStore), HttpPost]
        public async Task<ActionResult<GeneralMobile_StoreDTO>> CreateStore([FromBody] GeneralMobile_StoreDTO GeneralMobile_StoreDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);

            Store Store = new Store()
            {
                CodeDraft = GeneralMobile_StoreDTO.CodeDraft,
                Name = GeneralMobile_StoreDTO.Name,
                OwnerName = GeneralMobile_StoreDTO.OwnerName,
                OwnerPhone = GeneralMobile_StoreDTO.OwnerPhone,
                StoreTypeId = GeneralMobile_StoreDTO.StoreTypeId,
                ProvinceId = GeneralMobile_StoreDTO.ProvinceId,
                DistrictId = GeneralMobile_StoreDTO.DistrictId,
                WardId = GeneralMobile_StoreDTO.WardId,
                Address = GeneralMobile_StoreDTO.Address,
                CreatorId = CurrentUser.Id,
                DeliveryAddress = GeneralMobile_StoreDTO.Address,
                Latitude = GeneralMobile_StoreDTO.Latitude,
                Longitude = GeneralMobile_StoreDTO.Longitude,
                DeliveryLatitude = GeneralMobile_StoreDTO.Latitude,
                DeliveryLongitude = GeneralMobile_StoreDTO.Longitude,
                OrganizationId = CurrentUser.OrganizationId,
                Organization = CurrentUser.Organization,
                Telephone = GeneralMobile_StoreDTO.OwnerPhone,
                EstimatedRevenueId = GeneralMobile_StoreDTO.EstimatedRevenueId,
                StatusId = StatusEnum.ACTIVE.Id,
                StoreType = GeneralMobile_StoreDTO.StoreType == null ? null : new StoreType
                {
                    Id = GeneralMobile_StoreDTO.StoreType.Id,
                    Code = GeneralMobile_StoreDTO.StoreType.Code,
                    Name = GeneralMobile_StoreDTO.StoreType.Name,
                },
                Creator = CurrentUser == null ? null : new AppUser
                {
                    Id = CurrentUser.Id,
                    Username = CurrentUser.Username,
                    DisplayName = CurrentUser.DisplayName,
                    Address = CurrentUser.Address,
                    Email = CurrentUser.Email,
                    Phone = CurrentUser.Phone,
                },
                Province = GeneralMobile_StoreDTO.Province == null ? null : new Province
                {
                    Id = GeneralMobile_StoreDTO.Province.Id,
                    Code = GeneralMobile_StoreDTO.Province.Code,
                    Name = GeneralMobile_StoreDTO.Province.Name,
                },
                District = GeneralMobile_StoreDTO.District == null ? null : new District
                {
                    Id = GeneralMobile_StoreDTO.District.Id,
                    Code = GeneralMobile_StoreDTO.District.Code,
                    Name = GeneralMobile_StoreDTO.District.Name,
                },
                Ward = GeneralMobile_StoreDTO.Ward == null ? null : new Ward
                {
                    Id = GeneralMobile_StoreDTO.Ward.Id,
                    Code = GeneralMobile_StoreDTO.Ward.Code,
                    Name = GeneralMobile_StoreDTO.Ward.Name,
                },
                StoreStatusId = StoreStatusEnum.DRAFT.Id,
                StoreImageMappings = GeneralMobile_StoreDTO.StoreImageMappings?.Select(x => new StoreImageMapping
                {
                    ImageId = x.ImageId,
                    StoreId = x.StoreId,
                }).ToList(),
                BrandInStores = GeneralMobile_StoreDTO.BrandInStores?
                .Select(x => new BrandInStore
                {
                    Id = x.Id,
                    StoreId = x.StoreId,
                    BrandId = x.BrandId,
                    Top = x.Top,
                    CreatorId = x.CreatorId,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Brand = x.Brand == null ? null : new Brand
                    {
                        Id = x.Brand.Id,
                        Code = x.Brand.Code,
                        Name = x.Brand.Name,
                    },
                    Creator = x.Creator == null ? null : new AppUser
                    {
                        Id = x.Creator.Id,
                        Username = x.Creator.Username,
                        DisplayName = x.Creator.DisplayName,
                    },
                    BrandInStoreProductGroupingMappings = x.BrandInStoreProductGroupingMappings?.Select(x => new BrandInStoreProductGroupingMapping
                    {
                        BrandInStoreId = x.BrandInStoreId,
                        ProductGroupingId = x.ProductGroupingId,
                    }).ToList(),
                    BrandInStoreShowingCategoryMappings = x.BrandInStoreShowingCategoryMappings?.Select(x => new BrandInStoreShowingCategoryMapping
                    {
                        BrandInStoreId = x.BrandInStoreId,
                        ShowingCategoryId = x.ShowingCategoryId,
                    }).ToList()
                }).ToList(),
                StoreStoreGroupingMappings = GeneralMobile_StoreDTO.StoreStoreGroupingMappings?
                .Select(x => new StoreStoreGroupingMapping
                {
                    StoreGroupingId = x.StoreGroupingId,
                    StoreGrouping = x.StoreGrouping == null ? null : new StoreGrouping
                    {
                        Id = x.StoreGrouping.Id,
                        Code = x.StoreGrouping.Code,
                        Name = x.StoreGrouping.Name,
                        ParentId = x.StoreGrouping.ParentId,
                        Path = x.StoreGrouping.Path,
                        Level = x.StoreGrouping.Level,
                        StatusId = x.StoreGrouping.StatusId,
                    },
                }).ToList()
            };
            Store.AppUserStoreMappings = new List<AppUserStoreMapping>();
            Store.AppUserStoreMappings.Add(new AppUserStoreMapping
            {
                AppUserId = CurrentContext.UserId,
                AppUser = CurrentUser
            });
            Store.BaseLanguage = CurrentContext.Language;
            Store.AppUserId = CurrentContext.UserId;
            Store = await StoreService.Create(Store);
            GeneralMobile_StoreDTO = new GeneralMobile_StoreDTO(Store);
            if (Store.IsValidated)
                return GeneralMobile_StoreDTO;
            else
                return BadRequest(GeneralMobile_StoreDTO);
        }

        [Route(GeneralMobileRoute.UpdateDraftStore), HttpPost]
        public async Task<ActionResult<GeneralMobile_StoreDTO>> UpdateDraftStore([FromBody] GeneralMobile_StoreDTO GeneralMobile_StoreDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);
            Store Store = new Store()
            {
                CodeDraft = GeneralMobile_StoreDTO.CodeDraft,
                Name = GeneralMobile_StoreDTO.Name,
                Address = GeneralMobile_StoreDTO.Address,
                OrganizationId = CurrentUser.OrganizationId,
                Telephone = GeneralMobile_StoreDTO.Telephone,
                StatusId = StatusEnum.ACTIVE.Id,
                OwnerPhone = GeneralMobile_StoreDTO.OwnerPhone
            };

            List<Store> DuplicatedStores = await StoreService.FindDuplicatedStore(Store);
            GeneralMobile_StoreDTO.DuplicatedStores = DuplicatedStores.Select(s => new GeneralMobile_StoreDTO(s)).ToList();
            return GeneralMobile_StoreDTO;
        }

        [Route(GeneralMobileRoute.UpdateStore), HttpPost]
        public async Task<ActionResult<GeneralMobile_StoreDTO>> UpdateStore([FromBody] GeneralMobile_StoreDTO GeneralMobile_StoreDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Store Store = await StoreService.Get(GeneralMobile_StoreDTO.Id);
            if (Store == null)
                return NotFound();
            Store.CodeDraft = GeneralMobile_StoreDTO.CodeDraft;
            Store.OwnerPhone = GeneralMobile_StoreDTO.OwnerPhone;
            Store.OwnerName = GeneralMobile_StoreDTO.OwnerName;
            Store.OwnerEmail = GeneralMobile_StoreDTO.OwnerEmail;
            Store.ProvinceId = GeneralMobile_StoreDTO.ProvinceId;
            Store.DistrictId = GeneralMobile_StoreDTO.DistrictId;
            Store.WardId = GeneralMobile_StoreDTO.WardId;
            Store.Address = GeneralMobile_StoreDTO.Address;
            Store.Latitude = GeneralMobile_StoreDTO.Latitude;
            Store.Longitude = GeneralMobile_StoreDTO.Longitude;
            Store.DebtLimited = GeneralMobile_StoreDTO.DebtLimited;
            Store.Description = GeneralMobile_StoreDTO.Description;
            Store.EstimatedRevenueId = GeneralMobile_StoreDTO.EstimatedRevenueId;
            Store.BrandInStores = GeneralMobile_StoreDTO.BrandInStores?
                .Select(x => new BrandInStore
                {
                    Id = x.Id,
                    StoreId = x.StoreId,
                    BrandId = x.BrandId,
                    Top = x.Top,
                    CreatorId = x.CreatorId,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Brand = x.Brand == null ? null : new Brand
                    {
                        Id = x.Brand.Id,
                        Code = x.Brand.Code,
                        Name = x.Brand.Name,
                    },
                    Creator = x.Creator == null ? null : new AppUser
                    {
                        Id = x.Creator.Id,
                        Username = x.Creator.Username,
                        DisplayName = x.Creator.DisplayName,
                    },
                    BrandInStoreProductGroupingMappings = x.BrandInStoreProductGroupingMappings?.Select(x => new BrandInStoreProductGroupingMapping
                    {
                        BrandInStoreId = x.BrandInStoreId,
                        ProductGroupingId = x.ProductGroupingId,
                    }).ToList(),
                    BrandInStoreShowingCategoryMappings = x.BrandInStoreShowingCategoryMappings?.Select(x => new BrandInStoreShowingCategoryMapping
                    {
                        BrandInStoreId = x.BrandInStoreId,
                        ShowingCategoryId = x.ShowingCategoryId,
                    }).ToList()
                }).ToList();
            Store.StoreStoreGroupingMappings = GeneralMobile_StoreDTO.StoreStoreGroupingMappings?
                .Select(x => new StoreStoreGroupingMapping
                {
                    StoreGroupingId = x.StoreGroupingId,
                    StoreGrouping = x.StoreGrouping == null ? null : new StoreGrouping
                    {
                        Id = x.StoreGrouping.Id,
                        Code = x.StoreGrouping.Code,
                        Name = x.StoreGrouping.Name,
                        ParentId = x.StoreGrouping.ParentId,
                        Path = x.StoreGrouping.Path,
                        Level = x.StoreGrouping.Level,
                        StatusId = x.StoreGrouping.StatusId,
                    },
                }).ToList();
            Store.BaseLanguage = CurrentContext.Language;
            Store = await StoreService.Update(Store);
            GeneralMobile_StoreDTO = new GeneralMobile_StoreDTO(Store);
            if (Store.IsValidated)
                return GeneralMobile_StoreDTO;
            else
                return BadRequest(GeneralMobile_StoreDTO);
        }

        [Route(GeneralMobileRoute.GetNotification), HttpPost]
        public async Task<ActionResult<GeneralMobile_NotificationDTO>> GetNotification([FromBody] GeneralMobile_NotificationDTO GeneralMobile_NotificationDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Notification Notification = await NotificationService.Get(GeneralMobile_NotificationDTO.Id);
            return new GeneralMobile_NotificationDTO(Notification);
        }

        [Route(GeneralMobileRoute.UpdateGPS), HttpPost]
        public async Task<ActionResult<bool>> UpdateGPS([FromBody] GeneralMobile_AppUserDTO GeneralMobile_AppUserDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUser AppUser = new AppUser
            {
                Id = CurrentContext.UserId,
                Longitude = GeneralMobile_AppUserDTO.Longitude,
                Latitude = GeneralMobile_AppUserDTO.Latitude
            };
            AppUser = await AppUserService.UpdateGPS(AppUser);
            if (AppUser.IsValidated)
                return true;
            else
                return false;
        }

        [Route(GeneralMobileRoute.StoreReport), HttpPost]
        public async Task<ActionResult<GeneralMobile_StoreReportDTO>> StoreReport([FromBody] GeneralMobile_StoreReportFilterDTO GeneralMobile_StoreReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (GeneralMobile_StoreReportFilterDTO.StoreId == null || GeneralMobile_StoreReportFilterDTO.StoreId.Equal.HasValue == false)
                return new GeneralMobile_StoreReportDTO();

            DateTime Start = GeneralMobile_StoreReportFilterDTO.Date?.GreaterEqual == null ?
                    StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone) :
                    GeneralMobile_StoreReportFilterDTO.Date.GreaterEqual.Value;

            DateTime End = GeneralMobile_StoreReportFilterDTO.Date?.LessEqual == null ?
                    StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone).AddDays(1).AddSeconds(-1) :
                    GeneralMobile_StoreReportFilterDTO.Date.LessEqual.Value;

            GeneralMobile_StoreReportDTO GeneralMobile_StoreReportDTO = new GeneralMobile_StoreReportDTO();
            GeneralMobile_StoreReportDTO.ProblemCounter = await DWContext.Fact_Problem.AsNoTracking()
                .Where(x => x.CreatorId, new IdFilter { Equal = CurrentContext.UserId })
                .Where( x=> x.NoteAt, new DateFilter { GreaterEqual = Start, LessEqual = End })
                .Where(x => x.StoreId, new IdFilter { Equal = GeneralMobile_StoreReportFilterDTO.StoreId.Equal.Value })
                .CountWithNoLockAsync();
            GeneralMobile_StoreReportDTO.ImageCounter = await DWContext.Fact_Image.AsNoTracking()
                .Where(x => x.SaleEmployeeId, new IdFilter { Equal = CurrentContext.UserId })
                .Where(x => x.StoreId, new IdFilter { Equal = GeneralMobile_StoreReportFilterDTO.StoreId.Equal.Value })
                .Where(x => x.ShootingAt, new DateFilter { GreaterEqual = Start, LessEqual = End })
                .CountWithNoLockAsync();
            GeneralMobile_StoreReportDTO.SurveyResultCounter = await DataContext.SurveyResult.AsNoTracking()
                .Where(x => x.AppUserId, new IdFilter { Equal = CurrentContext.UserId })
                .Where(x => x.StoreId, new IdFilter { Equal = GeneralMobile_StoreReportFilterDTO.StoreId.Equal.Value })
                .Where(x => x.Time, new DateFilter { GreaterEqual = Start, LessEqual = End })
                .CountWithNoLockAsync();
            return GeneralMobile_StoreReportDTO;
        }

        [Route(GeneralMobileRoute.StoreStatistic), HttpPost]
        public async Task<ActionResult<GeneralMobile_StoreStatisticDTO>> StoreStatistic([FromBody] GeneralMobile_StoreStatisticFilterDTO GeneralMobile_StoreStatisticFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (GeneralMobile_StoreStatisticFilterDTO.SalesOrderTypeId == null || GeneralMobile_StoreStatisticFilterDTO.StoreId == null || GeneralMobile_StoreStatisticFilterDTO.StoreId.Equal.HasValue == false)
                return new GeneralMobile_StoreStatisticDTO();

            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            (Start, End) = ConvertTime(GeneralMobile_StoreStatisticFilterDTO.Time);

            GeneralMobile_StoreStatisticDTO GeneralMobile_StoreStatisticDTO = new GeneralMobile_StoreStatisticDTO();

            if (GeneralMobile_StoreStatisticFilterDTO.SalesOrderTypeId?.Equal == SalesOrderTypeEnum.DIRECT.Id)
            {
                //var query = from t in DataContext.DirectSalesOrderTransaction
                //            join o in DataContext.DirectSalesOrder on t.DirectSalesOrderId equals o.Id
                //            where t.BuyerStoreId == GeneralMobile_StoreStatisticFilterDTO.StoreId.Equal &&
                //            (o.GeneralApprovalStateId == GeneralApprovalStateEnum.APPROVED.Id || o.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_APPROVED.Id) &&
                //            Start <= t.OrderDate && t.OrderDate <= End
                //            select t;
                var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, new IdFilter { In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id } });
                var DirectSalesOrderIds = await DirectSalesOrderQuery.Select(x => x.DirectSalesOrderId).ToListWithNoLockAsync();

                var DirectSalesOrderTransactionQuery = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DeletedAt == null);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, new IdFilter { Equal = GeneralMobile_StoreStatisticFilterDTO.StoreId.Equal });
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds });
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id });
                GeneralMobile_StoreStatisticDTO.Revenue = await DirectSalesOrderTransactionQuery.Select(x => (x.Amount - (x.GeneralDiscountAmount ?? 0) + (x.TaxAmount ?? 0))).SumAsync();
            }
            else if (GeneralMobile_StoreStatisticFilterDTO.SalesOrderTypeId?.Equal == SalesOrderTypeEnum.INDIRECT.Id)
            {
                var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
                var IndirectSalesOrderIds = await IndirectSalesOrderQuery.Select(x => x.IndirectSalesOrderId).ToListWithNoLockAsync();

                var IndirectSalesOrderTransactionQuery = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, new IdFilter { Equal = GeneralMobile_StoreStatisticFilterDTO.StoreId.Equal });
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id });
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => !x.DeletedAt.HasValue);
                var a = await IndirectSalesOrderTransactionQuery.Select(x => x.IndirectSalesOrderTransactionId).ToListWithNoLockAsync();
                GeneralMobile_StoreStatisticDTO.Revenue = await IndirectSalesOrderTransactionQuery.Select(x => (x.Amount - x.GeneralDiscountAmount ?? 0)).SumAsync();
            }
            else
            {
                var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, new IdFilter { In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id } });
                var DirectSalesOrderIds = await DirectSalesOrderQuery.Select(x => x.DirectSalesOrderId).ToListWithNoLockAsync();

                var DirectSalesOrderTransactionQuery = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DeletedAt == null);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, new IdFilter { Equal = GeneralMobile_StoreStatisticFilterDTO.StoreId.Equal });
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds });
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id });

                var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
                IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
                var IndirectSalesOrderIds = await IndirectSalesOrderQuery.Select(x => x.IndirectSalesOrderId).ToListWithNoLockAsync();

                var IndirectSalesOrderTransactionQuery = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, new IdFilter { Equal = GeneralMobile_StoreStatisticFilterDTO.StoreId.Equal });
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id });
                IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => !x.DeletedAt.HasValue);
                GeneralMobile_StoreStatisticDTO.Revenue = await DirectSalesOrderTransactionQuery.Select(x => (x.Amount - (x.GeneralDiscountAmount ?? 0) + (x.TaxAmount ?? 0))).SumAsync();
                GeneralMobile_StoreStatisticDTO.Revenue = await IndirectSalesOrderTransactionQuery.Select(x => (x.Amount - x.GeneralDiscountAmount ?? 0)).SumAsync();

            }

            return GeneralMobile_StoreStatisticDTO;
        }

        [Route(GeneralMobileRoute.ListPath), HttpPost]
        public async Task<List<string>> ListPath()
        {
            return await PermissionService.ListPath(CurrentContext.UserId);
        }
        private Tuple<DateTime, DateTime> ConvertTime(IdFilter Time)
        {
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            DateTime Now = StaticParams.DateTimeNow;
            if (Time.Equal.HasValue == false)
            {
                Start = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddHours(0 - CurrentContext.TimeZone);
                End = Start.AddHours(CurrentContext.TimeZone).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
            }
            else if (Time.Equal.Value == TODAY)
            {
                Start = LocalStartDay(CurrentContext);
                End = LocalEndDay(CurrentContext);
            }
            else if (Time.Equal.Value == THIS_WEEK)
            {
                int diff = (7 + (Now.AddHours(CurrentContext.TimeZone).DayOfWeek - DayOfWeek.Monday)) % 7;
                Start = LocalStartDay(CurrentContext).AddDays(-1 * diff);
                End = Start.AddDays(7).AddSeconds(-1);
            }
            else if (Time.Equal.Value == LAST_WEEK)
            {
                int diff = (7 + (Now.AddHours(0 - CurrentContext.TimeZone).DayOfWeek - DayOfWeek.Monday)) % 7;
                Start = LocalStartDay(CurrentContext).AddDays(-1 * diff);
                Start = Start.AddDays(-7);
                End = Start.AddDays(7).AddSeconds(-1);
            }
            else if (Time.Equal.Value == THIS_MONTH)
            {
                Start = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddHours(0 - CurrentContext.TimeZone);
                End = Start.AddMonths(1).AddSeconds(-1);
            }
            else if (Time.Equal.Value == LAST_MONTH)
            {
                Start = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddMonths(-1).AddHours(0 - CurrentContext.TimeZone);
                End = Start.AddMonths(1).AddSeconds(-1);
            }
            else if (Time.Equal.Value == THIS_QUARTER)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.AddHours(CurrentContext.TimeZone).Month / 3m));
                Start = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, (this_quarter - 1) * 3 + 1, 1).AddHours(0 - CurrentContext.TimeZone);
                End = Start.AddMonths(3).AddSeconds(-1);
            }
            else if (Time.Equal.Value == YEAR)
            {
                Start = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, 1, 1).AddHours(0 - CurrentContext.TimeZone);
                End = Start.AddYears(1).AddSeconds(-1);
            }
            return Tuple.Create(Start, End);
        }
        private StoreChecking ConvertDTOToEntity(GeneralMobile_StoreCheckingDTO GeneralMobile_StoreCheckingDTO)
        {
            GeneralMobile_StoreCheckingDTO.TrimString();
            StoreChecking StoreChecking = new StoreChecking();
            StoreChecking.Id = GeneralMobile_StoreCheckingDTO.Id;
            StoreChecking.StoreId = GeneralMobile_StoreCheckingDTO.StoreId;
            StoreChecking.SaleEmployeeId = GeneralMobile_StoreCheckingDTO.SaleEmployeeId;
            StoreChecking.Longitude = GeneralMobile_StoreCheckingDTO.Longitude;
            StoreChecking.Latitude = GeneralMobile_StoreCheckingDTO.Latitude;
            StoreChecking.CheckOutLongitude = GeneralMobile_StoreCheckingDTO.CheckOutLongitude;
            StoreChecking.CheckOutLatitude = GeneralMobile_StoreCheckingDTO.CheckOutLatitude;
            StoreChecking.CheckInAt = GeneralMobile_StoreCheckingDTO.CheckInAt;
            StoreChecking.CheckOutAt = GeneralMobile_StoreCheckingDTO.CheckOutAt;
            StoreChecking.CheckInDistance = GeneralMobile_StoreCheckingDTO.CheckInDistance;
            StoreChecking.CheckOutDistance = GeneralMobile_StoreCheckingDTO.CheckOutDistance;
            StoreChecking.CountIndirectSalesOrder = GeneralMobile_StoreCheckingDTO.CountIndirectSalesOrder;
            StoreChecking.ImageCounter = GeneralMobile_StoreCheckingDTO.CountImage;
            StoreChecking.IsOpenedStore = GeneralMobile_StoreCheckingDTO.IsOpenedStore;
            StoreChecking.StoreCheckingImageMappings = GeneralMobile_StoreCheckingDTO.StoreCheckingImageMappings?
                .Select(x => new StoreCheckingImageMapping
                {
                    ImageId = x.ImageId,
                    AlbumId = x.AlbumId,
                    StoreId = x.StoreId,
                    SaleEmployeeId = x.SaleEmployeeId,
                    ShootingAt = x.ShootingAt,
                    Distance = x.Distance,
                    Album = x.Album == null ? null : new Album
                    {
                        Id = x.Album.Id,
                        Name = x.Album.Name,
                    },
                    SaleEmployee = x.SaleEmployee == null ? null : new AppUser
                    {
                        Id = x.SaleEmployee.Id,
                        Username = x.SaleEmployee.Username,
                        DisplayName = x.SaleEmployee.DisplayName,
                        Address = x.SaleEmployee.Address,
                        Email = x.SaleEmployee.Email,
                        Phone = x.SaleEmployee.Phone,
                        PositionId = x.SaleEmployee.PositionId,
                        Department = x.SaleEmployee.Department,
                        OrganizationId = x.SaleEmployee.OrganizationId,
                        SexId = x.SaleEmployee.SexId,
                        StatusId = x.SaleEmployee.StatusId,
                        Avatar = x.SaleEmployee.Avatar,
                        Birthday = x.SaleEmployee.Birthday,
                        ProvinceId = x.SaleEmployee.ProvinceId,
                    },
                    Image = x.Image == null ? null : new DMS.Entities.Image
                    {
                        Id = x.Image.Id,
                        Name = x.Image.Name,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                    },
                    Store = x.Store == null ? null : new Store
                    {
                        Id = x.Store.Id,
                        CodeDraft = x.Store.CodeDraft,
                        Code = x.Store.Code,
                        Name = x.Store.Name,
                        ParentStoreId = x.Store.ParentStoreId,
                        OrganizationId = x.Store.OrganizationId,
                        StoreTypeId = x.Store.StoreTypeId,
                        Telephone = x.Store.Telephone,
                        ProvinceId = x.Store.ProvinceId,
                        DistrictId = x.Store.DistrictId,
                        WardId = x.Store.WardId,
                        Address = x.Store.Address,
                        DeliveryAddress = x.Store.DeliveryAddress,
                        Latitude = x.Store.Latitude,
                        Longitude = x.Store.Longitude,
                        DeliveryLatitude = x.Store.DeliveryLatitude,
                        DeliveryLongitude = x.Store.DeliveryLongitude,
                        OwnerName = x.Store.OwnerName,
                        OwnerPhone = x.Store.OwnerPhone,
                        OwnerEmail = x.Store.OwnerEmail,
                        StatusId = x.Store.StatusId,
                    },
                }).ToList();
            StoreChecking.BaseLanguage = CurrentContext.Language;
            return StoreChecking;
        }

        private StoreScouting ConvertStoreScoutingToEntity(GeneralMobile_StoreScoutingDTO GeneralMobile_StoreScoutingDTO)
        {
            StoreScouting StoreScouting = new StoreScouting();
            StoreScouting.Id = GeneralMobile_StoreScoutingDTO.Id;
            StoreScouting.Code = GeneralMobile_StoreScoutingDTO.Code;
            StoreScouting.Name = GeneralMobile_StoreScoutingDTO.Name;
            StoreScouting.OwnerPhone = GeneralMobile_StoreScoutingDTO.OwnerPhone;
            StoreScouting.ProvinceId = GeneralMobile_StoreScoutingDTO.ProvinceId;
            StoreScouting.DistrictId = GeneralMobile_StoreScoutingDTO.DistrictId;
            StoreScouting.WardId = GeneralMobile_StoreScoutingDTO.WardId;
            StoreScouting.Address = GeneralMobile_StoreScoutingDTO.Address;
            StoreScouting.Latitude = GeneralMobile_StoreScoutingDTO.Latitude;
            StoreScouting.Longitude = GeneralMobile_StoreScoutingDTO.Longitude;
            StoreScouting.CreatorId = GeneralMobile_StoreScoutingDTO.CreatorId;
            StoreScouting.StoreScoutingStatusId = GeneralMobile_StoreScoutingDTO.StoreScoutingStatusId;
            StoreScouting.StoreScoutingTypeId = GeneralMobile_StoreScoutingDTO.StoreScoutingTypeId;
            StoreScouting.Description = GeneralMobile_StoreScoutingDTO.Description;
            StoreScouting.OrganizationId = GeneralMobile_StoreScoutingDTO.OrganizationId;
            StoreScouting.Creator = GeneralMobile_StoreScoutingDTO.Creator == null ? null : new AppUser
            {
                Id = GeneralMobile_StoreScoutingDTO.Creator.Id,
                Username = GeneralMobile_StoreScoutingDTO.Creator.Username,
                DisplayName = GeneralMobile_StoreScoutingDTO.Creator.DisplayName,
                Address = GeneralMobile_StoreScoutingDTO.Creator.Address,
                Email = GeneralMobile_StoreScoutingDTO.Creator.Email,
                Phone = GeneralMobile_StoreScoutingDTO.Creator.Phone,
                PositionId = GeneralMobile_StoreScoutingDTO.Creator.PositionId,
                Department = GeneralMobile_StoreScoutingDTO.Creator.Department,
                OrganizationId = GeneralMobile_StoreScoutingDTO.Creator.OrganizationId,
                StatusId = GeneralMobile_StoreScoutingDTO.Creator.StatusId,
                Avatar = GeneralMobile_StoreScoutingDTO.Creator.Avatar,
                ProvinceId = GeneralMobile_StoreScoutingDTO.Creator.ProvinceId,
                SexId = GeneralMobile_StoreScoutingDTO.Creator.SexId,
                Birthday = GeneralMobile_StoreScoutingDTO.Creator.Birthday,
            };
            StoreScouting.Organization = GeneralMobile_StoreScoutingDTO.Organization == null ? null : new Organization
            {
                Id = GeneralMobile_StoreScoutingDTO.Organization.Id,
                Code = GeneralMobile_StoreScoutingDTO.Organization.Code,
                Name = GeneralMobile_StoreScoutingDTO.Organization.Name,
                ParentId = GeneralMobile_StoreScoutingDTO.Organization.ParentId,
                Path = GeneralMobile_StoreScoutingDTO.Organization.Path,
                Level = GeneralMobile_StoreScoutingDTO.Organization.Level,
                StatusId = GeneralMobile_StoreScoutingDTO.Organization.StatusId,
                Phone = GeneralMobile_StoreScoutingDTO.Organization.Phone,
                Address = GeneralMobile_StoreScoutingDTO.Organization.Address,
                Email = GeneralMobile_StoreScoutingDTO.Organization.Email,
            };
            StoreScouting.District = GeneralMobile_StoreScoutingDTO.District == null ? null : new District
            {
                Id = GeneralMobile_StoreScoutingDTO.District.Id,
                Code = GeneralMobile_StoreScoutingDTO.District.Code,
                Name = GeneralMobile_StoreScoutingDTO.District.Name,
                Priority = GeneralMobile_StoreScoutingDTO.District.Priority,
                ProvinceId = GeneralMobile_StoreScoutingDTO.District.ProvinceId,
                StatusId = GeneralMobile_StoreScoutingDTO.District.StatusId,
            };
            StoreScouting.Province = GeneralMobile_StoreScoutingDTO.Province == null ? null : new Province
            {
                Id = GeneralMobile_StoreScoutingDTO.Province.Id,
                Code = GeneralMobile_StoreScoutingDTO.Province.Code,
                Name = GeneralMobile_StoreScoutingDTO.Province.Name,
                Priority = GeneralMobile_StoreScoutingDTO.Province.Priority,
                StatusId = GeneralMobile_StoreScoutingDTO.Province.StatusId,
            };
            StoreScouting.StoreScoutingStatus = GeneralMobile_StoreScoutingDTO.StoreScoutingStatus == null ? null : new StoreScoutingStatus
            {
                Id = GeneralMobile_StoreScoutingDTO.StoreScoutingStatus.Id,
                Code = GeneralMobile_StoreScoutingDTO.StoreScoutingStatus.Code,
                Name = GeneralMobile_StoreScoutingDTO.StoreScoutingStatus.Name,
            };
            StoreScouting.StoreScoutingType = GeneralMobile_StoreScoutingDTO.StoreScoutingType == null ? null : new StoreScoutingType
            {
                Id = GeneralMobile_StoreScoutingDTO.StoreScoutingType.Id,
                Code = GeneralMobile_StoreScoutingDTO.StoreScoutingType.Code,
                Name = GeneralMobile_StoreScoutingDTO.StoreScoutingType.Name,
            };
            StoreScouting.Ward = GeneralMobile_StoreScoutingDTO.Ward == null ? null : new Ward
            {
                Id = GeneralMobile_StoreScoutingDTO.Ward.Id,
                Code = GeneralMobile_StoreScoutingDTO.Ward.Code,
                Name = GeneralMobile_StoreScoutingDTO.Ward.Name,
                Priority = GeneralMobile_StoreScoutingDTO.Ward.Priority,
                DistrictId = GeneralMobile_StoreScoutingDTO.Ward.DistrictId,
                StatusId = GeneralMobile_StoreScoutingDTO.Ward.StatusId,
            };
            StoreScouting.StoreScoutingImageMappings = GeneralMobile_StoreScoutingDTO.StoreScoutingImageMappings?.Select(x => new StoreScoutingImageMapping
            {
                StoreScoutingId = x.StoreScoutingId,
                ImageId = x.ImageId,
                Image = x.Image == null ? null : new DMS.Entities.Image
                {
                    Id = x.Image.Id,
                    Name = x.Image.Name,
                    Url = x.Image.Url,
                    ThumbnailUrl = x.Image.ThumbnailUrl,
                }
            }).ToList();
            StoreScouting.BaseLanguage = CurrentContext.Language;
            return StoreScouting;
        }
        private StoreCheckingFilter ConvertFilterDTOToFilterEntity(GeneralMobile_StoreCheckingFilterDTO GeneralMobile_StoreCheckingFilterDTO)
        {
            StoreCheckingFilter StoreCheckingFilter = new StoreCheckingFilter();
            StoreCheckingFilter.Selects = StoreCheckingSelect.ALL;
            StoreCheckingFilter.Skip = GeneralMobile_StoreCheckingFilterDTO.Skip;
            StoreCheckingFilter.Take = GeneralMobile_StoreCheckingFilterDTO.Take;
            StoreCheckingFilter.OrderBy = GeneralMobile_StoreCheckingFilterDTO.OrderBy;
            StoreCheckingFilter.OrderType = GeneralMobile_StoreCheckingFilterDTO.OrderType;

            StoreCheckingFilter.Id = GeneralMobile_StoreCheckingFilterDTO.Id;
            StoreCheckingFilter.StoreId = GeneralMobile_StoreCheckingFilterDTO.StoreId;
            StoreCheckingFilter.SaleEmployeeId = GeneralMobile_StoreCheckingFilterDTO.SaleEmployeeId;
            StoreCheckingFilter.Longitude = GeneralMobile_StoreCheckingFilterDTO.Longitude;
            StoreCheckingFilter.Latitude = GeneralMobile_StoreCheckingFilterDTO.Latitude;
            StoreCheckingFilter.CheckInAt = GeneralMobile_StoreCheckingFilterDTO.CheckInAt;
            StoreCheckingFilter.CheckOutAt = GeneralMobile_StoreCheckingFilterDTO.CheckOutAt;
            StoreCheckingFilter.CountIndirectSalesOrder = GeneralMobile_StoreCheckingFilterDTO.CountIndirectSalesOrder;
            StoreCheckingFilter.CountImage = GeneralMobile_StoreCheckingFilterDTO.CountImage;
            return StoreCheckingFilter;
        }

        #region Crop and resize image
        [HttpGet, Route(GeneralMobileRoute.GetCroppedImage)]
        public async Task<IActionResult> Get()
        {
            string UrlParams = HttpContext.Request.Query["url"];
            string FileName = UrlParams.Split("/").Last();
            if (!String.IsNullOrEmpty(UrlParams))
            {
                var Result = await LoadImageByUrl(UrlParams);
                if (Result.Item1 == null)
                {
                    return NotFound();
                }
                return Ok(new GeneralMobile_ImageCropped
                {
                    Name = FileName,
                    Width = Result.Item1.Width,
                    Height = Result.Item1.Height,
                    MimeType = Result.Item2.MimeTypes.FirstOrDefault(),
                });
            }
            return NotFound();
        }

        [HttpGet, Route(GeneralMobileRoute.CroppedImage)]
        public async Task<IActionResult> Crop()
        {
            try
            {
                string UrlParams = HttpContext.Request.Query["url"];
                var Request = HttpContext.Request;
                string WidthParam = Request.Query["width"];
                string HeightParam = Request.Query["height"];
                string XParam = Request.Query["x"];
                string YParam = Request.Query["y"];
                int DestinationWidth = 0;
                int DestinationHeight = 0;
                int SourceX = 0;
                int SourceY = 0;
                if (!String.IsNullOrEmpty(UrlParams))
                {
                    var Result = await LoadImageByUrl(UrlParams);
                    string FileName = UrlParams.Split("/").Last();
                    if (Result.Item1 == null)
                    {
                        return NotFound();
                    }
                    DestinationWidth = Result.Item1.Width; // neu khong chi dinh width thi lay original width
                    DestinationHeight = Result.Item1.Height; // neu khong chi dinh height thi lay original width

                    if (!String.IsNullOrEmpty(WidthParam))
                    {
                        int.TryParse(WidthParam, out DestinationWidth);
                    }
                    if (!String.IsNullOrEmpty(HeightParam))
                    {
                        int.TryParse(HeightParam, out DestinationHeight);
                    }
                    if (!String.IsNullOrEmpty(XParam))
                    {
                        int.TryParse(XParam, out SourceX);
                    }
                    if (!String.IsNullOrEmpty(YParam))
                    {
                        int.TryParse(YParam, out SourceY);
                    }

                    SixLabors.ImageSharp.Image Image = CropImage(Result.Item1, SourceX, SourceY, DestinationWidth, DestinationHeight); // crop image
                    IImageFormat Format = Result.Item2; // format of image

                    MemoryStream OutputStream = new MemoryStream();
                    Image.Save(OutputStream, Format); // save to stream

                    Response.Headers.Add("Content-Type", Result
                        .Item2
                        .MimeTypes
                        .FirstOrDefault());
                    Response.Headers.Add("Content-Length", OutputStream
                        .Length
                        .ToString());

                    return File(OutputStream.ToArray(), "application/octet-stream", FileName);
                }
                return NotFound();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        [HttpGet, Route(GeneralMobileRoute.ResizeImage)]
        public async Task<IActionResult> Resize()
        {
            try
            {
                string UrlParams = HttpContext.Request.Query["url"];
                var Request = HttpContext.Request;
                string WidthParam = Request.Query["width"];
                string HeightParam = Request.Query["height"];
                int DestinationWidth = 0;
                int DestinationHeight = 0;
                if (!String.IsNullOrEmpty(UrlParams))
                {
                    var Result = await LoadImageByUrl(UrlParams);
                    string FileName = UrlParams.Split("/").Last();
                    if (Result.Item1 == null)
                    {
                        return NotFound();
                    }
                    DestinationWidth = Result.Item1.Width; // neu khong chi dinh width thi lay original width
                    DestinationHeight = Result.Item1.Height; // neu khong chi dinh height thi lay original width

                    if (!String.IsNullOrEmpty(WidthParam))
                    {
                        int.TryParse(WidthParam, out DestinationWidth);
                    }
                    if (!String.IsNullOrEmpty(HeightParam))
                    {
                        int.TryParse(HeightParam, out DestinationHeight);
                    }

                    Result.Item1.Mutate(x => x
                       .Resize(DestinationWidth, DestinationHeight)
                    ); // resize image
                    IImageFormat Format = Result.Item2; // format of image
                    MemoryStream OutputStream = new MemoryStream();
                    Result.Item1.Save(OutputStream, Format); // save to stream

                    Response.Headers.Add("Content-Type", Result
                        .Item2
                        .MimeTypes
                        .FirstOrDefault());
                    Response.Headers.Add("Content-Length", OutputStream
                        .Length
                        .ToString());

                    return File(OutputStream.ToArray(), "application/octet-stream", FileName);
                }
                return NotFound();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public async Task<(SixLabors.ImageSharp.Image, IImageFormat)> LoadImageByUrl(string Url)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Cookie", "Token=" + CurrentContext.Token);
                var RequestUrl = string.Format("{0}{1}", InternalServices.UTILS, Url);
                HttpResponseMessage Response = await httpClient.GetAsync(RequestUrl); // request to utils get image
                Stream InputStream = await Response.Content.ReadAsStreamAsync(); // read as binary data
                return await SixLabors.ImageSharp.Image.LoadWithFormatAsync(InputStream);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        private SixLabors.ImageSharp.Image CropImage(SixLabors.ImageSharp.Image Image, int SourceX, int SourceY, int DestinationWidth, int DestinationHeight)
        {
            Image.Mutate(x => x
                 .Crop(new Rectangle(SourceX, SourceY, DestinationWidth, DestinationHeight))
            );
            return Image;
        }
        #endregion

        #region Quay thưởng
        [Route(GeneralMobileRoute.CountLuckyDraw), HttpPost]
        public async Task<ActionResult<int>> CountLuckyDraw([FromBody] GeneralMobile_LuckyDrawFilterDTO GeneralMobile_LuckyDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);
            LuckyDrawFilter LuckyDrawFilter = ConvertLuckyDrawFilterDTOToFilterEntity(GeneralMobile_LuckyDrawFilterDTO);
            LuckyDrawFilter = await LuckyDrawService.ToFilter(LuckyDrawFilter);
            LuckyDrawFilter.StartAt = new DateFilter { LessEqual = StaticParams.DateTimeNow };
            LuckyDrawFilter.EndAt = new DateFilter { GreaterEqual = StaticParams.DateTimeNow };
            LuckyDrawFilter.OrganizationId = new IdFilter { Equal = CurrentUser.OrganizationId };
            LuckyDrawFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            LuckyDrawFilter.IsInAction = true;
            int count = await LuckyDrawService.Count(LuckyDrawFilter);
            return count;
        }
        [Route(GeneralMobileRoute.ListLuckyDraw), HttpPost]
        public async Task<ActionResult<List<GeneralMobile_LuckyDrawDTO>>> ListLuckyDraw([FromBody] GeneralMobile_LuckyDrawFilterDTO GeneralMobile_LuckyDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);
            LuckyDrawFilter LuckyDrawFilter = ConvertLuckyDrawFilterDTOToFilterEntity(GeneralMobile_LuckyDrawFilterDTO);
            LuckyDrawFilter = await LuckyDrawService.ToFilter(LuckyDrawFilter);
            LuckyDrawFilter.EndAt = new DateFilter { GreaterEqual = StaticParams.DateTimeNow };
            LuckyDrawFilter.StartAt = new DateFilter { LessEqual = StaticParams.DateTimeNow };
            LuckyDrawFilter.OrganizationId = new IdFilter { Equal = CurrentUser.OrganizationId };
            LuckyDrawFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            LuckyDrawFilter.IsInAction = true;
            List<LuckyDraw> LuckyDraws = await LuckyDrawService.List(LuckyDrawFilter);
            List<GeneralMobile_LuckyDrawDTO> LuckyDraw_LuckyDrawDTOs = LuckyDraws
                .Select(c => new GeneralMobile_LuckyDrawDTO(c)).ToList();
            return LuckyDraw_LuckyDrawDTOs;
        }
        [Route(GeneralMobileRoute.GetLuckyDraw), HttpPost]
        public async Task<ActionResult<GeneralMobile_LuckyDrawDTO>> GetLuckyDraw([FromBody] GeneralMobile_LuckyDrawDTO GeneralMobile_LuckyDrawDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDraw LuckyDraw = await LuckyDrawService.Get(GeneralMobile_LuckyDrawDTO.Id);
            return new GeneralMobile_LuckyDrawDTO(LuckyDraw);
        }
        [Route(GeneralMobileRoute.RegistrationLuckyDraw), HttpPost]
        public async Task<ActionResult<GeneralMobile_LuckyDrawRegistrationDTO>> RegistrationLuckyDraw([FromBody] GeneralMobile_LuckyDrawRegistrationDTO GeneralMobile_LuckyDrawRegistrationDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            LuckyDrawRegistration LuckyDrawRegistration = new LuckyDrawRegistration();
            LuckyDrawRegistration.LuckyDrawId = GeneralMobile_LuckyDrawRegistrationDTO.LuckyDrawId;
            LuckyDrawRegistration.AppUserId = CurrentContext.UserId;
            LuckyDrawRegistration.StoreId = GeneralMobile_LuckyDrawRegistrationDTO.StoreId;
            LuckyDrawRegistration.Revenue = GeneralMobile_LuckyDrawRegistrationDTO.Revenue;
            LuckyDrawRegistration.IsDrawnByStore = false;
            LuckyDrawRegistration.Time = StaticParams.DateTimeNow;
            LuckyDrawRegistration = await LuckyDrawRegistrationService.Create(LuckyDrawRegistration);
            GeneralMobile_LuckyDrawRegistrationDTO = new GeneralMobile_LuckyDrawRegistrationDTO(LuckyDrawRegistration);
            if (!LuckyDrawRegistration.IsValidated)
                return BadRequest(GeneralMobile_LuckyDrawRegistrationDTO);
            return GeneralMobile_LuckyDrawRegistrationDTO;
        }
        [Route(GeneralMobileRoute.SendLuckyDraw), HttpPost]
        public async Task<ActionResult<GeneralMobile_LuckyDrawRegistrationDTO>> SendLuckyDraw([FromBody] GeneralMobile_LuckyDrawRegistrationDTO GeneralMobile_LuckyDrawRegistrationDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);
            LuckyDrawRegistration LuckyDrawRegistration = new LuckyDrawRegistration();
            LuckyDrawRegistration.LuckyDrawId = GeneralMobile_LuckyDrawRegistrationDTO.LuckyDrawId;
            LuckyDrawRegistration.AppUserId = CurrentUser.Id;
            LuckyDrawRegistration.StoreId = GeneralMobile_LuckyDrawRegistrationDTO.StoreId;
            LuckyDrawRegistration.Revenue = GeneralMobile_LuckyDrawRegistrationDTO.Revenue;
            LuckyDrawRegistration.IsDrawnByStore = true;
            LuckyDrawRegistration.Time = StaticParams.DateTimeNow;
            LuckyDrawRegistration = await LuckyDrawRegistrationService.Create(LuckyDrawRegistration);
            GeneralMobile_LuckyDrawRegistrationDTO = new GeneralMobile_LuckyDrawRegistrationDTO(LuckyDrawRegistration);
            if (!LuckyDrawRegistration.IsValidated)
                return BadRequest(GeneralMobile_LuckyDrawRegistrationDTO);
            return GeneralMobile_LuckyDrawRegistrationDTO;
        }
        [Route(GeneralMobileRoute.ListLuckyDrawStore), HttpPost]
        public async Task<List<GeneralMobile_StoreDTO>> ListLuckyDrawStore([FromBody] GeneralMobile_StoreFilterDTO GeneralMobile_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = GeneralMobile_StoreFilterDTO.Skip;
            StoreFilter.Take = GeneralMobile_StoreFilterDTO.Take;
            StoreFilter.Search = GeneralMobile_StoreFilterDTO.Search;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.Id | StoreSelect.Address | StoreSelect.AppUser | StoreSelect.Code 
                | StoreSelect.Ward | StoreSelect.District | StoreSelect.Province
                | StoreSelect.Name | StoreSelect.Organization | StoreSelect.StoreType;
            StoreFilter.Id = GeneralMobile_StoreFilterDTO.Id;
            StoreFilter.Code = GeneralMobile_StoreFilterDTO.Code;
            StoreFilter.Name = GeneralMobile_StoreFilterDTO.Name;
            StoreFilter.OwnerName = GeneralMobile_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = GeneralMobile_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = GeneralMobile_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = GeneralMobile_StoreFilterDTO.StoreStatusId;
            StoreFilter.LuckyDrawId = GeneralMobile_StoreFilterDTO.LuckyDrawId;
            StoreFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            List<Store> Stores = await LuckyDrawService.ListStore(StoreFilter);
            List<GeneralMobile_StoreDTO> GeneralMobile_StoreDTOs = Stores
                .Select(x => new GeneralMobile_StoreDTO(x)).ToList();
            return GeneralMobile_StoreDTOs;
        }
        [Route(GeneralMobileRoute.CountLuckyDrawStore), HttpPost]
        public async Task<int> CountLuckyDrawStore([FromBody] GeneralMobile_StoreFilterDTO GeneralMobile_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = GeneralMobile_StoreFilterDTO.Skip;
            StoreFilter.Take = GeneralMobile_StoreFilterDTO.Take;
            StoreFilter.Search = GeneralMobile_StoreFilterDTO.Search;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.Id | StoreSelect.Address | StoreSelect.AppUser | StoreSelect.Code
                | StoreSelect.Name | StoreSelect.Organization | StoreSelect.StoreType;
            StoreFilter.Id = GeneralMobile_StoreFilterDTO.Id;
            StoreFilter.Code = GeneralMobile_StoreFilterDTO.Code;
            StoreFilter.Name = GeneralMobile_StoreFilterDTO.Name;
            StoreFilter.OwnerName = GeneralMobile_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = GeneralMobile_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = GeneralMobile_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = GeneralMobile_StoreFilterDTO.StoreStatusId;
            StoreFilter.LuckyDrawId = GeneralMobile_StoreFilterDTO.LuckyDrawId;
            StoreFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            var count = await LuckyDrawService.CountStore(StoreFilter);
            return count;
        }
        [Route(GeneralMobileRoute.ListLuckyDrawHistory), HttpPost]
        public async Task<List<GeneralMobile_LuckyDrawRegistrationDTO>> ListLuckyDrawHistory([FromBody] GeneralMobile_LuckyDrawRegistrationFilterDTO GeneralMobile_LuckyDrawRegistrationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter = new LuckyDrawRegistrationFilter();
            LuckyDrawRegistrationFilter.Take = GeneralMobile_LuckyDrawRegistrationFilterDTO.Take;
            LuckyDrawRegistrationFilter.Skip = GeneralMobile_LuckyDrawRegistrationFilterDTO.Skip;
            LuckyDrawRegistrationFilter.Selects = LuckyDrawRegistrationSelect.ALL;
            LuckyDrawRegistrationFilter.LuckyDrawId = GeneralMobile_LuckyDrawRegistrationFilterDTO.LuckyDrawId;
            LuckyDrawRegistrationFilter.StoreId = GeneralMobile_LuckyDrawRegistrationFilterDTO.StoreId;
            LuckyDrawRegistrationFilter.Revenue = GeneralMobile_LuckyDrawRegistrationFilterDTO.Revenue;
            LuckyDrawRegistrationFilter.Time = GeneralMobile_LuckyDrawRegistrationFilterDTO.Time;
            LuckyDrawRegistrationFilter.Search = GeneralMobile_LuckyDrawRegistrationFilterDTO.Search;
            LuckyDrawRegistrationFilter.TurnCounter = GeneralMobile_LuckyDrawRegistrationFilterDTO.TurnCounter;
            LuckyDrawRegistrationFilter.AppUserId = new IdFilter { Equal = CurrentContext.UserId };
            List<LuckyDrawRegistration> LuckyDrawRegistrations = await LuckyDrawRegistrationService.ListHistory(LuckyDrawRegistrationFilter);
            List<GeneralMobile_LuckyDrawRegistrationDTO> GeneralMobile_LuckyDrawRegistrationDTOs = LuckyDrawRegistrations
                .Select(c => new GeneralMobile_LuckyDrawRegistrationDTO(c)).ToList();
            return GeneralMobile_LuckyDrawRegistrationDTOs;
        }
        [Route(GeneralMobileRoute.CountLuckyDrawHistory), HttpPost]
        public async Task<int> CountLuckyDrawHistory([FromBody] GeneralMobile_LuckyDrawRegistrationFilterDTO GeneralMobile_LuckyDrawRegistrationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter = new LuckyDrawRegistrationFilter();
            LuckyDrawRegistrationFilter.Take = int.MaxValue;
            LuckyDrawRegistrationFilter.Skip = 0;
            LuckyDrawRegistrationFilter.Selects = LuckyDrawRegistrationSelect.ALL;
            LuckyDrawRegistrationFilter.LuckyDrawId = GeneralMobile_LuckyDrawRegistrationFilterDTO.LuckyDrawId;
            LuckyDrawRegistrationFilter.StoreId = GeneralMobile_LuckyDrawRegistrationFilterDTO.StoreId;
            LuckyDrawRegistrationFilter.Revenue = GeneralMobile_LuckyDrawRegistrationFilterDTO.Revenue;
            LuckyDrawRegistrationFilter.TurnCounter = GeneralMobile_LuckyDrawRegistrationFilterDTO.TurnCounter;
            LuckyDrawRegistrationFilter.AppUserId = new IdFilter { Equal = CurrentContext.UserId };
            LuckyDrawRegistrationFilter.Time = GeneralMobile_LuckyDrawRegistrationFilterDTO.Time;
            LuckyDrawRegistrationFilter.Search = GeneralMobile_LuckyDrawRegistrationFilterDTO.Search;
            int count = await LuckyDrawRegistrationService.CountHistory(LuckyDrawRegistrationFilter);
            return count;

        }
        [Route(GeneralMobileRoute.Draw), HttpPost]
        public async Task<GeneralMobile_LuckyDrawWinnerDTO> Draw([FromBody] GeneralMobile_LuckyDrawWinnerDTO GeneralMobile_LuckyDrawWinnerDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDrawWinner LuckyDrawWinner = new LuckyDrawWinner();
            LuckyDrawWinner.Id = GeneralMobile_LuckyDrawWinnerDTO.Id;
            LuckyDrawWinner = await LuckyDrawWinnerService.Draw(LuckyDrawWinner);
            return new GeneralMobile_LuckyDrawWinnerDTO(LuckyDrawWinner);
        }
        [Route(GeneralMobileRoute.CountTurnNotCompletedByEmployee), HttpPost]
        public async Task<int> CountTurnNotCompletedByEmployee([FromBody] GeneralMobile_LuckyDrawWinnerFilterDTO GeneralMobile_LuckyDrawWinnerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            LuckyDrawWinnerFilter LuckyDrawWinnerFilter = new LuckyDrawWinnerFilter();
            LuckyDrawWinnerFilter.Take = GeneralMobile_LuckyDrawWinnerFilterDTO.Take;
            LuckyDrawWinnerFilter.Skip = GeneralMobile_LuckyDrawWinnerFilterDTO.Skip;
            LuckyDrawWinnerFilter.Selects = LuckyDrawWinnerSelect.ALL;
            LuckyDrawWinnerFilter.LuckyDrawId = GeneralMobile_LuckyDrawWinnerFilterDTO.LuckyDrawId;
            LuckyDrawWinnerFilter.StoreId = GeneralMobile_LuckyDrawWinnerFilterDTO.StoreId;
            LuckyDrawWinnerFilter.AppUserId = new IdFilter { Equal = CurrentContext.UserId };
            LuckyDrawWinnerFilter.IsDrawnByStore = false;
            LuckyDrawWinnerFilter.Used = false;
            return await LuckyDrawWinnerService.Count(LuckyDrawWinnerFilter);
        }
        [Route(GeneralMobileRoute.ListTurnNotCompletedByEmployee), HttpPost]
        public async Task<List<GeneralMobile_LuckyDrawWinnerDTO>> ListTurnNotCompletedByEmployee([FromBody] GeneralMobile_LuckyDrawWinnerFilterDTO GeneralMobile_LuckyDrawWinnerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            LuckyDrawWinnerFilter LuckyDrawWinnerFilter = new LuckyDrawWinnerFilter();
            LuckyDrawWinnerFilter.Take = GeneralMobile_LuckyDrawWinnerFilterDTO.Take;
            LuckyDrawWinnerFilter.Skip = GeneralMobile_LuckyDrawWinnerFilterDTO.Skip;
            LuckyDrawWinnerFilter.Selects = LuckyDrawWinnerSelect.ALL;
            LuckyDrawWinnerFilter.AppUserId = new IdFilter { Equal = CurrentContext.UserId };
            LuckyDrawWinnerFilter.LuckyDrawId = GeneralMobile_LuckyDrawWinnerFilterDTO.LuckyDrawId;
            LuckyDrawWinnerFilter.StoreId = GeneralMobile_LuckyDrawWinnerFilterDTO.StoreId;
            LuckyDrawWinnerFilter.IsDrawnByStore = false;
            LuckyDrawWinnerFilter.Used = false;
            var LuckyDrawWinners = await LuckyDrawWinnerService.List(LuckyDrawWinnerFilter);
            var GeneralMobile_LuckyDrawWinnerDTOs = LuckyDrawWinners.Select(x => new GeneralMobile_LuckyDrawWinnerDTO(x)).ToList();
            return GeneralMobile_LuckyDrawWinnerDTOs;
        }
        [Route(GeneralMobileRoute.CountTurnNotCompletedByStore), HttpPost]
        public async Task<int> CountTurnNotCompletedByStore([FromBody] GeneralMobile_LuckyDrawWinnerFilterDTO GeneralMobile_LuckyDrawWinnerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            LuckyDrawWinnerFilter LuckyDrawWinnerFilter = new LuckyDrawWinnerFilter();
            LuckyDrawWinnerFilter.Take = GeneralMobile_LuckyDrawWinnerFilterDTO.Take;
            LuckyDrawWinnerFilter.Skip = GeneralMobile_LuckyDrawWinnerFilterDTO.Skip;
            LuckyDrawWinnerFilter.Selects = LuckyDrawWinnerSelect.ALL;
            LuckyDrawWinnerFilter.AppUserId = new IdFilter { Equal = CurrentContext.UserId };
            LuckyDrawWinnerFilter.LuckyDrawId = GeneralMobile_LuckyDrawWinnerFilterDTO.LuckyDrawId;
            LuckyDrawWinnerFilter.StoreId = GeneralMobile_LuckyDrawWinnerFilterDTO.StoreId;
            LuckyDrawWinnerFilter.IsDrawnByStore = true;
            LuckyDrawWinnerFilter.Used = false;
            return await LuckyDrawWinnerService.Count(LuckyDrawWinnerFilter);
        }
        private LuckyDraw ConvertLuckyDrawDTOToEntity(GeneralMobile_LuckyDrawDTO GeneralMobile_LuckyDrawDTO)
        {
            GeneralMobile_LuckyDrawDTO.TrimString();
            LuckyDraw LuckyDraw = new LuckyDraw();
            LuckyDraw.Id = GeneralMobile_LuckyDrawDTO.Id;
            LuckyDraw.Code = GeneralMobile_LuckyDrawDTO.Code;
            LuckyDraw.Name = GeneralMobile_LuckyDrawDTO.Name;
            LuckyDraw.LuckyDrawTypeId = GeneralMobile_LuckyDrawDTO.LuckyDrawTypeId;
            LuckyDraw.OrganizationId = GeneralMobile_LuckyDrawDTO.OrganizationId;
            LuckyDraw.RevenuePerTurn = GeneralMobile_LuckyDrawDTO.RevenuePerTurn;
            LuckyDraw.StartAt = GeneralMobile_LuckyDrawDTO.StartAt;
            LuckyDraw.EndAt = GeneralMobile_LuckyDrawDTO.EndAt;
            LuckyDraw.ImageId = GeneralMobile_LuckyDrawDTO.ImageId;
            LuckyDraw.StoreId = GeneralMobile_LuckyDrawDTO.StoreId;
            LuckyDraw.AvatarImageId = GeneralMobile_LuckyDrawDTO.AvatarImageId;
            LuckyDraw.Description = GeneralMobile_LuckyDrawDTO.Description;
            LuckyDraw.StatusId = GeneralMobile_LuckyDrawDTO.StatusId;
            LuckyDraw.Used = GeneralMobile_LuckyDrawDTO.Used;
            LuckyDraw.Image = GeneralMobile_LuckyDrawDTO.Image == null ? null : new Entities.Image
            {
                Id = GeneralMobile_LuckyDrawDTO.Image.Id,
                Name = GeneralMobile_LuckyDrawDTO.Image.Name,
                Url = GeneralMobile_LuckyDrawDTO.Image.Url,
                ThumbnailUrl = GeneralMobile_LuckyDrawDTO.Image.ThumbnailUrl,
            };
            LuckyDraw.AvatarImage = GeneralMobile_LuckyDrawDTO.AvatarImage == null ? null : new Entities.Image
            {
                Id = GeneralMobile_LuckyDrawDTO.AvatarImage.Id,
                Name = GeneralMobile_LuckyDrawDTO.AvatarImage.Name,
                Url = GeneralMobile_LuckyDrawDTO.AvatarImage.Url,
                ThumbnailUrl = GeneralMobile_LuckyDrawDTO.AvatarImage.ThumbnailUrl,
            };
            LuckyDraw.LuckyDrawType = GeneralMobile_LuckyDrawDTO.LuckyDrawType == null ? null : new LuckyDrawType
            {
                Id = GeneralMobile_LuckyDrawDTO.LuckyDrawType.Id,
                Code = GeneralMobile_LuckyDrawDTO.LuckyDrawType.Code,
                Name = GeneralMobile_LuckyDrawDTO.LuckyDrawType.Name,
            };
            LuckyDraw.Organization = GeneralMobile_LuckyDrawDTO.Organization == null ? null : new Organization
            {
                Id = GeneralMobile_LuckyDrawDTO.Organization.Id,
                Code = GeneralMobile_LuckyDrawDTO.Organization.Code,
                Name = GeneralMobile_LuckyDrawDTO.Organization.Name,
                ParentId = GeneralMobile_LuckyDrawDTO.Organization.ParentId,
                Path = GeneralMobile_LuckyDrawDTO.Organization.Path,
                Level = GeneralMobile_LuckyDrawDTO.Organization.Level,
                StatusId = GeneralMobile_LuckyDrawDTO.Organization.StatusId,
                Phone = GeneralMobile_LuckyDrawDTO.Organization.Phone,
                Email = GeneralMobile_LuckyDrawDTO.Organization.Email,
                Address = GeneralMobile_LuckyDrawDTO.Organization.Address,
                IsDisplay = GeneralMobile_LuckyDrawDTO.Organization.IsDisplay,
            };
            LuckyDraw.Status = GeneralMobile_LuckyDrawDTO.Status == null ? null : new Status
            {
                Id = GeneralMobile_LuckyDrawDTO.Status.Id,
                Code = GeneralMobile_LuckyDrawDTO.Status.Code,
                Name = GeneralMobile_LuckyDrawDTO.Status.Name,
            };
            LuckyDraw.LuckyDrawStoreGroupingMappings = GeneralMobile_LuckyDrawDTO.LuckyDrawStoreGroupingMappings?
                .Select(x => new LuckyDrawStoreGroupingMapping
                {
                    StoreGroupingId = x.StoreGroupingId,
                    StoreGrouping = x.StoreGrouping == null ? null : new StoreGrouping
                    {
                        Id = x.StoreGrouping.Id,
                        Code = x.StoreGrouping.Code,
                        Name = x.StoreGrouping.Name,
                        ParentId = x.StoreGrouping.ParentId,
                        Path = x.StoreGrouping.Path,
                        Level = x.StoreGrouping.Level,
                        StatusId = x.StoreGrouping.StatusId,
                    },
                }).ToList();
            LuckyDraw.LuckyDrawStoreMappings = GeneralMobile_LuckyDrawDTO.LuckyDrawStoreMappings?
                .Select(x => new LuckyDrawStoreMapping
                {
                    StoreId = x.StoreId,
                    Store = x.Store == null ? null : new Store
                    {
                        Id = x.Store.Id,
                        Code = x.Store.Code,
                        CodeDraft = x.Store.CodeDraft,
                        Name = x.Store.Name,
                        ParentStoreId = x.Store.ParentStoreId,
                        OrganizationId = x.Store.OrganizationId,
                        StoreTypeId = x.Store.StoreTypeId,
                        Telephone = x.Store.Telephone,
                        ProvinceId = x.Store.ProvinceId,
                        DistrictId = x.Store.DistrictId,
                        WardId = x.Store.WardId,
                        Address = x.Store.Address,
                        DeliveryAddress = x.Store.DeliveryAddress,
                        Latitude = x.Store.Latitude,
                        Longitude = x.Store.Longitude,
                        DeliveryLatitude = x.Store.DeliveryLatitude,
                        DeliveryLongitude = x.Store.DeliveryLongitude,
                        OwnerName = x.Store.OwnerName,
                        OwnerPhone = x.Store.OwnerPhone,
                        OwnerEmail = x.Store.OwnerEmail,
                        TaxCode = x.Store.TaxCode,
                        LegalEntity = x.Store.LegalEntity,
                        CreatorId = x.Store.CreatorId,
                        StatusId = x.Store.StatusId,
                        Used = x.Store.Used,
                        StoreScoutingId = x.Store.StoreScoutingId,
                        StoreStatusId = x.Store.StoreStatusId,
                    },
                }).ToList();
            LuckyDraw.LuckyDrawStoreTypeMappings = GeneralMobile_LuckyDrawDTO.LuckyDrawStoreTypeMappings?
                .Select(x => new LuckyDrawStoreTypeMapping
                {
                    StoreTypeId = x.StoreTypeId,
                    StoreType = x.StoreType == null ? null : new StoreType
                    {
                        Id = x.StoreType.Id,
                        Code = x.StoreType.Code,
                        Name = x.StoreType.Name,
                        ColorId = x.StoreType.ColorId,
                        StatusId = x.StoreType.StatusId,
                    },
                }).ToList();
            LuckyDraw.LuckyDrawStructures = GeneralMobile_LuckyDrawDTO.LuckyDrawStructures?
                .Select(x => new LuckyDrawStructure
                {
                    Id = x.Id,
                    Name = x.Name,
                    Value = x.Value,
                    Quantity = x.Quantity,
                }).ToList();
            LuckyDraw.LuckyDrawWinners = GeneralMobile_LuckyDrawDTO.LuckyDrawWinners?
                .Select(x => new LuckyDrawWinner
                {
                    Id = x.Id,
                    LuckyDrawStructureId = x.LuckyDrawStructureId,
                    LuckyDrawRegistrationId = x.LuckyDrawRegistrationId,
                    Time = x.Time,
                    LuckyDrawRegistration = x.LuckyDrawRegistration == null ? null : new LuckyDrawRegistration
                    {
                        Id = x.LuckyDrawRegistration.Id,
                        LuckyDrawId = x.LuckyDrawRegistration.LuckyDrawId,
                        AppUserId = x.LuckyDrawRegistration.AppUserId,
                        StoreId = x.LuckyDrawRegistration.StoreId,
                        Revenue = x.LuckyDrawRegistration.Revenue,
                        TurnCounter = x.LuckyDrawRegistration.TurnCounter,
                        //IsDrawnByStore = x.LuckyDrawRegistration.IsDrawnByStore,
                        Time = x.LuckyDrawRegistration.Time,
                    },
                    LuckyDrawStructure = x.LuckyDrawStructure == null ? null : new LuckyDrawStructure
                    {
                        Id = x.LuckyDrawStructure.Id,
                        LuckyDrawId = x.LuckyDrawStructure.LuckyDrawId,
                        Name = x.LuckyDrawStructure.Name,
                        Value = x.LuckyDrawStructure.Value,
                        Quantity = x.LuckyDrawStructure.Quantity,
                    },
                }).ToList();
            LuckyDraw.LuckyDrawRegistrations = GeneralMobile_LuckyDrawDTO.LuckyDrawRegistrations?
            .Select(x => new LuckyDrawRegistration
            {
                Id = x.Id,
                StoreId = x.StoreId,
                AppUserId = x.AppUserId,
                RemainingTurnCounter = x.RemainingTurnCounter,
                AppUser = x.AppUser == null ? null : new AppUser
                {
                    Id = x.AppUser.Id,
                    Username = x.AppUser.Username,
                    DisplayName = x.AppUser.DisplayName,
                    Address = x.AppUser.Address,
                    Email = x.AppUser.Email,
                    Phone = x.AppUser.Phone,
                    SexId = x.AppUser.SexId,
                    Birthday = x.AppUser.Birthday,
                    Avatar = x.AppUser.Avatar,
                    PositionId = x.AppUser.PositionId,
                    Department = x.AppUser.Department,
                    OrganizationId = x.AppUser.OrganizationId,
                    ProvinceId = x.AppUser.ProvinceId,
                    StatusId = x.AppUser.StatusId,
                },
                Store = x.Store == null ? null : new Store
                {
                    Id = x.Store.Id,
                    Code = x.Store.Code,
                    CodeDraft = x.Store.CodeDraft,
                    Name = x.Store.Name,
                    ParentStoreId = x.Store.ParentStoreId,
                    OrganizationId = x.Store.OrganizationId,
                    StoreTypeId = x.Store.StoreTypeId,
                    Telephone = x.Store.Telephone,
                    ProvinceId = x.Store.ProvinceId,
                    DistrictId = x.Store.DistrictId,
                    WardId = x.Store.WardId,
                    Address = x.Store.Address,
                    DeliveryAddress = x.Store.DeliveryAddress,
                    Latitude = x.Store.Latitude,
                    Longitude = x.Store.Longitude,
                    DeliveryLatitude = x.Store.DeliveryLatitude,
                    DeliveryLongitude = x.Store.DeliveryLongitude,
                    OwnerName = x.Store.OwnerName,
                    OwnerPhone = x.Store.OwnerPhone,
                    OwnerEmail = x.Store.OwnerEmail,
                    TaxCode = x.Store.TaxCode,
                    LegalEntity = x.Store.LegalEntity,
                    CreatorId = x.Store.CreatorId,
                    StatusId = x.Store.StatusId,
                    Used = x.Store.Used,
                    StoreScoutingId = x.Store.StoreScoutingId,
                    StoreStatusId = x.Store.StoreStatusId,
                },
            }).ToList();
            LuckyDraw.BaseLanguage = CurrentContext.Language;
            return LuckyDraw;
        }
        private LuckyDrawFilter ConvertLuckyDrawFilterDTOToFilterEntity(GeneralMobile_LuckyDrawFilterDTO GeneralMobile_LuckyDrawFilterDTO)
        {
            LuckyDrawFilter LuckyDrawFilter = new LuckyDrawFilter();
            LuckyDrawFilter.Selects = LuckyDrawSelect.ALL;
            LuckyDrawFilter.Skip = GeneralMobile_LuckyDrawFilterDTO.Skip;
            LuckyDrawFilter.Take = GeneralMobile_LuckyDrawFilterDTO.Take;
            LuckyDrawFilter.OrderBy = GeneralMobile_LuckyDrawFilterDTO.OrderBy;
            LuckyDrawFilter.OrderType = GeneralMobile_LuckyDrawFilterDTO.OrderType;

            LuckyDrawFilter.Id = GeneralMobile_LuckyDrawFilterDTO.Id;
            LuckyDrawFilter.Code = GeneralMobile_LuckyDrawFilterDTO.Code;
            LuckyDrawFilter.Name = GeneralMobile_LuckyDrawFilterDTO.Name;
            LuckyDrawFilter.LuckyDrawTypeId = GeneralMobile_LuckyDrawFilterDTO.LuckyDrawTypeId;
            LuckyDrawFilter.OrganizationId = GeneralMobile_LuckyDrawFilterDTO.OrganizationId;
            LuckyDrawFilter.RevenuePerTurn = GeneralMobile_LuckyDrawFilterDTO.RevenuePerTurn;
            LuckyDrawFilter.StartAt = GeneralMobile_LuckyDrawFilterDTO.StartAt;
            LuckyDrawFilter.EndAt = GeneralMobile_LuckyDrawFilterDTO.EndAt;
            LuckyDrawFilter.ImageId = GeneralMobile_LuckyDrawFilterDTO.ImageId;
            LuckyDrawFilter.IsInAction = GeneralMobile_LuckyDrawFilterDTO.IsInAction;
            LuckyDrawFilter.AvatarImageId = GeneralMobile_LuckyDrawFilterDTO.AvatarImageId;
            LuckyDrawFilter.StatusId = GeneralMobile_LuckyDrawFilterDTO.StatusId;
            LuckyDrawFilter.CreatedAt = GeneralMobile_LuckyDrawFilterDTO.CreatedAt;
            LuckyDrawFilter.UpdatedAt = GeneralMobile_LuckyDrawFilterDTO.UpdatedAt;
            return LuckyDrawFilter;
        }
        #endregion

        [Route(GeneralMobileRoute.CreateSystemLog), HttpPost]
        public async Task<bool> CreateSystemLog([FromBody] GeneralMobile_SystemLogDTO GeneralMobile_SystemLogDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            SystemLog SystemLog = new SystemLog();
            SystemLog.ModuleName = StaticParams.ModuleName;
            SystemLog.ClassName = "GeneralMobileController";
            SystemLog.MethodName = GeneralMobile_SystemLogDTO.MethodName;
            SystemLog.Exception = GeneralMobile_SystemLogDTO.Exception;
            SystemLog.Time = StaticParams.DateTimeNow;
            SystemLog.AppUserId = CurrentContext.UserId;
            var AppUser = await AppUserService.Get(CurrentContext.UserId);
            SystemLog.AppUser = AppUser.Username;

            RabbitManager.PublishSingle(SystemLog, RoutingKeyEnum.SystemLogSend.Code);
            return true;
        }
    }

    public class Image64DTO : DataDTO
    {
        public string FileName { get; set; }
        public string Content { get; set; }
    }
}

