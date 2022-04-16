using DMS.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.kpi_product_grouping
{
    public partial class KpiProductGroupingController : RpcController
    {
        [Route(KpiProductGroupingRoute.SingleListAppUser), HttpPost]
        public async Task<List<KpiProductGrouping_AppUserDTO>> SingleListAppUser([FromBody] KpiProductGrouping_AppUserFilterDTO KpiProductGrouping_AppUserFilterDTO)
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
            AppUserFilter.StatusId = new IdFilter { Equal = 1 };
            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<KpiProductGrouping_AppUserDTO> KpiProductGrouping_AppUserDTOs = AppUsers
                .Select(x => new KpiProductGrouping_AppUserDTO(x)).ToList();
            return KpiProductGrouping_AppUserDTOs;
        }
        [Route(KpiProductGroupingRoute.SingleListKpiPeriod), HttpPost]
        public async Task<List<KpiProductGrouping_KpiPeriodDTO>> SingleListKpiPeriod([FromBody] KpiProductGrouping_KpiPeriodFilterDTO KpiProductGrouping_KpiPeriodFilterDTO)
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
            List<KpiPeriod> KpiPeriods = await KpiPeriodService.List(KpiPeriodFilter);
            List<KpiProductGrouping_KpiPeriodDTO> KpiProductGrouping_KpiPeriodDTOs = KpiPeriods
                .Select(x => new KpiProductGrouping_KpiPeriodDTO(x)).ToList();
            return KpiProductGrouping_KpiPeriodDTOs;
        }
        [Route(KpiProductGroupingRoute.SingleListKpiProductGroupingType), HttpPost]
        public async Task<List<KpiProductGrouping_KpiProductGroupingTypeDTO>> SingleListKpiProductGroupingType([FromBody] KpiProductGrouping_KpiProductGroupingTypeFilterDTO KpiProductGrouping_KpiProductGroupingTypeFilterDTO)
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
        [Route(KpiProductGroupingRoute.SingleListStatus), HttpPost]
        public async Task<List<KpiProductGrouping_StatusDTO>> SingleListStatus([FromBody] KpiProductGrouping_StatusFilterDTO KpiProductGrouping_StatusFilterDTO)
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
            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<KpiProductGrouping_StatusDTO> KpiProductGrouping_StatusDTOs = Statuses
                .Select(x => new KpiProductGrouping_StatusDTO(x)).ToList();
            return KpiProductGrouping_StatusDTOs;
        }
        [Route(KpiProductGroupingRoute.SingleListProductGrouping), HttpPost]
        public async Task<List<KpiProductGrouping_ProductGroupingDTO>> SingleListProductGrouping([FromBody] KpiProductGrouping_ProductGroupingFilterDTO KpiProductGrouping_ProductGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter();
            ProductGroupingFilter.Skip = 0;
            ProductGroupingFilter.Take = int.MaxValue;
            ProductGroupingFilter.OrderBy = ProductGroupingOrder.Id;
            ProductGroupingFilter.OrderType = OrderType.ASC;
            ProductGroupingFilter.Selects = ProductGroupingSelect.Id | ProductGroupingSelect.Code
                | ProductGroupingSelect.Name | ProductGroupingSelect.Parent;

            List<ProductGrouping> KpiProductGroupingGroupings = await ProductGroupingService.List(ProductGroupingFilter);
            List<KpiProductGrouping_ProductGroupingDTO> KpiProductGrouping_ProductGroupingDTOs = KpiProductGroupingGroupings
                .Select(x => new KpiProductGrouping_ProductGroupingDTO(x)).ToList();
            return KpiProductGrouping_ProductGroupingDTOs;
        }
        [Route(KpiProductGroupingRoute.SingleListKpiYear), HttpPost]
        public async Task<List<KpiProductGrouping_KpiYearDTO>> SingleListKpiYear([FromBody] KpiProductGrouping_KpiYearFilterDTO KpiProductGrouping_KpiYearFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiYearFilter KpiYearFilter = new KpiYearFilter();
            KpiYearFilter.Skip = 0;
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
        [Route(KpiProductGroupingRoute.SingleListOrganization), HttpPost]
        public async Task<List<KpiProductGrouping_OrganizationDTO>> SingleListOrganization([FromBody] KpiProductGrouping_OrganizationFilterDTO KpiProductGrouping_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = KpiProductGrouping_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = KpiProductGrouping_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = KpiProductGrouping_OrganizationFilterDTO.Name;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<KpiProductGrouping_OrganizationDTO> KpiProductGrouping_OrganizationDTOs = Organizations
                .Select(x => new KpiProductGrouping_OrganizationDTO(x)).ToList();
            return KpiProductGrouping_OrganizationDTOs;
        }
    }
}

