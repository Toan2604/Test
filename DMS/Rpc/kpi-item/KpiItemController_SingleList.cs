using DMS.Entities;
using DMS.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.kpi_item
{
    public partial class KpiItemController
    {
        [Route(KpiItemRoute.FilterListAppUser), HttpPost]
        public async Task<List<KpiItem_AppUserDTO>> FilterListAppUser([FromBody] KpiItem_AppUserFilterDTO KpiItem_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = KpiItem_AppUserFilterDTO.Id;
            AppUserFilter.Username = KpiItem_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = KpiItem_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = KpiItem_AppUserFilterDTO.Address;
            AppUserFilter.Email = KpiItem_AppUserFilterDTO.Email;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<KpiItem_AppUserDTO> KpiItem_AppUserDTOs = AppUsers
                .Select(x => new KpiItem_AppUserDTO(x)).ToList();
            return KpiItem_AppUserDTOs;
        }
        [Route(KpiItemRoute.FilterListKpiPeriod), HttpPost]
        public async Task<List<KpiItem_KpiPeriodDTO>> FilterListKpiPeriod([FromBody] KpiItem_KpiPeriodFilterDTO KpiItem_KpiPeriodFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiPeriodFilter KpiPeriodFilter = new KpiPeriodFilter();
            KpiPeriodFilter.Skip = 0;
            KpiPeriodFilter.Take = 20;
            KpiPeriodFilter.OrderBy = KpiPeriodOrder.Id;
            KpiPeriodFilter.OrderType = OrderType.ASC;
            KpiPeriodFilter.Selects = KpiPeriodSelect.ALL;
            KpiPeriodFilter.Id = KpiItem_KpiPeriodFilterDTO.Id;
            KpiPeriodFilter.Code = KpiItem_KpiPeriodFilterDTO.Code;
            KpiPeriodFilter.Name = KpiItem_KpiPeriodFilterDTO.Name;

            List<KpiPeriod> KpiPeriods = await KpiPeriodService.List(KpiPeriodFilter);
            List<KpiItem_KpiPeriodDTO> KpiItem_KpiPeriodDTOs = KpiPeriods
                .Select(x => new KpiItem_KpiPeriodDTO(x)).ToList();
            return KpiItem_KpiPeriodDTOs;
        }

        [Route(KpiItemRoute.FilterListKpiYear), HttpPost]
        public async Task<List<KpiItem_KpiYearDTO>> FilterListKpiYear([FromBody] KpiItem_KpiYearFilterDTO KpiItem_KpiYearFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiYearFilter KpiYearFilter = new KpiYearFilter();
            KpiYearFilter.Skip = 0;
            KpiYearFilter.Take = 20;
            KpiYearFilter.OrderBy = KpiYearOrder.Id;
            KpiYearFilter.OrderType = OrderType.ASC;
            KpiYearFilter.Selects = KpiYearSelect.ALL;
            KpiYearFilter.Id = KpiItem_KpiYearFilterDTO.Id;
            KpiYearFilter.Code = KpiItem_KpiYearFilterDTO.Code;
            KpiYearFilter.Name = KpiItem_KpiYearFilterDTO.Name;

            List<KpiYear> KpiYears = await KpiYearService.List(KpiYearFilter);
            List<KpiItem_KpiYearDTO> KpiItem_KpiYearDTOs = KpiYears
                .Select(x => new KpiItem_KpiYearDTO(x)).ToList();
            return KpiItem_KpiYearDTOs;
        }

        [Route(KpiItemRoute.FilterListOrganization), HttpPost]
        public async Task<List<KpiItem_OrganizationDTO>> FilterListOrganization([FromBody] KpiItem_OrganizationFilterDTO KpiItem_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = KpiItem_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = KpiItem_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = KpiItem_OrganizationFilterDTO.Name;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<KpiItem_OrganizationDTO> KpiItem_OrganizationDTOs = Organizations
                .Select(x => new KpiItem_OrganizationDTO(x)).ToList();
            return KpiItem_OrganizationDTOs;
        }
        [Route(KpiItemRoute.FilterListStatus), HttpPost]
        public async Task<List<KpiItem_StatusDTO>> FilterListStatus()
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
            List<KpiItem_StatusDTO> KpiItem_StatusDTOs = Statuses
                .Select(x => new KpiItem_StatusDTO(x)).ToList();
            return KpiItem_StatusDTOs;
        }

        [Route(KpiItemRoute.FilterListKpiCriteriaItem), HttpPost]
        public async Task<List<KpiItem_KpiCriteriaItemDTO>> FilterListKpiCriteriaItem()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiCriteriaItemFilter KpiCriteriaItemFilter = new KpiCriteriaItemFilter();
            KpiCriteriaItemFilter.Skip = 0;
            KpiCriteriaItemFilter.Take = int.MaxValue;
            KpiCriteriaItemFilter.Take = 20;
            KpiCriteriaItemFilter.OrderBy = KpiCriteriaItemOrder.Id;
            KpiCriteriaItemFilter.OrderType = OrderType.ASC;
            KpiCriteriaItemFilter.Selects = KpiCriteriaItemSelect.ALL;

            List<KpiCriteriaItem> KpiCriteriaItemes = await KpiCriteriaItemService.List(KpiCriteriaItemFilter);
            List<KpiItem_KpiCriteriaItemDTO> KpiItem_KpiCriteriaItemDTOs = KpiCriteriaItemes
                .Select(x => new KpiItem_KpiCriteriaItemDTO(x)).ToList();
            return KpiItem_KpiCriteriaItemDTOs;
        }

        [Route(KpiItemRoute.FilterListKpiItemType), HttpPost]
        public async Task<List<KpiItem_KpiItemTypeDTO>> FilterListKpiItemType([FromBody] KpiItem_KpiItemTypeFilterDTO KpiItem_KpiItemTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiItemTypeFilter KpiItemTypeFilter = new KpiItemTypeFilter();
            KpiItemTypeFilter.Skip = 0;
            KpiItemTypeFilter.Take = 20;
            KpiItemTypeFilter.OrderBy = KpiItemTypeOrder.Id;
            KpiItemTypeFilter.OrderType = OrderType.ASC;
            KpiItemTypeFilter.Selects = KpiItemTypeSelect.ALL;
            KpiItemTypeFilter.Id = KpiItem_KpiItemTypeFilterDTO.Id;
            KpiItemTypeFilter.Code = KpiItem_KpiItemTypeFilterDTO.Code;
            KpiItemTypeFilter.Name = KpiItem_KpiItemTypeFilterDTO.Name;

            List<KpiItemType> KpiItemTypes = await KpiItemTypeService.List(KpiItemTypeFilter);
            List<KpiItem_KpiItemTypeDTO> KpiItem_KpiItemTypeDTOs = KpiItemTypes
                .Select(x => new KpiItem_KpiItemTypeDTO(x)).ToList();
            return KpiItem_KpiItemTypeDTOs;
        }

        [Route(KpiItemRoute.FilterListItem), HttpPost]
        public async Task<List<KpiItem_ItemDTO>> FilterListItem([FromBody] KpiItem_ItemFilterDTO KpiItem_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Skip = 0;
            ItemFilter.Take = 20;
            ItemFilter.OrderBy = ItemOrder.Id;
            ItemFilter.OrderType = OrderType.ASC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.Id = KpiItem_ItemFilterDTO.Id;
            ItemFilter.ProductId = KpiItem_ItemFilterDTO.ProductId;
            ItemFilter.Code = KpiItem_ItemFilterDTO.Code;
            ItemFilter.Name = KpiItem_ItemFilterDTO.Name;
            ItemFilter.ScanCode = KpiItem_ItemFilterDTO.ScanCode;
            ItemFilter.SalePrice = KpiItem_ItemFilterDTO.SalePrice;
            ItemFilter.RetailPrice = KpiItem_ItemFilterDTO.RetailPrice;
            ItemFilter.StatusId = KpiItem_ItemFilterDTO.StatusId;
            ItemFilter.Search = KpiItem_ItemFilterDTO.Search;

            List<Item> Items = await ItemService.List(ItemFilter);
            List<KpiItem_ItemDTO> KpiItem_ItemDTOs = Items
                .Select(x => new KpiItem_ItemDTO(x)).ToList();
            return KpiItem_ItemDTOs;
        }

        [Route(KpiItemRoute.FilterListSupplier), HttpPost]
        public async Task<List<KpiItem_SupplierDTO>> FilterListSupplier([FromBody] KpiItem_SupplierFilterDTO KpiItem_SupplierFilterDTO)
        {
            SupplierFilter SupplierFilter = new SupplierFilter();
            SupplierFilter.Skip = 0;
            SupplierFilter.Take = 20;
            SupplierFilter.OrderBy = SupplierOrder.Id;
            SupplierFilter.OrderType = OrderType.ASC;
            SupplierFilter.Selects = SupplierSelect.ALL;
            SupplierFilter.Id = KpiItem_SupplierFilterDTO.Id;
            SupplierFilter.Code = KpiItem_SupplierFilterDTO.Code;
            SupplierFilter.Name = KpiItem_SupplierFilterDTO.Name;
            SupplierFilter.TaxCode = KpiItem_SupplierFilterDTO.TaxCode;

            List<Supplier> Suppliers = await SupplierService.List(SupplierFilter);
            List<KpiItem_SupplierDTO> KpiItem_SupplierDTOs = Suppliers
                .Select(x => new KpiItem_SupplierDTO(x)).ToList();
            return KpiItem_SupplierDTOs;
        }
        [Route(KpiItemRoute.FilterListProductType), HttpPost]
        public async Task<List<KpiItem_ProductTypeDTO>> FilterListProductType([FromBody] KpiItem_ProductTypeFilterDTO KpiItem_ProductTypeFilterDTO)
        {
            ProductTypeFilter ProductTypeFilter = new ProductTypeFilter();
            ProductTypeFilter.Skip = 0;
            ProductTypeFilter.Take = 20;
            ProductTypeFilter.OrderBy = ProductTypeOrder.Id;
            ProductTypeFilter.OrderType = OrderType.ASC;
            ProductTypeFilter.Selects = ProductTypeSelect.ALL;
            ProductTypeFilter.Id = KpiItem_ProductTypeFilterDTO.Id;
            ProductTypeFilter.Code = KpiItem_ProductTypeFilterDTO.Code;
            ProductTypeFilter.Name = KpiItem_ProductTypeFilterDTO.Name;
            ProductTypeFilter.Description = KpiItem_ProductTypeFilterDTO.Description;

            List<ProductType> ProductTypes = await ProductTypeService.List(ProductTypeFilter);
            List<KpiItem_ProductTypeDTO> KpiItem_ProductTypeDTOs = ProductTypes
                .Select(x => new KpiItem_ProductTypeDTO(x)).ToList();
            return KpiItem_ProductTypeDTOs;
        }

        [Route(KpiItemRoute.FilterListProductGrouping), HttpPost]
        public async Task<List<KpiItem_ProductGroupingDTO>> FilterListProductGrouping([FromBody] KpiItem_ProductGroupingFilterDTO KpiItem_ProductGroupingFilterDTO)
        {
            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter();
            ProductGroupingFilter.Skip = 0;
            ProductGroupingFilter.Take = int.MaxValue;
            ProductGroupingFilter.OrderBy = ProductGroupingOrder.Id;
            ProductGroupingFilter.OrderType = OrderType.ASC;
            ProductGroupingFilter.Selects = ProductGroupingSelect.Id | ProductGroupingSelect.Code
                | ProductGroupingSelect.Name | ProductGroupingSelect.Parent;

            List<ProductGrouping> ProductGroupings = await ProductGroupingService.List(ProductGroupingFilter);
            List<KpiItem_ProductGroupingDTO> KpiItem_ProductGroupingDTOs = ProductGroupings
                .Select(x => new KpiItem_ProductGroupingDTO(x)).ToList();
            return KpiItem_ProductGroupingDTOs;
        }

        [Route(KpiItemRoute.SingleListAppUser), HttpPost]
        public async Task<List<KpiItem_AppUserDTO>> SingleListAppUser([FromBody] KpiItem_AppUserFilterDTO KpiItem_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = KpiItem_AppUserFilterDTO.Id;
            AppUserFilter.Username = KpiItem_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = KpiItem_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = KpiItem_AppUserFilterDTO.Address;
            AppUserFilter.Email = KpiItem_AppUserFilterDTO.Email;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<KpiItem_AppUserDTO> KpiItem_AppUserDTOs = AppUsers
                .Select(x => new KpiItem_AppUserDTO(x)).ToList();
            return KpiItem_AppUserDTOs;
        }
        [Route(KpiItemRoute.SingleListKpiPeriod), HttpPost]
        public async Task<List<KpiItem_KpiPeriodDTO>> SingleListKpiPeriod([FromBody] KpiItem_KpiPeriodFilterDTO KpiItem_KpiPeriodFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiPeriodFilter KpiPeriodFilter = new KpiPeriodFilter();
            KpiPeriodFilter.Skip = 0;
            KpiPeriodFilter.Take = 20;
            KpiPeriodFilter.OrderBy = KpiPeriodOrder.Id;
            KpiPeriodFilter.OrderType = OrderType.ASC;
            KpiPeriodFilter.Selects = KpiPeriodSelect.ALL;
            KpiPeriodFilter.Id = KpiItem_KpiPeriodFilterDTO.Id;
            KpiPeriodFilter.Code = KpiItem_KpiPeriodFilterDTO.Code;
            KpiPeriodFilter.Name = KpiItem_KpiPeriodFilterDTO.Name;

            List<KpiPeriod> KpiPeriods = await KpiPeriodService.List(KpiPeriodFilter);
            List<KpiItem_KpiPeriodDTO> KpiItem_KpiPeriodDTOs = KpiPeriods
                .Select(x => new KpiItem_KpiPeriodDTO(x)).ToList();
            return KpiItem_KpiPeriodDTOs;
        }
        [Route(KpiItemRoute.SingleListKpiYear), HttpPost]
        public async Task<List<KpiItem_KpiYearDTO>> SingleListKpiYear([FromBody] KpiItem_KpiYearFilterDTO KpiItem_KpiYearFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiYearFilter KpiYearFilter = new KpiYearFilter();
            KpiYearFilter.Skip = 0;
            KpiYearFilter.Take = 20;
            KpiYearFilter.OrderBy = KpiYearOrder.Id;
            KpiYearFilter.OrderType = OrderType.ASC;
            KpiYearFilter.Selects = KpiYearSelect.ALL;
            KpiYearFilter.Id = KpiItem_KpiYearFilterDTO.Id;
            KpiYearFilter.Code = KpiItem_KpiYearFilterDTO.Code;
            KpiYearFilter.Name = KpiItem_KpiYearFilterDTO.Name;

            List<KpiYear> KpiYears = await KpiYearService.List(KpiYearFilter);
            List<KpiItem_KpiYearDTO> KpiItem_KpiYearDTOs = KpiYears
                .Select(x => new KpiItem_KpiYearDTO(x)).ToList();
            return KpiItem_KpiYearDTOs;
        }
        [Route(KpiItemRoute.SingleListOrganization), HttpPost]
        public async Task<List<KpiItem_OrganizationDTO>> SingleListOrganization([FromBody] KpiItem_OrganizationFilterDTO KpiItem_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = KpiItem_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = KpiItem_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = KpiItem_OrganizationFilterDTO.Name;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<KpiItem_OrganizationDTO> KpiItem_OrganizationDTOs = Organizations
                .Select(x => new KpiItem_OrganizationDTO(x)).ToList();
            return KpiItem_OrganizationDTOs;
        }
        [Route(KpiItemRoute.SingleListStatus), HttpPost]
        public async Task<List<KpiItem_StatusDTO>> SingleListStatus()
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
            List<KpiItem_StatusDTO> KpiItem_StatusDTOs = Statuses
                .Select(x => new KpiItem_StatusDTO(x)).ToList();
            return KpiItem_StatusDTOs;
        }

        [Route(KpiItemRoute.SingleListKpiCriteriaItem), HttpPost]
        public async Task<List<KpiItem_KpiCriteriaItemDTO>> SingleListKpiCriteriaItem()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiCriteriaItemFilter KpiCriteriaItemFilter = new KpiCriteriaItemFilter();
            KpiCriteriaItemFilter.Skip = 0;
            KpiCriteriaItemFilter.Take = int.MaxValue;
            KpiCriteriaItemFilter.Take = 20;
            KpiCriteriaItemFilter.OrderBy = KpiCriteriaItemOrder.Id;
            KpiCriteriaItemFilter.OrderType = OrderType.ASC;
            KpiCriteriaItemFilter.Selects = KpiCriteriaItemSelect.ALL;

            List<KpiCriteriaItem> KpiCriteriaItemes = await KpiCriteriaItemService.List(KpiCriteriaItemFilter);
            List<KpiItem_KpiCriteriaItemDTO> KpiItem_KpiCriteriaItemDTOs = KpiCriteriaItemes
                .Select(x => new KpiItem_KpiCriteriaItemDTO(x)).ToList();
            return KpiItem_KpiCriteriaItemDTOs;
        }

        [Route(KpiItemRoute.SingleListKpiItemType), HttpPost]
        public async Task<List<KpiItem_KpiItemTypeDTO>> SingleListKpiItemType([FromBody] KpiItem_KpiItemTypeFilterDTO KpiItem_KpiItemTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiItemTypeFilter KpiItemTypeFilter = new KpiItemTypeFilter();
            KpiItemTypeFilter.Skip = 0;
            KpiItemTypeFilter.Take = 20;
            KpiItemTypeFilter.OrderBy = KpiItemTypeOrder.Id;
            KpiItemTypeFilter.OrderType = OrderType.ASC;
            KpiItemTypeFilter.Selects = KpiItemTypeSelect.ALL;
            KpiItemTypeFilter.Id = KpiItem_KpiItemTypeFilterDTO.Id;
            KpiItemTypeFilter.Code = KpiItem_KpiItemTypeFilterDTO.Code;
            KpiItemTypeFilter.Name = KpiItem_KpiItemTypeFilterDTO.Name;
            List<KpiItemType> KpiItemTypes = await KpiItemTypeService.List(KpiItemTypeFilter);
            List<KpiItem_KpiItemTypeDTO> KpiItem_KpiItemTypeDTOs = KpiItemTypes
                .Select(x => new KpiItem_KpiItemTypeDTO(x)).ToList();
            return KpiItem_KpiItemTypeDTOs;
        }

        [Route(KpiItemRoute.SingleListItem), HttpPost]
        public async Task<List<KpiItem_ItemDTO>> SingleListItem([FromBody] KpiItem_ItemFilterDTO KpiItem_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Skip = 0;
            ItemFilter.Take = 20;
            ItemFilter.OrderBy = ItemOrder.Id;
            ItemFilter.OrderType = OrderType.ASC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.Id = KpiItem_ItemFilterDTO.Id;
            ItemFilter.ProductId = KpiItem_ItemFilterDTO.ProductId;
            ItemFilter.Code = KpiItem_ItemFilterDTO.Code;
            ItemFilter.Name = KpiItem_ItemFilterDTO.Name;
            ItemFilter.ScanCode = KpiItem_ItemFilterDTO.ScanCode;
            ItemFilter.SalePrice = KpiItem_ItemFilterDTO.SalePrice;
            ItemFilter.RetailPrice = KpiItem_ItemFilterDTO.RetailPrice;
            ItemFilter.StatusId = KpiItem_ItemFilterDTO.StatusId;
            ItemFilter.Search = KpiItem_ItemFilterDTO.Search;

            List<Item> Items = await ItemService.List(ItemFilter);
            List<KpiItem_ItemDTO> KpiItem_ItemDTOs = Items
                .Select(x => new KpiItem_ItemDTO(x)).ToList();
            return KpiItem_ItemDTOs;
        }

        [Route(KpiItemRoute.CountAppUser), HttpPost]
        public async Task<long> CountAppUser([FromBody] KpiItem_AppUserFilterDTO KpiItem_AppUserFilterDTO)
        {
            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Id = KpiItem_AppUserFilterDTO.Id;
            AppUserFilter.Username = KpiItem_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = KpiItem_AppUserFilterDTO.DisplayName;
            AppUserFilter.Email = KpiItem_AppUserFilterDTO.Email;
            AppUserFilter.Phone = KpiItem_AppUserFilterDTO.Phone;
            AppUserFilter.OrganizationId = KpiItem_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            {
                if (AppUserFilter.Id.In == null) AppUserFilter.Id.In = new List<long>();
                AppUserFilter.Id.In.AddRange(await FilterAppUser(AppUserService, OrganizationService, CurrentContext));
            }

            return await KpiItemService.CountAppUser(AppUserFilter, KpiItem_AppUserFilterDTO.KpiYearId, KpiItem_AppUserFilterDTO.KpiPeriodId, KpiItem_AppUserFilterDTO.KpiItemTypeId);
        }

        [Route(KpiItemRoute.ListAppUser), HttpPost]
        public async Task<List<KpiItem_AppUserDTO>> ListAppUser([FromBody] KpiItem_AppUserFilterDTO KpiItem_AppUserFilterDTO)
        {
            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = KpiItem_AppUserFilterDTO.Skip;
            AppUserFilter.Take = KpiItem_AppUserFilterDTO.Take;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = KpiItem_AppUserFilterDTO.Id;
            AppUserFilter.Username = KpiItem_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = KpiItem_AppUserFilterDTO.DisplayName;
            AppUserFilter.Email = KpiItem_AppUserFilterDTO.Email;
            AppUserFilter.OrganizationId = KpiItem_AppUserFilterDTO.OrganizationId;
            AppUserFilter.Phone = KpiItem_AppUserFilterDTO.Phone;
            AppUserFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            {
                if (AppUserFilter.Id.In == null) AppUserFilter.Id.In = new List<long>();
                AppUserFilter.Id.In.AddRange(await FilterAppUser(AppUserService, OrganizationService, CurrentContext));
            }

            List<AppUser> AppUsers = await KpiItemService.ListAppUser(AppUserFilter, KpiItem_AppUserFilterDTO.KpiYearId, KpiItem_AppUserFilterDTO.KpiPeriodId, KpiItem_AppUserFilterDTO.KpiItemTypeId);
            List<KpiItem_AppUserDTO> KpiItem_AppUserDTOs = AppUsers
                .Select(x => new KpiItem_AppUserDTO(x)).ToList();
            return KpiItem_AppUserDTOs;
        }

        [Route(KpiItemRoute.CountItem), HttpPost]
        public async Task<long> CountItem([FromBody] KpiItem_ItemFilterDTO KpiItem_ItemFilterDTO)
        {
            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Code = KpiItem_ItemFilterDTO.Code;
            ItemFilter.Name = KpiItem_ItemFilterDTO.Name;
            ItemFilter.ProductGroupingId = KpiItem_ItemFilterDTO.ProductGroupingId;
            ItemFilter.ProductId = KpiItem_ItemFilterDTO.ProductId;
            ItemFilter.ProductTypeId = KpiItem_ItemFilterDTO.ProductTypeId;
            ItemFilter.RetailPrice = KpiItem_ItemFilterDTO.RetailPrice;
            ItemFilter.SalePrice = KpiItem_ItemFilterDTO.SalePrice;
            ItemFilter.ScanCode = KpiItem_ItemFilterDTO.ScanCode;
            ItemFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            ItemFilter.SupplierId = KpiItem_ItemFilterDTO.SupplierId;
            ItemFilter.Search = KpiItem_ItemFilterDTO.Search;
            ItemFilter.IsNew = KpiItem_ItemFilterDTO.IsNew;

            return await ItemService.Count(ItemFilter);
        }
        [Route(KpiItemRoute.ListItem), HttpPost]
        public async Task<List<KpiItem_ItemDTO>> ListItem([FromBody] KpiItem_ItemFilterDTO KpiItem_ItemFilterDTO)
        {
            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Skip = KpiItem_ItemFilterDTO.Skip;
            ItemFilter.Take = KpiItem_ItemFilterDTO.Take;
            ItemFilter.OrderBy = ItemOrder.Id;
            ItemFilter.OrderType = OrderType.ASC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.Id = KpiItem_ItemFilterDTO.Id;
            ItemFilter.Code = KpiItem_ItemFilterDTO.Code;
            ItemFilter.Name = KpiItem_ItemFilterDTO.Name;
            ItemFilter.ProductGroupingId = KpiItem_ItemFilterDTO.ProductGroupingId;
            ItemFilter.ProductId = KpiItem_ItemFilterDTO.ProductId;
            ItemFilter.ProductTypeId = KpiItem_ItemFilterDTO.ProductTypeId;
            ItemFilter.RetailPrice = KpiItem_ItemFilterDTO.RetailPrice;
            ItemFilter.SalePrice = KpiItem_ItemFilterDTO.SalePrice;
            ItemFilter.ScanCode = KpiItem_ItemFilterDTO.ScanCode;
            ItemFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            ItemFilter.SupplierId = KpiItem_ItemFilterDTO.SupplierId;
            ItemFilter.Search = KpiItem_ItemFilterDTO.Search;
            ItemFilter.IsNew = KpiItem_ItemFilterDTO.IsNew;

            List<Item> Items = await ItemService.List(ItemFilter);
            List<KpiItem_ItemDTO> KpiItem_ItemDTOs = Items
                .Select(x => new KpiItem_ItemDTO(x)).ToList();
            return KpiItem_ItemDTOs;
        }
    }
}

