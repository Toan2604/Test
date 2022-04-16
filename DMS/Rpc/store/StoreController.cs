using DMS.Common;
using DMS.DWModels;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MBrand;
using DMS.Services.MDistrict;
using DMS.Services.MEstimatedRevenue;
using DMS.Services.MOrganization;
using DMS.Services.MProductGrouping;
using DMS.Services.MProvince;
using DMS.Services.MShowingCategory;
using DMS.Services.MStatus;
using DMS.Services.MStore;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreScouting;
using DMS.Services.MStoreStatus;
using DMS.Services.MStoreType;
using DMS.Services.MStoreUser;
using DMS.Services.MWard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;
namespace DMS.Rpc.store
{
    public partial class StoreController : RpcController
    {
        private IShowingCategoryService ShowingCategoryService;
        private IAppUserService AppUserService;
        private IBrandService BrandService;
        private IDistrictService DistrictService;
        private IOrganizationService OrganizationService;
        private IProductGroupingService ProductGroupingService;
        private IProvinceService ProvinceService;
        private IStoreStatusService StoreStatusService;
        private IStatusService StatusService;
        private IStoreScoutingService StoreScoutingService;
        private IStoreGroupingService StoreGroupingService;
        private IStoreTypeService StoreTypeService;
        private IWardService WardService;
        private IStoreService StoreService;
        private IStoreUserService StoreUserService;
        private ICurrentContext CurrentContext;
        private IEstimatedRevenueService EstimatedRevenueService;
        private ITimeService TimeService;
        private DataContext DataContext;
        private DWContext DWContext;
        public StoreController(
            IEstimatedRevenueService EstimatedRevenueService,
            IShowingCategoryService ShowingCategoryService,
            IAppUserService AppUserService,
            IBrandService BrandService,
            IDistrictService DistrictService,
            IOrganizationService OrganizationService,
            IProductGroupingService ProductGroupingService,
            IProvinceService ProvinceService,
            IStoreStatusService StoreStatusService,
            IStatusService StatusService,
            IStoreScoutingService StoreScoutingService,
            IStoreGroupingService StoreGroupingService,
            IStoreTypeService StoreTypeService,
            IWardService WardService,
            IStoreService StoreService,
            IStoreUserService StoreUserService,
            ICurrentContext CurrentContext,
            ITimeService TimeService,
            DataContext DataContext,
            DWContext DWContext
        )
        {
            this.EstimatedRevenueService = EstimatedRevenueService;
            this.ShowingCategoryService = ShowingCategoryService;
            this.AppUserService = AppUserService;
            this.BrandService = BrandService;
            this.DistrictService = DistrictService;
            this.OrganizationService = OrganizationService;
            this.ProductGroupingService = ProductGroupingService;
            this.ProvinceService = ProvinceService;
            this.StoreStatusService = StoreStatusService;
            this.StatusService = StatusService;
            this.StoreScoutingService = StoreScoutingService;
            this.StoreGroupingService = StoreGroupingService;
            this.StoreTypeService = StoreTypeService;
            this.WardService = WardService;
            this.StoreService = StoreService;
            this.StoreUserService = StoreUserService;
            this.TimeService = TimeService;
            this.CurrentContext = CurrentContext;
            this.DataContext = DataContext;
            this.DWContext = DWContext;
        }
        [Route(StoreRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] Store_StoreFilterDTO Store_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StoreFilter StoreFilter = ConvertFilterDTOToFilterEntity(Store_StoreFilterDTO);
            StoreFilter = StoreService.ToFilter(StoreFilter);
            int count = await StoreService.Count(StoreFilter);
            return count;
        }
        [Route(StoreRoute.List), HttpPost]
        public async Task<ActionResult<List<Store_StoreDTO>>> List([FromBody] Store_StoreFilterDTO Store_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StoreFilter StoreFilter = ConvertFilterDTOToFilterEntity(Store_StoreFilterDTO);
            StoreFilter = StoreService.ToFilter(StoreFilter);
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<Store_StoreDTO> Store_StoreDTOs = Stores
                .Select(c => new Store_StoreDTO(c)).ToList();
            return Store_StoreDTOs;
        }
        [Route(StoreRoute.Get), HttpPost]
        public async Task<ActionResult<Store_StoreDTO>> Get([FromBody] Store_StoreDTO Store_StoreDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (!await HasPermission(Store_StoreDTO.Id))
                return Forbid();
            Store Store = await StoreService.Get(Store_StoreDTO.Id);
            return new Store_StoreDTO(Store);
        }
        [Route(StoreRoute.GetDraft), HttpPost]
        public async Task<ActionResult<Store_StoreDTO>> GetDraft([FromBody] Store_StoreScoutingDTO Store_StoreScoutingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StoreScouting StoreScouting = await StoreScoutingService.Get(Store_StoreScoutingDTO.Id);
            Store_StoreDTO Store_StoreDTO = new Store_StoreDTO
            {
                Address = StoreScouting.Address,
                AppUserId = StoreScouting.CreatorId,
                OrganizationId = StoreScouting.OrganizationId,
                DistrictId = StoreScouting.DistrictId,
                Latitude = StoreScouting.Latitude,
                Longitude = StoreScouting.Longitude,
                Name = StoreScouting.Name,
                OwnerPhone = StoreScouting.OwnerPhone,
                ProvinceId = StoreScouting.ProvinceId,
                StatusId = StatusEnum.INACTIVE.Id,
                StoreStatusId = StoreStatusEnum.OFFICIAL.Id,
                WardId = StoreScouting.WardId,
                AppUser = StoreScouting.Creator == null ? null : new Store_AppUserDTO(StoreScouting.Creator),
                Organization = StoreScouting.Organization == null ? null : new Store_OrganizationDTO(StoreScouting.Organization),
                District = StoreScouting.District == null ? null : new Store_DistrictDTO(StoreScouting.District),
                Province = StoreScouting.Province == null ? null : new Store_ProvinceDTO(StoreScouting.Province),
                Ward = StoreScouting.Ward == null ? null : new Store_WardDTO(StoreScouting.Ward),
                Status = new Store_StatusDTO
                {
                    Id = StatusEnum.INACTIVE.Id,
                    Code = StatusEnum.INACTIVE.Code,
                    Name = StatusEnum.INACTIVE.Name,
                },
                StoreScoutingId = StoreScouting.Id,
                StoreScouting = new Store_StoreScoutingDTO(StoreScouting)
            };
            return Store_StoreDTO;
        }
        [Route(StoreRoute.Create), HttpPost]
        public async Task<ActionResult<Store_StoreDTO>> Create([FromBody] Store_StoreDTO Store_StoreDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (!await HasPermission(Store_StoreDTO.Id))
                return Forbid();
            Store Store = ConvertDTOToEntity(Store_StoreDTO);
            Store = await StoreService.Create(Store);
            Store_StoreDTO = new Store_StoreDTO(Store);
            if (Store.IsValidated)
                return Store_StoreDTO;
            else
                return BadRequest(Store_StoreDTO);
        }
        [Route(StoreRoute.UpdateDraft), HttpPost]
        public async Task<ActionResult<Store_StoreDTO>> UpdateDraft([FromBody] Store_StoreDTO Store_StoreDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (!await HasPermission(Store_StoreDTO.Id))
                return Forbid();
            Store Store = ConvertDTOToEntity(Store_StoreDTO);
            List<Store> DuplicatedStores = await StoreService.FindDuplicatedStore(Store);
            Store_StoreDTO.DuplicatedStores = DuplicatedStores.Select(s => new Store_StoreDTO(s)).ToList();
            return Store_StoreDTO;
        }
        [Route(StoreRoute.Update), HttpPost]
        public async Task<ActionResult<Store_StoreDTO>> Update([FromBody] Store_StoreDTO Store_StoreDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (!await HasPermission(Store_StoreDTO.Id))
                return Forbid();
            Store Store = ConvertDTOToEntity(Store_StoreDTO);
            Store = await StoreService.Update(Store);
            Store_StoreDTO = new Store_StoreDTO(Store);
            if (Store.IsValidated)
                return Store_StoreDTO;
            else
                return BadRequest(Store_StoreDTO);
        }
        [Route(StoreRoute.Approve), HttpPost]
        public async Task<ActionResult<Store_StoreDTO>> Approve([FromBody] Store_StoreDTO Store_StoreDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (!await HasPermission(Store_StoreDTO.Id))
                return Forbid();
            Store Store = ConvertDTOToEntity(Store_StoreDTO);
            Store.StoreStatusId = StoreStatusEnum.OFFICIAL.Id;
            Store = await StoreService.Update(Store);
            Store_StoreDTO = new Store_StoreDTO(Store);
            if (Store.IsValidated)
                return Store_StoreDTO;
            else
                return BadRequest(Store_StoreDTO);
        }
        [Route(StoreRoute.Delete), HttpPost]
        public async Task<ActionResult<Store_StoreDTO>> Delete([FromBody] Store_StoreDTO Store_StoreDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (!await HasPermission(Store_StoreDTO.Id))
                return Forbid();
            Store Store = ConvertDTOToEntity(Store_StoreDTO);
            Store = await StoreService.Delete(Store);
            Store_StoreDTO = new Store_StoreDTO(Store);
            if (Store.IsValidated)
                return Store_StoreDTO;
            else
                return BadRequest(Store_StoreDTO);
        }
        [Route(StoreRoute.Import), HttpPost]
        public async Task<ActionResult<List<Store_StoreDTO>>> Import(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            FileInfo FileInfo = new FileInfo(file.FileName);
            if (!FileInfo.Extension.Equals(".xlsx"))
                return BadRequest("Định dạng file không hợp lệ");
            MemoryStream MemoryStream = new MemoryStream();
            file.CopyTo(MemoryStream);
            #region MDM
            List<Store> ParentStores = await StoreService.List(new StoreFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreSelect.Code | StoreSelect.Name | StoreSelect.Id
            });
            List<Organization> Organizations = await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.Id | OrganizationSelect.Code
            });
            List<StoreType> StoreTypes = await StoreTypeService.List(new StoreTypeFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreTypeSelect.Id | StoreTypeSelect.Code
            });
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(new StoreGroupingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreGroupingSelect.Id | StoreGroupingSelect.Code
            });
            List<Province> Provinces = await ProvinceService.List(new ProvinceFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProvinceSelect.Id | ProvinceSelect.Code
            });
            List<District> Districts = await DistrictService.List(new DistrictFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = DistrictSelect.Id | DistrictSelect.Code
            });
            List<Ward> Wards = await WardService.List(new WardFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = WardSelect.Id | WardSelect.Code
            });
            List<StoreStatus> StoreStatuses = await StoreStatusService.List(new StoreStatusFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreStatusSelect.Id | StoreStatusSelect.Code | StoreStatusSelect.Name
            });
            List<Status> Statuses = await StatusService.List(new StatusFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StatusSelect.Id | StatusSelect.Code | StatusSelect.Name
            });
            List<Store> All = await StoreService.List(new StoreFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.Name | StoreSelect.CodeDraft | StoreSelect.CreatedAt | StoreSelect.Creator
            });
            List<string> AllIds = All.Select(a => a.Code).ToList();
            Dictionary<string, Store> DictionaryAll = All.ToDictionary(x => x.Code, y => y);
            #endregion  // To get Id from Db by CodeValue in DTO
            List<Store_ImportDTO> Store_ImportDTOs = new List<Store_ImportDTO>();
            using (ExcelPackage excelPackage = new ExcelPackage(MemoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Đại lý"];
                if (worksheet == null)
                    return BadRequest("File không đúng biểu mẫu import");
                #region khai báo các cột
                int StartColumn = 1;
                int StartRow = 1;
                int SttColumnn = 0 + StartColumn;
                int CodeColumn = 1 + StartColumn;
                int CodeDraftColumn = 2 + StartColumn;
                int NameColumn = 3 + StartColumn;
                int OrganizationCodeColumn = 4 + StartColumn;
                int ParentStoreCodeColumn = 5 + StartColumn;
                int StoreTypeCodeColumn = 6 + StartColumn;
                int StoreGroupingCodeColumn = 7 + StartColumn;
                int LegalEntityColumn = 8 + StartColumn;
                int TaxCodeColumn = 9 + StartColumn;
                int ProvinceCodeColumn = 10 + StartColumn;
                int DistrictCodeColumn = 11 + StartColumn;
                int WardCodeColumn = 12 + StartColumn;
                int AddressColumn = 13 + StartColumn;
                int LongitudeColumn = 14 + StartColumn;
                int LatitudeColumn = 15 + StartColumn;
                int DeliveryAddressColumn = 16 + StartColumn;
                int DeliveryLongitudeColumn = 17 + StartColumn;
                int DeliveryLatitudeColumn = 18 + StartColumn;
                int TelephoneColumn = 19 + StartColumn;
                int OwnerNameColumn = 20 + StartColumn;
                int OwnerPhoneColumn = 21 + StartColumn;
                int OwnerEmailColumn = 22 + StartColumn;
                int StoreStatusColumn = 23 + StartColumn;
                int StatusColumn = 24 + StartColumn;
                #endregion
                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    #region Lấy thông tin từng dòng
                    string stt = worksheet.Cells[i + StartRow, SttColumnn].Value?.ToString();
                    if (stt != null && stt.ToLower() == "END".ToLower())
                        break;
                    bool convert = long.TryParse(stt, out long Stt);
                    if (convert == false)
                        continue;
                    Store_ImportDTO Store_ImportDTO = new Store_ImportDTO();
                    Store_ImportDTOs.Add(Store_ImportDTO);
                    Store_ImportDTO.Stt = Stt;
                    Store_ImportDTO.CodeValue = worksheet.Cells[i + StartRow, CodeColumn].Value?.ToString();
                    Store_ImportDTO.CodeDraftValue = worksheet.Cells[i + StartRow, CodeDraftColumn].Value?.ToString();
                    Store_ImportDTO.NameValue = worksheet.Cells[i + StartRow, NameColumn].Value?.ToString();
                    Store_ImportDTO.OrganizationCodeValue = worksheet.Cells[i + StartRow, OrganizationCodeColumn].Value?.ToString();
                    Store_ImportDTO.ParentStoreCodeValue = worksheet.Cells[i + StartRow, ParentStoreCodeColumn].Value?.ToString();
                    Store_ImportDTO.StoreTypeCodeValue = worksheet.Cells[i + StartRow, StoreTypeCodeColumn].Value?.ToString();
                    Store_ImportDTO.StoreGroupingCodeValue = worksheet.Cells[i + StartRow, StoreGroupingCodeColumn].Value?.ToString();
                    Store_ImportDTO.LegalEntityValue = worksheet.Cells[i + StartRow, LegalEntityColumn].Value?.ToString();
                    Store_ImportDTO.TaxCodeValue = worksheet.Cells[i + StartRow, TaxCodeColumn].Value?.ToString();
                    Store_ImportDTO.ProvinceCodeValue = worksheet.Cells[i + StartRow, ProvinceCodeColumn].Value?.ToString();
                    Store_ImportDTO.DistrictCodeValue = worksheet.Cells[i + StartRow, DistrictCodeColumn].Value?.ToString();
                    Store_ImportDTO.WardCodeValue = worksheet.Cells[i + StartRow, WardCodeColumn].Value?.ToString();
                    Store_ImportDTO.AddressValue = worksheet.Cells[i + StartRow, AddressColumn].Value?.ToString();
                    Store_ImportDTO.LongitudeValue = worksheet.Cells[i + StartRow, LongitudeColumn].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(Store_ImportDTO.LongitudeValue) && Store_ImportDTO.LongitudeValue.Contains(","))
                        Store_ImportDTO.LongitudeValue = Store_ImportDTO.LongitudeValue.Replace(",", ".");
                    Store_ImportDTO.LatitudeValue = worksheet.Cells[i + StartRow, LatitudeColumn].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(Store_ImportDTO.LatitudeValue) && Store_ImportDTO.LatitudeValue.Contains(","))
                        Store_ImportDTO.LatitudeValue = Store_ImportDTO.LatitudeValue.Replace(",", ".");
                    Store_ImportDTO.DeliveryAddressValue = worksheet.Cells[i + StartRow, DeliveryAddressColumn].Value?.ToString();
                    Store_ImportDTO.DeliveryLongitudeValue = worksheet.Cells[i + StartRow, DeliveryLongitudeColumn].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(Store_ImportDTO.DeliveryLongitudeValue) && Store_ImportDTO.DeliveryLongitudeValue.Contains(","))
                        Store_ImportDTO.DeliveryLongitudeValue = Store_ImportDTO.DeliveryLongitudeValue.Replace(",", ".");
                    Store_ImportDTO.DeliveryLatitudeValue = worksheet.Cells[i + StartRow, DeliveryLatitudeColumn].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(Store_ImportDTO.DeliveryLatitudeValue) && Store_ImportDTO.DeliveryLatitudeValue.Contains(","))
                        Store_ImportDTO.DeliveryLatitudeValue = Store_ImportDTO.DeliveryLatitudeValue.Replace(",", ".");
                    Store_ImportDTO.TelephoneValue = worksheet.Cells[i + StartRow, TelephoneColumn].Value?.ToString();
                    Store_ImportDTO.OwnerNameValue = worksheet.Cells[i + StartRow, OwnerNameColumn].Value?.ToString();
                    Store_ImportDTO.OwnerPhoneValue = worksheet.Cells[i + StartRow, OwnerPhoneColumn].Value?.ToString();
                    Store_ImportDTO.OwnerEmailValue = worksheet.Cells[i + StartRow, OwnerEmailColumn].Value?.ToString();
                    Store_ImportDTO.StoreStatusNameValue = worksheet.Cells[i + StartRow, StoreStatusColumn].Value?.ToString();
                    Store_ImportDTO.StatusNameValue = worksheet.Cells[i + StartRow, StatusColumn].Value?.ToString();
                    #endregion
                }
            }
            // Filter Ids by CodeValue from DTO
            Parallel.ForEach(Store_ImportDTOs, Store_ImportDTO =>
            {
                if (!string.IsNullOrWhiteSpace(Store_ImportDTO.CodeValue) && AllIds.Contains(Store_ImportDTO.CodeValue))
                {
                    Store_ImportDTO.IsNew = false;
                }
                else Store_ImportDTO.IsNew = true;
                Store_ImportDTO.OrganizationId = Organizations.Where(x => x.Code.Equals(Store_ImportDTO.OrganizationCodeValue)).Select(x => x.Id).FirstOrDefault();
                Store_ImportDTO.Longitude = decimal.TryParse(Store_ImportDTO.LongitudeValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal Longitude) ? Longitude : 106;
                Store_ImportDTO.Latitude = decimal.TryParse(Store_ImportDTO.LatitudeValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal Latitude) ? Latitude : 21;
                Store_ImportDTO.DeliveryLongitude = decimal.TryParse(Store_ImportDTO.DeliveryLongitudeValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal DeliveryLongitude) ? DeliveryLongitude : 106;
                Store_ImportDTO.DeliveryLatitude = decimal.TryParse(Store_ImportDTO.DeliveryLatitudeValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal DeliveryLatitude) ? DeliveryLatitude : 21;
                if (!string.IsNullOrWhiteSpace(Store_ImportDTO.StoreTypeCodeValue))
                    Store_ImportDTO.StoreTypeId = StoreTypes.Where(x => x.Code.Equals(Store_ImportDTO.StoreTypeCodeValue)).Select(x => x.Id).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(Store_ImportDTO.StoreGroupingCodeValue))
                {
                    List<string> Codes = Store_ImportDTO.StoreGroupingCodeValue.Split(";").ToList();
                    Store_ImportDTO.StoreGroupingIds = new List<long>();
                    foreach (string Code in Codes)
                    {
                        long StoreGroupingId = StoreGroupings.Where(x => x.Code.Trim().Equals(Code.Trim())).Select(x => x.Id).FirstOrDefault();
                        Store_ImportDTO.StoreGroupingIds.Add(StoreGroupingId);
                    }
                }
                if (!string.IsNullOrWhiteSpace(Store_ImportDTO.ProvinceCodeValue))
                    Store_ImportDTO.ProvinceId = Provinces.Where(x => x.Code.Equals(Store_ImportDTO.ProvinceCodeValue)).Select(x => (long?)x.Id).FirstOrDefault() == null ?
                    -1 : Provinces.Where(x => x.Code.Equals(Store_ImportDTO.ProvinceCodeValue)).Select(x => (long?)x.Id).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(Store_ImportDTO.DistrictCodeValue))
                    Store_ImportDTO.DistrictId = Districts.Where(x => x.Code.Equals(Store_ImportDTO.DistrictCodeValue)).Select(x => (long?)x.Id).FirstOrDefault() == null ?
                    -1 : Districts.Where(x => x.Code.Equals(Store_ImportDTO.DistrictCodeValue)).Select(x => (long?)x.Id).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(Store_ImportDTO.WardCodeValue))
                    Store_ImportDTO.WardId = Wards.Where(x => x.Code.Equals(Store_ImportDTO.WardCodeValue)).Select(x => (long?)x.Id).FirstOrDefault() == null ?
                    -1 : Wards.Where(x => x.Code.Equals(Store_ImportDTO.WardCodeValue)).Select(x => (long?)x.Id).FirstOrDefault();
                if (string.IsNullOrEmpty(Store_ImportDTO.StoreStatusNameValue))
                {
                    Store_ImportDTO.StoreStatusId = -1;
                }
                else
                {
                    string StoreStatusNameValue = Store_ImportDTO.StoreStatusNameValue;
                    Store_ImportDTO.StoreStatusId = StoreStatuses.Where(x => x.Name.ToLower().Equals(StoreStatusNameValue == null ? string.Empty : StoreStatusNameValue.Trim().ToLower())).Select(x => x.Id).FirstOrDefault();
                }
                if (string.IsNullOrEmpty(Store_ImportDTO.StatusNameValue))
                {
                    Store_ImportDTO.StatusId = -1;
                }
                else
                {
                    string StatusNameValue = Store_ImportDTO.StatusNameValue;
                    Store_ImportDTO.StatusId = Statuses.Where(x => x.Name.ToLower().Equals(StatusNameValue == null ? string.Empty : StatusNameValue.Trim().ToLower())).Select(x => x.Id).FirstOrDefault();
                }
            });
            if (Store_ImportDTOs.Count < 1)
            {
                return BadRequest("Chưa nhập STT");
            }
            // Convert DTO to Entity
            Dictionary<long, Store> DictionaryStores = Store_ImportDTOs.ToDictionary(x => x.Stt, y => new Store());
            Parallel.ForEach(Store_ImportDTOs, Store_ImportDTO =>
            {
                Store Store = DictionaryStores[Store_ImportDTO.Stt];
                if (Store_ImportDTO.IsNew == false)
                {
                    Store Old = DictionaryAll[Store_ImportDTO.CodeValue];
                    Store.Id = Old.Id;
                    Store.ParentStoreId = Old.ParentStoreId;
                    Store.CreatorId = Old.Creator.Id;
                    Store.CreatedAt = Old.CreatedAt;
                }
                else
                {
                    Store.CreatorId = CurrentContext.UserId;
                }
                Store.Code = Store_ImportDTO.CodeValue;
                Store.CodeDraft = Store_ImportDTO.CodeDraftValue;
                Store.Name = Store_ImportDTO.NameValue;
                Store.OrganizationId = Store_ImportDTO.OrganizationId;
                Store.Organization = new Organization { Code = Store_ImportDTO.OrganizationCodeValue };
                Store.ParentStore = new Store { Code = Store_ImportDTO.ParentStoreCodeValue };
                Store.StoreTypeId = Store_ImportDTO.StoreTypeId;
                Store.StoreType = new StoreType { Code = Store_ImportDTO.StoreTypeCodeValue };
                Store.StoreStoreGroupingMappings = Store_ImportDTO.StoreGroupingIds?.Select(x => new StoreStoreGroupingMapping
                {
                    StoreGroupingId = x
                }).ToList();
                Store.LegalEntity = Store_ImportDTO.LegalEntityValue;
                Store.TaxCode = Store_ImportDTO.TaxCodeValue;
                Store.ProvinceId = Store_ImportDTO.ProvinceId;
                if (Store_ImportDTO.ProvinceId.HasValue)
                {
                    Store.Province = new Province { Code = Store_ImportDTO.ProvinceCodeValue };
                }
                Store.DistrictId = Store_ImportDTO.DistrictId;
                if (Store_ImportDTO.DistrictId.HasValue)
                {
                    Store.District = new District { Code = Store_ImportDTO.DistrictCodeValue };
                }
                Store.WardId = Store_ImportDTO.WardId;
                if (Store_ImportDTO.WardId.HasValue)
                {
                    Store.Ward = new Ward { Code = Store_ImportDTO.WardCodeValue };
                }
                Store.Address = Store_ImportDTO.AddressValue;
                Store.Longitude = Store_ImportDTO.Longitude;
                Store.Latitude = Store_ImportDTO.Latitude;
                Store.DeliveryAddress = Store_ImportDTO.DeliveryAddressValue;
                Store.DeliveryLongitude = Store_ImportDTO.DeliveryLongitude;
                Store.DeliveryLatitude = Store_ImportDTO.DeliveryLatitude;
                Store.Telephone = Store_ImportDTO.TelephoneValue;
                Store.OwnerName = Store_ImportDTO.OwnerNameValue;
                Store.OwnerPhone = Store_ImportDTO.OwnerPhoneValue;
                Store.OwnerEmail = Store_ImportDTO.OwnerEmailValue;
                Store.StoreStatusId = Store_ImportDTO.StoreStatusId;
                Store.StatusId = Store_ImportDTO.StatusId;
                Store.BaseLanguage = CurrentContext.Language;
            });
            List<Store> Stores = DictionaryStores.Select(x => x.Value).ToList();
            StringBuilder errorContent = new StringBuilder();
            Stores = await StoreService.Import(Stores);
            if (Stores == null)
                return Ok();
            List<Store_StoreDTO> Store_StoreDTOs = Stores
                .Select(c => new Store_StoreDTO(c)).ToList();
            // Get error content from all of Errors of Stores
            for (int i = 0; i < Stores.Count; i++)
            {
                if (!Stores[i].IsValidated)
                {
                    errorContent.Append($"Lỗi dòng thứ {i + 1}:");
                    foreach (var Error in Stores[i].Errors)
                    {
                        errorContent.Append($" {Error.Value},");
                    }
                }
                errorContent.AppendLine("");
            }
            if (Stores.Any(s => !s.IsValidated))
                return BadRequest(errorContent.ToString());
            return Ok(Store_StoreDTOs);
        }
        [Route(StoreRoute.ImportBrand), HttpPost]
        public async Task<ActionResult<bool>> ImportBrand(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            FileInfo FileInfo = new FileInfo(file.FileName);
            if (!FileInfo.Extension.Equals(".xlsx"))
                return BadRequest("Định dạng file không hợp lệ");
            DataFile DataFile = new DataFile
            {
                Name = file.FileName,
                Content = file.OpenReadStream(),
            };
            #region MDM
            List<Brand> Brands = await BrandService.List(new BrandFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = BrandSelect.ALL
            });
            List<ProductGrouping> ProductGroupings = await ProductGroupingService.List(new ProductGroupingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProductGroupingSelect.ALL
            });
            List<Store> All = await StoreService.Export(new StoreFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.Name | StoreSelect.CodeDraft
            });
            HashSet<string> StoreCodes = new HashSet<string>(All.Select(x => x.Code).Distinct().ToList());
            Dictionary<string, Store> DictionaryStore = All.ToDictionary(x => x.Code, y => y);
            Dictionary<string, Brand> DictionaryBrand = Brands.ToDictionary(x => x.Code.ToLower(), y => y);
            Dictionary<string, ProductGrouping> DictionaryProductGrouping = ProductGroupings.ToDictionary(x => x.Code, y => y);
            #endregion
            List<Store_BrandInStoreImportDTO> Store_BrandInStoreImportDTOs = new List<Store_BrandInStoreImportDTO>();
            StringBuilder errorContent = new StringBuilder();
            using (ExcelPackage excelPackage = new ExcelPackage(DataFile.Content))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    return BadRequest("File không đúng biểu mẫu import");
                #region khai báo các cột
                int StartColumn = 1;
                int StartRow = 4;
                int SttColumnn = 0 + StartColumn;
                int StoreCodeColumn = 1 + StartColumn;
                int BrandCode1Column = 3 + StartColumn;
                int ProductGrouping1Column = 4 + StartColumn;
                int BrandCode2Column = 5 + StartColumn;
                int ProductGrouping2Column = 6 + StartColumn;
                int BrandCode3Column = 7 + StartColumn;
                int ProductGrouping3Column = 8 + StartColumn;
                int BrandCode4Column = 9 + StartColumn;
                int ProductGrouping4Column = 10 + StartColumn;
                int BrandCode5Column = 11 + StartColumn;
                int ProductGrouping5Column = 12 + StartColumn;
                #endregion
                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    #region Lấy thông tin từng dòng
                    string stt = worksheet.Cells[i, SttColumnn].Value?.ToString();
                    if (stt != null && stt.ToLower() == "END".ToLower())
                        break;
                    bool convert = long.TryParse(stt, out long Stt);
                    if (convert == false)
                        continue;
                    Store_BrandInStoreImportDTO Store_BrandInStoreImportDTO = new Store_BrandInStoreImportDTO();
                    Store_BrandInStoreImportDTOs.Add(Store_BrandInStoreImportDTO);
                    Store_BrandInStoreImportDTO.STT = long.Parse(stt);
                    Store_BrandInStoreImportDTO.StoreCode = worksheet.Cells[i, StoreCodeColumn].Value?.ToString();
                    Store_BrandInStoreImportDTO.BrandCode1 = worksheet.Cells[i, BrandCode1Column].Value?.ToString().ToLower();
                    Store_BrandInStoreImportDTO.ProductGrouping1 = worksheet.Cells[i, ProductGrouping1Column].Value?.ToString();
                    Store_BrandInStoreImportDTO.BrandCode2 = worksheet.Cells[i, BrandCode2Column].Value?.ToString().ToLower();
                    Store_BrandInStoreImportDTO.ProductGrouping2 = worksheet.Cells[i, ProductGrouping2Column].Value?.ToString();
                    Store_BrandInStoreImportDTO.BrandCode3 = worksheet.Cells[i, BrandCode3Column].Value?.ToString().ToLower();
                    Store_BrandInStoreImportDTO.ProductGrouping3 = worksheet.Cells[i, ProductGrouping3Column].Value?.ToString();
                    Store_BrandInStoreImportDTO.BrandCode4 = worksheet.Cells[i, BrandCode4Column].Value?.ToString().ToLower();
                    Store_BrandInStoreImportDTO.ProductGrouping4 = worksheet.Cells[i, ProductGrouping4Column].Value?.ToString();
                    Store_BrandInStoreImportDTO.BrandCode5 = worksheet.Cells[i, BrandCode5Column].Value?.ToString().ToLower();
                    Store_BrandInStoreImportDTO.ProductGrouping5 = worksheet.Cells[i, ProductGrouping5Column].Value?.ToString();
                    #endregion
                    if (!string.IsNullOrWhiteSpace(Store_BrandInStoreImportDTO.StoreCode))
                    {
                        if (!StoreCodes.Contains(Store_BrandInStoreImportDTO.StoreCode))
                        {
                            errorContent.AppendLine($"Lỗi dòng thứ {i}: Mã đại lý không tồn tại");
                            continue;
                        }
                    }
                    Store_BrandInStoreImportDTO.StoreCode = Store_BrandInStoreImportDTO.StoreCode.Trim();
                    Store_BrandInStoreImportDTO.StoreId = DictionaryStore[Store_BrandInStoreImportDTO.StoreCode].Id;
                    if (!string.IsNullOrWhiteSpace(Store_BrandInStoreImportDTO.BrandCode1) && Store_BrandInStoreImportDTO.BrandCode1 != "0")
                    {
                        Store_BrandInStoreImportDTO.BrandCode1 = Store_BrandInStoreImportDTO.BrandCode1.Trim();
                        Store_BrandInStoreImportDTO.Brand1Id = DictionaryBrand[Store_BrandInStoreImportDTO.BrandCode1].Id;
                        if (!string.IsNullOrWhiteSpace(Store_BrandInStoreImportDTO.ProductGrouping1) && Store_BrandInStoreImportDTO.ProductGrouping1 != "0")
                        {
                            Store_BrandInStoreImportDTO.ProductGrouping1Ids = new List<long>();
                            var ProductGroupingCodes = Store_BrandInStoreImportDTO.ProductGrouping1.Split(';');
                            foreach (var ProductGroupingCode in ProductGroupingCodes)
                            {
                                var ProductGroupingId = DictionaryProductGrouping[ProductGroupingCode.Trim()].Id;
                                Store_BrandInStoreImportDTO.ProductGrouping1Ids.Add(ProductGroupingId);
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(Store_BrandInStoreImportDTO.BrandCode2) && Store_BrandInStoreImportDTO.BrandCode2 != "0")
                    {
                        Store_BrandInStoreImportDTO.BrandCode2 = Store_BrandInStoreImportDTO.BrandCode2.Trim();
                        Store_BrandInStoreImportDTO.Brand2Id = DictionaryBrand[Store_BrandInStoreImportDTO.BrandCode2].Id;
                        if (!string.IsNullOrWhiteSpace(Store_BrandInStoreImportDTO.ProductGrouping2) && Store_BrandInStoreImportDTO.ProductGrouping2 != "0")
                        {
                            Store_BrandInStoreImportDTO.ProductGrouping2Ids = new List<long>();
                            var ProductGroupingCodes = Store_BrandInStoreImportDTO.ProductGrouping2.Split(';');
                            foreach (var ProductGroupingCode in ProductGroupingCodes)
                            {
                                var ProductGroupingId = DictionaryProductGrouping[ProductGroupingCode.Trim()].Id;
                                Store_BrandInStoreImportDTO.ProductGrouping2Ids.Add(ProductGroupingId);
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(Store_BrandInStoreImportDTO.BrandCode3) && Store_BrandInStoreImportDTO.BrandCode3 != "0")
                    {
                        Store_BrandInStoreImportDTO.BrandCode3 = Store_BrandInStoreImportDTO.BrandCode3.Trim();
                        Store_BrandInStoreImportDTO.Brand3Id = DictionaryBrand[Store_BrandInStoreImportDTO.BrandCode3].Id;
                        if (!string.IsNullOrWhiteSpace(Store_BrandInStoreImportDTO.ProductGrouping3) && Store_BrandInStoreImportDTO.ProductGrouping3 != "0")
                        {
                            Store_BrandInStoreImportDTO.ProductGrouping3Ids = new List<long>();
                            var ProductGroupingCodes = Store_BrandInStoreImportDTO.ProductGrouping3.Split(';');
                            foreach (var ProductGroupingCode in ProductGroupingCodes)
                            {
                                var ProductGroupingId = DictionaryProductGrouping[ProductGroupingCode.Trim()].Id;
                                Store_BrandInStoreImportDTO.ProductGrouping3Ids.Add(ProductGroupingId);
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(Store_BrandInStoreImportDTO.BrandCode4) && Store_BrandInStoreImportDTO.BrandCode4 != "0")
                    {
                        Store_BrandInStoreImportDTO.BrandCode4 = Store_BrandInStoreImportDTO.BrandCode4.Trim();
                        Store_BrandInStoreImportDTO.Brand4Id = DictionaryBrand[Store_BrandInStoreImportDTO.BrandCode4].Id;
                        if (!string.IsNullOrWhiteSpace(Store_BrandInStoreImportDTO.ProductGrouping4) && Store_BrandInStoreImportDTO.ProductGrouping4 != "0")
                        {
                            Store_BrandInStoreImportDTO.ProductGrouping4Ids = new List<long>();
                            var ProductGroupingCodes = Store_BrandInStoreImportDTO.ProductGrouping4.Split(';');
                            foreach (var ProductGroupingCode in ProductGroupingCodes)
                            {
                                var ProductGroupingId = DictionaryProductGrouping[ProductGroupingCode.Trim()].Id;
                                Store_BrandInStoreImportDTO.ProductGrouping4Ids.Add(ProductGroupingId);
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(Store_BrandInStoreImportDTO.BrandCode5) && Store_BrandInStoreImportDTO.BrandCode5 != "0")
                    {
                        Store_BrandInStoreImportDTO.BrandCode5 = Store_BrandInStoreImportDTO.BrandCode5.Trim();
                        Store_BrandInStoreImportDTO.Brand5Id = DictionaryBrand[Store_BrandInStoreImportDTO.BrandCode5].Id;
                        if (!string.IsNullOrWhiteSpace(Store_BrandInStoreImportDTO.ProductGrouping5) && Store_BrandInStoreImportDTO.ProductGrouping5 != "0")
                        {
                            Store_BrandInStoreImportDTO.ProductGrouping5Ids = new List<long>();
                            var ProductGroupingCodes = Store_BrandInStoreImportDTO.ProductGrouping5.Split(';');
                            foreach (var ProductGroupingCode in ProductGroupingCodes)
                            {
                                var ProductGroupingId = DictionaryProductGrouping[ProductGroupingCode.Trim()].Id;
                                Store_BrandInStoreImportDTO.ProductGrouping5Ids.Add(ProductGroupingId);
                            }
                        }
                    }
                }
            }
            if (errorContent.Length > 0)
                return BadRequest(errorContent.ToString());
            try
            {
                List<BrandInStoreDAO> BrandInStoreDAOs = new List<BrandInStoreDAO>();
                foreach (var Store_BrandInStoreImportDTO in Store_BrandInStoreImportDTOs)
                {
                    await DataContext.BrandInStore.Where(x => x.Id == Store_BrandInStoreImportDTO.StoreId).UpdateFromQueryAsync(x => new BrandInStoreDAO { DeletedAt = StaticParams.DateTimeNow });
                    if (Store_BrandInStoreImportDTO.Brand1Id.HasValue)
                    {
                        BrandInStoreDAO BrandInStore1 = new BrandInStoreDAO()
                        {
                            BrandId = Store_BrandInStoreImportDTO.Brand1Id.Value,
                            StoreId = Store_BrandInStoreImportDTO.StoreId,
                            Top = 1,
                            CreatorId = CurrentContext.UserId,
                            CreatedAt = StaticParams.DateTimeNow,
                            UpdatedAt = StaticParams.DateTimeNow,
                            RowId = Guid.NewGuid()
                        };
                        BrandInStoreDAOs.Add(BrandInStore1);
                    }
                    if (Store_BrandInStoreImportDTO.Brand2Id.HasValue)
                    {
                        BrandInStoreDAO BrandInStore2 = new BrandInStoreDAO()
                        {
                            BrandId = Store_BrandInStoreImportDTO.Brand2Id.Value,
                            StoreId = Store_BrandInStoreImportDTO.StoreId,
                            Top = 2,
                            CreatorId = CurrentContext.UserId,
                            CreatedAt = StaticParams.DateTimeNow,
                            UpdatedAt = StaticParams.DateTimeNow,
                            RowId = Guid.NewGuid()
                        };
                        BrandInStoreDAOs.Add(BrandInStore2);
                    }
                    if (Store_BrandInStoreImportDTO.Brand3Id.HasValue)
                    {
                        BrandInStoreDAO BrandInStore3 = new BrandInStoreDAO()
                        {
                            BrandId = Store_BrandInStoreImportDTO.Brand3Id.Value,
                            StoreId = Store_BrandInStoreImportDTO.StoreId,
                            Top = 3,
                            CreatorId = CurrentContext.UserId,
                            CreatedAt = StaticParams.DateTimeNow,
                            UpdatedAt = StaticParams.DateTimeNow,
                            RowId = Guid.NewGuid()
                        };
                        BrandInStoreDAOs.Add(BrandInStore3);
                    }
                    if (Store_BrandInStoreImportDTO.Brand4Id.HasValue)
                    {
                        BrandInStoreDAO BrandInStore4 = new BrandInStoreDAO()
                        {
                            BrandId = Store_BrandInStoreImportDTO.Brand4Id.Value,
                            StoreId = Store_BrandInStoreImportDTO.StoreId,
                            Top = 4,
                            CreatorId = CurrentContext.UserId,
                            CreatedAt = StaticParams.DateTimeNow,
                            UpdatedAt = StaticParams.DateTimeNow,
                            RowId = Guid.NewGuid()
                        };
                        BrandInStoreDAOs.Add(BrandInStore4);
                    }
                    if (Store_BrandInStoreImportDTO.Brand5Id.HasValue)
                    {
                        BrandInStoreDAO BrandInStore5 = new BrandInStoreDAO()
                        {
                            BrandId = Store_BrandInStoreImportDTO.Brand5Id.Value,
                            StoreId = Store_BrandInStoreImportDTO.StoreId,
                            Top = 5,
                            CreatorId = CurrentContext.UserId,
                            CreatedAt = StaticParams.DateTimeNow,
                            UpdatedAt = StaticParams.DateTimeNow,
                            RowId = Guid.NewGuid()
                        };
                        BrandInStoreDAOs.Add(BrandInStore5);
                    }
                }
                await DataContext.BulkMergeAsync(BrandInStoreDAOs);
                List<BrandInStoreProductGroupingMappingDAO> BrandInStoreProductGroupingMappingDAOs = new List<BrandInStoreProductGroupingMappingDAO>();
                foreach (var Store_BrandInStoreImportDTO in Store_BrandInStoreImportDTOs)
                {
                    if (Store_BrandInStoreImportDTO.Brand1Id.HasValue)
                    {
                        if (Store_BrandInStoreImportDTO.ProductGrouping1Ids != null)
                        {
                            var BrandInStoreDAO = BrandInStoreDAOs.Where(x => x.BrandId == Store_BrandInStoreImportDTO.Brand1Id.Value && x.StoreId == Store_BrandInStoreImportDTO.StoreId).FirstOrDefault();
                            if (BrandInStoreDAO != null)
                            {
                                foreach (var ProductGrouping1Id in Store_BrandInStoreImportDTO.ProductGrouping1Ids)
                                {
                                    BrandInStoreProductGroupingMappingDAO BrandInStoreProductGroupingMappingDAO = new BrandInStoreProductGroupingMappingDAO
                                    {
                                        BrandInStoreId = BrandInStoreDAO.Id,
                                        ProductGroupingId = ProductGrouping1Id
                                    };
                                    BrandInStoreProductGroupingMappingDAOs.Add(BrandInStoreProductGroupingMappingDAO);
                                }
                            }
                        }
                    }
                    if (Store_BrandInStoreImportDTO.Brand2Id.HasValue)
                    {
                        if (Store_BrandInStoreImportDTO.ProductGrouping2Ids != null)
                        {
                            var BrandInStoreDAO = BrandInStoreDAOs.Where(x => x.BrandId == Store_BrandInStoreImportDTO.Brand2Id.Value && x.StoreId == Store_BrandInStoreImportDTO.StoreId).FirstOrDefault();
                            if (BrandInStoreDAO != null)
                            {
                                foreach (var ProductGrouping2Id in Store_BrandInStoreImportDTO.ProductGrouping2Ids)
                                {
                                    BrandInStoreProductGroupingMappingDAO BrandInStoreProductGroupingMappingDAO = new BrandInStoreProductGroupingMappingDAO
                                    {
                                        BrandInStoreId = BrandInStoreDAO.Id,
                                        ProductGroupingId = ProductGrouping2Id
                                    };
                                    BrandInStoreProductGroupingMappingDAOs.Add(BrandInStoreProductGroupingMappingDAO);
                                }
                            }
                        }
                    }
                    if (Store_BrandInStoreImportDTO.Brand3Id.HasValue)
                    {
                        if (Store_BrandInStoreImportDTO.ProductGrouping3Ids != null)
                        {
                            var BrandInStoreDAO = BrandInStoreDAOs.Where(x => x.BrandId == Store_BrandInStoreImportDTO.Brand3Id.Value && x.StoreId == Store_BrandInStoreImportDTO.StoreId).FirstOrDefault();
                            if (BrandInStoreDAO != null)
                            {
                                foreach (var ProductGrouping3Id in Store_BrandInStoreImportDTO.ProductGrouping3Ids)
                                {
                                    BrandInStoreProductGroupingMappingDAO BrandInStoreProductGroupingMappingDAO = new BrandInStoreProductGroupingMappingDAO
                                    {
                                        BrandInStoreId = BrandInStoreDAO.Id,
                                        ProductGroupingId = ProductGrouping3Id
                                    };
                                    BrandInStoreProductGroupingMappingDAOs.Add(BrandInStoreProductGroupingMappingDAO);
                                }
                            }
                        }
                    }
                    if (Store_BrandInStoreImportDTO.Brand4Id.HasValue)
                    {
                        if (Store_BrandInStoreImportDTO.ProductGrouping4Ids != null)
                        {
                            var BrandInStoreDAO = BrandInStoreDAOs.Where(x => x.BrandId == Store_BrandInStoreImportDTO.Brand4Id.Value && x.StoreId == Store_BrandInStoreImportDTO.StoreId).FirstOrDefault();
                            if (BrandInStoreDAO != null)
                            {
                                foreach (var ProductGrouping4Id in Store_BrandInStoreImportDTO.ProductGrouping4Ids)
                                {
                                    BrandInStoreProductGroupingMappingDAO BrandInStoreProductGroupingMappingDAO = new BrandInStoreProductGroupingMappingDAO
                                    {
                                        BrandInStoreId = BrandInStoreDAO.Id,
                                        ProductGroupingId = ProductGrouping4Id
                                    };
                                    BrandInStoreProductGroupingMappingDAOs.Add(BrandInStoreProductGroupingMappingDAO);
                                }
                            }
                        }
                    }
                    if (Store_BrandInStoreImportDTO.Brand5Id.HasValue)
                    {
                        if (Store_BrandInStoreImportDTO.ProductGrouping5Ids != null)
                        {
                            var BrandInStoreDAO = BrandInStoreDAOs.Where(x => x.BrandId == Store_BrandInStoreImportDTO.Brand5Id.Value && x.StoreId == Store_BrandInStoreImportDTO.StoreId).FirstOrDefault();
                            if (BrandInStoreDAO != null)
                            {
                                foreach (var ProductGrouping5Id in Store_BrandInStoreImportDTO.ProductGrouping5Ids)
                                {
                                    BrandInStoreProductGroupingMappingDAO BrandInStoreProductGroupingMappingDAO = new BrandInStoreProductGroupingMappingDAO
                                    {
                                        BrandInStoreId = BrandInStoreDAO.Id,
                                        ProductGroupingId = ProductGrouping5Id
                                    };
                                    BrandInStoreProductGroupingMappingDAOs.Add(BrandInStoreProductGroupingMappingDAO);
                                }
                            }
                        }
                    }
                }
                List<BrandInStoreProductGroupingMappingDAO> NewList = new List<BrandInStoreProductGroupingMappingDAO>();
                foreach (BrandInStoreProductGroupingMappingDAO BrandInStoreProductGroupingMappingDAO in BrandInStoreProductGroupingMappingDAOs)
                {
                    BrandInStoreProductGroupingMappingDAO Old = NewList
                        .Where(x => x.BrandInStoreId == BrandInStoreProductGroupingMappingDAO.BrandInStoreId && x.ProductGroupingId == BrandInStoreProductGroupingMappingDAO.ProductGroupingId)
                        .FirstOrDefault();
                    if (Old == null)
                    {
                        NewList.Add(BrandInStoreProductGroupingMappingDAO);
                    }
                }
                await DataContext.BulkMergeAsync(NewList);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route(StoreRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] Store_StoreFilterDTO Store_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();
            StoreFilter StoreFilter = ConvertFilterDTOToFilterEntity(Store_StoreFilterDTO);
            StoreFilter = StoreService.ToFilter(StoreFilter);
            StoreFilter.Skip = 0;
            StoreFilter.Take = int.MaxValue;
            StoreFilter.Selects = StoreSelect.ALL;
            List<Store> Stores = await StoreService.Export(StoreFilter);
            List<Store_StoreExportDTO> Store_StoreExportDTOs = Stores.Select(x => new Store_StoreExportDTO(x)).ToList();
            long stt = 1;
            foreach (var Store_StoreExportDTO in Store_StoreExportDTOs)
            {
                Store_StoreExportDTO.STT = stt++;
            }
            foreach (var Store_StoreExportDTO in Store_StoreExportDTOs)
            {
                Store_StoreExportDTO.BrandInStoreTop1 = Store_StoreExportDTO.BrandInStores.Where(x => x.Top == 1).FirstOrDefault();
                Store_StoreExportDTO.BrandInStoreTop2 = Store_StoreExportDTO.BrandInStores.Where(x => x.Top == 2).FirstOrDefault();
                Store_StoreExportDTO.BrandInStoreTop3 = Store_StoreExportDTO.BrandInStores.Where(x => x.Top == 3).FirstOrDefault();
                Store_StoreExportDTO.BrandInStoreTop4 = Store_StoreExportDTO.BrandInStores.Where(x => x.Top == 4).FirstOrDefault();
                Store_StoreExportDTO.BrandInStoreTop5 = Store_StoreExportDTO.BrandInStores.Where(x => x.Top == 5).FirstOrDefault();
                Store_StoreExportDTO.CreateAtString = Store_StoreExportDTO.CreatedAt.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
                if (Store_StoreExportDTO.BrandInStoreTop1 != null)
                {
                    var ProductGroupings = Store_StoreExportDTO.BrandInStoreTop1.BrandInStoreProductGroupingMappings?.Select(x => x.ProductGrouping).ToList();
                    var ProductGroupingNames = ProductGroupings.Select(x => x.Name).ToList();
                    Store_StoreExportDTO.BrandInStoreTop1.ProductGroupings = string.Join(';', ProductGroupingNames);
                    var ShowingCategories = Store_StoreExportDTO.BrandInStoreTop1.BrandInStoreShowingCategoryMappings?.Select(x => x.ShowingCategory).ToList();
                    if (ShowingCategories != null)
                    {
                        var ShowingCategoryNames = ShowingCategories.Select(x => x.Name).ToList();
                        Store_StoreExportDTO.BrandInStoreTop1.ShowingCategories = string.Join(';', ShowingCategoryNames);
                    }
                }
                if (Store_StoreExportDTO.BrandInStoreTop2 != null)
                {
                    var ProductGroupings = Store_StoreExportDTO.BrandInStoreTop2.BrandInStoreProductGroupingMappings?.Select(x => x.ProductGrouping).ToList();
                    var ProductGroupingNames = ProductGroupings.Select(x => x.Name).ToList();
                    Store_StoreExportDTO.BrandInStoreTop2.ProductGroupings = string.Join(';', ProductGroupingNames);
                    var ShowingCategories = Store_StoreExportDTO.BrandInStoreTop2.BrandInStoreShowingCategoryMappings?.Select(x => x.ShowingCategory).ToList();
                    if (ShowingCategories != null)
                    {
                        var ShowingCategoryNames = ShowingCategories.Select(x => x.Name).ToList();
                        Store_StoreExportDTO.BrandInStoreTop2.ShowingCategories = string.Join(';', ShowingCategoryNames);
                    }
                }
                if (Store_StoreExportDTO.BrandInStoreTop3 != null)
                {
                    var ProductGroupings = Store_StoreExportDTO.BrandInStoreTop3.BrandInStoreProductGroupingMappings?.Select(x => x.ProductGrouping).ToList();
                    var ProductGroupingNames = ProductGroupings.Select(x => x.Name).ToList();
                    Store_StoreExportDTO.BrandInStoreTop3.ProductGroupings = string.Join(';', ProductGroupingNames);
                    var ShowingCategories = Store_StoreExportDTO.BrandInStoreTop3.BrandInStoreShowingCategoryMappings?.Select(x => x.ShowingCategory).ToList();
                    if (ShowingCategories != null)
                    {
                        var ShowingCategoryNames = ShowingCategories.Select(x => x.Name).ToList();
                        Store_StoreExportDTO.BrandInStoreTop3.ShowingCategories = string.Join(';', ShowingCategoryNames);
                    }
                }
                if (Store_StoreExportDTO.BrandInStoreTop4 != null)
                {
                    var ProductGroupings = Store_StoreExportDTO.BrandInStoreTop4.BrandInStoreProductGroupingMappings?.Select(x => x.ProductGrouping).ToList();
                    var ProductGroupingNames = ProductGroupings.Select(x => x.Name).ToList();
                    Store_StoreExportDTO.BrandInStoreTop4.ProductGroupings = string.Join(';', ProductGroupingNames);
                    var ShowingCategories = Store_StoreExportDTO.BrandInStoreTop4.BrandInStoreShowingCategoryMappings?.Select(x => x.ShowingCategory).ToList();
                    if (ShowingCategories != null)
                    {
                        var ShowingCategoryNames = ShowingCategories.Select(x => x.Name).ToList();
                        Store_StoreExportDTO.BrandInStoreTop4.ShowingCategories = string.Join(';', ShowingCategoryNames);
                    }
                }
                if (Store_StoreExportDTO.BrandInStoreTop5 != null)
                {
                    var ProductGroupings = Store_StoreExportDTO.BrandInStoreTop5.BrandInStoreProductGroupingMappings?.Select(x => x.ProductGrouping).ToList();
                    var ProductGroupingNames = ProductGroupings.Select(x => x.Name).ToList();
                    Store_StoreExportDTO.BrandInStoreTop5.ProductGroupings = string.Join(';', ProductGroupingNames);
                    var ShowingCategories = Store_StoreExportDTO.BrandInStoreTop5.BrandInStoreShowingCategoryMappings?.Select(x => x.ShowingCategory).ToList();
                    if (ShowingCategories != null)
                    {
                        var ShowingCategoryNames = ShowingCategories.Select(x => x.Name).ToList();
                        Store_StoreExportDTO.BrandInStoreTop5.ShowingCategories = string.Join(';', ShowingCategoryNames);
                    }
                }
            };
            Parallel.ForEach(Store_StoreExportDTOs, Store_StoreExportDTO =>
            {
                if (Store_StoreExportDTO.StoreStoreGroupingMappings != null)
                {
                    var StoreGroupingNames = Store_StoreExportDTO.StoreStoreGroupingMappings.Select(x => x.StoreGrouping.Name).ToList();
                    Store_StoreExportDTO.StoreGroupingName = string.Join(';', StoreGroupingNames);
                }
            });
            Parallel.ForEach(Store_StoreExportDTOs, Store_StoreExportDTO =>
            {
                if (Store_StoreExportDTO.AppUserStoreMappings != null)
                {
                    var AppUserNames = Store_StoreExportDTO.AppUserStoreMappings.Select(x => x.AppUser.DisplayName).ToList();
                    Store_StoreExportDTO.AppUserName = string.Join(';', AppUserNames);
                }
            });
            List<ShowingCategory> DbShowingCategories = await ShowingCategoryService.List(new ShowingCategoryFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ShowingCategorySelect.Id | ShowingCategorySelect.Code | ShowingCategorySelect.Name,
            });
            string path = "Templates/Store_Export.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Data = Store_StoreExportDTOs;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            Data.ShowingCategories = DbShowingCategories;
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };
            return File(output.ToArray(), "application/octet-stream", "Stores.xlsx");
        }
        [Route(StoreRoute.ExportTemplate), HttpPost]
        public async Task<ActionResult> ExportTemplate()
        {
            List<Store> ParentStores = await StoreService.List(new StoreFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.Name | StoreSelect.Address | StoreSelect.Telephone,
                StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
            });
            List<Organization> Organizations = await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.Code | OrganizationSelect.Name | OrganizationSelect.Path,
                StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
            });
            List<StoreType> StoreTypes = await StoreTypeService.List(new StoreTypeFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreTypeSelect.Id | StoreTypeSelect.Code | StoreTypeSelect.Name,
                StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
            });
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(new StoreGroupingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreGroupingSelect.Id | StoreGroupingSelect.Code | StoreGroupingSelect.Name,
                StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
            });
            List<Province> Provinces = await ProvinceService.List(new ProvinceFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProvinceSelect.Id | ProvinceSelect.Code | ProvinceSelect.Name,
                StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
            });
            List<District> Districts = await DistrictService.List(new DistrictFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = DistrictSelect.Id | DistrictSelect.Code | DistrictSelect.Name | DistrictSelect.Province,
                StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
            });
            List<Ward> Wards = await WardService.List(new WardFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = WardSelect.Id | WardSelect.Code | WardSelect.Name | WardSelect.District,
                StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
            });
            List<ShowingCategory> ShowingCategories = await ShowingCategoryService.List(new ShowingCategoryFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ShowingCategorySelect.Id | ShowingCategorySelect.Code | ShowingCategorySelect.Name,
            });
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            MemoryStream MemoryStream = new MemoryStream();
            string tempPath = "Templates/Store_Template.xlsx";
            using (var xlPackage = new ExcelPackage(new FileInfo(tempPath)))
            {
                #region sheet Organization 
                var worksheet_Organization = xlPackage.Workbook.Worksheets["Đơn vị tổ chức"];
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                int startRow_Organization = 2;
                int numberCell_Organizations = 1;
                for (var i = 0; i < Organizations.Count; i++)
                {
                    Organization Organization = Organizations[i];
                    worksheet_Organization.Cells[startRow_Organization + i, numberCell_Organizations].Value = Organization.Code;
                    worksheet_Organization.Cells[startRow_Organization + i, numberCell_Organizations + 1].Value = Organization.Name;
                }
                #endregion
                #region sheet ParentStore 
                var worksheet_ParentStore = xlPackage.Workbook.Worksheets["Đại lý cha"];
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                int startRow_ParentStore = 2;
                int numberCell_ParentStores = 1;
                for (var i = 0; i < ParentStores.Count; i++)
                {
                    Store Store = ParentStores[i];
                    worksheet_ParentStore.Cells[startRow_ParentStore + i, numberCell_ParentStores].Value = Store.Code;
                    worksheet_ParentStore.Cells[startRow_ParentStore + i, numberCell_ParentStores + 1].Value = Store.Name;
                    worksheet_ParentStore.Cells[startRow_ParentStore + i, numberCell_ParentStores + 2].Value = Store.Address;
                    worksheet_ParentStore.Cells[startRow_ParentStore + i, numberCell_ParentStores + 3].Value = Store.Telephone;
                }
                #endregion
                #region sheet StoreType 
                var worksheet_StoreType = xlPackage.Workbook.Worksheets["Loại đại lý"];
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                int startRow_StoreType = 2;
                int numberCell_StoreTypes = 1;
                for (var i = 0; i < StoreTypes.Count; i++)
                {
                    StoreType StoreType = StoreTypes[i];
                    worksheet_StoreType.Cells[startRow_StoreType + i, numberCell_StoreTypes].Value = StoreType.Code;
                    worksheet_StoreType.Cells[startRow_StoreType + i, numberCell_StoreTypes + 1].Value = StoreType.Name;
                }
                #endregion
                #region sheet StoreGrouping 
                var worksheet_StoreGroup = xlPackage.Workbook.Worksheets["Nhóm đại lý"];
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                int startRow_StoreGroup = 2;
                int numberCell_StoreGroup = 1;
                for (var i = 0; i < StoreGroupings.Count; i++)
                {
                    StoreGrouping StoreGrouping = StoreGroupings[i];
                    worksheet_StoreGroup.Cells[startRow_StoreGroup + i, numberCell_StoreGroup].Value = StoreGrouping.Code;
                    worksheet_StoreGroup.Cells[startRow_StoreGroup + i, numberCell_StoreGroup + 1].Value = StoreGrouping.Name;
                }
                #endregion
                #region sheet ShowingCategory 
                var worksheet_ShowingCategory = xlPackage.Workbook.Worksheets["Danh mục sản phẩm trưng bày"];
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                int startRow_ShowingCategory = 2;
                int numberCell_ShowingCategory = 1;
                for (var i = 0; i < ShowingCategories.Count; i++)
                {
                    ShowingCategory ShowingCategory = ShowingCategories[i];
                    worksheet_ShowingCategory.Cells[startRow_ShowingCategory + i, numberCell_ShowingCategory].Value = ShowingCategory.Code;
                    worksheet_ShowingCategory.Cells[startRow_ShowingCategory + i, numberCell_ShowingCategory + 1].Value = ShowingCategory.Name;
                }
                #endregion
                #region sheet Province 
                var worksheet_Province = xlPackage.Workbook.Worksheets["Tỉnh, Thành phố"];
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                int startRow_Province = 2;
                int numberCell_Provinces = 1;
                for (var i = 0; i < Provinces.Count; i++)
                {
                    Province Province = Provinces[i];
                    worksheet_Province.Cells[startRow_Province + i, numberCell_Provinces].Value = Province.Code;
                    worksheet_Province.Cells[startRow_Province + i, numberCell_Provinces + 1].Value = Province.Name;
                }
                #endregion
                #region sheet District 
                var worksheet_District = xlPackage.Workbook.Worksheets["Quận, Huyện"];
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                int startRow_District = 2;
                int numberCell_Districts = 1;
                for (var i = 0; i < Districts.Count; i++)
                {
                    District District = Districts[i];
                    worksheet_District.Cells[startRow_District + i, numberCell_Districts].Value = District.Code;
                    worksheet_District.Cells[startRow_District + i, numberCell_Districts + 1].Value = District.Name;
                    worksheet_District.Cells[startRow_District + i, numberCell_Districts + 2].Value = District.Province?.Name;
                }
                #endregion
                #region sheet Ward 
                var worksheet_Ward = xlPackage.Workbook.Worksheets["Phường, Xã"];
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                int startRow_Ward = 2;
                int numberCell_Wards = 1;
                for (var i = 0; i < Wards.Count; i++)
                {
                    Ward Ward = Wards[i];
                    worksheet_Ward.Cells[startRow_Ward + i, numberCell_Wards].Value = Ward.Code;
                    worksheet_Ward.Cells[startRow_Ward + i, numberCell_Wards + 1].Value = Ward.Name;
                    worksheet_Ward.Cells[startRow_Ward + i, numberCell_Wards + 2].Value = Ward.District?.Name;
                    worksheet_Ward.Cells[startRow_Ward + i, numberCell_Wards + 3].Value = Ward.District?.Province?.Name;
                }
                #endregion
                xlPackage.SaveAs(MemoryStream);
            }
            return File(MemoryStream.ToArray(), "application/octet-stream", "Template_Store.xlsx");
        }
        [Route(StoreRoute.ExportStoreProfile), HttpPost]
        public async Task<ActionResult> ExportStoreProfile([FromBody] Store_StoreFilterDTO Store_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StoreFilter StoreFilter = ConvertFilterDTOToFilterEntity(Store_StoreFilterDTO);
            StoreFilter.Take = int.MaxValue;
            StoreFilter.Skip = 0;
            StoreFilter.Selects = StoreSelect.Id | StoreSelect.Name | StoreSelect.Code | StoreSelect.Address | StoreSelect.Province |
                StoreSelect.District | StoreSelect.Telephone | StoreSelect.StoreType | StoreSelect.ParentStore | StoreSelect.Description |
                StoreSelect.EstimatedRevenue | StoreSelect.Status;
            StoreFilter = StoreService.ToFilter(StoreFilter);
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<Store_StoreProfileExportDTO> Store_StoreProfileExportDTOs = Stores.Select(x => new Store_StoreProfileExportDTO(x)).OrderBy(x => x.Id).ToList();
            DateTime StartMonthTime = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddHours(0 - CurrentContext.TimeZone);
            DateFilter OrderDate = new DateFilter { GreaterEqual = StartMonthTime };
            var StoreIds = Stores.Select(x => x.Id).Distinct().ToList();
            IdFilter StoreIdFilter = new IdFilter { In = StoreIds };

            #region Product grouping of Rang Dong and top 1 Brand
            long RangDongBrandId = DataContext.Brand.Where(x => x.Code == "RD").Select(x => x.Id).FirstOrDefault();
            var BrandInStoreQuery = DataContext.BrandInStore.AsNoTracking();
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.DeletedAt == null);
            BrandInStoreQuery = BrandInStoreQuery.Where(x => x.StoreId, StoreIdFilter);
            List<long> BrandInStoreIds = await BrandInStoreQuery.Where(x => x.BrandId == RangDongBrandId).Select(x => x.Id).ToListWithNoLockAsync(); // Chỉ lấy của Rạng Đông.

            var BrandInStoreProductGroupingMappingQuery = DataContext.BrandInStoreProductGroupingMapping.AsNoTracking();
            BrandInStoreProductGroupingMappingQuery = BrandInStoreProductGroupingMappingQuery.Where(x => x.BrandInStoreId, new IdFilter { In = BrandInStoreIds });
            var BrandInStoreProductGroupingMapping = await BrandInStoreProductGroupingMappingQuery.Select(x => new
            {
                ProductGroupingId = x.ProductGroupingId,
                ProductGroupingName = x.ProductGrouping.Name,
                StoreId = x.BrandInStore.StoreId,
            }).OrderBy(x => x.StoreId).ToListWithNoLockAsync();

            var Top1BrandInStores = await BrandInStoreQuery.Where(x => x.Top == 1)
                .Select(x => new { StoreId = x.StoreId, Brand = x.Brand.Name }).Distinct().OrderBy(x => x.StoreId).ToListWithNoLockAsync();
            #endregion

            #region Store checking
            var StoreCheckingQuery = DataContext.StoreChecking.AsNoTracking();
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.CheckInAt, OrderDate);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.StoreId, StoreIdFilter);
            var StoreCheckingCounter = await StoreCheckingQuery.GroupBy(x => x.StoreId).Select(x => new
            {
                StoreId = x.Key,
                Counter = x.Count()
            }).OrderBy(x => x.StoreId).ToListWithNoLockAsync();
            #endregion

            var ProductGroupingQuery = DataContext.ProductGrouping.AsNoTracking();
            ProductGroupingQuery = ProductGroupingQuery.Where(x => x.DeletedAt == null);
            ProductGroupingQuery = ProductGroupingQuery.Where(x => x.HasChildren == false);
            ProductGroupingQuery = ProductGroupingQuery.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });
            var ProductGroupings = await ProductGroupingQuery.Select(x => new
            {
                ProductGroupingId = x.Id,
                ProductGroupingName = x.Name,
            }).Distinct().OrderBy(x => x.ProductGroupingId).ToListWithNoLockAsync();

            long STT = 1;
            foreach (var Store_StoreProfileExportDTO in Store_StoreProfileExportDTOs)
            {
                Store_StoreProfileExportDTO.STT = STT.ToString();
                STT++;
                Store_StoreProfileExportDTO.ProductGroupings = new List<StoreExport_ProductGroupingDTO>();
                for (int i = 0; i < ProductGroupings.Count; i++)
                {
                    Store_StoreProfileExportDTO.ProductGroupings.Add(new StoreExport_ProductGroupingDTO
                    {
                        ProductGroupingId = ProductGroupings[i].ProductGroupingId,
                        ProductGroupingName = ProductGroupings[i].ProductGroupingName,
                    });
                }
            }

            #region Revenue
            DateTime Now = StaticParams.DateTimeNow;
            DateTime StartMonth = new DateTime();
            DateTime EndMonth = new DateTime();
            DateTime StartQuarter = new DateTime();
            DateTime EndQuarter = new DateTime();
            (StartMonth, EndMonth) = TimeService.ConvertDashboardTime(new IdFilter { Equal = DashboardPeriodTimeEnum.THIS_MONTH.Id }, CurrentContext);
            (StartQuarter, EndQuarter) = TimeService.ConvertDashboardTime(new IdFilter { Equal = DashboardPeriodTimeEnum.THIS_QUARTER.Id }, CurrentContext);

            var IndirectSalesOrder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.BuyerStoreId, StoreIdFilter);
            var IndirectRevenueInMonth = await IndirectSalesOrder_query
                .Where(x => x.OrderDate, new DateFilter { GreaterEqual = StartMonth, LessEqual = EndMonth })
                .GroupBy(x => x.BuyerStoreId).Select(x => new { StoreId = x.Key, Revenue = x.Sum(x => x.Total) })
                .OrderBy(x => x.StoreId).ToListWithNoLockAsync();
            var IndirectRevenueInQuarter = await IndirectSalesOrder_query
                .Where(x => x.OrderDate, new DateFilter { GreaterEqual = StartQuarter, LessEqual = EndQuarter })
                .GroupBy(x => x.BuyerStoreId).Select(x => new { StoreId = x.Key, Revenue = x.Sum(x => x.Total) })
                .OrderBy(x => x.StoreId).ToListWithNoLockAsync();

            var DirectSalesOrder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.BuyerStoreId, StoreIdFilter);
            DirectSalesOrder_query = DirectSalesOrder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long>
            {
                GeneralApprovalStateEnum.APPROVED.Id,
                GeneralApprovalStateEnum.STORE_APPROVED.Id
            }
            });
            var DirectRevenueInMonth = await DirectSalesOrder_query
                .Where(x => x.OrderDate, new DateFilter { GreaterEqual = StartMonth, LessEqual = EndMonth })
                .GroupBy(x => x.BuyerStoreId).Select(x => new { StoreId = x.Key, Revenue = x.Sum(x => x.Total) })
                .OrderBy(x => x.StoreId).ToListWithNoLockAsync();
            var DirectRevenueInQuarter = await DirectSalesOrder_query
                .Where(x => x.OrderDate, new DateFilter { GreaterEqual = StartQuarter, LessEqual = EndQuarter })
                .GroupBy(x => x.BuyerStoreId).Select(x => new { StoreId = x.Key, Revenue = x.Sum(x => x.Total) })
                .OrderBy(x => x.StoreId).ToListWithNoLockAsync();
            #endregion

            int StoreIndex = 0;
            int MappingIndex = 0;
            int TopBrandIndex = 0;
            int StoreCheckingIndex = 0;
            int OrderIndex = 0;
            while (StoreIndex < Store_StoreProfileExportDTOs.Count && MappingIndex < BrandInStoreProductGroupingMapping.Count)
            {
                if (Store_StoreProfileExportDTOs[StoreIndex].Id > BrandInStoreProductGroupingMapping[MappingIndex].StoreId)
                    MappingIndex++;
                else if (Store_StoreProfileExportDTOs[StoreIndex].Id < BrandInStoreProductGroupingMapping[MappingIndex].StoreId)
                    StoreIndex++;
                else if (Store_StoreProfileExportDTOs[StoreIndex].Id == BrandInStoreProductGroupingMapping[MappingIndex].StoreId)
                {
                    var subProductGrouping = Store_StoreProfileExportDTOs[StoreIndex].ProductGroupings
                        .Where(x => x.ProductGroupingId == BrandInStoreProductGroupingMapping[MappingIndex].ProductGroupingId).FirstOrDefault();
                    if (subProductGrouping != null) subProductGrouping.Value = "x";

                    MappingIndex++;
                }
            }

            StoreIndex = 0;
            while (StoreIndex < Store_StoreProfileExportDTOs.Count && TopBrandIndex < Top1BrandInStores.Count)
            {
                if (Store_StoreProfileExportDTOs[StoreIndex].Id > Top1BrandInStores[TopBrandIndex].StoreId)
                    TopBrandIndex++;
                else if (Store_StoreProfileExportDTOs[StoreIndex].Id < Top1BrandInStores[TopBrandIndex].StoreId)
                    StoreIndex++;
                else if (Store_StoreProfileExportDTOs[StoreIndex].Id == Top1BrandInStores[TopBrandIndex].StoreId)
                {
                    Store_StoreProfileExportDTOs[StoreIndex].Top1Brand = Top1BrandInStores[TopBrandIndex].Brand;

                    TopBrandIndex++;
                }
            }

            StoreIndex = 0;
            while (StoreIndex < Store_StoreProfileExportDTOs.Count && StoreCheckingIndex < StoreCheckingCounter.Count)
            {
                if (Store_StoreProfileExportDTOs[StoreIndex].Id > StoreCheckingCounter[StoreCheckingIndex].StoreId)
                    StoreCheckingIndex++;
                else if (Store_StoreProfileExportDTOs[StoreIndex].Id < StoreCheckingCounter[StoreCheckingIndex].StoreId)
                    StoreIndex++;
                else if (Store_StoreProfileExportDTOs[StoreIndex].Id == StoreCheckingCounter[StoreCheckingIndex].StoreId)
                {
                    Store_StoreProfileExportDTOs[StoreIndex].CheckingCounterInMonth = StoreCheckingCounter[StoreCheckingIndex].Counter;

                    StoreCheckingIndex++;
                }
            }

            StoreIndex = 0;
            while (StoreIndex < Store_StoreProfileExportDTOs.Count && OrderIndex < IndirectRevenueInMonth.Count)
            {
                if (Store_StoreProfileExportDTOs[StoreIndex].Id > IndirectRevenueInMonth[OrderIndex].StoreId)
                    OrderIndex++;
                else if (Store_StoreProfileExportDTOs[StoreIndex].Id < IndirectRevenueInMonth[OrderIndex].StoreId)
                    StoreIndex++;
                else if (Store_StoreProfileExportDTOs[StoreIndex].Id == IndirectRevenueInMonth[OrderIndex].StoreId)
                {
                    Store_StoreProfileExportDTOs[StoreIndex].MonthlyRevenue += IndirectRevenueInMonth[OrderIndex].Revenue;
                    OrderIndex++;
                }
            }
            StoreIndex = 0;
            OrderIndex = 0;
            while (StoreIndex < Store_StoreProfileExportDTOs.Count && OrderIndex < DirectRevenueInMonth.Count)
            {
                if (Store_StoreProfileExportDTOs[StoreIndex].Id > DirectRevenueInMonth[OrderIndex].StoreId)
                    OrderIndex++;
                else if (Store_StoreProfileExportDTOs[StoreIndex].Id < DirectRevenueInMonth[OrderIndex].StoreId)
                    StoreIndex++;
                else if (Store_StoreProfileExportDTOs[StoreIndex].Id == DirectRevenueInMonth[OrderIndex].StoreId)
                {
                    Store_StoreProfileExportDTOs[StoreIndex].MonthlyRevenue += DirectRevenueInMonth[OrderIndex].Revenue;
                    OrderIndex++;
                }
            }
            StoreIndex = 0;
            OrderIndex = 0;
            while (StoreIndex < Store_StoreProfileExportDTOs.Count && OrderIndex < IndirectRevenueInQuarter.Count)
            {
                if (Store_StoreProfileExportDTOs[StoreIndex].Id > IndirectRevenueInQuarter[OrderIndex].StoreId)
                    OrderIndex++;
                else if (Store_StoreProfileExportDTOs[StoreIndex].Id < IndirectRevenueInQuarter[OrderIndex].StoreId)
                    StoreIndex++;
                else if (Store_StoreProfileExportDTOs[StoreIndex].Id == IndirectRevenueInQuarter[OrderIndex].StoreId)
                {
                    Store_StoreProfileExportDTOs[StoreIndex].QuarterlyRevenue += IndirectRevenueInQuarter[OrderIndex].Revenue;
                    OrderIndex++;
                }
            }
            StoreIndex = 0;
            OrderIndex = 0;
            while (StoreIndex < Store_StoreProfileExportDTOs.Count && OrderIndex < DirectRevenueInQuarter.Count)
            {
                if (Store_StoreProfileExportDTOs[StoreIndex].Id > DirectRevenueInQuarter[OrderIndex].StoreId)
                    OrderIndex++;
                else if (Store_StoreProfileExportDTOs[StoreIndex].Id < DirectRevenueInQuarter[OrderIndex].StoreId)
                    StoreIndex++;
                else if (Store_StoreProfileExportDTOs[StoreIndex].Id == DirectRevenueInQuarter[OrderIndex].StoreId)
                {
                    Store_StoreProfileExportDTOs[StoreIndex].QuarterlyRevenue += DirectRevenueInQuarter[OrderIndex].Revenue;
                    OrderIndex++;
                }
            }

            MemoryStream output = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(output))
            {
                #region Tieu de bao cao
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add("Hồ sơ đại lý");
                ws.Cells.Style.Font.Name = "Times New Roman";
                ws.Cells.Style.Font.Size = 12;
                ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                // Không hiểu sao ô AL4 này ko apply format đã set ở trên nên phải set riêng
                ws.Cells["AL4"].Style.Font.Name = "Times New Roman";
                ws.Cells["AL4"].Style.Font.Size = 12;
                ws.Cells["AL4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                ws.Cells["A1"].Value = "HỒ SƠ ĐẠI LÝ";
                ws.Cells["A1"].Style.Font.Size = 20;
                ws.Cells["A1"].Style.Font.Bold = true;
                ws.Cells["A1:J1"].Merge = true;
                ws.Cells["A1:J1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                ws.Cells["D2"].Value = "Ngày xuất hồ sơ";
                ws.Cells["D2"].Style.Font.Bold = true;
                ws.Cells["D2"].Style.Font.Italic = true;
                ws.Cells["E2"].Value = StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).ToString("dd/MM/yyyy HH:mm");
                #endregion

                #region Header
                ws.Row(5).Height = 30;
                int StartHeaderRow = 4;
                int StartColumn = 1;

                List<string> StoreInfoHeader = new List<string>
                {
                    "STT",
                    "Mã đại lý tự sinh",
                    "Tên đại lý",
                    "Địa chỉ",
                    "Tỉnh/Thành phố",
                    "Quận/Huyện",
                    "Số điện thoại",
                    "Cấp đại lý",
                    "Đại lý cấp 1",
                    "Miêu tả đại lý"
                };
                int EndColumn = StoreInfoHeader.Count;
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow}"]
                    .LoadFromArrays(new List<string[]> { StoreInfoHeader.ToArray() });
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow}"].AutoFitColumns();

                for (int i = 0; i < StoreInfoHeader.Count; i++)
                {
                    ws.Cells[$"{ConvertColumnNameExcel(StartColumn + i)}{StartHeaderRow}:{ConvertColumnNameExcel(StartColumn + i)}{StartHeaderRow + 1}"].Merge = true;
                }
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow + 1}"].Style.Font.Bold = true;
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow + 1}"]
                    .Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow + 1}"]
                    .Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#E2EFD9"));

                StartColumn = EndColumn + 1;
                List<string> ProductGroupingHeader = new List<string>
                {
                    "Nhóm sản phẩm Công ty đang bán",
                    "Tổng số nhóm"
                };
                for (int i = 0; i < ProductGroupings.Count; i++)
                {
                    ProductGroupingHeader.Add(ProductGroupings[i].ProductGroupingName);
                    ws.Column(StartColumn + i).Width = 10;
                    if (i < 2) continue;
                    else ws.Column(StartColumn + i).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }
                EndColumn += ProductGroupingHeader.Count;
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow + 1}"]
                    .LoadFromArrays(new List<string[]> { new string[] { "Nhóm sản phẩm Công ty đang bán" }, ProductGroupingHeader.ToArray() });
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow + 1}"].Style.Font.Bold = true;
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow}"].Merge = true;
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow + 1}"]
                    .Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow}"]
                    .Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFC000"));
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow + 1}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow + 1}"]
                    .Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FEF2CB"));

                StartColumn = EndColumn + 1;
                List<string> StoreInfoHeader2 = new List<string>
                {
                    "Hãng xếp hạng số 1",
                    "Tần suất ghé thăm trong tháng",
                    "Ước doanh thu ngành đèn/ tháng",
                };
                EndColumn += StoreInfoHeader2.Count;
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow}"]
                    .LoadFromArrays(new List<string[]> { StoreInfoHeader2.ToArray() });
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow}"].Style.Font.Bold = true;
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow}"]
                    .Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow}"]
                    .Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#E2EFD9"));
                for (int i = 0; i < StoreInfoHeader2.Count; i++)
                {
                    ws.Cells[$"{ConvertColumnNameExcel(StartColumn + i)}{StartHeaderRow}:{ConvertColumnNameExcel(StartColumn + i)}{StartHeaderRow + 1}"].Merge = true;
                    ws.Column(StartColumn + i).Width = 20;
                }

                StartColumn = EndColumn + 1;
                List<string> KpiHeader = new List<string>
                {
                    "Kế hoạch doanh thu Tháng",
                    "Số doanh thu trong tháng",
                    "Tỷ lệ hoàn thành Tháng (%)",
                    "Kế hoạch doanh thu Quý",
                    "Số doanh thu trong Quý",
                    "Tỷ lệ hoàn thành Quý (%)"
                };
                EndColumn += KpiHeader.Count;

                for (int i = 0; i < KpiHeader.Count; i++)
                {
                    ws.Column(StartColumn + i).Width = 20;
                }
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow + 1}"]
                    .LoadFromArrays(new List<string[]> { new string[] { "KPI Đại lý" }, KpiHeader.ToArray() });
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow + 1}"].Style.Font.Bold = true;
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow}"].Merge = true;
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow + 1}"]
                    .Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow}"]
                    .Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#5B9BD5"));
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow + 1}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow + 1}"]
                    .Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#DEEAF6"));

                StartColumn = EndColumn + 1;
                List<string> StatusHeader = new List<string>
                {
                    "Trạng thái",
                };
                EndColumn += StatusHeader.Count;
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow}"]
                    .LoadFromArrays(new List<string[]> { StatusHeader.ToArray() });
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow}"].Style.Font.Bold = true;
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow}"]
                    .Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow}"]
                    .Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#E2EFD9"));
                for (int i = 0; i < StatusHeader.Count; i++)
                {
                    ws.Cells[$"{ConvertColumnNameExcel(StartColumn + i)}{StartHeaderRow}:{ConvertColumnNameExcel(StartColumn + i)}{StartHeaderRow + 1}"].Merge = true;
                    ws.Column(StartColumn + i).Width = 15;
                }

                ws.Cells[$"{ConvertColumnNameExcel(1)}{StartHeaderRow}:{ConvertColumnNameExcel(EndColumn)}{StartHeaderRow + 1}"]
                .Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                #endregion

                #region Dữ liệu báo cáo

                int StartRow = 6;
                int EndRow = StartRow - 1;
                StartColumn = 1;
                EndColumn = 0;
                if (Store_StoreProfileExportDTOs != null && Store_StoreProfileExportDTOs.Count > 0)
                {
                    #region Store info - string column
                    List<Object[]> StoreInfoData = new List<object[]>();
                    foreach (var StoreProfile in Store_StoreProfileExportDTOs)
                    {
                        StoreInfoData.Add(new object[]
                        {
                            StoreProfile.STT,
                            StoreProfile.Code,
                            StoreProfile.Name,
                            StoreProfile.Address,
                            StoreProfile.Province,
                            StoreProfile.District,
                            StoreProfile.Phone,
                            StoreProfile.StoreType,
                            StoreProfile.ParentStore,
                            StoreProfile.Description,
                            StoreProfile.ListProductGroupingName,
                        });
                        EndRow++;
                    };
                    EndColumn += 11;
                    ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartRow}:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].LoadFromArrays(StoreInfoData);
                    StartColumn = EndColumn + 1;
                    #endregion

                    #region Product grouping counter - number column
                    EndRow = StartRow;
                    List<Object[]> ProductGroupingCounterData = new List<object[]>();
                    foreach (var StoreProfile in Store_StoreProfileExportDTOs)
                    {
                        ProductGroupingCounterData.Add(new object[]
                        {
                            StoreProfile.ProductGroupingsCounter,
                        });
                        EndRow++;
                    };
                    EndColumn += 1;
                    ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartRow}:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].LoadFromArrays(ProductGroupingCounterData);
                    StartColumn = EndColumn + 1;
                    #endregion

                    #region Product grouping column
                    for (int i = 0; i < ProductGroupings.Count; i++)
                    {
                        EndRow = StartRow;
                        List<Object[]> ProductGroupingData = new List<object[]>();
                        foreach (var StoreProfile in Store_StoreProfileExportDTOs)
                        {
                            string value = StoreProfile.ProductGroupings.Where(x => x.ProductGroupingId == ProductGroupings[i].ProductGroupingId)
                                .Select(x => x.Value).FirstOrDefault();
                            ProductGroupingData.Add(new object[]
                            {
                            value,
                            });
                            EndRow++;
                        };
                        EndColumn += 1;
                        ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartRow}:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].LoadFromArrays(ProductGroupingData);
                        StartColumn = EndColumn + 1;
                    }
                    #endregion

                    #region Top 1 Brand - string column
                    EndRow = StartRow;
                    List<Object[]> Top1BrandData = new List<object[]>();
                    foreach (var StoreProfile in Store_StoreProfileExportDTOs)
                    {
                        Top1BrandData.Add(new object[]
                        {
                            StoreProfile.Top1Brand,
                        });
                        EndRow++;
                    };
                    EndColumn += 1;
                    ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartRow}:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].LoadFromArrays(Top1BrandData);
                    StartColumn = EndColumn + 1;
                    #endregion

                    #region Checking counter - number column
                    EndRow = StartRow;
                    List<Object[]> CheckingCounterData = new List<object[]>();
                    foreach (var StoreProfile in Store_StoreProfileExportDTOs)
                    {
                        CheckingCounterData.Add(new object[]
                        {
                            StoreProfile.CheckingCounterInMonth,
                        });
                        EndRow++;
                    };
                    EndColumn += 1;
                    ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartRow}:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].LoadFromArrays(CheckingCounterData);
                    StartColumn = EndColumn + 1;
                    #endregion

                    #region Estimated revenue - string column
                    EndRow = StartRow;
                    List<Object[]> EstimatedRevenueData = new List<object[]>();
                    foreach (var StoreProfile in Store_StoreProfileExportDTOs)
                    {
                        EstimatedRevenueData.Add(new object[]
                        {
                            StoreProfile.EstimatedRevenue,
                        });
                        EndRow++;
                    };
                    EndColumn += 1;
                    ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartRow}:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].LoadFromArrays(EstimatedRevenueData);
                    StartColumn = EndColumn + 1;
                    #endregion

                    #region KPI - number column
                    EndRow = StartRow;
                    List<Object[]> KpiData = new List<object[]>();
                    foreach (var StoreProfile in Store_StoreProfileExportDTOs)
                    {
                        KpiData.Add(new object[]
                        {
                            StoreProfile.MonthlyRevenuePlanned,
                            StoreProfile.MonthlyRevenue,
                            StoreProfile.MonthlyRevenueRatio,
                            StoreProfile.QuarterlyRevenuePlanned,
                            StoreProfile.QuarterlyRevenue,
                            StoreProfile.QuarterlyRevenueRatio,
                        });
                        EndRow++;
                    };
                    EndColumn += 6;
                    ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartRow}:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].LoadFromArrays(KpiData);
                    // format monthly revenue column value
                    ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartRow}:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].Style.Numberformat.Format = "#,##0";
                    StartColumn = EndColumn + 1;
                    #endregion

                    #region Status - string column
                    EndRow = StartRow;
                    List<Object[]> StatusData = new List<object[]>();
                    foreach (var StoreProfile in Store_StoreProfileExportDTOs)
                    {
                        StatusData.Add(new object[]
                        {
                            StoreProfile.Status,
                        });
                        EndRow++;
                    };
                    EndColumn += 1;
                    ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartRow}:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].LoadFromArrays(StatusData);
                    StartColumn = EndColumn + 1;
                    #endregion
                }
                #endregion

                ws.Cells[$"A4:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].Style.WrapText = true;

                // All borders
                ws.Cells[$"A4:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                // fix big column
                ws.Column(2).Width = 22; // Code column
                ws.Column(3).Width = 22; // Name column
                ws.Column(4).Width = 30; // Address column
                ws.Column(10).Width = 50; // Description column
                ws.Column(11).Width = 40; // List product grouping name column

                excel.Save();
            }

            return File(output.ToArray(), "application/octet-stream", "Store_Profile.xlsx");

        }
        [Route(StoreRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Id = new IdFilter { In = Ids };
            StoreFilter.Selects = StoreSelect.Id;
            StoreFilter.Skip = 0;
            StoreFilter.Take = int.MaxValue;
            List<Store> Stores = await StoreService.List(StoreFilter);
            Stores = await StoreService.BulkDelete(Stores);
            if (Stores.Any(x => !x.IsValidated))
                return BadRequest(Stores.Where(x => !x.IsValidated));
            return true;
        }
        [HttpPost]
        [Route(StoreRoute.SaveImage)]
        public async Task<ActionResult<Store_ImageDTO>> SaveImage(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            MemoryStream memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            Image Image = new Image
            {
                Name = file.FileName,
                Content = memoryStream.ToArray(),
            };
            Image = await StoreService.SaveImage(Image);
            if (Image == null)
                return BadRequest();
            Store_ImageDTO Store_ImageDTO = new Store_ImageDTO
            {
                Id = Image.Id,
                Name = Image.Name,
                Url = Image.Url,
                ThumbnailUrl = Image.ThumbnailUrl,
            };
            return Ok(Store_ImageDTO);
        }
        private async Task<bool> HasPermission(long Id)
        {
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter = StoreService.ToFilter(StoreFilter);
            if (Id == 0)
            {
            }
            else
            {
                StoreFilter.Id = new IdFilter { Equal = Id };
                int count = await StoreService.Count(StoreFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }
        #region StoreUser
        [Route(StoreRoute.CreateDraft), HttpPost]
        public async Task<ActionResult<Store_StoreUserDTO>> CreateDraft([FromBody] Store_StoreUserDTO Store_StoreUserDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StoreUser StoreUser = new StoreUser
            {
                Id = Store_StoreUserDTO.Id,
                StatusId = Store_StoreUserDTO.StatusId,
                StoreId = Store_StoreUserDTO.StoreId,
                CreatedAt = Store_StoreUserDTO.CreatedAt,
                UpdatedAt = Store_StoreUserDTO.UpdatedAt,
                DisplayName = Store_StoreUserDTO.DisplayName,
                Password = Store_StoreUserDTO.Password,
                Username = Store_StoreUserDTO.Username,
            };
            StoreUser = await StoreUserService.CreateDraft(StoreUser);
            Store_StoreUserDTO = new Store_StoreUserDTO(StoreUser);
            if (StoreUser.IsValidated)
                return Store_StoreUserDTO;
            else
                return BadRequest(Store_StoreUserDTO);
        }
        [Route(StoreRoute.CreateStoreUser), HttpPost]
        public async Task<ActionResult<Store_StoreUserDTO>> CreateStoreUser([FromBody] Store_StoreUserDTO Store_StoreUserDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StoreUser StoreUser = new StoreUser
            {
                Id = Store_StoreUserDTO.Id,
                StatusId = Store_StoreUserDTO.StatusId,
                StoreId = Store_StoreUserDTO.StoreId,
                CreatedAt = Store_StoreUserDTO.CreatedAt,
                UpdatedAt = Store_StoreUserDTO.UpdatedAt,
                DisplayName = Store_StoreUserDTO.DisplayName,
                Password = Store_StoreUserDTO.Password,
                Username = Store_StoreUserDTO.Username,
            };
            StoreUser = await StoreUserService.Create(StoreUser);
            Store_StoreUserDTO = new Store_StoreUserDTO(StoreUser);
            if (StoreUser.IsValidated)
                return Store_StoreUserDTO;
            else
                return BadRequest(Store_StoreUserDTO);
        }
        [Route(StoreRoute.BulkCreateStoreUser), HttpPost]
        public async Task<ActionResult<Store_StoreUserDTO>> BulkCreateStoreUser([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StoreFilter StoreFilter = new StoreFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.Name,
                Id = new IdFilter { In = Ids },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };
            List<Store> Stores = await StoreService.List(StoreFilter);
            var StoreUsers = Stores.Select(x => new StoreUser
            {
                StoreId = x.Id,
                DisplayName = x.Name,
                Username = x.Code.Split('.')[2],
                Password = "appdailyrangdong"
            }).ToList();
            StoreUsers = await StoreUserService.BulkCreateStoreUser(StoreUsers);
            List<Store_StoreUserDTO> Store_StoreUserDTOs = StoreUsers
                .Select(c => new Store_StoreUserDTO(c)).ToList();
            return Ok(Store_StoreUserDTOs);
        }
        [Route(StoreRoute.ResetPassword), HttpPost]
        public async Task<ActionResult<Store_StoreUserDTO>> ResetPassword([FromBody] Store_ChangePasswordDTO Store_ChangePasswordDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StoreUser StoreUser = new StoreUser
            {
                Id = Store_ChangePasswordDTO.Id
            };
            StoreUser = await StoreUserService.ResetPassword(StoreUser);
            Store_StoreUserDTO Store_StoreUserDTO = new Store_StoreUserDTO(StoreUser);
            if (StoreUser.IsValidated)
                return Store_StoreUserDTO;
            else
                return BadRequest(Store_StoreUserDTO);
        }
        [Route(StoreRoute.LockStoreUser), HttpPost]
        public async Task<ActionResult<Store_StoreUserDTO>> LockStoreUser([FromBody] Store_StoreUserDTO Store_StoreUserDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StoreUser StoreUser = await StoreUserService.Get(Store_StoreUserDTO.Id);
            StoreUser.StatusId = Store_StoreUserDTO.StatusId;
            StoreUser = await StoreUserService.LockStoreUser(StoreUser);
            Store_StoreUserDTO = new Store_StoreUserDTO(StoreUser);
            if (StoreUser.IsValidated)
                return Store_StoreUserDTO;
            else
                return BadRequest(Store_StoreUserDTO);
        }
        #endregion
        #region external sync
        [Route(StoreRoute.BulkCreate), HttpPost]
        public async Task<ActionResult<Store_StoreDTO>> BulkCreate([FromBody] List<Store_StoreDTO> Store_StoreDTOs)
        {
            if (!ModelState.IsValid)
            {
                throw new BindException(ModelState);
            }
            List<Store> Stores = Store_StoreDTOs
                .Select(q => ConvertDTOToEntity(q))
                .ToList();
            Stores.ForEach(q => q.CreatorId = CurrentContext.UserId);
            Stores = await StoreService.Import(Stores);
            if (Stores == null)
                return Ok();
            else if (Stores != null && Stores.Any(q => q.IsValidated == false))
                return BadRequest(Stores);
            return Ok();
        }
        #endregion
        private Store ConvertDTOToEntity(Store_StoreDTO Store_StoreDTO)
        {
            Store_StoreDTO.TrimString();
            Store Store = new Store();
            Store.Id = Store_StoreDTO.Id;
            Store.Code = Store_StoreDTO.Code;
            Store.CodeDraft = Store_StoreDTO.CodeDraft;
            Store.CreatorId = Store_StoreDTO.CreatorId;
            Store.Name = Store_StoreDTO.Name;
            Store.ParentStoreId = Store_StoreDTO.ParentStoreId;
            Store.OrganizationId = Store_StoreDTO.OrganizationId;
            Store.StoreTypeId = Store_StoreDTO.StoreTypeId;
            Store.Telephone = Store_StoreDTO.Telephone;
            Store.ProvinceId = Store_StoreDTO.ProvinceId;
            Store.DistrictId = Store_StoreDTO.DistrictId;
            Store.WardId = Store_StoreDTO.WardId;
            Store.Address = Store_StoreDTO.Address;
            Store.DeliveryAddress = Store_StoreDTO.DeliveryAddress;
            Store.Latitude = Store_StoreDTO.Latitude;
            Store.Longitude = Store_StoreDTO.Longitude;
            Store.DeliveryLatitude = Store_StoreDTO.DeliveryLatitude;
            Store.DeliveryLongitude = Store_StoreDTO.DeliveryLongitude;
            Store.OwnerName = Store_StoreDTO.OwnerName;
            Store.OwnerPhone = Store_StoreDTO.OwnerPhone;
            Store.OwnerEmail = Store_StoreDTO.OwnerEmail;
            Store.TaxCode = Store_StoreDTO.TaxCode;
            Store.LegalEntity = Store_StoreDTO.LegalEntity;
            Store.StatusId = Store_StoreDTO.StatusId;
            Store.StoreScoutingId = Store_StoreDTO.StoreScoutingId;
            Store.StoreStatusId = Store_StoreDTO.StoreStatusId;
            Store.AppUserId = Store_StoreDTO.AppUserId;
            Store.Description = Store_StoreDTO.Description;
            Store.DebtLimited = Store_StoreDTO.DebtLimited;
            Store.EstimatedRevenueId = Store_StoreDTO.EstimatedRevenueId;
            Store.IsStoreApprovalDirectSalesOrder = Store_StoreDTO.IsStoreApprovalDirectSalesOrder;
            Store.District = Store_StoreDTO.District == null ? null : new District
            {
                Id = Store_StoreDTO.District.Id,
                Name = Store_StoreDTO.District.Name,
                Priority = Store_StoreDTO.District.Priority,
                ProvinceId = Store_StoreDTO.District.ProvinceId,
                StatusId = Store_StoreDTO.District.StatusId,
            };
            Store.Organization = Store_StoreDTO.Organization == null ? null : new Organization
            {
                Id = Store_StoreDTO.Organization.Id,
                Code = Store_StoreDTO.Organization.Code,
                Name = Store_StoreDTO.Organization.Name,
                ParentId = Store_StoreDTO.Organization.ParentId,
                Path = Store_StoreDTO.Organization.Path,
                Level = Store_StoreDTO.Organization.Level,
                StatusId = Store_StoreDTO.Organization.StatusId,
                Phone = Store_StoreDTO.Organization.Phone,
                Address = Store_StoreDTO.Organization.Address,
                Email = Store_StoreDTO.Organization.Email,
            };
            Store.ParentStore = Store_StoreDTO.ParentStore == null ? null : new Store
            {
                Id = Store_StoreDTO.ParentStore.Id,
                Code = Store_StoreDTO.ParentStore.Code,
                Name = Store_StoreDTO.ParentStore.Name,
                ParentStoreId = Store_StoreDTO.ParentStore.ParentStoreId,
                OrganizationId = Store_StoreDTO.ParentStore.OrganizationId,
                StoreTypeId = Store_StoreDTO.ParentStore.StoreTypeId,
                Telephone = Store_StoreDTO.ParentStore.Telephone,
                ProvinceId = Store_StoreDTO.ParentStore.ProvinceId,
                DistrictId = Store_StoreDTO.ParentStore.DistrictId,
                WardId = Store_StoreDTO.ParentStore.WardId,
                Address = Store_StoreDTO.ParentStore.Address,
                DeliveryAddress = Store_StoreDTO.ParentStore.DeliveryAddress,
                Latitude = Store_StoreDTO.ParentStore.Latitude,
                Longitude = Store_StoreDTO.ParentStore.Longitude,
                DeliveryLatitude = Store_StoreDTO.ParentStore.DeliveryLatitude,
                DeliveryLongitude = Store_StoreDTO.ParentStore.DeliveryLongitude,
                OwnerName = Store_StoreDTO.ParentStore.OwnerName,
                OwnerPhone = Store_StoreDTO.ParentStore.OwnerPhone,
                OwnerEmail = Store_StoreDTO.ParentStore.OwnerEmail,
                StatusId = Store_StoreDTO.ParentStore.StatusId,
            };
            Store.Province = Store_StoreDTO.Province == null ? null : new Province
            {
                Id = Store_StoreDTO.Province.Id,
                Name = Store_StoreDTO.Province.Name,
                Priority = Store_StoreDTO.Province.Priority,
                StatusId = Store_StoreDTO.Province.StatusId,
            };
            Store.Status = Store_StoreDTO.Status == null ? null : new Status
            {
                Id = Store_StoreDTO.Status.Id,
                Code = Store_StoreDTO.Status.Code,
                Name = Store_StoreDTO.Status.Name,
            };
            Store.StoreStatus = Store_StoreDTO.StoreStatus == null ? null : new StoreStatus
            {
                Id = Store_StoreDTO.StoreStatus.Id,
                Code = Store_StoreDTO.StoreStatus.Code,
                Name = Store_StoreDTO.StoreStatus.Name,
            };
            Store.StoreScouting = Store_StoreDTO.StoreScouting == null ? null : new StoreScouting
            {
                Id = Store_StoreDTO.StoreScouting.Id,
                Code = Store_StoreDTO.StoreScouting.Code,
                Name = Store_StoreDTO.StoreScouting.Name,
                Address = Store_StoreDTO.StoreScouting.Address,
                CreatorId = Store_StoreDTO.StoreScouting.CreatorId,
                DistrictId = Store_StoreDTO.StoreScouting.DistrictId,
                Latitude = Store_StoreDTO.StoreScouting.Latitude,
                Longitude = Store_StoreDTO.StoreScouting.Longitude,
                OwnerPhone = Store_StoreDTO.StoreScouting.OwnerPhone,
                ProvinceId = Store_StoreDTO.StoreScouting.ProvinceId,
                StoreScoutingStatusId = Store_StoreDTO.StoreScouting.StoreScoutingStatusId,
                WardId = Store_StoreDTO.StoreScouting.WardId,
            };
            Store.StoreType = Store_StoreDTO.StoreType == null ? null : new StoreType
            {
                Id = Store_StoreDTO.StoreType.Id,
                Code = Store_StoreDTO.StoreType.Code,
                Name = Store_StoreDTO.StoreType.Name,
                StatusId = Store_StoreDTO.StoreType.StatusId,
            };
            Store.Ward = Store_StoreDTO.Ward == null ? null : new Ward
            {
                Id = Store_StoreDTO.Ward.Id,
                Name = Store_StoreDTO.Ward.Name,
                Priority = Store_StoreDTO.Ward.Priority,
                DistrictId = Store_StoreDTO.Ward.DistrictId,
                StatusId = Store_StoreDTO.Ward.StatusId,
            };
            Store.AppUser = Store_StoreDTO.AppUser == null ? null : new AppUser
            {
                Id = Store_StoreDTO.AppUser.Id,
                Username = Store_StoreDTO.AppUser.Username,
                DisplayName = Store_StoreDTO.AppUser.DisplayName,
                Address = Store_StoreDTO.AppUser.Address,
                Email = Store_StoreDTO.AppUser.Email,
                Phone = Store_StoreDTO.AppUser.Phone,
            };
            Store.Creator = Store_StoreDTO.Creator == null ? null : new AppUser
            {
                Id = Store_StoreDTO.Creator.Id,
                Username = Store_StoreDTO.Creator.Username,
                DisplayName = Store_StoreDTO.Creator.DisplayName,
                Address = Store_StoreDTO.Creator.Address,
                Email = Store_StoreDTO.Creator.Email,
                Phone = Store_StoreDTO.Creator.Phone,
            };
            Store.StoreImageMappings = Store_StoreDTO.StoreImageMappings?
                .Select(x => new StoreImageMapping
                {
                    StoreId = x.StoreId,
                    ImageId = x.ImageId,
                    Image = new Image
                    {
                        Id = x.Image.Id,
                        Name = x.Image.Name,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                    }
                }).ToList();
            Store.AppUserStoreMappings = Store_StoreDTO.AppUserStoreMappings?
                .Select(x => new AppUserStoreMapping
                {
                    AppUserId = x.AppUserId,
                    AppUser = new AppUser
                    {
                        Id = x.AppUser.Id,
                        Username = x.AppUser.Username,
                        DisplayName = x.AppUser.DisplayName,
                    }
                }).ToList();
            Store.BrandInStores = Store_StoreDTO.BrandInStores?
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
                        ProductGrouping = x.ProductGrouping == null ? null : new ProductGrouping
                        {
                            Id = x.ProductGrouping.Id,
                            Name = x.ProductGrouping.Name,
                            Code = x.ProductGrouping.Code
                        }
                    }).ToList(),
                    BrandInStoreShowingCategoryMappings = x.BrandInStoreShowingCategoryMappings?.Select(x => new BrandInStoreShowingCategoryMapping
                    {
                        BrandInStoreId = x.BrandInStoreId,
                        ShowingCategoryId = x.ShowingCategoryId,
                        ShowingCategory = x.ShowingCategory == null ? null : new ShowingCategory
                        {
                            Id = x.ShowingCategory.Id,
                            Name = x.ShowingCategory.Name,
                            Code = x.ShowingCategory.Code
                        }
                    }).ToList()
                }).ToList();
            Store.StoreStoreGroupingMappings = Store_StoreDTO.StoreStoreGroupingMappings?
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
            return Store;
        }
        private StoreFilter ConvertFilterDTOToFilterEntity(Store_StoreFilterDTO Store_StoreFilterDTO)
        {
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Skip = Store_StoreFilterDTO.Skip;
            StoreFilter.Take = Store_StoreFilterDTO.Take;
            StoreFilter.OrderBy = Store_StoreFilterDTO.OrderBy;
            StoreFilter.OrderType = Store_StoreFilterDTO.OrderType;
            StoreFilter.Id = Store_StoreFilterDTO.Id;
            StoreFilter.Code = Store_StoreFilterDTO.Code;
            StoreFilter.CreatorId = Store_StoreFilterDTO.CreatorId;
            StoreFilter.CodeDraft = Store_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = Store_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = Store_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = Store_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = Store_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = Store_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StoreUserStatusId = Store_StoreFilterDTO.StoreUserStatusId;
            StoreFilter.Telephone = Store_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = Store_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = Store_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = Store_StoreFilterDTO.WardId;
            StoreFilter.Address = Store_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = Store_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = Store_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = Store_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = Store_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = Store_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = Store_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = Store_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = Store_StoreFilterDTO.OwnerEmail;
            StoreFilter.AppUserId = Store_StoreFilterDTO.AppUserId;
            StoreFilter.StatusId = Store_StoreFilterDTO.StatusId;
            StoreFilter.StoreStatusId = Store_StoreFilterDTO.StoreStatusId;
            StoreFilter.CreatedAt = Store_StoreFilterDTO.CreatedAt;
            return StoreFilter;
        }

        private string ConvertColumnNameExcel(int ColumnNumber)
        {
            string ColumnString = Char.ConvertFromUtf32(ColumnNumber + 64);
            if (ColumnNumber > 26) ColumnString = Char.ConvertFromUtf32((ColumnNumber - 1) / 26 + 64) + Char.ConvertFromUtf32((ColumnNumber - 1) % 26 + 1 + 64);
            return ColumnString;
        }
    }
}
