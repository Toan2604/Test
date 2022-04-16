using DMS.Entities;
using DMS.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.kpi_product_grouping
{
    public partial class KpiProductGroupingController : RpcController
    {
        [Route(KpiProductGroupingRoute.FilterListAppUser), HttpPost]
        public async Task<List<KpiProductGrouping_AppUserDTO>> FilterListAppUser([FromBody] KpiProductGrouping_AppUserFilterDTO KpiProductGrouping_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = KpiProductGrouping_AppUserFilterDTO.Id;
            AppUserFilter.Username = KpiProductGrouping_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = KpiProductGrouping_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = KpiProductGrouping_AppUserFilterDTO.Address;
            AppUserFilter.Email = KpiProductGrouping_AppUserFilterDTO.Email;
            AppUserFilter.Phone = KpiProductGrouping_AppUserFilterDTO.Phone;
            AppUserFilter.SexId = KpiProductGrouping_AppUserFilterDTO.SexId;
            AppUserFilter.Birthday = KpiProductGrouping_AppUserFilterDTO.Birthday;
            AppUserFilter.PositionId = KpiProductGrouping_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = KpiProductGrouping_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = KpiProductGrouping_AppUserFilterDTO.OrganizationId;
            AppUserFilter.ProvinceId = KpiProductGrouping_AppUserFilterDTO.ProvinceId;
            AppUserFilter.StatusId = KpiProductGrouping_AppUserFilterDTO.StatusId;

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<KpiProductGrouping_AppUserDTO> KpiProductGrouping_AppUserDTOs = AppUsers
                .Select(x => new KpiProductGrouping_AppUserDTO(x)).ToList();
            return KpiProductGrouping_AppUserDTOs;
        }
        [Route(KpiProductGroupingRoute.FilterListOrganization), HttpPost]
        public async Task<List<KpiProductGrouping_OrganizationDTO>> FilterListOrganization([FromBody] KpiProductGrouping_OrganizationFilterDTO KpiProductGrouping_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = KpiProductGrouping_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = KpiProductGrouping_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = KpiProductGrouping_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = KpiProductGrouping_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = KpiProductGrouping_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = KpiProductGrouping_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = KpiProductGrouping_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = KpiProductGrouping_OrganizationFilterDTO.Phone;
            OrganizationFilter.Address = KpiProductGrouping_OrganizationFilterDTO.Address;
            OrganizationFilter.Email = KpiProductGrouping_OrganizationFilterDTO.Email;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.OrFilter == null) OrganizationFilter.OrFilter = new List<OrganizationFilter>();
            if (CurrentContext.Filters != null)
            {
                foreach (var currentFilter in CurrentContext.Filters)
                {
                    OrganizationFilter subFilter = new OrganizationFilter();
                    OrganizationFilter.OrFilter.Add(subFilter);
                    List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                    foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                    {
                        if (FilterPermissionDefinition.Name == nameof(KpiProductGroupingFilter.OrganizationId))
                            subFilter.Id = FilterPermissionDefinition.IdFilter;
                    }
                }
            }

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<KpiProductGrouping_OrganizationDTO> KpiProductGrouping_OrganizationDTOs = Organizations
                .Select(x => new KpiProductGrouping_OrganizationDTO(x)).ToList();
            return KpiProductGrouping_OrganizationDTOs;
        }
        [Route(KpiProductGroupingRoute.FilterListKpiPeriod), HttpPost]
        public async Task<List<KpiProductGrouping_KpiPeriodDTO>> FilterListKpiPeriod([FromBody] KpiProductGrouping_KpiPeriodFilterDTO KpiProductGrouping_KpiPeriodFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiPeriodFilter KpiPeriodFilter = new KpiPeriodFilter();
            KpiPeriodFilter.Skip = 0;
            KpiPeriodFilter.Take = int.MaxValue;
            KpiPeriodFilter.Take = 20;
            KpiPeriodFilter.OrderBy = KpiPeriodOrder.Id;
            KpiPeriodFilter.OrderType = OrderType.ASC;
            KpiPeriodFilter.Selects = KpiPeriodSelect.ALL;

            KpiPeriodFilter.Id = KpiProductGrouping_KpiPeriodFilterDTO.Id;
            KpiPeriodFilter.Code = KpiProductGrouping_KpiPeriodFilterDTO.Code;
            KpiPeriodFilter.Name = KpiProductGrouping_KpiPeriodFilterDTO.Name;

            List<KpiPeriod> KpiPeriods = await KpiPeriodService.List(KpiPeriodFilter);
            List<KpiProductGrouping_KpiPeriodDTO> KpiProductGrouping_KpiPeriodDTOs = KpiPeriods
                .Select(x => new KpiProductGrouping_KpiPeriodDTO(x)).ToList();
            return KpiProductGrouping_KpiPeriodDTOs;
        }
        [Route(KpiProductGroupingRoute.FilterListKpiYear), HttpPost]
        public async Task<List<KpiProductGrouping_KpiYearDTO>> FilterListKpiYear([FromBody] KpiProductGrouping_KpiYearFilterDTO KpiProductGrouping_KpiYearFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiYearFilter KpiYearFilter = new KpiYearFilter();
            KpiYearFilter.Skip = 0;
            KpiYearFilter.Take = int.MaxValue;
            KpiYearFilter.Take = 20;
            KpiYearFilter.OrderBy = KpiYearOrder.Id;
            KpiYearFilter.OrderType = OrderType.ASC;
            KpiYearFilter.Selects = KpiYearSelect.ALL;

            KpiYearFilter.Id = KpiProductGrouping_KpiYearFilterDTO.Id;
            KpiYearFilter.Code = KpiProductGrouping_KpiYearFilterDTO.Code;
            KpiYearFilter.Name = KpiProductGrouping_KpiYearFilterDTO.Name;

            List<KpiYear> KpiYears = await KpiYearService.List(KpiYearFilter);
            List<KpiProductGrouping_KpiYearDTO> KpiProductGrouping_KpiYearDTOs = KpiYears
                .Select(x => new KpiProductGrouping_KpiYearDTO(x)).ToList();
            return KpiProductGrouping_KpiYearDTOs;
        }
        [Route(KpiProductGroupingRoute.FilterListKpiProductGroupingType), HttpPost]
        public async Task<List<KpiProductGrouping_KpiProductGroupingTypeDTO>> FilterListKpiProductGroupingType([FromBody] KpiProductGrouping_KpiProductGroupingTypeFilterDTO KpiProductGrouping_KpiProductGroupingTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiProductGroupingTypeFilter KpiProductGroupingTypeFilter = new KpiProductGroupingTypeFilter();
            KpiProductGroupingTypeFilter.Skip = 0;
            KpiProductGroupingTypeFilter.Take = 20;
            KpiProductGroupingTypeFilter.OrderBy = KpiProductGroupingTypeOrder.Id;
            KpiProductGroupingTypeFilter.OrderType = OrderType.ASC;
            KpiProductGroupingTypeFilter.Selects = KpiProductGroupingTypeSelect.ALL;
            KpiProductGroupingTypeFilter.Id = KpiProductGrouping_KpiProductGroupingTypeFilterDTO.Id;
            KpiProductGroupingTypeFilter.Code = KpiProductGrouping_KpiProductGroupingTypeFilterDTO.Code;
            KpiProductGroupingTypeFilter.Name = KpiProductGrouping_KpiProductGroupingTypeFilterDTO.Name;

            List<KpiProductGroupingType> KpiProductGroupingTypes = await KpiProductGroupingTypeService.List(KpiProductGroupingTypeFilter);
            List<KpiProductGrouping_KpiProductGroupingTypeDTO> KpiProductGrouping_KpiProductGroupingTypeDTOs = KpiProductGroupingTypes
                .Select(x => new KpiProductGrouping_KpiProductGroupingTypeDTO(x)).ToList();
            return KpiProductGrouping_KpiProductGroupingTypeDTOs;
        }
        [Route(KpiProductGroupingRoute.FilterListStatus), HttpPost]
        public async Task<List<KpiProductGrouping_StatusDTO>> FilterListStatus([FromBody] KpiProductGrouping_StatusFilterDTO KpiProductGrouping_StatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = int.MaxValue;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            StatusFilter.Id = KpiProductGrouping_StatusFilterDTO.Id;
            StatusFilter.Code = KpiProductGrouping_StatusFilterDTO.Code;
            StatusFilter.Name = KpiProductGrouping_StatusFilterDTO.Name;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<KpiProductGrouping_StatusDTO> KpiProductGrouping_StatusDTOs = Statuses
                .Select(x => new KpiProductGrouping_StatusDTO(x)).ToList();
            return KpiProductGrouping_StatusDTOs;
        }

        #region Item Popup
        [Route(KpiProductGroupingRoute.FilterListCategory), HttpPost]
        public async Task<List<KpiProductGrouping_CategoryDTO>> FilterListCategory([FromBody] KpiProductGrouping_CategoryFilterDTO KpiProductGrouping_CategoryFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            CategoryFilter CategoryFilter = new CategoryFilter();
            CategoryFilter.Skip = 0;
            CategoryFilter.Take = int.MaxValue;
            CategoryFilter.OrderBy = CategoryOrder.Id;
            CategoryFilter.OrderType = OrderType.ASC;
            CategoryFilter.Selects = CategorySelect.ALL;
            CategoryFilter.Id = KpiProductGrouping_CategoryFilterDTO.Id;
            CategoryFilter.Code = KpiProductGrouping_CategoryFilterDTO.Code;
            CategoryFilter.Name = KpiProductGrouping_CategoryFilterDTO.Name;
            CategoryFilter.ParentId = KpiProductGrouping_CategoryFilterDTO.ParentId;
            CategoryFilter.Path = KpiProductGrouping_CategoryFilterDTO.Path;
            CategoryFilter.Level = KpiProductGrouping_CategoryFilterDTO.Level;
            CategoryFilter.StatusId = KpiProductGrouping_CategoryFilterDTO.StatusId;
            CategoryFilter.ImageId = KpiProductGrouping_CategoryFilterDTO.ImageId;
            CategoryFilter.RowId = KpiProductGrouping_CategoryFilterDTO.RowId;

            List<Category> Categories = await CategoryService.List(CategoryFilter);
            List<KpiProductGrouping_CategoryDTO> KpiProductGrouping_CategoryDTOs = Categories
                .Select(x => new KpiProductGrouping_CategoryDTO(x)).ToList();
            return KpiProductGrouping_CategoryDTOs;
        }

        [Route(KpiProductGroupingRoute.FilterListProductType), HttpPost]
        public async Task<List<KpiProductGrouping_ProductTypeDTO>> FilterListProductType([FromBody] KpiProductGrouping_ProductTypeFilterDTO KpiProductGrouping_ProductTypeFilterDTO)
        {
            ProductTypeFilter ProductTypeFilter = new ProductTypeFilter();
            ProductTypeFilter.Skip = 0;
            ProductTypeFilter.Take = 20;
            ProductTypeFilter.OrderBy = ProductTypeOrder.Id;
            ProductTypeFilter.OrderType = OrderType.ASC;
            ProductTypeFilter.Selects = ProductTypeSelect.ALL;
            ProductTypeFilter.Id = KpiProductGrouping_ProductTypeFilterDTO.Id;
            ProductTypeFilter.Code = KpiProductGrouping_ProductTypeFilterDTO.Code;
            ProductTypeFilter.Name = KpiProductGrouping_ProductTypeFilterDTO.Name;
            ProductTypeFilter.Description = KpiProductGrouping_ProductTypeFilterDTO.Description;

            List<ProductType> ProductTypes = await ProductTypeService.List(ProductTypeFilter);
            List<KpiProductGrouping_ProductTypeDTO> KpiProductGrouping_ProductTypeDTOs = ProductTypes
                .Select(x => new KpiProductGrouping_ProductTypeDTO(x)).ToList();
            return KpiProductGrouping_ProductTypeDTOs;
        }

        [Route(KpiProductGroupingRoute.FilterListProductGrouping), HttpPost]
        public async Task<List<KpiProductGrouping_ProductGroupingDTO>> FilterListProductGrouping([FromBody] KpiProductGrouping_ProductGroupingFilterDTO KpiProductGrouping_ProductGroupingFilterDTO)
        {
            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter();
            ProductGroupingFilter.Skip = 0;
            ProductGroupingFilter.Take = int.MaxValue;
            ProductGroupingFilter.OrderBy = ProductGroupingOrder.Id;
            ProductGroupingFilter.OrderType = OrderType.ASC;
            ProductGroupingFilter.Selects = ProductGroupingSelect.Id | ProductGroupingSelect.Code
                | ProductGroupingSelect.Name | ProductGroupingSelect.Parent;

            List<ProductGrouping> ProductGroupings = await ProductGroupingService.List(ProductGroupingFilter);
            List<KpiProductGrouping_ProductGroupingDTO> KpiProductGrouping_ProductGroupingDTOs = ProductGroupings
                .Select(x => new KpiProductGrouping_ProductGroupingDTO(x)).ToList();
            return KpiProductGrouping_ProductGroupingDTOs;
        }

        [Route(KpiProductGroupingRoute.FilterListBrand), HttpPost]
        public async Task<List<KpiProductGrouping_BrandDTO>> FilterListBrand([FromBody] KpiProductGrouping_BrandFilterDTO KpiProductGrouping_BrandFilterDTO)
        {
            BrandFilter BrandFilter = new BrandFilter();
            BrandFilter.Skip = 0;
            BrandFilter.Take = 20;
            BrandFilter.OrderBy = BrandOrder.Id;
            BrandFilter.OrderType = OrderType.ASC;
            BrandFilter.Selects = BrandSelect.ALL;
            BrandFilter.Id = KpiProductGrouping_BrandFilterDTO.Id;
            BrandFilter.Code = KpiProductGrouping_BrandFilterDTO.Code;
            BrandFilter.Name = KpiProductGrouping_BrandFilterDTO.Name;
            BrandFilter.Description = KpiProductGrouping_BrandFilterDTO.Description;
            BrandFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            BrandFilter.UpdateTime = KpiProductGrouping_BrandFilterDTO.UpdateTime;

            List<Brand> Brands = await BrandService.List(BrandFilter);
            List<KpiProductGrouping_BrandDTO> KpiProductGrouping_BrandDTOs = Brands
                .Select(x => new KpiProductGrouping_BrandDTO(x)).ToList();
            return KpiProductGrouping_BrandDTOs;
        }
        #endregion

    }
}

