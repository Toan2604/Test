using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public partial class MobileController
    {
        private const long THIS_MONTH = 1;
        private const long LAST_MONTH = 2;
        private const long THIS_QUARTER = 3;
        private const long YEAR = 4;
        private const long TODAY = 5;
        private const long THIS_WEEK = 6;
        private const long LAST_WEEK = 7;

        [Route(GeneralMobileRoute.SingleListTime), HttpPost]
        public List<GeneralMobile_EnumList> SingleListTime()
        {
            List<GeneralMobile_EnumList> Dashborad_EnumLists = new List<GeneralMobile_EnumList>();
            Dashborad_EnumLists.Add(new GeneralMobile_EnumList { Id = THIS_MONTH, Name = "Tháng này" });
            Dashborad_EnumLists.Add(new GeneralMobile_EnumList { Id = LAST_MONTH, Name = "Tháng trước" });
            Dashborad_EnumLists.Add(new GeneralMobile_EnumList { Id = THIS_QUARTER, Name = "Quý này" });
            Dashborad_EnumLists.Add(new GeneralMobile_EnumList { Id = YEAR, Name = "Năm" });
            return Dashborad_EnumLists;
        }

        [Route(GeneralMobileRoute.FilterListTime), HttpPost]
        public List<GeneralMobile_EnumList> FilterListTime()
        {
            List<GeneralMobile_EnumList> Dashborad_EnumLists = new List<GeneralMobile_EnumList>();
            Dashborad_EnumLists.Add(new GeneralMobile_EnumList { Id = 1, Name = "Hôm nay" });
            Dashborad_EnumLists.Add(new GeneralMobile_EnumList { Id = 2, Name = "3 ngày trước" });
            Dashborad_EnumLists.Add(new GeneralMobile_EnumList { Id = 3, Name = "10 ngày trước" });
            Dashborad_EnumLists.Add(new GeneralMobile_EnumList { Id = 4, Name = "30 ngày trước" });
            return Dashborad_EnumLists;
        }

        [Route(GeneralMobileRoute.FilterListDay), HttpPost]
        public List<GeneralMobile_EnumList> FilterListDay()
        {
            List<GeneralMobile_EnumList> Dashborad_EnumLists = new List<GeneralMobile_EnumList>();
            Dashborad_EnumLists.Add(new GeneralMobile_EnumList { Id = 1, Name = "Ngày thứ 1" });
            Dashborad_EnumLists.Add(new GeneralMobile_EnumList { Id = 2, Name = "Ngày thứ 2" });
            Dashborad_EnumLists.Add(new GeneralMobile_EnumList { Id = 3, Name = "Ngày thứ 3" });
            return Dashborad_EnumLists;
        }

        [Route(GeneralMobileRoute.SingleListSalesOrderType), HttpPost]
        public List<GenericEnum> SingleListSalesOrderType()
        {
            return SalesOrderTypeEnum.SalesOrderTypeEnumList;
        }

        [Route(GeneralMobileRoute.SingleListAlbum), HttpPost]
        public async Task<List<GeneralMobile_AlbumDTO>> SingleListAlbum([FromBody] GeneralMobile_AlbumFilterDTO GeneralMobile_AlbumFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AlbumFilter AlbumFilter = new AlbumFilter();
            AlbumFilter.Skip = 0;
            AlbumFilter.Take = 10;
            AlbumFilter.OrderBy = AlbumOrder.Id;
            AlbumFilter.OrderType = OrderType.ASC;
            AlbumFilter.Selects = AlbumSelect.ALL;
            AlbumFilter.Id = GeneralMobile_AlbumFilterDTO.Id;
            AlbumFilter.Name = GeneralMobile_AlbumFilterDTO.Name;
            AlbumFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Album> Albums = await AlbumService.List(AlbumFilter);
            List<GeneralMobile_AlbumDTO> GeneralMobile_AlbumDTOs = Albums
                .Select(x => new GeneralMobile_AlbumDTO(x)).ToList();
            return GeneralMobile_AlbumDTOs;
        }
        [Route(GeneralMobileRoute.SingleListAppUser), HttpPost]
        public async Task<List<GeneralMobile_AppUserDTO>> SingleListAppUser([FromBody] GeneralMobile_AppUserFilterDTO GeneralMobile_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = GeneralMobile_AppUserFilterDTO.Id;
            AppUserFilter.Username = GeneralMobile_AppUserFilterDTO.Username;
            AppUserFilter.Password = GeneralMobile_AppUserFilterDTO.Password;
            AppUserFilter.DisplayName = GeneralMobile_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = GeneralMobile_AppUserFilterDTO.Address;
            AppUserFilter.Email = GeneralMobile_AppUserFilterDTO.Email;
            AppUserFilter.Phone = GeneralMobile_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = GeneralMobile_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = GeneralMobile_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = GeneralMobile_AppUserFilterDTO.OrganizationId;
            AppUserFilter.SexId = GeneralMobile_AppUserFilterDTO.SexId;
            AppUserFilter.StatusId = GeneralMobile_AppUserFilterDTO.StatusId;
            AppUserFilter.Birthday = GeneralMobile_AppUserFilterDTO.Birthday;
            AppUserFilter.ProvinceId = GeneralMobile_AppUserFilterDTO.ProvinceId;

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<GeneralMobile_AppUserDTO> GeneralMobile_AppUserDTOs = AppUsers
                .Select(x => new GeneralMobile_AppUserDTO(x)).ToList();
            return GeneralMobile_AppUserDTOs;
        }

        [Route(GeneralMobileRoute.SingleListEroute), HttpPost]
        public async Task<List<GeneralMobile_ERouteDTO>> SingleListEroute(GeneralMobile_ERouteFilterDTO GeneralMobile_ERouteFilterDTO)
        {
            ERouteFilter ERouteFilter = new ERouteFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                AppUserId = new IdFilter { Equal = CurrentContext.UserId },
                Selects = ERouteSelect.Id | ERouteSelect.Name | ERouteSelect.Code
            };

            List<ERoute> ERoutes = await ERouteService.List(ERouteFilter);
            List<GeneralMobile_ERouteDTO> GeneralMobile_ERouteDTOs = ERoutes
                .Select(x => new GeneralMobile_ERouteDTO(x)).ToList();
            return GeneralMobile_ERouteDTOs;
        }

        [Route(GeneralMobileRoute.SingleListStore), HttpPost]
        public async Task<List<GeneralMobile_StoreDTO>> SingleListStore([FromBody] GeneralMobile_StoreFilterDTO GeneralMobile_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.Id |StoreSelect.Address | StoreSelect.AppUser | StoreSelect.Code |StoreSelect.CodeDraft |StoreSelect.DeliveryAddress 
                | StoreSelect.DeliveryLatitude | StoreSelect.DeliveryLongitude | StoreSelect.HasChecking | StoreSelect.IsStoreApprovalDirectSalesOrder 
                | StoreSelect.Latitude | StoreSelect.Longitude | StoreSelect.Name | StoreSelect.Organization | StoreSelect.OwnerEmail | StoreSelect.OwnerEmail
                | StoreSelect.OwnerName | StoreSelect.OwnerPhone | StoreSelect.StoreStatusId | StoreSelect.StoreStatus | StoreSelect.StoreType | StoreSelect.Telephone 
                | StoreSelect.Province | StoreSelect.District | StoreSelect.Ward |StoreSelect.Description | StoreSelect.DebtLimited;
            StoreFilter.Id = GeneralMobile_StoreFilterDTO.Id;
            StoreFilter.Code = GeneralMobile_StoreFilterDTO.Code;
            StoreFilter.Name = GeneralMobile_StoreFilterDTO.Name;
            StoreFilter.DeliveryAddress = GeneralMobile_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = GeneralMobile_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = GeneralMobile_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = GeneralMobile_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = GeneralMobile_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = GeneralMobile_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = GeneralMobile_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = GeneralMobile_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = GeneralMobile_StoreFilterDTO.StoreStatusId;
            StoreFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            AppUser AppUser = await AppUserService.Get(CurrentContext.UserId);
            StoreFilter.OrganizationId = new IdFilter { Equal = AppUser.OrganizationId };

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<GeneralMobile_StoreDTO> GeneralMobile_StoreDTOs = Stores
                .Select(x => new GeneralMobile_StoreDTO(x)).ToList();
            return GeneralMobile_StoreDTOs;
        }

        [Route(GeneralMobileRoute.SingleListEstimatedRevenue), HttpPost]
        public async Task<List<GeneralMobile_StoreEstimatedRevenueDTO>> SingleListEstimatedRevenue([FromBody] GeneralMobile_StoreEstimatedRevenueFilterDTO GeneralMobile_StoreEstimatedRevenueFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            EstimatedRevenueFilter EstimatedRevenueFilter = new EstimatedRevenueFilter();
            EstimatedRevenueFilter.Skip = 0;
            EstimatedRevenueFilter.Take = 20;
            EstimatedRevenueFilter.OrderBy = EstimatedRevenueOrder.Id;
            EstimatedRevenueFilter.OrderType = OrderType.ASC;
            EstimatedRevenueFilter.Selects = EstimatedRevenueSelect.ALL;
            EstimatedRevenueFilter.Id = GeneralMobile_StoreEstimatedRevenueFilterDTO.Id;
            EstimatedRevenueFilter.Code = GeneralMobile_StoreEstimatedRevenueFilterDTO.Code;
            EstimatedRevenueFilter.Name = GeneralMobile_StoreEstimatedRevenueFilterDTO.Name;

            List<EstimatedRevenue> EstimatedRevenuees = await EstimatedRevenueService.List(EstimatedRevenueFilter);
            List<GeneralMobile_StoreEstimatedRevenueDTO> GeneralMobile_StoreEstimatedRevenueDTOs = EstimatedRevenuees
                .Select(x => new GeneralMobile_StoreEstimatedRevenueDTO(x)).ToList();
            return GeneralMobile_StoreEstimatedRevenueDTOs;
        }

        [Route(GeneralMobileRoute.SingleListStoreGrouping), HttpPost]
        public async Task<List<GeneralMobile_StoreGroupingDTO>> SingleListStoreGrouping([FromBody] GeneralMobile_StoreGroupingFilterDTO GeneralMobile_StoreGroupingFilterDTO)
        {
            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = 99999;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Code = GeneralMobile_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = GeneralMobile_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.Level = GeneralMobile_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.Path = GeneralMobile_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<GeneralMobile_StoreGroupingDTO> GeneralMobile_StoreGroupingDTOs = StoreGroupings
                .Select(x => new GeneralMobile_StoreGroupingDTO(x)).ToList();
            return GeneralMobile_StoreGroupingDTOs;
        }

        [Route(GeneralMobileRoute.SingleListStoreStatus), HttpPost]
        public async Task<List<GeneralMobile_StoreStatusDTO>> SingleListStoreStatus([FromBody] GeneralMobile_StoreStatusFilterDTO GeneralMobile_StoreStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreStatusFilter StoreStatusFilter = new StoreStatusFilter();
            StoreStatusFilter.Skip = 0;
            StoreStatusFilter.Take = 20;
            StoreStatusFilter.OrderBy = StoreStatusOrder.Id;
            StoreStatusFilter.OrderType = OrderType.ASC;
            StoreStatusFilter.Selects = StoreStatusSelect.ALL;
            StoreStatusFilter.Id = new IdFilter { NotEqual = StoreStatusEnum.ALL.Id };
            StoreStatusFilter.Code = GeneralMobile_StoreStatusFilterDTO.Code;
            StoreStatusFilter.Name = GeneralMobile_StoreStatusFilterDTO.Name;

            List<StoreStatus> StoreStatuses = await StoreStatusService.List(StoreStatusFilter);
            List<GeneralMobile_StoreStatusDTO> GeneralMobile_StoreStatusDTOs = StoreStatuses
                .Select(x => new GeneralMobile_StoreStatusDTO(x)).ToList();
            return GeneralMobile_StoreStatusDTOs;
        }

        [Route(GeneralMobileRoute.SingleListStoreType), HttpPost]
        public async Task<List<GeneralMobile_StoreTypeDTO>> SingleListStoreType([FromBody] GeneralMobile_StoreTypeFilterDTO GeneralMobile_StoreTypeFilterDTO)
        {
            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = GeneralMobile_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = GeneralMobile_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = GeneralMobile_StoreTypeFilterDTO.Name;
            StoreTypeFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<GeneralMobile_StoreTypeDTO> GeneralMobile_StoreTypeDTOs = StoreTypes
                .Select(x => new GeneralMobile_StoreTypeDTO(x)).ToList();
            return GeneralMobile_StoreTypeDTOs;
        }

        [Route(GeneralMobileRoute.SingleListTaxType), HttpPost]
        public async Task<List<GeneralMobile_TaxTypeDTO>> SingleListTaxType([FromBody] GeneralMobile_TaxTypeFilterDTO GeneralMobile_TaxTypeFilterDTO)
        {
            TaxTypeFilter TaxTypeFilter = new TaxTypeFilter();
            TaxTypeFilter.Skip = 0;
            TaxTypeFilter.Take = 20;
            TaxTypeFilter.OrderBy = TaxTypeOrder.Id;
            TaxTypeFilter.OrderType = OrderType.ASC;
            TaxTypeFilter.Selects = TaxTypeSelect.ALL;
            TaxTypeFilter.Id = GeneralMobile_TaxTypeFilterDTO.Id;
            TaxTypeFilter.Code = GeneralMobile_TaxTypeFilterDTO.Code;
            TaxTypeFilter.Name = GeneralMobile_TaxTypeFilterDTO.Name;
            TaxTypeFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<TaxType> TaxTypes = await TaxTypeService.List(TaxTypeFilter);
            List<GeneralMobile_TaxTypeDTO> GeneralMobile_TaxTypeDTOs = TaxTypes
                .Select(x => new GeneralMobile_TaxTypeDTO(x)).ToList();
            return GeneralMobile_TaxTypeDTOs;
        }

        [Route(GeneralMobileRoute.SingleListUnitOfMeasure), HttpPost]
        public async Task<List<GeneralMobile_UnitOfMeasureDTO>> SingleListUnitOfMeasure([FromBody] GeneralMobile_UnitOfMeasureFilterDTO GeneralMobile_UnitOfMeasureFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var Product = await ProductService.MobileGet(GeneralMobile_UnitOfMeasureFilterDTO.ProductId.Equal ?? 0);

            List<GeneralMobile_UnitOfMeasureDTO> GeneralMobile_UnitOfMeasureDTOs = new List<GeneralMobile_UnitOfMeasureDTO>();
            if (Product != null && Product.UnitOfMeasureGrouping != null && Product.UnitOfMeasureGrouping.StatusId == Enums.StatusEnum.ACTIVE.Id)
            {
                GeneralMobile_UnitOfMeasureDTOs = Product.UnitOfMeasureGrouping.UnitOfMeasureGroupingContents.Select(x => new GeneralMobile_UnitOfMeasureDTO(x)).ToList();
            }
            if (Product != null)
            {
                GeneralMobile_UnitOfMeasureDTOs.Add(new GeneralMobile_UnitOfMeasureDTO
                {
                    Id = Product.UnitOfMeasure.Id,
                    Code = Product.UnitOfMeasure.Code,
                    Name = Product.UnitOfMeasure.Name,
                    Description = Product.UnitOfMeasure.Description,
                    StatusId = Product.UnitOfMeasure.StatusId,
                    Factor = 1
                });
            }

            return GeneralMobile_UnitOfMeasureDTOs;
        }

        [Route(GeneralMobileRoute.SingleListProblemType), HttpPost]
        public async Task<List<GeneralMobile_ProblemTypeDTO>> SingleListProblemType()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProblemTypeFilter ProblemTypeFilter = new ProblemTypeFilter();
            ProblemTypeFilter.Skip = 0;
            ProblemTypeFilter.Take = 20;
            ProblemTypeFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            ProblemTypeFilter.OrderBy = ProblemTypeOrder.Id;
            ProblemTypeFilter.OrderType = OrderType.ASC;
            ProblemTypeFilter.Selects = ProblemTypeSelect.ALL;

            List<ProblemType> ProblemTypes = await ProblemTypeService.List(ProblemTypeFilter);
            List<GeneralMobile_ProblemTypeDTO> GeneralMobile_ProblemTypeDTOs = ProblemTypes

                .Select(x => new GeneralMobile_ProblemTypeDTO(x)).ToList();
            return GeneralMobile_ProblemTypeDTOs;
        }

        [Route(GeneralMobileRoute.SingleListStoreScoutingType), HttpPost]
        public async Task<List<GeneralMobile_StoreScoutingTypeDTO>> SingleListStoreScoutingType()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreScoutingTypeFilter StoreScoutingTypeFilter = new StoreScoutingTypeFilter();
            StoreScoutingTypeFilter.Skip = 0;
            StoreScoutingTypeFilter.Take = 20;
            StoreScoutingTypeFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreScoutingTypeFilter.OrderBy = StoreScoutingTypeOrder.Id;
            StoreScoutingTypeFilter.OrderType = OrderType.ASC;
            StoreScoutingTypeFilter.Selects = StoreScoutingTypeSelect.ALL;

            List<StoreScoutingType> StoreScoutingTypes = await StoreScoutingTypeService.List(StoreScoutingTypeFilter);
            List<GeneralMobile_StoreScoutingTypeDTO> GeneralMobile_StoreScoutingTypeDTOs = StoreScoutingTypes

                .Select(x => new GeneralMobile_StoreScoutingTypeDTO(x)).ToList();
            return GeneralMobile_StoreScoutingTypeDTOs;
        }

        [Route(GeneralMobileRoute.SingleListBrand), HttpPost]
        public async Task<List<GeneralMobile_BrandDTO>> SingleListBrand([FromBody] GeneralMobile_BrandFilterDTO GeneralMobile_BrandFilterDTO)
        {
            BrandFilter BrandFilter = new BrandFilter();
            BrandFilter.Skip = 0;
            BrandFilter.Take = int.MaxValue;
            BrandFilter.OrderBy = BrandOrder.Id;
            BrandFilter.OrderType = OrderType.ASC;
            BrandFilter.Selects = BrandSelect.ALL;
            BrandFilter.Id = GeneralMobile_BrandFilterDTO.Id;
            BrandFilter.Code = GeneralMobile_BrandFilterDTO.Code;
            BrandFilter.Name = GeneralMobile_BrandFilterDTO.Name;
            BrandFilter.Description = GeneralMobile_BrandFilterDTO.Description;
            BrandFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            BrandFilter.UpdateTime = GeneralMobile_BrandFilterDTO.UpdateTime;

            List<Brand> Brands = await BrandService.List(BrandFilter);
            List<GeneralMobile_BrandDTO> GeneralMobile_BrandDTOs = Brands
                .Select(x => new GeneralMobile_BrandDTO(x)).ToList();
            return GeneralMobile_BrandDTOs;
        }

        [Route(GeneralMobileRoute.SingleListColor), HttpPost]
        public async Task<List<GeneralMobile_ColorDTO>> SingleListColor([FromBody] GeneralMobile_ColorFilterDTO GeneralMobile_ColorFilterDTO)
        {
            ColorFilter ColorFilter = new ColorFilter();
            ColorFilter.Skip = 0;
            ColorFilter.Take = 20;
            ColorFilter.OrderBy = ColorOrder.Id;
            ColorFilter.OrderType = OrderType.ASC;
            ColorFilter.Selects = ColorSelect.ALL;

            List<Color> Colores = await ColorService.List(ColorFilter);
            List<GeneralMobile_ColorDTO> GeneralMobile_ColorDTOs = Colores
                .Select(x => new GeneralMobile_ColorDTO(x)).ToList();
            return GeneralMobile_ColorDTOs;
        }

        [Route(GeneralMobileRoute.SingleListSupplier), HttpPost]
        public async Task<List<GeneralMobile_SupplierDTO>> SingleListSupplier([FromBody] GeneralMobile_SupplierFilterDTO GeneralMobile_SupplierFilterDTO)
        {
            SupplierFilter SupplierFilter = new SupplierFilter();
            SupplierFilter.Skip = 0;
            SupplierFilter.Take = 20;
            SupplierFilter.OrderBy = SupplierOrder.Id;
            SupplierFilter.OrderType = OrderType.ASC;
            SupplierFilter.Selects = SupplierSelect.ALL;
            SupplierFilter.Id = GeneralMobile_SupplierFilterDTO.Id;
            SupplierFilter.Code = GeneralMobile_SupplierFilterDTO.Code;
            SupplierFilter.Name = GeneralMobile_SupplierFilterDTO.Name;
            SupplierFilter.TaxCode = GeneralMobile_SupplierFilterDTO.TaxCode;
            SupplierFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Supplier> Suppliers = await SupplierService.List(SupplierFilter);
            List<GeneralMobile_SupplierDTO> GeneralMobile_SupplierDTOs = Suppliers
                .Select(x => new GeneralMobile_SupplierDTO(x)).ToList();
            return GeneralMobile_SupplierDTOs;
        }

        [Route(GeneralMobileRoute.SingleListProductGrouping), HttpPost]
        public async Task<List<GeneralMobile_ProductGroupingDTO>> SingleListProductGrouping([FromBody] GeneralMobile_ProductGroupingFilterDTO GeneralMobile_ProductGroupingFilterDTO)
        {
            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter();
            ProductGroupingFilter.Skip = 0;
            ProductGroupingFilter.Take = int.MaxValue;
            ProductGroupingFilter.OrderBy = ProductGroupingOrder.Id;
            ProductGroupingFilter.OrderType = OrderType.ASC;
            ProductGroupingFilter.Selects = ProductGroupingSelect.Id | ProductGroupingSelect.Code
                | ProductGroupingSelect.Name | ProductGroupingSelect.Parent;
            ProductGroupingFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<ProductGrouping> ProductGroupings = await ProductGroupingService.List(ProductGroupingFilter);
            ProductGroupings = ProductGroupings.Where(x => x.HasChildren == false).ToList();
            List<GeneralMobile_ProductGroupingDTO> GeneralMobile_ProductGroupingDTOs = ProductGroupings
                .Select(x => new GeneralMobile_ProductGroupingDTO(x)).ToList();
            return GeneralMobile_ProductGroupingDTOs;
        }

        [Route(GeneralMobileRoute.SingleListProvince), HttpPost]
        public async Task<List<GeneralMobile_ProvinceDTO>> SingleListProvince([FromBody] GeneralMobile_ProvinceFilterDTO GeneralMobile_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = GeneralMobile_ProvinceFilterDTO.Skip;
            ProvinceFilter.Take = GeneralMobile_ProvinceFilterDTO.Take;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = GeneralMobile_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = GeneralMobile_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<GeneralMobile_ProvinceDTO> GeneralMobile_ProvinceDTOs = Provinces
                .Select(x => new GeneralMobile_ProvinceDTO(x)).ToList();
            return GeneralMobile_ProvinceDTOs;
        }
        [Route(GeneralMobileRoute.SingleListDistrict), HttpPost]
        public async Task<List<GeneralMobile_DistrictDTO>> SingleListDistrict([FromBody] GeneralMobile_DistrictFilterDTO GeneralMobile_DistrictFilterDTO)
        {
            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = GeneralMobile_DistrictFilterDTO.Skip;
            DistrictFilter.Take = GeneralMobile_DistrictFilterDTO.Take;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = GeneralMobile_DistrictFilterDTO.Id;
            DistrictFilter.Name = GeneralMobile_DistrictFilterDTO.Name;
            DistrictFilter.Priority = GeneralMobile_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = GeneralMobile_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<GeneralMobile_DistrictDTO> GeneralMobile_DistrictDTOs = Districts
                .Select(x => new GeneralMobile_DistrictDTO(x)).ToList();
            return GeneralMobile_DistrictDTOs;
        }
        [Route(GeneralMobileRoute.SingleListWard), HttpPost]
        public async Task<List<GeneralMobile_WardDTO>> SingleListWard([FromBody] GeneralMobile_WardFilterDTO GeneralMobile_WardFilterDTO)
        {
            WardFilter WardFilter = new WardFilter();
            WardFilter.Skip = GeneralMobile_WardFilterDTO.Skip;
            WardFilter.Take = GeneralMobile_WardFilterDTO.Take;
            WardFilter.OrderBy = WardOrder.Priority;
            WardFilter.OrderType = OrderType.ASC;
            WardFilter.Selects = WardSelect.ALL;
            WardFilter.Id = GeneralMobile_WardFilterDTO.Id;
            WardFilter.Name = GeneralMobile_WardFilterDTO.Name;
            WardFilter.DistrictId = GeneralMobile_WardFilterDTO.DistrictId;
            WardFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            List<Ward> Wards = await WardService.List(WardFilter);
            List<GeneralMobile_WardDTO> GeneralMobile_WardDTOs = Wards
                .Select(x => new GeneralMobile_WardDTO(x)).ToList();
            return GeneralMobile_WardDTOs;
        }

        [Route(GeneralMobileRoute.SingleListStoreCheckingStatus), HttpPost]
        public async Task<List<GeneralMobile_StoreCheckingStatusDTO>> SingleListStoreCheckingStatus([FromBody] GeneralMobile_StoreCheckingStatusFilterDTO GeneralMobile_StoreCheckingStatusFilterDTO)
        {
            List<GeneralMobile_StoreCheckingStatusDTO> GeneralMobile_StoreCheckingStatusDTOs = new List<GeneralMobile_StoreCheckingStatusDTO>();
            GeneralMobile_StoreCheckingStatusDTOs = StoreCheckingStatusEnum.StoreCheckingStatusEnumList
                    .Select(x => new GeneralMobile_StoreCheckingStatusDTO
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                    }).ToList();
            if (GeneralMobile_StoreCheckingStatusFilterDTO.Id?.In != null)
            {
                GeneralMobile_StoreCheckingStatusDTOs = GeneralMobile_StoreCheckingStatusDTOs
                    .Where(x => GeneralMobile_StoreCheckingStatusFilterDTO.Id.In.Contains(x.Id)).ToList();
            }
            return GeneralMobile_StoreCheckingStatusDTOs;
        }

        [Route(GeneralMobileRoute.SingleListStoreDraftType), HttpPost]
        public async Task<List<GenericEnum>> SingleListStoreDraftType()
        {
            return StoreDraftTypeEnum.StoreDraftTypeEnumList;
        }

        [Route(GeneralMobileRoute.SingleListCategory), HttpPost]
        public async Task<List<GeneralMobile_CategoryDTO>> SingleListCategory([FromBody] GeneralMobile_CategoryFilterDTO GeneralMobile_CategoryFilterDTO)
        {
            CategoryFilter CategoryFilter = new CategoryFilter();
            CategoryFilter.Skip = 0;
            CategoryFilter.Take = int.MaxValue;
            CategoryFilter.OrderBy = CategoryOrder.OrderNumber;
            CategoryFilter.OrderType = OrderType.ASC;
            CategoryFilter.Selects = CategorySelect.ALL;

            CategoryFilter.Id = GeneralMobile_CategoryFilterDTO.Id;
            CategoryFilter.Code = GeneralMobile_CategoryFilterDTO.Code;
            CategoryFilter.Name = GeneralMobile_CategoryFilterDTO.Name;
            CategoryFilter.Level = GeneralMobile_CategoryFilterDTO.Level;
            CategoryFilter.ParentId = GeneralMobile_CategoryFilterDTO.ParentId;
            CategoryFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            CategoryFilter.HasChildren = false; // mac dinh chi lay nut la

            List<Category> Categories = await CategoryService.List(CategoryFilter);
            List<GeneralMobile_CategoryDTO> GeneralMobile_CategoryDTOs = Categories
                .Select(x => new GeneralMobile_CategoryDTO(x)).ToList();
            return GeneralMobile_CategoryDTOs;
        }

        [Route(GeneralMobileRoute.CountBanner), HttpPost]
        public async Task<ActionResult<int>> CountBanner([FromBody] GeneralMobile_BannerFilterDTO GeneralMobile_BannerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            var UserId = CurrentContext.UserId;
            List<AppUser> AppUsers = await AppUserService.List(new AppUserFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Id = new IdFilter { Equal = UserId },
                Selects = AppUserSelect.Organization,
            });
            AppUser AppUser = AppUsers.FirstOrDefault();
            BannerFilter BannerFilter = new BannerFilter();
            BannerFilter.Selects = BannerSelect.ALL;
            BannerFilter.Skip = GeneralMobile_BannerFilterDTO.Skip;
            BannerFilter.Take = GeneralMobile_BannerFilterDTO.Take;
            BannerFilter.OrderBy = GeneralMobile_BannerFilterDTO.OrderBy;
            BannerFilter.OrderType = GeneralMobile_BannerFilterDTO.OrderType;
            BannerFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            if (AppUser != null)
                BannerFilter.MobileOrganizationId = new IdFilter { Equal = AppUser.OrganizationId };

            int count = await BannerService.Count(BannerFilter);
            return count;
        }

        [Route(GeneralMobileRoute.ListBanner), HttpPost]
        public async Task<ActionResult<List<GeneralMobile_BannerDTO>>> ListBanner([FromBody] GeneralMobile_BannerFilterDTO GeneralMobile_BannerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            var UserId = CurrentContext.UserId;
            List<AppUser> AppUsers = await AppUserService.List(new AppUserFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Id = new IdFilter { Equal = UserId },
                Selects = AppUserSelect.Organization,
            });
            AppUser AppUser = AppUsers.FirstOrDefault();
            BannerFilter BannerFilter = new BannerFilter();
            BannerFilter.Selects = BannerSelect.ALL;
            BannerFilter.Skip = GeneralMobile_BannerFilterDTO.Skip;
            BannerFilter.Take = GeneralMobile_BannerFilterDTO.Take;
            BannerFilter.OrderBy = BannerOrder.Priority;
            BannerFilter.OrderType = OrderType.ASC;
            BannerFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            if (AppUser != null)
                BannerFilter.MobileOrganizationId = new IdFilter { Equal = AppUser.OrganizationId };

            List<Banner> Banners = await BannerService.List(BannerFilter);
            List<GeneralMobile_BannerDTO> GeneralMobile_BannerDTOs = Banners
                .Select(c => new GeneralMobile_BannerDTO(c)).ToList();
            return GeneralMobile_BannerDTOs;
        }

        [Route(GeneralMobileRoute.GetBanner), HttpPost]
        public async Task<ActionResult<GeneralMobile_BannerDTO>> GetBanner([FromBody] GeneralMobile_BannerDTO GeneralMobile_BannerDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Banner Banner = await BannerService.Get(GeneralMobile_BannerDTO.Id);
            return new GeneralMobile_BannerDTO(Banner);
        }

        [Route(GeneralMobileRoute.GetStore), HttpPost]
        public async Task<ActionResult<GeneralMobile_StoreDTO>> Get([FromBody] GeneralMobile_StoreDTO GeneralMobile_StoreDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Store Store = await StoreService.Get(GeneralMobile_StoreDTO.Id);
            GeneralMobile_StoreDTO = new GeneralMobile_StoreDTO(Store);
            DateTime Start = StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone);
            DateTime End = Start.AddDays(1).AddSeconds(-1);
            List<Album> Albums = await AlbumService.List(new AlbumFilter
            {
                StoreId = new IdFilter { Equal = GeneralMobile_StoreDTO.Id },
                Selects = AlbumSelect.ALL,
                ShootingAt = new DateFilter { GreaterEqual = Start, LessEqual = End },
                Skip = 0,
                Take = int.MaxValue,
            });
            GeneralMobile_StoreDTO.AlbumImageMappings = Albums
                .SelectMany(x => x.AlbumImageMappings
                .Where(x => x.StoreId == GeneralMobile_StoreDTO.Id)
                .Where(x => x.SaleEmployeeId.HasValue && x.SaleEmployeeId.Value == CurrentContext.UserId)
                .Where(x => Start <= x.ShootingAt && x.ShootingAt <= End)
                .Select(m => new GeneralMobile_AlbumImageMappingDTO
                {
                    AlbumId = m.AlbumId,
                    ImageId = m.ImageId,
                    StoreId = m.StoreId,
                    Distance = m.Distance,
                    Album = m.Album == null ? null : new GeneralMobile_AlbumDTO
                    {
                        Id = m.Album.Id,
                        Name = m.Album.Name,
                    },
                    Image = m.Image == null ? null : new GeneralMobile_ImageDTO
                    {
                        Id = m.Image.Id,
                        Name = m.Image.Name,
                        Url = m.Image.Url,
                    },
                })).ToList();

            List<StoreChecking> StoreCheckings = await StoreCheckingService.List(new StoreCheckingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreCheckingSelect.Id,
                CheckOutAt = new DateFilter { GreaterEqual = Start, LessEqual = End },
                SaleEmployeeId = new IdFilter { Equal = CurrentContext.UserId },
                StoreId = new IdFilter { Equal = GeneralMobile_StoreDTO.Id }
            });
            var StoreCheckingIds = StoreCheckings.Select(x => x.Id).ToList();
            var query = from scim in DataContext.StoreCheckingImageMapping
                        join a in DataContext.Album on scim.AlbumId equals a.Id
                        join i in DataContext.Image on scim.ImageId equals i.Id
                        where StoreCheckingIds.Contains(scim.StoreCheckingId) &&
                        scim.SaleEmployeeId == CurrentContext.UserId
                        select new GeneralMobile_AlbumImageMappingDTO
                        {
                            AlbumId = scim.AlbumId,
                            ImageId = scim.ImageId,
                            StoreId = scim.StoreId,
                            Album = new GeneralMobile_AlbumDTO
                            {
                                Id = a.Id,
                                Name = a.Name,
                            },
                            Image = new GeneralMobile_ImageDTO
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Url = i.Url,
                            },
                        };
            var StoreCheckingImageMappings = await query.ToListWithNoLockAsync();
            GeneralMobile_StoreDTO.AlbumImageMappings.AddRange(StoreCheckingImageMappings);
            return GeneralMobile_StoreDTO;
        }
        [Route(GeneralMobileRoute.ListStoreBalance), HttpPost]
        public async Task<ActionResult<List<GeneralMobile_StoreBalanceDTO>>> ListStoreBalance([FromBody] GeneralMobile_StoreBalanceFilterDTO GeneralMobile_StoreBalanceFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StoreBalanceFilter StoreBalanceFilter = new StoreBalanceFilter();
            StoreBalanceFilter.Take = int.MaxValue;
            StoreBalanceFilter.Skip = 0;
            StoreBalanceFilter.Selects = StoreBalanceSelect.Id | StoreBalanceSelect.Store | StoreBalanceSelect.Organization | StoreBalanceSelect.CreditAmount | StoreBalanceSelect.DebitAmount | StoreBalanceSelect.UpdatedAt;
            StoreBalanceFilter.StoreId = GeneralMobile_StoreBalanceFilterDTO.StoreId;
            StoreBalanceFilter.OrganizationId = GeneralMobile_StoreBalanceFilterDTO.OrganizationId;
            StoreBalanceFilter.CreditAmount = GeneralMobile_StoreBalanceFilterDTO.CreditAmount;
            StoreBalanceFilter.DebitAmount = GeneralMobile_StoreBalanceFilterDTO.DebitAmount;

            List<StoreBalance> StoreBalances = await StoreBalanceService.List(StoreBalanceFilter);
            List<GeneralMobile_StoreBalanceDTO> GeneralMobile_StoreBalanceDTOs  = StoreBalances
                .Select(x => new GeneralMobile_StoreBalanceDTO(x)).ToList();
            foreach (var StoreBalanceDTO in GeneralMobile_StoreBalanceDTOs)
            {
                StoreBalanceDTO.BalanceAmount = StoreBalanceDTO.DebitAmount.Value - StoreBalanceDTO.CreditAmount.Value;
            }
            return GeneralMobile_StoreBalanceDTOs;
        }

        [Route(GeneralMobileRoute.CountStore), HttpPost]
        public async Task<long> CountStore([FromBody] GeneralMobile_StoreFilterDTO GeneralMobile_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Search = GeneralMobile_StoreFilterDTO.Search;
            StoreFilter.Skip = GeneralMobile_StoreFilterDTO.Skip;
            StoreFilter.Take = GeneralMobile_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.DESC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = GeneralMobile_StoreFilterDTO.Id;
            StoreFilter.Code = GeneralMobile_StoreFilterDTO.Code;
            StoreFilter.Name = GeneralMobile_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = GeneralMobile_StoreFilterDTO.ParentStoreId;
            StoreFilter.StoreTypeId = GeneralMobile_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = GeneralMobile_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StoreCheckingStatusId = GeneralMobile_StoreFilterDTO.StoreCheckingStatusId;
            StoreFilter.Telephone = GeneralMobile_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = GeneralMobile_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = GeneralMobile_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = GeneralMobile_StoreFilterDTO.WardId;
            StoreFilter.Address = GeneralMobile_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = GeneralMobile_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = GeneralMobile_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = GeneralMobile_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = GeneralMobile_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = GeneralMobile_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = GeneralMobile_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = GeneralMobile_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = GeneralMobile_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = GeneralMobile_StoreFilterDTO.StoreStatusId;
            StoreFilter.StoreDraftTypeId = GeneralMobile_StoreFilterDTO.StoreDraftTypeId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            return await StoreCheckingService.CountStore(StoreFilter, GeneralMobile_StoreFilterDTO.ERouteId);
        }

        [Route(GeneralMobileRoute.ListStore), HttpPost]
        public async Task<List<GeneralMobile_StoreDTO>> ListStore([FromBody] GeneralMobile_StoreFilterDTO GeneralMobile_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Search = GeneralMobile_StoreFilterDTO.Search;
            StoreFilter.Skip = GeneralMobile_StoreFilterDTO.Skip;
            StoreFilter.Take = GeneralMobile_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.DESC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = GeneralMobile_StoreFilterDTO.Id;
            StoreFilter.Code = GeneralMobile_StoreFilterDTO.Code;
            StoreFilter.Name = GeneralMobile_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = GeneralMobile_StoreFilterDTO.ParentStoreId;
            StoreFilter.StoreTypeId = GeneralMobile_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = GeneralMobile_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StoreCheckingStatusId = GeneralMobile_StoreFilterDTO.StoreCheckingStatusId;
            StoreFilter.Telephone = GeneralMobile_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = GeneralMobile_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = GeneralMobile_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = GeneralMobile_StoreFilterDTO.WardId;
            StoreFilter.Address = GeneralMobile_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = GeneralMobile_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = GeneralMobile_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = GeneralMobile_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = GeneralMobile_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = GeneralMobile_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = GeneralMobile_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = GeneralMobile_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = GeneralMobile_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = GeneralMobile_StoreFilterDTO.StoreStatusId;
            StoreFilter.StoreDraftTypeId = GeneralMobile_StoreFilterDTO.StoreDraftTypeId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Store> Stores = await StoreCheckingService.ListStore(StoreFilter, GeneralMobile_StoreFilterDTO.ERouteId);
            List<GeneralMobile_StoreDTO> GeneralMobile_StoreDTOs = Stores
                .Select(x => new GeneralMobile_StoreDTO(x)).ToList();
            return GeneralMobile_StoreDTOs;
        }

        [Route(GeneralMobileRoute.CountBuyerStore), HttpPost]
        public async Task<long> CountBuyerStore([FromBody] GeneralMobile_StoreFilterDTO GeneralMobile_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Search = GeneralMobile_StoreFilterDTO.Search;
            StoreFilter.Skip = GeneralMobile_StoreFilterDTO.Skip;
            StoreFilter.Take = GeneralMobile_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.DESC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = GeneralMobile_StoreFilterDTO.Id;
            StoreFilter.Code = GeneralMobile_StoreFilterDTO.Code;
            StoreFilter.Name = GeneralMobile_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = GeneralMobile_StoreFilterDTO.ParentStoreId;
            StoreFilter.StoreTypeId = GeneralMobile_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = GeneralMobile_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StoreCheckingStatusId = GeneralMobile_StoreFilterDTO.StoreCheckingStatusId;
            StoreFilter.Telephone = GeneralMobile_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = GeneralMobile_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = GeneralMobile_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = GeneralMobile_StoreFilterDTO.WardId;
            StoreFilter.Address = GeneralMobile_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = GeneralMobile_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = GeneralMobile_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = GeneralMobile_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = GeneralMobile_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = GeneralMobile_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = GeneralMobile_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = GeneralMobile_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = GeneralMobile_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreDraftTypeId = GeneralMobile_StoreFilterDTO.StoreDraftTypeId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            AppUser AppUser = await AppUserService.Get(CurrentContext.UserId);
            var StoreIds = AppUser.AppUserStoreMappings.Select(x => x.StoreId).ToList();
            if (StoreIds.Any())
            {
                StoreFilter.Id = new IdFilter { In = StoreIds };
            }

            return await StoreCheckingService.CountStore(StoreFilter, GeneralMobile_StoreFilterDTO.ERouteId);
        }

        [Route(GeneralMobileRoute.ListBuyerStore), HttpPost]
        public async Task<List<GeneralMobile_StoreDTO>> ListBuyerStore([FromBody] GeneralMobile_StoreFilterDTO GeneralMobile_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Search = GeneralMobile_StoreFilterDTO.Search;
            StoreFilter.Skip = GeneralMobile_StoreFilterDTO.Skip;
            StoreFilter.Take = GeneralMobile_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.DESC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = GeneralMobile_StoreFilterDTO.Id;
            StoreFilter.Code = GeneralMobile_StoreFilterDTO.Code;
            StoreFilter.Name = GeneralMobile_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = GeneralMobile_StoreFilterDTO.ParentStoreId;
            StoreFilter.StoreTypeId = GeneralMobile_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = GeneralMobile_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StoreCheckingStatusId = GeneralMobile_StoreFilterDTO.StoreCheckingStatusId;
            StoreFilter.Telephone = GeneralMobile_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = GeneralMobile_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = GeneralMobile_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = GeneralMobile_StoreFilterDTO.WardId;
            StoreFilter.Address = GeneralMobile_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = GeneralMobile_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = GeneralMobile_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = GeneralMobile_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = GeneralMobile_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = GeneralMobile_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = GeneralMobile_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = GeneralMobile_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = GeneralMobile_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreDraftTypeId = GeneralMobile_StoreFilterDTO.StoreDraftTypeId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Store> Stores = await StoreCheckingService.ListStore(StoreFilter, GeneralMobile_StoreFilterDTO.ERouteId);
            List<GeneralMobile_StoreDTO> GeneralMobile_StoreDTOs = Stores
                .Select(x => new GeneralMobile_StoreDTO(x)).ToList();
            return GeneralMobile_StoreDTOs;
        }

        [Route(GeneralMobileRoute.CountStorePlanned), HttpPost]
        public async Task<long> CountStorePlanned([FromBody] GeneralMobile_StoreFilterDTO GeneralMobile_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Search = GeneralMobile_StoreFilterDTO.Search;
            StoreFilter.Skip = GeneralMobile_StoreFilterDTO.Skip;
            StoreFilter.Take = GeneralMobile_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.DESC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = GeneralMobile_StoreFilterDTO.Id;
            StoreFilter.Code = GeneralMobile_StoreFilterDTO.Code;
            StoreFilter.Name = GeneralMobile_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = GeneralMobile_StoreFilterDTO.ParentStoreId;
            StoreFilter.StoreTypeId = GeneralMobile_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = GeneralMobile_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StoreCheckingStatusId = GeneralMobile_StoreFilterDTO.StoreCheckingStatusId;
            StoreFilter.Telephone = GeneralMobile_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = GeneralMobile_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = GeneralMobile_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = GeneralMobile_StoreFilterDTO.WardId;
            StoreFilter.Address = GeneralMobile_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = GeneralMobile_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = GeneralMobile_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = GeneralMobile_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = GeneralMobile_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = GeneralMobile_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = GeneralMobile_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = GeneralMobile_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = GeneralMobile_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = GeneralMobile_StoreFilterDTO.StoreStatusId;
            StoreFilter.StoreDraftTypeId = GeneralMobile_StoreFilterDTO.StoreDraftTypeId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            return await StoreCheckingService.CountStorePlanned(StoreFilter, GeneralMobile_StoreFilterDTO.ERouteId);
        }

        [Route(GeneralMobileRoute.ListStorePlanned), HttpPost]
        public async Task<List<GeneralMobile_StoreDTO>> ListStorePlanned([FromBody] GeneralMobile_StoreFilterDTO GeneralMobile_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Search = GeneralMobile_StoreFilterDTO.Search;
            StoreFilter.Skip = GeneralMobile_StoreFilterDTO.Skip;
            StoreFilter.Take = GeneralMobile_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.DESC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = GeneralMobile_StoreFilterDTO.Id;
            StoreFilter.Code = GeneralMobile_StoreFilterDTO.Code;
            StoreFilter.Name = GeneralMobile_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = GeneralMobile_StoreFilterDTO.ParentStoreId;
            StoreFilter.StoreTypeId = GeneralMobile_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = GeneralMobile_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StoreCheckingStatusId = GeneralMobile_StoreFilterDTO.StoreCheckingStatusId;
            StoreFilter.Telephone = GeneralMobile_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = GeneralMobile_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = GeneralMobile_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = GeneralMobile_StoreFilterDTO.WardId;
            StoreFilter.Address = GeneralMobile_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = GeneralMobile_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = GeneralMobile_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = GeneralMobile_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = GeneralMobile_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = GeneralMobile_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = GeneralMobile_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = GeneralMobile_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = GeneralMobile_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = GeneralMobile_StoreFilterDTO.StoreStatusId;
            StoreFilter.StoreDraftTypeId = GeneralMobile_StoreFilterDTO.StoreDraftTypeId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Store> Stores = await StoreCheckingService.ListStorePlanned(StoreFilter, GeneralMobile_StoreFilterDTO.ERouteId);
            List<GeneralMobile_StoreDTO> GeneralMobile_StoreDTOs = Stores
                .Select(x => new GeneralMobile_StoreDTO(x)).ToList();
            return GeneralMobile_StoreDTOs;
        }

        [Route(GeneralMobileRoute.CountStoreUnPlanned), HttpPost]
        public async Task<long> CountStoreUnPlanned([FromBody] GeneralMobile_StoreFilterDTO GeneralMobile_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Search = GeneralMobile_StoreFilterDTO.Search;
            StoreFilter.Skip = GeneralMobile_StoreFilterDTO.Skip;
            StoreFilter.Take = GeneralMobile_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.DESC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = GeneralMobile_StoreFilterDTO.Id;
            StoreFilter.Code = GeneralMobile_StoreFilterDTO.Code;
            StoreFilter.Name = GeneralMobile_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = GeneralMobile_StoreFilterDTO.ParentStoreId;
            StoreFilter.StoreTypeId = GeneralMobile_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = GeneralMobile_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StoreCheckingStatusId = GeneralMobile_StoreFilterDTO.StoreCheckingStatusId;
            StoreFilter.Telephone = GeneralMobile_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = GeneralMobile_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = GeneralMobile_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = GeneralMobile_StoreFilterDTO.WardId;
            StoreFilter.Address = GeneralMobile_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = GeneralMobile_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = GeneralMobile_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = GeneralMobile_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = GeneralMobile_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = GeneralMobile_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = GeneralMobile_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = GeneralMobile_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = GeneralMobile_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = GeneralMobile_StoreFilterDTO.StoreStatusId;
            StoreFilter.StoreDraftTypeId = GeneralMobile_StoreFilterDTO.StoreDraftTypeId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            return await StoreCheckingService.CountStoreUnPlanned(StoreFilter, GeneralMobile_StoreFilterDTO.ERouteId, GeneralMobile_StoreFilterDTO.Time);
        }

        [Route(GeneralMobileRoute.ListStoreUnPlanned), HttpPost]
        public async Task<List<GeneralMobile_StoreDTO>> ListStoreUnPlanned([FromBody] GeneralMobile_StoreFilterDTO GeneralMobile_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Search = GeneralMobile_StoreFilterDTO.Search;
            StoreFilter.Skip = GeneralMobile_StoreFilterDTO.Skip;
            StoreFilter.Take = GeneralMobile_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.DESC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = GeneralMobile_StoreFilterDTO.Id;
            StoreFilter.Code = GeneralMobile_StoreFilterDTO.Code;
            StoreFilter.Name = GeneralMobile_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = GeneralMobile_StoreFilterDTO.ParentStoreId;
            StoreFilter.StoreTypeId = GeneralMobile_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = GeneralMobile_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StoreCheckingStatusId = GeneralMobile_StoreFilterDTO.StoreCheckingStatusId;
            StoreFilter.Telephone = GeneralMobile_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = GeneralMobile_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = GeneralMobile_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = GeneralMobile_StoreFilterDTO.WardId;
            StoreFilter.Address = GeneralMobile_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = GeneralMobile_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = GeneralMobile_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = GeneralMobile_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = GeneralMobile_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = GeneralMobile_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = GeneralMobile_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = GeneralMobile_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = GeneralMobile_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = GeneralMobile_StoreFilterDTO.StoreStatusId;
            StoreFilter.StoreDraftTypeId = GeneralMobile_StoreFilterDTO.StoreDraftTypeId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Store> Stores = await StoreCheckingService.ListStoreUnPlanned(StoreFilter, GeneralMobile_StoreFilterDTO.ERouteId, GeneralMobile_StoreFilterDTO.Time);
            List<GeneralMobile_StoreDTO> GeneralMobile_StoreDTOs = Stores
                .Select(x => new GeneralMobile_StoreDTO(x)).ToList();
            return GeneralMobile_StoreDTOs;
        }

        [Route(GeneralMobileRoute.CountStoreInScope), HttpPost]
        public async Task<long> CountStoreInScope([FromBody] GeneralMobile_StoreFilterDTO GeneralMobile_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Search = GeneralMobile_StoreFilterDTO.Search;
            StoreFilter.Skip = GeneralMobile_StoreFilterDTO.Skip;
            StoreFilter.Take = GeneralMobile_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.DESC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = GeneralMobile_StoreFilterDTO.Id;
            StoreFilter.Code = GeneralMobile_StoreFilterDTO.Code;
            StoreFilter.Name = GeneralMobile_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = GeneralMobile_StoreFilterDTO.ParentStoreId;
            StoreFilter.StoreTypeId = GeneralMobile_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = GeneralMobile_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StoreCheckingStatusId = GeneralMobile_StoreFilterDTO.StoreCheckingStatusId;
            StoreFilter.Telephone = GeneralMobile_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = GeneralMobile_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = GeneralMobile_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = GeneralMobile_StoreFilterDTO.WardId;
            StoreFilter.Address = GeneralMobile_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = GeneralMobile_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = GeneralMobile_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = GeneralMobile_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = GeneralMobile_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = GeneralMobile_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = GeneralMobile_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = GeneralMobile_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = GeneralMobile_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = GeneralMobile_StoreFilterDTO.StoreStatusId;
            StoreFilter.StoreDraftTypeId = GeneralMobile_StoreFilterDTO.StoreDraftTypeId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            return await StoreCheckingService.CountStoreInScope(StoreFilter, GeneralMobile_StoreFilterDTO.ERouteId);
        }

        [Route(GeneralMobileRoute.ListStoreInScope), HttpPost]
        public async Task<List<GeneralMobile_StoreDTO>> ListStoreInScope([FromBody] GeneralMobile_StoreFilterDTO GeneralMobile_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Search = GeneralMobile_StoreFilterDTO.Search;
            StoreFilter.Skip = GeneralMobile_StoreFilterDTO.Skip;
            StoreFilter.Take = GeneralMobile_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.DESC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = GeneralMobile_StoreFilterDTO.Id;
            StoreFilter.Code = GeneralMobile_StoreFilterDTO.Code;
            StoreFilter.Name = GeneralMobile_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = GeneralMobile_StoreFilterDTO.ParentStoreId;
            StoreFilter.StoreTypeId = GeneralMobile_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = GeneralMobile_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StoreCheckingStatusId = GeneralMobile_StoreFilterDTO.StoreCheckingStatusId;
            StoreFilter.Telephone = GeneralMobile_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = GeneralMobile_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = GeneralMobile_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = GeneralMobile_StoreFilterDTO.WardId;
            StoreFilter.Address = GeneralMobile_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = GeneralMobile_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = GeneralMobile_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = GeneralMobile_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = GeneralMobile_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = GeneralMobile_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = GeneralMobile_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = GeneralMobile_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = GeneralMobile_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = GeneralMobile_StoreFilterDTO.StoreStatusId;
            StoreFilter.StoreDraftTypeId = GeneralMobile_StoreFilterDTO.StoreDraftTypeId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Store> Stores = await StoreCheckingService.ListStoreInScope(StoreFilter, GeneralMobile_StoreFilterDTO.ERouteId);
            List<GeneralMobile_StoreDTO> GeneralMobile_StoreDTOs = Stores
                .Select(x => new GeneralMobile_StoreDTO(x)).ToList();
            return GeneralMobile_StoreDTOs;
        }

        [Route(GeneralMobileRoute.CountItem), HttpPost]
        public async Task<long> CountItem([FromBody] GeneralMobile_ItemFilterDTO GeneralMobile_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Search = GeneralMobile_ItemFilterDTO.Search; 
            ItemFilter.Id = GeneralMobile_ItemFilterDTO.Id;
            ItemFilter.Code = GeneralMobile_ItemFilterDTO.Code;
            ItemFilter.Name = GeneralMobile_ItemFilterDTO.Name;
            ItemFilter.OtherName = GeneralMobile_ItemFilterDTO.OtherName;
            ItemFilter.ProductGroupingId = GeneralMobile_ItemFilterDTO.ProductGroupingId;
            ItemFilter.ProductId = GeneralMobile_ItemFilterDTO.ProductId;
            ItemFilter.ProductTypeId = GeneralMobile_ItemFilterDTO.ProductTypeId;
            ItemFilter.RetailPrice = GeneralMobile_ItemFilterDTO.RetailPrice;
            ItemFilter.SalePrice = GeneralMobile_ItemFilterDTO.SalePrice;
            ItemFilter.ScanCode = GeneralMobile_ItemFilterDTO.ScanCode;
            ItemFilter.IsNew = GeneralMobile_ItemFilterDTO.IsNew;
            ItemFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            ItemFilter.SupplierId = GeneralMobile_ItemFilterDTO.SupplierId;
            return await IndirectSalesOrderService.MobileCountItem(ItemFilter, CurrentContext.UserId, GeneralMobile_ItemFilterDTO.StoreId?.Equal);
        }

        [Route(GeneralMobileRoute.ListItem), HttpPost]
        public async Task<List<GeneralMobile_ItemDTO>> ListItem([FromBody] GeneralMobile_ItemFilterDTO GeneralMobile_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Search = GeneralMobile_ItemFilterDTO.Search;
            ItemFilter.Skip = GeneralMobile_ItemFilterDTO.Skip;
            ItemFilter.Take = GeneralMobile_ItemFilterDTO.Take;
            ItemFilter.OrderBy = ItemOrder.Score;
            ItemFilter.OrderType = OrderType.DESC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.Id = GeneralMobile_ItemFilterDTO.Id;
            ItemFilter.Code = GeneralMobile_ItemFilterDTO.Code;
            ItemFilter.Name = GeneralMobile_ItemFilterDTO.Name;
            ItemFilter.OtherName = GeneralMobile_ItemFilterDTO.OtherName;
            ItemFilter.ProductGroupingId = GeneralMobile_ItemFilterDTO.ProductGroupingId;
            ItemFilter.ProductId = GeneralMobile_ItemFilterDTO.ProductId;
            ItemFilter.ProductTypeId = GeneralMobile_ItemFilterDTO.ProductTypeId;
            ItemFilter.RetailPrice = GeneralMobile_ItemFilterDTO.RetailPrice;
            ItemFilter.SalePrice = GeneralMobile_ItemFilterDTO.SalePrice;
            ItemFilter.ScanCode = GeneralMobile_ItemFilterDTO.ScanCode;
            ItemFilter.IsNew = GeneralMobile_ItemFilterDTO.IsNew;
            ItemFilter.BrandId = GeneralMobile_ItemFilterDTO.BrandId;
            ItemFilter.CategoryId = GeneralMobile_ItemFilterDTO.CategoryId;
            ItemFilter.ProductGroupingId = GeneralMobile_ItemFilterDTO.ProductGroupingId;
            ItemFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            ItemFilter.SupplierId = GeneralMobile_ItemFilterDTO.SupplierId;

            if (GeneralMobile_ItemFilterDTO.StoreId == null)
                GeneralMobile_ItemFilterDTO.StoreId = new IdFilter();
            List<Item> Items = await IndirectSalesOrderService.MobileListItem(ItemFilter, CurrentContext.UserId, GeneralMobile_ItemFilterDTO.StoreId.Equal);
            List<GeneralMobile_ItemDTO> GeneralMobile_ItemDTOs = Items
                .Select(x => new GeneralMobile_ItemDTO(x)).ToList();
            return GeneralMobile_ItemDTOs;
        }
        [Route(GeneralMobileRoute.ListItemDirectOrder), HttpPost]
        public async Task<List<GeneralMobile_ItemDTO>> ListItemDirectOrder([FromBody] GeneralMobile_ItemFilterDTO GeneralMobile_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Search = GeneralMobile_ItemFilterDTO.Search;
            ItemFilter.Skip = GeneralMobile_ItemFilterDTO.Skip;
            ItemFilter.Take = GeneralMobile_ItemFilterDTO.Take;
            ItemFilter.OrderBy = ItemOrder.Id;
            ItemFilter.OrderType = OrderType.DESC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.Id = GeneralMobile_ItemFilterDTO.Id;
            ItemFilter.Code = GeneralMobile_ItemFilterDTO.Code;
            ItemFilter.Name = GeneralMobile_ItemFilterDTO.Name;
            ItemFilter.OtherName = GeneralMobile_ItemFilterDTO.OtherName;
            ItemFilter.ProductGroupingId = GeneralMobile_ItemFilterDTO.ProductGroupingId;
            ItemFilter.ProductId = GeneralMobile_ItemFilterDTO.ProductId;
            ItemFilter.ProductTypeId = GeneralMobile_ItemFilterDTO.ProductTypeId;
            ItemFilter.RetailPrice = GeneralMobile_ItemFilterDTO.RetailPrice;
            ItemFilter.SalePrice = GeneralMobile_ItemFilterDTO.SalePrice;
            ItemFilter.ScanCode = GeneralMobile_ItemFilterDTO.ScanCode;
            ItemFilter.IsNew = GeneralMobile_ItemFilterDTO.IsNew;
            ItemFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            ItemFilter.SupplierId = GeneralMobile_ItemFilterDTO.SupplierId;

            if (GeneralMobile_ItemFilterDTO.StoreId == null)
                GeneralMobile_ItemFilterDTO.StoreId = new IdFilter();
            List<Item> Items = await DirectSalesOrderService.MobileListItem(ItemFilter, CurrentContext.UserId, GeneralMobile_ItemFilterDTO.StoreId.Equal);
            List<GeneralMobile_ItemDTO> GeneralMobile_ItemDTOs = Items
                .Select(x => new GeneralMobile_ItemDTO(x)).ToList();
            return GeneralMobile_ItemDTOs;
        }

        [Route(GeneralMobileRoute.GetItem), HttpPost]
        public async Task<GeneralMobile_ItemDTO> GetItem([FromBody] GeneralMobile_ItemDTO GeneralMobile_ItemDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            Item Item = await ItemService.MobileGet(GeneralMobile_ItemDTO.Id);
            if (Item == null)
                return null;
            GeneralMobile_ItemDTO = new GeneralMobile_ItemDTO(Item);
            return GeneralMobile_ItemDTO;
        }

        [Route(GeneralMobileRoute.CountProblem), HttpPost]
        public async Task<long> CountProblem([FromBody] GeneralMobile_ProblemFilterDTO GeneralMobile_ProblemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUser appUser = await AppUserService.Get(CurrentContext.UserId);

            ProblemFilter ProblemFilter = new ProblemFilter();
            ProblemFilter.Id = GeneralMobile_ProblemFilterDTO.Id;
            ProblemFilter.StoreCheckingId = GeneralMobile_ProblemFilterDTO.StoreCheckingId;
            ProblemFilter.AppUserId = new IdFilter { Equal = appUser.Id };
            ProblemFilter.StoreId = GeneralMobile_ProblemFilterDTO.StoreId;
            ProblemFilter.NoteAt = GeneralMobile_ProblemFilterDTO.NoteAt;
            ProblemFilter.CompletedAt = GeneralMobile_ProblemFilterDTO.CompletedAt;
            ProblemFilter.ProblemTypeId = GeneralMobile_ProblemFilterDTO.ProblemTypeId;
            ProblemFilter.ProblemStatusId = GeneralMobile_ProblemFilterDTO.ProblemStatusId;
            return await ProblemService.Count(ProblemFilter);
        }

        [Route(GeneralMobileRoute.ListProblem), HttpPost]
        public async Task<List<GeneralMobile_ProblemDTO>> ListProblem([FromBody] GeneralMobile_ProblemFilterDTO GeneralMobile_ProblemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long CurrentUserId = CurrentContext.UserId;

            ProblemFilter ProblemFilter = new ProblemFilter();
            ProblemFilter.Skip = GeneralMobile_ProblemFilterDTO.Skip;
            ProblemFilter.Take = GeneralMobile_ProblemFilterDTO.Take;
            ProblemFilter.OrderBy = ProblemOrder.NoteAt;
            ProblemFilter.OrderType = OrderType.DESC;
            ProblemFilter.Selects = ProblemSelect.ALL;
            ProblemFilter.Id = GeneralMobile_ProblemFilterDTO.Id;
            ProblemFilter.StoreCheckingId = GeneralMobile_ProblemFilterDTO.StoreCheckingId;
            ProblemFilter.StoreId = GeneralMobile_ProblemFilterDTO.StoreId;
            ProblemFilter.NoteAt = GeneralMobile_ProblemFilterDTO.NoteAt;
            ProblemFilter.CompletedAt = GeneralMobile_ProblemFilterDTO.CompletedAt;
            ProblemFilter.ProblemTypeId = GeneralMobile_ProblemFilterDTO.ProblemTypeId;
            ProblemFilter.ProblemStatusId = GeneralMobile_ProblemFilterDTO.ProblemStatusId;
            if (ProblemFilter.StoreId == null || !ProblemFilter.StoreId.HasValue) //nếu ko truyền storeid (chỉ lấy vấn đề của ng đăng nhập)
                ProblemFilter.AppUserId = new IdFilter { Equal = CurrentUserId };
            List<Problem> Problems = await ProblemService.List(ProblemFilter);
            List<GeneralMobile_ProblemDTO> GeneralMobile_ProblemDTOs = Problems
                .Select(x => new GeneralMobile_ProblemDTO(x)).ToList();
            return GeneralMobile_ProblemDTOs;
        }

        [Route(GeneralMobileRoute.GetProblem), HttpPost]
        public async Task<ActionResult<GeneralMobile_ProblemDTO>> Get([FromBody] GeneralMobile_ProblemDTO GeneralMobile_ProblemDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Problem Problem = await ProblemService.Get(GeneralMobile_ProblemDTO.Id);
            return new GeneralMobile_ProblemDTO(Problem);
        }

        [Route(GeneralMobileRoute.CountSurvey), HttpPost]
        public async Task<long> CountSurvey([FromBody] GeneralMobile_SurveyFilterDTO GeneralMobile_SurveyFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            DateTime Now = StaticParams.DateTimeNow;
            AppUser appUser = await AppUserService.Get(CurrentContext.UserId);

            SurveyFilter SurveyFilter = new SurveyFilter();
            SurveyFilter.Selects = SurveySelect.ALL;
            SurveyFilter.Skip = GeneralMobile_SurveyFilterDTO.Skip;
            SurveyFilter.Take = GeneralMobile_SurveyFilterDTO.Take;
            SurveyFilter.OrderBy = GeneralMobile_SurveyFilterDTO.OrderBy;
            SurveyFilter.OrderType = GeneralMobile_SurveyFilterDTO.OrderType;

            SurveyFilter.Id = GeneralMobile_SurveyFilterDTO.Id;
            SurveyFilter.Title = GeneralMobile_SurveyFilterDTO.Title;
            SurveyFilter.Description = GeneralMobile_SurveyFilterDTO.Description;
            SurveyFilter.StartAt = new DateFilter { LessEqual = Now };
            SurveyFilter.EndAt = new DateFilter { GreaterEqual = Now };
            SurveyFilter.CreatorId = GeneralMobile_SurveyFilterDTO.CreatorId;
            SurveyFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            return await SurveyService.Count(SurveyFilter);
        }

        [Route(GeneralMobileRoute.ListSurvey), HttpPost]
        public async Task<List<GeneralMobile_SurveyDTO>> ListSurvey([FromBody] GeneralMobile_SurveyFilterDTO GeneralMobile_SurveyFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            DateTime Now = StaticParams.DateTimeNow;
            AppUser appUser = await AppUserService.Get(CurrentContext.UserId);

            SurveyFilter SurveyFilter = new SurveyFilter();
            SurveyFilter.Selects = SurveySelect.ALL;
            SurveyFilter.Skip = GeneralMobile_SurveyFilterDTO.Skip;
            SurveyFilter.Take = GeneralMobile_SurveyFilterDTO.Take;
            SurveyFilter.OrderBy = GeneralMobile_SurveyFilterDTO.OrderBy;
            SurveyFilter.OrderType = GeneralMobile_SurveyFilterDTO.OrderType;

            SurveyFilter.Id = GeneralMobile_SurveyFilterDTO.Id;
            SurveyFilter.Title = GeneralMobile_SurveyFilterDTO.Title;
            SurveyFilter.Description = GeneralMobile_SurveyFilterDTO.Description;
            SurveyFilter.StartAt = new DateFilter { LessEqual = Now };
            SurveyFilter.EndAt = new DateFilter { GreaterEqual = Now };
            SurveyFilter.CreatorId = GeneralMobile_SurveyFilterDTO.CreatorId;
            SurveyFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Survey> Surveys = await SurveyService.List(SurveyFilter);
            List<GeneralMobile_SurveyDTO> GeneralMobile_SurveyDTOs = Surveys
                .Select(x => new GeneralMobile_SurveyDTO(x)).ToList();
            return GeneralMobile_SurveyDTOs;
        }

        [Route(GeneralMobileRoute.CountStoreScouting), HttpPost]
        public async Task<long> CountStoreScouting([FromBody] GeneralMobile_StoreScoutingFilterDTO GeneralMobile_StoreScoutingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUser appUser = await AppUserService.Get(CurrentContext.UserId);

            StoreScoutingFilter StoreScoutingFilter = new StoreScoutingFilter();
            StoreScoutingFilter.Selects = StoreScoutingSelect.ALL;
            StoreScoutingFilter.Skip = GeneralMobile_StoreScoutingFilterDTO.Skip;
            StoreScoutingFilter.Take = GeneralMobile_StoreScoutingFilterDTO.Take;
            StoreScoutingFilter.OrderBy = GeneralMobile_StoreScoutingFilterDTO.OrderBy;
            StoreScoutingFilter.OrderType = GeneralMobile_StoreScoutingFilterDTO.OrderType;

            StoreScoutingFilter.Id = GeneralMobile_StoreScoutingFilterDTO.Id;
            StoreScoutingFilter.Code = GeneralMobile_StoreScoutingFilterDTO.Code;
            StoreScoutingFilter.Name = GeneralMobile_StoreScoutingFilterDTO.Name;
            StoreScoutingFilter.CreatedAt = GeneralMobile_StoreScoutingFilterDTO.CreatedAt;
            //StoreScoutingFilter.AppUserId = new IdFilter { Equal = appUser.Id };
            StoreScoutingFilter.DistrictId = GeneralMobile_StoreScoutingFilterDTO.DistrictId;
            StoreScoutingFilter.OrganizationId = new IdFilter { Equal = appUser.OrganizationId };
            StoreScoutingFilter.ProvinceId = GeneralMobile_StoreScoutingFilterDTO.ProvinceId;
            StoreScoutingFilter.WardId = GeneralMobile_StoreScoutingFilterDTO.WardId;
            //StoreScoutingFilter.StoreScoutingStatusId = GeneralMobile_StoreScoutingFilterDTO.StoreScoutingStatusId;
            StoreScoutingFilter.StoreScoutingStatusId = new IdFilter { Equal = StoreScoutingStatusEnum.NOTOPEN.Id }; // chi lay cac cua hang cam co chua mo
            StoreScoutingFilter.OwnerPhone = GeneralMobile_StoreScoutingFilterDTO.OwnerPhone;

            return await StoreScoutingService.Count(StoreScoutingFilter);
        }

        [Route(GeneralMobileRoute.ListStoreScouting), HttpPost]
        public async Task<List<GeneralMobile_StoreScoutingDTO>> ListStoreScouting([FromBody] GeneralMobile_StoreScoutingFilterDTO GeneralMobile_StoreScoutingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUser appUser = await AppUserService.Get(CurrentContext.UserId);

            StoreScoutingFilter StoreScoutingFilter = new StoreScoutingFilter();
            StoreScoutingFilter.Selects = StoreScoutingSelect.ALL;
            StoreScoutingFilter.Skip = GeneralMobile_StoreScoutingFilterDTO.Skip;
            StoreScoutingFilter.Take = GeneralMobile_StoreScoutingFilterDTO.Take;
            StoreScoutingFilter.OrderBy = GeneralMobile_StoreScoutingFilterDTO.OrderBy;
            StoreScoutingFilter.OrderType = GeneralMobile_StoreScoutingFilterDTO.OrderType;

            StoreScoutingFilter.Id = GeneralMobile_StoreScoutingFilterDTO.Id;
            StoreScoutingFilter.Code = GeneralMobile_StoreScoutingFilterDTO.Code;
            StoreScoutingFilter.Name = GeneralMobile_StoreScoutingFilterDTO.Name;
            StoreScoutingFilter.CreatedAt = GeneralMobile_StoreScoutingFilterDTO.CreatedAt;
            //StoreScoutingFilter.AppUserId = new IdFilter { Equal = appUser.Id };
            StoreScoutingFilter.DistrictId = GeneralMobile_StoreScoutingFilterDTO.DistrictId;
            StoreScoutingFilter.OrganizationId = new IdFilter { Equal = appUser.OrganizationId };
            StoreScoutingFilter.ProvinceId = GeneralMobile_StoreScoutingFilterDTO.ProvinceId;
            StoreScoutingFilter.WardId = GeneralMobile_StoreScoutingFilterDTO.WardId;
            //StoreScoutingFilter.StoreScoutingStatusId = GeneralMobile_StoreScoutingFilterDTO.StoreScoutingStatusId;
            StoreScoutingFilter.StoreScoutingStatusId = new IdFilter { Equal = StoreScoutingStatusEnum.NOTOPEN.Id }; // chi lay cac cua hang cam co chua mo
            StoreScoutingFilter.OwnerPhone = GeneralMobile_StoreScoutingFilterDTO.OwnerPhone;

            List<StoreScouting> StoreScoutings = await StoreScoutingService.List(StoreScoutingFilter);
            List<GeneralMobile_StoreScoutingDTO> GeneralMobile_StoreScoutingDTOs = StoreScoutings
                .Select(x => new GeneralMobile_StoreScoutingDTO(x)).ToList();
            return GeneralMobile_StoreScoutingDTOs;
        }

        [Route(GeneralMobileRoute.CountRewardHistory), HttpPost]
        public async Task<long> CountRewardHistory([FromBody] GeneralMobile_RewardHistoryFilterDTO GeneralMobile_RewardHistoryFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUser appUser = await AppUserService.Get(CurrentContext.UserId);

            RewardHistoryFilter RewardHistoryFilter = new RewardHistoryFilter();
            RewardHistoryFilter.Selects = RewardHistorySelect.ALL;
            RewardHistoryFilter.Skip = GeneralMobile_RewardHistoryFilterDTO.Skip;
            RewardHistoryFilter.Take = GeneralMobile_RewardHistoryFilterDTO.Take;
            RewardHistoryFilter.OrderBy = GeneralMobile_RewardHistoryFilterDTO.OrderBy;
            RewardHistoryFilter.OrderType = GeneralMobile_RewardHistoryFilterDTO.OrderType;

            RewardHistoryFilter.Id = GeneralMobile_RewardHistoryFilterDTO.Id;
            RewardHistoryFilter.CreatedAt = GeneralMobile_RewardHistoryFilterDTO.CreatedAt;
            RewardHistoryFilter.StoreId = GeneralMobile_RewardHistoryFilterDTO.StoreId;
            RewardHistoryFilter.Search = GeneralMobile_RewardHistoryFilterDTO.Search;
            RewardHistoryFilter.AppUserId = new IdFilter { Equal = appUser.Id };

            return await RewardHistoryService.Count(RewardHistoryFilter);
        }

        [Route(GeneralMobileRoute.ListRewardHistory), HttpPost]
        public async Task<List<GeneralMobile_RewardHistoryDTO>> ListRewardHistory([FromBody] GeneralMobile_RewardHistoryFilterDTO GeneralMobile_RewardHistoryFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUser appUser = await AppUserService.Get(CurrentContext.UserId);

            RewardHistoryFilter RewardHistoryFilter = new RewardHistoryFilter();
            RewardHistoryFilter.Selects = RewardHistorySelect.ALL;
            RewardHistoryFilter.Skip = GeneralMobile_RewardHistoryFilterDTO.Skip;
            RewardHistoryFilter.Take = GeneralMobile_RewardHistoryFilterDTO.Take;
            RewardHistoryFilter.OrderBy = GeneralMobile_RewardHistoryFilterDTO.OrderBy;
            RewardHistoryFilter.OrderType = GeneralMobile_RewardHistoryFilterDTO.OrderType;

            RewardHistoryFilter.Id = GeneralMobile_RewardHistoryFilterDTO.Id;
            RewardHistoryFilter.CreatedAt = GeneralMobile_RewardHistoryFilterDTO.CreatedAt;
            RewardHistoryFilter.StoreId = GeneralMobile_RewardHistoryFilterDTO.StoreId;
            RewardHistoryFilter.Search = GeneralMobile_RewardHistoryFilterDTO.Search;
            RewardHistoryFilter.AppUserId = new IdFilter { Equal = appUser.Id };

            List<RewardHistory> RewardHistorys = await RewardHistoryService.List(RewardHistoryFilter);
            List<GeneralMobile_RewardHistoryDTO> GeneralMobile_RewardHistoryDTOs = RewardHistorys
                .Select(x => new GeneralMobile_RewardHistoryDTO(x)).ToList();
            return GeneralMobile_RewardHistoryDTOs;
        }

        [Route(GeneralMobileRoute.GetRewardHistory), HttpPost]
        public async Task<ActionResult<GeneralMobile_RewardHistoryDTO>> GetRewardHistory([FromBody] GeneralMobile_RewardHistoryDTO GeneralMobile_RewardHistoryDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            RewardHistory RewardHistory = await RewardHistoryService.Get(GeneralMobile_RewardHistoryDTO.Id);
            return new GeneralMobile_RewardHistoryDTO(RewardHistory);
        }

        [Route(GeneralMobileRoute.CreateRewardHistory), HttpPost]
        public async Task<ActionResult<GeneralMobile_RewardHistoryDTO>> CreateRewardHistory([FromBody] GeneralMobile_RewardHistoryDTO GeneralMobile_RewardHistoryDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            RewardHistory RewardHistory = new RewardHistory
            {
                Id = GeneralMobile_RewardHistoryDTO.Id,
                AppUserId = CurrentContext.UserId,
                TurnCounter = GeneralMobile_RewardHistoryDTO.TurnCounter,
                Revenue = GeneralMobile_RewardHistoryDTO.Revenue,
                StoreId = GeneralMobile_RewardHistoryDTO.StoreId
            };
            RewardHistory.BaseLanguage = CurrentContext.Language;
            RewardHistory = await RewardHistoryService.Create(RewardHistory);
            GeneralMobile_RewardHistoryDTO = new GeneralMobile_RewardHistoryDTO(RewardHistory);
            if (RewardHistory.IsValidated)
            {
                return GeneralMobile_RewardHistoryDTO;
            }
            else
                return BadRequest(GeneralMobile_RewardHistoryDTO);
        }

        [Route(GeneralMobileRoute.LuckyDraw), HttpPost]
        public async Task<ActionResult<GeneralMobile_LuckyNumberDTO>> LuckyNumber([FromBody] GeneralMobile_RewardHistoryDTO GeneralMobile_RewardHistoryDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            RewardHistory RewardHistory = await RewardHistoryService.Get(GeneralMobile_RewardHistoryDTO.Id);
            if (RewardHistory != null)
            {
                if (RewardHistory.TurnCounter <= RewardHistory.RewardHistoryContents.Count())
                {
                    return BadRequest("Đã hết số lần quay thưởng");
                }
                else
                {
                    LuckyNumber LuckyNumber = await LuckyNumberService.LuckyDraw(GeneralMobile_RewardHistoryDTO.Id);
                    if (LuckyNumber == null)
                        return BadRequest("Đã hết giải thưởng");
                    else
                    {
                        return Ok(new GeneralMobile_LuckyNumberDTO(LuckyNumber));
                    }
                }
            }
            else
            {
                return BadRequest();
            }
        }

   
        [Route(GeneralMobileRoute.CountProductGrouping), HttpPost]
        public async Task<long> CountProductGrouping([FromBody] GeneralMobile_ProductGroupingFilterDTO GeneralMobile_ProductGroupingFilterDTO)
        {
            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter();
            ProductGroupingFilter.Skip = 0;
            ProductGroupingFilter.Take = 999999;
            ProductGroupingFilter.OrderBy = ProductGroupingOrder.Id;
            ProductGroupingFilter.OrderType = OrderType.ASC;
            ProductGroupingFilter.Selects = ProductGroupingSelect.ALL;
            ProductGroupingFilter.Id = GeneralMobile_ProductGroupingFilterDTO.Id;
            ProductGroupingFilter.Code = GeneralMobile_ProductGroupingFilterDTO.Code;
            ProductGroupingFilter.Name = GeneralMobile_ProductGroupingFilterDTO.Name;
            ProductGroupingFilter.ParentId = GeneralMobile_ProductGroupingFilterDTO.ParentId;
            ProductGroupingFilter.Path = GeneralMobile_ProductGroupingFilterDTO.Path;

            List<ProductGrouping> ProductGroupings = await ProductGroupingService.List(ProductGroupingFilter);
            ProductGroupings = ProductGroupings.Where(x => x.HasChildren == false).ToList();
            return ProductGroupings.Count();
        }
        [Route(GeneralMobileRoute.ListProductGrouping), HttpPost]
        public async Task<List<GeneralMobile_ProductGroupingDTO>> ListProductGrouping([FromBody] GeneralMobile_ProductGroupingFilterDTO GeneralMobile_ProductGroupingFilterDTO)
        {
            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter();
            ProductGroupingFilter.Skip = 0;
            ProductGroupingFilter.Take = 999999;
            ProductGroupingFilter.OrderBy = ProductGroupingOrder.Id;
            ProductGroupingFilter.OrderType = OrderType.ASC;
            ProductGroupingFilter.Selects = ProductGroupingSelect.ALL;
            ProductGroupingFilter.Id = GeneralMobile_ProductGroupingFilterDTO.Id;
            ProductGroupingFilter.Code = GeneralMobile_ProductGroupingFilterDTO.Code;
            ProductGroupingFilter.Name = GeneralMobile_ProductGroupingFilterDTO.Name;
            ProductGroupingFilter.ParentId = GeneralMobile_ProductGroupingFilterDTO.ParentId;
            ProductGroupingFilter.Path = GeneralMobile_ProductGroupingFilterDTO.Path;

            List<ProductGrouping> ProductGroupings = await ProductGroupingService.List(ProductGroupingFilter);
            ProductGroupings = ProductGroupings.Where(x => x.HasChildren == false).ToList();
            List<GeneralMobile_ProductGroupingDTO> GeneralMobile_ProductGroupingDTOs = ProductGroupings
                .Select(x => new GeneralMobile_ProductGroupingDTO(x)).ToList();
            return GeneralMobile_ProductGroupingDTOs;
        }
        [Route(GeneralMobileRoute.CountShowingCategory), HttpPost]
        public async Task<long> CountShowingCategory([FromBody] GeneralMobile_ShowingCategoryFilterDTO GeneralMobile_ShowingCategoryFilterDTO)
        {
            ShowingCategoryFilter ShowingCategoryFilter = new ShowingCategoryFilter();
            ShowingCategoryFilter.Skip = 0;
            ShowingCategoryFilter.Take = 999999;
            ShowingCategoryFilter.OrderBy = ShowingCategoryOrder.Id;
            ShowingCategoryFilter.OrderType = OrderType.ASC;
            ShowingCategoryFilter.Selects = ShowingCategorySelect.ALL;
            ShowingCategoryFilter.Id = GeneralMobile_ShowingCategoryFilterDTO.Id;
            ShowingCategoryFilter.Code = GeneralMobile_ShowingCategoryFilterDTO.Code;
            ShowingCategoryFilter.Name = GeneralMobile_ShowingCategoryFilterDTO.Name;
            ShowingCategoryFilter.ParentId = GeneralMobile_ShowingCategoryFilterDTO.ParentId;
            ShowingCategoryFilter.Path = GeneralMobile_ShowingCategoryFilterDTO.Path;

            List<ShowingCategory> ShowingCategories = await ShowingCategoryService.List(ShowingCategoryFilter);
            ShowingCategories = ShowingCategories.Where(x => x.HasChildren == false).ToList();
            return ShowingCategories.Count();
        }
        [Route(GeneralMobileRoute.ListShowingCategory), HttpPost]
        public async Task<List<GeneralMobile_ShowingCategoryDTO>> ListShowingCategory([FromBody] GeneralMobile_ShowingCategoryFilterDTO GeneralMobile_ShowingCategoryFilterDTO)
        {
            ShowingCategoryFilter ShowingCategoryFilter = new ShowingCategoryFilter();
            ShowingCategoryFilter.Skip = 0;
            ShowingCategoryFilter.Take = 999999;
            ShowingCategoryFilter.OrderBy = ShowingCategoryOrder.Id;
            ShowingCategoryFilter.OrderType = OrderType.ASC;
            ShowingCategoryFilter.Selects = ShowingCategorySelect.ALL;
            ShowingCategoryFilter.Id = GeneralMobile_ShowingCategoryFilterDTO.Id;
            ShowingCategoryFilter.Code = GeneralMobile_ShowingCategoryFilterDTO.Code;
            ShowingCategoryFilter.Name = GeneralMobile_ShowingCategoryFilterDTO.Name;
            ShowingCategoryFilter.ParentId = GeneralMobile_ShowingCategoryFilterDTO.ParentId;
            ShowingCategoryFilter.Path = GeneralMobile_ShowingCategoryFilterDTO.Path;

            List<ShowingCategory> ShowingCategorys = await ShowingCategoryService.List(ShowingCategoryFilter);
            ShowingCategorys = ShowingCategorys.Where(x => x.HasChildren == false).ToList();
            List<GeneralMobile_ShowingCategoryDTO> GeneralMobile_ShowingCategoryDTOs = ShowingCategorys
                .Select(x => new GeneralMobile_ShowingCategoryDTO(x)).ToList();
            return GeneralMobile_ShowingCategoryDTOs;
        }
        [Route(GeneralMobileRoute.CountBrand), HttpPost]
        public async Task<int> CountBrand([FromBody] GeneralMobile_BrandFilterDTO GeneralMobile_BrandFilterDTO)
        {
            BrandFilter BrandFilter = new BrandFilter();
            BrandFilter.Id = GeneralMobile_BrandFilterDTO.Id;
            BrandFilter.Code = GeneralMobile_BrandFilterDTO.Code;
            BrandFilter.Name = GeneralMobile_BrandFilterDTO.Name;
            BrandFilter.Description = GeneralMobile_BrandFilterDTO.Description;
            BrandFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            BrandFilter.UpdateTime = GeneralMobile_BrandFilterDTO.UpdateTime;

            return await BrandService.Count(BrandFilter);
        }
        [Route(GeneralMobileRoute.ListBrand), HttpPost]
        public async Task<List<GeneralMobile_BrandDTO>> ListBrand([FromBody] GeneralMobile_BrandFilterDTO GeneralMobile_BrandFilterDTO)
        {
            BrandFilter BrandFilter = new BrandFilter();
            BrandFilter.Skip = GeneralMobile_BrandFilterDTO.Skip;
            BrandFilter.Take = GeneralMobile_BrandFilterDTO.Take;
            BrandFilter.OrderBy = BrandOrder.Id;
            BrandFilter.OrderType = OrderType.ASC;
            BrandFilter.Selects = BrandSelect.ALL;
            BrandFilter.Id = GeneralMobile_BrandFilterDTO.Id;
            BrandFilter.Code = GeneralMobile_BrandFilterDTO.Code;
            BrandFilter.Name = GeneralMobile_BrandFilterDTO.Name;
            BrandFilter.Description = GeneralMobile_BrandFilterDTO.Description;
            BrandFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            BrandFilter.UpdateTime = GeneralMobile_BrandFilterDTO.UpdateTime;

            List<Brand> Brands = await BrandService.List(BrandFilter);
            List<GeneralMobile_BrandDTO> GeneralMobile_BrandDTOs = Brands
                .Select(x => new GeneralMobile_BrandDTO(x)).ToList();
            return GeneralMobile_BrandDTOs;
        }
        [Route(GeneralMobileRoute.ListRequestState), HttpPost]
        public async Task<List<GeneralMobile_RequestStateDTO>> ListRequestState([FromBody] GeneralMobile_RequestStateFilterDTO GeneralMobile_RequestStateFilterDTO)
        {
            RequestStateFilter RequestStateFilter = new RequestStateFilter();
            RequestStateFilter.Skip = GeneralMobile_RequestStateFilterDTO.Skip;
            RequestStateFilter.Take = GeneralMobile_RequestStateFilterDTO.Take;
            RequestStateFilter.OrderBy = RequestStateOrder.Id;
            RequestStateFilter.OrderType = OrderType.ASC;
            RequestStateFilter.Selects = RequestStateSelect.ALL;
            RequestStateFilter.Id = GeneralMobile_RequestStateFilterDTO.Id;
            RequestStateFilter.Code = GeneralMobile_RequestStateFilterDTO.Code;
            RequestStateFilter.Name = GeneralMobile_RequestStateFilterDTO.Name;

            List<RequestState> RequestStates = await RequestStateService.List(RequestStateFilter);
            List<GeneralMobile_RequestStateDTO> GeneralMobile_RequestStateDTOs = RequestStates
                .Select(x => new GeneralMobile_RequestStateDTO(x)).ToList();
            return GeneralMobile_RequestStateDTOs;
        }
        [Route(GeneralMobileRoute.CountProduct), HttpPost]
        public async Task<long> CountProduct([FromBody] GeneralMobile_ProductFilterDTO GeneralMobile_ProductFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductFilter ProductFilter = new ProductFilter();
            ProductFilter.Search = GeneralMobile_ProductFilterDTO.Search;
            ProductFilter.Id = GeneralMobile_ProductFilterDTO.Id;
            ProductFilter.Code = GeneralMobile_ProductFilterDTO.Code;
            ProductFilter.Name = GeneralMobile_ProductFilterDTO.Name;
            ProductFilter.OtherName = GeneralMobile_ProductFilterDTO.OtherName;
            ProductFilter.ProductGroupingId = GeneralMobile_ProductFilterDTO.ProductGroupingId;
            ProductFilter.ProductTypeId = GeneralMobile_ProductFilterDTO.ProductTypeId;
            ProductFilter.RetailPrice = GeneralMobile_ProductFilterDTO.RetailPrice;
            ProductFilter.SalePrice = GeneralMobile_ProductFilterDTO.SalePrice;
            ProductFilter.ScanCode = GeneralMobile_ProductFilterDTO.ScanCode;
            ProductFilter.IsNew = GeneralMobile_ProductFilterDTO.IsNew;
            ProductFilter.BrandId = GeneralMobile_ProductFilterDTO.BrandId;
            ProductFilter.CategoryId = GeneralMobile_ProductFilterDTO.CategoryId;
            ProductFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            ProductFilter.SupplierId = GeneralMobile_ProductFilterDTO.SupplierId;
            ProductFilter.ItemSalePrice = GeneralMobile_ProductFilterDTO.ItemSalePrice;
            return await IndirectSalesOrderService.MobileCountProduct(ProductFilter, CurrentContext.UserId, GeneralMobile_ProductFilterDTO.StoreId?.Equal);
        }
        [Route(GeneralMobileRoute.ListProduct), HttpPost]
        public async Task<List<GeneralMobile_ProductDTO>> ListProduct([FromBody] GeneralMobile_ProductFilterDTO GeneralMobile_ProductFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductFilter ProductFilter = new ProductFilter();
            ProductFilter.Search = GeneralMobile_ProductFilterDTO.Search;
            ProductFilter.Skip = GeneralMobile_ProductFilterDTO.Skip;
            ProductFilter.Take = GeneralMobile_ProductFilterDTO.Take;
            ProductFilter.OrderType = GeneralMobile_ProductFilterDTO.OrderType;
            ProductFilter.OrderBy = GeneralMobile_ProductFilterDTO.OrderBy;
            ProductFilter.Selects = ProductSelect.ALL;
            ProductFilter.Id = GeneralMobile_ProductFilterDTO.Id;
            ProductFilter.Code = GeneralMobile_ProductFilterDTO.Code;
            ProductFilter.Name = GeneralMobile_ProductFilterDTO.Name;
            ProductFilter.OtherName = GeneralMobile_ProductFilterDTO.OtherName;
            ProductFilter.ProductGroupingId = GeneralMobile_ProductFilterDTO.ProductGroupingId;
            ProductFilter.ProductTypeId = GeneralMobile_ProductFilterDTO.ProductTypeId;
            ProductFilter.RetailPrice = GeneralMobile_ProductFilterDTO.RetailPrice;
            ProductFilter.SalePrice = GeneralMobile_ProductFilterDTO.SalePrice;
            ProductFilter.ScanCode = GeneralMobile_ProductFilterDTO.ScanCode;
            ProductFilter.IsNew = GeneralMobile_ProductFilterDTO.IsNew;
            ProductFilter.BrandId = GeneralMobile_ProductFilterDTO.BrandId;
            ProductFilter.CategoryId = GeneralMobile_ProductFilterDTO.CategoryId;
            ProductFilter.ProductGroupingId = GeneralMobile_ProductFilterDTO.ProductGroupingId;
            ProductFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            ProductFilter.SupplierId = GeneralMobile_ProductFilterDTO.SupplierId;
            ProductFilter.ItemSalePrice = GeneralMobile_ProductFilterDTO.ItemSalePrice;
            if (ProductFilter.Search != null)
            {
                ProductFilter.OrderBy = ProductOrder.ESScore;
            }
            if (GeneralMobile_ProductFilterDTO.StoreId == null)
                GeneralMobile_ProductFilterDTO.StoreId = new IdFilter();
            List<Product> Products = new List<Product>();

            if (ProductFilter.OrderBy == ProductOrder.SalePrice || (ProductFilter.ItemSalePrice != null && ProductFilter.ItemSalePrice.HasValue))
            {
                ProductFilter SubProductFilter = ProductFilter.Clone();
                SubProductFilter.Skip = 0;
                SubProductFilter.Take = int.MaxValue;
                SubProductFilter.OrderBy = new ProductOrder();
                SubProductFilter.ItemSalePrice = null;
                SubProductFilter.Selects = ProductSelect.Id | ProductSelect.UsedVariationId | ProductSelect.VariationGrouping;

                Products = await IndirectSalesOrderService.MobileListProduct(SubProductFilter, CurrentContext.UserId, GeneralMobile_ProductFilterDTO.StoreId.Equal);
                Products = Products.Where(x => x.Items != null).ToList();
                if (ProductFilter.ItemSalePrice != null)
                {
                    if (ProductFilter.ItemSalePrice.GreaterEqual != null)
                        Products = Products.Where(x => x.Items[0].SalePrice >= ProductFilter.ItemSalePrice.GreaterEqual.Value).ToList();
                    if (ProductFilter.ItemSalePrice.LessEqual != null)
                        Products = Products.Where(x => x.Items[0].SalePrice <= ProductFilter.ItemSalePrice.LessEqual.Value).ToList();
                }
                Products = Products.Where(x => x.Items != null && x.Items.Any()).ToList();
                if (ProductFilter.OrderBy == ProductOrder.SalePrice)
                {
                    if (ProductFilter.OrderType == OrderType.ASC)
                        Products = Products.OrderBy(x => x.Items.FirstOrDefault().SalePrice).ToList();
                    else
                        Products = Products.OrderByDescending(x => x.Items[0].SalePrice).ToList();
                }

                List<long> SubProductIds = Products.Select(x => x.Id).Skip(ProductFilter.Skip).Take(ProductFilter.Take).ToList();
                ProductFilter.Id.In = SubProductIds;
                ProductFilter.Skip = 0;
            }

            Products = await IndirectSalesOrderService.MobileListProduct(ProductFilter, CurrentContext.UserId, GeneralMobile_ProductFilterDTO.StoreId.Equal);

            if (ProductFilter.OrderBy == ProductOrder.SalePrice)
            {
                if (ProductFilter.OrderType == OrderType.ASC)
                    Products = Products.OrderBy(x => x.Items[0].SalePrice).ToList();
                else
                    Products = Products.OrderByDescending(x => x.Items[0].SalePrice).ToList();
            }

            List<GeneralMobile_ProductDTO> GeneralMobile_ProductDTOs = Products
                .Select(x => new GeneralMobile_ProductDTO(x)).ToList();
            return GeneralMobile_ProductDTOs;
        }
        [Route(GeneralMobileRoute.CountProductDirectOrder), HttpPost]
        public async Task<long> CountProductDirectOrder([FromBody] GeneralMobile_ProductFilterDTO GeneralMobile_ProductFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductFilter ProductFilter = new ProductFilter();
            ProductFilter.Search = GeneralMobile_ProductFilterDTO.Search;
            ProductFilter.Id = GeneralMobile_ProductFilterDTO.Id;
            ProductFilter.Code = GeneralMobile_ProductFilterDTO.Code;
            ProductFilter.Name = GeneralMobile_ProductFilterDTO.Name;
            ProductFilter.OtherName = GeneralMobile_ProductFilterDTO.OtherName;
            ProductFilter.ProductGroupingId = GeneralMobile_ProductFilterDTO.ProductGroupingId;
            ProductFilter.ProductTypeId = GeneralMobile_ProductFilterDTO.ProductTypeId;
            ProductFilter.RetailPrice = GeneralMobile_ProductFilterDTO.RetailPrice;
            ProductFilter.SalePrice = GeneralMobile_ProductFilterDTO.SalePrice;
            ProductFilter.ScanCode = GeneralMobile_ProductFilterDTO.ScanCode;
            ProductFilter.IsNew = GeneralMobile_ProductFilterDTO.IsNew;
            ProductFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            ProductFilter.SupplierId = GeneralMobile_ProductFilterDTO.SupplierId;
            ProductFilter.BrandId = GeneralMobile_ProductFilterDTO.BrandId;
            ProductFilter.CategoryId = GeneralMobile_ProductFilterDTO.CategoryId;
            ProductFilter.ItemSalePrice = GeneralMobile_ProductFilterDTO.ItemSalePrice;
            return await DirectSalesOrderService.MobileCountProduct(ProductFilter, CurrentContext.UserId, GeneralMobile_ProductFilterDTO.StoreId?.Equal);
        }
        [Route(GeneralMobileRoute.ListProductDirectOrder), HttpPost]
        public async Task<List<GeneralMobile_ProductDTO>> ListProductDirectOrder([FromBody] GeneralMobile_ProductFilterDTO GeneralMobile_ProductFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductFilter ProductFilter = new ProductFilter();
            ProductFilter.Search = GeneralMobile_ProductFilterDTO.Search;
            ProductFilter.Skip = GeneralMobile_ProductFilterDTO.Skip;
            ProductFilter.Take = GeneralMobile_ProductFilterDTO.Take;
            ProductFilter.OrderType = GeneralMobile_ProductFilterDTO.OrderType;
            ProductFilter.OrderBy = GeneralMobile_ProductFilterDTO.OrderBy;
            ProductFilter.Selects = ProductSelect.ALL;
            ProductFilter.Id = GeneralMobile_ProductFilterDTO.Id;
            ProductFilter.Code = GeneralMobile_ProductFilterDTO.Code;
            ProductFilter.Name = GeneralMobile_ProductFilterDTO.Name;
            ProductFilter.OtherName = GeneralMobile_ProductFilterDTO.OtherName;
            ProductFilter.ProductGroupingId = GeneralMobile_ProductFilterDTO.ProductGroupingId;
            ProductFilter.ProductTypeId = GeneralMobile_ProductFilterDTO.ProductTypeId;
            ProductFilter.RetailPrice = GeneralMobile_ProductFilterDTO.RetailPrice;
            ProductFilter.SalePrice = GeneralMobile_ProductFilterDTO.SalePrice;
            ProductFilter.ScanCode = GeneralMobile_ProductFilterDTO.ScanCode;
            ProductFilter.IsNew = GeneralMobile_ProductFilterDTO.IsNew;
            ProductFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            ProductFilter.SupplierId = GeneralMobile_ProductFilterDTO.SupplierId;
            ProductFilter.BrandId = GeneralMobile_ProductFilterDTO.BrandId;
            ProductFilter.CategoryId = GeneralMobile_ProductFilterDTO.CategoryId;
            ProductFilter.ItemSalePrice = GeneralMobile_ProductFilterDTO.ItemSalePrice;

            if (GeneralMobile_ProductFilterDTO.StoreId == null)
                GeneralMobile_ProductFilterDTO.StoreId = new IdFilter();

            List<Product> Products = new List<Product>();
            if (ProductFilter.OrderBy == ProductOrder.SalePrice || (ProductFilter.ItemSalePrice != null && ProductFilter.ItemSalePrice.HasValue))
            {
                ProductFilter SubProductFilter = ProductFilter.Clone();
                SubProductFilter.Skip = 0;
                SubProductFilter.Take = int.MaxValue;
                SubProductFilter.OrderBy = new ProductOrder();
                SubProductFilter.ItemSalePrice = null;
                SubProductFilter.Selects = ProductSelect.Id | ProductSelect.UsedVariationId | ProductSelect.VariationGrouping;

                Products = await DirectSalesOrderService.MobileListProduct(SubProductFilter, CurrentContext.UserId, GeneralMobile_ProductFilterDTO.StoreId.Equal);

                if (ProductFilter.ItemSalePrice != null)
                {
                    if (ProductFilter.ItemSalePrice.GreaterEqual != null)
                        Products = Products.Where(x => x.Items[0].SalePrice >= ProductFilter.ItemSalePrice.GreaterEqual.Value).ToList();
                    if (ProductFilter.ItemSalePrice.LessEqual != null)
                        Products = Products.Where(x => x.Items[0].SalePrice <= ProductFilter.ItemSalePrice.LessEqual.Value).ToList();
                }

                if (ProductFilter.OrderBy == ProductOrder.SalePrice)
                {
                    if (ProductFilter.OrderType == OrderType.ASC)
                        Products = Products.OrderBy(x => x.Items[0].SalePrice).ToList();
                    else
                        Products = Products.OrderByDescending(x => x.Items[0].SalePrice).ToList();
                }

                List<long> SubProductIds = Products.Select(x => x.Id).Skip(ProductFilter.Skip).Take(ProductFilter.Take).ToList();
                ProductFilter.Id.In = SubProductIds;
                ProductFilter.Skip = 0;
            }

            Products = await DirectSalesOrderService.MobileListProduct(ProductFilter, CurrentContext.UserId, GeneralMobile_ProductFilterDTO.StoreId.Equal);
            
            if (ProductFilter.OrderBy == ProductOrder.SalePrice)
            {
                if (ProductFilter.OrderType == OrderType.ASC)
                    Products = Products.OrderBy(x => x.Items[0].SalePrice).ToList();
                else
                    Products = Products.OrderByDescending(x => x.Items[0].SalePrice).ToList();
            }

            List<GeneralMobile_ProductDTO> GeneralMobile_ProductDTOs = Products
                .Select(x => new GeneralMobile_ProductDTO(x)).ToList();

            return GeneralMobile_ProductDTOs;
        }
        [Route(GeneralMobileRoute.GetItemByVariation), HttpPost]
        public async Task<GeneralMobile_ItemDTO> GetItemByVariation([FromBody] GeneralMobile_ProductDetailDTO GeneralMobile_ProductDetailDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long ProductId = GeneralMobile_ProductDetailDTO.ProductId;
            if (GeneralMobile_ProductDetailDTO.StoreId == null)
                GeneralMobile_ProductDetailDTO.StoreId = new long();
            List<long> VariationIds = GeneralMobile_ProductDetailDTO.VariationIds;
            if (ProductId == 0)
            {
                return new GeneralMobile_ItemDTO();
            }
            Item Item = await ItemService.GetItemByVariation(ProductId, VariationIds, GeneralMobile_ProductDetailDTO.StoreId.Value);
            if (Item == null)
                return new GeneralMobile_ItemDTO();
            return new GeneralMobile_ItemDTO(Item);
        }
        [Route(GeneralMobileRoute.GetProduct), HttpPost]
        public async Task<ActionResult<GeneralMobile_ProductDTO>> GetProduct([FromBody] GeneralMobile_ProductDTO GeneralMobile_ProductDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Product Product = await ProductService.Get(GeneralMobile_ProductDTO.Id);
            if (Product == null)
                return null;
            return new GeneralMobile_ProductDTO(Product);
        }
        [Route(GeneralMobileRoute.ListUploadedImage), HttpPost]
        public async Task<ActionResult<List<GeneralMobile_StoreImageDTO>>> ListUploadedImage([FromBody] GeneralMobile_StoreImageFilterDTO filter)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            //chỉ lấy các ảnh trong vòng 1 tháng trở lại
            DateTime Start = LocalStartDay(CurrentContext).AddDays(-30);
            DateTime End = LocalEndDay(CurrentContext);

            #region filter date
            if (filter.Time != null && filter.Time.Equal.HasValue)
            {
                if (filter.Time.Equal.Value == 1) //Today
                {
                    Start = LocalStartDay(CurrentContext);
                    End = LocalEndDay(CurrentContext);
                }
                else if (filter.Time.Equal.Value == 2) //3 ngày trước
                {
                    Start = LocalStartDay(CurrentContext).AddDays(-3);
                    End = LocalEndDay(CurrentContext);
                }
                else if (filter.Time.Equal.Value == 3) //10 ngày trước
                {
                    Start = LocalStartDay(CurrentContext).AddDays(-10);
                    End = LocalEndDay(CurrentContext);
                }
                else if (filter.Time.Equal.Value == 4) //30 ngày trước
                {
                }
            } 
            #endregion
            var StoreImageQuery = DWContext.Fact_Image.AsNoTracking();
            StoreImageQuery = StoreImageQuery.Where(x => x.StoreId, filter.StoreId);
            StoreImageQuery = StoreImageQuery.Where(x => x.SaleEmployeeId, new IdFilter { Equal = CurrentContext.UserId });
            StoreImageQuery = StoreImageQuery.Where(x => x.ShootingAt, new DateFilter { GreaterEqual = Start, LessEqual = End });
            var Images = await StoreImageQuery.Select(x => new GeneralMobile_StoreImageDTO
            {
                ImageId = x.ImageId,
                AlbumId = x.AlbumId,
                ShootingAt = x.ShootingAt,
                Url = x.Url,
                //AlbumName = x.AlbumName,
                StoreId = x.StoreId
            }).ToListWithNoLockAsync();
            return Images;
        }

    }
}

