using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MOrganization;
using DMS.Services.MShowingCategory;
using DMS.Services.MShowingItem;
using DMS.Services.MShowingOrderWithDraw;
using DMS.Services.MStatus;
using DMS.Services.MStore;
using DMS.Services.MUnitOfMeasure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_order_with_draw
{
    public partial class ShowingOrderWithDrawController : RpcController
    {
        private IAppUserService AppUserService;
        private IShowingCategoryService ShowingCategoryService;
        private IOrganizationService OrganizationService;
        private IStatusService StatusService;
        private IShowingItemService ShowingItemService;
        private IUnitOfMeasureService UnitOfMeasureService;
        private IShowingOrderWithDrawService ShowingOrderWithDrawService;
        private IStoreService StoreService;
        private ICurrentContext CurrentContext;
        private DataContext DataContext;
        public ShowingOrderWithDrawController(
            IAppUserService AppUserService,
            IShowingCategoryService ShowingCategoryService,
            IOrganizationService OrganizationService,
            IStatusService StatusService,
            IShowingItemService ShowingItemService,
            IUnitOfMeasureService UnitOfMeasureService,
            IShowingOrderWithDrawService ShowingOrderWithDrawService,
            IStoreService StoreService,
            ICurrentContext CurrentContext,
            DataContext DataContext
        )
        {
            this.AppUserService = AppUserService;
            this.ShowingCategoryService = ShowingCategoryService;
            this.OrganizationService = OrganizationService;
            this.StatusService = StatusService;
            this.ShowingItemService = ShowingItemService;
            this.UnitOfMeasureService = UnitOfMeasureService;
            this.ShowingOrderWithDrawService = ShowingOrderWithDrawService;
            this.StoreService = StoreService;
            this.CurrentContext = CurrentContext;
            this.DataContext = DataContext;
        }

        [Route(ShowingOrderWithDrawRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingOrderWithDrawFilter ShowingOrderWithDrawFilter = ConvertFilterDTOToFilterEntity(ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO);
            ShowingOrderWithDrawFilter = await ShowingOrderWithDrawService.ToFilter(ShowingOrderWithDrawFilter);
            int count = await ShowingOrderWithDrawService.Count(ShowingOrderWithDrawFilter);
            return count;
        }

        [Route(ShowingOrderWithDrawRoute.List), HttpPost]
        public async Task<ActionResult<List<ShowingOrderWithDraw_ShowingOrderWithDrawDTO>>> List([FromBody] ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingOrderWithDrawFilter ShowingOrderWithDrawFilter = ConvertFilterDTOToFilterEntity(ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO);
            ShowingOrderWithDrawFilter = await ShowingOrderWithDrawService.ToFilter(ShowingOrderWithDrawFilter);
            List<ShowingOrderWithDraw> ShowingOrderWithDraws = await ShowingOrderWithDrawService.List(ShowingOrderWithDrawFilter);
            List<ShowingOrderWithDraw_ShowingOrderWithDrawDTO> ShowingOrderWithDraw_ShowingOrderWithDrawDTOs = ShowingOrderWithDraws
                .Select(c => new ShowingOrderWithDraw_ShowingOrderWithDrawDTO(c)).ToList();
            return ShowingOrderWithDraw_ShowingOrderWithDrawDTOs;
        }

        [Route(ShowingOrderWithDrawRoute.Get), HttpPost]
        public async Task<ActionResult<ShowingOrderWithDraw_ShowingOrderWithDrawDTO>> Get([FromBody] ShowingOrderWithDraw_ShowingOrderWithDrawDTO ShowingOrderWithDraw_ShowingOrderWithDrawDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Id))
                return Forbid();

            ShowingOrderWithDraw ShowingOrderWithDraw = await ShowingOrderWithDrawService.Get(ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Id);
            return new ShowingOrderWithDraw_ShowingOrderWithDrawDTO(ShowingOrderWithDraw);
        }

        [Route(ShowingOrderWithDrawRoute.Create), HttpPost]
        public async Task<ActionResult<ShowingOrderWithDraw_ShowingOrderWithDrawDTO>> Create([FromBody] ShowingOrderWithDraw_ShowingOrderWithDrawDTO ShowingOrderWithDraw_ShowingOrderWithDrawDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Id))
                return Forbid();

            ShowingOrderWithDraw ShowingOrderWithDraw = ConvertDTOToEntity(ShowingOrderWithDraw_ShowingOrderWithDrawDTO);
            ShowingOrderWithDraw = await ShowingOrderWithDrawService.Create(ShowingOrderWithDraw);
            ShowingOrderWithDraw_ShowingOrderWithDrawDTO = new ShowingOrderWithDraw_ShowingOrderWithDrawDTO(ShowingOrderWithDraw);
            if (ShowingOrderWithDraw.IsValidated)
                return ShowingOrderWithDraw_ShowingOrderWithDrawDTO;
            else
                return BadRequest(ShowingOrderWithDraw_ShowingOrderWithDrawDTO);
        }

        [Route(ShowingOrderWithDrawRoute.Update), HttpPost]
        public async Task<ActionResult<ShowingOrderWithDraw_ShowingOrderWithDrawDTO>> Update([FromBody] ShowingOrderWithDraw_ShowingOrderWithDrawDTO ShowingOrderWithDraw_ShowingOrderWithDrawDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Id))
                return Forbid();

            ShowingOrderWithDraw ShowingOrderWithDraw = ConvertDTOToEntity(ShowingOrderWithDraw_ShowingOrderWithDrawDTO);
            ShowingOrderWithDraw = await ShowingOrderWithDrawService.Update(ShowingOrderWithDraw);
            ShowingOrderWithDraw_ShowingOrderWithDrawDTO = new ShowingOrderWithDraw_ShowingOrderWithDrawDTO(ShowingOrderWithDraw);
            if (ShowingOrderWithDraw.IsValidated)
                return ShowingOrderWithDraw_ShowingOrderWithDrawDTO;
            else
                return BadRequest(ShowingOrderWithDraw_ShowingOrderWithDrawDTO);
        }

        [Route(ShowingOrderWithDrawRoute.Delete), HttpPost]
        public async Task<ActionResult<ShowingOrderWithDraw_ShowingOrderWithDrawDTO>> Delete([FromBody] ShowingOrderWithDraw_ShowingOrderWithDrawDTO ShowingOrderWithDraw_ShowingOrderWithDrawDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Id))
                return Forbid();

            ShowingOrderWithDraw ShowingOrderWithDraw = ConvertDTOToEntity(ShowingOrderWithDraw_ShowingOrderWithDrawDTO);
            ShowingOrderWithDraw = await ShowingOrderWithDrawService.Delete(ShowingOrderWithDraw);
            ShowingOrderWithDraw_ShowingOrderWithDrawDTO = new ShowingOrderWithDraw_ShowingOrderWithDrawDTO(ShowingOrderWithDraw);
            if (ShowingOrderWithDraw.IsValidated)
                return ShowingOrderWithDraw_ShowingOrderWithDrawDTO;
            else
                return BadRequest(ShowingOrderWithDraw_ShowingOrderWithDrawDTO);
        }

        [Route(ShowingOrderWithDrawRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.Date?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.Date.GreaterEqual.Value;

            DateTime End = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.Date.LessEqual.Value;

            ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.Skip = 0;
            ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.Take = int.MaxValue;
            List<ShowingOrderWithDraw_ShowingOrderWithDrawDTO> ShowingOrderWithDraw_ShowingOrderWithDrawDTOs = (await List(ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO)).Value;
            var Ids = ShowingOrderWithDraw_ShowingOrderWithDrawDTOs.Select(x => x.Id).ToList();
            var culture = System.Globalization.CultureInfo.GetCultureInfo("en-EN"); // define locale
            var ShowingOrderWithDraw_ShowingOrderContentWithDrawDTOs = await DataContext.ShowingOrderContentWithDraw
                .Where(x => Ids.Contains(x.ShowingOrderWithDrawId))
                .Select(x => new ShowingOrderWithDraw_ShowingOrderContentWithDrawDTO
                {
                    Id = x.Id,
                    ShowingItemId = x.ShowingItemId,
                    eAmount = x.Amount.ToString("N0", culture),
                    eQuantity = x.Quantity.ToString("N0", culture),
                    eSalePrice = x.SalePrice.ToString("N0", culture),
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    ShowingOrderWithDrawId = x.ShowingOrderWithDrawId,
                    ShowingItem = x.ShowingItem == null ? null : new ShowingOrderWithDraw_ShowingItemDTO
                    {
                        Id = x.ShowingItem.Id,
                        Code = x.ShowingItem.Code,
                        Name = x.ShowingItem.Name,
                        SalePrice = x.ShowingItem.SalePrice,
                        ShowingCategoryId = x.ShowingItem.ShowingCategoryId,
                        ShowingCategory = x.ShowingItem.ShowingCategory == null ? null : new ShowingOrderWithDraw_ShowingCategoryDTO
                        {
                            Id = x.ShowingItem.ShowingCategory.Id,
                            Code = x.ShowingItem.ShowingCategory.Code,
                            Name = x.ShowingItem.ShowingCategory.Name,
                        },
                    },
                    UnitOfMeasure = x.UnitOfMeasure == null ? null : new ShowingOrderWithDraw_UnitOfMeasureDTO
                    {
                        Id = x.UnitOfMeasure.Id,
                        Code = x.UnitOfMeasure.Code,
                        Name = x.UnitOfMeasure.Name,
                    }
                }).ToListWithNoLockAsync();

            var stt = 1;
            foreach (var ShowingOrderWithDraw_ShowingOrderWithDrawDTO in ShowingOrderWithDraw_ShowingOrderWithDrawDTOs)
            {
                ShowingOrderWithDraw_ShowingOrderWithDrawDTO.STT = stt++;
                ShowingOrderWithDraw_ShowingOrderWithDrawDTO.eDate = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Date.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
                ShowingOrderWithDraw_ShowingOrderWithDrawDTO.eTotal = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Total.ToString("N0", culture);
                ShowingOrderWithDraw_ShowingOrderWithDrawDTO.ShowingOrderContentWithDraws = ShowingOrderWithDraw_ShowingOrderContentWithDrawDTOs.Where(x => x.ShowingOrderWithDrawId == ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Id).ToList();
            }

            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            string path = "Templates/POSMWITHDRAW_Export.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.Data = ShowingOrderWithDraw_ShowingOrderWithDrawDTOs;
            Data.Total = ShowingOrderWithDraw_ShowingOrderWithDrawDTOs
                .Sum(x => x.Total)
                .ToString("N0", culture);
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "POSM_WITHDRAW_Export.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            ShowingOrderWithDrawFilter ShowingOrderWithDrawFilter = new ShowingOrderWithDrawFilter();
            ShowingOrderWithDrawFilter = await ShowingOrderWithDrawService.ToFilter(ShowingOrderWithDrawFilter);
            if (Id == 0)
            {

            }
            else
            {
                ShowingOrderWithDrawFilter.Id = new IdFilter { Equal = Id };
                int count = await ShowingOrderWithDrawService.Count(ShowingOrderWithDrawFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private ShowingOrderWithDraw ConvertDTOToEntity(ShowingOrderWithDraw_ShowingOrderWithDrawDTO ShowingOrderWithDraw_ShowingOrderWithDrawDTO)
        {
            ShowingOrderWithDraw_ShowingOrderWithDrawDTO.TrimString();
            ShowingOrderWithDraw ShowingOrderWithDraw = new ShowingOrderWithDraw();
            ShowingOrderWithDraw.Id = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Id;
            ShowingOrderWithDraw.Code = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Code;
            ShowingOrderWithDraw.AppUserId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUserId;
            ShowingOrderWithDraw.StoreId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.StoreId;
            ShowingOrderWithDraw.OrganizationId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.OrganizationId;
            ShowingOrderWithDraw.Date = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Date;
            ShowingOrderWithDraw.ShowingWarehouseId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.ShowingWarehouseId;
            ShowingOrderWithDraw.StatusId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.StatusId;
            ShowingOrderWithDraw.Total = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Total;
            ShowingOrderWithDraw.RowId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.RowId;
            ShowingOrderWithDraw.AppUser = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser == null ? null : new AppUser
            {
                Id = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.Id,
                Username = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.Username,
                DisplayName = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.DisplayName,
                Address = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.Address,
                Email = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.Email,
                Phone = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.Phone,
                SexId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.SexId,
                Birthday = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.Birthday,
                Avatar = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.Avatar,
                PositionId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.PositionId,
                Department = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.Department,
                OrganizationId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.OrganizationId,
                ProvinceId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.ProvinceId,
                Longitude = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.Longitude,
                Latitude = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.Latitude,
                StatusId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.StatusId,
                GPSUpdatedAt = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.GPSUpdatedAt,
                RowId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.AppUser.RowId,
            };
            ShowingOrderWithDraw.Organization = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Organization == null ? null : new Organization
            {
                Id = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Organization.Id,
                Code = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Organization.Code,
                Name = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Organization.Name,
                ParentId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Organization.ParentId,
                Path = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Organization.Path,
                Level = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Organization.Level,
                StatusId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Organization.StatusId,
                Phone = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Organization.Phone,
                Email = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Organization.Email,
                Address = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Organization.Address,
                RowId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Organization.RowId,
            };
            ShowingOrderWithDraw.ShowingWarehouse = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.ShowingWarehouse == null ? null : new ShowingWarehouse
            {
                Id = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.ShowingWarehouse.Id,
                Code = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.ShowingWarehouse.Code,
                Name = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.ShowingWarehouse.Name,
                Address = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.ShowingWarehouse.Address,
                OrganizationId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.ShowingWarehouse.OrganizationId,
                ProvinceId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.ShowingWarehouse.ProvinceId,
                DistrictId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.ShowingWarehouse.DistrictId,
                WardId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.ShowingWarehouse.WardId,
                StatusId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.ShowingWarehouse.StatusId,
                RowId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.ShowingWarehouse.RowId,
            };
            ShowingOrderWithDraw.Status = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Status == null ? null : new Status
            {
                Id = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Status.Id,
                Code = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Status.Code,
                Name = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Status.Name,
            };
            ShowingOrderWithDraw.Store = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store == null ? null : new Store
            {
                Id = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.Id,
                Code = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.Code,
                CodeDraft = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.CodeDraft,
                Name = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.Name,
                UnsignName = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.UnsignName,
                ParentStoreId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.ParentStoreId,
                OrganizationId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.OrganizationId,
                StoreTypeId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.StoreTypeId,
                Telephone = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.Telephone,
                ProvinceId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.ProvinceId,
                DistrictId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.DistrictId,
                WardId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.WardId,
                Address = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.Address,
                UnsignAddress = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.UnsignAddress,
                DeliveryAddress = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.DeliveryAddress,
                Latitude = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.Latitude,
                Longitude = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.Longitude,
                DeliveryLatitude = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.DeliveryLatitude,
                DeliveryLongitude = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.DeliveryLongitude,
                OwnerName = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.OwnerName,
                OwnerPhone = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.OwnerPhone,
                OwnerEmail = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.OwnerEmail,
                TaxCode = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.TaxCode,
                LegalEntity = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.LegalEntity,
                CreatorId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.CreatorId,
                AppUserId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.AppUserId,
                StatusId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.StatusId,
                RowId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.RowId,
                Used = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.Used,
                StoreScoutingId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.StoreScoutingId,
                StoreStatusId = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Store.StoreStatusId,
            };
            ShowingOrderWithDraw.ShowingOrderContentWithDraws = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.ShowingOrderContentWithDraws?
                .Select(x => new ShowingOrderContentWithDraw
                {
                    Id = x.Id,
                    ShowingItemId = x.ShowingItemId,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    SalePrice = x.SalePrice,
                    Quantity = x.Quantity,
                    Amount = x.Amount,
                    ShowingItem = x.ShowingItem == null ? null : new ShowingItem
                    {
                        Id = x.ShowingItem.Id,
                        Code = x.ShowingItem.Code,
                        Name = x.ShowingItem.Name,
                        ShowingCategoryId = x.ShowingItem.ShowingCategoryId,
                        UnitOfMeasureId = x.ShowingItem.UnitOfMeasureId,
                        SalePrice = x.ShowingItem.SalePrice,
                        Description = x.ShowingItem.Description,
                        StatusId = x.ShowingItem.StatusId,
                        Used = x.ShowingItem.Used,
                        RowId = x.ShowingItem.RowId,
                    },
                    UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                    {
                        Id = x.UnitOfMeasure.Id,
                        Code = x.UnitOfMeasure.Code,
                        Name = x.UnitOfMeasure.Name,
                        Description = x.UnitOfMeasure.Description,
                        StatusId = x.UnitOfMeasure.StatusId,
                        Used = x.UnitOfMeasure.Used,
                        RowId = x.UnitOfMeasure.RowId,
                    },
                }).ToList();
            ShowingOrderWithDraw.Stores = ShowingOrderWithDraw_ShowingOrderWithDrawDTO.Stores?.Select(x => new Store
            {
                Id = x.Id,
                Code = x.Code,
                CodeDraft = x.CodeDraft,
                Name = x.Name,
                UnsignName = x.UnsignName,
                ParentStoreId = x.ParentStoreId,
                OrganizationId = x.OrganizationId,
                StoreTypeId = x.StoreTypeId,
                Telephone = x.Telephone,
                ProvinceId = x.ProvinceId,
                DistrictId = x.DistrictId,
                WardId = x.WardId,
                Address = x.Address,
                UnsignAddress = x.UnsignAddress,
                DeliveryAddress = x.DeliveryAddress,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                DeliveryLatitude = x.DeliveryLatitude,
                DeliveryLongitude = x.DeliveryLongitude,
                OwnerName = x.OwnerName,
                OwnerPhone = x.OwnerPhone,
                OwnerEmail = x.OwnerEmail,
                TaxCode = x.TaxCode,
                LegalEntity = x.LegalEntity,
                CreatorId = x.CreatorId,
                AppUserId = x.AppUserId,
                StatusId = x.StatusId,
                RowId = x.RowId,
                Used = x.Used,
                StoreScoutingId = x.StoreScoutingId,
                StoreStatusId = x.StoreStatusId,
            }).ToList();
            ShowingOrderWithDraw.BaseLanguage = CurrentContext.Language;
            return ShowingOrderWithDraw;
        }

        private ShowingOrderWithDrawFilter ConvertFilterDTOToFilterEntity(ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO)
        {
            ShowingOrderWithDrawFilter ShowingOrderWithDrawFilter = new ShowingOrderWithDrawFilter();
            ShowingOrderWithDrawFilter.Selects = ShowingOrderWithDrawSelect.ALL;
            ShowingOrderWithDrawFilter.Skip = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.Skip;
            ShowingOrderWithDrawFilter.Take = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.Take;
            ShowingOrderWithDrawFilter.OrderBy = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.OrderBy;
            ShowingOrderWithDrawFilter.OrderType = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.OrderType;

            ShowingOrderWithDrawFilter.Id = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.Id;
            ShowingOrderWithDrawFilter.Code = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.Code;
            ShowingOrderWithDrawFilter.AppUserId = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.AppUserId;
            ShowingOrderWithDrawFilter.OrganizationId = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.OrganizationId;
            ShowingOrderWithDrawFilter.Date = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.Date;
            ShowingOrderWithDrawFilter.ShowingWarehouseId = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.ShowingWarehouseId;
            ShowingOrderWithDrawFilter.StoreId = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.StoreId;
            ShowingOrderWithDrawFilter.ShowingItemId = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.ShowingItemId;
            ShowingOrderWithDrawFilter.StatusId = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.StatusId;
            ShowingOrderWithDrawFilter.Total = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.Total;
            ShowingOrderWithDrawFilter.RowId = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.RowId;
            ShowingOrderWithDrawFilter.CreatedAt = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.CreatedAt;
            ShowingOrderWithDrawFilter.UpdatedAt = ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO.UpdatedAt;
            return ShowingOrderWithDrawFilter;
        }
    }
}

