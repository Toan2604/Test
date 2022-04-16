using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MOrganization;
using DMS.Services.MShowingCategory;
using DMS.Services.MShowingItem;
using DMS.Services.MShowingOrder;
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

namespace DMS.Rpc.posm.showing_order
{
    public partial class ShowingOrderController : RpcController
    {
        private IAppUserService AppUserService;
        private IShowingCategoryService ShowingCategoryService;
        private IOrganizationService OrganizationService;
        private IStatusService StatusService;
        private IShowingItemService ShowingItemService;
        private IUnitOfMeasureService UnitOfMeasureService;
        private IShowingOrderService ShowingOrderService;
        private IStoreService StoreService;
        private ICurrentContext CurrentContext;
        private DataContext DataContext;
        public ShowingOrderController(
            IAppUserService AppUserService,
            IShowingCategoryService ShowingCategoryService,
            IOrganizationService OrganizationService,
            IStatusService StatusService,
            IShowingItemService ShowingItemService,
            IUnitOfMeasureService UnitOfMeasureService,
            IShowingOrderService ShowingOrderService,
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
            this.ShowingOrderService = ShowingOrderService;
            this.StoreService = StoreService;
            this.CurrentContext = CurrentContext;
            this.DataContext = DataContext;
        }

        [Route(ShowingOrderRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] ShowingOrder_ShowingOrderFilterDTO ShowingOrder_ShowingOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingOrderFilter ShowingOrderFilter = ConvertFilterDTOToFilterEntity(ShowingOrder_ShowingOrderFilterDTO);
            ShowingOrderFilter = await ShowingOrderService.ToFilter(ShowingOrderFilter);
            int count = await ShowingOrderService.Count(ShowingOrderFilter);
            return count;
        }

        [Route(ShowingOrderRoute.List), HttpPost]
        public async Task<ActionResult<List<ShowingOrder_ShowingOrderDTO>>> List([FromBody] ShowingOrder_ShowingOrderFilterDTO ShowingOrder_ShowingOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingOrderFilter ShowingOrderFilter = ConvertFilterDTOToFilterEntity(ShowingOrder_ShowingOrderFilterDTO);
            ShowingOrderFilter = await ShowingOrderService.ToFilter(ShowingOrderFilter);
            List<ShowingOrder> ShowingOrders = await ShowingOrderService.List(ShowingOrderFilter);
            List<ShowingOrder_ShowingOrderDTO> ShowingOrder_ShowingOrderDTOs = ShowingOrders
                .Select(c => new ShowingOrder_ShowingOrderDTO(c)).ToList();
            return ShowingOrder_ShowingOrderDTOs;
        }

        [Route(ShowingOrderRoute.Get), HttpPost]
        public async Task<ActionResult<ShowingOrder_ShowingOrderDTO>> Get([FromBody] ShowingOrder_ShowingOrderDTO ShowingOrder_ShowingOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ShowingOrder_ShowingOrderDTO.Id))
                return Forbid();

            ShowingOrder ShowingOrder = await ShowingOrderService.Get(ShowingOrder_ShowingOrderDTO.Id);
            return new ShowingOrder_ShowingOrderDTO(ShowingOrder);
        }

        [Route(ShowingOrderRoute.Create), HttpPost]
        public async Task<ActionResult<ShowingOrder_ShowingOrderDTO>> Create([FromBody] ShowingOrder_ShowingOrderDTO ShowingOrder_ShowingOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ShowingOrder_ShowingOrderDTO.Id))
                return Forbid();

            ShowingOrder ShowingOrder = ConvertDTOToEntity(ShowingOrder_ShowingOrderDTO);
            ShowingOrder = await ShowingOrderService.Create(ShowingOrder);
            ShowingOrder_ShowingOrderDTO = new ShowingOrder_ShowingOrderDTO(ShowingOrder);
            if (ShowingOrder.IsValidated)
                return ShowingOrder_ShowingOrderDTO;
            else
                return BadRequest(ShowingOrder_ShowingOrderDTO);
        }

        [Route(ShowingOrderRoute.Update), HttpPost]
        public async Task<ActionResult<ShowingOrder_ShowingOrderDTO>> Update([FromBody] ShowingOrder_ShowingOrderDTO ShowingOrder_ShowingOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ShowingOrder_ShowingOrderDTO.Id))
                return Forbid();

            ShowingOrder ShowingOrder = ConvertDTOToEntity(ShowingOrder_ShowingOrderDTO);
            ShowingOrder = await ShowingOrderService.Update(ShowingOrder);
            ShowingOrder_ShowingOrderDTO = new ShowingOrder_ShowingOrderDTO(ShowingOrder);
            if (ShowingOrder.IsValidated)
                return ShowingOrder_ShowingOrderDTO;
            else
                return BadRequest(ShowingOrder_ShowingOrderDTO);
        }

        [Route(ShowingOrderRoute.Delete), HttpPost]
        public async Task<ActionResult<ShowingOrder_ShowingOrderDTO>> Delete([FromBody] ShowingOrder_ShowingOrderDTO ShowingOrder_ShowingOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ShowingOrder_ShowingOrderDTO.Id))
                return Forbid();

            ShowingOrder ShowingOrder = ConvertDTOToEntity(ShowingOrder_ShowingOrderDTO);
            ShowingOrder = await ShowingOrderService.Delete(ShowingOrder);
            ShowingOrder_ShowingOrderDTO = new ShowingOrder_ShowingOrderDTO(ShowingOrder);
            if (ShowingOrder.IsValidated)
                return ShowingOrder_ShowingOrderDTO;
            else
                return BadRequest(ShowingOrder_ShowingOrderDTO);
        }

        [Route(ShowingOrderRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] ShowingOrder_ShowingOrderFilterDTO ShowingOrder_ShowingOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = ShowingOrder_ShowingOrderFilterDTO.Date?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ShowingOrder_ShowingOrderFilterDTO.Date.GreaterEqual.Value;

            DateTime End = ShowingOrder_ShowingOrderFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ShowingOrder_ShowingOrderFilterDTO.Date.LessEqual.Value;

            ShowingOrder_ShowingOrderFilterDTO.Skip = 0;
            ShowingOrder_ShowingOrderFilterDTO.Take = int.MaxValue;
            List<ShowingOrder_ShowingOrderDTO> ShowingOrder_ShowingOrderDTOs = (await List(ShowingOrder_ShowingOrderFilterDTO)).Value;
            var Ids = ShowingOrder_ShowingOrderDTOs.Select(x => x.Id).ToList();
            var culture = System.Globalization.CultureInfo.GetCultureInfo("en-EN"); // define locale
            var ShowingOrder_ShowingOrderContentDTOs = await DataContext.ShowingOrderContent
                .Where(x => Ids.Contains(x.ShowingOrderId))
                .Select(x => new ShowingOrder_ShowingOrderContentDTO
                {
                    Id = x.Id,
                    ShowingItemId = x.ShowingItemId,
                    eAmount = x.Amount.ToString("N0", culture),
                    eQuantity = x.Quantity.ToString("N0", culture),
                    eSalePrice = x.SalePrice.ToString("N0", culture),
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    ShowingOrderId = x.ShowingOrderId,
                    ShowingItem = x.ShowingItem == null ? null : new ShowingOrder_ShowingItemDTO
                    {
                        Id = x.ShowingItem.Id,
                        Code = x.ShowingItem.Code,
                        Name = x.ShowingItem.Name,
                        SalePrice = x.ShowingItem.SalePrice,
                        ShowingCategoryId = x.ShowingItem.ShowingCategoryId,
                        ShowingCategory = x.ShowingItem.ShowingCategory == null ? null : new ShowingOrder_ShowingCategoryDTO
                        {
                            Id = x.ShowingItem.ShowingCategory.Id,
                            Code = x.ShowingItem.ShowingCategory.Code,
                            Name = x.ShowingItem.ShowingCategory.Name,
                        },
                    },
                    UnitOfMeasure = x.UnitOfMeasure == null ? null : new ShowingOrder_UnitOfMeasureDTO
                    {
                        Id = x.UnitOfMeasure.Id,
                        Code = x.UnitOfMeasure.Code,
                        Name = x.UnitOfMeasure.Name,
                    }
                }).ToListWithNoLockAsync();

            var stt = 1;
            foreach (var ShowingOrder_ShowingOrderDTO in ShowingOrder_ShowingOrderDTOs)
            {
                ShowingOrder_ShowingOrderDTO.STT = stt++;
                ShowingOrder_ShowingOrderDTO.eDate = ShowingOrder_ShowingOrderDTO.Date.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
                ShowingOrder_ShowingOrderDTO.eTotal = ShowingOrder_ShowingOrderDTO.Total.ToString("N0", culture);
                ShowingOrder_ShowingOrderDTO.ShowingOrderContents = ShowingOrder_ShowingOrderContentDTOs.Where(x => x.ShowingOrderId == ShowingOrder_ShowingOrderDTO.Id).ToList();
            }

            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            string path = "Templates/POSM_Export.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.Data = ShowingOrder_ShowingOrderDTOs;
            Data.Total = ShowingOrder_ShowingOrderDTOs
                .Sum(x => x.Total)
                .ToString("N0", culture);
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "POSM_Export.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            ShowingOrderFilter ShowingOrderFilter = new ShowingOrderFilter();
            ShowingOrderFilter = await ShowingOrderService.ToFilter(ShowingOrderFilter);
            if (Id == 0)
            {

            }
            else
            {
                ShowingOrderFilter.Id = new IdFilter { Equal = Id };
                int count = await ShowingOrderService.Count(ShowingOrderFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private ShowingOrder ConvertDTOToEntity(ShowingOrder_ShowingOrderDTO ShowingOrder_ShowingOrderDTO)
        {
            ShowingOrder_ShowingOrderDTO.TrimString();
            ShowingOrder ShowingOrder = new ShowingOrder();
            ShowingOrder.Id = ShowingOrder_ShowingOrderDTO.Id;
            ShowingOrder.Code = ShowingOrder_ShowingOrderDTO.Code;
            ShowingOrder.AppUserId = ShowingOrder_ShowingOrderDTO.AppUserId;
            ShowingOrder.StoreId = ShowingOrder_ShowingOrderDTO.StoreId;
            ShowingOrder.OrganizationId = ShowingOrder_ShowingOrderDTO.OrganizationId;
            ShowingOrder.Date = ShowingOrder_ShowingOrderDTO.Date;
            ShowingOrder.ShowingWarehouseId = ShowingOrder_ShowingOrderDTO.ShowingWarehouseId;
            ShowingOrder.StatusId = ShowingOrder_ShowingOrderDTO.StatusId;
            ShowingOrder.Total = ShowingOrder_ShowingOrderDTO.Total;
            ShowingOrder.RowId = ShowingOrder_ShowingOrderDTO.RowId;
            ShowingOrder.AppUser = ShowingOrder_ShowingOrderDTO.AppUser == null ? null : new AppUser
            {
                Id = ShowingOrder_ShowingOrderDTO.AppUser.Id,
                Username = ShowingOrder_ShowingOrderDTO.AppUser.Username,
                DisplayName = ShowingOrder_ShowingOrderDTO.AppUser.DisplayName,
                Address = ShowingOrder_ShowingOrderDTO.AppUser.Address,
                Email = ShowingOrder_ShowingOrderDTO.AppUser.Email,
                Phone = ShowingOrder_ShowingOrderDTO.AppUser.Phone,
                SexId = ShowingOrder_ShowingOrderDTO.AppUser.SexId,
                Birthday = ShowingOrder_ShowingOrderDTO.AppUser.Birthday,
                Avatar = ShowingOrder_ShowingOrderDTO.AppUser.Avatar,
                PositionId = ShowingOrder_ShowingOrderDTO.AppUser.PositionId,
                Department = ShowingOrder_ShowingOrderDTO.AppUser.Department,
                OrganizationId = ShowingOrder_ShowingOrderDTO.AppUser.OrganizationId,
                ProvinceId = ShowingOrder_ShowingOrderDTO.AppUser.ProvinceId,
                Longitude = ShowingOrder_ShowingOrderDTO.AppUser.Longitude,
                Latitude = ShowingOrder_ShowingOrderDTO.AppUser.Latitude,
                StatusId = ShowingOrder_ShowingOrderDTO.AppUser.StatusId,
                GPSUpdatedAt = ShowingOrder_ShowingOrderDTO.AppUser.GPSUpdatedAt,
                RowId = ShowingOrder_ShowingOrderDTO.AppUser.RowId,
            };
            ShowingOrder.Organization = ShowingOrder_ShowingOrderDTO.Organization == null ? null : new Organization
            {
                Id = ShowingOrder_ShowingOrderDTO.Organization.Id,
                Code = ShowingOrder_ShowingOrderDTO.Organization.Code,
                Name = ShowingOrder_ShowingOrderDTO.Organization.Name,
                ParentId = ShowingOrder_ShowingOrderDTO.Organization.ParentId,
                Path = ShowingOrder_ShowingOrderDTO.Organization.Path,
                Level = ShowingOrder_ShowingOrderDTO.Organization.Level,
                StatusId = ShowingOrder_ShowingOrderDTO.Organization.StatusId,
                Phone = ShowingOrder_ShowingOrderDTO.Organization.Phone,
                Email = ShowingOrder_ShowingOrderDTO.Organization.Email,
                Address = ShowingOrder_ShowingOrderDTO.Organization.Address,
                RowId = ShowingOrder_ShowingOrderDTO.Organization.RowId,
            };
            ShowingOrder.ShowingWarehouse = ShowingOrder_ShowingOrderDTO.ShowingWarehouse == null ? null : new ShowingWarehouse
            {
                Id = ShowingOrder_ShowingOrderDTO.ShowingWarehouse.Id,
                Code = ShowingOrder_ShowingOrderDTO.ShowingWarehouse.Code,
                Name = ShowingOrder_ShowingOrderDTO.ShowingWarehouse.Name,
                Address = ShowingOrder_ShowingOrderDTO.ShowingWarehouse.Address,
                OrganizationId = ShowingOrder_ShowingOrderDTO.ShowingWarehouse.OrganizationId,
                ProvinceId = ShowingOrder_ShowingOrderDTO.ShowingWarehouse.ProvinceId,
                DistrictId = ShowingOrder_ShowingOrderDTO.ShowingWarehouse.DistrictId,
                WardId = ShowingOrder_ShowingOrderDTO.ShowingWarehouse.WardId,
                StatusId = ShowingOrder_ShowingOrderDTO.ShowingWarehouse.StatusId,
                RowId = ShowingOrder_ShowingOrderDTO.ShowingWarehouse.RowId,
            };
            ShowingOrder.Status = ShowingOrder_ShowingOrderDTO.Status == null ? null : new Status
            {
                Id = ShowingOrder_ShowingOrderDTO.Status.Id,
                Code = ShowingOrder_ShowingOrderDTO.Status.Code,
                Name = ShowingOrder_ShowingOrderDTO.Status.Name,
            };
            ShowingOrder.Store = ShowingOrder_ShowingOrderDTO.Store == null ? null : new Store
            {
                Id = ShowingOrder_ShowingOrderDTO.Store.Id,
                Code = ShowingOrder_ShowingOrderDTO.Store.Code,
                CodeDraft = ShowingOrder_ShowingOrderDTO.Store.CodeDraft,
                Name = ShowingOrder_ShowingOrderDTO.Store.Name,
                UnsignName = ShowingOrder_ShowingOrderDTO.Store.UnsignName,
                ParentStoreId = ShowingOrder_ShowingOrderDTO.Store.ParentStoreId,
                OrganizationId = ShowingOrder_ShowingOrderDTO.Store.OrganizationId,
                StoreTypeId = ShowingOrder_ShowingOrderDTO.Store.StoreTypeId,
                Telephone = ShowingOrder_ShowingOrderDTO.Store.Telephone,
                ProvinceId = ShowingOrder_ShowingOrderDTO.Store.ProvinceId,
                DistrictId = ShowingOrder_ShowingOrderDTO.Store.DistrictId,
                WardId = ShowingOrder_ShowingOrderDTO.Store.WardId,
                Address = ShowingOrder_ShowingOrderDTO.Store.Address,
                UnsignAddress = ShowingOrder_ShowingOrderDTO.Store.UnsignAddress,
                DeliveryAddress = ShowingOrder_ShowingOrderDTO.Store.DeliveryAddress,
                Latitude = ShowingOrder_ShowingOrderDTO.Store.Latitude,
                Longitude = ShowingOrder_ShowingOrderDTO.Store.Longitude,
                DeliveryLatitude = ShowingOrder_ShowingOrderDTO.Store.DeliveryLatitude,
                DeliveryLongitude = ShowingOrder_ShowingOrderDTO.Store.DeliveryLongitude,
                OwnerName = ShowingOrder_ShowingOrderDTO.Store.OwnerName,
                OwnerPhone = ShowingOrder_ShowingOrderDTO.Store.OwnerPhone,
                OwnerEmail = ShowingOrder_ShowingOrderDTO.Store.OwnerEmail,
                TaxCode = ShowingOrder_ShowingOrderDTO.Store.TaxCode,
                LegalEntity = ShowingOrder_ShowingOrderDTO.Store.LegalEntity,
                CreatorId = ShowingOrder_ShowingOrderDTO.Store.CreatorId,
                AppUserId = ShowingOrder_ShowingOrderDTO.Store.AppUserId,
                StatusId = ShowingOrder_ShowingOrderDTO.Store.StatusId,
                RowId = ShowingOrder_ShowingOrderDTO.Store.RowId,
                Used = ShowingOrder_ShowingOrderDTO.Store.Used,
                StoreScoutingId = ShowingOrder_ShowingOrderDTO.Store.StoreScoutingId,
                StoreStatusId = ShowingOrder_ShowingOrderDTO.Store.StoreStatusId,
            };
            ShowingOrder.ShowingOrderContents = ShowingOrder_ShowingOrderDTO.ShowingOrderContents?
                .Select(x => new ShowingOrderContent
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
            ShowingOrder.Stores = ShowingOrder_ShowingOrderDTO.Stores?.Select(x => new Store
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
            ShowingOrder.BaseLanguage = CurrentContext.Language;
            return ShowingOrder;
        }

        private ShowingOrderFilter ConvertFilterDTOToFilterEntity(ShowingOrder_ShowingOrderFilterDTO ShowingOrder_ShowingOrderFilterDTO)
        {
            ShowingOrderFilter ShowingOrderFilter = new ShowingOrderFilter();
            ShowingOrderFilter.Selects = ShowingOrderSelect.ALL;
            ShowingOrderFilter.Skip = ShowingOrder_ShowingOrderFilterDTO.Skip;
            ShowingOrderFilter.Take = ShowingOrder_ShowingOrderFilterDTO.Take;
            ShowingOrderFilter.OrderBy = ShowingOrder_ShowingOrderFilterDTO.OrderBy;
            ShowingOrderFilter.OrderType = ShowingOrder_ShowingOrderFilterDTO.OrderType;

            ShowingOrderFilter.Id = ShowingOrder_ShowingOrderFilterDTO.Id;
            ShowingOrderFilter.Code = ShowingOrder_ShowingOrderFilterDTO.Code;
            ShowingOrderFilter.AppUserId = ShowingOrder_ShowingOrderFilterDTO.AppUserId;
            ShowingOrderFilter.OrganizationId = ShowingOrder_ShowingOrderFilterDTO.OrganizationId;
            ShowingOrderFilter.Date = ShowingOrder_ShowingOrderFilterDTO.Date;
            ShowingOrderFilter.ShowingWarehouseId = ShowingOrder_ShowingOrderFilterDTO.ShowingWarehouseId;
            ShowingOrderFilter.StoreId = ShowingOrder_ShowingOrderFilterDTO.StoreId;
            ShowingOrderFilter.ShowingItemId = ShowingOrder_ShowingOrderFilterDTO.ShowingItemId;
            ShowingOrderFilter.StatusId = ShowingOrder_ShowingOrderFilterDTO.StatusId;
            ShowingOrderFilter.Total = ShowingOrder_ShowingOrderFilterDTO.Total;
            ShowingOrderFilter.RowId = ShowingOrder_ShowingOrderFilterDTO.RowId;
            ShowingOrderFilter.CreatedAt = ShowingOrder_ShowingOrderFilterDTO.CreatedAt;
            ShowingOrderFilter.UpdatedAt = ShowingOrder_ShowingOrderFilterDTO.UpdatedAt;
            return ShowingOrderFilter;
        }
    }
}

